using backend.Data;
using backend.DTOs;
using backend.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using System.IdentityModel.Tokens.Jwt;
using System.IO.Compression;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod());
});

// register the data context and provider in the service container
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(connectionString));

//Authentication: перевірка JWT-токена за схемою Bearer (OAuth 2.0)
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });
builder.Services.AddAuthorization();

//Swagger руєстрація сервісів для опису та документування АРІ
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Meme Board API",
        Version = "v1",
        Description = "Meme Board MVP REST API Documentation"
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT-токен з /auth/login"
    });
    options.AddSecurityRequirement(doc => new OpenApiSecurityRequirement
    {
        { new OpenApiSecuritySchemeReference("Bearer", doc), new List<string>() }
    });
});

var app = builder.Build();

app.UseCors();

//генерація опису (джсон) та інтерактивної сторінки SwaggerUI
app.UseSwagger();
app.UseSwaggerUI(options => {
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Meme Board API v1");
    options.DocumentTitle = "Meme Board API";
});

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.UseStaticFiles();

var appName = app.Configuration["AppSettings:AppName"];
var welcome = app.Configuration["AppSettings:WelcomeMessage"];
var version = app.Configuration["AppSettings:Version"];

app.Logger.LogInformation("{AppName} API запущено. Середовище: {Env}, версія: {Version}",
    appName,
    app.Environment.EnvironmentName,
    version);

app.MapGet("/", () => {
    app.Logger.LogInformation("Запит до головного ендпоінта {AppName}", appName);
    return $"{welcome} ({appName}, версія {version})";
})
.WithTags("Service");

// READ: all categories
app.MapGet("/categories", async (AppDbContext db) =>
{
    var categories = await db.Categories.OrderBy(c => c.Name).ToListAsync();
    return Results.Ok(categories.Select(c => new CategoryResponseDto(c.Id, c.Name, c.Icon)));
})
.WithTags("Categories");

// CREATE: add category (via API, not seed)
app.MapPost("/categories", async (CreateCategoryDto dto, AppDbContext db) =>
{
    var name = dto.Name.Trim();
    if (string.IsNullOrWhiteSpace(name))
    if (string.IsNullOrWhiteSpace(name))
        return Results.BadRequest("Назва категорії не може бути порожньою");

    if (await db.Categories.AnyAsync(c => c.Name == name))
        return Results.Conflict("Категорія з такою назвою вже існує");

    var category = new Category { Name = name, Icon = dto.Icon.Trim() };
    db.Categories.Add(category);
    await db.SaveChangesAsync();

    return Results.Created($"/categories/{category.Id}",
        new CategoryResponseDto(category.Id, category.Name, category.Icon));
})
.RequireAuthorization()
.WithTags("Categories");

//CREATE: upload image
app.MapPost("/upload", async(ClaimsPrincipal principal, IFormFile formFile, AppDbContext db) => 
{
    var userIdStr = principal.FindFirstValue(ClaimTypes.NameIdentifier);

    if (!int.TryParse(userIdStr, out var userId))
        return Results.Unauthorized();

    if (formFile is null || formFile.Length == 0)
        return Results.BadRequest("File is not choosen");

    if (formFile.Length > 2097152)
        return Results.BadRequest("The file is too large");

    var allowed = new[] { "image/jpeg", "image/png" };
    if (!allowed.Contains(formFile.ContentType))
        return Results.BadRequest("Only jpeg or png");

    using var ms = new MemoryStream();
    await formFile.CopyToAsync(ms);
    var base64 = Convert.ToBase64String(ms.ToArray());

    return Results.Ok(new { imageBase64 = base64, imageContentType = formFile.ContentType });

})
.RequireAuthorization()
.DisableAntiforgery()
.WithTags("Upload");

// READ: all memes
app.MapGet("/memes", async (AppDbContext db, ClaimsPrincipal principal, bool mine = false, string? category = null, string? Title = null) =>
{
    var query = db.Memes
        .Include(m => m.User)
        .Include(m => m.Likes)
        .Include(m => m.Category)
        .AsQueryable();

    if (mine)
    {
        var userIdStr = principal.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!int.TryParse(userIdStr, out var userId))
            return Results.Unauthorized();
        query = query.Where(m => m.UserId == userId);
    }

    if(!string.IsNullOrWhiteSpace(category))
        query = query.Where(m => m.Category != null && m.Category.Name == category);

    if (!string.IsNullOrWhiteSpace(Title))
        query = query.Where(m => m.Title.ToUpper().Contains(Title.ToUpper()));

    var memes = await query
        .OrderByDescending(m => m.CreatedAt)
        .ToListAsync();

    return Results.Ok(memes.Select(m => new MemeResponseDto(
        m.Id, m.Title, m.ImageBase64, m.ImageContentType, m.CreatedAt, m.User?.Name ?? "", m.Likes.Count, m.Category?.Name ?? "")));
})
.WithTags("Memes");

// READ: one meme by id
app.MapGet("/memes/{id}", async (int id, AppDbContext db) =>
{
    if (await db.Memes.FindAsync(id) is not Meme meme)
        return Results.NotFound();

    await db.Entry(meme).Reference(m => m.User).LoadAsync();
    await db.Entry(meme).Collection(m => m.Likes).LoadAsync();
    await db.Entry(meme).Reference(m => m.Category).LoadAsync();
    return Results.Ok(new MemeResponseDto(
        meme.Id, meme.Title, meme.ImageBase64, meme.ImageContentType, meme.CreatedAt, meme.User?.Name ?? "", meme.Likes.Count, meme.Category?.Name ?? ""));
})
.WithTags("Memes");

// CREATE: add new meme
app.MapPost("/memes", async (CreateMemeDto dto, AppDbContext db, ClaimsPrincipal principal) =>
{
    var userIdStr = principal.FindFirstValue(ClaimTypes.NameIdentifier);

    if (!int.TryParse(userIdStr, out var userId))
        return Results.Unauthorized();

    var meme = new Meme
    {
        Title = dto.Title,
        ImageBase64 = dto.ImageBase64,
        ImageContentType = dto.ImageContentType,
        UserId = userId,
        CreatedAt = DateTime.UtcNow,
        CategoryId = dto.CategoryId,
    };

    db.Memes.Add(meme);
    await db.SaveChangesAsync();

    await db.Entry(meme).Reference(m => m.User).LoadAsync();
    await db.Entry(meme).Reference(m => m.Category).LoadAsync();
    return Results.Created($"/memes/{meme.Id}", new MemeResponseDto(
        meme.Id, meme.Title, meme.ImageBase64, meme.ImageContentType, meme.CreatedAt, meme.User?.Name ?? "", 0, meme.Category?.Name ?? ""));
})
.RequireAuthorization()
.WithTags("Memes");

// UPDATE: edit existing meme
app.MapPut("/memes/{id}", async (int id, UpdateMemeDto dto, AppDbContext db, ClaimsPrincipal principal) =>
{
    var userIdStr = principal.FindFirstValue(ClaimTypes.NameIdentifier);
    if (!int.TryParse(userIdStr, out var userId))
        return Results.Unauthorized();

    var meme = await db.Memes.FindAsync(id);
    if (meme is null) return Results.NotFound();
    if (meme.UserId != userId) return Results.Forbid();

    meme.Title = dto.Title;
    meme.ImageUrl = dto.ImageUrl;
    meme.CategoryId = dto.CategoryId;
    await db.SaveChangesAsync();
    return Results.NoContent();
})
.RequireAuthorization()
.WithTags("Memes");

// DELETE: remove meme
app.MapDelete("/memes/{id}", async (int id, AppDbContext db, ClaimsPrincipal principal) =>
{
    var userIdStr = principal.FindFirstValue(ClaimTypes.NameIdentifier);
    if (!int.TryParse(userIdStr, out var userId))
        return Results.Unauthorized();

    var meme = await db.Memes.FindAsync(id);
    if (meme is null) return Results.NotFound();
    if (meme.UserId != userId) return Results.Forbid();

    db.Memes.Remove(meme);
    await db.SaveChangesAsync();
    return Results.NoContent();
})
.RequireAuthorization()
.WithTags("Memes");

// LIKE: add like to meme
app.MapPost("/memes/{id}/like", async (int id, AppDbContext db, ClaimsPrincipal principal) =>
{
    var userIdStr = principal.FindFirstValue(ClaimTypes.NameIdentifier);

    if (!int.TryParse(userIdStr, out var userId))
        return Results.Unauthorized();

    if (await db.Memes.FindAsync(id) is not Meme meme)
        return Results.NotFound();

    if (await db.MemeLikes.AnyAsync(l => l.MemeId == id && l.UserId == userId))
        return Results.NoContent();

    db.MemeLikes.Add(new MemeLike { UserId = userId, MemeId = id });
    await db.SaveChangesAsync();
    return Results.NoContent();
})
.RequireAuthorization()
.WithTags("Memes");

// UNLIKE: remove like from meme
app.MapDelete("/memes/{id}/like", async (int id, AppDbContext db, ClaimsPrincipal principal) =>
{
    var userIdStr = principal.FindFirstValue(ClaimTypes.NameIdentifier);
    if (!int.TryParse(userIdStr, out var userId))
        return Results.Unauthorized();

    var like = await db.MemeLikes.FirstOrDefaultAsync(l => l.MemeId == id && l.UserId == userId);
    if (like is not null)
    {
        db.MemeLikes.Remove(like);
        await db.SaveChangesAsync();
    }
    return Results.NoContent();
})
.RequireAuthorization()
.WithTags("Memes");

app.MapGet("/boom", () =>
{
    throw new Exception("Тестова помилка: перевірка Middleware");
})
.WithTags("Service");

//Registration: створення користувача з хешуванням пароля
app.MapPost("/auth/register", async (RegisterDto dto, AppDbContext db) =>
{
    if (await db.Users.AnyAsync(u => u.Email == dto.Email))
        return Results.Conflict("Користувач з таким email вже існує");

    var user = new User
    {
        Name = dto.Name,
        Email = dto.Email,
        PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
        Role = "user"
    };

    db.Users.Add(user);
    await db.SaveChangesAsync();
    return Results.Created($"/auth/users/{user.Id}",
        new { user.Id, user.Name, user.Email, user.Role });
})
.WithTags("Auth");

//перевіряє email і пароль, якщо все ок повертає JWT
app.MapPost("/auth/login", async (LoginDto dto, AppDbContext db, IConfiguration config) =>
{
    var user = await db.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
    if (user is null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
        return Results.Unauthorized();

    var token = CreateToken(user, config);
    return Results.Ok(new { access_token = token, token_type = "Bearer" });
})
.WithTags("Auth");

// формування JWT: claims (id, email, role) + підпис секретним ключем з appsettings
static string CreateToken(User user, IConfiguration config)
{
    var claims = new[]
    {
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Email, user.Email),
        new Claim(ClaimTypes.Role, user.Role)
    };
    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]!));
    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
    var token = new JwtSecurityToken(
        issuer: config["Jwt:Issuer"],
        audience: config["Jwt:Audience"],
        claims: claims,
        expires: DateTime.UtcNow.AddHours(2),
        signingCredentials: creds);
    return new JwtSecurityTokenHandler().WriteToken(token);
}

// поточний користувач з даних токена (ClaimsPrincipal)
app.MapGet("/auth/me", (ClaimsPrincipal principal) =>
    Results.Ok(new
    {
        Id = principal.FindFirstValue(ClaimTypes.NameIdentifier),
        Email = principal.FindFirstValue(ClaimTypes.Email),
        Role = principal.FindFirstValue(ClaimTypes.Role)
    }))
    .RequireAuthorization()
    .WithTags("Auth");

app.Run();

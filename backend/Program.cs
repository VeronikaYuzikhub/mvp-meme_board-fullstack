using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.Models;
using Microsoft.OpenApi;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using backend.DTOs;

var builder = WebApplication.CreateBuilder(args);

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

//генерація опису (джсон) та інтерактивної сторінки SwaggerUI
app.UseSwagger();
app.UseSwaggerUI(options => {
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Meme Board API v1");
    options.DocumentTitle = "Meme Board API";
});

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

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

// READ: all memes
app.MapGet("/memes", async (AppDbContext db) =>
{
    var memes = await db.Memes
        .Include(m => m.User)
        .OrderByDescending(m => m.CreatedAt)
        .ToListAsync();

    return Results.Ok(memes.Select(m => new MemeResponseDto(
        m.Id, m.Title, m.ImageUrl, m.CreatedAt, m.User?.Name ?? "")));
})
.WithTags("Memes");

// READ: one meme by id
app.MapGet("/memes/{id}", async (int id, AppDbContext db) =>
{
    if (await db.Memes.FindAsync(id) is not Meme meme)
        return Results.NotFound();

    await db.Entry(meme).Reference(m => m.User).LoadAsync();
    return Results.Ok(new MemeResponseDto(
        meme.Id, meme.Title, meme.ImageUrl, meme.CreatedAt, meme.User?.Name ?? ""));
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
        ImageUrl = dto.ImageUrl,
        UserId = userId,
        CreatedAt = DateTime.UtcNow
    };

    db.Memes.Add(meme);
    await db.SaveChangesAsync();

    await db.Entry(meme).Reference(m => m.User).LoadAsync();
    return Results.Created($"/memes/{meme.Id}", new MemeResponseDto(
        meme.Id, meme.Title, meme.ImageUrl, meme.CreatedAt, meme.User?.Name ?? ""));
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

record RegisterDto(string Name, string Email, string Password);
record LoginDto(string Email, string Password);
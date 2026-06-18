using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.Models;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

// register the data context and provider in the service container
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(connectionString));

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
});

var app = builder.Build();

//генерація опису (джсон) та інтерактивної сторінки SwaggerUI
app.UseSwagger();
app.UseSwaggerUI(options => {
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Meme Board API v1");
    options.DocumentTitle = "Meme Board API";
});

app.UseMiddleware<ExceptionHandlingMiddleware>();

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
    await db.Memes.ToListAsync())
    .WithTags("Memes");

// READ: one meme by id
app.MapGet("/memes/{id}", async(int id, AppDbContext db) =>
    await db.Memes.FindAsync(id) is Meme meme
    ? Results.Ok(meme)
    : Results.NotFound())
    .WithTags("Memes");

// CREATE: add new meme
app.MapPost("/memes", async (Meme meme, AppDbContext db) =>
{
    db.Memes.Add(meme);
    await db.SaveChangesAsync();
    return Results.Created($"/memes/{meme.Id}", meme);
})
.WithTags("Memes");

// UPDATE: edit existing meme
app.MapPut("/memes/{id}", async (int id, Meme input, AppDbContext db) =>
{
    var meme = await db.Memes.FindAsync(id);
    if (meme is null) return Results.NotFound();

    meme.Title = input.Title;
    meme.ImageUrl = input.ImageUrl;
    await db.SaveChangesAsync();
    return Results.NoContent();
})
.WithTags("Memes");

// DELETE: remove meme
app.MapDelete("/memes/{id}", async (int id, AppDbContext db) =>
{
    var meme = await db.Memes.FindAsync(id);
    if (meme is null) return Results.NotFound();

    db.Memes.Remove(meme);
    await db.SaveChangesAsync();
    return Results.NoContent();
})
.WithTags("Memes");

app.MapGet("/boom", () =>
{
    throw new Exception("Тестова помилка: перевірка Middleware");
})
.WithTags("Service");

app.Run();
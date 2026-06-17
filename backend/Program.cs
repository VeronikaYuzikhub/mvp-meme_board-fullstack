using Microsoft.EntityFrameworkCore;
using backend.Data;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
// реєструємо у контейнері сервісів
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(connectionString));

var app = builder.Build();
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
});

app.MapGet("/memes", async (AppDbContext db) =>
    await db.Memes.ToListAsync());

app.MapGet("/boom", () =>
{
    throw new Exception("Тестова помилка: перевірка Middleware");
});

app.Run();
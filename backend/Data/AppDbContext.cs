using Microsoft.EntityFrameworkCore;
using backend.Models;

namespace backend.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Meme> Memes => Set<Meme>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Initial seed data for development and lab demo (replaced by UI later)
        modelBuilder.Entity<Meme>().HasData(
            new Meme { Id = 1, Title = "Коли дедлайн завтра", ImageUrl = "https://i.imgflip.com/1bij.jpg" },
            new Meme { Id = 2, Title = "Програміст vs Bug", ImageUrl = "https://i.imgflip.com/26am.jpg" },
            new Meme { Id = 3, Title = "Meme Board MVP", ImageUrl = "https://i.imgflip.com/aujac7.jpg" });
    }
}
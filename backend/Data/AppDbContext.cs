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
    public DbSet<User> Users => Set<User>();
    public DbSet<MemeLike> MemeLikes => Set<MemeLike>();
    public DbSet<Category> Categories => Set<Category>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Initial seed data for development and lab demo (replaced by UI later)
        modelBuilder.Entity<Meme>()
            .HasOne(m => m.User)
            .WithMany()
            .HasForeignKey(m => m.UserId);
        modelBuilder.Entity<MemeLike>()
        .HasOne(l => l.User)
        .WithMany()
        .HasForeignKey(l => l.UserId);
        modelBuilder.Entity<MemeLike>()
            .HasOne(l => l.Meme)
            .WithMany(m => m.Likes)
            .HasForeignKey(l => l.MemeId);
        modelBuilder.Entity<MemeLike>()
            .HasIndex(l => new { l.UserId, l.MemeId })
            .IsUnique();
        modelBuilder.Entity<Meme>()
            .HasOne(m => m.Category)
            .WithMany(c => c.Memes)
            .HasForeignKey(m => m.CategoryId);
        modelBuilder.Entity<Category>()
            .HasIndex(c => c.Name)
            .IsUnique();

        /*    .HasData(
          new Meme { Id = 1, Title = "Коли дедлайн завтра", ImageUrl = "https://i.imgflip.com/1bij.jpg" },
          new Meme { Id = 2, Title = "Програміст vs Bug", ImageUrl = "https://i.imgflip.com/26am.jpg" },
          new Meme { Id = 3, Title = "Meme Board MVP", ImageUrl = "https://i.imgflip.com/aujac7.jpg" });
        */
    }
}
namespace backend.Models;

public class Meme
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public int UserId { get; set; }
    public DateTime CreatedAt { get; set; }
    public User? User { get; set; }
    public ICollection<MemeLike> Likes { get; set; } = new List<MemeLike>();
    public int? CategoryId { get; set; }
    public Category? Category { get; set; }
    public string? ImageBase64 { get; set; }
    public string ImageContentType { get; set; } = "image/jpeg/png";
}
namespace backend.Models;

public class MemeLike
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int MemeId { get; set; }
    public User? User { get; set; }
    public Meme? Meme { get; set; }
}
namespace backend.DTOs;

public record MemeResponseDto(int Id, string Title, string ImageUrl, DateTime CreatedAt, string AuthorName, int LikeCount);
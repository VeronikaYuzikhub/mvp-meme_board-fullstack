namespace backend.DTOs;

public record MemeResponseDto(int Id, string Title, string? Description, string? ImageBase64, string ImageContentType, string ImageUrl, DateTime CreatedAt, string AuthorName, int LikeCount, string CategoryName, bool IsLikedByMe);
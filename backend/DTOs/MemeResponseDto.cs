namespace backend.DTOs;

public record MemeResponseDto(int Id, string Title, string ImageBase64, string ImageContentType, DateTime CreatedAt, string AuthorName, int LikeCount, string CategoryName);
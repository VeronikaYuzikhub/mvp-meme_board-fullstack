namespace backend.DTOs;

public record UpdateMemeDto(string Title, string ImageUrl, int CategoryId, string? Description = null, string? ImageBase64 = null, string? ImageContentType = null);
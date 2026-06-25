namespace backend.DTOs;

public record CreateMemeDto(string Title, string ImageBase64, string ImageContentType, int CategoryId, string? Description = null);
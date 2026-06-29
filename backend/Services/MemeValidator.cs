using backend.Models;

namespace backend.Services;

public static class MemeValidator
{
    public static bool IsTitleLengthCorrect(string title, int limit)
        => title.Length <= limit;

    public static bool IsDescLengthCorrect(string? description, int limit)
        => (description?.Length ?? 0) <= limit;
}

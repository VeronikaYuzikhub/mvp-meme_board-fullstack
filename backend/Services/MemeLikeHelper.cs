using backend.Models;

namespace backend.Services;

public static class MemeLikeHelper
{
    public static bool IsSameLike(int memeId, int userId, MemeLike like)
        => like.MemeId == memeId && like.UserId == userId;

    public static bool IsLikedByUser(int? currentUserId, MemeLike like)
        => currentUserId.HasValue && like.UserId == currentUserId.Value;
}

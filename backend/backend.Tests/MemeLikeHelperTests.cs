using backend.Models;
using backend.Services;
using Xunit;

namespace backend.Tests;

public class MemeLikeHelperTests
{
    [Fact]
    public void IsSameLike_ReturnsTrue_WhenMemeIdAndUserIdMatch()
    {
        //Arrange
        var like = new MemeLike { MemeId = 1, UserId = 5 };
        //Act
        var result = MemeLikeHelper.IsSameLike(1, 5, like);
        //Assert
        Assert.True(result);
    }

    [Fact]
    public void IsSameLike_ReturnsFalse_WhenUserIdDifferent()
    {
        var like = new MemeLike { MemeId = 1, UserId = 5 };
        var result = MemeLikeHelper.IsSameLike(1, 99, like);
        Assert.False(result);
    }
}

using backend.Models;
using backend.Services;
using Xunit;

namespace backend.Tests;

public class MemeValidatorTests
{
    [Fact]
    public void IsTitleLengthCorrect_ReturnsTrue_WhenTitleShorterThanLimit()
    {
        var result = MemeValidator.IsTitleLengthCorrect("Funny cat", 100);
        Assert.True(result);
    }

    [Fact]
    public void IsTitleLengthCorrect_ReturnsFalse_WhenTitleLongerThanLimit()
    {
        var longTitle = new string('a', 101);
        var result = MemeValidator.IsTitleLengthCorrect(longTitle, 100);
        Assert.False(result);
    }
}

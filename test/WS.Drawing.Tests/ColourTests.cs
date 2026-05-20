namespace WS.Drawing.Tests;

public class ColourTests
{
    [Fact]
    public void Lighten_ReturnsSame_WhenZero()
    {
        var c = new Colour(10, 20, 30, 200);
        var result = c.Lighten(0f);
        Assert.Equal(c, result);
    }

    [Fact]
    public void Lighten_ReturnsWhite_When100()
    {
        var c = new Colour(10, 20, 30, 128);
        var result = c.Lighten(100f);
        Assert.Equal(new Colour(255, 255, 255, 128), result);
    }

    [Fact]
    public void Lighten_InterpolatesHalfway()
    {
        var c = new Colour(0, 64, 192, 255);
        var result = c.Lighten(50f);
        // Expect a perceptually lighter blue preserving hue (HSL-based): approx (96,149,255)
        Assert.Equal(new Colour(96, 149, 255, 255), result);
    }

    [Fact]
    public void Lighten_InvalidPercentage_Throws()
    {
        var c = new Colour(10, 20, 30);
        Assert.Throws<ArgumentOutOfRangeException>(() => c.Lighten(-0.1f));
        Assert.Throws<ArgumentOutOfRangeException>(() => c.Lighten(100.1f));
    }
}

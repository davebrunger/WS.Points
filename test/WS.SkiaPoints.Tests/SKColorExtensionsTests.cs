namespace WS.SkiaPoints.Tests;

public class SKColorExtensionsTests
{
    [Fact]
    public void FromColour_ConvertsValues()
    {
        var c = new Colour(10, 20, 30, 40);
        var sk = c.ToSKColor();

        Assert.Equal((byte)10, sk.Red);
        Assert.Equal((byte)20, sk.Green);
        Assert.Equal((byte)30, sk.Blue);
        Assert.Equal((byte)40, sk.Alpha);
    }
}

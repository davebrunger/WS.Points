namespace WS.Drawing.Tests;

public class ColourConversionTests
{
    [Theory]
    [InlineData(0,0,0)]
    [InlineData(255,255,255)]
    [InlineData(255,0,0)]
    [InlineData(0,255,0)]
    [InlineData(0,0,255)]
    [InlineData(255,255,0)]
    [InlineData(0,255,255)]
    [InlineData(255,0,255)]
    [InlineData(128,64,32)]
    [InlineData(10,20,30)]
    public void RgbToHsl_Then_HslToRgb_RoundsTripApproximately(byte r, byte g, byte b)
    {
        var (h, s, l) = Colour.RgbToHsl(r, g, b);
        var (r2, g2, b2) = Colour.HslToRgb(h, s, l);

        // Allow a tolerance of 1 due to rounding during conversions.
        Assert.InRange(Math.Abs(r2 - r), 0, 1);
        Assert.InRange(Math.Abs(g2 - g), 0, 1);
        Assert.InRange(Math.Abs(b2 - b), 0, 1);
    }

    [Fact]
    public void HslToRgb_Achromatic_ProducesGrey()
    {
        // s == 0 should produce greyscale with all channels == round(l*255)
        var (r, g, b) = Colour.HslToRgb(0.0, 0.0, 0.25);
        var expected = (byte)Math.Round(0.25 * 255.0);
        Assert.Equal(expected, r);
        Assert.Equal(expected, g);
        Assert.Equal(expected, b);
    }

    [Fact]
    public void RgbToHsl_ProducesValidRanges()
    {
        var (h, s, l) = Colour.RgbToHsl(10, 20, 30);
        Assert.InRange(h, 0.0, 1.0);
        Assert.InRange(s, 0.0, 1.0);
        Assert.InRange(l, 0.0, 1.0);
    }
}

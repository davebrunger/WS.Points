using System;
using WS.Points.Drawing;
using Xunit;

namespace WS.Points.Tests;

public class LabConversionTests
{
    [Theory]
    [InlineData(0,0,0)]
    [InlineData(255,255,255)]
    [InlineData(255,0,0)]
    [InlineData(0,255,0)]
    [InlineData(0,0,255)]
    [InlineData(128,64,32)]
    [InlineData(10,20,30)]
    public void RgbToLab_Then_LabToRgb_RoundsTripApproximately(byte r, byte g, byte b)
    {
        var (L, a, bb) = Colour.RgbToLab(r, g, b);
        var (r2, g2, b2) = Colour.LabToRgb(L, a, bb);

        Assert.InRange(Math.Abs(r2 - r), 0, 1);
        Assert.InRange(Math.Abs(g2 - g), 0, 1);
        Assert.InRange(Math.Abs(b2 - b), 0, 1);
    }

    [Fact]
    public void Lighten_UsingLab_IncreasesLightness_TowardWhite()
    {
        var original = new Colour(60, 120, 200);
        var light50 = original.Lighten(50f);
        var light100 = original.Lighten(100f);

        // 100% must be white
        Assert.Equal(new Colour(255,255,255, original.Alpha), light100);

        // 50% should produce channels closer to 255 than original
        Assert.True(light50.Red >= original.Red);
        Assert.True(light50.Green >= original.Green);
        Assert.True(light50.Blue >= original.Blue);
    }
}

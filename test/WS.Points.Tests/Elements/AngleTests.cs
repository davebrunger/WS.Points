namespace WS.Points.Tests;

public class AngleTests
{
    [Fact]
    public void Constructor_Normalizes_To_0_2PI()
    {
        var a = new AngleInRadians(2.0 * Math.PI) + 0.5;
        Assert.Equal(0.5, (double)a, 9);
    }

    [Fact]
    public void ImplicitDoubleConversion_RoundTrip()
    {
        double v = (double)AngleInRadians.Pi / 3.0;
        AngleInRadians a = v;
        Assert.Equal(v, (double)a, 9);
    }
}

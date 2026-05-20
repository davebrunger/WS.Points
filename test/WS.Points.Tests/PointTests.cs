namespace WS.Points.Tests;

public class PointTests
{
    [Fact]
    public void Rotate_90Degrees()
    {
        var p = new Point(1, 0);
        var r = p.Rotate(AngleInRadians.Pi / 2);
        Assert.Equal(0.0, r.X, 9);
        Assert.Equal(1.0, r.Y, 9);
    }

    [Fact]
    public void Translate_AddsVector()
    {
        var p = new Point(1, 2);
        var t = p.Translate(new Vector(3, 4));
        Assert.Equal(4.0, t.X, 9);
        Assert.Equal(6.0, t.Y, 9);
    }

    [Fact]
    public void Flip_About_XAxis()
    {
        var p = new Point(1, 2);
        var flipped = p.Flip(Line.XAxis);
        Assert.Equal(1.0, flipped.X, 9);
        Assert.Equal(-2.0, flipped.Y, 9);
    }
}

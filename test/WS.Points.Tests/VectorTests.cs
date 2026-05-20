namespace WS.Points.Tests;

public class VectorTests
{
    [Fact]
    public void Rotate_And_Unit()
    {
        var v = new Vector(1, 0);
        var r = v.Rotate(AngleInRadians.Pi / 2);
        Assert.Equal(0.0, r.X, 9);
        Assert.Equal(1.0, r.Y, 9);

        var u = new Vector(3, 4).Unit();
        Assert.Equal(0.6, u.X, 9);
        Assert.Equal(0.8, u.Y, 9);
    }

    [Fact]
    public void Flip_Across_XAxis()
    {
        var v = new Vector(1, 2);
        var flipped = v.Flip(Vector.UnitX);
        Assert.Equal(1.0, flipped.X, 9);
        Assert.Equal(-2.0, flipped.Y, 9);
    }

    [Fact]
    public void Dot_And_Cross()
    {
        var a = new Vector(2, 3);
        var b = new Vector(4, -1);
        // dot = 2*4 + 3*(-1) = 5
        Assert.Equal(5.0, a.Dot(b), 9);
        // cross = 2*(-1) - 3*4 = -2 -12 = -14
        Assert.Equal(-14.0, a.Cross(b), 9);
    }
}

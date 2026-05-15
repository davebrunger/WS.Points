namespace WS.Points.Tests;

public class EllipseTests
{
    [Fact]
    public void Contains_Point_On_Axes()
    {
        var e = new Ellipse(new Point(0,0), new Vector(1,0), 2.0, 1.0);
        Assert.True(e.Contains(new Point(2,0)));
        Assert.True(e.Contains(new Point(0,1)));
        Assert.False(e.Contains(new Point(2.1, 0)));
        Assert.False(e.Contains(new Point(0, 1.1)));
    }

    [Fact]
    public void Scale_About_Origin_Affects_Centre_And_Radii()
    {
        var e = new Ellipse(new Point(1,1), new Vector(1,0), 2.0, 1.0);
        var s = e.Scale(2.0, 3.0);
        Assert.Equal(2.0, s.Centre.X, 9);
        Assert.Equal(3.0, s.Centre.Y, 9);
        Assert.Equal(4.0, s.MajorRadius, 9);
        Assert.Equal(3.0, s.MinorRadius, 9);
    }
}

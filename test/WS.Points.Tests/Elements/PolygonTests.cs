namespace WS.Points.Tests;

public class PolygonTests
{
    [Fact]
    public void RegularPolygon_Creates_Correct_Number_Of_Vertices()
    {
        var tri = Polygon.RegularPolygon(1.0, 3);
        Assert.Equal(3, tri.Count);
    }

    [Fact]
    public void RegularPolygon_Vertices_On_Circumference()
    {
        var r = 2.0;
        var poly = Polygon.RegularPolygon(r, 5);
        foreach (var p in poly)
        {
            var dist = Math.Sqrt(p.X * p.X + p.Y * p.Y);
            Assert.Equal(r, dist, 9);
        }
    }

    [Fact]
    public void Scale_About_Origin()
    {
        var poly = Polygon.From(new Point(1, 1), new Point(-1, 1));
        var s = poly.Scale(2.0, 3.0);
        Assert.Equal(2.0, s[0].X, 9);
        Assert.Equal(3.0, s[0].Y, 9);
        Assert.Equal(-2.0, s[1].X, 9);
        Assert.Equal(3.0, s[1].Y, 9);
    }
}

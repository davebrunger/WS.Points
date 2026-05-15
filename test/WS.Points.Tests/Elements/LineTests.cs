namespace WS.Points.Tests;

public class LineTests
{
    [Fact]
    public void Project_Onto_XAxis()
    {
        var axis = Line.XAxis;
        var p = new Point(1, 1);
        var proj = axis.Project(p);
        Assert.Equal(1.0, proj.X, 9);
        Assert.Equal(0.0, proj.Y, 9);
    }

    [Fact]
    public void DistanceTo_Point_Returns_Expected()
    {
        var axis = Line.XAxis;
        var p = new Point(3, 4);
        var d = axis.DistanceTo(p);
        Assert.Equal(4.0, d, 9);
    }
}

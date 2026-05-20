namespace WS.Points;

/// <summary>
/// Represents an infinite line defined by a point on the line and a direction vector.
/// </summary>
public record Line(Point Point, Vector Direction)
{
    /// <summary>
    /// A horizontal line along the X axis through the origin.
    /// </summary>
    public static Line XAxis { get; } = new Line(Point.Origin, Vector.UnitX);

    /// <summary>
    /// A vertical line along the Y axis through the origin.
    /// </summary>
    public static Line YAxis { get; } = new Line(Point.Origin, Vector.UnitY);

    /// <summary>
    /// Whether the line is degenerate (direction vector is zero).
    /// </summary>
    public bool IsDegenerate
    {
        get
        {
            return Direction.X == 0.0 && Direction.Y == 0.0;
        }
    }

    /// <summary>
    /// Projects the specified point orthogonally onto the infinite line.
    /// </summary>
    /// <param name="p">Point to project.</param>
    /// <returns>The projected point on the line.</returns>
    public Point Project(Point p)
    {
        var v = Vector.FromPoints(Point, p);
        var denom = Direction.Dot(Direction);
        if (denom == 0.0)
        {
            throw new InvalidOperationException("Cannot project onto a degenerate line (zero direction vector).");
        }
        var t = v.Dot(Direction) / denom;
        return new Point(Point.X + t * Direction.X, Point.Y + t * Direction.Y);
    }

    /// <summary>
    /// Returns the shortest distance from the line to the given point.
    /// </summary>
    public double DistanceTo(Point p)
    {
        var proj = Project(p);
        var rx = p.X - proj.X;
        var ry = p.Y - proj.Y;
        return Math.Sqrt(rx * rx + ry * ry);
    }

    /// <summary>
    /// Determines whether the given point lies on the line, within an optional epsilon tolerance.
    /// </summary>
    public bool Contains(Point p, double epsilon = 1e-9)
    {
        return DistanceTo(p) <= Math.Abs(epsilon);
    }

    /// <summary>
    /// Translates the line by the specified offset vector.
    /// </summary>
    public Line Translate(Vector offset)
    {
        return new Line(Point.Translate(offset), Direction);
    }

    /// <summary>
    /// Rotates the line (both point and direction) about the origin by the given angle.
    /// </summary>
    public Line Rotate(AngleInRadians angle)
    {
        return new Line(Point.Rotate(angle), Direction.Rotate(angle));
    }
 
    /// <summary>
    /// Reflects the line across the specified axis line.
    /// </summary>
    public Line Flip(Line axis)
    {
        return new Line(Point.Flip(axis), Direction.Flip(axis.Direction));
    }
}

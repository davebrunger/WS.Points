namespace WS.Points;

/// <summary>
/// Represents a 2D point with X and Y coordinates.
/// </summary>
public record Point(double X, double Y)
{
    /// <summary>
    /// A convenience instance representing the origin (0,0).
    /// </summary>
    public static Point Origin { get; } = new Point(0, 0);

    /// <summary>
    /// Returns a new <see cref="Point"/> rotated about the origin by the specified <see cref="AngleInRadians"/>.
    /// </summary>
    /// <param name="angle">Rotation angle in radians.</param>
    /// <returns>The rotated point.</returns>
    public Point Rotate(AngleInRadians angle)
    {
        var a = (double)angle;
        var cos = Math.Cos(a);
        var sin = Math.Sin(a);
        var x = X * cos - Y * sin;
        var y = X * sin + Y * cos;
        return new Point(x, y);
    }

    /// <summary>
    /// Reflects this point across the given <see cref="Line"/> and returns the reflected point.
    /// </summary>
    /// <param name="axis">The line to reflect across.</param>
    /// <returns>The reflected point.</returns>
    public Point Flip(Line axis)
    {
        var u = axis.Direction.Unit();
        var v = Vector.FromPoints(axis.Point, this);
        var t = v.Dot(u);
        var cx = axis.Point.X + t * u.X;
        var cy = axis.Point.Y + t * u.Y;
        var rx = 2.0 * cx - X;
        var ry = 2.0 * cy - Y;
        return new Point(rx, ry);
    }

    /// <summary>
    /// Translates this point by the specified vector and returns the result.
    /// </summary>
    /// <param name="offset">Translation vector.</param>
    /// <returns>The translated point.</returns>
    public Point Translate(Vector offset)
    {
        return new Point(X + offset.X, Y + offset.Y);
    }
}

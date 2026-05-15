namespace WS.Points.Elements;

/// <summary>
/// Represents a 2D vector and provides common vector operations.
/// </summary>
public record Vector(double X, double Y)
{
    /// <summary>
    /// Unit vector along the X axis.
    /// </summary>
    public static Vector UnitX { get; } = new Vector(1.0, 0.0);

    /// <summary>
    /// Unit vector along the Y axis.
    /// </summary>
    public static Vector UnitY { get; } = new Vector(0.0, 1.0);

    /// <summary>
    /// Rotates the vector by the given angle around the origin.
    /// </summary>
    /// <param name="angle">Rotation angle in radians.</param>
    /// <returns>The rotated vector.</returns>
    public Vector Rotate(AngleInRadians angle)
    {
        var a = (double)angle;
        var cos = Math.Cos(a);
        var sin = Math.Sin(a);
        var x = X * cos - Y * sin;
        var y = X * sin + Y * cos;
        return new Vector(x, y);
    }

    /// <summary>
    /// Scales the vector by a uniform factor.
    /// </summary>
    /// <param name="factor">Scale factor.</param>
    /// <returns>The scaled vector.</returns>
    public Vector Scale(double factor)
    {
        return new Vector(X * factor, Y * factor);
    }

    /// <summary>
    /// Creates a vector from two points (from -> to).
    /// </summary>
    public static Vector FromPoints(Point from, Point to)
    {
        return new Vector(to.X - from.X, to.Y - from.Y);
    }

    /// <summary>
    /// Creates a vector from the origin to the specified point.
    /// </summary>
    public static Vector FromOrigin(Point to)
    {
        return new Vector(to.X, to.Y);
    }

    /// <summary>
    /// Reflects this vector across the specified axis vector.
    /// </summary>
    /// <param name="axis">Axis to reflect across (direction only is significant).</param>
    /// <returns>The reflected vector.</returns>
    public Vector Flip(Vector axis)
    {
        var u = axis.Unit();
        var dot = Dot(u);
        var rx = 2.0 * dot * u.X - X;
        var ry = 2.0 * dot * u.Y - Y;
        return new Vector(rx, ry);
    }

    /// <summary>
    /// Returns the dot product of this vector with <paramref name="other"/> (this · other).
    /// </summary>
    public double Dot(Vector other)
    {
        return X * other.X + Y * other.Y;
    }

    /// <summary>
    /// Returns the 2D cross product (scalar) of this vector with <paramref name="other"/>.
    /// Defined as X1*Y2 - Y1*X2.
    /// </summary>
    public double Cross(Vector other)
    {
        return X * other.Y - Y * other.X;
    }

    /// <summary>
    /// Returns a unit-length vector in the same direction as this vector.
    /// </summary>
    /// <returns>The normalized vector.</returns>
    /// <exception cref="InvalidOperationException">Thrown when attempting to normalize the zero vector.</exception>
    public Vector Unit()
    {
        var lenSq = X * X + Y * Y;
        if (lenSq == 0.0)
        {
            throw new InvalidOperationException("Cannot normalize zero vector.");
        }
        var inv = 1.0 / Math.Sqrt(lenSq);
        return new Vector(X * inv, Y * inv);
    }
}

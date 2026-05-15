namespace WS.Points.Elements;

/// <summary>
/// Represents an angle in radians and normalizes to the range [0, 2π).
/// </summary>
public record AngleInRadians
{
    /// <summary>
    /// A convenience instance representing zero radians.
    /// </summary>
    public static AngleInRadians Zero { get; } = new AngleInRadians(0.0);

    /// <summary>
    /// A convenience instance representing π radians.
    /// </summary>
    public static AngleInRadians Pi { get; } = new AngleInRadians(Math.PI);



    /// <summary>
    /// The normalized angle value in radians.
    /// </summary>
    public double Value { get; }

    /// <summary>
    /// Initializes a new instance of <see cref="AngleInRadians"/> and normalizes the value.
    /// </summary>
    /// <param name="value">Angle value in radians. Will be normalized into the range [0, 2π).</param>
    public AngleInRadians(double value)
    {
        Value = Normalize(value);
    }

    private static double Normalize(double value)
    {
        var twoPi = 2.0 * Math.PI;
        var v = value % twoPi;
        if (v < 0)
        {
            v += twoPi;
        }
        return v;
    }
    
    /// <summary>
    /// Converts an <see cref="AngleInRadians"/> to a <see cref="double"/> representing radians.
    /// </summary>
    /// <param name="a">Angle value wrapper.</param>
    /// <returns>Normalized angle value in radians.</returns>
    public static implicit operator double(AngleInRadians a) => a.Value;

    /// <summary>
    /// Converts a <see cref="double"/> to an <see cref="AngleInRadians"/>, normalizing the value.
    /// </summary>
    /// <param name="d">Double value in radians.</param>
    /// <returns>A normalized <see cref="AngleInRadians"/> instance.</returns>
    public static implicit operator AngleInRadians(double d) => new AngleInRadians(d);

    /// <summary>
    /// Adds two angles and returns the normalized result.
    /// </summary>
    /// <param name="a">Left-hand angle.</param>
    /// <param name="b">Right-hand angle.</param>
    /// <returns>Normalized sum of the two angles.</returns>
    public static AngleInRadians operator +(AngleInRadians a, AngleInRadians b) => new AngleInRadians(a.Value + b.Value);

    /// <summary>
    /// Subtracts the right-hand angle from the left-hand angle and returns the normalized result.
    /// </summary>
    /// <param name="a">Left-hand angle.</param>
    /// <param name="b">Right-hand angle to subtract.</param>
    /// <returns>Normalized difference of the two angles.</returns>
    public static AngleInRadians operator -(AngleInRadians a, AngleInRadians b) => new AngleInRadians(a.Value - b.Value);

    /// <summary>
    /// Multiplies an angle by a scalar and returns the normalized result.
    /// </summary>
    /// <param name="a">Angle to multiply.</param>
    /// <param name="scalar">Scalar multiplier.</param>
    /// <returns>Normalized scaled angle.</returns>
    public static AngleInRadians operator *(AngleInRadians a, double scalar) => new AngleInRadians(a.Value * scalar);

    /// <summary>
    /// Multiplies a scalar by an angle and returns the normalized result.
    /// </summary>
    /// <param name="scalar">Scalar multiplier.</param>
    /// <param name="a">Angle to multiply.</param>
    /// <returns>Normalized scaled angle.</returns>
    public static AngleInRadians operator *(double scalar, AngleInRadians a) => new AngleInRadians(a.Value * scalar);

    /// <summary>
    /// Divides an angle by a scalar and returns the normalized result.
    /// </summary>
    /// <param name="a">Angle to divide.</param>
    /// <param name="scalar">Scalar divisor.</param>
    /// <returns>Normalized resulting angle.</returns>
    public static AngleInRadians operator /(AngleInRadians a, double scalar) => new AngleInRadians(a.Value / scalar);
}

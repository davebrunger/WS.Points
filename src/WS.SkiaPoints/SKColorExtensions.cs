namespace WS.SkiaPoints;

/// <summary>
/// Helpers to convert between WS.Points <see cref="WS.Points.Drawing.Colour"/> and Skia's <see cref="SKColor"/>.
/// </summary>
public static class SKColorExtensions
{
    /// <summary>
    /// Converts the provided <paramref name="colour"/> to an <see cref="SKColor"/>.
    /// </summary>
    public static SKColor FromColour(Colour colour)
    {
        return new SKColor(colour.Red, colour.Green, colour.Blue, colour.Alpha);
    }

    /// <summary>
    /// Extension helper to convert a <see cref="Colour"/> to <see cref="SKColor"/>.
    /// </summary>
    public static SKColor ToSKColor(this Colour colour) => FromColour(colour);
}
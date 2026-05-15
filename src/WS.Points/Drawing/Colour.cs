namespace WS.Points.Drawing;

/// <summary>
/// Represents an RGBA colour with 8-bit channels.
/// </summary>
/// <param name="Red">Red channel (0-255).</param>
/// <param name="Green">Green channel (0-255).</param>
/// <param name="Blue">Blue channel (0-255).</param>
/// <param name="Alpha">Alpha channel (0-255), defaults to 255 (opaque).</param>
public record Colour(byte Red, byte Green, byte Blue, byte Alpha = 255);
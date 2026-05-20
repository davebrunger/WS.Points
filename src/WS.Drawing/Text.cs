namespace WS.Drawing;

/// <summary>
/// Represents a text shape to be drawn by renderers.
/// </summary>
/// <param name="Value">The text content.</param>
/// <param name="Center">The center point where the text is placed.</param>
/// <param name="FontSizeInPixels">Font size in pixels forwarded to the renderer.</param>
/// <param name="StrokeColour">Colour used to render the text.</param>
/// <param name="Style">The style of the text.</param>
public record Text(string Value, Point Center, float FontSizeInPixels, Colour StrokeColour, TextStyle Style = TextStyle.None) : Shape(null!, FontSizeInPixels, StrokeColour, null!)
{
    /// <summary>
    /// Draws this text shape using the provided drawing context.
    /// </summary>
    /// <param name="context">The drawing context to render into.</param>
    public override void Draw(IDrawingContext context)
    {
        context.DrawText(Value, Center, FontSizeInPixels, StrokeColour, Style);
    }
}
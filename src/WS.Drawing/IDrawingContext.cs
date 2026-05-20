namespace WS.Drawing;

/// <summary>
/// Abstraction for a drawing surface/renderer consumed by shapes and elements.
/// Implementations should map polygon and ellipse drawing requests to concrete drawing APIs.
/// </summary>
public interface IDrawingContext
{
    /// <summary>
    /// Draws the provided <paramref name="polygon"/> using the specified stroke and fill parameters.
    /// </summary>
    void DrawPolygon(Polygon polygon, double strokeWidth, Colour strokeColour, Colour fillColour);

    /// <summary>
    /// Draws the provided <paramref name="ellipse"/> using the specified stroke and fill parameters.
    /// </summary>
    void DrawEllipse(Ellipse ellipse, double strokeWidth, Colour strokeColour, Colour fillColour);

    /// <summary>
    /// Draws the provided text at the specified location using the specified colour.
    /// </summary>
    /// <param name="text">Text to draw.</param>
    /// <param name="centre">Centre point where the text will be placed.</param>
    /// <param name="fontSizeInPixels">Font size in pixels to use when rendering the text.</param>
    /// <param name="strokeColour">Colour to render the text with.</param>
    /// <param name="style">The style of the text.</param>
    void DrawText(string text, Point centre, float fontSizeInPixels, Colour strokeColour, TextStyle style);
}
namespace WS.Points.Drawing;

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
}
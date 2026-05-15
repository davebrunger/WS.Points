namespace WS.Points.Elements;

/// <summary>
/// Context passed to elements when they are asked to draw themselves by a higher-level shape.
/// Implementations typically forward to an underlying <see cref="Drawing.IDrawingContext"/> with
/// the shape-level stroke/fill parameters applied.
/// </summary>
public interface IElementContext
{
    /// <summary>
    /// Request that the provided <paramref name="polygon"/> be drawn using the enclosing shape's parameters.
    /// </summary>
    void DrawPolygon(Polygon polygon);

    /// <summary>
    /// Request that the provided <paramref name="ellipse"/> be drawn using the enclosing shape's parameters.
    /// </summary>
    void DrawEllipse(Ellipse ellipse);
}
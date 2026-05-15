namespace WS.RendererCli.Drawings;

/// <summary>
/// Represents a drawable composition of <see cref="Shape"/>s intended for the CLI renderer.
/// </summary>
public interface IDrawing
{
    /// <summary>
    /// Width of the drawing in pixels.
    /// </summary>
    int WidthPixels { get; }

    /// <summary>
    /// Height of the drawing in pixels.
    /// </summary>
    int HeightPixels { get; }

    /// <summary>
    /// Origin point (in pixel coordinates) for the drawing.
    /// </summary>
    Point Origin { get; }

    /// <summary>
    /// Shapes that make up this drawing.
    /// </summary>
    IEnumerable<Shape> Shapes { get; }
}
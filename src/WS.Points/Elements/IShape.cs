namespace WS.Points.Elements;

/// <summary>
/// Common interface for geometric shapes that support basic transforms and drawing.
/// </summary>
public interface IShape
{
    /// <summary>
    /// Scales the shape by the specified factors along the X and Y axes.
    /// Scaling is performed relative to the origin (0,0).
    /// </summary>
    /// <param name="scaleX">Scale factor along the X axis.</param>
    /// <param name="scaleY">Scale factor along the Y axis.</param>
    /// <returns>A new shape that is the result of scaling this shape about the origin.</returns>
    IShape Scale(double scaleX, double scaleY);

    /// <summary>
    /// Uniformly scales the shape about the origin.
    /// </summary>
    /// <param name="factor">Uniform scale factor.</param>
    /// <returns>A new shape scaled about the origin.</returns>
    IShape Scale(double factor);

    /// <summary>
    /// Rotates the shape about the origin by the specified angle.
    /// </summary>
    /// <param name="angle">Rotation angle.</param>
    /// <returns>The rotated shape.</returns>
    IShape Rotate(AngleInRadians angle);

    /// <summary>
    /// Reflects the shape across the provided axis line.
    /// </summary>
    /// <param name="axis">Axis line to reflect across.</param>
    /// <returns>The reflected shape.</returns>
    IShape Flip(Line axis);

    /// <summary>
    /// Translates the shape by the given offset vector.
    /// </summary>
    /// <param name="offset">Translation vector.</param>
    /// <returns>The translated shape.</returns>
    IShape Translate(Vector offset);
    
    /// <summary>
    /// Draws the shape using the supplied <paramref name="context"/>.
    /// </summary>
    /// <param name="context">Element context providing draw callbacks.</param>
    void Draw(IElementContext context);
}

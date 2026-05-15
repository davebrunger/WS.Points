namespace WS.Points.Drawing;

/// <summary>
/// Represents a drawable shape combining an <see cref="IShape"/> element with stroke and fill parameters.
/// </summary>
/// <param name="Element">Underlying element implementing <see cref="IShape"/>.</param>
/// <param name="StrokeWidth">Stroke width used when drawing the element.</param>
/// <param name="StrokeColour">Stroke colour.</param>
/// <param name="FillColour">Fill colour.</param>
public record Shape(IShape Element, double StrokeWidth, Colour StrokeColour, Colour FillColour)
{
    /// <summary>
    /// Scales the underlying element. By default the shape's <see cref="StrokeWidth"/> is scaled
    /// with the element; pass <c>maintainStrokeWidth: true</c> to keep the stroke width unchanged.
    /// </summary>
    /// <param name="scaleX">Scale factor in the X axis.</param>
    /// <param name="scaleY">Scale factor in the Y axis.</param>
    /// <param name="maintainStrokeWidth">When <c>true</c> the stroke width is not scaled.</param>
    /// <returns>A new <see cref="Shape"/> with the scaled element and adjusted stroke width.</returns>
    public Shape Scale(double scaleX, double scaleY, bool maintainStrokeWidth = false)
    {
        var scaledElement = Element.Scale(scaleX, scaleY);
        if (maintainStrokeWidth)
        {
            return this with { Element = scaledElement };
        }

        var sw = StrokeWidth * Math.Abs((scaleX + scaleY) / 2.0);
        return this with { Element = scaledElement, StrokeWidth = sw };
    }

    /// <summary>
    /// Uniform scale. By default the shape's <see cref="StrokeWidth"/> is scaled
    /// with the element; pass <c>maintainStrokeWidth: true</c> to keep the stroke width unchanged.
    /// </summary>
    /// <param name="factor">Uniform scale factor.</param>
    /// <param name="maintainStrokeWidth">When <c>true</c> the stroke width is not scaled.</param>
    /// <returns>A new <see cref="Shape"/> with the scaled element and adjusted stroke width.</returns>
    public Shape Scale(double factor, bool maintainStrokeWidth = false)
    {
        var scaledElement = Element.Scale(factor);
        if (maintainStrokeWidth)
        {
            return this with { Element = scaledElement };
        }

        var sw = StrokeWidth * Math.Abs(factor);
        return this with { Element = scaledElement, StrokeWidth = sw };
    }

    /// <summary>
    /// Rotates the shape's element about the origin by <paramref name="angle"/>.
    /// </summary>
    /// <param name="angle">Rotation angle in radians.</param>
    public Shape Rotate(AngleInRadians angle)
    {
        var rotated = Element.Rotate(angle);
        return this with { Element = rotated };
    }

    /// <summary>
    /// Reflects the shape's element across the provided <paramref name="axis"/> line.
    /// </summary>
    /// <param name="axis">Axis line to reflect across.</param>
    public Shape Flip(Line axis)
    {
        var flipped = Element.Flip(axis);
        return this with { Element = flipped };
    }
    /// <summary>
    /// Translates the shape's element by the given <paramref name="offset"/> vector.
    /// </summary>
    /// <param name="offset">Translation vector.</param>
    public Shape Translate(Vector offset)
    {
        var translated = Element.Translate(offset);
        return this with { Element = translated };
    }
    /// <summary>
    /// Creates a polygon shape from the provided points.
    /// </summary>
    /// <param name="strokeWidth">Stroke width for the polygon outline.</param>
    /// <param name="strokeColour">Stroke colour.</param>
    /// <param name="fillColour">Fill colour.</param>
    /// <param name="points">Points that make up the polygon, in order.</param>
    /// <returns>A new <see cref="Shape"/> wrapping a <see cref="WS.Points.Elements.Polygon"/> element.</returns>
    public static Shape Polygon(double strokeWidth, Colour strokeColour, Colour fillColour, params Point[] points)
    {
        var polygon = new Polygon(points.ToImmutableList());
        return new Shape(polygon, strokeWidth, strokeColour, fillColour);
    }   
    /// <summary>
    /// Creates a polygon shape from the provided points.
    /// </summary>
    /// <param name="strokeWidth">Stroke width for the polygon outline.</param>
    /// <param name="strokeColour">Stroke colour.</param>
    /// <param name="fillColour">Fill colour.</param>
    /// <param name="points">Sequence of points that make up the polygon, in order.</param>
    /// <returns>A new <see cref="Shape"/> wrapping a <see cref="WS.Points.Elements.Polygon"/> element.</returns>
    public static Shape Polygon(double strokeWidth, Colour strokeColour, Colour fillColour, IEnumerable<Point> points)
    {
        var polygon = new Polygon(points.ToImmutableList());
        return new Shape(polygon, strokeWidth, strokeColour, fillColour);
    }
    /// <summary>
    /// Creates a regular polygon shape with the specified circumradius and number of sides.
    /// </summary>
    /// <param name="strokeWidth">Stroke width for the polygon outline.</param>
    /// <param name="strokeColour">Stroke colour.</param>
    /// <param name="fillColour">Fill colour.</param>
    /// <param name="radius">Circumradius of the regular polygon.</param>
    /// <param name="sides">Number of sides (must be &gt;= 3).</param>
    /// <returns>A new <see cref="Shape"/> wrapping a regular <see cref="WS.Points.Elements.Polygon"/> element.</returns>
    public static Shape RegularPolygon(double strokeWidth, Colour strokeColour, Colour fillColour, double radius, int sides)
    {
        return new Shape(Elements.Polygon.RegularPolygon(radius, sides), strokeWidth, strokeColour, fillColour);
    }   

    /// <summary>
    /// Creates a line shape of the given <paramref name="length"/> aligned along the X axis.
    /// </summary>
    /// <param name="strokeWidth">Stroke width for the line.</param>
    /// <param name="strokeColour">Stroke colour.</param>
    /// <param name="fillColour">Fill colour (unused for lines but kept for API uniformity).</param>
    /// <param name="length">Length of the line in the X direction.</param>
    /// <returns>A new <see cref="Shape"/> representing the line.</returns>
    public static Shape Line(double strokeWidth, Colour strokeColour, Colour fillColour, double length)
    {
        return Polygon(strokeWidth, strokeColour, fillColour, new Point(0, 0), new Point(length, 0));
    }
    
    /// <summary>
    /// Creates an ellipse shape with the given parameters.
    /// </summary>
    /// <param name="strokeWidth">Stroke width for the ellipse outline.</param>
    /// <param name="strokeColour">Stroke colour.</param>
    /// <param name="fillColour">Fill colour.</param>
    /// <param name="centre">Centre point of the ellipse.</param>
    /// <param name="majorAxisDirection">Unit vector indicating the major axis direction.</param>
    /// <param name="majorRadius">Major radius length.</param>
    /// <param name="minorRadius">Minor radius length.</param>
    /// <returns>A new <see cref="Shape"/> wrapping an <see cref="Ellipse"/> element.</returns>
    public static Shape Ellipse(double strokeWidth, Colour strokeColour, Colour fillColour, Point centre, Vector majorAxisDirection, double majorRadius, double minorRadius)
    {
        var ellipse = new Ellipse(centre, majorAxisDirection, majorRadius, minorRadius);
        return new Shape(ellipse, strokeWidth, strokeColour, fillColour);
    }

    /// <summary>
    /// Creates a circle shape with the given centre and radius.
    /// </summary>
    /// <param name="strokeWidth">Stroke width for the circle outline.</param>
    /// <param name="strokeColour">Stroke colour.</param>
    /// <param name="fillColour">Fill colour.</param>
    /// <param name="centre">Centre point of the circle.</param>
    /// <param name="radius">Radius of the circle.</param>
    /// <returns>A new <see cref="Shape"/> wrapping an <see cref="Ellipse"/> that represents the circle.</returns>
    public static Shape Circle(double strokeWidth, Colour strokeColour, Colour fillColour, Point centre, double radius)
    {
        var circle = new Ellipse(centre, Vector.UnitX, radius, radius);
        return new Shape(circle, strokeWidth, strokeColour, fillColour);
    }

    /// <summary>
    /// Creates a circle shape centred at the origin with the specified radius.
    /// </summary>
    /// <param name="strokeWidth">Stroke width for the circle outline.</param>
    /// <param name="strokeColour">Stroke colour.</param>
    /// <param name="fillColour">Fill colour.</param>
    /// <param name="radius">Radius of the circle.</param>
    /// <returns>A new <see cref="Shape"/> wrapping a centered <see cref="Ellipse"/>.</returns>
    public static Shape Circle(double strokeWidth, Colour strokeColour, Colour fillColour, double radius)
    {
        var circle = new Ellipse(new Point(0, 0), Vector.UnitX, radius, radius);
        return new Shape(circle, strokeWidth, strokeColour, fillColour);
    }

    /// <summary>
    /// Draws this shape using the provided <see cref="IDrawingContext"/>.
    /// </summary>
    /// <param name="context">Drawing context to use for rendering.</param>
    public void Draw(IDrawingContext context)
    {
        Element.Draw(new DrawingElementContext(context, StrokeWidth, StrokeColour, FillColour));
    }

    private class DrawingElementContext : IElementContext
    {
        public IDrawingContext Context { get; }
        public double StrokeWidth { get; }
        public Colour StrokeColour { get; }
        public Colour FillColour { get; }

        public DrawingElementContext(IDrawingContext context, double strokeWidth, Colour strokeColour, Colour fillColour)
        {
            Context = context;
            StrokeWidth = strokeWidth;
            StrokeColour = strokeColour;
            FillColour = fillColour;
        }

        public void DrawEllipse(Ellipse ellipse)
        {
            Context.DrawEllipse(ellipse, StrokeWidth, StrokeColour, FillColour);
        }

        public void DrawPolygon(Polygon polygon)
        {
            Context.DrawPolygon(polygon, StrokeWidth, StrokeColour, FillColour);
        }
    }
}

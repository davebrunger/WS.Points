namespace WS.Points.Elements;

/// <summary>
/// Represents a polygon as an immutable list of vertices.
/// </summary>
public record Polygon(ImmutableList<Point> Points) : IReadOnlyList<Point>, IShape
{
    /// <summary>
    /// Number of vertices in the polygon.
    /// </summary>
    public int Count
    {
        get
        {
            return Points.Count;
        }
    }

    /// <summary>
    /// Indexer to access the vertex at the given index.
    /// </summary>
    public Point this[int index]
    {
        get
        {
            return Points[index];
        }
    }

    /// <summary>
    /// Creates a polygon from a params list of points.
    /// </summary>
    public Polygon(params Point[] points) : this(points.ToImmutableList())
    {
    }

    /// <summary>
    /// Creates a polygon from an enumerable of points.
    /// </summary>
    public Polygon(IEnumerable<Point> points) : this(points.ToImmutableList())
    {
    }

    /// <summary>
    /// Returns an enumerator over the polygon vertices.
    /// </summary>
    public IEnumerator<Point> GetEnumerator()
    {
        return ((IEnumerable<Point>)Points).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    /// <summary>
    /// Creates a polygon from the provided points.
    /// </summary>
    public static Polygon From(params Point[] points)
    {
        return new Polygon(points.ToImmutableList());
    }

    /// <summary>
    /// Creates a regular polygon with the specified circumradius and number of sides, centered on the origin.
    /// </summary>
    /// <param name="radius">Circumradius of the polygon.</param>
    /// <param name="sides">Number of sides (>= 3).</param>
    public static Polygon RegularPolygon(double radius, int sides)
    {
        if (double.IsNaN(radius) || double.IsInfinity(radius))
        {
            throw new ArgumentException("Radius must be a finite number.", nameof(radius));
        }
        if (sides < 3)
        {
            throw new ArgumentOutOfRangeException(nameof(sides), "sides must be >= 3");
        }

        var startPoint = new Point(radius, 0);
        double angleStep = 2.0 * Math.PI / sides;

        var pts = Enumerable.Range(0, sides)
            .Select(i => startPoint.Rotate(angleStep * i))
            .ToImmutableList();

        return new Polygon(pts);
    }

    /// <summary>
    /// Returns a new polygon rotated about the origin by the specified angle.
    /// </summary>
    public Polygon Rotate(AngleInRadians angle)
    {
        return new Polygon(Points.Select(p => p.Rotate(angle)));
    }

    /// <summary>
    /// Returns a new polygon reflected across the specified <see cref="Line"/>.
    /// </summary>
    public Polygon Flip(Line axis)
    {
        return new Polygon(Points.Select(p => p.Flip(axis)));
    }

    /// <summary>
    /// Uniformly scales the polygon about the origin.
    /// </summary>
    public Polygon Scale(double factor)
    {
        return Scale(factor, factor);
    }

    /// <summary>
    /// Scales the polygon by independent X and Y factors about the origin.
    /// </summary>
    public Polygon Scale(double scaleX, double scaleY)
    {
        return new Polygon(Points.Select(p => new Point(p.X * scaleX, p.Y * scaleY)));
    }

    /// <summary>
    /// Translates the polygon by the given offset vector.
    /// </summary>
    public Polygon Translate(Vector offset)
    {
        return new Polygon(Points.Select(p => p.Translate(offset)));
    }

    /// <summary>
    /// Draws the polygon using the provided <paramref name="context"/>.
    /// </summary>
    /// <param name="context">Element drawing context.</param>
    public void Draw(IElementContext context)
    {
        context.DrawPolygon(this);
    }

    IShape IShape.Scale(double scaleX, double scaleY)
    {
        return Scale(scaleX, scaleY);
    }

    IShape IShape.Scale(double factor)
    {
        return Scale(factor);
    }

    IShape IShape.Rotate(AngleInRadians angle)
    {
        return Rotate(angle);
    }

    IShape IShape.Flip(Line axis)
    {
        return Flip(axis);
    }

    IShape IShape.Translate(Vector offset)
    {
        return Translate(offset);
    }
}

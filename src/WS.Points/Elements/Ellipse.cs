namespace WS.Points.Elements;

/// <summary>
/// Represents a general ellipse defined by two radius vectors from the centre.
/// The ellipse is the set <c>Centre + MajorRadius * cos(t) + MinorRadius * sin(t)</c> for t in [0, 2&#x03C0;).
/// Using vectors for the radii allows anisotropic scaling and rotation of the principal axes.
/// </summary>
public record Ellipse : IShape
{
    /// <summary>
    /// The centre point of the ellipse.
    /// </summary>
    public Point Centre { get; init; }

    /// <summary>
    /// Unit vector pointing along the major axis direction.
    /// </summary>
    public Vector MajorAxisDirection { get; init; }

    /// <summary>
    /// Scalar length of the semi-major axis.
    /// </summary>
    public double MajorRadius { get; init; }

    /// <summary>
    /// Scalar length of the semi-minor axis.
    /// </summary>
    public double MinorRadius { get; init; }

    /// <summary>
    /// Initializes a new instance of <see cref="Ellipse"/> with the specified centre, major axis direction and radii.
    /// </summary>
    /// <param name="centre">Ellipse centre.</param>
    /// <param name="majorAxisDirection">Direction of the major axis (non-zero; will be normalized).</param>
    /// <param name="majorRadius">Length of the semi-major axis (positive).</param>
    /// <param name="minorRadius">Length of the semi-minor axis (positive).</param>
    /// <exception cref="ArgumentException">Thrown when direction is zero or either radius is non-positive.</exception>
    public Ellipse(Point centre, Vector majorAxisDirection, double majorRadius, double minorRadius)
    {
        if (majorAxisDirection.X == 0.0 && majorAxisDirection.Y == 0.0)
            throw new ArgumentException("Major axis direction must be non-zero.");
        if (majorRadius <= 0.0 || minorRadius <= 0.0)
            throw new ArgumentException("Radii must be positive.");

        Centre = centre;
        MajorAxisDirection = majorAxisDirection.Unit();
        MajorRadius = majorRadius;
        MinorRadius = minorRadius;
    }

    /// <summary>
    /// Determines whether the specified point lies inside or on the ellipse.
    /// Uses the parameterisation <c>Centre + MajorRadius*c + MinorRadius*s</c> and checks <c>c^2 + s^2 &lt;= 1</c>.
    /// </summary>
    /// <param name="p">Point to test.</param>
    /// <returns>True if point is inside or on the ellipse; otherwise false.</returns>
    public bool Contains(Point p)
    {
        var vec = Vector.FromPoints(Centre, p);

        // Project onto major axis and its perpendicular (both unit-length)
        var dotMajor = vec.Dot(MajorAxisDirection);
        var perp = new Vector(-MajorAxisDirection.Y, MajorAxisDirection.X);
        var dotPerp = vec.Dot(perp);

        var u = dotMajor / MajorRadius;
        var w = dotPerp / MinorRadius;

        return u * u + w * w <= 1.0 + 1e-12;
    }

    /// <summary>
    /// Returns a translated ellipse.
    /// </summary>
    public Ellipse Translate(Vector offset)
    {
        return new Ellipse(Centre.Translate(offset), MajorAxisDirection, MajorRadius, MinorRadius);
    }

    /// <summary>
    /// Rotates the ellipse (centre and radius vectors) about the origin by the specified angle.
    /// </summary>
    public Ellipse Rotate(AngleInRadians angle)
    {
        return new Ellipse(Centre.Rotate(angle), MajorAxisDirection.Rotate(angle), MajorRadius, MinorRadius);
    }

    /// <summary>
    /// Scales the ellipse by separate X/Y scaling factors applied to the radius vectors' components.
    /// The scaling is performed relative to the origin (0,0); the centre is scaled accordingly.
    /// </summary>
    public Ellipse Scale(double scaleX, double scaleY)
    {
        // Transform the two radius vectors and then compute principal axes via eigen-decomposition
        // majVec = MajorAxisDirection * MajorRadius
        var majVecX = MajorAxisDirection.X * MajorRadius;
        var majVecY = MajorAxisDirection.Y * MajorRadius;
        // minVec is the perpendicular direction times MinorRadius
        var minVecX = -MajorAxisDirection.Y * MinorRadius;
        var minVecY = MajorAxisDirection.X * MinorRadius;

        // Apply non-uniform scaling
        var a_x = majVecX * scaleX;
        var a_y = majVecY * scaleY;
        var b_x = minVecX * scaleX;
        var b_y = minVecY * scaleY;

        // Symmetric matrix Q = A * A^T where A = [a b]
        var q00 = a_x * a_x + b_x * b_x;
        var q01 = a_x * a_y + b_x * b_y;
        var q11 = a_y * a_y + b_y * b_y;

        var trace = q00 + q11;
        var det = q00 * q11 - q01 * q01;
        var disc = Math.Max(0.0, trace * trace - 4.0 * det);
        var sqrtDisc = Math.Sqrt(disc);
        var lambda1 = (trace + sqrtDisc) / 2.0;
        var lambda2 = (trace - sqrtDisc) / 2.0;

        // Singular values (semi-axis lengths)
        var sigma1 = Math.Sqrt(Math.Max(0.0, lambda1));
        var sigma2 = Math.Sqrt(Math.Max(0.0, lambda2));

        // Compute eigenvector for lambda1
        double v1x, v1y;
        if (Math.Abs(q01) > 1e-12)
        {
            v1x = q01;
            v1y = lambda1 - q00;
        }
        else
        {
            // Diagonal matrix: eigenvectors are standard basis
            if (q00 >= q11)
            {
                v1x = 1.0; v1y = 0.0;
            }
            else
            {
                v1x = 0.0; v1y = 1.0;
            }
        }

        var len = Math.Sqrt(v1x * v1x + v1y * v1y);
        if (len == 0.0)
        {
            throw new InvalidOperationException("Failed to compute ellipse principal axes.");
        }
        var dirX = v1x / len;
        var dirY = v1y / len;

        var newCentre = new Point(Centre.X * scaleX, Centre.Y * scaleY);
        var newMajorDir = new Vector(dirX, dirY);
        var newMajorRadius = sigma1;
        var newMinorRadius = sigma2;

        return new Ellipse(newCentre, newMajorDir, newMajorRadius, newMinorRadius);
    }

    /// <summary>
    /// Uniformly scales the ellipse about the origin.
    /// </summary>
    public Ellipse Scale(double factor)
    {
        return Scale(factor, factor);
    }

    /// <summary>
    /// Reflects the ellipse across the given axis line.
    /// </summary>
    public Ellipse Flip(Line axis)
    {
        var newCentre = Centre.Flip(axis);
        var newDir = MajorAxisDirection.Flip(axis.Direction).Unit();
        return new Ellipse(newCentre, newDir, MajorRadius, MinorRadius);
    }

    /// <summary>
    /// Draws the ellipse using the provided <paramref name="context"/>.
    /// </summary>
    /// <param name="context">Element drawing context.</param>
    public void Draw(IElementContext context)
    {
        context.DrawEllipse(this);
    }

    /// <summary>
    /// Creates a circular <see cref="Ellipse"/> with the specified radius centred at the origin.
    /// </summary>
    /// <param name="radius">Radius of the circle (positive).</param>
    /// <returns>A new <see cref="Ellipse"/> representing the circle.</returns>
    public static Ellipse Circle(double radius)
    {
        return new Ellipse(Point.Origin, Vector.UnitX, radius, radius);
    }

    /// <summary>
    /// Creates a circular <see cref="Ellipse"/> with the specified centre and radius.
    /// </summary>
    /// <param name="centre">Centre point of the circle.</param>
    /// <param name="radius">Radius of the circle (positive).</param>
    /// <returns>A new <see cref="Ellipse"/> representing the circle.</returns>
    public static Ellipse Circle(Point centre, double radius)
    {
        return new Ellipse(centre, Vector.UnitX, radius, radius);
    }

    /// <inheritdoc/>
    IShape IShape.Scale(double scaleX, double scaleY)
    {
        return Scale(scaleX, scaleY);
    }

    /// <inheritdoc/>
    IShape IShape.Scale(double factor)
    {
        return Scale(factor);
    }

    /// <inheritdoc/>
    IShape IShape.Rotate(AngleInRadians angle)
    {
        return Rotate(angle);
    }

    /// <inheritdoc/>
    IShape IShape.Flip(Line axis)
    {
        return Flip(axis);
    }

    /// <inheritdoc/>
    IShape IShape.Translate(Vector offset)
    {
        return Translate(offset);
    }
}

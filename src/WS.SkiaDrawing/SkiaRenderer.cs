namespace WS.SkiaPoints;

/// <summary>
/// Minimal renderer wrapper that demonstrates consuming WS.Points types with SkiaSharp.
/// </summary>
public class SkiaRenderer : IDrawingContext
{
    private readonly SKCanvas canvas;
    private readonly float originCanvasX;
    private readonly float originCanvasY;

    /// <summary>
    /// Initializes a new instance of <see cref="SkiaRenderer"/> which will draw into the provided <paramref name="canvas"/>.
    /// </summary>
    /// <param name="canvas">Skia canvas to draw into.</param>
    /// <param name="originUp">Origin location expressed in a y-positive-up coordinate system (pixels). For example, (0,0) specifies the bottom-left of the canvas. If <c>null</c>, the origin defaults to (0,0).</param>
    public SkiaRenderer(SKCanvas canvas, Point? originUp = null)
    {
        this.canvas = canvas;

        // Calculate the origin in Skia canvas device coordinates. The supplied
        // origin is in a y-positive-up coordinate system (pixels). For example,
        // originUp = (0,0) indicates the bottom-left of the canvas. Skia's
        // coordinate system is y-positive-down, so convert by subtracting the
        // supplied y from the canvas height.
        var bounds = canvas.LocalClipBounds;
        var canvasHeight = bounds.Height;
        if (canvasHeight == 0)
        {
            // Some SKCanvas instances report an empty LocalClipBounds; fall back to DeviceClipBounds.
            canvasHeight = canvas.DeviceClipBounds.Height;
        }

        // (debug logging removed)

        var origin = originUp ?? Point.Origin;
        originCanvasX = (float)origin.X;
        originCanvasY = canvasHeight - (float)origin.Y;
    }

    /// <summary>
    /// Draws a filled ellipse to the configured Skia canvas using the given <paramref name="ellipse"/>.
    /// This is a simple example showing how to map WS.Points types to Skia primitives.
    /// </summary>
    public void DrawEllipse(Ellipse ellipse, double strokeWidth, Colour strokeColour, Colour fillColour)
    {
        // Compute transform: ellipse centre
        var cx = (float)ellipse.Centre.X;
        var cy = (float)ellipse.Centre.Y;

        // Build radii vectors from direction + scalar radii
        var majX = (float)(ellipse.MajorAxisDirection.X * ellipse.MajorRadius);
        var majY = (float)(ellipse.MajorAxisDirection.Y * ellipse.MajorRadius);
        var minX = (float)(-ellipse.MajorAxisDirection.Y * ellipse.MinorRadius);
        var minY = (float)(ellipse.MajorAxisDirection.X * ellipse.MinorRadius);

        // Approximate ellipse transform via matrix that maps unit circle to ellipse
        var matrix = new SKMatrix
        {
            ScaleX = majX,
            SkewX = majY,
            TransX = cx,
            SkewY = minX,
            ScaleY = minY,
            TransY = cy,
            Persp0 = 0,
            Persp1 = 0,
            Persp2 = 1
        };

        using var fillPaint = new SKPaint
        {
            Style = SKPaintStyle.Fill,
            Color = fillColour.ToSKColor(),
            BlendMode = SKBlendMode.Src,
            IsAntialias = true
        };

        using var strokePaint = new SKPaint
        {
            Style = SKPaintStyle.Stroke,
            Color = strokeColour.ToSKColor(),
            StrokeWidth = (float)strokeWidth,
            IsStroke = true,
            BlendMode = SKBlendMode.Src,
            IsAntialias = true
        };

        canvas.Save();
        // Apply origin transform: translate to the requested origin, then flip Y
        // so that the drawing coordinate system is y-positive-up.
        canvas.Translate(originCanvasX, originCanvasY);
        canvas.Scale(1, -1);
        canvas.Concat(in matrix);
        // draw unit circle transformed by matrix => ellipse
        canvas.DrawOval(new SKRect(-1, -1, 1, 1), fillPaint);
        if (strokeWidth > 0)
        {
            canvas.DrawOval(new SKRect(-1, -1, 1, 1), strokePaint);
        }
        canvas.Restore();
    }

    /// <summary>
    /// Draws a filled and stroked polygon on the configured canvas.
    /// </summary>
    /// <param name="polygon">Polygon to draw.</param>
    /// <param name="strokeWidth">Stroke width for outline.</param>
    /// <param name="strokeColour">Stroke colour.</param>
    /// <param name="fillColour">Fill colour.</param>
    public void DrawPolygon(Polygon polygon, double strokeWidth, Colour strokeColour, Colour fillColour)
    {
        if (polygon is null)
        {
            return;
        }

        var pts = polygon.Points;
        if (pts == null || pts.Count == 0)
        {
            return;
        }

        using var path = new SKPath();

        // Move to first
        var first = pts[0];
        path.MoveTo((float)first.X, (float)first.Y);

        for (int i = 1; i < pts.Count; i++)
        {
            var p = pts[i];
            path.LineTo((float)p.X, (float)p.Y);
        }

        // Close the polygon
        path.Close();

        using var fillPaint = new SKPaint
        {
            Style = SKPaintStyle.Fill,
            Color = fillColour.ToSKColor(),
            BlendMode = SKBlendMode.Src,
            IsAntialias = true
        };

        using var strokePaint = new SKPaint
        {
            Style = SKPaintStyle.Stroke,
            Color = strokeColour.ToSKColor(),
            StrokeWidth = (float)strokeWidth,
            IsStroke = true,
            BlendMode = SKBlendMode.Src,
            IsAntialias = true
        };

        // Apply origin transform so polygon coordinates (which are y-positive-up)
        // render correctly on Skia's y-positive-down canvas.
        canvas.Save();
        canvas.Translate(originCanvasX, originCanvasY);
        canvas.Scale(1, -1);
        canvas.DrawPath(path, fillPaint);
        if (strokeWidth > 0)
        {
            canvas.DrawPath(path, strokePaint);
        }
        canvas.Restore();
    }

    /// <summary>
    /// Draws text centered at the specified location using the configured Skia canvas.
    /// </summary>
    /// <param name="text">Text to render.</param>
    /// <param name="centre">Centre point (in y-positive-up coordinates) where the text will be placed.</param>
    /// <param name="fontSizeInPixels">Font size in pixels to use when rendering the text.</param>
    /// <param name="strokeColour">Colour to use for the rendered text.</param>
    /// <param name="style">The style of the text.</param>
    public void DrawText(string text, Point centre, float fontSizeInPixels, Colour strokeColour, TextStyle style)
    {
        using var paint = new SKPaint
        {
            Style = SKPaintStyle.Fill,
            Color = strokeColour.ToSKColor(),
            BlendMode = SKBlendMode.Src,
            IsAntialias = true,
        };

        var font = new SKFont
        {
            // Note that although SKFont documentation describes Size as "The size of the font in points", in practice 
            // it appears to render at the specified pixel size. This matches our expectation since the WS.Points API 
            // specifies font size in pixels.
            Size = fontSizeInPixels,
                Typeface = style switch
                {
                    TextStyle s when s.HasFlag(TextStyle.Bold) && s.HasFlag(TextStyle.Italic) => SKTypeface.FromFamilyName(null, SKFontStyle.BoldItalic),
                    TextStyle s when s.HasFlag(TextStyle.Bold) => SKTypeface.FromFamilyName(null, SKFontStyle.Bold),
                    TextStyle s when s.HasFlag(TextStyle.Italic) => SKTypeface.FromFamilyName(null, SKFontStyle.Italic),
                    _ => SKTypeface.FromFamilyName(null, SKFontStyle.Normal)
                }
        };
        
        var yOffset = font.Metrics.CapHeight / 2;
        canvas.Save();
        canvas.Translate(originCanvasX, originCanvasY + yOffset);
        
        // We want to render text with y-positive-up coordinates, but Skia's DrawText doesn't support arbitrary transforms 
        // like DrawPath does, so we need to flip the text ourselves by negating the Y coordinate.
        canvas.DrawText(text, (float)centre.X, -(float)centre.Y, SKTextAlign.Center, font, paint);

        canvas.Restore();
    }
}

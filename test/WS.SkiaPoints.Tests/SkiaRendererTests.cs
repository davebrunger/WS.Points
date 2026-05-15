namespace WS.SkiaPoints.Tests;

public class SkiaRendererTests
{
    [Fact]
    public void DrawEllipse_RendersPixels()
    {
        using var bitmap = new SKBitmap(100, 100, SKColorType.Rgba8888, SKAlphaType.Premul);
        using var canvas = new SKCanvas(bitmap);

        var renderer = new SkiaRenderer(canvas);

        var ellipse = new Ellipse(new Point(50, 50), new Vector(1, 0), 20.0, 10.0);
        renderer.DrawEllipse(ellipse, 2.0, new Colour(0, 0, 0), new Colour(255, 0, 0));

        var c = bitmap.GetPixel(50, 50);
        Assert.NotEqual(SKColors.Transparent, c);
    }

    [Fact]
    public void DrawPolygon_RendersPixels()
    {
        using var bitmap = new SKBitmap(100, 100, SKColorType.Rgba8888, SKAlphaType.Premul);
        using var canvas = new SKCanvas(bitmap);

        var renderer = new SkiaRenderer(canvas);

        var poly = Polygon.From(new Point(10, 10), new Point(90, 10), new Point(50, 80));
        renderer.DrawPolygon(poly, 2.0, new Colour(0, 0, 0), new Colour(255, 0, 0));

        var c = bitmap.GetPixel(50, 30);
        Assert.NotEqual(SKColors.Transparent, c);
    }
}

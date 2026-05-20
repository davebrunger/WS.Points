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
    IEnumerable<IDrawable> Shapes { get; }

    /// <summary>
    /// Suggested output filename for the drawing. May include directory components;
    /// the caller will sanitize and ensure a `.png` extension when saving.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Renders the drawing into a PNG and writes it to a stream provided by <paramref name="getStream"/>.
    /// This default implementation uses SkiaSharp to render the shapes into an <c>SKBitmap</c>, encodes
    /// the bitmap as PNG and then writes it to the supplied stream.
    /// </summary>
    /// <param name="getStream">A factory that accepts the preferred filename and returns a tuple of the
    /// resolved path and an open writable <see cref="Stream"/> to which the PNG will be written. The
    /// caller is responsible for providing a stream with appropriate lifetime; this implementation will
    /// dispose the stream after writing.</param>
    /// <returns>The path returned by <paramref name="getStream"/> where the PNG was saved.</returns>
    string SaveAsPng(Func<string, string, (string Path, Stream Stream)> getStream)
    {
        using var bitmap = new SKBitmap(WidthPixels, HeightPixels, SKColorType.Rgba8888, SKAlphaType.Premul);
        using var canvas = new SKCanvas(bitmap);

        // Clear to transparent
        canvas.Clear(SKColors.Transparent);

        var renderer = new SkiaRenderer(canvas, Origin);

        foreach (var shape in Shapes)
        {
            shape.Draw(renderer);
        }

        // Encode and save as PNG
        using var image = SKImage.FromBitmap(bitmap);
        using var data = image.Encode(SKEncodedImageFormat.Png, 100);
        var (path, stream) = getStream(Name, "png");
        using (stream)
        {
            data.SaveTo(stream);
        }
        return path;
    }
}
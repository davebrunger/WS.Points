const string OutputFileName = "image.png";

var solutionRoot = FindSolutionRoot();
if (solutionRoot is null)
{
    Console.Error.WriteLine("Solution root not found. Saving output to current directory.");
    solutionRoot = Directory.GetCurrentDirectory();
}

var outputDir = Path.Combine(solutionRoot, "output");
Directory.CreateDirectory(outputDir);

var outputPath = Path.Combine(outputDir, OutputFileName);

using var bitmap = DrawShapes(new SpellLevel(4));

// Encode and save as PNG
using var image = SKImage.FromBitmap(bitmap);
using var data = image.Encode(SKEncodedImageFormat.Png, 100);
using var fs = File.OpenWrite(outputPath);
data.SaveTo(fs);

Console.WriteLine($"Saved image to {outputPath}");

SKBitmap DrawShapes(IDrawing drawing)
{
    var bitmap = new SKBitmap(drawing.WidthPixels, drawing.HeightPixels, SKColorType.Rgba8888, SKAlphaType.Premul);
    using var canvas = new SKCanvas(bitmap);

    // Clear to transparent
    canvas.Clear(SKColors.Transparent);

    var renderer = new SkiaRenderer(canvas, drawing.Origin);
    
    foreach (var shape in drawing.Shapes)
    {
        shape.Draw(renderer);
    }

    // (debug pixel sampling removed)

    return bitmap;
}

string? FindSolutionRoot()
{
    var dir = new DirectoryInfo(Directory.GetCurrentDirectory());
    for (int i = 0; i < 6 && dir != null; i++)
    {
        var sln = Path.Combine(dir.FullName, "WS.Points.slnx");
        if (File.Exists(sln)) return dir.FullName;
        dir = dir.Parent;
    }
    return null;
}

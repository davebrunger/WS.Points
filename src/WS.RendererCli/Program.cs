/*
 CLI entry point that renders a sample IDrawing to a PNG file using SkiaSharp.
 Locates the repository root, creates an `output` directory and writes the drawing to the
 filename suggested by the drawing's OutputFileName.
*/
var fileUtilities = new FileUtilities(new FileSystem());
var solutionRoot = fileUtilities.FindSolutionRoot();
var outputDir = Path.Combine(solutionRoot, "output");
Directory.CreateDirectory(outputDir);

for (int level = 0; level <= 9; level++)
{
    IDrawing drawing = new SpellLevel(level);

    var outputPath = drawing.SaveAsPng((fileName, extension) =>
    {
        var sanitizedFileName = fileUtilities.SanitizeFileName(fileName, extension);
        var outputPath = Path.Combine(outputDir, sanitizedFileName);
        return (outputPath, (Stream)new FileStream(outputPath, FileMode.Create, FileAccess.Write));
    });

    Console.WriteLine($"Saved image to {outputPath}");
}

Console.WriteLine("Done");
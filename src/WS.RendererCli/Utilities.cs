namespace WS.RendererCli;

internal static class Utilities
{
    public static string FindSolutionRoot()
    {
        var dir = new DirectoryInfo(Directory.GetCurrentDirectory());
        for (int i = 0; i < 6 && dir != null; i++)
        {
            var sln = Path.Combine(dir.FullName, "WS.Points.slnx");
            if (File.Exists(sln))
            {
                return dir.FullName;
            }
            dir = dir.Parent;
        }
        Console.Error.WriteLine("Solution root not found. Saving output to current directory.");
        return Directory.GetCurrentDirectory();
    }

    public static string SanitizeFileName(string name, string extension)
    {
        var fileName = name;
        fileName = Path.GetFileName(fileName) ?? fileName;
        if (string.IsNullOrWhiteSpace(fileName)) fileName = "image";
        var invalidChars = Path.GetInvalidFileNameChars();
        fileName = string.Concat(fileName.Split(invalidChars, StringSplitOptions.RemoveEmptyEntries));
        if (!fileName.EndsWith($".{extension}", StringComparison.OrdinalIgnoreCase))
        {
            fileName = Path.ChangeExtension(fileName, extension);
        }
        return fileName;
    }
}
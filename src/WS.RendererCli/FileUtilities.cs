using System.IO.Abstractions;

namespace WS.RendererCli;

/// <summary>
/// Provides small file-related helper utilities used by the renderer CLI.
/// </summary>
public class FileUtilities
{
    private readonly IFileSystem fileSystem;

    /// <summary>
    /// Initializes a new instance of <see cref="FileUtilities"/>.
    /// </summary>
    /// <param name="fileSystem">An abstraction over the file system for testability.</param>
    public FileUtilities(IFileSystem fileSystem)
    {
        this.fileSystem = fileSystem;
    }
    
    /// <summary>
    /// Searches up the directory tree to find the repository solution root containing
    /// <c>WS.Points.slnx</c>. The search walks up to six parent directories.
    /// </summary>
    /// <returns>
    /// The full path to the solution root if found; otherwise the current working directory.
    /// </returns>
    public string FindSolutionRoot()
    {
        var dir = new DirectoryInfo(fileSystem.Directory.GetCurrentDirectory());
        for (int i = 0; i < 6 && dir != null; i++)
        {
            var sln = fileSystem.Path.Combine(dir.FullName, "WS.Points.slnx");
            if (fileSystem.File.Exists(sln))
            {
                return dir.FullName;
            }
            dir = dir.Parent;
        }
        Console.Error.WriteLine("Solution root not found. Saving output to current directory.");
        return fileSystem.Directory.GetCurrentDirectory();
    }

    /// <summary>
    /// Sanitizes a suggested filename by stripping directory components, removing invalid
    /// filename characters and ensuring the requested extension is present.
    /// </summary>
    /// <param name="name">The suggested filename which may include path components.</param>
    /// <param name="extension">The desired file extension (without leading dot), e.g. "png".</param>
    /// <returns>A sanitized filename including the requested extension.</returns>
    public string SanitizeFileName(string name, string extension)
    {
        var fileName = name;
        fileName = fileSystem.Path.GetFileName(fileName) ?? fileName;
        if (string.IsNullOrWhiteSpace(fileName)) fileName = "image";
        var invalidChars = fileSystem.Path.GetInvalidFileNameChars();
        fileName = string.Concat(fileName.Split(invalidChars, StringSplitOptions.RemoveEmptyEntries));
        if (!fileName.EndsWith($".{extension}", StringComparison.OrdinalIgnoreCase))
        {
            fileName = fileSystem.Path.ChangeExtension(fileName, extension);
        }
        return fileName;
    }
}
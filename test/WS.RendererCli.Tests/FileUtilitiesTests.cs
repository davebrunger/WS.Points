using System.Collections.Generic;
using System.IO.Abstractions.TestingHelpers;
using WS.RendererCli;
using Xunit;

namespace WS.RendererCli.Tests;

public class FileUtilitiesTests
{
    [Fact]
    public void SanitizeFileName_StripsPathAndInvalidChars_AndEnsuresExtension()
    {
        var mock = new MockFileSystem();
        var utils = new FileUtilities(mock);

        var result = utils.SanitizeFileName("..\\some\\pa th\\my*file.PNG", "png");

        Assert.Equal("myfile.png", result.ToLowerInvariant());
    }

    [Fact]
    public void FindSolutionRoot_FindsAncestorContainingSolutionFile()
    {
        var files = new Dictionary<string, MockFileData>
        {
            { "C:\\repo\\WS.Points.slnx", new MockFileData(string.Empty) },
            { "C:\\repo\\src\\WS.RendererCli\\dummy.txt", new MockFileData("x") }
        };
        var mock = new MockFileSystem(files, "C:\\repo\\src\\WS.RendererCli");

        var utils = new FileUtilities(mock);

        var root = utils.FindSolutionRoot();

        Assert.Equal("C:\\repo", root);
    }
}

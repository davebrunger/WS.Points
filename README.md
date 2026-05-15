# WS.Points

A small .NET 10 library and sample CLI renderer for 2D geometric shapes with SkiaSharp rendering.

## Contents
- `src/WS.Points`: Core geometry and drawing abstractions (shapes, points, vectors).
- `src/WS.SkiaPoints`: Skia-based renderer adapter.
- `src/WS.RendererCli`: Minimal CLI that renders a sample drawing to `output/image.png`.
- `test/`: Unit tests.

## Prerequisites
- .NET 10 SDK: https://dotnet.microsoft.com/

## Build
```bash
dotnet build WS.Points.slnx
```

## Run the CLI renderer
```bash
dotnet run --project src/WS.RendererCli/WS.RendererCli.csproj
# Output: output/image.png
```

## Run tests
```bash
dotnet test
```

## Notes
- The CLI renderer writes a PNG to `output/image.png`.
- Project coding standards: top-level programs, XML docs (CS1591), and file-scoped namespaces.

## License

This project is released under the MIT License — see the [LICENSE](LICENSE) file for details.

Summary: you are free to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, provided that the original copyright and license
notice are included in all copies or substantial portions of the Software. The software is provided "as is", without warranty of any kind.

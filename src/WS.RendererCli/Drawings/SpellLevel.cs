namespace WS.RendererCli.Drawings;

/// <summary>
/// Helper methods for rendering spell-level artwork in the CLI renderer.
/// This file is intentionally minimal — implement drawing logic against the
/// Skia canvas in the `Draw` method when ready.
/// </summary>
internal class SpellLevel : IDrawing
{
    public int WidthPixels => 100;

    public int HeightPixels => 100;

    public Point Origin => new Point(50, 50);

    public string Name => $"{School} Level {Level}";

    public int Level { get; private init; }

    public School School { get; private init; }

    public SpellLevel(int level, School school)
    {
        if (level < 0 || level > 9) {
            throw new ArgumentOutOfRangeException(nameof(level), "Level must be between 0 and 9.");
        }
        Level = level;
        School = school;
    }

    public IEnumerable<IDrawable> Shapes
    {
        get
        {
            var triangle = Shape.RegularPolygon(0, Colours.Black, School.ToColour(), 10.0, 3)
                .Rotate(AngleInRadians.Pi / 2)
                .Translate(new Vector(0, 40.0))
                .Rotate(-AngleInRadians.Pi / 9);

            for (int i = 0; i < Level; i++)
            {
                var rotation = -2.0 * Math.PI * (i / 9.0);
                yield return triangle.Rotate(rotation);
            }

            yield return Shape.Circle(0, Colours.Black, School.ToColour(), 36.0);
            yield return Shape.Circle(0, Colours.Black, Colours.Transparent, 24.0);
            yield return Shape.Circle(0, Colours.Black, School.ToColour(), 12.0);
            yield return Shape.Circle(0, Colours.Black, Colours.Blue, 4.0).Translate(new Vector(0, 24.0));
        }
    }
}

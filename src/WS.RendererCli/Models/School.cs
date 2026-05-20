namespace WS.RendererCli.Models;

/// <summary>
/// Magical schools used by the renderer sample drawings.
/// </summary>
public enum School
{
    /// <summary>Abjuration school.</summary>
    Abjuration,
    /// <summary>Conjuration school.</summary>
    Conjuration,
    /// <summary>Divination school.</summary>
    Divination,
    /// <summary>Enchantment school.</summary>
    Enchantment,
    /// <summary>Evocation school.</summary>
    Evocation,
    /// <summary>Illusion school.</summary>
    Illusion,
    /// <summary>Necromancy school.</summary>
    Necromancy,
    /// <summary>Transmutation school.</summary>
    Transmutation
}

/// <summary>
/// Extension helpers for <see cref="School"/>.
/// </summary>
public static class SchoolExtensions
{
    /// <summary>
    /// Maps a <see cref="School"/> to a representative <see cref="Colour"/>.
    /// </summary>
    /// <param name="school">The school to map.</param>
    /// <returns>A <see cref="Colour"/> associated with the school.</returns>
    public static Colour ToColour(this School school)
    {
        return school switch
        {
            School.Abjuration => new Colour(102, 165, 230),
            School.Conjuration => new Colour(228, 129, 47),
            School.Divination => new Colour(152, 185, 205),
            School.Enchantment => new Colour(239, 143, 213),
            School.Evocation => new Colour(209, 108, 92),
            School.Illusion => new Colour(186, 154, 248),
            School.Necromancy => new Colour(181, 237, 129),
            School.Transmutation => new Colour(235, 153, 93),
            _ => throw new ArgumentOutOfRangeException(nameof(school), $"Unknown school: {school}")
        };
    }
}
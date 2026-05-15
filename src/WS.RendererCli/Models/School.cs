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
            School.Abjuration => Colours.LightBlue,
            School.Conjuration => Colours.Gold,
            School.Divination => Colours.Cyan,
            School.Enchantment => Colours.Violet,
            School.Evocation => Colours.IndianRed,
            School.Illusion => Colours.MediumPurple,
            School.Necromancy => Colours.LightGreen,
            School.Transmutation => Colours.LightSalmon,
            _ => throw new ArgumentOutOfRangeException(nameof(school), $"Unknown school: {school}")
        };
    }
}
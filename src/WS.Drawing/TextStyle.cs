namespace WS.Drawing;

[Flags]
/// <summary>
/// Text styling options that can be combined using bitwise operations.
/// </summary>
/// <remarks>
/// Use this enum to express text decorations such as bold, italic and underline
/// when rendering textual content. Because the enum is decorated with
/// <see cref="FlagsAttribute"/>, multiple values may be combined (for
/// example, <c>Bold | Italic</c>).
/// </remarks>
public enum TextStyle
{
    /// <summary>
    /// No text style applied.
    /// </summary>
    None = 0,

    /// <summary>
    /// Render text using a bold weight.
    /// </summary>
    Bold = 1,

    /// <summary>
    /// Render text using an italic style.
    /// </summary>
    Italic = 2,
}
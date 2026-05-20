namespace WS.Drawing;

public interface IDrawable
{
    /// <summary>
    /// Draws the element using the provided drawing context.
    /// </summary>
    void Draw(IDrawingContext context);
}
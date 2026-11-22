public class EPosition
{
    // x: Horizontal coordinate in the caller's coordinate space.
    public int x;
    // y: Vertical coordinate in the caller's coordinate space.
    public int y;
    public int anchor;
    public sbyte follow;
    public sbyte count;
    public sbyte dir = 1;
    public short index = -1;
    /// <summary>Initialize e position state and its supplied dependencies.</summary>
    /// <param name="x">Horizontal coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="y">Vertical coordinate in the caller&#x27;s coordinate space.</param>
    public EPosition(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    /// <summary>Initialize e position state and its supplied dependencies.</summary>
    /// <param name="x">Horizontal coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="y">Vertical coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="fol">Fol.</param>
    public EPosition(int x, int y, int fol)
    {
        this.x = x;
        this.y = y;
        follow = (sbyte)fol;
    }

    /// <summary>Initialize e position state and its supplied dependencies.</summary>
    public EPosition()
    {
    }
}

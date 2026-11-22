public class PartFrame
{
    public short idSmallImg;
    // dx: Horizontal difference or offset.
    public short dx;
    // dy: Vertical difference or offset.
    public short dy;
    public sbyte flip;
    public sbyte onTop;
    /// <summary>Initialize part frame state and its supplied dependencies.</summary>
    /// <param name="dx">Horizontal difference or offset.</param>
    /// <param name="dy">Vertical difference or offset.</param>
    /// <param name="idSmall">Identifier small.</param>
    public PartFrame(int dx, int dy, int idSmall)
    {
        idSmallImg = (short)idSmall;
        this.dx = (short)dx;
        this.dy = (short)dy;
    }
}

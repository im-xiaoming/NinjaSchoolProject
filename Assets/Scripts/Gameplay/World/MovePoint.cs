public class MovePoint
{
    public int xEnd;
    public int yEnd;
    public int dir;
    public int cvx;
    public int cvy;
    public int status;
    /// <summary>Initialize move point state and its supplied dependencies.</summary>
    /// <param name="xEnd">X end.</param>
    /// <param name="yEnd">Y end.</param>
    /// <param name="act">Act.</param>
    /// <param name="dir">Dir.</param>
    public MovePoint(int xEnd, int yEnd, int act, int dir)
    {
        this.xEnd = xEnd;
        this.yEnd = yEnd;
        this.dir = dir;
        status = act;
    }

    /// <summary>Initialize move point state and its supplied dependencies.</summary>
    /// <param name="xEnd">X end.</param>
    /// <param name="yEnd">Y end.</param>
    public MovePoint(int xEnd, int yEnd)
    {
        this.xEnd = xEnd;
        this.yEnd = yEnd;
    }
}

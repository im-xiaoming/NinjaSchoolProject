public class Waypoint
{
    public short minX;
    public short minY;
    public short maxX;
    public short maxY;
    /// <summary>Initialize waypoint state and its supplied dependencies.</summary>
    /// <param name="minX">Min x.</param>
    /// <param name="minY">Min y.</param>
    /// <param name="maxX">Max x.</param>
    /// <param name="maxY">Max y.</param>
    public Waypoint(short minX, short minY, short maxX, short maxY)
    {
        this.minX = minX;
        this.minY = minY;
        this.maxX = maxX;
        this.maxY = maxY;
    }
}

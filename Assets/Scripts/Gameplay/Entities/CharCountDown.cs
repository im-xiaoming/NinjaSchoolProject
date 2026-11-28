public class CharCountDown
{
    private long maxTime;
    // w: Width or horizontal span.
    private int w;
    public bool WantDestroy;
    public string tile = string.Empty;
    private long timesv;
    private int timeper;
    private int per;
    /// <summary>Initialize character count down state and its supplied dependencies.</summary>
    /// <param name="time">Time.</param>
    /// <param name="tile">Tile.</param>
    public CharCountDown(long time, string tile)
    {
        this.tile = tile;
        maxTime = time;
        w = 60;
        timeper = (int)(maxTime - mSystem.currentTimeMillis());
    }

    /// <summary>Draw this object&#x27;s current state through the legacy graphics wrapper.</summary>
    /// <param name="g">Legacy drawing context.</param>
    /// <param name="x">Horizontal coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="y">Vertical coordinate in the caller&#x27;s coordinate space.</param>
    /// <remarks>Locals: num: Numeric intermediate used by this operation; the legacy name is retained; num2: Numeric intermediate used by this operation; the legacy name is retained.</remarks>
    public void paint(mGraphics g, int x, int y)
    {
        int num = 0;
        long num2 = maxTime - mSystem.currentTimeMillis();
        num = (int)(60 - num2 * w / timeper);
        g.drawRegion(GameCanvas.imghealth[1], 0, 9, 62, 9, 0, x, y, 0);
        g.drawRegion(GameCanvas.imghealth[0], 0, 7, num, 7, 0, x, y + 1, 0);
        mFont.tahoma_7.drawString(g, tile, x + w / 2 + 1, y - 10 + 1 - 3, 2);
        mFont.tahoma_7_white.drawString(g, tile, x + w / 2, y - 10 - 3, 2);
        mFont.tahoma_7b_white.drawString(g, per + "%", x + w / 2, y, 2);
    }

    /// <summary>Advance this object&#x27;s state for the current client logic tick.</summary>
    /// <remarks>Locals: num: Numeric intermediate used by this operation; the legacy name is retained.</remarks>
    public void update()
    {
        long num = maxTime - mSystem.currentTimeMillis();
        per = (int)(100 - num * 100 / timeper);
        if (maxTime - mSystem.currentTimeMillis() <= 0)
        {
            WantDestroy = true;
        }
    }

}

public class Lanterns
{
    // x: Horizontal coordinate in the caller's coordinate space.
    public int x;
    // y: Vertical coordinate in the caller's coordinate space.
    public int y;
    // dy: Vertical difference or offset.
    public int dy = 3;
    public int yStart;
    public bool isEnd;
    /// <summary>Initialize lanterns state and its supplied dependencies.</summary>
    /// <param name="x">Horizontal coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="y">Vertical coordinate in the caller&#x27;s coordinate space.</param>
    public Lanterns(int x, int y)
    {
        this.x = x;
        this.y = (yStart = y);
        isEnd = false;
    }

    /// <summary>Advance this object&#x27;s state for the current client logic tick.</summary>
    public void update()
    {
        y -= dy;
        if (yStart - y > 150)
        {
            isEnd = true;
        }
    }

    /// <summary>Draw this object&#x27;s current state through the legacy graphics wrapper.</summary>
    /// <param name="g">Legacy drawing context.</param>
    public void paint(mGraphics g)
    {
        using (Interp.Offset(g, this))
            PaintAtRenderPosition(g);
    }

    /// <summary>Draw at the interpolated position without changing logical gameplay coordinates.</summary>
    /// <param name="g">Legacy drawing context.</param>
    private void PaintAtRenderPosition(mGraphics g)
    {
        if (GameCanvas.gameTick % 10 < 8)
        {
            SmallImage.drawSmallImage(g, 1292, x, y, 0, mGraphics.VCENTER | mGraphics.HCENTER);
        }
        else
        {
            SmallImage.drawSmallImage(g, 1291, x, y, 0, mGraphics.VCENTER | mGraphics.HCENTER);
        }
    }
}

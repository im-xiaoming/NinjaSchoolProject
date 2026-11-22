public class BuNhin
{
    // x: Horizontal coordinate in the caller's coordinate space.
    public short x;
    // y: Vertical coordinate in the caller's coordinate space.
    public short y;
    public string name;
    public bool isInjure;
    /// <summary>Initialize bu nhin state and its supplied dependencies.</summary>
    /// <param name="name">Name.</param>
    /// <param name="x">Horizontal coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="y">Vertical coordinate in the caller&#x27;s coordinate space.</param>
    public BuNhin(string name, short x, short y)
    {
        this.x = x;
        this.y = y;
        this.name = name;
    }

    /// <summary>Handle injure.</summary>
    public void doInjure()
    {
        isInjure = true;
    }

    /// <summary>Check whether paint applies to the supplied state.</summary>
    public bool isPaint()
    {
        if (x < GameScr.cmx)
        {
            return false;
        }

        if (x > GameScr.cmx + GameScr.gW)
        {
            return false;
        }

        if (y < GameScr.cmy)
        {
            return false;
        }

        if (y > GameScr.cmy + GameScr.gH + 30)
        {
            return false;
        }

        return true;
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
        if (isPaint())
        {
            mFont.tahoma_7_yellow.drawString(g, name, x, y - 32, 2, mFont.tahoma_7_grey);
            SmallImage.drawSmallImage(g, 1180, x, y, 0, mGraphics.BOTTOM | mGraphics.HCENTER);
            if (isInjure)
            {
                SmallImage.drawSmallImage(g, 288, x, y, 0, mGraphics.BOTTOM | mGraphics.HCENTER);
                isInjure = false;
            }
        }
    }
}

using System.Threading;

public class ItemMap
{
    // x: Horizontal coordinate in the caller's coordinate space.
    public int x;
    // y: Vertical coordinate in the caller's coordinate space.
    public int y;
    public int xEnd;
    public int yEnd;
    // f: Current field/value.
    public int f;
    // vx: Horizontal velocity.
    public int vx;
    // vy: Vertical velocity.
    public int vy;
    public int itemMapID;
    public int IdCharMove;
    public ItemTemplate template;
    public sbyte status;
    public int sizeItem = 24;
    public MyImage imgCaptcha;
    private int frame;
    // Da: Da.
    public static int Da;
    public long TimeStart;
    // b: Buffer or intermediate output value.
    public static object b;
    public static bool isb;
    public bool isWait;
    /// <summary>Initialize item map state and its supplied dependencies.</summary>
    /// <param name="itemMapID">Item map identifier.</param>
    /// <param name="itemTemplateID">Item template identifier.</param>
    /// <param name="x">Horizontal coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="y">Vertical coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="xEnd">X end.</param>
    /// <param name="yEnd">Y end.</param>
    public ItemMap(short itemMapID, short itemTemplateID, int x, int y, int xEnd, int yEnd)
    {
        this.itemMapID = itemMapID;
        template = ItemTemplates.get(itemTemplateID);
        this.x = xEnd;
        this.y = y;
        this.xEnd = xEnd;
        this.yEnd = yEnd;
        vx = xEnd - x >> 2;
        vy = 5;
        TimeStart = mSystem.currentTimeMillis();
    }

    /// <summary>Initialize item map state and its supplied dependencies.</summary>
    /// <param name="itemMapID">Item map identifier.</param>
    /// <param name="itemTemplateID">Item template identifier.</param>
    /// <param name="x">Horizontal coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="y">Vertical coordinate in the caller&#x27;s coordinate space.</param>
    public ItemMap(short itemMapID, short itemTemplateID, int x, int y)
    {
        this.itemMapID = itemMapID;
        template = ItemTemplates.get(itemTemplateID);
        xEnd = x;
        this.x = x;
        yEnd = y;
        this.y = y;
        status = 1;
    }

    /// <summary>Set point.</summary>
    /// <param name="xEnd">X end.</param>
    /// <param name="yEnd">Y end.</param>
    public void setPoint(int xEnd, int yEnd)
    {
        this.xEnd = xEnd;
        this.yEnd = yEnd;
        vx = xEnd - x >> 2;
        vy = yEnd - y >> 2;
        status = 2;
    }

    /// <summary>Advance this object&#x27;s state for the current client logic tick.</summary>
    public void update()
    {
        if (status == 2 && x == xEnd && y == yEnd)
        {
            GameScr.vItemMap.removeElement(this);
            if (Char.getMyChar().itemFocus != null && Char.getMyChar().itemFocus.Equals(this))
            {
                Char.getMyChar().itemFocus = null;
            }

            return;
        }

        if (status > 0)
        {
            if (vx == 0)
            {
                x = xEnd;
            }

            if (vy == 0)
            {
                y = yEnd;
            }

            if (x != xEnd)
            {
                x += vx;
                if ((vx > 0 && x > xEnd) || (vx < 0 && x < xEnd))
                {
                    x = xEnd;
                }
            }

            if (y != yEnd)
            {
                y += vy;
                if ((vy > 0 && y > yEnd) || (vy < 0 && y < yEnd))
                {
                    y = yEnd;
                }
            }
        }
        else
        {
            status -= 4;
            if (status < -12)
            {
                y -= 12;
                status = 1;
            }
        }

        if (GameCanvas.gameTick % 5 == 0)
        {
            frame++;
            if (frame > 2)
            {
                frame = 0;
            }
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
    /// <remarks>Locals: num: Numeric intermediate used by this operation; the legacy name is retained; num2: Numeric intermediate used by this operation; the legacy name is retained.</remarks>
    private void PaintAtRenderPosition(mGraphics g)
    {
        if (imgCaptcha != null && imgCaptcha.img != null)
        {
            int num = 0;
            if (status <= 0)
            {
                num = status;
            }

            g.drawImage(imgCaptcha.img, x, y + num, mGraphics.BOTTOM | mGraphics.HCENTER);
            return;
        }

        int num2 = 0;
        if (status <= 0)
        {
            num2 = status;
        }

        SmallImage.drawSmallImage(g, template.iconID, x, y + num2, 0, mGraphics.BOTTOM | mGraphics.HCENTER);
        if (Char.getMyChar().itemFocus != null && Char.getMyChar().itemFocus.Equals(this) && status != 2)
        {
            SmallImage.drawSmallImage(g, 988, x, y - 20, 0, mGraphics.VCENTER | mGraphics.HCENTER);
        }
    }

    /// <summary>Check whether no my item applies to the supplied state.</summary>
    public bool isNoMyItem()
    {
        return mSystem.currentTimeMillis() - TimeStart >= 15000;
    }

    /// <summary>Wait on the shared ground-item monitor for up to the supplied milliseconds.</summary>
    /// <param name="milisSeconds">Milis seconds (seconds).</param>
    public static void s(int milisSeconds = 1000)
    {
        isb = true;
        lock (b)
        {
            Monitor.Wait(b, milisSeconds);
        }
    }

    /// <summary>Wake threads waiting on the shared ground-item monitor.</summary>
    public static void d()
    {
        if (isb)
        {
            lock (b)
            {
                Monitor.PulseAll(b);
            }

            isb = false;
        }
    }

    /// <summary>Initialize item map state and its supplied dependencies.</summary>
    static ItemMap()
    {
        b = new object ();
    }
}

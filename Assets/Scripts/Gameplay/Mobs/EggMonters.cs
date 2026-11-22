public class EggMonters
{
    // x: Horizontal coordinate in the caller's coordinate space.
    public int x;
    // y: Vertical coordinate in the caller's coordinate space.
    public int y;
    public int frame;
    public sbyte status;
    public const sbyte EGG_NOTACTIVE = 2;
    public const sbyte EGG_FLY = 0;
    public const sbyte EGG_BREAK = 1;
    // vy: Vertical velocity.
    public int vy;
    public static Mob ownerEgg;
    /// <summary>Initialize egg monters state and its supplied dependencies.</summary>
    /// <param name="x">Horizontal coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="y">Vertical coordinate in the caller&#x27;s coordinate space.</param>
    /// <remarks>Locals: i: Loop/collection index; mob: Monster being inspected or targeted.</remarks>
    public EggMonters(int x, int y)
    {
        status = 0;
        this.x = x;
        this.y = y;
        for (int i = 0; i < GameScr.vMob.size(); i++)
        {
            Mob mob = (Mob)GameScr.vMob.elementAt(i);
            if (mob.templateId == 202)
            {
                ownerEgg = mob;
            }
        }
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

        if (ownerEgg != null && ownerEgg.status == 8)
        {
            return false;
        }

        return true;
    }

    /// <summary>Advance this object&#x27;s state for the current client logic tick.</summary>
    public void update()
    {
        if (!isPaint())
        {
            return;
        }

        if (status == 0)
        {
            vy++;
            y += vy;
            frame++;
            if (frame > 3)
            {
                frame = 0;
            }

            if ((TileMap.tileTypeAtPixel(x, y) & TileMap.T_TOP) == TileMap.T_TOP)
            {
                status = 1;
                vy = 0;
            }
        }
        else if (status == 1)
        {
            frame++;
            if (frame > 6)
            {
                frame = 6;
                ownerEgg.status = 5;
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
    private void PaintAtRenderPosition(mGraphics g)
    {
        if (isPaint())
        {
            g.drawRegion(GameScr.imgEggMonters, 0, frame * 32, 32, 32, 0, x, y, mGraphics.HCENTER | mGraphics.BOTTOM);
        }
    }
}

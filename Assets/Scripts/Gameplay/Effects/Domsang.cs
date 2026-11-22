public class Domsang
{
    // xS: X s.
    public int xS;
    // yS: Y s.
    public int yS;
    public int frame;
    public int typeEff;
    private int dem;
    /// <summary>Initialize domsang state and its supplied dependencies.</summary>
    /// <param name="x">Horizontal coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="y">Vertical coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="type">Type.</param>
    public Domsang(int x, int y, int type)
    {
        xS = x;
        yS = y;
        typeEff = type;
    }

    /// <summary>Advance this object&#x27;s state for the current client logic tick.</summary>
    public void update()
    {
        if (typeEff == 1)
        {
            dem++;
            if (dem % 2 == 0)
            {
                frame++;
            }
        }
        else if (typeEff == 0)
        {
            frame++;
        }
        else if (typeEff == 2 || typeEff == 3 || typeEff == 4 || typeEff == 5)
        {
            dem++;
            if (dem % 2 == 0)
            {
                frame++;
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
        if (typeEff == 0)
        {
            g.drawRegion(GameScr.imgMatcho, 0, frame * GameScr.imgMatcho.getHeight() / 6, GameScr.imgMatcho.getWidth(), GameScr.imgMatcho.getHeight() / 6, 0, xS, yS, 0);
        }
        else if (typeEff == 1)
        {
            g.drawRegion(GameScr.imgFiremoto, 0, frame * GameScr.imgFiremoto.getHeight() / 6, GameScr.imgFiremoto.getWidth(), GameScr.imgFiremoto.getHeight() / 6, 0, xS, yS, mGraphics.BOTTOM | mGraphics.HCENTER);
        }
        else if (typeEff == 2)
        {
            g.drawRegion(GameScr.imgsmokeFollow, 0, frame * GameScr.imgsmokeFollow.getHeight() / 4, GameScr.imgsmokeFollow.getWidth(), GameScr.imgsmokeFollow.getHeight() / 4, 0, xS + 20, yS + 4, mGraphics.TOP | mGraphics.LEFT);
        }
        else if (typeEff == 3)
        {
            g.drawRegion(GameScr.imgEffMob1, 0, frame * GameScr.imgEffMob1.getHeight() / 4, GameScr.imgEffMob1.getWidth(), GameScr.imgEffMob1.getHeight() / 4, 0, xS + 20, yS + 4, mGraphics.TOP | mGraphics.LEFT);
        }
        else if (typeEff == 4)
        {
            g.drawRegion(GameScr.imgEffMob2, 0, frame * GameScr.imgEffMob2.getHeight() / 4, GameScr.imgEffMob2.getWidth(), GameScr.imgEffMob1.getHeight() / 4, 0, xS + 20, yS + 4, mGraphics.TOP | mGraphics.LEFT);
        }
        else if (typeEff == 5)
        {
            g.drawRegion(GameScr.imgEffMob3, 0, frame * GameScr.imgEffMob3.getHeight() / 4, GameScr.imgEffMob3.getWidth(), GameScr.imgEffMob3.getHeight() / 4, 0, xS + 20, yS + 4, mGraphics.TOP | mGraphics.LEFT);
        }
    }
}

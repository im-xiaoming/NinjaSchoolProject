public class ServerEffect : Effect2
{
    // eff: Effect.
    public EffectCharPaint eff;
    // i0: I0.
    private int i0;
    // x: Horizontal coordinate in the caller's coordinate space.
    private int x;
    // y: Vertical coordinate in the caller's coordinate space.
    private int y;
    private int dir = 1;
    // c: Client character being inspected or controlled.
    private Char c;
    private short loopCount;
    private long endTime;
    /// <summary>Add server effect.</summary>
    /// <param name="id">Identifier.</param>
    /// <param name="cx">Character horizontal position in map pixels.</param>
    /// <param name="cy">Character vertical position in map pixels.</param>
    /// <param name="loopCount">Loop count.</param>
    public static void addServerEffect(int id, int cx, int cy, int loopCount)
    {
        ServerEffect serverEffect = new ServerEffect();
        serverEffect.eff = GameScr.efs[id];
        serverEffect.x = cx;
        serverEffect.y = cy;
        serverEffect.loopCount = (short)loopCount;
        Effect2.vEffect2.addElement(serverEffect);
    }

    /// <summary>Add server effect.</summary>
    /// <param name="id">Identifier.</param>
    /// <param name="cx">Character horizontal position in map pixels.</param>
    /// <param name="cy">Character vertical position in map pixels.</param>
    /// <param name="loopCount">Loop count.</param>
    /// <param name="dir">Dir.</param>
    public static void addServerEffect(int id, int cx, int cy, int loopCount, sbyte dir)
    {
        ServerEffect serverEffect = new ServerEffect();
        serverEffect.eff = GameScr.efs[id];
        serverEffect.x = cx;
        serverEffect.y = cy;
        serverEffect.loopCount = (short)loopCount;
        serverEffect.dir = dir;
        Effect2.vEffect2.addElement(serverEffect);
    }

    /// <summary>Add server effect.</summary>
    /// <param name="id">Identifier.</param>
    /// <param name="c">Client character being inspected or controlled.</param>
    /// <param name="loopCount">Loop count.</param>
    public static void addServerEffect(int id, Char c, int loopCount)
    {
        ServerEffect serverEffect = new ServerEffect();
        serverEffect.eff = GameScr.efs[id];
        serverEffect.c = c;
        serverEffect.loopCount = (short)loopCount;
        Effect2.vEffect2.addElement(serverEffect);
    }

    /// <summary>Add server effect with time.</summary>
    /// <param name="id">Identifier.</param>
    /// <param name="cx">Character horizontal position in map pixels.</param>
    /// <param name="cy">Character vertical position in map pixels.</param>
    /// <param name="timeLengthInSecond">Time length in second.</param>
    public static void addServerEffectWithTime(int id, int cx, int cy, long timeLengthInSecond)
    {
        ServerEffect serverEffect = new ServerEffect();
        serverEffect.eff = GameScr.efs[id];
        serverEffect.x = cx;
        serverEffect.y = cy;
        serverEffect.endTime = mSystem.getCurrentTimeMillis() + timeLengthInSecond * 1000;
        Effect2.vEffect2.addElement(serverEffect);
    }

    /// <summary>Add server effect with time.</summary>
    /// <param name="id">Identifier.</param>
    /// <param name="c">Client character being inspected or controlled.</param>
    /// <param name="timeLengthInSecond">Time length in second.</param>
    public static void addServerEffectWithTime(int id, Char c, long timeLengthInSecond)
    {
        ServerEffect serverEffect = new ServerEffect();
        serverEffect.eff = GameScr.efs[id];
        serverEffect.c = c;
        serverEffect.endTime = mSystem.getCurrentTimeMillis() + timeLengthInSecond * 1000;
        Effect2.vEffect2.addElement(serverEffect);
    }

    /// <summary>Draw this object&#x27;s current state through the legacy graphics wrapper.</summary>
    /// <param name="g">Legacy drawing context.</param>
    public override void paint(mGraphics g)
    {
        using (Interp.Offset(g, c))
            PaintAtRenderPosition(g);
    }

    /// <summary>Draw at the interpolated position without changing logical gameplay coordinates.</summary>
    /// <param name="g">Legacy drawing context.</param>
    /// <remarks>Locals: num: Entry selected from the source collection; num2: Entry selected from the source collection.</remarks>
    private void PaintAtRenderPosition(mGraphics g)
    {
        if (c != null)
        {
            x = c.cx;
            y = c.cy;
        }

        int num = x + eff.arrEfInfo[i0].dx * dir;
        int num2 = y + eff.arrEfInfo[i0].dy;
        if (GameCanvas.isPaint(num, num2))
        {
            SmallImage.drawSmallImage(g, eff.arrEfInfo[i0].idImg, num, num2, (dir != 1) ? 2 : 0, mGraphics.VCENTER | mGraphics.HCENTER);
        }
    }

    /// <summary>Advance this object&#x27;s state for the current client logic tick.</summary>
    public override void update()
    {
        if (endTime != 0)
        {
            i0++;
            if (i0 >= eff.arrEfInfo.Length)
            {
                i0 = 0;
            }

            if (mSystem.getCurrentTimeMillis() - endTime > 0)
            {
                if (eff.idEf == 120)
                {
                    GameCanvas.isBallEffect = false;
                }

                Effect2.vEffect2.removeElement(this);
            }
        }
        else
        {
            i0++;
            if (i0 >= eff.arrEfInfo.Length)
            {
                loopCount--;
                if (loopCount <= 0)
                {
                    if (eff.idEf == 120)
                    {
                        GameCanvas.isBallEffect = false;
                    }

                    Effect2.vEffect2.removeElement(this);
                }
                else
                {
                    i0 = 0;
                }
            }
        }

        if (GameCanvas.gameTick % 11 == 0 && c != null && c != Char.getMyChar() && !GameScr.vCharInMap.contains(c))
        {
            Effect2.vEffect2.removeElement(this);
        }
    }
}

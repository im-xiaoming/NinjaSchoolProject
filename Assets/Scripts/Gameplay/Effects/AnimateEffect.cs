public class AnimateEffect : Effect2
{
    private readonly System.Collections.Generic.Dictionary<Position, UnityEngine.Vector2> paintPositions = new System.Collections.Generic.Dictionary<Position, UnityEngine.Vector2>();
    /// <summary>Capture the previous logical state for render interpolation.</summary>
    /// <remarks>Locals: i: Loop/collection index; p: Entry selected from the source collection.</remarks>
    public void CapturePaintState()
    {
        paintPositions.Clear();
        for (int i = 0; i < list.size(); i++)
        {
            var p = (Position)list.elementAt(i);
            paintPositions[p] = new UnityEngine.Vector2(p.x / 10f, p.y / 10f);
        }
    }

    /// <summary>Compute particle offset.</summary>
    /// <param name="p">Current player/point or request payload.</param>
    private UnityEngine.Vector2 ParticleOffset(Position p)
    {
        UnityEngine.Vector2 previous;
        var current = new UnityEngine.Vector2(p.x / 10f, p.y / 10f);
        if (!NsoGameControl.SmoothEffects || !paintPositions.TryGetValue(p, out previous) || (current - previous).sqrMagnitude > 90f * 90f)
            return UnityEngine.Vector2.zero;
        return UnityEngine.Vector2.Lerp(previous, current, Interp.TickFraction) - new UnityEngine.Vector2(p.x / 10, p.y / 10);
    }

    // img: Image.
    private static FrameImage img;
    private static FrameImage imgSnow;
    private static Image imgTuyet;
    private static Image imgCoBay;
    public const sbyte RAIN = 0;
    public const sbyte FALLING_LEAVES = 1;
    public const sbyte SNOW = 3;
    private sbyte type;
    private int number;
    private MyVector list = new MyVector();
    private bool isStop;
    /// <summary>Initialize animate effect state and its supplied dependencies.</summary>
    /// <param name="type">Type.</param>
    /// <param name="isStart">Is start.</param>
    /// <param name="number">Number.</param>
    /// <param name="timeLimit">Retained for call compatibility; not read by this implementation.</param>
    /// <remarks>Locals: i: Loop/collection index.</remarks>
    public AnimateEffect(sbyte type, bool isStart, int number, int timeLimit)
    {
        this.type = type;
        this.number = number;
        loadAni();
        switch (type)
        {
            case 1:
                number = 10;
                if (img == null)
                {
                    img = new FrameImage(imgCoBay, 16, 10);
                }

                break;
            case 3:
                if (imgSnow == null)
                {
                    imgSnow = new FrameImage(imgTuyet, 5, 5);
                }

                break;
        }

        isStart = false;
        for (int i = 0; i < number; i++)
        {
            Position position = null;
            position = new Position((GameScr.cmx - 20 + Res.rnd((GameCanvas.w + 40) / 5) * 5) * 10, (GameScr.cmy - 20 + Res.rnd(GameCanvas.h / 5) * 5) * 10);
            if (type == 3)
            {
                position.h = Res.rnd(3);
            }
            else
            {
                position.h = Res.rnd(4);
            }

            position.limitY = 16 + Res.rnd(3) * 4;
            position.v = Res.random(-1, 1);
            position.color = Res.rnd(position.limitY);
            list.addElement(position);
        }
    }

    /// <summary>Load ani.</summary>
    public static void loadAni()
    {
        if (imgTuyet == null)
        {
            imgTuyet = GameCanvas.loadImage("/u/tuyet");
        }

        if (imgCoBay == null)
        {
            imgCoBay = GameCanvas.loadImage("/u/cobay");
        }
    }

    /// <summary>Stop animate effect.</summary>
    public void stop()
    {
        isStop = true;
    }

    /// <summary>Draw this object&#x27;s current state through the legacy graphics wrapper.</summary>
    /// <param name="g">Legacy drawing context.</param>
    /// <remarks>Locals: i: Loop/collection index; num: Numeric intermediate used by this operation; the legacy name is retained.</remarks>
    public override void paint(mGraphics g)
    {
        g.translate(-g.getTranslateX(), -g.getTranslateY());
        switch (type)
        {
            case 1:
                paintFallingLeaves(g);
                break;
            case 3:
            {
                for (int i = 0; i < number; i++)
                {
                    Position position = (Position)list.elementAt(i);
                    if (position.h > 0)
                    {
                        GameScr.gI();
                        int num = Scroll.gI().cmx * ((2 - position.h) * 20) / 120 - GameScr.cmx;
                        var offset = ParticleOffset(position);
                        using (g.Offset(offset.x, offset.y))
                            imgSnow.drawFrame(position.h, num + position.x / 10, position.y / 10 - GameScr.cmy, 2, 0, g);
                    }
                }

                break;
            }
        }
    }

    /// <summary>Advance this object&#x27;s state for the current client logic tick.</summary>
    public override void update()
    {
        switch (type)
        {
            case 1:
                updateFallingLeaves();
                break;
            case 3:
                updateSnow();
                break;
        }
    }

    /// <summary>Update falling leaves.</summary>
    /// <remarks>Locals: i: Loop/collection index.</remarks>
    private void updateFallingLeaves()
    {
        for (int i = 0; i < number; i++)
        {
            Position position = (Position)list.elementAt(i);
            position.y += 10;
            position.x += position.v * 10;
            position.color++;
            if (position.color >= position.limitY)
            {
                position.color = 0;
            }

            if (position.y / 10 > GameScr.cmy + GameCanvas.h - (3 - position.h) * 40 || position.x / 10 < GameScr.cmx - GameCanvas.hw || position.x / 10 > GameScr.cmx + GameCanvas.w + GameCanvas.hw)
            {
                if (position.y / 10 > 24)
                {
                    setLeaveFall(position.x / 10 + GameScr.cmx, position.y / 10 + GameScr.cmy);
                }

                rndPos(position);
            }
        }
    }

    /// <summary>Draw falling leaves in the current graphics context.</summary>
    /// <param name="g">Legacy drawing context.</param>
    /// <remarks>Locals: i: Loop/collection index.</remarks>
    private void paintFallingLeaves(mGraphics g)
    {
        for (int i = 0; i < number; i++)
        {
            Position position = (Position)list.elementAt(i);
            var offset = ParticleOffset(position);
            using (g.Offset(offset.x, offset.y))
                img.drawFrame(position.color / (position.limitY / 4), position.x / 10 - GameScr.cmx, position.y / 10 - GameScr.cmy, 0, 3, g);
        }
    }

    /// <summary>Set leave fall.</summary>
    /// <param name="x0">X0.</param>
    /// <param name="y0">Y0.</param>
    /// <remarks>Locals: po: Po.</remarks>
    private void setLeaveFall(int x0, int y0)
    {
        Position po = new Position(x0, y0);
        po.limitY = 200;
        po.g = Res.rnd(4);
        po.layer = new Layer();
        ActionPaint acPaint = delegate (mGraphics g, int x, int y)
        {
            img.drawFrame(po.g, x0, y0, 0, 3, g);
        };
        ActionUpdate acUpdate = delegate
        {
            po.limitY--;
            if (po.limitY > 0)
            {
            }
        };
        po.layer.acPaint = acPaint;
        po.layer.acUpdate = acUpdate;
    }

    /// <summary>Perform rnd position.</summary>
    /// <param name="pos">Position.</param>
    private void rndPos(Position pos)
    {
        if (isStop)
        {
            list.removeElement(pos);
            number = list.size();
            return;
        }

        pos.x = (GameScr.cmx - 20 + Res.rnd((GameCanvas.w + 40) / 5) * 5) * 10;
        pos.y = (GameScr.cmy - GameCanvas.hh + Res.rnd(GameCanvas.h / 5) * 5) * 10;
        if (type == 3)
        {
            pos.h = Res.rnd(3);
        }
        else
        {
            pos.h = Res.rnd(4);
        }
    }

    /// <summary>Set limit x.</summary>
    /// <param name="pos">Position.</param>
    /// <remarks>Locals: num: Numeric intermediate used by this operation; the legacy name is retained.</remarks>
    private void setLimitX(Position pos)
    {
        int num = GameScr.cmx * ((2 - pos.h) * 20) / 120;
        if (pos.x / 10 + num < GameScr.cmx - 10)
        {
            pos.x += (GameCanvas.w + 20) * 10;
        }

        if (pos.x / 10 + num > GameScr.cmx + GameCanvas.w + 10)
        {
            pos.x -= (GameCanvas.w + 20) * 10;
        }
    }

    /// <summary>Update snow.</summary>
    /// <remarks>Locals: i: Loop/collection index.</remarks>
    private void updateSnow()
    {
        for (int i = 0; i < number; i++)
        {
            Position position = (Position)list.elementAt(i);
            position.y += (position.h + 1) * 5;
            position.x += (position.h + 1) * 2;
            if (position.y / 10 > GameScr.cmy + GameCanvas.h - (3 - position.h) * 30)
            {
                rndPos(position);
            }

            setLimitX(position);
        }
    }
}

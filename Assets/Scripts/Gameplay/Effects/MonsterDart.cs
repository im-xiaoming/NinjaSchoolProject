public class MonsterDart : Effect2
{
    public int angle;
    // vx: Horizontal velocity.
    public int vx;
    // vy: Vertical velocity.
    public int vy;
    // va: Va.
    public short va = 12288;
    // x: Horizontal coordinate in the caller's coordinate space.
    public int x;
    // y: Vertical coordinate in the caller's coordinate space.
    public int y;
    // z: Z.
    public int z;
    private int life;
    public bool isSpeedUp;
    public int dame;
    public int dameMp;
    public int typeAtt;
    // c: Client character being inspected or controlled.
    public Char c;
    // b: Buffer or intermediate output value.
    public BuNhin b;
    public bool isBoss;
    public int idBoss;
    public short levelBoss;
    private int countangle = 1;
    public static int level;
    public static MyVector vDomsang = new MyVector();
    public bool isEnd;
    public static int regMirro = 0;
    private int frame;
    private int index;
    public static sbyte[] FRAME = new sbyte[25]
    {
        0,
        1,
        2,
        1,
        0,
        1,
        2,
        1,
        0,
        1,
        2,
        1,
        0,
        1,
        2,
        1,
        0,
        1,
        2,
        1,
        0,
        1,
        2,
        1,
        0
    };
    public static int[] TRANSFORM = new int[16]
    {
        0,
        0,
        0,
        7,
        6,
        6,
        6,
        2,
        2,
        3,
        3,
        4,
        5,
        5,
        5,
        1
    };
    public static int[] ARROWINDEX = new int[18]
    {
        0,
        15,
        37,
        52,
        75,
        105,
        127,
        142,
        165,
        195,
        217,
        232,
        255,
        285,
        307,
        322,
        345,
        370
    };
    /// <summary>Initialize monster dart state and its supplied dependencies.</summary>
    /// <param name="x">Horizontal coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="y">Vertical coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="isBoss">Is boss.</param>
    /// <param name="levelBoss">Level boss.</param>
    /// <param name="idBoss">Identifier boss.</param>
    /// <param name="dame">Dame.</param>
    /// <param name="dameMp">Dame mana points.</param>
    /// <param name="c">Client character being inspected or controlled.</param>
    public MonsterDart(int x, int y, bool isBoss, short levelBoss, int idBoss, int dame, int dameMp, Char c)
    {
        typeAtt = 0;
        this.x = x;
        this.y = y;
        this.isBoss = isBoss;
        this.levelBoss = levelBoss;
        this.idBoss = idBoss;
        this.dame = dame;
        this.dameMp = dameMp;
        this.c = c;
        if (isBoss)
        {
            setAngle(countangle * 90);
            countangle++;
            if (countangle > 3)
            {
                countangle = 1;
            }
        }
        else if (x > c.cx)
        {
            setAngle(240);
        }
        else
        {
            setAngle(300);
        }
    }

    /// <summary>Initialize monster dart state and its supplied dependencies.</summary>
    /// <param name="x">Horizontal coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="y">Vertical coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="b">Buffer or intermediate output value.</param>
    public MonsterDart(int x, int y, BuNhin b)
    {
        typeAtt = 1;
        this.x = x;
        this.y = y;
        this.b = b;
        if (x > b.x)
        {
            setAngle(240);
        }
        else
        {
            setAngle(300);
        }
    }

    /// <summary>Set angle.</summary>
    /// <param name="angle">Angle.</param>
    public void setAngle(int angle)
    {
        this.angle = angle;
        vx = va * Res.cos(angle) >> 10;
        vy = va * Res.sin(angle) >> 10;
    }

    /// <summary>Add monster dart.</summary>
    /// <param name="x">Horizontal coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="y">Vertical coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="isBoss">Is boss.</param>
    /// <param name="levelBoss">Level boss.</param>
    /// <param name="idBoss">Identifier boss.</param>
    /// <param name="dame">Dame.</param>
    /// <param name="dameMp">Dame mana points.</param>
    /// <param name="c">Client character being inspected or controlled.</param>
    public static void addMonsterDart(int x, int y, bool isBoss, short levelBoss, int idBoss, int dame, int dameMp, Char c)
    {
        level = levelBoss;
        Effect2.vEffect2.addElement(new MonsterDart(x, y, isBoss, levelBoss, idBoss, dame, dameMp, c));
    }

    /// <summary>Add monster dart.</summary>
    /// <param name="x">Horizontal coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="y">Vertical coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="b">Buffer or intermediate output value.</param>
    public static void addMonsterDart(int x, int y, BuNhin b)
    {
        Effect2.vEffect2.addElement(new MonsterDart(x, y, b));
    }

    /// <summary>Advance this object&#x27;s state for the current client logic tick.</summary>
    /// <remarks>Locals: o: Object being inspected or converted; o2: O2; o3: O3; o4: O4; num: Numeric intermediate used by this operation; the legacy name is retained; num2: Numeric intermediate used by this operation; the legacy name is retained; num3: Numeric intermediate used by this operation; the legacy name is retained; num4: Numeric intermediate used by this operation; the legacy name is retained; num5: Numeric intermediate used by this operation; the legacy name is retained; i: Loop/collection index.</remarks>
    public override void update()
    {
        if (level > 100)
        {
            Domsang o = new Domsang(x, y, 2);
            vDomsang.addElement(o);
        }
        else if (level > 50 && level <= 100)
        {
            Domsang o2 = new Domsang(x, y, 5);
            vDomsang.addElement(o2);
        }
        else if (level > 30 && level <= 50)
        {
            Domsang o3 = new Domsang(x, y, 4);
            vDomsang.addElement(o3);
        }
        else if (level > 0 && level <= 30)
        {
            Domsang o4 = new Domsang(x, y, 3);
            vDomsang.addElement(o4);
        }

        updateAngle();
        int num = 0;
        int num2 = 0;
        if (typeAtt == 0)
        {
            num = c.cx - x;
            num2 = c.cy - 10 - y;
            life++;
            if (c.statusMe == 5 || c.statusMe == 14)
            {
                x += (c.cx - x) / 2;
                y += (c.cy - y) / 2;
            }

            if ((Res.abs(num) < 16 && Res.abs(num2) < 16) || life > 60)
            {
                c.doInjure(dame, dameMp, isBoss, idBoss);
                Effect2.vEffect2.removeElement(this);
                return;
            }
        }
        else if (typeAtt == 1)
        {
            num = b.x - x;
            num2 = b.y - 10 - y;
            life++;
            if ((Res.abs(num) < 16 && Res.abs(num2) < 16) || life > 60)
            {
                b.doInjure();
                Effect2.vEffect2.removeElement(this);
                return;
            }
        }

        int num3 = Res.angle(num, num2);
        if (Math.abs(num3 - angle) < 90 || num * num + num2 * num2 > 4096)
        {
            if (Math.abs(num3 - angle) < 15)
            {
                angle = num3;
            }
            else if ((num3 - angle >= 0 && num3 - angle < 180) || num3 - angle < -180)
            {
                angle = Res.fixangle(angle + 15);
            }
            else
            {
                angle = Res.fixangle(angle - 15);
            }
        }

        num += vx;
        int num4 = num >> 10;
        x += num4;
        num &= 0x3FF;
        num2 += vy;
        int num5 = num2 >> 10;
        y += num5;
        num2 &= 0x3FF;
        for (int i = 0; i < vDomsang.size(); i++)
        {
            Domsang domsang = (Domsang)vDomsang.elementAt(i);
            domsang.update();
            if (domsang.frame > 3)
            {
                vDomsang.removeElementAt(i);
            }
        }
    }

    /// <summary>Compute tickz.</summary>
    /// <param name="time">Time.</param>
    private int tickz(int time)
    {
        return GameCanvas.gameTick % time;
    }

    /// <summary>Update angle.</summary>
    /// <remarks>Locals: num: Horizontal position/difference used by this calculation; num2: Vertical position/difference used by this calculation; num3: Numeric intermediate used by this operation; the legacy name is retained; num4: Numeric intermediate used by this operation; the legacy name is retained; num5: Numeric intermediate used by this operation; the legacy name is retained.</remarks>
    private void updateAngle()
    {
        if (c == null)
        {
            isEnd = true;
            return;
        }

        int num = c.cx - x;
        int num2 = c.cy - (c.ch >> 1) - y;
        life++;
        if ((Res.abs(num) < 16 && Res.abs(num2) < 16) || life > 60)
        {
            isEnd = true;
            return;
        }

        int num3 = Res.angle(num, num2);
        if (Math.abs(num3 - angle) < 90 || num * num + num2 * num2 > 4096)
        {
            if (Math.abs(num3 - angle) < 15)
            {
                angle = num3;
            }
            else if ((num3 - angle >= 0 && num3 - angle < 180) || num3 - angle < -180)
            {
                angle = Res.fixangle(angle + 15);
            }
            else
            {
                angle = Res.fixangle(angle - 15);
            }
        }

        if (!isSpeedUp && va < 8192)
        {
            va += 1024;
        }

        vx = va * Res.cos(angle) >> 10;
        vy = va * Res.sin(angle) >> 10;
        num += vx;
        int num4 = num >> 10;
        x += num4;
        num &= 0x3FF;
        num2 += vy;
        int num5 = num2 >> 10;
        y += num5;
        num2 &= 0x3FF;
        index = findDirIndexFromAngle(Res.angle(num4, -num5));
        frame = FRAME[index];
        regMirro = TRANSFORM[index];
    }

    /// <summary>Find dir index from angle.</summary>
    /// <param name="angle">Angle.</param>
    /// <remarks>Locals: i: Loop/collection index.</remarks>
    public static int findDirIndexFromAngle(int angle)
    {
        for (int i = 0; i < ARROWINDEX.Length - 1; i++)
        {
            if (angle >= ARROWINDEX[i] && angle <= ARROWINDEX[i + 1])
            {
                if (i >= 16)
                {
                    return 0;
                }

                return i;
            }
        }

        return 0;
    }

    /// <summary>Draw this object&#x27;s current state through the legacy graphics wrapper.</summary>
    /// <param name="g">Legacy drawing context.</param>
    public override void paint(mGraphics g)
    {
        using (Interp.Offset(g, this))
            PaintAtRenderPosition(g);
    }

    /// <summary>Draw at the interpolated position without changing logical gameplay coordinates.</summary>
    /// <param name="g">Legacy drawing context.</param>
    /// <remarks>Locals: num: Numeric intermediate used by this operation; the legacy name is retained; i: Loop/collection index.</remarks>
    private void PaintAtRenderPosition(mGraphics g)
    {
        if (isBoss)
        {
            int num = tickz(7);
            if (idBoss == 114)
            {
                if (num < 4)
                {
                    SmallImage.drawSmallImage(g, 1299, x, y, 0, mGraphics.VCENTER | mGraphics.HCENTER);
                }
                else
                {
                    SmallImage.drawSmallImage(g, 1307, x, y, 0, mGraphics.VCENTER | mGraphics.HCENTER);
                }
            }
            else if (idBoss == 115)
            {
                num = tickz(20);
                if (num < 4)
                {
                    SmallImage.drawSmallImage(g, 1379, x, y, 0, mGraphics.VCENTER | mGraphics.HCENTER);
                }
                else if (num < 8)
                {
                    SmallImage.drawSmallImage(g, 1380, x, y, 0, mGraphics.VCENTER | mGraphics.HCENTER);
                }
                else if (num < 12)
                {
                    SmallImage.drawSmallImage(g, 1379, x, y, 0, mGraphics.VCENTER | mGraphics.HCENTER);
                }
                else if (num < 16)
                {
                    SmallImage.drawSmallImage(g, 1382, x, y, 0, mGraphics.VCENTER | mGraphics.HCENTER);
                }
            }
            else if (idBoss == 116)
            {
                num = tickz(17);
                if (num < 4)
                {
                    SmallImage.drawSmallImage(g, 1399, x, y, 0, mGraphics.VCENTER | mGraphics.HCENTER);
                }
                else if (num < 8)
                {
                    SmallImage.drawSmallImage(g, 1400, x, y, 0, mGraphics.VCENTER | mGraphics.HCENTER);
                }
                else if (num < 12)
                {
                    SmallImage.drawSmallImage(g, 1401, x, y, 0, mGraphics.VCENTER | mGraphics.HCENTER);
                }
                else if (num < 16)
                {
                    SmallImage.drawSmallImage(g, 1402, x, y, 0, mGraphics.VCENTER | mGraphics.HCENTER);
                }
            }
            else if (idBoss == 139)
            {
                num = tickz(20);
                if (num < 4)
                {
                    SmallImage.drawSmallImage(g, 1459, x, y, 0, mGraphics.VCENTER | mGraphics.HCENTER);
                }
                else if (num < 8)
                {
                    SmallImage.drawSmallImage(g, 1380, x, y, 0, mGraphics.VCENTER | mGraphics.HCENTER);
                }
                else if (num < 12)
                {
                    SmallImage.drawSmallImage(g, 1461, x, y, 0, mGraphics.VCENTER | mGraphics.HCENTER);
                }
                else if (num < 16)
                {
                    SmallImage.drawSmallImage(g, 1382, x, y, 0, mGraphics.VCENTER | mGraphics.HCENTER);
                }
            }
            else if (idBoss == 144 || idBoss == 163)
            {
                num = tickz(20);
                if (num < 4)
                {
                    SmallImage.drawSmallImage(g, 1459, x, y, 0, mGraphics.VCENTER | mGraphics.HCENTER);
                }
                else if (num < 8)
                {
                    SmallImage.drawSmallImage(g, 1380, x, y, 0, mGraphics.VCENTER | mGraphics.HCENTER);
                }
                else if (num < 12)
                {
                    SmallImage.drawSmallImage(g, 1461, x, y, 0, mGraphics.VCENTER | mGraphics.HCENTER);
                }
                else if (num < 16)
                {
                    SmallImage.drawSmallImage(g, 1382, x, y, 0, mGraphics.VCENTER | mGraphics.HCENTER);
                }
            }
        }
        else
        {
            if (level > 100)
            {
                g.drawRegion(GameScr.imgEffMob, 0, frame * 23, 31, 23, regMirro, x, y, 0);
            }
            else if (level > 50 && level <= 100)
            {
                g.drawRegion(GameScr.imgEffMob, 0, 0, 14, 14, regMirro, x, y, 0);
            }
            else if (level > 30 && level <= 50)
            {
                g.drawRegion(GameScr.imgEffMob, 0, 0, 8, 8, regMirro, x, y, 0);
            }
            else if (level > 0 && level <= 30)
            {
                g.drawRegion(GameScr.imgEffMob, 0, 0, 8, 8, regMirro, x, y, 0);
            }

            for (int i = 0; i < vDomsang.size(); i++)
            {
                ((Domsang)vDomsang.elementAt(i))?.paint(g);
            }
        }
    }
}

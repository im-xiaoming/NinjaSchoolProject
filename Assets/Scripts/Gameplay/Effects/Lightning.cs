using System;

public class Lightning : Effect2
{
    private int[] color = new int[2]
    {
        16579837,
        11188220
    };
    public MyVector listPos = new MyVector();
    public MyVector[] list;
    public EPosition posangle;
    private long timeDel;
    public bool isContinue;
    public bool isRemove = true;
    public bool isRun;
    private int tick;
    private int cou;
    private int dem;
    // aa: Aa.
    private int aa = 7;
    /// <summary>Add light.</summary>
    /// <param name="position">Position.</param>
    /// <param name="firstPos">First position.</param>
    /// <param name="isContinue">Is continue.</param>
    /// <param name="color">Color.</param>
    /// <remarks>Locals: i: Loop/collection index.</remarks>
    public static void addLight(MyVector position, EPosition firstPos, bool isContinue, int color)
    {
        Lightning lightning = new Lightning();
        lightning.color[1] = color;
        for (int i = 0; i < position.size(); i++)
        {
            EPosition ePosition = (EPosition)position.elementAt(i);
            if (ePosition != null && (Res.abs(ePosition.x - firstPos.x) >= 100 || Res.abs(ePosition.y - firstPos.y) >= 50))
            {
                position.removeElementAt(i);
            }
        }

        lightning.setInfo(position, firstPos, isContinue);
        Effect2.vEffect2.addElement(lightning);
    }

    /// <summary>Reset lightning.</summary>
    public void reset()
    {
        timeDel = mSystem.getCurrentTimeMillis() / 10;
        listPos.removeAllElements();
    }

    /// <summary>Set info.</summary>
    /// <param name="position">Position.</param>
    /// <param name="angle">Angle.</param>
    /// <param name="isContinue">Is continue.</param>
    /// <remarks>Locals: i: Loop/collection index; num: Numeric intermediate used by this operation; the legacy name is retained; j: Nested loop/collection index; x: Horizontal position/difference used by this calculation; y: Vertical position/difference used by this calculation; num2: Number of entries in the source collection; num3: Horizontal position/difference used by this calculation; num4: Numeric intermediate used by this operation; the legacy name is retained; num5: Numeric intermediate used by this operation; the legacy name is retained; num6: Numeric intermediate used by this operation; the legacy name is retained; num7: Numeric intermediate used by this operation; the legacy name is retained; num8: Numeric intermediate used by this operation; the legacy name is retained; o: Object being inspected or converted; k: Key or nested iteration index; num9: Number of entries in the source collection; l: Length or current list; num10: Numeric intermediate used by this operation; the legacy name is retained; m: Current message or map entry; num11: Numeric intermediate used by this operation; the legacy name is retained; num12: Numeric intermediate used by this operation; the legacy name is retained; num13: Numeric intermediate used by this operation; the legacy name is retained.</remarks>
    public void setInfo(MyVector position, EPosition angle, bool isContinue)
    {
        if (position.size() == 0)
        {
            return;
        }

        isRun = true;
        this.isContinue = isContinue;
        if (!isContinue)
        {
            orderVector(position);
        }

        listPos = position;
        posangle = angle;
        list = new MyVector[position.size()];
        for (int i = 0; i < list.Length; i++)
        {
            list[i] = new MyVector();
        }

        angle.follow = -1;
        list[0].addElement(angle);
        int num = -1;
        for (int j = 0; j < position.size(); j++)
        {
            int x = angle.x;
            int y = angle.y;
            if (isContinue && num != -1)
            {
                EPosition ePosition = (EPosition)position.elementAt(num);
                x = ePosition.x;
                y = ePosition.y;
            }

            num = (isContinue ? (num + 1) : rndIndexList(position));
            int num2 = list[num].size() - 1;
            EPosition ePosition2 = (EPosition)position.elementAt(num);
            int num3 = Res.angle(ePosition2.x - x, -(ePosition2.y - y));
            int num4 = Res.rnd(15) + 10;
            int num5 = 0;
            int num6 = 0;
            while (true)
            {
                num6 = 0;
                if (num5 != 0)
                {
                    num6 = num3 - 5 + Res.rnd(10);
                }

                num6 = Res.fixangle(num6);
                int num7 = num4 * num5 * Res.cos(num6) >> 10;
                int num8 = -(num4 * num5 * Res.sin(num6)) >> 10;
                EPosition o = new EPosition(x + num7, y + num8, num2++);
                list[num].addElement(o);
                if (Res.distance(x, y, x + num7, y + num8) >= Res.distance(x, y, ePosition2.x, ePosition2.y) - 20)
                {
                    break;
                }

                num5++;
            }
        }

        for (int k = 0; k < list.Length; k++)
        {
            int num9 = list[k].size();
            EPosition ePosition3 = (EPosition)position.elementAt(k);
            ePosition3.follow = (sbyte)(list[k].size() - 1);
            ePosition3.index = -1;
            EPosition ePosition4 = new EPosition(ePosition3.x, ePosition3.y, ePosition3.follow);
            ePosition4.index = -1;
            list[k].addElement(ePosition4);
            for (int l = 1; l < num9; l++)
            {
                EPosition ePosition5 = (EPosition)list[k].elementAt(l);
                int num10 = Res.rnd(2);
                for (int m = 0; m < num10; m++)
                {
                    int angle2 = 180 + Res.rnd(180);
                    int num11 = 5 + Res.rnd(10);
                    int num12 = num11 * Res.cos(Res.fixangle(angle2)) >> 10;
                    int num13 = -(num11 * Res.sin(Res.fixangle(angle2))) >> 10;
                    EPosition ePosition6 = new EPosition(ePosition5.x + num12, ePosition5.y + num13, l);
                    ePosition6.index = 0;
                    list[k].addElement(ePosition6);
                }
            }
        }
    }

    /// <summary>Compute order vector.</summary>
    /// <param name="obj">Obj.</param>
    /// <remarks>Locals: num: Number of entries in the source collection; i: Loop/collection index; j: Nested loop/collection index.</remarks>
    public static MyVector orderVector(MyVector obj)
    {
        int num = obj.size();
        for (int i = 0; i < num - 1; i++)
        {
            EPosition ePosition = (EPosition)obj.elementAt(i);
            for (int j = i + 1; j < num; j++)
            {
                EPosition ePosition2 = (EPosition)obj.elementAt(j);
                if (ePosition.x > ePosition2.x)
                {
                    obj.setElementAt(ePosition, j);
                    obj.setElementAt(ePosition2, i);
                    ePosition = ePosition2;
                }
            }
        }

        return obj;
    }

    /// <summary>Compute rnd index list.</summary>
    /// <param name="pos">Position.</param>
    /// <remarks>Locals: num: Numeric intermediate used by this operation; the legacy name is retained; i: Loop/collection index; num2: Numeric intermediate used by this operation; the legacy name is retained; j: Nested loop/collection index.</remarks>
    private int rndIndexList(MyVector pos)
    {
        int num = 0;
        for (int i = 0; i < pos.size(); i++)
        {
            EPosition ePosition = (EPosition)pos.elementAt(i);
            if (ePosition.index == -1)
            {
                num++;
            }
        }

        if (num != 0)
        {
            num = Res.rnd(num);
            int num2 = 0;
            for (int j = 0; j < pos.size(); j++)
            {
                EPosition ePosition2 = (EPosition)pos.elementAt(j);
                if (ePosition2.index == -1)
                {
                    if (num == num2)
                    {
                        ePosition2.index = 0;
                        return j;
                    }

                    num2++;
                }
            }

            return -1;
        }

        return -1;
    }

    /// <summary>Advance this object&#x27;s state for the current client logic tick.</summary>
    /// <remarks>Locals: num: Numeric intermediate used by this operation; the legacy name is retained; i: Loop/collection index.</remarks>
    public override void update()
    {
        if (posangle == null)
        {
            Effect2.vRemoveEffect2.addElement(this);
            return;
        }

        int num = 0;
        try
        {
            if (GameCanvas.gameTick % 2 == 1)
            {
                num = 1;
                posangle.follow = -1;
                posangle.index = -1;
                num = 2;
                for (int i = 0; i < listPos.size(); i++)
                {
                    EPosition ePosition = (EPosition)listPos.elementAt(i);
                    ePosition.index = -1;
                    ePosition.follow = -1;
                }

                num = 3;
                if (isContinue && isRemove && listPos.size() > 1 && mSystem.getCurrentTimeMillis() / 10 - timeDel > 30)
                {
                    timeDel = mSystem.getCurrentTimeMillis() / 10;
                    posangle = (EPosition)listPos.elementAt(0);
                    listPos.removeElementAt(0);
                }

                num = 4;
                setInfo(listPos, posangle, isContinue);
                if (tick > 3)
                {
                    aa = 7;
                    isRun = false;
                    Effect2.vEffect2.removeElement(this);
                }

                num = 5;
                tick++;
            }
        }
        catch (Exception)
        {
        }
    }

    /// <summary>Draw this object&#x27;s current state through the legacy graphics wrapper.</summary>
    /// <param name="g">Legacy drawing context.</param>
    /// <remarks>Locals: i: Loop/collection index; j: Nested loop/collection index.</remarks>
    public override void paint(mGraphics g)
    {
        dem = 0;
        cou++;
        if (cou >= 12)
        {
            cou = 0;
        }

        if (list == null)
        {
            return;
        }

        for (int i = 0; i < list.Length; i++)
        {
            for (int j = 0; j < list[i].size(); j++)
            {
                EPosition ePosition = (EPosition)list[i].elementAt(j);
                if (ePosition.follow < 0 || ePosition.follow >= list[i].size())
                {
                    continue;
                }

                EPosition ePosition2 = (EPosition)list[i].elementAt(ePosition.follow);
                if (GameCanvas.isPaint(ePosition.x, ePosition.y) && GameCanvas.isPaint(ePosition2.x, ePosition2.y))
                {
                    paintLine(g, ePosition, ePosition2);
                }

                if (isContinue && isRemove)
                {
                    dem++;
                    if (dem >= aa)
                    {
                        aa += 7;
                        return;
                    }
                }
            }

            EPosition ePosition3 = (EPosition)listPos.elementAt(i);
            ePosition3.count = (sbyte)(ePosition3.count + 1);
            if (ePosition3.count >= 12)
            {
                ePosition3.count = 0;
            }
        }
    }

    /// <summary>Draw line in the current graphics context.</summary>
    /// <param name="g">Legacy drawing context.</param>
    /// <param name="pos1">Pos1.</param>
    /// <param name="pos2">Pos2.</param>
    private void paintLine(mGraphics g, EPosition pos1, EPosition pos2)
    {
        g.setColor(color[0]);
        g.drawLine(pos1.x, pos1.y, pos2.x, pos2.y);
        if (pos1.index == -1)
        {
            g.setColor(color[1]);
            g.drawLine(pos1.x - 1, pos1.y, pos2.x - 1, pos2.y);
            if (isContinue && isRemove)
            {
                g.drawLine(pos1.x + 1, pos1.y, pos2.x + 1, pos2.y);
            }
        }
    }
}

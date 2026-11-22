public class MainObject
{
    public MyVector vecEFfect = new MyVector();
    /// <summary>Add dataeff.</summary>
    /// <param name="id">Identifier.</param>
    /// <param name="timelive">Timelive.</param>
    /// <param name="miliSecondWait">Mili second wait.</param>
    /// <param name="isHead">Is head.</param>
    /// <remarks>Locals: i: Loop/collection index; o: Object being inspected or converted.</remarks>
    public void addDataeff(short id, long timelive, int miliSecondWait, bool isHead)
    {
        for (int i = 0; i < vecEFfect.size(); i++)
        {
            DataSkillEff dataSkillEff = (DataSkillEff)vecEFfect.elementAt(i);
            if (dataSkillEff != null && dataSkillEff.idEff == id)
            {
                dataSkillEff.timelive = timelive + mSystem.currentTimeMillis();
                dataSkillEff.miliSecondWait = miliSecondWait;
                dataSkillEff.setTypeEff(timelive);
                return;
            }
        }

        DataSkillEff o = new DataSkillEff(id, timelive, miliSecondWait, isHead);
        vecEFfect.addElement(o);
    }

    /// <summary>Return start point paint fly.</summary>
    public int getStartPointPaintFly()
    {
        return 0;
    }

    /// <summary>Draw top data effect in the current graphics context.</summary>
    /// <param name="g">Legacy drawing context.</param>
    /// <param name="x">Horizontal coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="y">Vertical coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="dy">Vertical difference or offset.</param>
    /// <remarks>Locals: i: Loop/collection index; y2: Y2.</remarks>
    public void paintTopDataEff(mGraphics g, int x, int y, int dy)
    {
        for (int i = 0; i < vecEFfect.size(); i++)
        {
            DataSkillEff dataSkillEff = (DataSkillEff)vecEFfect.elementAt(i);
            if (dataSkillEff != null)
            {
                int y2 = y;
                if (dataSkillEff.isHead)
                {
                    y2 = y + dy + 4;
                }

                dataSkillEff.paintTopEff(g, x, y2);
            }
        }
    }

    /// <summary>Draw bottom data effect in the current graphics context.</summary>
    /// <param name="g">Legacy drawing context.</param>
    /// <param name="x">Horizontal coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="y">Vertical coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="dy">Vertical difference or offset.</param>
    /// <remarks>Locals: i: Loop/collection index; y2: Y2.</remarks>
    public void paintBottomDataEff(mGraphics g, int x, int y, int dy)
    {
        for (int i = 0; i < vecEFfect.size(); i++)
        {
            DataSkillEff dataSkillEff = (DataSkillEff)vecEFfect.elementAt(i);
            if (dataSkillEff != null)
            {
                int y2 = y;
                if (dataSkillEff.isHead)
                {
                    y2 = y + dy + 4;
                }

                dataSkillEff.paintBottomEff(g, x, y2);
            }
        }
    }

    /// <summary>Update data effect.</summary>
    /// <param name="type">Type.</param>
    /// <param name="statusMe">Status me.</param>
    /// <remarks>Locals: i: Loop/collection index.</remarks>
    public void updateDataEff(sbyte type, int statusMe)
    {
        for (int i = 0; i < vecEFfect.size(); i++)
        {
            DataSkillEff dataSkillEff = (DataSkillEff)vecEFfect.elementAt(i);
            if (dataSkillEff != null)
            {
                dataSkillEff.update();
                if (dataSkillEff.wantDetroy)
                {
                    vecEFfect.removeElement(dataSkillEff);
                }
            }
        }

        if (type == 0 && statusMe == 14)
        {
            removeAllEff();
        }

        if (type == 1 && statusMe == 0)
        {
            removeAllEff();
        }
    }

    /// <summary>Retain the setid pp compatibility hook without performing work.</summary>
    /// <param name="id">Retained for call compatibility; not read by this implementation.</param>
    public void setidPP(short id)
    {
    }

    /// <summary>Retain the set thoi trang compatibility hook without performing work.</summary>
    /// <param name="arr">Retained for call compatibility; not read by this implementation.</param>
    public virtual void setThoiTrang(short[] arr)
    {
    }

    /// <summary>Remove all effect.</summary>
    public void removeAllEff()
    {
        vecEFfect.removeAllElements();
    }
}

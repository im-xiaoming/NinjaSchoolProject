using Functions.Auto;
using System;
using System.Collections;

public partial class Char
{
    /// <summary>Start die.</summary>
    /// <param name="toX">To x.</param>
    /// <param name="toY">To y.</param>
    /// <remarks>Locals: i: Loop/collection index.</remarks>
    public void startDie(short toX, short toY)
    {
        if (me)
        {
            isLockKey = true;
            for (int i = 0; i < GameScr.vCharInMap.size(); i++)
            {
                Char @char = (Char)GameScr.vCharInMap.elementAt(i);
                @char.killCharId = -9999;
            }
        }

        statusMe = 5;
        cp2 = toX;
        cp3 = toY;
        cp1 = 0;
        cHP = 0;
        testCharId = -9999;
        killCharId = -9999;
    }

    /// <summary>Wait for to die.</summary>
    /// <param name="toX">To x.</param>
    /// <param name="toY">To y.</param>
    public void waitToDie(short toX, short toY)
    {
        wdx = toX;
        wdy = toY;
    }

    /// <summary>Perform change status stand.</summary>
    public void changeStatusStand()
    {
        timeBocdau = 0;
        statusMe = 1;
        // Offline fix: the fall that set cyStartFall (setCharFallFromJump) is over once the character stands.
        // The original only cleared it on one landing path, so a stale value (e.g. 401 from a previous map)
        // made updateCharFall skip every floor above that height: walking off a ledge in Cửa biển Kawaguchi
        // fell through the ground to the bottom row and the server killed the character (isCrossMap).
        cyStartFall = 0;
        timeSummon = mSystem.currentTimeMillis();
    }

    /// <summary>Perform live from dead.</summary>
    public void liveFromDead()
    {
        cHP = cMaxHP;
        cMP = cMaxMP;
        changeStatusStand();
        cp1 = (cp2 = (cp3 = 0));
        ServerEffect.addServerEffect(20, this, 2);
        GameScr.gI().center = null;
    }
}

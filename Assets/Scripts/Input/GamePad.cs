public class GamePad
{
    // xC: X c.
    private int xC;
    // yC: Y c.
    private int yC;
    // xM: X m.
    private int xM;
    // yM: Y m.
    private int yM;
    private int xMLast;
    private int yMLast;
    // R: R.
    private int R;
    // r: Result being assembled or returned.
    private int r;
    // d: Distance or intermediate difference.
    private int d;
    private int xTemp;
    private int yTemp;
    private int deltaX;
    private int deltaY;
    private int delta;
    private int angle;
    private bool isGamePad;
    /// <summary>Initialize game pad state and its supplied dependencies.</summary>
    public GamePad()
    {
        R = 28;
    }

    /// <summary>Advance this object&#x27;s state for the current client logic tick.</summary>
    public void update()
    {
        if (Main.isPC)
        {
            return;
        }

        if (GameCanvas.isPointerDown && !GameCanvas.isPointerJustRelease)
        {
            xTemp = GameCanvas.pxFirst;
            yTemp = GameCanvas.pyFirst;
            if (xTemp > (GameCanvas.w >> 1) - 100 || yTemp < GameCanvas.h >> 1)
            {
                return;
            }

            if (!isGamePad)
            {
                xC = (xM = xTemp);
                yC = (yM = yTemp);
            }

            isGamePad = true;
            deltaX = GameCanvas.px - xC;
            deltaY = GameCanvas.py - yC;
            delta = Math.pow(deltaX, 2) + Math.pow(deltaY, 2);
            d = Res.sqrt(delta);
            if (Math.abs(deltaX) <= 4 && Math.abs(deltaY) <= 4)
            {
                return;
            }

            angle = Res.angle(deltaX, deltaY);
            if (!GameCanvas.isPointerHoldIn(xC - R, yC - R, 2 * R, 2 * R))
            {
                if (d != 0)
                {
                    yM = deltaY * R / d;
                    xM = deltaX * R / d;
                    xM += xC;
                    yM += yC;
                    if (!Res.inRect(xC - R, yC - R, 2 * R, 2 * R, xM, yM))
                    {
                        xM = xMLast;
                        yM = yMLast;
                    }
                    else
                    {
                        xMLast = xM;
                        yMLast = yM;
                    }
                }
                else
                {
                    xM = xMLast;
                    yM = yMLast;
                }
            }
            else
            {
                xM = GameCanvas.px;
                yM = GameCanvas.py;
            }

            resetHold();
            if (checkPointerMove(2))
            {
                if ((angle <= 360 && angle > 340) || (angle > 0 && angle <= 90))
                {
                    GameScr.auto = 0;
                    GameCanvas.keyHold[6] = true;
                    GameCanvas.keyPressedz[6] = true;
                }
                else if (angle > 290 && angle <= 340)
                {
                    GameScr.auto = 0;
                    GameCanvas.keyHold[3] = true;
                    GameCanvas.keyPressedz[3] = true;
                }
                else if (angle > 250 && angle <= 290)
                {
                    GameScr.auto = 0;
                    GameCanvas.keyHold[2] = true;
                    GameCanvas.keyPressedz[2] = true;
                }
                else if (angle > 200 && angle <= 250)
                {
                    GameScr.auto = 0;
                    GameCanvas.keyHold[1] = true;
                    GameCanvas.keyPressedz[1] = true;
                }
                else if (angle > 90 && angle <= 200)
                {
                    GameScr.auto = 0;
                    GameCanvas.keyHold[4] = true;
                    GameCanvas.keyPressedz[4] = true;
                }
            }
            else
            {
                resetHold();
            }
        }
        else
        {
            xM = (xC = 50);
            yM = (yC = GameCanvas.h - 50);
            isGamePad = false;
            resetHold();
        }
    }

    /// <summary>Check pointer move.</summary>
    /// <param name="distance">Distance.</param>
    /// <remarks>Locals: num: Numeric intermediate used by this operation; the legacy name is retained; i: Horizontal position/difference used by this calculation; i2: Vertical position/difference used by this calculation.</remarks>
    private bool checkPointerMove(int distance)
    {
        if (Main.isPC)
        {
            return false;
        }

        if (Char.getMyChar().statusMe == 3)
        {
            return true;
        }

        for (int num = 2; num > 0; num--)
        {
            int i = GameCanvas.arrPos[num].x - GameCanvas.arrPos[num - 1].x;
            int i2 = GameCanvas.arrPos[num].y - GameCanvas.arrPos[num - 1].y;
            if (Res.abs(i) > distance && Res.abs(i2) > distance)
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>Reset hold.</summary>
    private void resetHold()
    {
        GameCanvas.keyHold[Key.NUM1] = false;
        GameCanvas.keyHold[Key.NUM2] = false;
        GameCanvas.keyHold[Key.NUM3] = false;
        GameCanvas.keyHold[Key.NUM4] = false;
        GameCanvas.keyHold[Key.NUM6] = false;
    }

    /// <summary>Draw this object&#x27;s current state through the legacy graphics wrapper.</summary>
    /// <param name="g">Legacy drawing context.</param>
    public void paint(mGraphics g)
    {
        if (!Main.isPC)
        {
            g.drawImage(GameScr.imgAnalog1, xC, yC, mGraphics.HCENTER | mGraphics.VCENTER);
            g.drawImage(GameScr.imgAnalog2, xM, yM, mGraphics.HCENTER | mGraphics.VCENTER);
        }
    }
}

public class Paint
{
    public static int COLORBACKGROUND = 6562304;
    public static int COLORLIGHT = 9581056;
    public static int COLORDARK = 3937280;
    public static int COLORBORDER = 15224576;
    public static int COLORFOCUS = 16777215;
    public static Image imgBg;
    public static Image imgLogo;
    public static Image imgLB;
    public static Image imgLT;
    public static Image imgRB;
    public static Image imgRT;
    public static Image imgChuong;
    public static Image imgSelectBoard;
    public static Image imgtoiSmall;
    public static Image imgTayTren;
    public static Image imgTayDuoi;
    public static Image imgCheckPass;
    public static Image[] imgTick = new Image[2];
    public static Image[] imgMsg = new Image[2];
    public static int hTab = 24;
    public static int lenCaption = 0;
    public int[] color = new int[7]
    {
        15970400,
        11528155,
        2250052,
        16374659,
        15906669,
        12931125,
        3108954
    };
    public static int CmdButtonH
    {
        get
        {
            return GameScr.imgLbtn != null ? GameScr.imgLbtn.getHeight() : mScreen.cmdH;
        }
    }

    // top of the bottom buttons (Chọn / Đóng ...): the button image ends a gap above the screen's bottom edge
    // ("Ninja School UI Layout" > Bottom Buttons > Bottom Gap)
    public static int CmdY
    {
        get
        {
            return GameCanvas.h - CmdButtonH - NsoUiLayout.Buttons.bottomGap;
        }
    }

    public static int CmdButtonW
    {
        get
        {
            return GameScr.imgLbtn != null ? GameScr.imgLbtn.getWidth() : 70;
        }
    }

    // left edge of the left button / of the right button: a padding away from the screen sides
    // ("Ninja School UI Layout" > Bottom Buttons > Side Padding)
    public static int CmdLeftX
    {
        get
        {
            return NsoUiLayout.Buttons.sidePadding;
        }
    }

    public static int CmdRightX
    {
        get
        {
            return GameCanvas.w - CmdButtonW - NsoUiLayout.Buttons.sidePadding;
        }
    }

    /// <summary>Draw command bar in the current graphics context.</summary>
    /// <param name="g">Legacy drawing context.</param>
    /// <param name="left">Left.</param>
    /// <param name="center">Center.</param>
    /// <param name="right">Right.</param>
    /// <remarks>Locals: num: Numeric intermediate used by this operation; the legacy name is retained.</remarks>
    public void paintCmdBar(mGraphics g, Command left, Command center, Command right)
    {
        int cmdY = CmdY;
        // the left / right button under the mouse is drawn with its focused image (hover); the centre one is not
        int btnW = CmdButtonW, btnH = CmdButtonH;
        int leftX = CmdLeftX, rightX = CmdRightX; // captions are centred on their button
        mFont tahoma_7b_yellow = mFont.tahoma_7b_yellow;
        int num = 3;
        if (left != null)
        {
            lenCaption = tahoma_7b_yellow.getWidth(left.caption);
            if (lenCaption > 0)
            {
                if (left.x > 0 && left.y > 0)
                {
                    Command.hoverEnabled = true;
                    left.paint(g);
                    Command.hoverEnabled = false;
                }
                else if (lenCaption < 40)
                {
                    if (mScreen.keyTouch == 0 || Command.keySelected == left || g.isMouseOver(leftX, cmdY, btnW, btnH))
                    {
                        g.drawImage(GameScr.imgLbtnFocus, leftX, cmdY, 0);
                    }
                    else
                    {
                        g.drawImage(GameScr.imgLbtn, leftX, cmdY, 0);
                    }

                    TextCalib.Use("buttonLeft");
                    tahoma_7b_yellow.drawString(g, left.caption, leftX + btnW / 2, cmdY + num, 2);
                    TextCalib.Clear();
                }
                else
                {
                    if (mScreen.keyTouch == 0 || Command.keySelected == left || g.isMouseOver(leftX, cmdY, btnW, btnH))
                    {
                        g.drawImage(GameScr.imgLbtnFocus, leftX, cmdY, 0);
                    }
                    else
                    {
                        g.drawImage(GameScr.imgLbtn, leftX, cmdY, 0);
                    }

                    TextCalib.Use("buttonLeft");
                    tahoma_7b_yellow.drawString(g, left.caption, leftX + btnW / 2, cmdY + num, 2);
                    TextCalib.Clear();
                }
            }
        }

        if (center != null)
        {
            lenCaption = tahoma_7b_yellow.getWidth(center.caption);
            if (lenCaption > 0)
            {
                if (center.x > 0 && center.y > 0)
                {
                    center.paint(g);
                }
                else if (lenCaption < 40)
                {
                    if (mScreen.keyTouch == 1 || Command.keySelected == center)
                    {
                        g.drawImage(GameScr.imgLbtnFocus, GameCanvas.hw - 35, cmdY, 0);
                    }
                    else
                    {
                        g.drawImage(GameScr.imgLbtn, GameCanvas.hw - 35, cmdY, 0);
                    }

                    TextCalib.Use("buttonCenter");
                    tahoma_7b_yellow.drawString(g, center.caption, GameCanvas.hw, cmdY + num, 2);
                    TextCalib.Clear();
                }
                else
                {
                    if (mScreen.keyTouch == 1 || Command.keySelected == center)
                    {
                        g.drawImage(GameScr.imgLbtnFocus, GameCanvas.hw - 35, cmdY, 0);
                    }
                    else
                    {
                        g.drawImage(GameScr.imgLbtn, GameCanvas.hw - 35, cmdY, 0);
                    }

                    TextCalib.Use("buttonCenter");
                    tahoma_7b_yellow.drawString(g, center.caption, GameCanvas.hw, cmdY + num, 2);
                    TextCalib.Clear();
                }
            }
        }

        if (right == null)
        {
            return;
        }

        lenCaption = tahoma_7b_yellow.getWidth(right.caption);
        if (lenCaption <= 0)
        {
            return;
        }

        if (right.x > 0 && right.y > 0)
        {
            Command.hoverEnabled = true;
            right.paint(g);
            Command.hoverEnabled = false;
        }
        else if (lenCaption < 40)
        {
            if (mScreen.keyTouch == 2 || Command.keySelected == right || g.isMouseOver(rightX, cmdY, btnW, btnH))
            {
                g.drawImage(GameScr.imgLbtnFocus, rightX, cmdY, 0);
            }
            else
            {
                g.drawImage(GameScr.imgLbtn, rightX, cmdY, 0);
            }

            TextCalib.Use("buttonRight");
            tahoma_7b_yellow.drawString(g, right.caption, rightX + btnW / 2, cmdY + num, 2);
            TextCalib.Clear();
        }
        else
        {
            if (mScreen.keyTouch == 2 || Command.keySelected == right || g.isMouseOver(rightX, cmdY, btnW, btnH))
            {
                g.drawImage(GameScr.imgLbtnFocus, rightX, cmdY, 0);
            }
            else
            {
                g.drawImage(GameScr.imgLbtn, rightX, cmdY, 0);
            }

            TextCalib.Use("buttonRight");
            tahoma_7b_yellow.drawString(g, right.caption, rightX + btnW / 2, cmdY + num, 2);
            TextCalib.Clear();
        }
    }

    /// <summary>Draw logo in the current graphics context.</summary>
    /// <param name="g">Legacy drawing context.</param>
    /// <param name="x">Horizontal coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="y">Vertical coordinate in the caller&#x27;s coordinate space.</param>
    public void paintLogo(mGraphics g, int x, int y)
    {
        g.drawImage(imgLogo, x, y, 3);
    }

    /// <summary>Draw input tf in the current graphics context.</summary>
    /// <param name="g">Legacy drawing context.</param>
    /// <param name="isz">Isz.</param>
    /// <param name="x">Horizontal coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="y">Vertical coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="w">Width or horizontal span.</param>
    /// <param name="h">Height or vertical span.</param>
    /// <param name="xText">X text.</param>
    /// <param name="yText">Y text.</param>
    /// <param name="text">Text fragment assembled or read by this operation.</param>
    /// <remarks>Locals: i: Loop/collection index; j: Nested loop/collection index.</remarks>
    public void paintInputTf(mGraphics g, bool isz, int x, int y, int w, int h, int xText, int yText, string text)
    {
        g.setColor(0);
        if (isz)
        {
            g.drawRegion(GameScr.imgTf, 0, 81, 29, 27, 0, x, y, 0);
            g.drawRegion(GameScr.imgTf, 0, 135, 29, 27, 0, x + w - 29, y, 0);
            g.drawRegion(GameScr.imgTf, 0, 108, 29, 27, 0, x + w - 58, y, 0);
            for (int i = 0; i < (w - 58) / 29; i++)
            {
                g.drawRegion(GameScr.imgTf, 0, 108, 29, 27, 0, x + 29 + i * 29, y, 0);
            }
        }
        else
        {
            g.drawRegion(GameScr.imgTf, 0, 0, 29, 27, 0, x, y, 0);
            g.drawRegion(GameScr.imgTf, 0, 54, 29, 27, 0, x + w - 29, y, 0);
            g.drawRegion(GameScr.imgTf, 0, 27, 29, 27, 0, x + w - 58, y, 0);
            for (int j = 0; j < (w - 58) / 29; j++)
            {
                g.drawRegion(GameScr.imgTf, 0, 27, 29, 27, 0, x + 29 + j * 29, y, 0);
            }
        }

        g.setClip(x + 3, y + 1, w - 4, h - 4);
        yText -= 3;
        mFont.tahoma_8b.drawString(g, text, xText, yText, 0);
    }

    /// <summary>Draw logo in the current graphics context.</summary>
    /// <param name="g">Legacy drawing context.</param>
    public void paintLogo(mGraphics g)
    {
        g.setColor(8916494);
        g.fillRect(0, 0, GameCanvas.w, GameCanvas.h);
        g.drawImage(imgLogo, GameCanvas.instance.getWidth() >> 1, GameCanvas.instance.getHeight() >> 1, 3);
    }

    /// <summary>Draw frame in the current graphics context.</summary>
    /// <param name="x">Horizontal coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="y">Vertical coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="w">Width or horizontal span.</param>
    /// <param name="h">Height or vertical span.</param>
    /// <param name="g">Legacy drawing context.</param>
    /// <remarks>Locals: i: Loop/collection index; j: Nested loop/collection index.</remarks>
    public void paintFrame(int x, int y, int w, int h, mGraphics g)
    {
        if (mGraphics.zoomLevel == 1)
        {
            g.setColor(COLORBACKGROUND);
            g.fillRect(x, y, w, h);
            g.setColor(4144959, 0.9f);
            g.drawRect(x - 2, y - 2, w + 3, h + 3);
            g.setColor(13619410);
            g.drawRect(x - 1, y - 1, w + 1, h + 1);
            g.setColor(5720393);
            g.drawRect(x, y, w - 1, h - 1);
            g.drawImage(GameCanvas.imgBorder[0], x - 2, y - 2, mGraphics.TOP | mGraphics.LEFT);
            g.drawRegion(GameCanvas.imgBorder[0], 0, 0, GameCanvas.wImgimgBorder, GameCanvas.hImgimgBorder, 2, x + w + 2, y - 2, mGraphics.TOP | mGraphics.RIGHT);
            g.drawRegion(GameCanvas.imgBorder[0], 0, 0, GameCanvas.wImgimgBorder, GameCanvas.hImgimgBorder, 1, x - 2, y + h + 2, mGraphics.BOTTOM | mGraphics.LEFT);
            g.drawRegion(GameCanvas.imgBorder[0], 0, 0, GameCanvas.wImgimgBorder, GameCanvas.hImgimgBorder, 3, x + w + 2, y + h + 2, mGraphics.BOTTOM | mGraphics.RIGHT);
            g.drawImage(GameCanvas.imgBorder[2], x + w / 2, y - GameCanvas.imgBorder[1].getHeight() / 2 + 2, mGraphics.TOP | mGraphics.HCENTER);
            return;
        }

        g.setColor(COLORBACKGROUND);
        g.fillRect(x - 1, y, w + 1, h);
        for (int i = 0; i < w / GameCanvas.borderLineW - 2; i++)
        {
            g.drawImage(GameCanvas.imgBorder[1], x + i * GameCanvas.borderLineW, y - 2);
            g.drawRegion(GameCanvas.imgBorder[1], 0, 0, 12, GameCanvas.borderLineH, 3, x + i * GameCanvas.borderLineW, y + h - 1, StaticObj.TOP_LEFT);
        }

        for (int j = 0; j < h / GameCanvas.borderLineH - 2; j++)
        {
            g.drawImage(GameCanvas.imgBorder[4], x - 3 + MotherCanvas.addL, y + j * GameCanvas.borderLineH, StaticObj.TOP_LEFT);
            g.drawImage(GameCanvas.imgBorder[3], x + w - (mGraphics.zoomLevel - 2) + MotherCanvas.addR, y + j * GameCanvas.borderLineH, StaticObj.TOP_LEFT);
        }

        g.drawImage(GameCanvas.imgBorder[0], x - 2, y - 2, mGraphics.TOP | mGraphics.LEFT);
        g.drawRegion(GameCanvas.imgBorder[0], 0, 0, GameCanvas.borderConnerW, GameCanvas.borderConnerH, 2, x + w + 2, y - 2, StaticObj.TOP_RIGHT);
        g.drawRegion(GameCanvas.imgBorder[0], 0, 0, GameCanvas.borderConnerW, GameCanvas.borderConnerH, 1, x - 2, y + h + 1, StaticObj.BOTTOM_LEFT);
        g.drawRegion(GameCanvas.imgBorder[0], 0, 0, GameCanvas.borderConnerW, GameCanvas.borderConnerH, 3, x + w + 2, y + h + 1, StaticObj.BOTTOM_RIGHT);
        g.drawImage(GameCanvas.imgBorder[2], x + w / 2, y - 7, StaticObj.TOP_CENTER);
    }

}

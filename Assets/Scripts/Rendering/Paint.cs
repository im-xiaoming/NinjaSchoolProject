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
    /// <summary>Draw default bg in the current graphics context.</summary>
    /// <param name="g">Legacy drawing context.</param>
    public void paintDefaultBg(mGraphics g)
    {
        g.setColor(8916494);
        g.fillRect(0, 0, GameCanvas.w, GameCanvas.h);
        g.drawImage(imgBg, GameCanvas.w / 2, GameCanvas.h / 2 - hTab / 2 - 1, 3);
        g.drawImage(imgLT, 0, 0, 0);
        g.drawImage(imgRT, GameCanvas.w, 0, mGraphics.TOP | mGraphics.RIGHT);
        g.drawImage(imgLB, 0, GameCanvas.h - hTab - 2, mGraphics.BOTTOM | mGraphics.LEFT);
        g.drawImage(imgRB, GameCanvas.w, GameCanvas.h - hTab - 2, mGraphics.BOTTOM | mGraphics.RIGHT);
        g.setColor(16774843);
        g.drawRect(0, 0, GameCanvas.w, 0);
        g.drawRect(0, GameCanvas.h - hTab - 2, GameCanvas.w, 0);
        g.drawRect(0, 0, 0, GameCanvas.h - hTab);
        g.drawRect(GameCanvas.w - 1, 0, 0, GameCanvas.h - hTab);
    }

    /// <summary>Perform paintfill default bg in the current graphics context.</summary>
    /// <param name="g">Legacy drawing context.</param>
    public void paintfillDefaultBg(mGraphics g)
    {
        g.setColor(205314);
        g.fillRect(0, 0, GameCanvas.w, GameCanvas.h);
    }

    /// <summary>Retain the repaint circle bg compatibility hook without performing work.</summary>
    public void repaintCircleBg()
    {
    }

    /// <summary>Retain the paint solid bg compatibility hook without performing work.</summary>
    /// <param name="g">Retained for call compatibility; not read by this implementation.</param>
    public void paintSolidBg(mGraphics g)
    {
    }

    /// <summary>Draw default popup in the current graphics context.</summary>
    /// <param name="g">Legacy drawing context.</param>
    /// <param name="x">Horizontal coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="y">Vertical coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="w">Width or horizontal span.</param>
    /// <param name="h">Height or vertical span.</param>
    public void paintDefaultPopup(mGraphics g, int x, int y, int w, int h)
    {
        g.setColor(8411138);
        g.fillRect(x, y, w, h);
        g.setColor(13606712);
        g.drawRect(x, y, w, h);
    }

    /// <summary>Draw white popup in the current graphics context.</summary>
    /// <param name="g">Legacy drawing context.</param>
    /// <param name="y">Vertical coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="x">Horizontal coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="width">Width.</param>
    /// <param name="height">Height.</param>
    public void paintWhitePopup(mGraphics g, int y, int x, int width, int height)
    {
        g.setColor(16776363);
        g.fillRect(x, y, width, height);
        g.setColor(0);
        g.drawRect(x - 1, y - 1, width + 1, height + 1);
    }

    /// <summary>Draw default popup h in the current graphics context.</summary>
    /// <param name="g">Legacy drawing context.</param>
    /// <param name="h">Height or vertical span.</param>
    public void paintDefaultPopupH(mGraphics g, int h)
    {
        g.setColor(14279153);
        g.fillRect(8, GameCanvas.h - (h + 37), GameCanvas.w - 16, h + 4);
        g.setColor(4682453);
        g.fillRect(10, GameCanvas.h - (h + 35), GameCanvas.w - 20, h);
    }

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

    /// <summary>Draw select in the current graphics context.</summary>
    /// <param name="g">Legacy drawing context.</param>
    /// <param name="x">Horizontal coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="y">Vertical coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="w">Width or horizontal span.</param>
    /// <param name="h">Height or vertical span.</param>
    public void paintSelect(mGraphics g, int x, int y, int w, int h)
    {
        g.setColor(16774843);
        g.fillRect(x, y, w, h);
    }

    /// <summary>Draw logo in the current graphics context.</summary>
    /// <param name="g">Legacy drawing context.</param>
    /// <param name="x">Horizontal coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="y">Vertical coordinate in the caller&#x27;s coordinate space.</param>
    public void paintLogo(mGraphics g, int x, int y)
    {
        g.drawImage(imgLogo, x, y, 3);
    }

    /// <summary>Retain the paint hotline compatibility hook without performing work.</summary>
    /// <param name="g">Retained for call compatibility; not read by this implementation.</param>
    /// <param name="number">Retained for call compatibility; not read by this implementation.</param>
    public void paintHotline(mGraphics g, string number)
    {
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

    /// <summary>Draw input tf new in the current graphics context.</summary>
    /// <param name="g">Legacy drawing context.</param>
    /// <param name="isz">Isz.</param>
    /// <param name="x">Horizontal coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="y">Vertical coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="w">Width or horizontal span.</param>
    /// <param name="h">Retained for call compatibility; not read by this implementation.</param>
    /// <param name="xText">X text.</param>
    /// <param name="yText">Y text.</param>
    /// <param name="text">Text fragment assembled or read by this operation.</param>
    /// <remarks>Locals: i: Loop/collection index; j: Nested loop/collection index.</remarks>
    public void paintInputTf_new(mGraphics g, bool isz, int x, int y, int w, int h, int xText, int yText, string text)
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

        yText -= 3;
        mFont.tahoma_8b.drawString(g, text, xText, yText, 0);
    }

    /// <summary>Draw back menu in the current graphics context.</summary>
    /// <param name="g">Legacy drawing context.</param>
    /// <param name="x">Horizontal coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="y">Vertical coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="w">Width or horizontal span.</param>
    /// <param name="h">Height or vertical span.</param>
    /// <param name="isz">Isz.</param>
    public void paintBackMenu(mGraphics g, int x, int y, int w, int h, bool isz)
    {
        if (isz)
        {
            g.setColor(16646144);
            g.fillRoundRect(x, y, w, h, 10, 10);
            g.setColor(16770612);
        }
        else
        {
            g.setColor(16775097);
            g.fillRoundRect(x, y, w, h, 10, 10);
            g.setColor(16775097);
        }

        g.fillRoundRect(x + 3, y + 3, w - 6, h - 6, 10, 10);
    }

    /// <summary>Draw message bg in the current graphics context.</summary>
    /// <param name="g">Legacy drawing context.</param>
    /// <param name="x">Horizontal coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="y">Vertical coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="w">Width or horizontal span.</param>
    /// <param name="h">Height or vertical span.</param>
    /// <param name="title">Retained for call compatibility; not read by this implementation.</param>
    /// <param name="subTitle">Sub title.</param>
    /// <param name="check">Check.</param>
    public void paintMsgBG(mGraphics g, int x, int y, int w, int h, string title, string subTitle, string check)
    {
        g.setClip(x, y, w, h);
        g.setColor(5215093);
        g.fillRect(x, y, w - 1, h - 1);
        g.setColor(16777215);
        g.drawRect(x, y, w - 1, h - 1);
        g.setColor(3502080);
        g.fillRect(x + 1, y + 25, w - 2, mScreen.ITEM_HEIGHT);
        mFont.tahoma_8b.drawString(g, subTitle, x + 10, y + 28, 0);
        mFont.tahoma_8b.drawString(g, check, x + GameCanvas.w - 45, y + 28, 2);
    }

    /// <summary>Draw default screen list in the current graphics context.</summary>
    /// <param name="g">Legacy drawing context.</param>
    /// <param name="title">Retained for call compatibility; not read by this implementation.</param>
    /// <param name="subTitle">Sub title.</param>
    /// <param name="check">Check.</param>
    public void paintDefaultScrList(mGraphics g, string title, string subTitle, string check)
    {
        g.setClip(0, 0, GameCanvas.w, GameCanvas.h);
        paintDefaultBg(g);
        g.setColor(16770612);
        g.fillRect(0, 25, GameCanvas.w, mScreen.ITEM_HEIGHT);
        mFont.tahoma_8b.drawString(g, subTitle, 10, 28, 0);
        mFont.tahoma_8b.drawString(g, check, GameCanvas.w - 20, 28, 2);
    }

    /// <summary>Draw check in the current graphics context.</summary>
    /// <param name="g">Legacy drawing context.</param>
    /// <param name="x">Horizontal coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="y">Vertical coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="index">Index.</param>
    public void paintCheck(mGraphics g, int x, int y, int index)
    {
        g.drawImage(imgTick[1], x, y, 3);
        if (index == 1)
        {
            g.drawImage(imgTick[0], x + 1, y - 3, 3);
        }
    }

    /// <summary>Draw image message in the current graphics context.</summary>
    /// <param name="g">Legacy drawing context.</param>
    /// <param name="x">Horizontal coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="y">Vertical coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="index">Index.</param>
    public void paintImgMsg(mGraphics g, int x, int y, int index)
    {
        g.drawImage(imgMsg[index], x, y, 0);
    }

    /// <summary>Draw title board in the current graphics context.</summary>
    /// <param name="g">Legacy drawing context.</param>
    /// <param name="roomId">Retained for call compatibility; not read by this implementation.</param>
    public void paintTitleBoard(mGraphics g, int roomId)
    {
        paintDefaultBg(g);
    }

    /// <summary>Draw check pass in the current graphics context.</summary>
    /// <param name="g">Legacy drawing context.</param>
    /// <param name="x">Horizontal coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="y">Vertical coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="check">Check.</param>
    /// <param name="focus">Retained for call compatibility; not read by this implementation.</param>
    public void paintCheckPass(mGraphics g, int x, int y, bool check, bool focus)
    {
        g.drawRegion(imgCheckPass, 0, check ? 14 : 0, 14, 14, 0, x - 2, y - 2, 0);
    }

    /// <summary>Draw input dlg in the current graphics context.</summary>
    /// <param name="g">Legacy drawing context.</param>
    /// <param name="x">Horizontal coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="y">Vertical coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="w">Width or horizontal span.</param>
    /// <param name="h">Height or vertical span.</param>
    /// <param name="str">Text.</param>
    /// <param name="imgCapcha">Image capcha.</param>
    /// <remarks>Locals: num: Measured vertical size; num2: Numeric intermediate used by this operation; the legacy name is retained; num3: Numeric intermediate used by this operation; the legacy name is retained; num4: Measured vertical size; num5: Numeric intermediate used by this operation; the legacy name is retained; num6: Numeric intermediate used by this operation; the legacy name is retained.</remarks>
    public void paintInputDlg(mGraphics g, int x, int y, int w, int h, string[] str, Image imgCapcha)
    {
        if (imgCapcha == null)
        {
            paintFrame(x, y, w, h, g);
            int num = y + 20 - mFont.tahoma_8b.getHeight();
            int num2 = 0;
            int num3 = num;
            while (num2 < str.Length)
            {
                mFont.tahoma_8b.drawString(g, str[num2], x + w / 2, num3, 2);
                num2++;
                num3 += mFont.tahoma_8b.getHeight();
            }

            return;
        }

        paintFrame(x, y - imgCapcha.getHeight(), w, h + imgCapcha.getHeight(), g);
        g.drawImage(imgCapcha, x + w / 2, y + h / 2 - 4, mGraphics.HCENTER | mGraphics.BOTTOM);
        int num4 = y + 20 - mFont.tahoma_8b.getHeight();
        int num5 = 0;
        int num6 = num4;
        while (num5 < str.Length)
        {
            mFont.tahoma_8b.drawString(g, str[num5], x + w / 2, num6 - imgCapcha.getHeight(), 2);
            num5++;
            num6 += mFont.tahoma_8b.getHeight();
        }
    }

    /// <summary>Retain the paint icon main menu compatibility hook without performing work.</summary>
    /// <param name="g">Retained for call compatibility; not read by this implementation.</param>
    /// <param name="x">Retained for call compatibility; not read by this implementation.</param>
    /// <param name="y">Retained for call compatibility; not read by this implementation.</param>
    /// <param name="isz">Retained for call compatibility; not read by this implementation.</param>
    /// <param name="isSe">Retained for call compatibility; not read by this implementation.</param>
    /// <param name="i">Retained for call compatibility; not read by this implementation.</param>
    /// <param name="wStr">Retained for call compatibility; not read by this implementation.</param>
    public void paintIconMainMenu(mGraphics g, int x, int y, bool isz, bool isSe, int i, int wStr)
    {
    }

    /// <summary>Draw line room in the current graphics context.</summary>
    /// <param name="g">Legacy drawing context.</param>
    /// <param name="x">Horizontal coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="y">Vertical coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="xTo">X to.</param>
    /// <param name="yTo">Y to.</param>
    public void paintLineRoom(mGraphics g, int x, int y, int xTo, int yTo)
    {
        g.setColor(16774843);
        g.drawLine(x, y, xTo, yTo);
    }

    /// <summary>Draw cell containt in the current graphics context.</summary>
    /// <param name="g">Legacy drawing context.</param>
    /// <param name="x">Horizontal coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="y">Vertical coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="w">Width or horizontal span.</param>
    /// <param name="h">Retained for call compatibility; not read by this implementation.</param>
    /// <param name="isz">Isz.</param>
    public void paintCellContaint(mGraphics g, int x, int y, int w, int h, bool isz)
    {
        if (isz)
        {
            g.setColor(13132288);
            g.fillRect(x + 2, y + 2, w - 3, w - 3);
        }

        g.setColor(3502080);
        g.drawRect(x, y, w, w);
    }

    /// <summary>Draw scroll in the current graphics context.</summary>
    /// <param name="g">Legacy drawing context.</param>
    /// <param name="x">Horizontal coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="y">Vertical coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="h">Height or vertical span.</param>
    public void paintScroll(mGraphics g, int x, int y, int h)
    {
        g.setColor(3847752);
        g.fillRect(x, y, 4, h);
    }

    /// <summary>Return color message.</summary>
    public int[] getColorMsg()
    {
        return color;
    }

    /// <summary>Draw logo in the current graphics context.</summary>
    /// <param name="g">Legacy drawing context.</param>
    public void paintLogo(mGraphics g)
    {
        g.setColor(8916494);
        g.fillRect(0, 0, GameCanvas.w, GameCanvas.h);
        g.drawImage(imgLogo, GameCanvas.instance.getWidth() >> 1, GameCanvas.instance.getHeight() >> 1, 3);
    }

    /// <summary>Draw text login in the current graphics context.</summary>
    /// <param name="g">Legacy drawing context.</param>
    /// <param name="isRes">Is res.</param>
    /// <remarks>Locals: num: Numeric intermediate used by this operation; the legacy name is retained.</remarks>
    public void paintTextLogin(mGraphics g, bool isRes)
    {
        int num = 0;
        if (!isRes && GameCanvas.h <= 240)
        {
            num = 15;
        }

        mFont.tahoma_7b_white.drawString(g, mResources.LOGINLABELS[0], GameCanvas.hw, GameCanvas.hh + 60 - num, 2);
        mFont.tahoma_7b_white.drawString(g, mResources.LOGINLABELS[1], GameCanvas.hw, GameCanvas.hh + 73 - num, 2);
    }

    /// <summary>Draw sellect board in the current graphics context.</summary>
    /// <param name="g">Legacy drawing context.</param>
    /// <param name="x">Horizontal coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="y">Vertical coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="w">Retained for call compatibility; not read by this implementation.</param>
    /// <param name="h">Retained for call compatibility; not read by this implementation.</param>
    public void paintSellectBoard(mGraphics g, int x, int y, int w, int h)
    {
        g.drawImage(imgSelectBoard, x - 7, y, 0);
    }

    /// <summary>Check whether register using wap applies to the supplied state.</summary>
    public int isRegisterUsingWAP()
    {
        return 0;
    }

    /// <summary>Return card.</summary>
    public string getCard()
    {
        return "/vmg/card.on";
    }

    /// <summary>Draw sellected shop in the current graphics context.</summary>
    /// <param name="g">Legacy drawing context.</param>
    /// <param name="x">Horizontal coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="y">Vertical coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="w">Retained for call compatibility; not read by this implementation.</param>
    /// <param name="h">Retained for call compatibility; not read by this implementation.</param>
    public void paintSellectedShop(mGraphics g, int x, int y, int w, int h)
    {
        g.setColor(16777215);
        g.drawRect(x, y, 40, 40);
        g.drawRect(x + 1, y + 1, 38, 38);
    }

    /// <summary>Retain the do select compatibility hook without performing work.</summary>
    /// <param name="focus">Retained for call compatibility; not read by this implementation.</param>
    public void doSelect(int focus)
    {
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

    /// <summary>Draw frame border in the current graphics context.</summary>
    /// <param name="x">Horizontal coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="y">Vertical coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="w">Width or horizontal span.</param>
    /// <param name="h">Height or vertical span.</param>
    /// <param name="g">Legacy drawing context.</param>
    /// <remarks>Locals: i: Loop/collection index; j: Nested loop/collection index.</remarks>
    public void paintFrameBorder(int x, int y, int w, int h, mGraphics g)
    {
        if (mGraphics.zoomLevel == 1)
        {
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

    /// <summary>Draw frame inside in the current graphics context.</summary>
    /// <param name="x">Horizontal coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="y">Vertical coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="w">Width or horizontal span.</param>
    /// <param name="h">Height or vertical span.</param>
    /// <param name="g">Legacy drawing context.</param>
    public void paintFrameInside(int x, int y, int w, int h, mGraphics g)
    {
        g.setColor(COLORBACKGROUND);
        g.fillRect(x, y, w, h);
    }

    /// <summary>Draw frame inside selected in the current graphics context.</summary>
    /// <param name="x">Horizontal coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="y">Vertical coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="w">Width or horizontal span.</param>
    /// <param name="h">Height or vertical span.</param>
    /// <param name="g">Legacy drawing context.</param>
    public void paintFrameInsideSelected(int x, int y, int w, int h, mGraphics g)
    {
        g.setColor(COLORLIGHT);
        g.fillRect(x, y, w, h);
    }
}

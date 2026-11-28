public class Command
{
    public string caption;
    public string[] subCaption;
    public IActionListener actionListener;
    public int idAction;
    public Image back;
    public Image focus;
    // img: Image.
    public Image img;
    // x: Horizontal coordinate in the caller's coordinate space.
    public int x;
    // y: Vertical coordinate in the caller's coordinate space.
    public int y;
    // paint(): whether this button was on the bottom row (y == Paint.CmdY) last time, and that row's y then
    private bool onBottomRow;
    private int bottomRowY;
    // w: Width or horizontal span.
    public int w = mScreen.cmdW;
    // h: Height or vertical span.
    public int h = mScreen.cmdH;
    private int lenCaption;
    private bool isFocus;
    // p: Current player/point or request payload.
    public object p;
    /// <summary>Initialize command state and its supplied dependencies.</summary>
    /// <param name="caption">Caption.</param>
    /// <param name="actionListener">Action listener.</param>
    /// <param name="action">Action.</param>
    /// <param name="p">Current player/point or request payload.</param>
    /// <param name="x">Horizontal coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="y">Vertical coordinate in the caller&#x27;s coordinate space.</param>
    public Command(string caption, IActionListener actionListener, int action, object p, int x, int y)
    {
        this.caption = caption;
        idAction = action;
        this.actionListener = actionListener;
        this.p = p;
        this.x = x;
        this.y = y;
        w = mScreen.cmdW;
        h = mScreen.cmdH;
        back = null;
        focus = null;
    }

    /// <summary>Initialize command state and its supplied dependencies.</summary>
    public Command()
    {
    }

    /// <summary>Initialize command state and its supplied dependencies.</summary>
    /// <param name="caption">Caption.</param>
    public Command(string caption)
    {
        this.caption = caption;
    }

    /// <summary>Initialize command state and its supplied dependencies.</summary>
    /// <param name="caption">Caption.</param>
    /// <param name="actionListener">Action listener.</param>
    /// <param name="action">Action.</param>
    /// <param name="p">Current player/point or request payload.</param>
    public Command(string caption, IActionListener actionListener, int action, object p)
    {
        this.caption = caption;
        idAction = action;
        this.actionListener = actionListener;
        this.p = p;
    }

    /// <summary>Initialize command state and its supplied dependencies.</summary>
    /// <param name="caption">Caption.</param>
    /// <param name="action">Action.</param>
    /// <param name="p">Current player/point or request payload.</param>
    public Command(string caption, int action, object p)
    {
        this.caption = caption;
        idAction = action;
        this.p = p;
    }

    /// <summary>Initialize command state and its supplied dependencies.</summary>
    /// <param name="caption">Caption.</param>
    /// <param name="action">Action.</param>
    public Command(string caption, int action)
    {
        this.caption = caption;
        idAction = action;
    }

    /// <summary>Initialize command state and its supplied dependencies.</summary>
    /// <param name="caption">Caption.</param>
    /// <param name="action">Action.</param>
    /// <param name="x">Horizontal coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="y">Vertical coordinate in the caller&#x27;s coordinate space.</param>
    public Command(string caption, int action, int x, int y)
    {
        this.caption = caption;
        idAction = action;
        this.x = x;
        this.y = y;
    }

    /// <summary>Perform setpos.</summary>
    /// <param name="x">Horizontal coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="y">Vertical coordinate in the caller&#x27;s coordinate space.</param>
    public void setpos(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    /// <summary>Perform perform action.</summary>
    public void performAction()
    {
        MenuAudio.Click(); // button sound on menu screens (Settings > Âm thanh > Âm thanh nút bấm)
        if (idAction > 0)
        {
            if (actionListener != null)
            {
                actionListener.perform(idAction, p);
            }
            else
            {
                GameScr.gI().actionPerform(idAction, p);
            }
        }
    }

    /// <summary>Perform perform action.</summary>
    /// <param name="str">Retained for call compatibility; not read by this implementation.</param>
    public void performAction(string str)
    {
        MenuAudio.Click(); // button sound on menu screens (Settings > Âm thanh > Âm thanh nút bấm)
        if (idAction > 0)
        {
            if (actionListener != null)
            {
                actionListener.perform(idAction, p);
            }
            else
            {
                GameScr.gI().actionPerform(idAction, p);
            }
        }
    }

    // when above 0 the icon image is drawn at this size (logical pixels, top-left at x,y) instead of its natural size
    public int imgDrawSize;
    // Hover highlight is on only while the bottom bar paints its left / right button (Paint.paintCmdBar).
    public static bool hoverEnabled;
    // Offline addition: the bottom button chosen with the arrow keys / gamepad in a Dialog
    // (Dialog.paint sets it only while painting its command bar). Drawn with the focused image.
    public static Command keySelected;
    /// <summary>Draw this object&#x27;s current state through the legacy graphics wrapper.</summary>
    /// <param name="g">Legacy drawing context.</param>
    public void paint(mGraphics g)
    {
        if (back != null)
        {
            g.drawImage(back, x, y, 0);
        }

        if (focus != null && isFocus)
        {
            g.drawImage(focus, x, y, 0);
        }

        if (img != null && imgDrawSize > 0)
        {
            g.drawImageScale(img, x, y, imgDrawSize, imgDrawSize, 0);
        }
        else if (img != null)
        {
            g.drawImage(img, x + mGraphics.getImageWidth(img) / 2, y + mGraphics.getImageHeight(img) / 2, mGraphics.HCENTER | mGraphics.VCENTER);
        }
        else if (caption != string.Empty)
        {
            // every caption button keeps at least "Ninja School UI Layout" > Bottom Buttons > Bottom Gap above the screen's bottom:
            // a button placed lower (screens hard-code GameCanvas.h - 26 etc.) is lifted to Paint.CmdY.
            // y itself is changed so the click area (input) moves with the picture.
            // a button that sat on the bottom row last frame follows the row when Bottom Gap changes (Settings > Giao diện)
            if (onBottomRow && y == bottomRowY && y != Paint.CmdY)
            {
                y = Paint.CmdY;
            }

            if (y > Paint.CmdY && y + Paint.CmdButtonH > GameCanvas.h - NsoUiLayout.Buttons.bottomGap)
            {
                y = Paint.CmdY;
            }

            onBottomRow = y == Paint.CmdY;
            bottomRowY = Paint.CmdY;
            if (!isFocus && this != keySelected && !(hoverEnabled && g.isMouseOver(x, y, GameScr.imgLbtn.getWidth(), GameScr.imgLbtn.getHeight()))) // hover = focused look
            {
                g.drawImage(GameScr.imgLbtn, x, y, 0);
            }
            else
            {
                g.drawImage(GameScr.imgLbtnFocus, x, y, 0);
            }

            TextCalib.Use("command");
            mFont.tahoma_7b_yellow.drawString(g, caption, x + 36, y + 3, 2);
            TextCalib.Clear();
        }
    }

    /// <summary>Compute input.</summary>
    public bool input()
    {
        if (GameCanvas.isPointerHoldIn(x - 3, y - 3, w + 6, h + 6))
        {
            isFocus = true;
            if (GameCanvas.isPointerJustRelease && GameCanvas.isPointerClick)
            {
                Sound.play(Sound.MBClick, 0.5f);
                return true;
            }
        }
        else
        {
            isFocus = false;
        }

        return false;
    }

}

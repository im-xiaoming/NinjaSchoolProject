public class MsgDlg : Dialog
{
    public string[] info;
    public bool isWait;
    public int timeShow;
    // h: Height or vertical span.
    private int h;
    private int padLeft;
    public bool isPaintBlackBg;
    public bool isIpad;
    // read by the Canvas view (MsgDlgView)
    public int Height { get { return h; } }
    public int PadLeft { get { return padLeft; } }
    /// <summary>Initialize message dlg state and its supplied dependencies.</summary>
    public MsgDlg()
    {
        padLeft = 60;
    }

    /// <summary>Perform pleasewait.</summary>
    public void pleasewait()
    {
        setInfo(mResources.PLEASEWAIT, null, null, null);
        GameCanvas.currentDialog = this;
    }

    /// <summary>Show message dlg.</summary>
    public override void show()
    {
        GameCanvas.currentDialog = this;
    }

    /// <summary>Set info.</summary>
    /// <param name="info">Info.</param>
    /// <param name="left">Left.</param>
    /// <param name="center">Center.</param>
    /// <param name="right">Right.</param>
    public void setInfo(string info, Command left, Command center, Command right)
    {
        this.info = mFont.tahoma_8b.splitFontArray(info, GameCanvas.w - (padLeft * 2 + 40));
        base.left = left;
        base.center = center;
        base.right = right;
        if (center != null)
        {
            base.center.x = GameCanvas.w / 2 - 35;
            base.center.y = Paint.CmdY;
            if (left != null)
            {
                base.left.x = GameCanvas.w / 2 - 115;
                base.left.y = Paint.CmdY;
            }

            if (right != null)
            {
                base.right.x = GameCanvas.w / 2 + 45;
                base.right.y = Paint.CmdY;
            }
        }
        else
        {
            if (left != null)
            {
                base.left.x = GameCanvas.w / 2 - 80;
                base.left.y = Paint.CmdY;
            }

            if (right != null)
            {
                base.right.x = GameCanvas.w / 2 + 10;
                base.right.y = Paint.CmdY;
            }
        }

        isWait = false;
        h = 80;
        if (this.info.Length >= 5)
        {
            h = this.info.Length * mFont.tahoma_8b.getHeight() + 20;
        }
    }

    /// <summary>Set info ipad.</summary>
    /// <param name="info">Info.</param>
    /// <param name="left">Left.</param>
    /// <param name="center">Center.</param>
    /// <param name="right">Right.</param>
    /// <param name="isIpad">Is ipad.</param>
    public void setInfoIpad(string info, Command left, Command center, Command right, bool isIpad)
    {
        this.info = mFont.tahoma_8b.splitFontArray(info, GameCanvas.w - (padLeft * 2 + 40));
        base.left = left;
        base.center = center;
        base.right = right;
        isWait = false;
        h = 80;
        if (this.info.Length >= 5)
        {
            h = this.info.Length * mFont.tahoma_8b.getHeight() + 20;
        }

        this.isIpad = isIpad;
    }

    /// <summary>Draw this object&#x27;s current state through the legacy graphics wrapper.</summary>
    /// <param name="g">Legacy drawing context.</param>
    /// <remarks>The message box, its wait spinner and its buttons are the Canvas view MsgDlgView.</remarks>
    public override void paint(mGraphics g)
    {
    }

    /// <summary>Advance this object&#x27;s state for the current client logic tick.</summary>
    public override void update()
    {
        if (timeShow > 0)
        {
            timeShow--;
            if (timeShow == 1)
            {
                GameCanvas.endDlg();
                timeShow = 0;
            }
        }

        base.update();
    }
}

public class InputDlg : Dialog
{
    protected string[] info;
    public TField tfInput;
    private int padLeft;
    /// <summary>Initialize input dlg state and its supplied dependencies.</summary>
    public InputDlg()
    {
        padLeft = 40;
        if (GameCanvas.w <= 176)
        {
            padLeft = 10;
        }

        tfInput = new TField();
        tfInput.x = padLeft + 10;
        tfInput.y = GameCanvas.h - mScreen.ITEM_HEIGHT - 43;
        tfInput.width = GameCanvas.w - 2 * (padLeft + 10);
        tfInput.height = mScreen.ITEM_HEIGHT + 2;
        tfInput.isFocus = true;
        right = tfInput.cmdClear;
    }

    /// <summary>Show input dlg.</summary>
    /// <param name="info">Info.</param>
    /// <param name="ok">Ok.</param>
    /// <param name="type">Type.</param>
    public void show(string info, Command ok, int type)
    {
        tfInput.setText(string.Empty);
        tfInput.setIputType(type);
        this.info = mFont.tahoma_8b.splitFontArray(info, GameCanvas.w - padLeft * 2);
        left = new Command(mResources.CLOSE, GameCanvas.gI(), 8882, null);
        center = ok;
        show();
    }

    /// <summary>Draw this object&#x27;s current state through the legacy graphics wrapper.</summary>
    /// <param name="g">Legacy drawing context.</param>
    public override void paint(mGraphics g)
    {
        GameCanvas.paintz.paintInputDlg(g, padLeft, GameCanvas.h - 77 - mScreen.cmdH, GameCanvas.w - padLeft * 2, 69, info, GameCanvas.testCapcha);
        tfInput.paint(g);
        base.paint(g);
    }

    // Key routing, field update/focus, number stepping and button navigation live in Dialog.
    protected override TField FocusedField
    {
        get
        {
            return tfInput;
        }
    }

    /// <summary>Show input dlg.</summary>
    public override void show()
    {
        if (left != null)
        {
            left.x = GameCanvas.w / 2 - 160;
            left.y = Paint.CmdY;
        }

        if (center != null)
        {
            center.x = GameCanvas.w / 2 - 35;
            center.y = Paint.CmdY;
        }

        if (right != null)
        {
            right.x = GameCanvas.w / 2 + 88;
            right.y = Paint.CmdY;
        }

        GameCanvas.currentDialog = this;
    }

    /// <summary>Hide input dlg.</summary>
    public void hide()
    {
        GameCanvas.endDlg();
    }
}

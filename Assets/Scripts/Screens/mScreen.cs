public class mScreen
{
    public Command left;
    public Command center;
    public Command right;
    public Command cmdClose;
    public static int ITEM_HEIGHT;
    public static int cmdW;
    public static int cmdH;
    public static int keyTouch = -1;
    public static int height;
    public static int width;
    /// <summary>Make this screen active and apply its existing entry/reset behavior.</summary>
    public virtual void switchToMe()
    {
        GameCanvas.clearKeyPressed();
        GameCanvas.clearKeyHold();
        if (GameCanvas.currentScreen != null)
        {
            GameCanvas.currentScreen.unLoad();
        }

        GameCanvas.currentScreen = this;
    }

    /// <summary>Retain the un load compatibility hook without performing work.</summary>
    public virtual void unLoad()
    {
    }

    /// <summary>Initialize position.</summary>
    public static void initPos()
    {
        if (GameCanvas.isTouch)
        {
            cmdH = 22;
        }
        else
        {
            cmdH = 24;
        }
    }

    /// <summary>Handle a key action using this control&#x27;s existing key mapping.</summary>
    /// <param name="keyCode">Retained for call compatibility; not read by this implementation.</param>
    public virtual void keyPress(int keyCode)
    {
    }

    /// <summary>Advance this object&#x27;s state for the current client logic tick.</summary>
    public virtual void update()
    {
    }

    /// <summary>Translate the current client key/pointer state into screen actions.</summary>
    public virtual void updateKey()
    {
        if (GameCanvas.keyPressedz[5] || getCmdPointerLast(GameCanvas.currentScreen.center))
        {
            GameCanvas.keyPressedz[5] = false;
            keyTouch = -1;
            GameCanvas.isPointerJustRelease = false;
            if (center != null)
            {
                center.performAction();
            }

            Sound.play(Sound.MBClick, 0.5f);
        }

        if (GameCanvas.keyPressedz[12] || getCmdPointerLast(GameCanvas.currentScreen.left))
        {
            GameCanvas.keyPressedz[12] = false;
            keyTouch = -1;
            GameCanvas.isPointerJustRelease = false;
            if (ChatTextField.gI().isShow)
            {
                if (ChatTextField.gI().left != null)
                {
                    ChatTextField.gI().left.performAction();
                }
            }
            else if (left != null)
            {
                left.performAction();
            }

            Sound.play(Sound.MBClick, 0.5f);
        }

        if (!GameCanvas.keyPressedz[13] && !getCmdPointerLast(GameCanvas.currentScreen.right))
        {
            return;
        }

        GameCanvas.keyPressedz[13] = false;
        keyTouch = -1;
        GameCanvas.isPointerJustRelease = false;
        Sound.play(Sound.MBClick, 0.5f);
        if (ChatTextField.gI().isShow)
        {
            if (ChatTextField.gI().right != null)
            {
                ChatTextField.gI().right.performAction();
            }
        }
        else if (right != null)
        {
            right.performAction();
        }
    }

    /// <summary>Return command pointer last.</summary>
    /// <param name="cmd">Command.</param>
    public static bool getCmdPointerLast(Command cmd)
    {
        if (cmd == null)
        {
            return false;
        }

        if (cmd.x != 0 && cmd.y != 0)
        {
            return cmd.input();
        }

        if (GameCanvas.currentDialog != null)
        {
            if (GameCanvas.currentDialog.center != null && GameCanvas.isPointerHoldIn(GameCanvas.w - cmdW >> 1, Paint.CmdY - 5, cmdW, Paint.CmdButtonH + 10))
            {
                keyTouch = 1;
                if (cmd == GameCanvas.currentDialog.center && GameCanvas.isPointerClick && GameCanvas.isPointerJustRelease)
                {
                    return true;
                }
            }

            if (GameCanvas.currentDialog.left != null && GameCanvas.isPointerHoldIn(0, Paint.CmdY - 5, cmdW, Paint.CmdButtonH + 10))
            {
                keyTouch = 0;
                if (cmd == GameCanvas.currentDialog.left && GameCanvas.isPointerClick && GameCanvas.isPointerJustRelease)
                {
                    return true;
                }
            }

            if (GameCanvas.currentDialog.right != null && GameCanvas.isPointerHoldIn(GameCanvas.w - cmdW, Paint.CmdY - 5, cmdW, Paint.CmdButtonH + 10))
            {
                keyTouch = 2;
                if ((cmd == GameCanvas.currentDialog.right || cmd == ChatTextField.gI().right) && GameCanvas.isPointerClick && GameCanvas.isPointerJustRelease)
                {
                    return true;
                }
            }
        }
        else
        {
            if (cmd == GameCanvas.currentScreen.left && GameCanvas.isPointerHoldIn(0, Paint.CmdY - 5, cmdW, Paint.CmdButtonH + 10))
            {
                keyTouch = 0;
                if (GameCanvas.isPointerClick && GameCanvas.isPointerJustRelease)
                {
                    return true;
                }
            }

            if (cmd == GameCanvas.currentScreen.right && GameCanvas.isPointerHoldIn(GameCanvas.w - cmdW, Paint.CmdY - 5, cmdW, Paint.CmdButtonH + 10))
            {
                keyTouch = 2;
                if (GameCanvas.isPointerClick && GameCanvas.isPointerJustRelease)
                {
                    return true;
                }
            }

            if ((cmd == GameCanvas.currentScreen.center || ChatPopup.currentMultilineChatPopup != null) && GameCanvas.isPointerHoldIn(GameCanvas.w - cmdW >> 1, Paint.CmdY - 5, cmdW, Paint.CmdButtonH + 10))
            {
                keyTouch = 1;
                if (GameCanvas.isPointerClick && GameCanvas.isPointerJustRelease)
                {
                    return true;
                }
            }
        }

        return false;
    }

    /// <summary>Draw this object&#x27;s current state through the legacy graphics wrapper.</summary>
    /// <param name="g">Legacy drawing context.</param>
    public virtual void paint(mGraphics g)
    {
        g.translate(-g.getTranslateX(), -g.getTranslateY());
        g.setClip(0, 0, GameCanvas.w, GameCanvas.h + 1);
        if (ChatPopup.currentMultilineChatPopup != null)
        {
            GameCanvas.paintz.paintCmdBar(g, null, ChatPopup.currentMultilineChatPopup.cmdNextLine, null);
        }
        else if (ChatTextField.gI().isShow)
        {
            GameCanvas.paintz.paintCmdBar(g, ChatTextField.gI().left, ChatTextField.gI().center, ChatTextField.gI().right);
        }
        else if (GameCanvas.currentDialog == null && !GameCanvas.menu.showMenu)
        {
            GameCanvas.paintz.paintCmdBar(g, left, center, right);
        }
    }
}

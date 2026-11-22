public class ChatPopup : Effect2, IActionListener
{
    public int sayWidth = 100;
    public int delay;
    public int sayRun;
    public string[] says;
    // cx: Character horizontal position in map pixels.
    private int cx;
    // cy: Character vertical position in map pixels.
    private int cy;
    // ch: Ch.
    private int ch;
    // c: Client character being inspected or controlled.
    public Char c;
    private bool outSide;
    private int currentLine;
    private string[] lines;
    public Command cmdNextLine;
    public static ChatPopup currentMultilineChatPopup;
    /// <summary>Add chat popup multi line.</summary>
    /// <param name="chat">Chat.</param>
    /// <param name="howLong">How long.</param>
    /// <param name="c">Client character being inspected or controlled.</param>
    /// <remarks>Locals: array: Working array for this operation.</remarks>
    public static void addChatPopupMultiLine(string chat, int howLong, Char c)
    {
        string[] array = Res.split(chat, "\n", 0);
        if (array.Length == 1)
        {
            addChatPopup(array[0], howLong, c);
            return;
        }

        currentMultilineChatPopup = addChatPopup(array[0], howLong, c);
        currentMultilineChatPopup.currentLine = 0;
        currentMultilineChatPopup.lines = array;
        currentMultilineChatPopup.cmdNextLine = new Command(mResources.NEXT, currentMultilineChatPopup, 8000, null);
    }

    /// <summary>Add chat popup out side.</summary>
    /// <param name="chat">Chat.</param>
    /// <param name="howLong">How long.</param>
    /// <param name="c">Client character being inspected or controlled.</param>
    public static ChatPopup addChatPopupOutSide(string chat, int howLong, Char c)
    {
        ChatPopup chatPopup = new ChatPopup();
        if (chat.Length < 10)
        {
            chatPopup.sayWidth = 64;
        }

        if (GameCanvas.w == 128)
        {
            chatPopup.sayWidth = 128;
        }

        chatPopup.says = mFont.tahoma_7_red.splitFontArray(chat, chatPopup.sayWidth - 10);
        chatPopup.delay = howLong;
        chatPopup.c = c;
        chatPopup.cx = c.cx;
        chatPopup.cy = c.cy;
        c.chatPopup = chatPopup;
        chatPopup.sayRun = 7;
        chatPopup.outSide = true;
        Effect2.vEffect2Outside.addElement(chatPopup);
        return chatPopup;
    }

    /// <summary>Add chat popup.</summary>
    /// <param name="chat">Chat.</param>
    /// <param name="howLong">How long.</param>
    /// <param name="c">Client character being inspected or controlled.</param>
    public static ChatPopup addChatPopup(string chat, int howLong, Char c)
    {
        ChatPopup chatPopup = new ChatPopup();
        if (chat.Length < 10)
        {
            chatPopup.sayWidth = 64;
        }

        if (GameCanvas.w == 128)
        {
            chatPopup.sayWidth = 128;
        }

        chatPopup.says = mFont.tahoma_7_red.splitFontArray(chat, chatPopup.sayWidth - 10);
        chatPopup.delay = howLong;
        chatPopup.c = c;
        chatPopup.cx = c.cx;
        chatPopup.cy = c.cy;
        c.chatPopup = chatPopup;
        chatPopup.sayRun = 7;
        Effect2.vEffect2.addElement(chatPopup);
        return chatPopup;
    }

    /// <summary>Add chat popup.</summary>
    /// <param name="chat">Chat.</param>
    /// <param name="howLong">How long.</param>
    /// <param name="x">Horizontal coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="y">Vertical coordinate in the caller&#x27;s coordinate space.</param>
    public static void addChatPopup(string chat, int howLong, int x, int y)
    {
        ChatPopup chatPopup = new ChatPopup();
        if (chat.Length < 10)
        {
            chatPopup.sayWidth = 60;
        }

        if (GameCanvas.w == 128)
        {
            chatPopup.sayWidth = 128;
        }

        chatPopup.says = mFont.tahoma_7_red.splitFontArray(chat, chatPopup.sayWidth - 10);
        chatPopup.delay = howLong;
        chatPopup.c = null;
        chatPopup.cx = x;
        chatPopup.cy = y;
        chatPopup.sayRun = 7;
        Effect2.vEffect2.addElement(chatPopup);
    }

    /// <summary>Advance this object&#x27;s state for the current client logic tick.</summary>
    public override void update()
    {
        if (c != null)
        {
            cx = c.cx;
            cy = c.cy;
            ch = c.ch + 10;
        }

        if (delay > 0)
        {
            delay--;
        }

        if (sayRun > 1)
        {
            sayRun--;
        }

        if ((c != null && c.chatPopup != null && c.chatPopup != this) || (c != null && c.chatPopup == null) || delay == 0)
        {
            Effect2.vEffect2Outside.removeElement(this);
            Effect2.vEffect2.removeElement(this);
        }
    }

    /// <summary>Draw this object&#x27;s current state through the legacy graphics wrapper.</summary>
    /// <param name="g">Legacy drawing context.</param>
    /// <remarks>Locals: num: Numeric intermediate used by this operation; the legacy name is retained; num2: Numeric intermediate used by this operation; the legacy name is retained; i: Loop/collection index.</remarks>
    public override void paint(mGraphics g)
    {
        int num = cx;
        int num2 = cy;
        if (outSide)
        {
            num -= GameScr.cmx;
            num2 -= GameScr.cmy;
            num2 += 35;
        }

        g.setColor(16777215);
        g.fillRoundRect(num - sayWidth / 2 - 1, num2 - ch - 15 + sayRun - says.Length * 12 - 10, sayWidth + 1, (says.Length + 1) * 12, 12, 12);
        g.setColor(0);
        g.drawRoundRect(num - sayWidth / 2 - 1, num2 - ch - 15 + sayRun - says.Length * 12 - 10, sayWidth + 1, (says.Length + 1) * 12, 12, 12);
        SmallImage.drawSmallImage(g, 941, num - 3, num2 - ch - 15 + sayRun + 2, 0, mGraphics.TOP | mGraphics.HCENTER);
        for (int i = 0; i < says.Length; i++)
        {
            mFont.tahoma_7.drawString(g, says[i], num, num2 - ch - 15 + sayRun + i * 12 - says.Length * 12 - 4, 2);
        }
    }

    /// <summary>Translate the current client key/pointer state into screen actions.</summary>
    public void updateKey()
    {
        if (GameCanvas.keyPressedz[5] || GameCanvas.keyPressedz[20] || mScreen.getCmdPointerLast(GameCanvas.currentScreen.center))
        {
            GameCanvas.keyPressedz[5] = false;
            GameCanvas.keyPressedz[20] = false;
            mScreen.keyTouch = -1;
            cmdNextLine.performAction();
        }

        if (GameCanvas.keyPressedz[5] || mScreen.getCmdPointerLast(GameCanvas.currentScreen.center))
        {
            GameCanvas.keyPressedz[5] = false;
            mScreen.keyTouch = -1;
            cmdNextLine.performAction();
        }
    }

    /// <summary>Draw command in the current graphics context.</summary>
    /// <param name="g">Legacy drawing context.</param>
    public void paintCmd(mGraphics g)
    {
        g.translate(-g.getTranslateX(), -g.getTranslateY());
        g.setClip(0, 0, GameCanvas.w, GameCanvas.h);
        GameCanvas.paintz.paintCmdBar(g, null, cmdNextLine, null);
    }

    /// <summary>Dispatch the selected menu/action identifier to its existing handler.</summary>
    /// <param name="idAction">Selected menu/action identifier.</param>
    /// <param name="p">Retained for call compatibility; not read by this implementation.</param>
    /// <remarks>Locals: num: Numeric intermediate used by this operation; the legacy name is retained.</remarks>
    public void perform(int idAction, object p)
    {
        if (idAction == 8000)
        {
            int num = currentMultilineChatPopup.currentLine;
            num++;
            if (num >= currentMultilineChatPopup.lines.Length)
            {
                currentMultilineChatPopup.c.chatPopup = null;
                currentMultilineChatPopup = null;
                return;
            }

            ChatPopup chatPopup = addChatPopup(currentMultilineChatPopup.lines[num], currentMultilineChatPopup.delay, currentMultilineChatPopup.c);
            chatPopup.currentLine = num;
            chatPopup.lines = currentMultilineChatPopup.lines;
            chatPopup.cmdNextLine = currentMultilineChatPopup.cmdNextLine;
            currentMultilineChatPopup = chatPopup;
        }
    }
}

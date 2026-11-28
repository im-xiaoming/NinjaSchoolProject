public class LanguageScr : mScreen, IActionListener
{
    private int popupW, popupH, popupX, popupY;
    private int indexRow;
    private mScreen returnScreen;
    // read by the Canvas view (LanguageView), which draws this screen's popup and buttons
    public int PopupX { get { return popupX; } }
    public int PopupY { get { return popupY; } }
    public int PopupW { get { return popupW; } }
    public int PopupH { get { return popupH; } }
    public int IndexRow { get { return indexRow; } }
    public string Title { get { return mResources.languageID == mResources.Lang_EN ? "Language" : "Ngôn ngữ"; } }

    /// <summary>A row was clicked in the Canvas view: select it and confirm, as a tap on the row did.</summary>
    /// <param name="row">0 = Tiếng Việt, 1 = English.</param>
    public void ClickRow(int row)
    {
        indexRow = row;
        perform(1000, null);
    }
    /// <summary>Make this screen active and apply its existing entry/reset behavior.</summary>
    public override void switchToMe()
    {
        returnScreen = GameCanvas.currentScreen;
        GameScr.gH = GameCanvas.h;
        GameCanvas.loadBG(GameCanvas.typeBg == 2 ? 0 : TileMap.bgID);
        base.switchToMe();
        if (GameScr.instance != null)
            GameScr.instance = null;
        TileMap.bgID = (sbyte)(mSystem.getCurrentTimeMillis() % 9);
        if (TileMap.bgID == 5 || TileMap.bgID == 6)
            TileMap.bgID = 4;
        GameScr.loadCamera(true);
        GameScr.cmx = 100;
        popupW = 220;
        popupH = 160;
        popupX = GameCanvas.w / 2 - popupW / 2;
        popupY = GameCanvas.h / 2 - popupH / 2;
        if (GameCanvas.h <= 250)
            popupY -= 10;
        center = new Command(mResources.languageID == mResources.Lang_EN ? "Select" : "Chọn", this, 1000, null);
        right = new Command(mResources.languageID == mResources.Lang_EN ? "Back" : "Quay lại", this, 1001, null);
        indexRow = loadLanguageID() == mResources.Lang_EN ? 1 : 0;
        if (GameCanvas.isTouch && GameCanvas.w >= 320)
        {
            center.x = GameCanvas.w / 2 - 35;
            right.x = GameCanvas.w / 2 + 88;
            center.y = right.y = Paint.CmdY;
        }
    }

    /// <summary>Draw this object&#x27;s current state through the legacy graphics wrapper.</summary>
    /// <param name="g">Legacy drawing context.</param>
    /// <remarks>Only the background is drawn here; the popup, its rows and the buttons are the Canvas view LanguageView.</remarks>
    public override void paint(mGraphics g)
    {
        g.setColor(0);
        g.fillRect(0, 0, GameCanvas.w, GameCanvas.h);
        GameCanvas.paintBGGameScr(g);
    }

    /// <summary>Save language identifier.</summary>
    /// <param name="languageID">Language identifier.</param>
    public static void saveLanguageID(int languageID)
    {
        RMS.saveRMSInt("indLanguage", languageID);
    }

    /// <summary>Load language identifier.</summary>
    public static int loadLanguageID()
    {
        return RMS.loadRMSInt("indLanguage");
    }

    /// <summary>Advance this object&#x27;s state for the current client logic tick.</summary>
    public override void update()
    {
        GameScr.cmx++;
        if (GameScr.cmx > GameCanvas.w * 3 + 100)
            GameScr.cmx = 100;
        base.update();
    }

    /// <summary>Translate the current client key/pointer state into screen actions.</summary>
    public override void updateKey()
    {
        if (GameCanvas.keyPressedz[Key.NUM2] || GameCanvas.keyPressedz[Key.NUM8] || GameCanvas.keyPressedz[24] || GameCanvas.keyPressedz[25])
        {
            indexRow = indexRow == 0 ? 1 : 0;
        }

        base.updateKey();
    }

    /// <summary>Return to previous screen.</summary>
    private void ReturnToPreviousScreen()
    {
        if (returnScreen == null || returnScreen is SplashScr)
        {
            SplashScr.gI().switchToMe(); // first start: the language is chosen, enter the world
            WorldEntry.Begin();
        }
        else
            returnScreen.switchToMe();
    }

    /// <summary>Dispatch the selected menu/action identifier to its existing handler.</summary>
    /// <param name="idAction">Selected menu/action identifier.</param>
    /// <param name="p">Retained for call compatibility; not read by this implementation.</param>
    public void perform(int idAction, object p)
    {
        if (idAction == 1001)
        {
            ReturnToPreviousScreen();
            return;
        }

        if (idAction == 1000)
        {
            int lang = indexRow == 1 ? mResources.Lang_EN : mResources.Lang_VI;
            saveLanguageID(lang);
            mResources.loadLanguage(lang);
            GameMidlet.isWorldver = lang == mResources.Lang_EN;
            ReturnToPreviousScreen();
        }
    }
}

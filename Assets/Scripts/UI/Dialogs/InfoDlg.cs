public class InfoDlg
{
    public static bool isShow;
    private static string title;
    private static string subtitke;
    public static int delay;
    public static bool isLock;
    /// <summary>Show info dlg.</summary>
    /// <param name="title">Title.</param>
    /// <param name="subtitle">Subtitle.</param>
    /// <param name="delay">Delay.</param>
    public static void show(string title, string subtitle, int delay)
    {
        if (title != null)
        {
            isShow = true;
            InfoDlg.title = title;
            subtitke = subtitle;
            InfoDlg.delay = delay;
        }
    }

    /// <summary>Show wait.</summary>
    public static void showWait()
    {
        show(mResources.PLEASEWAIT, null, 5000);
        isLock = true;
    }

    /// <summary>Show wait.</summary>
    /// <param name="str">Text.</param>
    public static void showWait(string str)
    {
        show(str, null, 5000);
        isLock = true;
    }

    /// <summary>Draw this object&#x27;s current state through the legacy graphics wrapper.</summary>
    /// <param name="g">Legacy drawing context.</param>
    /// <remarks>Locals: num: Numeric intermediate used by this operation; the legacy name is retained.</remarks>
    public static void paint(mGraphics g)
    {
        string mapName = title;
        if (TileMap.mapName1 != null)
        {
            mapName = TileMap.mapName1;
        }

        if (isShow && (!isLock || delay <= 4990) && !GameScr.isPaintAlert)
        {
            int num = 10;
            GameCanvas.paintz.paintFrame(GameCanvas.hw - 64, num, 128, 40, g);
            if (isLock)
            {
                GameCanvas.paintShukiren(GameCanvas.hw - mFont.tahoma_8b.getWidth(mapName) / 2 - 10, num + 20, g, false);
                mFont.tahoma_8b.drawString(g, mapName, GameCanvas.hw + 5, num + 13, 2);
            }
            else if (subtitke != null)
            {
                mFont.tahoma_8b.drawString(g, mapName, GameCanvas.hw, num + 8, 2);
                mFont.tahoma_7_white.drawString(g, subtitke, GameCanvas.hw, num + 22, 2);
            }
            else
            {
                mFont.tahoma_8b.drawString(g, mapName, GameCanvas.hw, num + 13, 2);
            }
        }
    }

    /// <summary>Advance this object&#x27;s state for the current client logic tick.</summary>
    public static void update()
    {
        if (delay > 0)
        {
            delay--;
            if (delay == 0)
            {
                hide();
            }
        }
    }

    /// <summary>Hide info dlg.</summary>
    public static void hide()
    {
        title = string.Empty;
        subtitke = null;
        isLock = false;
        delay = 0;
        isShow = false;
    }
}

public class SplashScr : mScreen
{
    public static int splashScrStat;
    public static SplashScr instance;
    public Image imgTitle = GameCanvas.loadImage("/tt");
    /// <summary>Initialize splash screen state and its supplied dependencies.</summary>
    public SplashScr()
    {
        instance = this;
    }

    /// <summary>Return the shared instance, creating it on first access when needed.</summary>
    public static SplashScr gI()
    {
        return (instance != null) ? instance : (instance = new SplashScr());
    }

    /// <summary>Make this screen active and apply its existing entry/reset behavior.</summary>
    public override void switchToMe()
    {
        base.switchToMe();
    }

    /// <summary>Load splash screen.</summary>
    public static void loadSplashScr()
    {
        splashScrStat = 0;
    }

    /// <summary>Advance this object&#x27;s state for the current client logic tick.</summary>
    public override void update()
    {
        // once: the splash stays on screen behind the wait dialog while the world loads
        if (splashScrStat == 6)
        {
            GameCanvas.instance.initGameCanvas();
            if (LanguageScr.loadLanguageID() >= 0)
                WorldEntry.Begin();
            else
                GameCanvas.languageScr.switchToMe();
        }

        splashScrStat++;
    }

    /// <summary>Draw this object&#x27;s current state through the legacy graphics wrapper.</summary>
    /// <param name="g">Legacy drawing context.</param>
    public override void paint(mGraphics g)
    {
        g.setColor(0);
        g.fillRect(0, 0, GameCanvas.w, GameCanvas.h);
        if (Main.isPC && imgTitle != null)
        {
            g.drawImage(imgTitle, GameCanvas.hw, GameCanvas.h / 2, 3);
        }
    }
}

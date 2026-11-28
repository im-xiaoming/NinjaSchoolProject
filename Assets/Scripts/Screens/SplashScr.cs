public class SplashScr : mScreen
{
    public static int splashScrStat;
    public static SplashScr instance;
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
    /// <remarks>The splash (black screen and title image) is the Canvas view SplashView.</remarks>
    public override void paint(mGraphics g)
    {
    }
}

// Offline deviation (user request): no login, title, server, character select or character create screen.
// The game connects to the embedded server right after the splash; the server signs the one local account in and
// makes a character on the first start (NsoServer.Net.Controller.EnsureCharacter). When the character list arrives
// the client selects the first character itself (Controller.Movement, message -126) and enters the world.
public static class WorldEntry
{
    /// <summary>Address of the embedded server (only used as labels: GameServer.Connect is an in-memory pipe).</summary>
    public const string Host = "127.0.0.1";
    public const int Port = 14444;

    /// <summary>Loads the saved language (Vietnamese unless English was chosen), as the removed title screen did.</summary>
    /// <remarks>Locals: lang: Saved language id, -1 when none was chosen yet.</remarks>
    public static void LoadLanguage()
    {
        int lang = LanguageScr.loadLanguageID();
        mResources.loadLanguage(lang == mResources.Lang_EN ? mResources.Lang_EN : mResources.Lang_VI);
    }

    /// <summary>Connects to the embedded server and starts the automatic sign-in / character select.</summary>
    public static void Begin()
    {
        if (Session_ME.gI().isConnected())
            Session_ME.gI().close();
        LoadLanguage();
        GameCanvas.startWaitDlgWithoutCancel();
        GameCanvas.connect(4);
        Service.gI().setClientType(); // the embedded server signs the local player in on its own
    }
}

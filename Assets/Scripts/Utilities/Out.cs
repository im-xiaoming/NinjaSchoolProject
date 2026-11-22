public class Out
{
    /// <summary>Retain the println compatibility hook without performing work.</summary>
    /// <param name="s">Retained for call compatibility; not read by this implementation.</param>
    public static void println(string s)
    {
    }

    /// <summary>Retain the log compatibility hook without performing work.</summary>
    /// <param name="str">Retained for call compatibility; not read by this implementation.</param>
    public static void Log(string str)
    {
    }

    /// <summary>Retain the log error compatibility hook without performing work.</summary>
    /// <param name="str">Retained for call compatibility; not read by this implementation.</param>
    public static void LogError(string str)
    {
    }

    /// <summary>Retain the log warning compatibility hook without performing work.</summary>
    /// <param name="str">Retained for call compatibility; not read by this implementation.</param>
    public static void LogWarning(string str)
    {
    }
}

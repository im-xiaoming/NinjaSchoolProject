using System;

public class Timer
{
    public static IActionListener timeListener;
    public static int idAction;
    public static long timeExecute;
    public static bool isON;
    /// <summary>Set timer.</summary>
    /// <param name="actionListener">Action listener.</param>
    /// <param name="action">Action.</param>
    /// <param name="timeEllapse">Time ellapse.</param>
    public static void setTimer(IActionListener actionListener, int action, long timeEllapse)
    {
        timeListener = actionListener;
        idAction = action;
        timeExecute = Environment.TickCount + timeEllapse;
        isON = true;
    }

    /// <summary>Advance this object&#x27;s state for the current client logic tick.</summary>
    /// <remarks>Locals: num: Timestamp used by this timer.</remarks>
    public static void update()
    {
        long num = Environment.TickCount;
        if (!isON || num <= timeExecute)
        {
            return;
        }

        isON = false;
        try
        {
            if (idAction > 0)
            {
                GameScr.gI().perform(idAction, null);
            }
        }
        catch (Exception ex)
        {
            Out.println(ex.Message + ex.StackTrace);
        }
    }
}

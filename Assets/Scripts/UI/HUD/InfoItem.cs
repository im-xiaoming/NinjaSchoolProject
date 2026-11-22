public class InfoItem
{
    // s: Source text or serialized input.
    public string s;
    // f: Current field/value.
    public mFont f;
    public int speed;
    /// <summary>Initialize info item state and its supplied dependencies.</summary>
    /// <param name="s">Source text or serialized input.</param>
    public InfoItem(string s)
    {
        f = mFont.tahoma_7_white;
        this.s = s;
        speed = 20;
    }

    /// <summary>Initialize info item state and its supplied dependencies.</summary>
    /// <param name="s">Source text or serialized input.</param>
    /// <param name="f">Current field/value.</param>
    /// <param name="speed">Speed.</param>
    public InfoItem(string s, mFont f, int speed)
    {
        this.f = f;
        this.s = s;
        this.speed = speed;
    }
}

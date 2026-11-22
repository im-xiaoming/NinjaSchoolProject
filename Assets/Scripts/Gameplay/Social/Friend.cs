public class Friend
{
    public string friendName;
    public sbyte type;
    /// <summary>Initialize friend state and its supplied dependencies.</summary>
    /// <param name="friendName">Friend name.</param>
    /// <param name="type">Type.</param>
    public Friend(string friendName, sbyte type)
    {
        this.friendName = friendName;
        this.type = type;
    }
}

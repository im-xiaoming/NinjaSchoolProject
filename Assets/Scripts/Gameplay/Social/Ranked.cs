public class Ranked
{
    // id: Identifier.
    public int id;
    public int ranked;
    public string name;
    public string stt;
    /// <summary>Initialize ranked state and its supplied dependencies.</summary>
    public Ranked()
    {
    }

    /// <summary>Initialize ranked state and its supplied dependencies.</summary>
    /// <param name="id">Identifier.</param>
    /// <param name="name">Name.</param>
    /// <param name="ranked">Ranked.</param>
    /// <param name="stt">Stt.</param>
    public Ranked(int id, string name, int ranked, string stt)
    {
        this.id = id;
        this.name = name;
        this.ranked = ranked;
        this.stt = stt;
    }
}

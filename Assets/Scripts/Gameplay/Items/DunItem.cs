public class DunItem
{
    // id: Identifier.
    public int id;
    public string name1;
    public string name2;
    /// <summary>Initialize dungeon item state and its supplied dependencies.</summary>
    public DunItem()
    {
    }

    /// <summary>Initialize dungeon item state and its supplied dependencies.</summary>
    /// <param name="id">Identifier.</param>
    /// <param name="name1">Name1.</param>
    /// <param name="name2">Name2.</param>
    public DunItem(int id, string name1, string name2)
    {
        this.id = id;
        this.name1 = name1;
        this.name2 = name2;
    }
}

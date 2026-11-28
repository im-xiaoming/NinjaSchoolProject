public class ItemTemplates
{
    public static mHashtable itemTemplates = new mHashtable();
    /// <summary>Add item templates.</summary>
    /// <param name="it">Current item/iterator entry.</param>
    public static void add(ItemTemplate it)
    {
        itemTemplates.put(it.id, it);
    }

    /// <summary>Return item templates.</summary>
    /// <param name="id">Identifier.</param>
    public static ItemTemplate get(short id)
    {
        return (ItemTemplate)itemTemplates.get(id);
    }

    /// <summary>Return icon.</summary>
    /// <param name="itemTemplateID">Item template identifier.</param>
    public static short getIcon(short itemTemplateID)
    {
        return get(itemTemplateID).iconID;
    }
}

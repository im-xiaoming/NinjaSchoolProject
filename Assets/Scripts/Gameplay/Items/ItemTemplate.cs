public class ItemTemplate
{
    // id: Identifier.
    public short id;
    public sbyte type;
    public sbyte gender;
    public string name;
    public string description;
    public sbyte level;
    public short iconID;
    public short part;
    public bool isUpToUp;
    // w: Width or horizontal span.
    public int w;
    // h: Height or vertical span.
    public int h;
    /// <summary>Initialize item template state and its supplied dependencies.</summary>
    /// <param name="templateID">Template identifier.</param>
    /// <param name="type">Type.</param>
    /// <param name="gender">Gender.</param>
    /// <param name="name">Name.</param>
    /// <param name="description">Description.</param>
    /// <param name="level">Level.</param>
    /// <param name="iconID">Icon identifier.</param>
    /// <param name="part">Part.</param>
    /// <param name="isUpToUp">Is up to up.</param>
    public ItemTemplate(short templateID, sbyte type, sbyte gender, string name, string description, sbyte level, short iconID, short part, bool isUpToUp)
    {
        id = templateID;
        this.type = type;
        this.gender = gender;
        this.name = name;
        this.description = description;
        this.level = level;
        this.iconID = iconID;
        this.part = part;
        this.isUpToUp = isUpToUp;
    }

    /// <summary>Check whether vat pham nhiem vu applies to the supplied state.</summary>
    public bool isVatPhamNhiemVu()
    {
        if (type >= 25)
        {
            return type <= 28;
        }

        return false;
    }
}

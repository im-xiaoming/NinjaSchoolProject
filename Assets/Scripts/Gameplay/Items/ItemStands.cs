public class ItemStands
{
    public Item item;
    public int price;
    public int timeEnd;
    public int timeStart;
    public string seller = string.Empty;
    /// <summary>Initialize item stands state and its supplied dependencies.</summary>
    /// <param name="item">Item instance being inspected or transferred.</param>
    /// <param name="seller">Seller.</param>
    /// <param name="price">Price.</param>
    public ItemStands(Item item, string seller, int price)
    {
        this.item = item;
        this.seller = seller;
        this.price = price;
    }

    /// <summary>Initialize item stands state and its supplied dependencies.</summary>
    public ItemStands()
    {
    }
}

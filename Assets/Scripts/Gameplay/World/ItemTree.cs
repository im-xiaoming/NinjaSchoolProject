public class ItemTree
{
    public int idTree;
    public int xTree;
    public int yTree;
    /// <summary>Initialize item tree state and its supplied dependencies.</summary>
    /// <param name="x">Horizontal coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="y">Vertical coordinate in the caller&#x27;s coordinate space.</param>
    public ItemTree(int x, int y)
    {
        xTree = x * 24 + 12;
        yTree = y * 24 + 24 + 2;
    }

    /// <summary>Return image from hash.</summary>
    public object getImageFromHash()
    {
        object obj = TileMap.itemMap.get(idTree + string.Empty);
        if (obj != null)
        {
            return obj;
        }

        return null;
    }

    /// <summary>Draw this object&#x27;s current state through the legacy graphics wrapper.</summary>
    /// <param name="g">Legacy drawing context.</param>
    public void paint(mGraphics g)
    {
        if (getImageFromHash() != null)
        {
            g.drawImage((Image)getImageFromHash(), xTree, yTree, mGraphics.BOTTOM | mGraphics.HCENTER);
        }
    }
}

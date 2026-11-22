public class ImageIcon
{
    // img: Image.
    public Image img;
    // id: Identifier.
    public short id;
    public int timeRemove;
    public bool isLoad;
    public long timeGetBack;
    /// <summary>Reset image icon.</summary>
    public void reset()
    {
        img = null;
    }

    /// <summary>Return height.</summary>
    public int getHeight()
    {
        if (img != null)
        {
            return img.getHeight();
        }

        return 0;
    }

    /// <summary>Return width.</summary>
    public int getWidth()
    {
        if (img != null)
        {
            return img.getWidth();
        }

        return 0;
    }
}

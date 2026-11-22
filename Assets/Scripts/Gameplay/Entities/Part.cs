public class Part
{
    public int type;
    // pi: Pi.
    public PartImage[] pi;
    /// <summary>Initialize part state and its supplied dependencies.</summary>
    /// <param name="type">Type.</param>
    public Part(int type)
    {
        this.type = type;
        if (type == 0)
        {
            pi = new PartImage[8];
        }

        if (type == 1)
        {
            pi = new PartImage[18];
        }

        if (type == 2)
        {
            pi = new PartImage[10];
        }

        if (type == 3)
        {
            pi = new PartImage[2];
        }
    }
}

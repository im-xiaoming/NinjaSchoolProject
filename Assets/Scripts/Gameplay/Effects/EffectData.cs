public class EffectData
{
    public sbyte[] data;
    public long timeremove;
    public MyVector listFrame = new MyVector();
    public MyVector listAnima = new MyVector();
    public SmallImage[] smallImage;
    public sbyte[][] frameChar = new sbyte[4][];
    public sbyte[] sequence;
    // fw: Fw.
    public int fw;
    // fh: Fh.
    public int fh;
    public sbyte[] indexStartSkill;
    public bool isLoad;
    /// <summary>Initialize effect data state and its supplied dependencies.</summary>
    public EffectData()
    {
    }

    /// <summary>Initialize effect data state and its supplied dependencies.</summary>
    /// <param name="id">Retained for call compatibility; not read by this implementation.</param>
    public EffectData(short id)
    {
    }

    /// <summary>Perform setdata.</summary>
    /// <param name="data">Data.</param>
    public void setdata(sbyte[] data)
    {
        if (data != null)
        {
            this.data = data;
        }
    }
}

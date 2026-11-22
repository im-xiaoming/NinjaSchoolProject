public class MobTemplate
{
    public sbyte rangeMove;
    public sbyte speed;
    public sbyte type;
    public sbyte typeFly;
    public short mobTemplateId;
    // hp: Current hit points.
    public int hp;
    public string name;
    // imgs: Images.
    public Image[] imgs;
    public ImageInfo[] imginfo;
    public Frame[] frameBoss;
    public sbyte[] frameBossMove;
    public sbyte[][] frameBossAttack;
    public sbyte[][] frameChar = new sbyte[4][];
    public sbyte[] sequence;
    public sbyte[] indexSplash = new sbyte[4];
    /// <summary>Return image info.</summary>
    /// <param name="frame">Frame.</param>
    public ImageInfo getImgInfo(sbyte frame)
    {
        return imginfo[frame];
    }
}

public class EffAtutoTemp
{
    public short idEffAuto;
    public ImageInfo[] imginfo;
    public Frame[] frameEffAuto;
    public short[] frameRunning;
    public long timerequest;
    public long timeUse = mSystem.currentTimeMillis();
    /// <summary>Return image info.</summary>
    /// <param name="frame">Frame.</param>
    public ImageInfo getImgInfo(sbyte frame)
    {
        return imginfo[frame];
    }
}

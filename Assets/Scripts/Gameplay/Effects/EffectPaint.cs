public class EffectPaint
{
    public int index;
    public Mob eMob;
    public Char eChar;
    public EffectCharPaint effCharPaint;
    public bool isFly;
    /// <summary>Return image identifier.</summary>
    public int getImgId()
    {
        return effCharPaint.arrEfInfo[index].idImg;
    }
}

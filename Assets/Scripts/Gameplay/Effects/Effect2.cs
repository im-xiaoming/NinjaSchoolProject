public abstract class Effect2
{
    public static MyVector vEffect2 = new MyVector();
    public static MyVector vRemoveEffect2 = new MyVector();
    public static MyVector vEffect2Outside = new MyVector();
    public static MyVector vAnimateEffect = new MyVector();
    /// <summary>Advance this object&#x27;s state for the current client logic tick.</summary>
    public virtual void update()
    {
    }

    /// <summary>Draw this object&#x27;s current state through the legacy graphics wrapper.</summary>
    /// <param name="g">Retained for call compatibility; not read by this implementation.</param>
    public virtual void paint(mGraphics g)
    {
    }
}

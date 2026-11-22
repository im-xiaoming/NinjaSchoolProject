public class Layer
{
    public ActionUpdate acUpdate;
    public ActionPaint acPaint;
    /// <summary>Advance this object&#x27;s state for the current client logic tick.</summary>
    public void update()
    {
        if (acUpdate != null)
        {
            acUpdate();
        }
    }

    /// <summary>Draw this object&#x27;s current state through the legacy graphics wrapper.</summary>
    /// <param name="g">Legacy drawing context.</param>
    /// <param name="x">Horizontal coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="y">Vertical coordinate in the caller&#x27;s coordinate space.</param>
    public void paint(mGraphics g, int x, int y)
    {
        if (acPaint != null)
        {
            acPaint(g, x, y);
        }
    }

    /// <summary>Handle a key action using this control&#x27;s existing key mapping.</summary>
    /// <param name="keyCode">Retained for call compatibility; not read by this implementation.</param>
    public void keyPress(int keyCode)
    {
    }
}

public class Position
{
    // x: Horizontal coordinate in the caller's coordinate space.
    public int x;
    // y: Vertical coordinate in the caller's coordinate space.
    public int y;
    public int anchor;
    // g: Drawing context.
    public int g;
    // v: Value being inspected or transformed.
    public int v;
    // w: Width or horizontal span.
    public int w;
    // h: Height or vertical span.
    public int h;
    public int color;
    public int limitY;
    public Layer layer;
    public short yTo;
    public short xTo;
    public short distant;
    /// <summary>Initialize position state and its supplied dependencies.</summary>
    public Position()
    {
        x = 0;
        y = 0;
    }

    /// <summary>Initialize position state and its supplied dependencies.</summary>
    /// <param name="x">Horizontal coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="y">Vertical coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="anchor">Anchor.</param>
    public Position(int x, int y, int anchor)
    {
        this.x = x;
        this.y = y;
        this.anchor = anchor;
    }

    /// <summary>Initialize position state and its supplied dependencies.</summary>
    /// <param name="x">Horizontal coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="y">Vertical coordinate in the caller&#x27;s coordinate space.</param>
    public Position(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    /// <summary>Compute translate.</summary>
    public int translate()
    {
        if (x == xTo && y == yTo)
        {
            return -1;
        }

        if (Math.abs((xTo - x) / 2) <= 1 && Math.abs((yTo - y) / 2) <= 1)
        {
            x = xTo;
            y = yTo;
            return 0;
        }

        if (x != xTo)
        {
            x += (xTo - x) / 2;
        }

        if (y != yTo)
        {
            y += (yTo - y) / 2;
        }

        if (Res.distance(x, y, xTo, yTo) <= distant / 5)
        {
            return 2;
        }

        return 1;
    }

    /// <summary>Advance this object&#x27;s state for the current client logic tick.</summary>
    public void update()
    {
        if (layer != null)
        {
            layer.update();
        }

        Out.println("vao update ko");
    }

    /// <summary>Draw this object&#x27;s current state through the legacy graphics wrapper.</summary>
    /// <param name="g">Legacy drawing context.</param>
    public void paint(mGraphics g)
    {
        if (layer != null)
        {
            layer.paint(g, x, y);
        }
    }
}

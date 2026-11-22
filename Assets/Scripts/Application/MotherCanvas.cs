public class MotherCanvas
{
    public static MotherCanvas instance;
    public GameCanvas tCanvas;
    public int zoomLevel = 1;
    public Image imgCache;
    // w: Width or horizontal span.
    public static int w;
    // h: Height or vertical span.
    public static int h;
    private int newWidth;
    private int newHeight;
    private int OUTPUTSIZE = 20;
    public static int addR;
    public static int addL;
    /// <summary>Initialize mother canvas state and its supplied dependencies.</summary>
    public MotherCanvas()
    {
        mSystem.setScale();
        w = getWidth();
        h = getHeight();
        if (Main.isPC)
        {
            // whole-number scale chosen from the screen height so pixel art stays sharp (art for x1..x4 exists)
            zoomLevel = w * h < 480000 ? 1 : h >= 1000 ? 4 : h >= 700 ? 3 : 2;
            if (NsoGameControl.UiZoomOverride >= 1 && NsoGameControl.UiZoomOverride <= 4)
            {
                zoomLevel = NsoGameControl.UiZoomOverride; // set on the "Ninja School Game Control" object
            }

            mGraphics.zoomLevel = zoomLevel;
            newWidth = w / zoomLevel + 2;
            newHeight = h / zoomLevel + 2;
            Main.main.doClearRMS();
        }
        else if (w * h >= 1228800)
        {
            mGraphics.zoomLevel = (zoomLevel = 4);
            newWidth = w / zoomLevel + 2;
            newHeight = h / zoomLevel + 2;
        }
        else if (w * h >= 691200)
        {
            mGraphics.zoomLevel = (zoomLevel = 3);
            newWidth = w / zoomLevel + 2;
            newHeight = h / zoomLevel + 2;
        }
        else if (w * h >= 240000)
        {
            mGraphics.zoomLevel = (zoomLevel = 2);
            newWidth = w / zoomLevel + 2;
            newHeight = h / zoomLevel + 2;
        }
        else
        {
            mGraphics.zoomLevel = (zoomLevel = 1);
        }

        if (mGraphics.zoomLevel == 3)
        {
            addR = 1;
            addL = 1;
        }
        else if (mGraphics.zoomLevel == 2)
        {
            addR = 0;
            addL = 1;
        }
    }

    /// <summary>Return width.</summary>
    public int getWidth()
    {
        return (int)ScaleGUI.WIDTH;
    }

    /// <summary>Return height.</summary>
    public int getHeight()
    {
        return (int)ScaleGUI.HEIGHT;
    }

    /// <summary>Set child canvas.</summary>
    /// <param name="tCanvas">T canvas.</param>
    public void setChildCanvas(GameCanvas tCanvas)
    {
        this.tCanvas = tCanvas;
    }

    /// <summary>Draw this object&#x27;s current state through the legacy graphics wrapper.</summary>
    /// <param name="g">Legacy drawing context.</param>
    protected void paint(mGraphics g)
    {
        tCanvas.paint(g);
    }

    /// <summary>Handle a key-down event using the client&#x27;s key-code mapping.</summary>
    /// <param name="keyCode">Client key code; this is not a Unity KeyCode value.</param>
    protected void keyPressed(int keyCode)
    {
        tCanvas.keyPressed(keyCode);
    }

    /// <summary>Handle a key-up event using the client&#x27;s key-code mapping.</summary>
    /// <param name="keyCode">Client key code; this is not a Unity KeyCode value.</param>
    protected void keyReleased(int keyCode)
    {
        tCanvas.keyReleased(keyCode);
    }

    /// <summary>Handle pointer movement while the pointer is held down.</summary>
    /// <param name="x">Horizontal coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="y">Vertical coordinate in the caller&#x27;s coordinate space.</param>
    protected void pointerDragged(int x, int y)
    {
        x /= zoomLevel;
        y /= zoomLevel;
        tCanvas.pointerDragged(x, y);
    }

    /// <summary>Handle a pointer-down event in the client&#x27;s logical coordinate space.</summary>
    /// <param name="x">Horizontal coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="y">Vertical coordinate in the caller&#x27;s coordinate space.</param>
    protected void pointerPressed(int x, int y)
    {
        x /= zoomLevel;
        y /= zoomLevel;
        tCanvas.pointerPressed(x, y);
    }

    /// <summary>Handle a pointer-up event in the client&#x27;s logical coordinate space.</summary>
    /// <param name="x">Horizontal coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="y">Vertical coordinate in the caller&#x27;s coordinate space.</param>
    protected void pointerReleased(int x, int y)
    {
        x /= zoomLevel;
        y /= zoomLevel;
        tCanvas.pointerReleased(x, y);
    }

    /// <summary>Return height new.</summary>
    public int getHeight_new()
    {
        if (zoomLevel == 1)
        {
            return getHeight();
        }

        return newHeight;
    }

    /// <summary>Return width new.</summary>
    public int getWidth_new()
    {
        if (zoomLevel == 1)
        {
            return getWidth();
        }

        return newWidth;
    }
}

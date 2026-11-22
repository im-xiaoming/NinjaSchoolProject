public class FrameImage
{
    public int frameWidth;
    public int frameHeight;
    public int nFrame;
    public Image imgFrame;
    // pos: Position.
    private int[] pos;
    private int totalHeight;
    private Image[] imgList;
    private bool isRotate;
    /// <summary>Initialize frame image state and its supplied dependencies.</summary>
    /// <param name="img">Image.</param>
    /// <param name="width">Width.</param>
    /// <param name="height">Height.</param>
    /// <remarks>Locals: i: Loop/collection index.</remarks>
    public FrameImage(Image img, int width, int height)
    {
        imgFrame = img;
        frameWidth = width;
        frameHeight = height;
        totalHeight = Image.getImageHeight(img);
        nFrame = totalHeight / height;
        pos = new int[nFrame];
        for (int i = 0; i < nFrame; i++)
        {
            pos[i] = i * height;
        }
    }

    /// <summary>Initialize frame image state and its supplied dependencies.</summary>
    /// <param name="img">Image.</param>
    /// <param name="n">Number of entries or iterations.</param>
    /// <remarks>Locals: num: Numeric intermediate used by this operation; the legacy name is retained; i: Loop/collection index.</remarks>
    public FrameImage(Image img, int n)
    {
        imgFrame = img;
        frameWidth = img.getWidth();
        frameHeight = img.getHeight();
        nFrame = n;
        imgList = new Image[nFrame];
        isRotate = true;
        int num = 360 / n;
        for (int i = 1; i < nFrame; i++)
        {
            switch (i * num)
            {
            }
        }
    }

    /// <summary>Draw region frame in the current graphics context.</summary>
    /// <param name="idx">Index.</param>
    /// <param name="x">Horizontal coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="y">Vertical coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="trans">Trans.</param>
    /// <param name="anchor">Anchor.</param>
    /// <param name="g">Legacy drawing context.</param>
    /// <param name="bulletID">Bullet identifier.</param>
    /// <param name="n">Number of entries or iterations.</param>
    public void drawRegionFrame(int idx, int x, int y, int trans, int anchor, mGraphics g, int bulletID, int n)
    {
        g.drawRegion(imgFrame, 0, (bulletID * n + idx) * imgFrame.getWidth(), imgFrame.getWidth(), imgFrame.getWidth(), trans, x, y, anchor);
    }

    /// <summary>Draw frame in the current graphics context.</summary>
    /// <param name="idx">Index.</param>
    /// <param name="x">Horizontal coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="y">Vertical coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="trans">Trans.</param>
    /// <param name="anchor">Anchor.</param>
    /// <param name="g">Legacy drawing context.</param>
    public void drawFrame(int idx, int x, int y, int trans, int anchor, mGraphics g)
    {
        if (!isRotate)
        {
            if (idx >= 0 && idx < nFrame)
            {
                g.drawRegion(imgFrame, 0, pos[idx], frameWidth, frameHeight, trans, x, y, anchor);
            }
        }
        else if (idx >= 0 && idx < nFrame)
        {
            switch (idx * (360 / nFrame))
            {
                case 0:
                    g.drawImage(imgFrame, x, y, 3);
                    break;
                case 90:
                    g.drawRegion(imgFrame, 0, 0, frameWidth, frameHeight, 5, x, y, anchor);
                    break;
                case 270:
                    g.drawRegion(imgFrame, 0, 0, frameWidth, frameHeight, 6, x, y, anchor);
                    break;
                case 180:
                    g.drawRegion(imgFrame, 0, 0, frameWidth, frameHeight, 3, x, y, anchor);
                    break;
                default:
                    g.drawRegion(imgList[idx], 0, 0, frameWidth, frameHeight, trans, x, y, anchor);
                    break;
            }
        }
    }

    /// <summary>Perform unload.</summary>
    public void unload()
    {
        imgFrame = null;
        pos = null;
    }
}

using System.Collections;
using UnityEngine;

public class mGraphics
{
    public static int HCENTER = 1;
    public static int VCENTER = 2;
    public static int LEFT = 4;
    public static int RIGHT = 8;
    public static int TOP = 16;
    public static int BOTTOM = 32;
    // r: Result being assembled or returned.
    private float r;
    // g: Drawing context.
    private float g;
    // b: Buffer or intermediate output value.
    private float b;
    // a: First operand or current actor.
    private float a;
    public int clipX;
    public int clipY;
    public int clipW;
    public int clipH;
    private bool isClip;
    private bool isTranslate = true;
    private int translateX;
    private int translateY;
    public static int zoomLevel = 1;
    public const int BASELINE = 64;
    public const int SOLID = 0;
    public const int DOTTED = 1;
    public const int TRANS_MIRROR = 2;
    public const int TRANS_MIRROR_ROT180 = 1;
    public const int TRANS_MIRROR_ROT270 = 4;
    public const int TRANS_MIRROR_ROT90 = 7;
    public const int TRANS_NONE = 0;
    public const int TRANS_ROT180 = 3;
    public const int TRANS_ROT270 = 6;
    public const int TRANS_ROT90 = 5;
    public static Hashtable cachedTextures = new Hashtable();
    private int clipTX;
    private int clipTY;
    private int currentBGColor;
    // pos: Position.
    private Vector2 pos = new Vector2(0f, 0f);
    // rect: Rectangle used by this layout/drawing calculation.
    private Rect rect;
    private Matrix4x4 matrixBackup;
    private Vector2 pivot;
    public Vector2 size = new Vector2(128f, 128f);
    public Vector2 relativePosition = new Vector2(0f, 0f);
    /// <summary>Perform cache.</summary>
    /// <param name="key">Key.</param>
    /// <param name="value">Value.</param>
    private void cache(string key, Texture value)
    {
        if (cachedTextures.Count > 400)
        {
            cachedTextures.Clear();
        }

        if (value.width * value.height < GameCanvas.w * GameCanvas.h)
        {
            cachedTextures.Add(key, value);
        }
    }

    /// <summary>Perform translate.</summary>
    /// <param name="tx">Tx.</param>
    /// <param name="ty">Ty.</param>
    public void translate(int tx, int ty)
    {
        if (tx == -getTranslateX() && ty == -getTranslateY())
            ClearScrollPaintOffset();
        tx *= zoomLevel;
        ty *= zoomLevel;
        translateX += tx;
        translateY += ty;
        isTranslate = true;
        if (translateX == 0 && translateY == 0)
        {
            isTranslate = false;
        }
    }

    // True when the mouse is over this rectangle (game pixels, in the current translation). Used for hover highlights.
    /// <summary>Check whether mouse over applies to the supplied state.</summary>
    /// <param name="x">Horizontal coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="y">Vertical coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="w">Width or horizontal span.</param>
    /// <param name="h">Height or vertical span.</param>
    /// <remarks>Locals: m: Current message or map entry; mx: Horizontal position/difference used by this calculation; my: Measured vertical size; rx: Rx; ry: Ry.</remarks>
    public bool isMouseOver(int x, int y, int w, int h)
    {
        if (!NsoInputSystemBridge.MousePresent)
            return false;
        Vector3 m = NsoInputSystemBridge.MousePosition;
        float mx = m.x, my = Screen.height - m.y; // screen pixels, Y down like the game
        float rx = x * zoomLevel + (isTranslate ? translateX : 0);
        float ry = y * zoomLevel + (isTranslate ? translateY : 0);
        return mx >= rx && mx < rx + w * zoomLevel && my >= ry && my < ry + h * zoomLevel;
    }

    /// <summary>Return translate x.</summary>
    public int getTranslateX()
    {
        return translateX / zoomLevel;
    }

    /// <summary>Return translate y.</summary>
    public int getTranslateY()
    {
        return translateY / zoomLevel;
    }

    /// <summary>Set clip.</summary>
    /// <param name="x">Horizontal coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="y">Vertical coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="w">Width or horizontal span.</param>
    /// <param name="h">Height or vertical span.</param>
    public void setClip(int x, int y, int w, int h)
    {
        ClearScrollPaintOffset();
        x *= zoomLevel;
        y *= zoomLevel;
        w *= zoomLevel;
        h *= zoomLevel;
        clipTX = translateX;
        clipTY = translateY;
        clipX = x;
        clipY = y;
        clipW = w;
        clipH = h;
        isClip = true;
    }

    /// <summary>Draw line in the current graphics context.</summary>
    /// <param name="x1">X1.</param>
    /// <param name="y1">Y1.</param>
    /// <param name="x2">X2.</param>
    /// <param name="y2">Y2.</param>
    /// <remarks>Locals: num: Numeric intermediate used by this operation; the legacy name is retained; num2: Numeric intermediate used by this operation; the legacy name is retained; num3: Horizontal position/difference used by this calculation; num4: Numeric intermediate used by this operation; the legacy name is retained; num5: Numeric intermediate used by this operation; the legacy name is retained; num6: Numeric intermediate used by this operation; the legacy name is retained; num7: Numeric intermediate used by this operation; the legacy name is retained; num8: Numeric intermediate used by this operation; the legacy name is retained.</remarks>
    public void drawLine(int x1, int y1, int x2, int y2)
    {
        x1 *= zoomLevel;
        y1 *= zoomLevel;
        x2 *= zoomLevel;
        y2 *= zoomLevel;
        if (y1 == y2)
        {
            if (x1 > x2)
            {
                int num = x2;
                x2 = x1;
                x1 = num;
            }

            fillRect(x1, y1, x2 - x1, 1);
            return;
        }

        if (x1 == x2)
        {
            if (y1 > y2)
            {
                int num2 = y2;
                y2 = y1;
                y1 = num2;
            }

            fillRect(x1, y1, 1, y2 - y1);
            return;
        }

        if (isTranslate)
        {
            x1 += translateX;
            y1 += translateY;
            x2 += translateX;
            y2 += translateY;
        }

        string key = "dl" + r + g + b;
        Texture2D texture2D = (Texture2D)cachedTextures[key];
        if (texture2D == null)
        {
            texture2D = new Texture2D(1, 1);
            Color color = new Color(r, g, b);
            texture2D.SetPixel(0, 0, color);
            texture2D.Apply();
            cache(key, texture2D);
        }

        Vector2 vector = new Vector2(x1 + scrollPaintOffset.x, y1 + scrollPaintOffset.y);
        Vector2 vector2 = new Vector2(x2 + scrollPaintOffset.x, y2 + scrollPaintOffset.y);
        Vector2 vector3 = vector2 - vector;
        float num3 = 57.29578f * Mathf.Atan(vector3.y / vector3.x);
        if (vector3.x < 0f)
        {
            num3 += 180f;
        }

        int num4 = (int)Mathf.Ceil(0f);
        GUIUtility.RotateAroundPivot(num3, vector);
        int num5 = 0;
        int num6 = 0;
        int num7 = 0;
        int num8 = 0;
        if (isClip)
        {
            num5 = clipX;
            num6 = clipY;
            num7 = clipW;
            num8 = clipH;
            if (isTranslate)
            {
                num5 += clipTX;
                num6 += clipTY;
            }
        }

        if (isClip)
        {
            GUI.BeginGroup(new Rect(num5, num6, num7, num8));
        }

        Graphics.DrawTexture(new Rect(vector.x - (float)num5, vector.y - (float)num4 - (float)num6, vector3.magnitude, 1f), texture2D);
        if (isClip)
        {
            GUI.EndGroup();
        }

        GUIUtility.RotateAroundPivot(0f - num3, vector);
    }

    /// <summary>Draw rectangle in the current graphics context.</summary>
    /// <param name="x">Horizontal coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="y">Vertical coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="w">Width or horizontal span.</param>
    /// <param name="h">Height or vertical span.</param>
    /// <remarks>Locals: num: Numeric intermediate used by this operation; the legacy name is retained.</remarks>
    public void drawRect(int x, int y, int w, int h)
    {
        int num = 1;
        fillRect(x, y, w, num);
        fillRect(x, y, num, h);
        fillRect(x + w, y, num, h + 1);
        fillRect(x, y + h, w + 1, num);
    }

    // Fractional placement for selected visual effects; the old integer drawing API stays intact.
    public struct OffsetScope : System.IDisposable
    {
        private Matrix4x4 previous;
        private bool changed;
        /// <summary>Initialize offset scope state and its supplied dependencies.</summary>
        /// <param name="x">Horizontal coordinate in the caller&#x27;s coordinate space.</param>
        /// <param name="y">Vertical coordinate in the caller&#x27;s coordinate space.</param>
        public OffsetScope(float x, float y)
        {
            changed = x != 0f || y != 0f;
            previous = changed ? GUI.matrix : Matrix4x4.identity;
            if (changed)
                GUI.matrix = previous * Matrix4x4.Translate(new Vector3(x * zoomLevel, y * zoomLevel, 0f));
        }

        /// <summary>Release resources owned by this instance.</summary>
        public void Dispose()
        {
            if (changed)
                GUI.matrix = previous;
        }
    }

    /// <summary>Compute offset.</summary>
    /// <param name="x">Horizontal coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="y">Vertical coordinate in the caller&#x27;s coordinate space.</param>
    public OffsetScope Offset(float x, float y)
    {
        return new OffsetScope(x, y);
    }

    private Vector2 scrollPaintOffset;
    // Apply before clipping: the viewport itself must not move with the scrolled content.
    /// <summary>Set scroll paint offset.</summary>
    /// <param name="x">Horizontal coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="y">Vertical coordinate in the caller&#x27;s coordinate space.</param>
    public void SetScrollPaintOffset(float x, float y)
    {
        scrollPaintOffset = new Vector2(x * zoomLevel, y * zoomLevel);
    }

    /// <summary>Clear scroll paint offset.</summary>
    public void ClearScrollPaintOffset()
    {
        scrollPaintOffset = Vector2.zero;
    }

    /// <summary>Perform fill rectangle.</summary>
    /// <param name="x">Horizontal coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="y">Vertical coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="w">Width or horizontal span.</param>
    /// <param name="h">Height or vertical span.</param>
    public void fillRect(int x, int y, int w, int h)
    {
        fillRect((float)x, y, w, h);
    }

    // Keep fractional HP-bar width until rasterization.
    /// <summary>Perform fill rectangle.</summary>
    /// <param name="x">Horizontal coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="y">Vertical coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="w">Width or horizontal span.</param>
    /// <param name="h">Height or vertical span.</param>
    /// <remarks>Locals: num: Numeric intermediate used by this operation; the legacy name is retained; num2: Numeric intermediate used by this operation; the legacy name is retained; num3: Numeric intermediate used by this operation; the legacy name is retained; num4: Numeric intermediate used by this operation; the legacy name is retained; num5: Numeric intermediate used by this operation; the legacy name is retained; num6: Numeric intermediate used by this operation; the legacy name is retained.</remarks>
    public void fillRect(float x, float y, float w, float h)
    {
        x *= zoomLevel;
        y *= zoomLevel;
        x += scrollPaintOffset.x;
        y += scrollPaintOffset.y;
        w *= zoomLevel;
        h *= zoomLevel;
        if (w < 0 || h < 0)
        {
            return;
        }

        if (isTranslate)
        {
            x += translateX;
            y += translateY;
        }

        int num = 1;
        int num2 = 1;
        string key = "fr" + num + num2 + r + g + b + a;
        Texture2D texture2D = (Texture2D)cachedTextures[key];
        if (texture2D == null)
        {
            texture2D = new Texture2D(num, num2);
            Color color = new Color(r, g, b, a);
            texture2D.SetPixel(0, 0, color);
            texture2D.Apply();
            cache(key, texture2D);
        }

        int num3 = 0;
        int num4 = 0;
        int num5 = 0;
        int num6 = 0;
        if (isClip)
        {
            num3 = clipX;
            num4 = clipY;
            num5 = clipW;
            num6 = clipH;
            if (isTranslate)
            {
                num3 += clipTX;
                num4 += clipTY;
            }
        }

        if (DrawRecorder.Armed)
            DrawRecorder.Fill(isClip ? intersectRect(new Rect(x, y, w, h), new Rect(num3, num4, num5, num6)) : new Rect(x, y, w, h), new Color(r, g, b, Mathf.Clamp01(a)));

        if (isClip)
        {
            GUI.BeginGroup(new Rect(num3, num4, num5, num6));
        }

        GUI.DrawTexture(new Rect(x - num3, y - num4, w, h), texture2D);
        if (isClip)
        {
            GUI.EndGroup();
        }
    }

    /// <summary>Set color.</summary>
    /// <param name="rgb">Red/green/blue channels.</param>
    /// <remarks>Locals: num: Numeric intermediate used by this operation; the legacy name is retained; num2: Numeric intermediate used by this operation; the legacy name is retained; num3: Numeric intermediate used by this operation; the legacy name is retained.</remarks>
    public void setColor(int rgb)
    {
        int num = rgb & 0xFF;
        int num2 = (rgb >> 8) & 0xFF;
        int num3 = (rgb >> 16) & 0xFF;
        b = (float)num / 256f;
        g = (float)num2 / 256f;
        r = (float)num3 / 256f;
        a = 255f;
    }

    /// <summary>Set color.</summary>
    /// <param name="color">Color.</param>
    public void setColor(Color color)
    {
        b = color.b;
        g = color.g;
        r = color.r;
    }

    /// <summary>Draw string in the current graphics context.</summary>
    /// <param name="s">Source text or serialized input.</param>
    /// <param name="x">Horizontal coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="y">Vertical coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="style">Style.</param>
    /// <remarks>Locals: num: Numeric intermediate used by this operation; the legacy name is retained; num2: Numeric intermediate used by this operation; the legacy name is retained; num3: Numeric intermediate used by this operation; the legacy name is retained; num4: Numeric intermediate used by this operation; the legacy name is retained.</remarks>
    public void drawString(string s, float x, float y, GUIStyle style)
    {
        x *= zoomLevel;
        y *= zoomLevel;
        x += scrollPaintOffset.x;
        y += scrollPaintOffset.y;
        if (isTranslate)
        {
            x += translateX;
            y += translateY;
        }

        int num = 0;
        int num2 = 0;
        int num3 = 0;
        int num4 = 0;
        if (isClip)
        {
            num = clipX;
            num2 = clipY;
            num3 = clipW;
            num4 = clipH;
            if (isTranslate)
            {
                num += clipTX;
                num2 += clipTY;
            }
        }

        if (isClip)
        {
            GUI.BeginGroup(new Rect(num, num2, num3, num4));
        }

        TextCalib.Record(s, style, x, y);
        GUI.Label(new Rect(x - num, y - num2, ScaleGUI.WIDTH, 100f), s, style);
        if (isClip)
        {
            GUI.EndGroup();
        }
    }

    /// <summary>Set color.</summary>
    /// <param name="rgb">Red/green/blue channels.</param>
    /// <param name="alpha">Alpha.</param>
    /// <remarks>Locals: num: Numeric intermediate used by this operation; the legacy name is retained; num2: Numeric intermediate used by this operation; the legacy name is retained; num3: Numeric intermediate used by this operation; the legacy name is retained.</remarks>
    public void setColor(int rgb, float alpha)
    {
        int num = rgb & 0xFF;
        int num2 = (rgb >> 8) & 0xFF;
        int num3 = (rgb >> 16) & 0xFF;
        b = (float)num / 256f;
        g = (float)num2 / 256f;
        r = (float)num3 / 256f;
        a = alpha;
    }

    /// <summary>Draw string in the current graphics context.</summary>
    /// <param name="s">Source text or serialized input.</param>
    /// <param name="x">Horizontal coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="y">Vertical coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="style">Style.</param>
    /// <param name="w">Width or horizontal span.</param>
    /// <remarks>Locals: num: Numeric intermediate used by this operation; the legacy name is retained; num2: Numeric intermediate used by this operation; the legacy name is retained; num3: Numeric intermediate used by this operation; the legacy name is retained; num4: Numeric intermediate used by this operation; the legacy name is retained.</remarks>
    public void drawString(string s, float x, float y, GUIStyle style, int w)
    {
        x *= zoomLevel;
        y *= zoomLevel;
        x += scrollPaintOffset.x;
        y += scrollPaintOffset.y;
        if (isTranslate)
        {
            x += translateX;
            y += translateY;
        }

        int num = 0;
        int num2 = 0;
        int num3 = 0;
        int num4 = 0;
        if (isClip)
        {
            num = clipX;
            num2 = clipY;
            num3 = clipW;
            num4 = clipH;
            if (isTranslate)
            {
                num += clipTX;
                num2 += clipTY;
            }
        }

        if (isClip)
        {
            GUI.BeginGroup(new Rect(num, num2, num3, num4));
        }

        GUI.Label(new Rect(x - num, y - num2 - 4, w, 100f), s, style);
        if (isClip)
        {
            GUI.EndGroup();
        }
    }

    /// <summary>Update position.</summary>
    /// <param name="anchor">Anchor.</param>
    private void UpdatePos(int anchor)
    {
        Vector2 vector = new Vector2(0f, 0f);
        switch (anchor)
        {
            case 3:
                vector = new Vector2(size.x / 2f, size.y / 2f);
                break;
            case 20:
                vector = new Vector2(0f, 0f);
                break;
            case 17:
                vector = new Vector2(Screen.width / 2, 0f);
                break;
            case 24:
                vector = new Vector2(Screen.width, 0f);
                break;
            case 6:
                vector = new Vector2(0f, Screen.height / 2);
                break;
            case 10:
                vector = new Vector2(Screen.width, Screen.height / 2);
                break;
            case 36:
                vector = new Vector2(0f, Screen.height);
                break;
            case 33:
                vector = new Vector2(Screen.width / 2, Screen.height);
                break;
            case 40:
                vector = new Vector2(Screen.width, Screen.height);
                break;
        }

        pos = vector + relativePosition;
        rect = new Rect(pos.x - size.x * 0.5f, pos.y - size.y * 0.5f, size.x, size.y);
        pivot = new Vector2(rect.xMin + rect.width * 0.5f, rect.yMin + rect.height * 0.5f);
    }

    /// <summary>Draw region in the current graphics context.</summary>
    /// <param name="arg0">Arg0.</param>
    /// <param name="x0">X0.</param>
    /// <param name="y0">Y0.</param>
    /// <param name="w0">W0.</param>
    /// <param name="h0">H0.</param>
    /// <param name="arg5">Arg5.</param>
    /// <param name="x">Horizontal coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="y">Vertical coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="arg8">Arg8.</param>
    public void drawRegion(Image arg0, int x0, int y0, int w0, int h0, int arg5, int x, int y, int arg8)
    {
        x *= zoomLevel;
        y *= zoomLevel;
        x0 *= zoomLevel;
        y0 *= zoomLevel;
        w0 *= zoomLevel;
        h0 *= zoomLevel;
        _drawRegion(arg0, x0, y0, w0, h0, arg5, x, y, arg8);
    }

    /// <summary>Draw region.</summary>
    /// <param name="image">Image.</param>
    /// <param name="x0">X0.</param>
    /// <param name="y0">Y0.</param>
    /// <param name="w">Width or horizontal span.</param>
    /// <param name="h">Height or vertical span.</param>
    /// <param name="transform">Transform.</param>
    /// <param name="x">Horizontal coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="y">Vertical coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="anchor">Anchor.</param>
    public void _drawRegion(Image image, float x0, float y0, int w, int h, int transform, float x, float y, int anchor)
    {
        _drawRegion(image, x0, y0, w, h, transform, x, y, anchor, null, GUI.color);
    }

    /// <summary>Draw region.</summary>
    /// <param name="image">Image.</param>
    /// <param name="x0">X0.</param>
    /// <param name="y0">Y0.</param>
    /// <param name="w">Width or horizontal span.</param>
    /// <param name="h">Height or vertical span.</param>
    /// <param name="transform">Transform.</param>
    /// <param name="x">Horizontal coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="y">Vertical coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="anchor">Anchor.</param>
    /// <param name="material">Material.</param>
    /// <param name="color">Color.</param>
    /// <remarks>Locals: num: Numeric intermediate used by this operation; the legacy name is retained; num2: Numeric intermediate used by this operation; the legacy name is retained; num3: Numeric intermediate used by this operation; the legacy name is retained; num4: Numeric intermediate used by this operation; the legacy name is retained; num5: Numeric intermediate used by this operation; the legacy name is retained; num6: Numeric intermediate used by this operation; the legacy name is retained; num7: Numeric intermediate used by this operation; the legacy name is retained; num8: Numeric intermediate used by this operation; the legacy name is retained; num9: Numeric intermediate used by this operation; the legacy name is retained; num10: Numeric intermediate used by this operation; the legacy name is retained; num11: Numeric intermediate used by this operation; the legacy name is retained; num12: Numeric intermediate used by this operation; the legacy name is retained; num13: Numeric intermediate used by this operation; the legacy name is retained; r: Rectangle used by this layout/drawing calculation; rect: Rectangle used by this layout/drawing calculation; num14: Numeric intermediate used by this operation; the legacy name is retained; num15: Numeric intermediate used by this operation; the legacy name is retained; num16: Numeric intermediate used by this operation; the legacy name is retained; num17: Numeric intermediate used by this operation; the legacy name is retained.</remarks>
    public void _drawRegion(Image image, float x0, float y0, int w, int h, int transform, float x, float y, int anchor, Material material, Color color)
    {
        x += scrollPaintOffset.x;
        y += scrollPaintOffset.y;
        if (image == null)
        {
            return;
        }

        if (isTranslate)
        {
            x += translateX;
            y += translateY;
        }

        float num = w;
        float num2 = h;
        float num3 = 0f;
        float num4 = 0f;
        float num5 = 0f;
        float num6 = 0f;
        float num7 = 1f;
        float num8 = 0f;
        int num9 = 1;
        if ((anchor & HCENTER) == HCENTER)
        {
            num5 -= num / 2f;
        }

        if ((anchor & VCENTER) == VCENTER)
        {
            num6 -= num2 / 2f;
        }

        if ((anchor & RIGHT) == RIGHT)
        {
            num5 -= num;
        }

        if ((anchor & BOTTOM) == BOTTOM)
        {
            num6 -= num2;
        }

        x += (int)num5;
        y += (int)num6;
        int num10 = 0;
        int num11 = 0;
        int num12 = 0;
        int num13 = 0;
        if (isClip)
        {
            if (transform == 5 && y + w < clipY + clipH && y > clipY)
            {
                num10 = 0;
                num11 = 0;
                num12 = GameCanvas.w;
                num13 = GameCanvas.h;
            }
            else
            {
                num10 = clipX;
                num11 = clipY;
                num12 = clipW;
                num13 = clipH;
            }

            if (isTranslate)
            {
                num10 += clipTX;
                num11 += clipTY;
            }

            Rect r = new Rect(x, y, w, h);
            Rect rect = intersectRect(r2: new Rect(num10, num11, num12, num13), r1: r);
            if (rect.width <= 0f || rect.height <= 0f)
            {
                return;
            }

            num = rect.width;
            num2 = rect.height;
            num3 = rect.x - r.x;
            num4 = rect.y - r.y;
        }

        float num14 = 0f;
        float num15 = 0f;
        switch (transform)
        {
            case 2:
                num14 += num;
                num7 = -1f;
                if (isClip)
                {
                    if (num10 > x)
                    {
                        num8 = 0f - num3;
                    }
                    else if (num10 + num12 < x + w)
                    {
                        num8 = -(num10 + num12 - x - w);
                    }
                }

                break;
            case 1:
                num9 = -1;
                num15 += num2;
                break;
            case 3:
                num9 = -1;
                num15 += num2;
                num7 = -1f;
                num14 += num;
                break;
        }

        int num16 = 0;
        int num17 = 0;
        if (transform == 5 || transform == 6 || transform == 4 || transform == 7)
        {
            matrixBackup = GUI.matrix;
            size = new Vector2(w, h);
            relativePosition = new Vector2(x, y);
            UpdatePos(3);
            switch (transform)
            {
                case 6:
                    UpdatePos(3);
                    break;
                case 5:
                    size = new Vector2(w, h);
                    UpdatePos(3);
                    break;
            }

            switch (transform)
            {
                case 5:
                    GUIUtility.RotateAroundPivot(90f, pivot);
                    break;
                case 6:
                    GUIUtility.RotateAroundPivot(270f, pivot);
                    break;
                case 4:
                    GUIUtility.RotateAroundPivot(270f, pivot);
                    num14 += num;
                    num7 = -1f;
                    if (isClip)
                    {
                        if (num10 > x)
                        {
                            num8 = 0f - num3;
                        }
                        else if (num10 + num12 < x + w)
                        {
                            num8 = -(num10 + num12 - x - w);
                        }
                    }

                    break;
                case 7:
                    GUIUtility.RotateAroundPivot(270f, pivot);
                    num9 = -1;
                    num15 += num2;
                    break;
            }
        }

        Rect destination = new Rect((float)x + num3 + num14 + (float)num16, (float)y + num4 + (float)num17 + num15, num * num7, num2 * (float)num9);
        // A mirrored destination must crop the opposite source edge. Otherwise glyph pixels
        // are replaced at the viewport boundary instead of moving with the scrolled line.
        float sourceOffsetX = num3 + num8;
        float sourceOffsetY = num4;
        if (transform == 2 || transform == 3)
            sourceOffsetX = w - num3 - num;
        if (transform == 1 || transform == 3)
            sourceOffsetY = h - num4 - num2;
        Rect source = new Rect((x0 + sourceOffsetX) / (float)image.texture.width, ((float)image.texture.height - num2 - (y0 + sourceOffsetY)) / (float)image.texture.height, num / (float)image.texture.width, num2 / (float)image.texture.height);
        if (DrawRecorder.Armed)
            DrawRecorder.Image(image.texture, destination, source, transform == 5 ? 90f : (transform == 6 || transform == 4 || transform == 7) ? 270f : 0f, color);
        if (material == null)
        {
            Graphics.DrawTexture(destination, image.texture, source, 0, 0, 0, 0);
        }
        else
        {
            Graphics.DrawTexture(destination, image.texture, source, 0, 0, 0, 0, color, material);
        }

        if (transform == 5 || transform == 6 || transform == 4 || transform == 7)
        {
            GUI.matrix = matrixBackup;
        }
    }

    // Draws an atlas region that holds a glyph packed turned 90 degrees. (x, y) is the top-left of the upright glyph,
    // which is h wide and w tall on screen (the region is w wide and h tall in the atlas).
    /// <summary>Draw rotated glyph.</summary>
    /// <param name="image">Image.</param>
    /// <param name="x0">X0.</param>
    /// <param name="y0">Y0.</param>
    /// <param name="w">Width or horizontal span.</param>
    /// <param name="h">Height or vertical span.</param>
    /// <param name="x">Horizontal coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="y">Vertical coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="material">Material.</param>
    /// <param name="color">Color.</param>
    public void _drawRotatedGlyph(Image image, float x0, float y0, int w, int h, float x, float y, Material material, Color color)
    {
        x += scrollPaintOffset.x;
        y += scrollPaintOffset.y;
        if (image == null)
        {
            return;
        }

        if (isTranslate)
        {
            x += translateX;
            y += translateY;
        }

        // Match bitmap pixel placement in final screen space. Fractional camera interpolation
        // otherwise changes sampling of rotated glyphs while a speech popup moves.
        x = Mathf.Round(x + GUI.matrix.m03) - GUI.matrix.m03;
        y = Mathf.Round(y + GUI.matrix.m13) - GUI.matrix.m13;
        // Clip the upright glyph first; atlas coordinates run in the rotated direction.
        if (isClip)
        {
            float viewportX = clipX + clipTX;
            float viewportY = clipY + clipTY;
            Rect upright = new Rect(x, y, h, w);
            Rect visible = intersectRect(upright, new Rect(viewportX, viewportY, clipW, clipH));
            if (visible.width <= 0f || visible.height <= 0f)
                return;
            float left = visible.x - x;
            float top = visible.y - y;
            x0 += w - top - visible.height;
            y0 += left;
            x = visible.x;
            y = visible.y;
            Vector2 clippedCenter = new Vector2(x + visible.width / 2f, y + visible.height / 2f);
            Rect clippedDestination = new Rect(clippedCenter.x - visible.height / 2f, clippedCenter.y - visible.width / 2f, visible.height, visible.width);
            Rect clippedSource = new Rect(x0 / image.texture.width, (image.texture.height - visible.width - y0) / image.texture.height, visible.height / image.texture.width, visible.width / image.texture.height);
            Matrix4x4 previousMatrix = GUI.matrix;
            try
            {
                GUI.matrix = previousMatrix * Matrix4x4.Translate(new Vector3(clippedCenter.x, clippedCenter.y, 0f)) * Matrix4x4.Rotate(Quaternion.Euler(0f, 0f, -90f)) * Matrix4x4.Translate(new Vector3(-clippedCenter.x, -clippedCenter.y, 0f));
                if (material == null)
                    Graphics.DrawTexture(clippedDestination, image.texture, clippedSource, 0, 0, 0, 0, color);
                else
                    Graphics.DrawTexture(clippedDestination, image.texture, clippedSource, 0, 0, 0, 0, color, material);
            }
            finally
            {
                GUI.matrix = previousMatrix;
            }

            return;
        }

        Vector2 center = new Vector2(x + h / 2f, y + w / 2f);
        Rect destination = new Rect(center.x - w / 2f, center.y - h / 2f, w, h);
        Rect source = new Rect(x0 / image.texture.width, (image.texture.height - h - y0) / image.texture.height, (float)w / image.texture.width, (float)h / image.texture.height);
        matrixBackup = GUI.matrix;
        GUI.matrix = matrixBackup * Matrix4x4.Translate(new Vector3(center.x, center.y, 0f)) * Matrix4x4.Rotate(Quaternion.Euler(0f, 0f, -90f)) * Matrix4x4.Translate(new Vector3(-center.x, -center.y, 0f));
        if (material == null)
        {
            Graphics.DrawTexture(destination, image.texture, source, 0, 0, 0, 0, color);
        }
        else
        {
            Graphics.DrawTexture(destination, image.texture, source, 0, 0, 0, 0, color, material);
        }

        GUI.matrix = matrixBackup;
    }

    /// <summary>Draw image in the current graphics context.</summary>
    /// <param name="image">Image.</param>
    /// <param name="x">Horizontal coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="y">Vertical coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="anchor">Anchor.</param>
    public void drawImage(Image image, int x, int y, int anchor)
    {
        if (image != null)
        {
            drawRegion(image, 0, 0, getImageWidth(image), getImageHeight(image), 0, x, y, anchor);
        }
    }

    /// <summary>Draw image in the current graphics context.</summary>
    /// <param name="image">Image.</param>
    /// <param name="x">Horizontal coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="y">Vertical coordinate in the caller&#x27;s coordinate space.</param>
    public void drawImage(Image image, int x, int y)
    {
        if (image != null)
        {
            drawRegion(image, 0, 0, getImageWidth(image), getImageHeight(image), 0, x, y, TOP | LEFT);
        }
    }

    /// <summary>Draw round rectangle in the current graphics context.</summary>
    /// <param name="x">Horizontal coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="y">Vertical coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="w">Width or horizontal span.</param>
    /// <param name="h">Height or vertical span.</param>
    /// <param name="arcWidth">Retained for call compatibility; not read by this implementation.</param>
    /// <param name="arcHeight">Retained for call compatibility; not read by this implementation.</param>
    public void drawRoundRect(int x, int y, int w, int h, int arcWidth, int arcHeight)
    {
        drawRect(x, y, w, h);
    }

    /// <summary>Perform fill round rectangle.</summary>
    /// <param name="x">Horizontal coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="y">Vertical coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="width">Width.</param>
    /// <param name="height">Height.</param>
    /// <param name="arcWidth">Retained for call compatibility; not read by this implementation.</param>
    /// <param name="arcHeight">Retained for call compatibility; not read by this implementation.</param>
    public void fillRoundRect(int x, int y, int width, int height, int arcWidth, int arcHeight)
    {
        fillRect(x, y, width, height);
    }

    /// <summary>Reset m graphics.</summary>
    public void reset()
    {
        ClearScrollPaintOffset();
        isClip = false;
        isTranslate = false;
        translateX = 0;
        translateY = 0;
    }

    /// <summary>Compute intersect rectangle.</summary>
    /// <param name="r1">Rectangle used by this layout/drawing calculation.</param>
    /// <param name="r2">Rectangle used by this layout/drawing calculation.</param>
    /// <remarks>Locals: num: Horizontal position/difference used by this calculation; num2: Vertical position/difference used by this calculation; x: Horizontal position/difference used by this calculation; y: Vertical position/difference used by this calculation; num3: Numeric intermediate used by this operation; the legacy name is retained; num4: Numeric intermediate used by this operation; the legacy name is retained; num5: Numeric intermediate used by this operation; the legacy name is retained; num6: Numeric intermediate used by this operation; the legacy name is retained.</remarks>
    public Rect intersectRect(Rect r1, Rect r2)
    {
        float num = r1.x;
        float num2 = r1.y;
        float x = r2.x;
        float y = r2.y;
        float num3 = num;
        num3 += r1.width;
        float num4 = num2;
        num4 += r1.height;
        float num5 = x;
        num5 += r2.width;
        float num6 = y;
        num6 += r2.height;
        if (num < x)
        {
            num = x;
        }

        if (num2 < y)
        {
            num2 = y;
        }

        if (num3 > num5)
        {
            num3 = num5;
        }

        if (num4 > num6)
        {
            num4 = num6;
        }

        num3 -= num;
        num4 -= num2;
        if (num3 < -30000f)
        {
            num3 = -30000f;
        }

        if (num4 < -30000f)
        {
            num4 = -30000f;
        }

        return new Rect(num, num2, (int)num3, (int)num4);
    }

    /// <summary>Draw image scale in the current graphics context.</summary>
    /// <param name="image">Image.</param>
    /// <param name="x">Horizontal coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="y">Vertical coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="w">Width or horizontal span.</param>
    /// <param name="h">Height or vertical span.</param>
    /// <param name="tranform">Tranform.</param>
    public void drawImageScale(Image image, int x, int y, int w, int h, int tranform)
    {
        x *= zoomLevel;
        y *= zoomLevel;
        w *= zoomLevel;
        h *= zoomLevel;
        if (image != null)
        {
            if (image.texture.filterMode != FilterMode.Bilinear)
                image.texture.filterMode = FilterMode.Bilinear; // scaled art would be jagged with point filtering
            if (DrawRecorder.Armed)
                DrawRecorder.Image(image.texture, new Rect(x + translateX + scrollPaintOffset.x, y + translateY + scrollPaintOffset.y, (tranform != 0) ? (-w) : w, h), new Rect(0, 0, 1, 1), 0f, Color.white);
            Graphics.DrawTexture(new Rect(x + translateX + scrollPaintOffset.x, y + translateY + scrollPaintOffset.y, (tranform != 0) ? (-w) : w, h), image.texture);
        }
    }

    // Pixel art drawn at a whole-number scale with point filtering, centred on (cx, cy) in game pixels: every source
    // pixel becomes a clean scale x scale block, so the edges stay sharp (no uneven, jagged steps from a fractional scale).
    /// <summary>DrawRecorder: true when a point (game px, translated) lies inside the current clip.</summary>
    public bool RecorderVisible(float gx, float gy)
    {
        if (!isClip)
            return true;
        float sx = gx * zoomLevel, sy = gy * zoomLevel;
        float cx = clipX + (isTranslate ? clipTX : 0), cy = clipY + (isTranslate ? clipTY : 0);
        return sx >= cx - 1 && sx < cx + clipW && sy >= cy - 1 && sy < cy + clipH;
    }

    /// <summary>Return image width.</summary>
    /// <param name="image">Image.</param>
    public static int getImageWidth(Image image)
    {
        return image.getWidth();
    }

    /// <summary>Return image height.</summary>
    /// <param name="image">Image.</param>
    public static int getImageHeight(Image image)
    {
        return image.getHeight();
    }
}

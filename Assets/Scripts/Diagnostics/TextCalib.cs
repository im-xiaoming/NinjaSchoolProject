using System.Collections.Generic;
using UnityEngine;

// Text alignment tuner. F6 opens a small panel: Tab / Shift+Tab picks an item, the arrow keys nudge it by 0.1 of a
// logical pixel (hold Shift for whole pixels), Backspace resets the item, F6 closes. Offsets are in logical pixels
// (the game's own units, so they stay right at every zoom level) and are saved in PlayerPrefs.
public class TextCalib : MonoBehaviour
{
    public static readonly string[] Items =
    {
        "global",
        "level",
        "exp",
        "hp",
        "mp",
        "zone",
        "button",
        "sign",
        "buttonLeft",
        "buttonCenter",
        "buttonRight",
        "command",
        "titleMenu",
        "menuCell"
    };
    public static readonly string[] Labels =
    {
        "All text",
        "Level number (HUD)",
        "Exp percent (HUD)",
        "HP number",
        "MP number",
        "Zone numbers",
        "Button captions (all)",
        "Zone sign number",
        "Left button caption",
        "Center button caption",
        "Right button caption",
        "Menu command button caption",
        "Title menu rows",
        "Menu cell captions"
    };
    // Items: global, level, exp, hp, mp, zone, button, sign, buttonLeft, buttonCenter, buttonRight, command.
    // Each text has its own offset (game pixels, +x right, +y down) on top of the original coordinates. Measured from a 1920x1080 run
    // at UI zoom 3 so the level and exp sit on the circle's centre and the hp/mp numbers on their bars' centre lines.
    private static readonly float[] defaultX =
    {
        0f,
        0f,
        0f,
        0f,
        0f,
        0f,
        0f,
        0f,
        0f,
        0f,
        0f,
        0f,
        0f,
        -0.5f
    }; // menuCell: anchor is 0.5 px right of the cell image centre
    private static readonly float[] defaultY =
    {
        0f,
        0f,
        0f,
        0f,
        0f,
        0f,
        0f,
        0f,
        0f,
        0f,
        0f,
        0f,
        0f,
        0f
    };
    private static readonly Dictionary<string, Vector2> values = new Dictionary<string, Vector2>();
    public static bool Open;
    // offset applied to the text drawn between Use() and Clear()
    public static float X, Y;
    public static int Selected = 1;
    private static int selected
    {
        get
        {
            return Selected;
        }

        set
        {
            Selected = value;
        }
    }

    /// <summary>Return text calib.</summary>
    /// <param name="key">Key.</param>
    /// <remarks>Locals: v: Value being inspected or transformed; i: Loop/collection index.</remarks>
    public static Vector2 Get(string key)
    {
        Vector2 v;
        if (values.TryGetValue(key, out v))
            return v;
        int i = System.Array.IndexOf(Items, key);
        v = new Vector2(PlayerPrefs.GetFloat("calibX_" + key, i >= 0 ? defaultX[i] : 0f), PlayerPrefs.GetFloat("calibY_" + key, i >= 0 ? defaultY[i] : 0f));
        values[key] = v;
        return v;
    }

    /// <summary>Perform put.</summary>
    /// <param name="key">Key.</param>
    /// <param name="v">Value being inspected or transformed.</param>
    public static void Put(string key, Vector2 v)
    {
        Set(key, v);
    }

    /// <summary>Attempt live.</summary>
    /// <param name="key">Key.</param>
    /// <param name="r">Rectangle used by this layout/drawing calculation.</param>
    public static bool TryLive(string key, out Rect r)
    {
        return rects.TryGetValue(key, out r) && Live(key);
    }

    // index of the item whose box (grown by margin screen pixels) contains the point, or -1
    /// <summary>Compute hit test.</summary>
    /// <param name="x">Horizontal coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="y">Vertical coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="margin">Margin.</param>
    /// <remarks>Locals: i: Number of entries in the source collection; r: Rectangle used by this layout/drawing calculation.</remarks>
    public static int HitTest(float x, float y, float margin)
    {
        for (int i = Items.Length - 1; i >= 0; i--)
        {
            Rect r;
            if (!TryLive(Items[i], out r))
                continue;
            r.xMin -= margin;
            r.xMax += margin;
            r.yMin -= margin;
            r.yMax += margin;
            if (r.Contains(new Vector2(x, y)))
                return i;
        }

        return -1;
    }

    /// <summary>Set text calib.</summary>
    /// <param name="key">Key.</param>
    /// <param name="v">Value being inspected or transformed.</param>
    private static void Set(string key, Vector2 v)
    {
        values[key] = v;
        PlayerPrefs.SetFloat("calibX_" + key, v.x);
        PlayerPrefs.SetFloat("calibY_" + key, v.y);
        PlayerPrefs.Save();
    }

    // key of the item being drawn right now (null for text that is not tuned) and where each tuned item was last drawn (screen pixels)
    public static string Key;
    private static readonly Dictionary<string, Rect> rects = new Dictionary<string, Rect>();
    private static readonly Dictionary<string, int> stamps = new Dictionary<string, int>();
    // an item counts as on screen if it was drawn in the last few frames
    /// <summary>Compute live.</summary>
    /// <param name="key">Key.</param>
    /// <remarks>Locals: f: Current field/value.</remarks>
    private static bool Live(string key)
    {
        int f;
        return stamps.TryGetValue(key, out f) && Time.frameCount - f <= 3;
    }

    /// <summary>Record text calib.</summary>
    /// <param name="s">Source text or serialized input.</param>
    /// <param name="style">Style.</param>
    /// <param name="x">Horizontal coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="y">Vertical coordinate in the caller&#x27;s coordinate space.</param>
    /// <remarks>Locals: rx: Rx.</remarks>
    public static void Record(string s, GUIStyle style, float x, float y)
    {
        if (Key == null || !Open)
            return;
        Vector2 size = style.CalcSize(new GUIContent(s));
        float rx = x;
        if (style.alignment == TextAnchor.UpperCenter)
            rx = x + ScaleGUI.WIDTH / 2f - size.x / 2f;
        else if (style.alignment == TextAnchor.UpperRight)
            rx = x + ScaleGUI.WIDTH - size.x;
        rects[Key] = new Rect(rx, y, size.x, size.y);
        stamps[Key] = Time.frameCount;
    }

    /// <summary>Record bitmap.</summary>
    /// <param name="key">Key.</param>
    /// <param name="x">Horizontal coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="y">Vertical coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="width">Width.</param>
    /// <param name="height">Height.</param>
    public static void RecordBitmap(string key, float x, float y, float width, float height)
    {
        if (key == null || !Open)
            return;
        rects[key] = new Rect(x, y, width, height);
        stamps[key] = Time.frameCount;
    }

    // The text drawn until Clear() is placed by its drawn pixels on this point (game pixels): centred on it (align 2) or with
    // its left edge on it (align 0), and always centred vertically. Same result at every zoom level. The item's own
    // offset (the Developer / Admin window) is added on top.
    public static bool HasBox;
    public static float BoxX, BoxY;
    /// <summary>Use box.</summary>
    /// <param name="key">Key.</param>
    /// <param name="x">Horizontal coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="y">Vertical coordinate in the caller&#x27;s coordinate space.</param>
    public static void UseBox(string key, float x, float y)
    {
        Use(key);
        HasBox = true;
        BoxX = x;
        BoxY = y;
    }

    /// <summary>Use text calib.</summary>
    /// <param name="key">Key.</param>
    /// <remarks>Locals: v: Value being inspected or transformed.</remarks>
    public static void Use(string key)
    {
        Key = key;
        Vector2 v = Get(key);
        X = v.x;
        Y = v.y;
    }

    /// <summary>Clear text calib.</summary>
    public static void Clear()
    {
        Key = null;
        HasBox = false;
        X = 0f;
        Y = 0f;
    }

    /// <summary>Perform boot.</summary>
    /// <remarks>Locals: go: Unity scene object.</remarks>
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void Boot()
    {
        GameObject go = new GameObject("TextCalib");
        DontDestroyOnLoad(go);
        go.AddComponent<TextCalib>();
    }

    // Enter: move the selected item so its centre lines up with the nearest other item (x), Shift+Enter: (y)
    /// <summary>Perform line.</summary>
    /// <param name="x">Horizontal coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="y">Vertical coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="w">Width or horizontal span.</param>
    /// <param name="h">Height or vertical span.</param>
    /// <param name="c">Current character/entry.</param>
    private static void Line(float x, float y, float w, float h, Color c)
    {
        GUI.color = c;
        GUI.DrawTexture(new Rect(x, y, w, h), Texture2D.whiteTexture);
    }

    // Boxes around every tuned text, and guide lines at the left / centre / right (x) and top / centre / bottom (y) of the
    // other items. A guide is cyan while the selected item is within 30 px of it and turns green (thicker, and both boxes
    // turn green) when it lines up with the selected item to within one pixel.
    /// <summary>Reset item.</summary>
    /// <param name="key">Key.</param>
    public static void ResetItem(string key)
    {
        PlayerPrefs.DeleteKey("calibX_" + key);
        PlayerPrefs.DeleteKey("calibY_" + key);
        values.Remove(key);
    }
// Keyboard routing is owned by NsoInputSystemBridge, not the repeated IMGUI paint loop.
}

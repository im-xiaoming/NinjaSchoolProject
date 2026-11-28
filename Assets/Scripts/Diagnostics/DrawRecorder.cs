using System;
using System.Collections.Generic;
using UnityEngine;

// Records what the game draws during one frame (images, filled rectangles, text) so a Canvas scene can be generated
// from it (Tools > Ninja School > Build Panel Scenes). Off unless a capture arms it; costs nothing otherwise.
// Coordinates are stored in game pixels (screen pixels / zoom level), top-left origin.
public static class DrawRecorder
{
    [Serializable]
    public class Op
    {
        public string kind;     // "image", "fill", "text"
        public string section;  // which part of GameScr.paint drew it ("hud", "panel", ...)
        public float x, y, w, h; // destination, game px (w < 0 / h < 0 = mirrored)
        public float rotation;  // degrees (clockwise on screen) for rotated regions
        public Color color;
        // image
        [NonSerialized] public Texture texture;
        public Rect uv;         // source rect in UV (0..1, bottom-left origin)
        public string sprite;   // file name of the exported crop, filled in when saving
        // text
        public string text;
        public int font;        // NsoNormalFont index
        public int align;       // mFont align: 0 left, 1 right, 2/3 centre
    }

    public static bool Armed;
    public static string Section = "";
    public static int TextDepth;
    public static readonly List<Op> Ops = new List<Op>();

    private static bool Recording
    {
        get { return Armed && TextDepth == 0 && Event.current != null && Event.current.type == EventType.Repaint; }
    }

    public static void Begin()
    {
        Ops.Clear();
        Armed = true;
    }

    public static void End()
    {
        Armed = false;
    }

    // screen-pixel rectangle (already translated / clipped) of a textured draw
    public static void Image(Texture tex, Rect dest, Rect uv, float rotation, Color color)
    {
        if (!Recording || tex == null)
            return;
        float z = Mathf.Max(1, mGraphics.zoomLevel);
        Ops.Add(new Op
        {
            kind = "image", section = Section, texture = tex, uv = uv, rotation = rotation, color = color,
            x = (dest.x + GUI.matrix.m03) / z, y = (dest.y + GUI.matrix.m13) / z, w = dest.width / z, h = dest.height / z
        });
    }

    public static void Fill(Rect dest, Color color)
    {
        if (!Recording || dest.width <= 0 || dest.height <= 0)
            return;
        float z = Mathf.Max(1, mGraphics.zoomLevel);
        Ops.Add(new Op
        {
            kind = "fill", section = Section, color = color,
            x = (dest.x + GUI.matrix.m03) / z, y = (dest.y + GUI.matrix.m13) / z, w = dest.width / z, h = dest.height / z
        });
    }

    // (x, y) in game px, already translated
    public static void Text(string text, int font, Color color, float x, float y, int align)
    {
        if (!Armed || TextDepth != 1 || Event.current == null || Event.current.type != EventType.Repaint || string.IsNullOrEmpty(text))
            return;
        float z = Mathf.Max(1, mGraphics.zoomLevel);
        Ops.Add(new Op
        {
            kind = "text", section = Section, text = text, font = font, color = color, align = align,
            x = x + GUI.matrix.m03 / z, y = y + GUI.matrix.m13 / z
        });
    }
}

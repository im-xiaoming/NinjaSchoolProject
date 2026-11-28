using UnityEngine;

// The game's 0xRRGGBB colour ints (mGraphics.setColor) as Unity colours.
public static class NsoUIColors
{
    public static Color Rgb(int rgb)
    {
        return new Color(((rgb >> 16) & 0xFF) / 255f, ((rgb >> 8) & 0xFF) / 255f, (rgb & 0xFF) / 255f, 1f);
    }
}

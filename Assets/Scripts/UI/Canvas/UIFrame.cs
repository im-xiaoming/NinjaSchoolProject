using UnityEngine;

// Canvas version of Paint.paintFrame (zoom level 2+ branch): a brown panel with the game's silver border.
// The children (Fill, Border Top / Bottom / Left / Right, Corner TL / TR / BL / BR, Top Ornament) are UI Images;
// Layout moves them to the frame rectangle the game would draw.
public class UIFrame : MonoBehaviour
{
    public RectTransform fill, borderTop, borderBottom, borderLeft, borderRight, cornerTL, cornerTR, cornerBL, cornerBR, ornament;

    public void Layout(int x, int y, int w, int h)
    {
        int nTop = w / 3 - 2, nSide = h / 2 - 2;
        NsoUI.Place(fill, x - 1, y, w + 1, h);
        NsoUI.Place(borderTop, x, y - 2, nTop * 3, 2);
        NsoUI.Place(borderBottom, x, y + h - 1, nTop * 3, 2);
        NsoUI.Place(borderLeft, x - 2, y, 2, (nSide - 1) * 2 + 3);
        NsoUI.Place(borderRight, x + w, y, 2, (nSide - 1) * 2 + 6);
        NsoUI.Place(cornerTL, x - 2, y - 2, 20, 20);
        NsoUI.Place(cornerTR, x + w + 2 - 20, y - 2, 20, 20);
        NsoUI.Place(cornerBL, x - 2, y + h + 1 - 20, 20, 20);
        NsoUI.Place(cornerBR, x + w + 2 - 20, y + h + 1 - 20, 20, 20);
        NsoUI.Place(ornament, x + w / 2f - 17.5f, y - 7, 35, 11);
    }
}

using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Canvas version of TField.paint / Paint.paintInputTf: the hd/tf frame (29 x 27 slices, focused or not),
// the text clipped inside it, the blinking caret and the small clear icon (touch only).
// Display only: typing, focus and clicks are still handled by the game's TField, so nothing here is a raycast target.
public class UITextField : MonoBehaviour
{
    public RawImage sliceTemplate;     // texture hd/tf: 29 x 162 game px, six 27 px rows
    public RectTransform clip;         // RectMask2D over the text area
    public TMP_Text text;              // child of clip
    public UnityEngine.UI.Image caret; // child of clip
    public RawImage clearIcon;         // er

    private const int SliceW = 29, SliceH = 27, TexRows = 162;
    private readonly List<RawImage> slices = new List<RawImage>();

    public void Hide()
    {
        if (gameObject.activeSelf)
            gameObject.SetActive(false);
    }

    public void Show(TField tf)
    {
        if (!gameObject.activeSelf)
            gameObject.SetActive(true);
        int x = tf.x, y = tf.y, w = tf.width, h = tf.height;

        // frame: left cap, right cap, then middle pieces (focused rows 81 / 108 / 135, normal rows 0 / 27 / 54)
        int baseRow = tf.isFocus ? 81 : 0;
        int middle = Mathf.Max(0, (w - 58) / SliceW);
        int n = 3 + middle;
        while (slices.Count < n)
        {
            RawImage s = Instantiate(sliceTemplate, sliceTemplate.transform.parent);
            s.name = "Slice " + (slices.Count + 1);
            s.gameObject.SetActive(true);
            s.transform.SetSiblingIndex(sliceTemplate.transform.GetSiblingIndex() + 1); // under the text, caret and icon
            slices.Add(s);
        }

        if (sliceTemplate.gameObject.activeSelf)
            sliceTemplate.gameObject.SetActive(false);
        Slice(0, x, y, baseRow);
        Slice(1, x + w - SliceW, y, baseRow + 54);
        Slice(2, x + w - 58, y, baseRow + 27);
        for (int i = 0; i < middle; i++)
            Slice(3 + i, x + SliceW + i * SliceW, y, baseRow + 27);
        for (int i = 0; i < slices.Count; i++)
            if (slices[i].gameObject.activeSelf != i < n)
                slices[i].gameObject.SetActive(i < n);

        // text area (the game clips to x + 3, y + 1, w - 4, h - 2); its children are placed inside it
        int cx = x + 3, cy = y + 1;
        NsoUI.Place(clip, cx, cy, w - 4, h - 2);
        string shown = tf.DisplayText ?? string.Empty;
        if (text.text != shown)
            text.text = shown;
        int textX = TField.TextGapX + tf.TextOffsetX + x + 1;
        int textY = y + (h - mFont.tahoma_8b.getHeight()) / 2 + 1;
        NsoUI.Place(text.rectTransform, textX - cx, textY - cy, 2000, 16);

        bool caretOn = tf.CaretVisible;
        if (caret.gameObject.activeSelf != caretOn)
            caret.gameObject.SetActive(caretOn);
        if (caretOn)
        {
            int index = Mathf.Clamp(tf.CaretIndex, 0, shown.Length);
            // width of the text before the caret; "a" is appended so trailing spaces count (as TField.paint does)
            float before = text.GetPreferredValues(shown.Substring(0, index) + "a").x - text.GetPreferredValues("a").x;
            int caretH = TField.CaretHeight;
            NsoUI.Place(caret.rectTransform, textX + before - 1 - cx, y + (h - caretH) / 2 + 6 - cy, 1, caretH);
        }

        bool clearOn = tf.ShowClearIcon;
        if (clearIcon.gameObject.activeSelf != clearOn)
            clearIcon.gameObject.SetActive(clearOn);
        if (clearOn)
            NsoUI.Place(clearIcon.rectTransform, x + w - 8 - 6.5f, y + h / 2 + 1 - 6.5f, 13, 13);
    }

    // one 29 x 27 piece of hd/tf, taken from texture row `row` (game px from the top)
    private void Slice(int i, int x, int y, int row)
    {
        RawImage s = slices[i];
        NsoUI.Place(s.rectTransform, x, y, SliceW, SliceH);
        s.uvRect = new Rect(0, 1f - (row + SliceH) / (float)TexRows, 1, SliceH / (float)TexRows);
    }
}

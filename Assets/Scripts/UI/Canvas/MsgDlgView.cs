using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Canvas version of MsgDlg.paint: the message box (paintFrame + tahoma_8b lines), the spinning shuriken of the
// "Xin chờ" wait box, and its buttons. The iPad variant is a black screen with the shuriken and "Xin chờ".
public class MsgDlgView : MonoBehaviour
{
    public GameObject content;
    public UIFrame frame;
    public TMP_Text lineTemplate;
    public RawImage spinner;
    public GameObject fullScreenBlack;
    public TMP_Text fullScreenText;
    public UICommandButton left, center, right;

    private readonly List<TMP_Text> lines = new List<TMP_Text>();

    private void LateUpdate()
    {
        MsgDlg dlg = GameCanvas.currentDialog as MsgDlg;
        bool on = dlg != null && dlg.info != null;
        if (content.activeSelf != on)
            content.SetActive(on);
        if (!on)
            return;

        int hw = GameCanvas.hw, lineH = mFont.tahoma_8b.getHeight();
        fullScreenBlack.SetActive(dlg.isIpad);
        frame.gameObject.SetActive(!dlg.isIpad);
        fullScreenText.gameObject.SetActive(dlg.isIpad);
        if (dlg.isIpad)
        {
            ShowLines(0);
            Spin(hw, GameCanvas.hh, true);
            NsoUI.Place(fullScreenText.rectTransform, hw - 100, GameCanvas.hh + mFont.tahoma_7b_yellow.getHeight(), 200, 16);
            fullScreenText.text = mResources.PLEASEWAIT;
        }
        else
        {
            int h = dlg.Height, padLeft = dlg.PadLeft;
            int top = GameCanvas.h - h - 38;
            frame.Layout(padLeft, top, GameCanvas.w - padLeft * 2, h);
            int y = top + (h - dlg.info.Length * lineH) / 2 - 2;
            if (dlg.isWait)
                y += 8;
            Spin(hw, y - 12, dlg.isWait);
            ShowLines(dlg.info.Length);
            for (int i = 0; i < dlg.info.Length; i++)
            {
                NsoUI.Place(lines[i].rectTransform, hw - 250, y + i * lineH, 500, 16);
                if (lines[i].text != dlg.info[i])
                    lines[i].text = dlg.info[i];
            }
        }

        Command selected = dlg.KeySelection;
        left.Show(dlg.left, 0, selected);
        center.Show(dlg.center, 1, selected);
        right.Show(dlg.right, 2, selected);
    }

    // GameCanvas.paintShukiren: frame gameTick % 3 of the 16 x 16 shuriken strip (u/f), centred on (x, y)
    private void Spin(int x, int y, bool visible)
    {
        if (spinner.gameObject.activeSelf != visible)
            spinner.gameObject.SetActive(visible);
        if (!visible)
            return;
        NsoUI.Place(spinner.rectTransform, x - 8, y - 8, 16, 16);
        Texture tex = spinner.texture;
        float frameH = 32f / tex.height; // 16 game pixels = 32 texture pixels in the x2 image
        int f = GameCanvas.gameTick % 3;
        spinner.uvRect = new Rect(0, 1f - (f + 1) * frameH, 1, frameH);
    }

    private void ShowLines(int count)
    {
        while (lines.Count < count)
        {
            TMP_Text t = Instantiate(lineTemplate, lineTemplate.transform.parent);
            t.name = "Line " + (lines.Count + 1);
            lines.Add(t);
        }

        lineTemplate.gameObject.SetActive(false);
        for (int i = 0; i < lines.Count; i++)
            if (lines[i].gameObject.activeSelf != i < count)
                lines[i].gameObject.SetActive(i < count);
    }
}

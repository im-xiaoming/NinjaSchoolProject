using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Canvas version of InputDlg.paint and Input2Dlg.paint (Paint.paintInputDlg): the frame, the centred tahoma_8b
// info lines, an optional captcha image, one or two text fields (Input2Dlg also draws a "title: " left of each)
// and the bottom buttons. Typing and field focus stay in the game (TField); this only shows them.
public class InputDlgView : MonoBehaviour
{
    public GameObject content;
    public UIFrame frame;
    public TMP_Text lineTemplate;
    public RawImage captcha;
    public TMP_Text title1, title2;
    public UITextField field1, field2;
    public UICommandButton left, center, right;

    private readonly List<TMP_Text> lines = new List<TMP_Text>();

    private void LateUpdate()
    {
        Dialog dlg = GameCanvas.currentDialog;
        InputDlg one = dlg as InputDlg;
        Input2Dlg two = dlg as Input2Dlg;
        bool on = (one != null && one.Info != null) || (two != null && two.Info != null);
        if (content.activeSelf != on)
            content.SetActive(on);
        if (!on)
            return;

        string[] info = one != null ? one.Info : two.Info;
        int x = one != null ? one.FrameX : two.FrameX;
        int y = one != null ? one.FrameY : two.FrameY;
        int w = one != null ? one.FrameW : two.FrameW;
        int h = one != null ? one.FrameH : two.FrameH;

        // captcha: the frame grows upward by the image height and the text moves up with it
        Image cap = GameCanvas.testCapcha;
        int capH = cap != null ? cap.getHeight() : 0;
        frame.Layout(x, y - capH, w, h + capH);
        bool capOn = cap != null;
        if (captcha.gameObject.activeSelf != capOn)
            captcha.gameObject.SetActive(capOn);
        if (capOn)
        {
            captcha.texture = cap.texture;
            int cw = cap.getWidth();
            NsoUI.Place(captcha.rectTransform, x + w / 2 - cw / 2, y + h / 2 - 4 - capH, cw, capH);
        }

        int lineH = mFont.tahoma_8b.getHeight();
        int top = y + 20 - lineH - capH;
        ShowLines(info.Length);
        for (int i = 0; i < info.Length; i++)
        {
            NsoUI.Place(lines[i].rectTransform, x + w / 2 - 250, top + i * lineH, 500, 16);
            if (lines[i].text != info[i])
                lines[i].text = info[i];
        }

        TField tf1 = one != null ? one.tfInput : two.tfInput;
        field1.Show(tf1);
        if (two != null)
        {
            field2.Show(two.tfInput2);
            Title(title1, two.tfInput, two.TitleWidth);
            Title(title2, two.tfInput2, two.TitleWidth);
        }
        else
        {
            field2.Hide();
            title1.gameObject.SetActive(false);
            title2.gameObject.SetActive(false);
        }

        Command selected = dlg.KeySelection;
        left.Show(dlg.left, 0, selected);
        center.Show(dlg.center, 1, selected);
        right.Show(dlg.right, 2, selected);
    }

    // Input2Dlg: "title: " drawn at (field x - title width - 5, field y + 2)
    private static void Title(TMP_Text label, TField tf, int titleWidth)
    {
        if (!label.gameObject.activeSelf)
            label.gameObject.SetActive(true);
        NsoUI.Place(label.rectTransform, tf.x - titleWidth - 5, tf.y + 2, 300, 16);
        string s = tf.title + ": ";
        if (label.text != s)
            label.text = s;
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

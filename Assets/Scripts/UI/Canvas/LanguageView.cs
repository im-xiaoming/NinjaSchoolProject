using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Canvas version of LanguageScr.paint: the "Ngôn ngữ / Language" popup with two rows and the Chọn / Quay lại buttons.
// The background behind it is still drawn by the game (GameCanvas.paintBGGameScr).
// Layout numbers come from LanguageScr itself (PopupX / PopupY / PopupW / PopupH, IndexRow).
public class LanguageView : MonoBehaviour
{
    public GameObject content;
    public UIFrame frame;
    public RectTransform titleFill;
    public RectTransform[] titleOutline = new RectTransform[4]; // top, left, right, bottom
    public TMP_Text titleText;
    public UnityEngine.UI.Image[] rowFills = new UnityEngine.UI.Image[2];
    public UnityEngine.UI.Image[] rowOutlineTop = new UnityEngine.UI.Image[2], rowOutlineLeft = new UnityEngine.UI.Image[2], rowOutlineRight = new UnityEngine.UI.Image[2], rowOutlineBottom = new UnityEngine.UI.Image[2];
    public TMP_Text[] rowLabels = new TMP_Text[2];
    public Button[] rowButtons = new Button[2];
    public UICommandButton left, center, right;

    private static readonly string[] Labels = { "Tiếng Việt", "English" };

    private void Awake()
    {
        for (int i = 0; i < rowButtons.Length; i++)
        {
            int row = i;
            rowButtons[i].onClick.AddListener(() =>
            {
                LanguageScr screen = GameCanvas.currentScreen as LanguageScr;
                if (screen != null)
                    screen.ClickRow(row);
            });
        }
    }

    private void LateUpdate()
    {
        LanguageScr screen = GameCanvas.currentScreen as LanguageScr;
        bool on = screen != null;
        if (content.activeSelf != on)
            content.SetActive(on);
        if (!on)
            return;

        int x = screen.PopupX, y = screen.PopupY, w = screen.PopupW, h = screen.PopupH, hw = GameCanvas.hw;
        frame.Layout(x, y, w, h);

        // title: fillRoundRect / drawRoundRect around the text, tahoma_8b centred at popupY + 10
        string title = screen.Title;
        int tw = mFont.tahoma_8b.getWidth(title);
        int tx = hw - tw / 2 - 12, ty = y + 7, tW = tw + 22, tH = 24;
        NsoUI.Place(titleFill, tx, ty, tW, tH);
        DrawRect(titleOutline[0], titleOutline[1], titleOutline[2], titleOutline[3], tx, ty, tW, tH);
        NsoUI.Place(titleText.rectTransform, hw - 100, y + 10, 200, 16);
        if (titleText.text != title)
            titleText.text = title;

        // rows: fillRect + drawRect, light when selected; tahoma_7b_white label
        int rowY = y + 45;
        for (int i = 0; i < 2; i++)
        {
            bool selected = i == screen.IndexRow;
            int rx = x + 15, ry = rowY + i * 36, rw = w - 30, rh = 30;
            NsoUI.Place(rowFills[i].rectTransform, rx, ry, rw, rh);
            rowFills[i].color = NsoUIColors.Rgb(selected ? Paint.COLORLIGHT : Paint.COLORDARK);
            Color line = NsoUIColors.Rgb(selected ? 11053224 : 5720393);
            rowOutlineTop[i].color = rowOutlineLeft[i].color = rowOutlineRight[i].color = rowOutlineBottom[i].color = line;
            DrawRect(rowOutlineTop[i].rectTransform, rowOutlineLeft[i].rectTransform, rowOutlineRight[i].rectTransform, rowOutlineBottom[i].rectTransform, rx, ry, rw, rh);
            NsoUI.Place(rowLabels[i].rectTransform, hw - 100, ry + 6, 200, 16);
            if (rowLabels[i].text != Labels[i])
                rowLabels[i].text = Labels[i];
        }

        left.Show(screen.left, 0);
        center.Show(screen.center, 1);
        right.Show(screen.right, 2);
    }

    // mGraphics.drawRect(x, y, w, h): four 1-pixel lines, the right and bottom ones one pixel longer
    public static void DrawRect(RectTransform top, RectTransform leftLine, RectTransform rightLine, RectTransform bottom, int x, int y, int w, int h)
    {
        NsoUI.Place(top, x, y, w, 1);
        NsoUI.Place(leftLine, x, y, 1, h);
        NsoUI.Place(rightLine, x + w, y, 1, h + 1);
        NsoUI.Place(bottom, x, y + h, w + 1, 1);
    }
}

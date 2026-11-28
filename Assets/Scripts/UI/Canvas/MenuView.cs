using System.Collections.Generic;
using TMPro;
using UnityEngine;

// Canvas version of the old Menu.paintMenu: the NPC command menu, a row of square buttons (hd/btnlBig0, btnlBig1
// when selected) that slide up from the bottom and scroll sideways, with tahoma_7_yellow captions, plus the
// Chọn / Đóng bottom buttons. The cells are display only: taps and drag-scrolling on them are still handled by
// Menu.updateMenuKey, so the cells are not raycast targets. Chọn / Đóng are Canvas buttons calling Menu methods.
public class MenuView : MonoBehaviour
{
    public GameObject content;
    public UnityEngine.UI.Image cellTemplate; // child "Caption" (TMP) is the first caption line
    public UnityEngine.Sprite normal, selected;
    public UICommandButton left, center, right;

    private const float CellSize = 57.5f; // btnlBig is 115 x 115 in the x2 images
    private const int LineH = 14;
    private readonly List<UnityEngine.UI.Image> cells = new List<UnityEngine.UI.Image>();
    private readonly List<List<TMP_Text>> captions = new List<List<TMP_Text>>();

    private void Awake()
    {
        left.clickOverride = () => GameCanvas.menu.PressSelect();
        right.clickOverride = () => GameCanvas.menu.PressClose();
    }

    private void LateUpdate()
    {
        Menu menu = GameCanvas.menu;
        bool on = menu != null && menu.showMenu && GameCanvas.currentDialog == null && menu.menuItems != null && Menu.menuTemY != null;
        if (content.activeSelf != on)
            content.SetActive(on);
        if (!on)
            return;

        int count = menu.menuItems.size();
        while (cells.Count < count)
        {
            UnityEngine.UI.Image c = Instantiate(cellTemplate, cellTemplate.transform.parent);
            c.name = "Cell " + (cells.Count + 1);
            List<TMP_Text> list = new List<TMP_Text>();
            list.Add(c.GetComponentInChildren<TMP_Text>(true));
            cells.Add(c);
            captions.Add(list);
        }

        cellTemplate.gameObject.SetActive(false);
        float scroll = menu.PaintScrollX;
        for (int i = 0; i < cells.Count; i++)
        {
            bool visible = i < count;
            if (cells[i].gameObject.activeSelf != visible)
                cells[i].gameObject.SetActive(visible);
            if (!visible)
                continue;

            float rowY = menu.PaintRowY(i);
            float cellX = menu.menuX + i * menu.menuW - 1 - scroll;
            UnityEngine.UI.Image cell = cells[i];
            NsoUI.Place(cell.rectTransform, cellX, rowY + 1, CellSize, CellSize);
            cell.sprite = i == menu.menuSelectedItem ? selected : normal;

            // caption lines, centred in the cell, as in paintMenu (positions relative to the cell)
            Command item = (Command)menu.menuItems.elementAt(i);
            string[] split = item.subCaption;
            int lineCount = split == null ? 1 : split.Length;
            float textTop = (menu.menuH - lineCount * LineH) / 2 + 1 - 1; // cell top is rowY + 1
            List<TMP_Text> list = captions[i];
            while (list.Count < lineCount)
            {
                TMP_Text t = Instantiate(list[0], list[0].transform.parent);
                t.name = "Caption " + (list.Count + 1);
                list.Add(t);
            }

            for (int j = 0; j < list.Count; j++)
            {
                bool lineOn = j < lineCount;
                if (list[j].gameObject.activeSelf != lineOn)
                    list[j].gameObject.SetActive(lineOn);
                if (!lineOn)
                    continue;
                // the old caption was centred at menuX + i * menuW + menuW / 2 - 2, i.e. 1 + menuW / 2 - 2 into the cell
                float centre = 1 + menu.menuW / 2 - 2;
                NsoUI.Place(list[j].rectTransform, centre - 50, textTop + j * LineH, 100, 16);
                string s = split == null ? item.caption : split[j];
                if (list[j].text != s)
                    list[j].text = s;
            }
        }

        left.Show(menu.LeftCommand, 0);
        center.Show(menu.CenterCommand, 1);
        right.Show(menu.CloseCommand, 2);
    }
}

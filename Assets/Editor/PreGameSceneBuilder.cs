using TMPro;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

// Builds the pre-game Canvas scenes from the original screens of github.com/im-xiaoming/NinjaSchool-Unity
// (Assets/Scripts/Screens/SelectServerScr.cs, SelectCharScr.cs, CreateCharScr.cs), which this port removed.
// Static UI only (no scripts). Every position is that code's paint() at 597 x 336 (zoom x2, touch + PC, as here).
// They are added to GameMenu.unity (the scene of everything before the world: Splash, Language, Wait Panel) as
// switched-off panels of its Canvas; rebuilding replaces only these three panels.
public static class PreGameSceneBuilder
{
    private const string MenuDir = "Assets/Scenes/Menu/";
    private const string X2 = "Assets/Sprites/Resources/res/x2/";
    private const string Hd = X2 + "hd/";
    private const string Captured = "Assets/UI/Panels/Sprites/";
    private const int W = 597, H = 336, HW = 298, HH = 168;
    private const int CmdY = 308; // Paint.CmdY = h - button height (22) - Bottom Gap (6)
    private static TMP_FontAsset[] fonts;
    private static Transform root;

    [MenuItem("Tools/Ninja School/Build Pre-Game Scenes")]
    public static void BuildAll()
    {
        if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            return;
        string previous = EditorSceneManager.GetActiveScene().path;
        fonts = new TMP_FontAsset[6];
        for (int i = 0; i < 6; i++)
            fonts[i] = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>("Assets/Fonts/Resources/TTF/NsoNormalFont" + i + " SDF.asset");

        var scene = EditorSceneManager.OpenScene(MenuDir + "GameMenu.unity");
        Transform canvas = null;
        foreach (var r in scene.GetRootGameObjects())
            if (r.GetComponent<Canvas>() != null)
                canvas = r.transform;
        Build(canvas, "Title Menu", SelectServerScr);
        Build(canvas, "Character Select", SelectCharScr);
        Build(canvas, "Create Character", CreateCharScr);
        EditorSceneManager.SaveScene(scene);
        if (!string.IsNullOrEmpty(previous))
            EditorSceneManager.OpenScene(previous);
    }

    private static void Build(Transform canvas, string name, System.Action layout)
    {
        Transform old = canvas.Find(name);
        if (old != null)
            Object.DestroyImmediate(old.gameObject);
        var panel = new GameObject(name, typeof(RectTransform));
        panel.layer = 5;
        var prt = (RectTransform)panel.transform;
        prt.SetParent(canvas, false);
        prt.anchorMin = prt.anchorMax = new Vector2(0.5f, 0.5f);
        prt.sizeDelta = new Vector2(W, H);
        root = prt;
        layout();
        panel.SetActive(false);
    }

    // ---------------- SelectServerScr.paint: logo + 5 menu rows ----------------
    private static void SelectServerScr()
    {
        // g.drawImagePixelScale(imgTitle, hw, TitleLogoCenterY = h * 26 / 100, scale = round(81 * 2 / 108) = 2)
        var logo = Node("Logo", root);
        Place(logo, HW - 54, H * 26 / 100 - 31, 108, 62);
        var li = logo.gameObject.AddComponent<UnityEngine.UI.Image>();
        li.sprite = Spr(X2 + "logo.png");
        li.raycastTarget = false;

        // rows: width 115, height 20, step 29 (h > 250), x = (w - 115) / 2, y = min(h * 41 / 100, h - (4 * 29 + 20) - 4)
        string[] captions = { "Chơi mới", "Tiếp tục", "Ngôn ngữ", "Cài đặt", "Thoát" };
        int rw = 115, rh = 20, step = 29, rx = (W - rw) / 2, ry = System.Math.Max(4, System.Math.Min(H * 41 / 100, H - (4 * step + rh) - 4));
        var menu = Node("Menu", root);
        for (int i = 0; i < 5; i++)
        {
            int y = ry + i * step;
            var row = Node(captions[i], menu);
            bool selected = i == 0; // menuSelection = 0 on switchToMe
            Rect(row, "Fill", rx, y, rw, rh, selected ? Paint.COLORLIGHT : Paint.COLORDARK, true);
            FrameBorder(row, rx, y, rw, rh);
            // tahoma_7b_white caption centred on the row (TextCalib box at hw, rowY + rowHeight / 2)
            var t = Text(row, "Caption", captions[i], 3, 16777215, HW, y, 2);
            Place(t.transform, rx, y, rw, rh);
            t.alignment = TextAlignmentOptions.Midline;
        }
    }

    // ---------------- SelectCharScr.paint (touch, w > 200, PC) ----------------
    private static void SelectCharScr()
    {
        // w1char 74, padchar 25, h1char 110, x = (w - 3 * 74 >> 1) - 20, y = hh - 55
        int w1 = 74, pad = 25, h1 = 110, x = ((W - 3 * w1) >> 1) - 20, y = HH - (h1 >> 1);
        var slots = Node("Slots", root);
        for (int i = 0; i < 3; i++)
        {
            int sx = x + i * (w1 + pad);
            var slot = Node("Slot " + (i + 1), slots);
            bool selected = i == 0; // indexSelect = 0 on PC
            Rect(slot, "Fill", sx, y, w1, h1, selected ? Paint.COLORLIGHT : Paint.COLORBACKGROUND, true);
            FrameBorder(slot, sx, y, w1, h1);
        }

        // slot 1 holds the saved character: drawn around (slot centre, y + h1char / 2 - 5), name / level below
        var c = slots.Find("Slot 1");
        int cx = x + w1 / 2, cy = y + h1 / 2 - 5;
        Character(c, cx, cy);
        Text(c, "Name", "admin", 3, 15655191, cx, y + h1 / 2 + 5, 2);
        Text(c, "Level", "Level: 30", 3, 16777215, cx, y + h1 / 2 + 22, 2);

        // left = Chọn at w/2 - 160, center has no caption (not drawn), right = Thoát at w/2 + 88
        var bar = Node("Command Bar", root);
        Button(bar, "Chọn", W / 2 - 160, CmdY);
        Button(bar, "Thoát", W / 2 + 88, CmdY);
    }

    // ---------------- CreateCharScr.paint (touch) ----------------
    private static void CreateCharScr()
    {
        // setPopupSize(170, 190), popupX = (w - 170) / 2, popupY = (h - 220) / 2
        int pw = 170, ph = 190, px = (W - pw) / 2, py = (H - 220) / 2;
        var popup = Node("Popup", root);
        Rect(popup, "Fill", px - 1, py, pw + 1, ph, Paint.COLORBACKGROUND, true);
        FrameBorder(popup, px, py, pw, ph);

        // the character (hairID / defaultLeg / defaultBody parts) is drawn around (w / 2, popupY + 50); its images
        // come from the game's part data, so this is only the anchor point to put them on
        var ch = Node("Character (anchor w/2, popupY+50)", popup);
        Place(ch, W / 2 - 1, py + 50 - 1, 2, 2);

        // tAddName: TField width 100, height 26, x = popupX + popupW / 2 - 50, y = popupY + 62, focused, empty
        // so it shows title_Null = "Tên nhân vật"
        TextField(popup, "Name Field", px + pw / 2 - 50, py + 62, 100, 26, "Tên nhân vật");

        // touch: cmdGender / cmdHair are buttons at popupX + popupW / 2 - cmdW / 2, popupY + 100 / popupY + 140
        Button(popup, "Nam", px + pw / 2 - 35, py + 70 + 30);
        Button(popup, "đeo băng", px + pw / 2 - 35, py + 70 + 70);
        popup.Find("Nam").name = "Gender (Nam)";
        popup.Find("đeo băng").name = "Hair (đeo băng)";

        // left = Đóng at w/2 - 160, center = Tạo mới at w/2 - 35, right = Xóa (tAddName.cmdClear) at w/2 + 88
        var bar = Node("Command Bar", root);
        Button(bar, "Đóng", W / 2 - 160, CmdY);
        Button(bar, "Tạo mới", W / 2 - 35, CmdY);
        Button(bar, "Xóa", W / 2 + 88, CmdY);
    }

    // ---------------- pieces ----------------

    // Paint.paintFrameBorder at zoom x2: border lines, corners and the top ornament (no fill)
    private static void FrameBorder(Transform parent, int x, int y, int w, int h)
    {
        var f = Node("Border", parent);
        int nTop = w / 3 - 2, nSide = h / 2 - 2;
        if (nTop > 0)
        {
            Img(f, "Top", "bd1", x, y - 2, nTop * 3, 2, false, false, true);
            Img(f, "Bottom", "bd1", x, y + h - 1, nTop * 3, 2, true, true, true);
        }

        if (nSide > 0)
        {
            Img(f, "Left", "bd4", x - 2, y, 2, (nSide - 1) * 2 + 3, false, false, true);
            Img(f, "Right", "bd3", x + w, y, 2, (nSide - 1) * 2 + 6, false, false, true);
        }

        Img(f, "Corner TL", "bd0", x - 2, y - 2, 20, 20, false, false, false);
        Img(f, "Corner TR", "bd0", x + w + 2 - 20, y - 2, 20, 20, true, false, false);
        Img(f, "Corner BL", "bd0", x - 2, y + h + 1 - 20, 20, 20, false, true, false);
        Img(f, "Corner BR", "bd0", x + w + 2 - 20, y + h + 1 - 20, 20, 20, true, true, false);
        Img(f, "Ornament", "bd2", x + w / 2f - 17.5f, y - 7, 35, 11, false, false, false);
    }

    // Command.paint: btnl 70 x 22 with a tahoma_7b_yellow caption
    private static void Button(Transform parent, string caption, int x, int y)
    {
        var g = Node(caption, parent);
        Place(g, x, y, 70, 22);
        var im = g.gameObject.AddComponent<UnityEngine.UI.Image>();
        im.sprite = Spr(Hd + "btnl.png");
        var b = g.gameObject.AddComponent<Button>();
        b.targetGraphic = im;
        b.transition = Selectable.Transition.SpriteSwap;
        var ss = b.spriteState;
        ss.highlightedSprite = ss.pressedSprite = ss.selectedSprite = Spr(Hd + "btnlf.png");
        b.spriteState = ss;
        var t = Text(g, "Caption", caption, 3, 16755200, 0, 0, 2);
        var tr = t.rectTransform;
        tr.anchorMin = Vector2.zero;
        tr.anchorMax = Vector2.one;
        tr.sizeDelta = Vector2.zero;
        tr.anchoredPosition = new Vector2(1, 0);
        t.margin = new Vector4(0, 3, 0, 0);
    }

    // TField.paint / paintInputTf (focused): hd/tf slices, text at x + TEXT_GAP_X + 1, y + (h - lineH) / 2 + 1
    private static void TextField(Transform parent, string name, int x, int y, int w, int h, string placeholder)
    {
        var field = Node(name, parent);
        Place(field, x, y, w, h);
        var tex = AssetDatabase.LoadAssetAtPath<Texture2D>(Hd + "tf.png");
        System.Action<string, int, int> slice = (n, sx, srcY) =>
        {
            var s = Node(n, field);
            var rr = (RectTransform)s;
            rr.anchorMin = rr.anchorMax = new Vector2(0, 1);
            rr.pivot = new Vector2(0, 1);
            rr.sizeDelta = new Vector2(29, 27);
            rr.anchoredPosition = new Vector2(sx, 0);
            var ri = s.gameObject.AddComponent<RawImage>();
            ri.texture = tex;
            ri.raycastTarget = false;
            ri.uvRect = new Rect(0, 1f - (srcY + 27) / 162f, 1, 27 / 162f);
        };
        slice("Left", 0, 81);
        slice("Right", w - 29, 135);
        slice("Middle End", w - 58, 108);
        for (int i = 0; i < (w - 58) / 29; i++)
            slice("Middle " + (i + 1), 29 + i * 29, 108);

        var area = Node("Text Area", field);
        var art = (RectTransform)area;
        art.anchorMin = Vector2.zero;
        art.anchorMax = Vector2.one;
        art.offsetMin = new Vector2(3, 1);
        art.offsetMax = new Vector2(-1, -1);
        area.gameObject.AddComponent<RectMask2D>();
        var ph = FillText(Text(area, "Placeholder", placeholder, 3, 15655191, 0, 0, 0));
        var tt = FillText(Text(area, "Text", "", 3, 15655191, 0, 0, 0));
        var input = field.gameObject.AddComponent<TMP_InputField>();
        input.textViewport = art;
        input.textComponent = tt;
        input.placeholder = ph;
        input.fontAsset = fonts[3];
        input.pointSize = 10.5f;
        input.caretColor = NsoUIColors.Rgb(11184810);
        input.customCaretColor = true;
        input.caretWidth = 1;
        var hit = field.gameObject.AddComponent<UnityEngine.UI.Image>();
        hit.color = new Color(1, 1, 1, 0);
        input.targetGraphic = hit;
    }

    private static TextMeshProUGUI FillText(TextMeshProUGUI t)
    {
        var r = t.rectTransform;
        r.anchorMin = Vector2.zero;
        r.anchorMax = Vector2.one;
        r.sizeDelta = Vector2.zero;
        r.anchoredPosition = Vector2.zero;
        t.alignment = TextAlignmentOptions.MidlineLeft;
        t.margin = new Vector4(2, 0, 0, 0);
        return t;
    }

    // the saved character "admin" (head, body, legs as the game drew them, captured from Trang bị),
    // centred on cx with its feet at feetY
    private static void Character(Transform parent, int cx, int feetY)
    {
        var c = Node("Character", parent);
        string[] files = { "4aeb072a1a18b014a1caba106b2927e2", "aaafdce53ec2cf3884486e9e271efc8a", "d570be69612a75235249b6cf4cd8530a", "02a2ba63d9c4d679ddf813e666b5f60d" };
        string[] names = { "Leg", "Leg 2", "Body", "Head" };
        float[,] r = { { 0, 18, 26, 16 }, { 6, 30, 15, 8 }, { 2, 21, 21, 11 }, { 1, 0, 24, 23 } };
        float bx = cx - 13, by = feetY - 38;
        for (int i = 0; i < files.Length; i++)
        {
            var p = Node(names[i], c);
            Place(p, bx + r[i, 0], by + r[i, 1], r[i, 2], r[i, 3]);
            var im = p.gameObject.AddComponent<UnityEngine.UI.Image>();
            im.sprite = Spr(Captured + files[i] + ".png");
            im.raycastTarget = false;
        }
    }

    // ---------------- primitives (game pixels from the top-left of the 597 x 336 screen) ----------------

    private static UnityEngine.Sprite Spr(string path)
    {
        return AssetDatabase.LoadAssetAtPath<UnityEngine.Sprite>(path);
    }

    private static Transform Node(string name, Transform parent)
    {
        var g = new GameObject(name, typeof(RectTransform));
        g.layer = 5;
        var rt = (RectTransform)g.transform;
        rt.SetParent(parent, false);
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.sizeDelta = Vector2.zero;
        return g.transform;
    }

    private static void Place(Transform t, float x, float y, float w, float h)
    {
        var rt = (RectTransform)t;
        rt.anchorMin = rt.anchorMax = new Vector2(0, 1);
        rt.pivot = new Vector2(0.5f, 0.5f);
        rt.sizeDelta = new Vector2(w, h);
        rt.anchoredPosition = new Vector2(x + w / 2f, -(y + h / 2f));
    }

    private static void Rect(Transform parent, string name, float x, float y, float w, float h, int color, bool raycast)
    {
        var g = Node(name, parent);
        Place(g, x, y, w, h);
        var im = g.gameObject.AddComponent<UnityEngine.UI.Image>();
        im.color = NsoUIColors.Rgb(color);
        im.raycastTarget = raycast;
    }

    private static void Img(Transform parent, string name, string file, float x, float y, float w, float h, bool fx, bool fy, bool tiled)
    {
        var g = Node(name, parent);
        Place(g, x, y, w, h);
        g.localScale = new Vector3(fx ? -1 : 1, fy ? -1 : 1, 1);
        var im = g.gameObject.AddComponent<UnityEngine.UI.Image>();
        im.sprite = Spr(Hd + file + ".png");
        im.raycastTarget = false;
        if (tiled)
        {
            im.type = UnityEngine.UI.Image.Type.Tiled;
            im.pixelsPerUnitMultiplier = 2;
        }
    }

    // mFont.drawString: align 0 = left at x, 2 = centred on x; y = top of the line. Font 3 = tahoma_8b / 7b.
    private static TextMeshProUGUI Text(Transform parent, string name, string s, int font, int color, float x, float y, int align)
    {
        var g = Node(name, parent);
        float w = 300, size = font == 1 ? 7.875f : 10.5f;
        Place(g, align == 2 ? x - w / 2 : x, y, w, size * 1.6f);
        var t = g.gameObject.AddComponent<TextMeshProUGUI>();
        t.font = fonts[font];
        t.fontSize = size;
        t.color = NsoUIColors.Rgb(color);
        t.text = s;
        t.raycastTarget = false;
        t.textWrappingMode = TextWrappingModes.NoWrap;
        t.margin = Vector4.zero;
        t.alignment = align == 2 ? TextAlignmentOptions.Top : TextAlignmentOptions.TopLeft;
        return t;
    }
}

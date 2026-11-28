using System.IO;
using TMPro;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

// Builds Canvas scenes from the captured panels (Assets/UI/Panels/Captures/*.json, made by PanelCapture in Play mode)
// into Assets/Scenes/Panels/: one scene per group of related panels (Groups below), each panel a child of the Canvas,
// the first one shown and the others switched off. Every drawn image, filled rectangle and text of the panel becomes a UI object at the
// same game-pixel position: Screen Space - Camera canvas, reference resolution = the game size at capture time.
public static class PanelSceneBuilder
{
    private const string Root = "Assets/UI/Panels";
    private const string SceneDir = "Assets/Scenes/Panels";
    // NsoNormalFont0..5 pixel size at zoom x1; TMP size = 0.875 x that (matches the existing menu scenes)
    private static readonly int[] FontPx = { 5, 9, 11, 12, 13, 13 };

    // scene name -> the captured panels it holds (capture file names)
    private static readonly string[][] Groups =
    {
        new[] { "HUD", "HUD" },
        new[] { "Character", "Hanh Trang", "Trang Bi", "Thong Tin", "Ky Nang", "Tiem Nang", "Thu Cuoi", "Vi Thu" },
        new[] { "Shops", "Item Shop", "Store", "Elite Shop", "Lucky Spin", "Auction Sale", "Auction Buy" },
        new[] { "Crafting", "Upgrade", "Convert", "Split", "Up Pearl", "Luyen Ngoc", "Kham Ngoc", "Giao Do", "Got Ngoc" },
        new[] { "Social", "Team", "Find Team", "Friends", "List", "Ranked List", "Clan", "Chars In Map", "Trade" },
        new[] { "Others", "Auto", "Task", "Zone", "Alert", "Item Info" },
    };

    [MenuItem("Tools/Ninja School/Build Panel Scenes")]
    public static void BuildAll()
    {
        if (!Directory.Exists(Root + "/Captures"))
        {
            Debug.LogWarning("No captures in " + Root + "/Captures. Run PanelCapture in Play mode first.");
            return;
        }

        if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            return;
        string previous = EditorSceneManager.GetActiveScene().path;
        ImportSprites();
        Directory.CreateDirectory(SceneDir);
        int n = 0;
        foreach (string[] group in Groups)
        {
            var caps = new System.Collections.Generic.List<PanelCapture.Capture>();
            for (int i = 1; i < group.Length; i++)
            {
                string json = Root + "/Captures/" + group[i] + ".json";
                if (File.Exists(json))
                    caps.Add(JsonUtility.FromJson<PanelCapture.Capture>(File.ReadAllText(json)));
            }

            if (caps.Count == 0)
                continue;
            Build(group[0], caps);
            n++;
        }

        AssetDatabase.SaveAssets();
        if (!string.IsNullOrEmpty(previous))
            EditorSceneManager.OpenScene(previous);
        Debug.Log("Built " + n + " panel scenes in " + SceneDir);
    }

    private static void ImportSprites()
    {
        AssetDatabase.Refresh();
        foreach (string file in Directory.GetFiles(Root + "/Sprites", "*.png"))
        {
            string path = file.Replace('\\', '/');
            TextureImporter imp = AssetImporter.GetAtPath(path) as TextureImporter;
            if (imp == null || imp.textureType == TextureImporterType.Sprite)
                continue;
            imp.textureType = TextureImporterType.Sprite;
            imp.spriteImportMode = SpriteImportMode.Single;
            imp.spritePixelsPerUnit = 100;
            imp.mipmapEnabled = false;
            imp.alphaIsTransparency = true;
            imp.filterMode = FilterMode.Point; // pixel art: no blending between texels
            imp.textureCompression = TextureImporterCompression.Uncompressed;
            imp.SaveAndReimport();
        }
    }

    private static void Build(string sceneName, System.Collections.Generic.List<PanelCapture.Capture> caps)
    {
        PanelCapture.Capture cap0 = caps[0];
        var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

        var camGo = new GameObject("Main Camera", typeof(Camera));
        camGo.tag = "MainCamera";
        var cam = camGo.GetComponent<Camera>();
        cam.orthographic = true;
        cam.orthographicSize = cap0.gameH / 100f;
        cam.clearFlags = CameraClearFlags.SolidColor;
        cam.backgroundColor = new Color(0.12f, 0.12f, 0.12f);
        camGo.transform.position = new Vector3(0, 0, -10);

        var canvasGo = new GameObject("Canvas", typeof(RectTransform), typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
        canvasGo.layer = 5;
        var canvas = canvasGo.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        canvas.worldCamera = cam;
        canvas.planeDistance = 5;
        canvas.sortingLayerName = "UI";
        canvas.pixelPerfect = true; // snap UI to screen pixels so the pixel art stays sharp
        var scaler = canvasGo.GetComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(cap0.gameW, cap0.gameH);
        scaler.matchWidthOrHeight = 1f;

        var es = new GameObject("EventSystem", typeof(UnityEngine.EventSystems.EventSystem));
        var module = System.Type.GetType("UnityEngine.InputSystem.UI.InputSystemUIInputModule, Unity.InputSystem");
        if (module != null)
            es.AddComponent(module);
        else
            es.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();

        bool firstPanel = true;
        foreach (PanelCapture.Capture cap in caps)
        {
        // the game screen, children placed in game pixels from its top-left corner
        var panel = new GameObject(cap.name, typeof(RectTransform));
        panel.layer = 5;
        var prt = (RectTransform)panel.transform;
        prt.SetParent(canvasGo.transform, false);
        prt.anchorMin = prt.anchorMax = new Vector2(0.5f, 0.5f);
        prt.sizeDelta = new Vector2(cap.gameW, cap.gameH);

        TMP_FontAsset[] fonts = new TMP_FontAsset[6];
        for (int i = 0; i < 6; i++)
            fonts[i] = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>("Assets/Fonts/Resources/TTF/NsoNormalFont" + i + " SDF.asset");

        int index = 0;
        var ops = cap.ops;
        var used = new bool[ops.Count];
        for (int k = 0; k < ops.Count; k++)
        {
            if (used[k])
                continue;
            DrawRecorder.Op op = ops[k];
            index++;
            if (op.kind == "image")
            {
                var sprite = AssetDatabase.LoadAssetAtPath<UnityEngine.Sprite>(Root + "/Sprites/" + op.sprite);
                if (sprite == null)
                    continue;
                // the same tile repeated edge to edge in a row or column (frame borders) becomes one tiled Image.
                // The game draws e.g. top and bottom border pieces alternately, so look a few draws ahead.
                // pieces may overlap (a 2 x 3 tile drawn every 2 px): the step is the distance between pieces
                int run = 1, at = k;
                bool horizontal = true;
                float step = 0;
                DrawRecorder.Op last = op;
                while (true)
                {
                    int found = -1;
                    for (int j = at + 1; j < ops.Count && j <= at + 4; j++)
                    {
                        if (used[j] || !SameTile(op, ops[j]))
                            continue;
                        DrawRecorder.Op next = ops[j];
                        float dx = next.x - last.x, dy = next.y - last.y;
                        bool h = Mathf.Abs(dy) < 0.01f && dx > 0.5f && dx <= Mathf.Abs(op.w) + 0.5f;
                        bool v = Mathf.Abs(dx) < 0.01f && dy > 0.5f && dy <= Mathf.Abs(op.h) + 0.5f;
                        bool ok = run == 1 ? (h || v) : horizontal ? (h && Mathf.Abs(dx - step) < 0.01f) : (v && Mathf.Abs(dy - step) < 0.01f);
                        if (ok)
                        {
                            if (run == 1)
                            {
                                horizontal = h;
                                step = h ? dx : dy;
                            }

                            found = j;
                            break;
                        }
                    }

                    if (found < 0)
                        break;
                    used[found] = true;
                    last = ops[found];
                    at = found;
                    run++;
                }

                var go = Node((run > 1 ? "Tiled " : "Image ") + index, prt);
                var img = go.AddComponent<UnityEngine.UI.Image>();
                img.sprite = sprite;
                img.raycastTarget = false;
                if (run > 1)
                {
                    float size = horizontal ? Mathf.Abs(op.w) : Mathf.Abs(op.h);
                    // overlapping pieces: each shows only its first `step` pixels, so tile that strip of the sprite
                    // and keep the last piece whole as its own Image
                    int tiles = run;
                    if (step < size - 0.01f)
                    {
                        img.sprite = Strip(op.sprite, sprite, horizontal, step / size, horizontal ? op.w < 0 : op.h < 0);
                        tiles = run - 1;
                        var end = Node("Image " + index + " end", prt);
                        Place(end, last.x, last.y, op.w, op.h);
                        var endImg = end.AddComponent<UnityEngine.UI.Image>();
                        endImg.sprite = sprite;
                        endImg.raycastTarget = false;
                        end.transform.SetSiblingIndex(go.transform.GetSiblingIndex() + 1);
                    }

                    float span = tiles * step;
                    float tw = horizontal ? span : Mathf.Abs(op.w);
                    float th = horizontal ? Mathf.Abs(op.h) : span;
                    // pieces are stored by their drawn rect: a mirrored one (negative size) by its right / bottom edge
                    float x0 = op.w < 0 ? op.x - Mathf.Abs(op.w) : op.x, y0 = op.h < 0 ? op.y - Mathf.Abs(op.h) : op.y;
                    Place(go, op.w < 0 ? x0 + tw : x0, op.h < 0 ? y0 + th : y0, op.w < 0 ? -tw : tw, op.h < 0 ? -th : th);
                    img.type = UnityEngine.UI.Image.Type.Tiled;
                    // keep each tile at its drawn size: sprite pixels per game pixel (across the run)
                    img.pixelsPerUnitMultiplier = horizontal ? img.sprite.rect.height / Mathf.Abs(op.h) : img.sprite.rect.width / Mathf.Abs(op.w);
                }
                else
                {
                    Place(go, op.x, op.y, op.w, op.h);
                }

                if (op.color.a > 0 && op.color != Color.white)
                    img.color = op.color;
                if (op.rotation != 0)
                    go.transform.localRotation = Quaternion.Euler(0, 0, -op.rotation);
            }
            else if (op.kind == "fill")
            {
                var go = Node("Rect " + index, prt);
                Place(go, op.x, op.y, op.w, op.h);
                var img = go.AddComponent<UnityEngine.UI.Image>();
                img.color = op.color;
                img.raycastTarget = false;
            }
            else if (op.kind == "text")
            {
                // HUD numbers drawn through TextCalib boxes are recorded at (0, 0); HudExtras places them from the code
                if (cap.name == "HUD" && op.x < 1 && op.y < 2)
                    continue;
                // the game draws a 1 px shadow copy first, then the text: two overlapping TMP texts look blurry, keep the text only
                if (IsShadowOf(op, ops, k))
                    continue;
                var go = Node("Text " + index + " " + Short(op.text), prt);
                const float w = 400;
                float left = op.align == 0 ? op.x : op.align == 1 ? op.x - w : op.x - w / 2;
                int f = Mathf.Clamp(op.font, 0, 5);
                float size = FontPx[f] * 0.875f;
                Place(go, left, op.y, w, size * 1.6f);
                var t = go.AddComponent<TextMeshProUGUI>();
                t.font = fonts[f];
                t.fontSize = size;
                t.text = op.text;
                t.color = new Color(op.color.r, op.color.g, op.color.b, 1);
                t.raycastTarget = false;
                t.textWrappingMode = TextWrappingModes.NoWrap;
                t.overflowMode = TextOverflowModes.Overflow;
                t.margin = Vector4.zero;
                t.alignment = op.align == 0 ? TextAlignmentOptions.TopLeft : op.align == 1 ? TextAlignmentOptions.TopRight : TextAlignmentOptions.Top;
            }
        }

        if (cap.name == "HUD")
        {
            HudExtras(prt, fonts, cap);
            GroupHud(prt);
            GroupSkills((RectTransform)prt.Find("Skill Bar"), cap);
            GroupStatusBar((RectTransform)prt.Find("Status Bar"));
            SetupExpSlider((RectTransform)prt.Find("EXP Bar"), cap.gameW - 61); // expBarW = gW - 61
        }

        panel.SetActive(firstPanel);
        firstPanel = false;
        }

        EditorSceneManager.SaveScene(scene, SceneDir + "/" + sceneName + ".unity");
    }

    // HUD parts the capture missed (drawn after GameScr.paint switches the recorder to "panel", or through TextCalib boxes).
    // Positions follow the game code for the PC touch layout (Main.isPC, Info.hI = 0 as in the capture):
    // GameScr.loadInforBar / paintInfoBar / paintTouchControl and the menu command (Command.paint with imgDrawSize).
    // Numbers are runtime values: the texts below are samples.
    private static void HudExtras(RectTransform prt, TMP_FontAsset[] fonts, PanelCapture.Capture cap)
    {
        int w = cap.gameW, h = cap.gameH;
        const int hpBarX = 52, hpBarY = 10;          // loadInforBar: hpBarY = 10 + Info.hI
        const int topIconSize = 24, topIconGap = 14; // GameScr constants
        const int potionBottomGap = 6;               // NsoUiLayout.Potions.bottomGap

        // level badge + numbers on the top bar (paintInfoBar, TextCalib box centres)
        Color levelColor = new Color(1f, 0.8f, 0f), levelShadow = Color.black;
        foreach (DrawRecorder.Op op in cap.ops)
            if (op.kind == "text" && op.x < 1 && op.y < 2)
            {
                if (op.y >= 1) levelShadow = op.color; else levelColor = op.color;
            }
        float badgeCenterX = hpBarX - 28.17f;
        HudText(prt, fonts, "Level", "30", 5, levelColor, badgeCenterX - 1f, hpBarY + 12.33f, 2);
        HudText(prt, fonts, "HP Value", "1000", 0, Color.white, hpBarX + 9.67f, hpBarY + 4.67f, 0);
        HudText(prt, fonts, "MP Value", "1000", 0, Color.white, hpBarX + 9.67f, hpBarY + 19.33f, 0);
        HudText(prt, fonts, "EXP Percent", "0.00%", 0, Color.white, badgeCenterX, hpBarY + 22f, 2);

        // menu and chat icons, left of the minimap (loadInforBar, PC branch)
        int topY = (42 - topIconSize) / 2;
        int chatX = w - 60 - topIconGap - topIconSize;
        int menuX = chatX - topIconGap - topIconSize;
        HudImage(prt, "Menu Button", "menu", menuX, topY, topIconSize, topIconSize);
        HudImage(prt, "Chat Button", "chat", chatX, topY, topIconSize, topIconSize);

        // HP / MP potion buttons (paintTouchControl, isPcLevelSmall): xHP = w - 85, xMP = w - 40
        UnityEngine.Sprite button = HdSprite("button");
        int bw = button != null ? Mathf.RoundToInt(button.rect.width / 3f) : 34;
        int bh = button != null ? Mathf.RoundToInt(button.rect.height / 3f) : 34;
        int py = h - bh - potionBottomGap;
        string[] names = { "HP Potion", "MP Potion" };
        string[] icons = { "hpp", "mpp" };
        int[] xs = { w - 85, w - 40 };
        for (int i = 0; i < 2; i++)
        {
            var root = Node(names[i], prt);
            Place(root, xs[i], py, bw, bh);
            var btn = HudImage(prt, "Button", "button", xs[i], py, bw, bh);
            btn.transform.SetParent(root.transform, true);
            SetupPotionButton(btn);
            UnityEngine.Sprite icon = HdSprite(icons[i]);
            float iw = icon != null ? icon.rect.width / 3f : 16, ih = icon != null ? icon.rect.height / 3f : 16;
            HudImage(prt, "Icon", icons[i], xs[i] + 17 - iw / 2f, py + 15 - ih / 2f, iw, ih).transform.SetParent(root.transform, true);
            // count: number_white, right-aligned at (x + 30, y + 22), y = top of the text
            float size = FontPx[0] * 0.875f;
            HudText(prt, fonts, "Count", "0", 0, Color.white, xs[i] + 30, py + 22 + size * 0.8f, 1).transform.SetParent(root.transform, true);
        }
    }

    // HUD groups in draw order; each is a panel-sized container, so children keep their positions
    private static readonly string[] HudGroups = { "EXP Bar", "Status Bar", "Task Info", "Minimap", "Skill Bar", "Top Buttons", "Potions" };

    public static void GroupHud(RectTransform prt)
    {
        var groups = new RectTransform[HudGroups.Length];
        var children = new System.Collections.Generic.List<RectTransform>();
        foreach (Transform c in prt)
            if (System.Array.IndexOf(HudGroups, c.name) < 0)
                children.Add((RectTransform)c);
        for (int g = 0; g < HudGroups.Length; g++)
        {
            Transform existing = prt.Find(HudGroups[g]);
            var rt = existing != null ? (RectTransform)existing : (RectTransform)Node(HudGroups[g], prt).transform;
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.pivot = new Vector2(0.5f, 0.5f);
            rt.sizeDelta = Vector2.zero;
            rt.anchoredPosition = Vector2.zero;
            rt.SetSiblingIndex(g);
            groups[g] = rt;
        }

        foreach (RectTransform c in children)
        {
            int g = HudGroupOf(c, prt.rect.width);
            if (g >= 0)
                c.SetParent(groups[g], false);
        }
    }

    private static int HudGroupOf(RectTransform c, float gameW)
    {
        string n = c.name;
        if (n == "HP Potion" || n == "MP Potion") return 6;
        if (n == "Menu Button" || n == "Chat Button") return 5;
        if (n.StartsWith("Level") || n == "HP Value" || n == "MP Value" || n == "EXP Percent") return 1;
        // centre and size in game pixels from the top-left
        float w = Mathf.Abs(c.sizeDelta.x), h = Mathf.Abs(c.sizeDelta.y);
        float cx = c.anchoredPosition.x, cy = -c.anchoredPosition.y;
        float top = cy - h / 2f;
        if (cy > 280) return 4;                              // skill slots along the bottom
        if (cx - w / 2f >= gameW - 62 && top < 45) return 3; // minimap at w - 60
        if (top < 4.5f && h <= 3.5f) return 0;               // exp bar strip and its ticks at the very top
        if (top < 50 && cx < 300) return 1;                  // HP / MP bars and the top bar frame
        return 2;                                            // task, target, map, time, site lines
    }

    // One "Skill N" per slot (paintSelectedSkill: imgSkill frame, the skill icon centred in it, and the selected skill's
    // focus frame). Slots sit every 25 + padSkill = 30 px, laid out by a HorizontalLayoutGroup (spacing 30 - 26 = 4).
    // Children are named "<kind> <op number>" by Build, so their game rects come from the capture ops.
    public static void GroupSkills(RectTransform bar, PanelCapture.Capture cap)
    {
        var kids = new System.Collections.Generic.List<RectTransform>();
        foreach (Transform c in bar)
            kids.Add((RectTransform)c);
        var slots = new System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<RectTransform, Rect>>();
        var others = new System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<RectTransform, Rect>>();
        foreach (RectTransform c in kids)
        {
            int n;
            string[] parts = c.name.Split(' ');
            if (parts.Length < 2 || !int.TryParse(parts[1], out n) || n < 1 || n > cap.ops.Count)
                continue;
            DrawRecorder.Op op = cap.ops[n - 1];
            var r = new Rect(op.x, op.y, Mathf.Abs(op.w), Mathf.Abs(op.h));
            if (op.kind == "image" && Mathf.Abs(op.w - 26) < 0.5f && Mathf.Abs(op.h - 26) < 0.5f)
                slots.Add(new System.Collections.Generic.KeyValuePair<RectTransform, Rect>(c, r));
            else
                others.Add(new System.Collections.Generic.KeyValuePair<RectTransform, Rect>(c, r));
        }

        if (slots.Count == 0)
            return;
        slots.Sort((a, b) => a.Value.x.CompareTo(b.Value.x));
        Rect first = slots[0].Value, lastSlot = slots[slots.Count - 1].Value;
        const float step = 30, slot = 26;

        // the bar: exactly the slots, keeping whatever anchors it has (bottom-centre if set in the scene)
        float barW = lastSlot.xMax - first.x, gw = cap.gameW, gh = cap.gameH;
        Vector2 a0 = bar.anchorMin;
        if (bar.anchorMin != bar.anchorMax)
            a0 = bar.anchorMin = bar.anchorMax = new Vector2(0.5f, 0f);
        bar.pivot = new Vector2(0.5f, 0.5f);
        bar.sizeDelta = new Vector2(barW, slot);
        float cx = first.x + barW / 2f, cy = first.y + slot / 2f; // game coords from top-left
        bar.anchoredPosition = new Vector2(cx - a0.x * gw, (gh - cy) - a0.y * gh);

        var layout = bar.GetComponent<UnityEngine.UI.HorizontalLayoutGroup>() ?? bar.gameObject.AddComponent<UnityEngine.UI.HorizontalLayoutGroup>();
        layout.padding = new RectOffset(0, 0, 0, 0);
        layout.spacing = step - slot;
        layout.childAlignment = TextAnchor.MiddleLeft;
        layout.childControlWidth = layout.childControlHeight = false;
        layout.childForceExpandWidth = layout.childForceExpandHeight = false;

        for (int i = 0; i < slots.Count; i++)
        {
            Rect sr = slots[i].Value;
            Transform old = bar.Find("Skill " + (i + 1));
            var group = old != null ? (RectTransform)old : (RectTransform)Node("Skill " + (i + 1), bar).transform;
            group.anchorMin = group.anchorMax = new Vector2(0, 0.5f);
            group.pivot = new Vector2(0.5f, 0.5f);
            group.sizeDelta = new Vector2(slot, slot);
            group.SetSiblingIndex(i);
            RectTransform focus = null;

            var members = new System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<RectTransform, Rect>>();
            members.Add(slots[i]);
            foreach (var o in others)
                if (sr.Contains(o.Value.center))
                    members.Add(o);
            foreach (var m in members)
            {
                RectTransform c = m.Key;
                Rect r = m.Value;
                bool isRect = c.name.StartsWith("Rect");
                Transform parent = group;
                if (isRect)
                {
                    if (focus == null)
                    {
                        Transform f = group.Find("Focus");
                        focus = f != null ? (RectTransform)f : (RectTransform)Node("Focus", group).transform;
                        focus.anchorMin = Vector2.zero;
                        focus.anchorMax = Vector2.one;
                        focus.sizeDelta = Vector2.zero;
                        focus.anchoredPosition = Vector2.zero;
                    }

                    parent = focus;
                }

                c.SetParent(parent, false);
                c.name = m.Key == slots[i].Key ? "Slot" : isRect ? "Focus Line" : "Icon";
                c.anchorMin = c.anchorMax = new Vector2(0, 1);
                c.pivot = new Vector2(0.5f, 0.5f);
                c.sizeDelta = new Vector2(r.width, r.height);
                c.anchoredPosition = new Vector2(r.x - sr.x + r.width / 2f, -(r.y - sr.y + r.height / 2f));
            }

            if (focus != null)
                focus.SetAsLastSibling(); // game order: slot frame, focus frame, icon
            Transform icon = group.Find("Icon");
            if (icon != null)
                icon.SetAsLastSibling();
        }
    }

    // Status Bar (paintInfoBar draws, in order: HP background, HP fill, MP background, MP fill, imgTopBar, then the numbers)
    // → HP Bar, MP Bar (Background, Fill, Value), Frame, Level Badge (Level Shadow, Level, EXP Percent).
    // The value texts go with their bars, under the frame: they sit inside the frame's open bar window.
    public static void GroupStatusBar(RectTransform bar)
    {
        if (bar == null || bar.Find("HP Bar") != null)
            return;
        var rects = new System.Collections.Generic.List<RectTransform>();
        RectTransform frame = null;
        foreach (Transform c in bar)
        {
            if (c.name.StartsWith("Rect"))
                rects.Add((RectTransform)c);
            else if (c.name.StartsWith("Image") && frame == null)
                frame = (RectTransform)c;
        }

        if (rects.Count < 4 || frame == null)
            return;
        Wrap(bar, "HP Bar", new[] { rects[0], rects[1], (RectTransform)bar.Find("HP Value") }, new[] { "Background", "Fill", "Value" });
        Wrap(bar, "MP Bar", new[] { rects[2], rects[3], (RectTransform)bar.Find("MP Value") }, new[] { "Background", "Fill", "Value" });
        // MP: background (hpBarX - 28, hpBarY + 13, 85, 11), fill (hpBarX, hpBarY + 16, mpBarW 57, 7), colour 21989
        SetupBarSlider((RectTransform)bar.Find("MP Bar"), new Rect(24, 23, 85, 11), new Rect(52, 26, 57, 7), MpColor);
        frame.name = "Frame";
        frame.SetAsLastSibling();
        Wrap(bar, "Level Badge", new[] { (RectTransform)bar.Find("Level"), (RectTransform)bar.Find("Level Shadow"), (RectTransform)bar.Find("EXP Percent") }, null);
    }

    // A bar (Background, Fill, Value) as a display-only Slider: Background, Slider > Fill Area > Fill, Value.
    // gameRect = the bar background in game px (paintInfoBar's black 0.8 rect), fillRect = full-width fill in game px.
    // value 1 = full bar (the game draws fill width = current * barW / max).
    public static readonly Color MpColor = new Color32(0x00, 0x55, 0xE5, 0xFF); // 21989
    public static readonly Color ExpBackColor = new Color32(0x5E, 0x7B, 0x00, 0xFF); // 6191872
    public static readonly Color ExpFillColor = new Color32(0x60, 0xE5, 0x00, 0xFF); // 6350080

    // EXP Bar (paintInfoBar): fillRect(0, hpBarY - 10, expBarW, 3) in 6191872, the exp part (w2) in 6350080,
    // then brown lines / ticks over it. As a display-only Slider (value = cExpR / exps[level]) under the lines.
    // In a fresh build the captured background rect (first child, full width x 3) becomes the Slider.
    public static void SetupExpSlider(RectTransform expBar, float expBarW)
    {
        UnityEngine.UI.Slider slider = expBar.GetComponentInChildren<UnityEngine.UI.Slider>(true);
        if (slider == null)
        {
            // build: background rect -> removed (the Slider draws it); lines and ticks -> "Lines"
            var lines = (RectTransform)Node("Lines", expBar).transform;
            lines.anchorMin = Vector2.zero;
            lines.anchorMax = Vector2.one;
            lines.sizeDelta = Vector2.zero;
            lines.anchoredPosition = Vector2.zero;
            var kids = new System.Collections.Generic.List<Transform>(EnumChildren(expBar));
            bool first = true;
            foreach (Transform c in kids)
            {
                if (c == lines) continue;
                if (first) { first = false; Object.DestroyImmediate(c.gameObject); continue; }
                c.SetParent(lines, false);
            }

            slider = Node("Slider", expBar).AddComponent<UnityEngine.UI.Slider>();
            slider.transform.SetAsFirstSibling();
            slider.value = 0;
        }

        var srt = (RectTransform)slider.transform;
        srt.anchorMin = srt.anchorMax = new Vector2(0, 1);
        srt.pivot = new Vector2(0, 1);
        srt.sizeDelta = new Vector2(expBarW, 3);
        srt.anchoredPosition = Vector2.zero; // (0, hpBarY - 10) = top-left
        srt.SetAsFirstSibling();             // lines and ticks drawn over it

        Transform bgT = slider.transform.Find("Background");
        var bg = bgT != null ? (RectTransform)bgT : (RectTransform)Node("Background", slider.transform).transform;
        Stretch(bg);
        bg.SetAsFirstSibling();
        var bgImg = bg.GetComponent<UnityEngine.UI.Image>() ?? bg.gameObject.AddComponent<UnityEngine.UI.Image>();
        bgImg.sprite = null;
        bgImg.type = UnityEngine.UI.Image.Type.Simple;
        bgImg.color = ExpBackColor;
        bgImg.raycastTarget = false;

        Transform handle = slider.transform.Find("Handle Slide Area");
        if (handle != null)
            Object.DestroyImmediate(handle.gameObject);
        Transform areaT = slider.transform.Find("Fill Area");
        var area = areaT != null ? (RectTransform)areaT : (RectTransform)Node("Fill Area", slider.transform).transform;
        Stretch(area);
        Transform fillT = area.Find("Fill");
        var fill = fillT != null ? (RectTransform)fillT : (RectTransform)Node("Fill", area).transform;
        fill.sizeDelta = Vector2.zero;
        var fillImg = fill.GetComponent<UnityEngine.UI.Image>() ?? fill.gameObject.AddComponent<UnityEngine.UI.Image>();
        fillImg.sprite = null;
        fillImg.type = UnityEngine.UI.Image.Type.Simple;
        fillImg.color = ExpFillColor;
        fillImg.raycastTarget = false;

        slider.fillRect = fill;
        slider.handleRect = null;
        slider.targetGraphic = null;
        slider.direction = UnityEngine.UI.Slider.Direction.LeftToRight;
        slider.minValue = 0;
        slider.maxValue = 1;
        slider.wholeNumbers = false;
        slider.interactable = false;
        slider.transition = UnityEngine.UI.Selectable.Transition.None;
        var nav = slider.navigation;
        nav.mode = UnityEngine.UI.Navigation.Mode.None;
        slider.navigation = nav;
    }

    private static void Stretch(RectTransform rt)
    {
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.pivot = new Vector2(0.5f, 0.5f);
        rt.sizeDelta = Vector2.zero;
        rt.anchoredPosition = Vector2.zero;
    }

    public static void SetupBarSlider(RectTransform bar, Rect gameRect, Rect fillRect, Color fillColor)
    {
        // the bar back at its game rect (Status Bar is panel-sized, top-left = game 0,0)
        bar.anchorMin = bar.anchorMax = new Vector2(0, 1);
        bar.pivot = new Vector2(0.5f, 0.5f);
        bar.sizeDelta = gameRect.size;
        bar.anchoredPosition = new Vector2(gameRect.x + gameRect.width / 2f, -(gameRect.y + gameRect.height / 2f));

        Transform bg = null, value = null, oldFill = null;
        UnityEngine.UI.Slider slider = null;
        foreach (Transform c in bar)
        {
            if (c.GetComponent<UnityEngine.UI.Slider>() != null) slider = c.GetComponent<UnityEngine.UI.Slider>();
            else if (c.name == "Background") bg = c;
            else if (c.name == "Value") value = c;
            else if (c.name == "Fill") oldFill = c;
        }

        if (bg != null)
        {
            bg.gameObject.SetActive(true);
            bg.GetComponent<UnityEngine.UI.Image>().color = new Color(0, 0, 0, 0.8f); // g.setColor(0, 0.8f)
            var r = (RectTransform)bg;
            r.anchorMin = r.anchorMax = new Vector2(0, 1);
            r.sizeDelta = gameRect.size;
            r.anchoredPosition = new Vector2(gameRect.width / 2f, -gameRect.height / 2f);
        }

        if (slider == null)
            slider = Node("Slider", bar).AddComponent<UnityEngine.UI.Slider>();
        // default Slider parts not used by a status bar
        foreach (Transform c in new System.Collections.Generic.List<Transform>(EnumChildren(slider.transform)))
            if (c.name == "Handle Slide Area" || c.name == "Background")
                Object.DestroyImmediate(c.gameObject);
        var srt = (RectTransform)slider.transform;
        srt.anchorMin = srt.anchorMax = new Vector2(0, 1);
        srt.pivot = new Vector2(0.5f, 0.5f);
        srt.sizeDelta = fillRect.size;
        srt.anchoredPosition = new Vector2(fillRect.x - gameRect.x + fillRect.width / 2f, -(fillRect.y - gameRect.y + fillRect.height / 2f));
        srt.SetSiblingIndex(bg != null ? bg.GetSiblingIndex() + 1 : 0);

        Transform areaT = slider.transform.Find("Fill Area");
        var area = areaT != null ? (RectTransform)areaT : (RectTransform)Node("Fill Area", slider.transform).transform;
        area.anchorMin = Vector2.zero;
        area.anchorMax = Vector2.one;
        area.pivot = new Vector2(0.5f, 0.5f);
        area.sizeDelta = Vector2.zero;
        area.anchoredPosition = Vector2.zero;
        Transform fillT = area.Find("Fill");
        var fill = fillT != null ? (RectTransform)fillT : (RectTransform)Node("Fill", area).transform;
        var fillImg = fill.GetComponent<UnityEngine.UI.Image>() ?? fill.gameObject.AddComponent<UnityEngine.UI.Image>();
        fillImg.sprite = null; // a plain rectangle like g.fillRect, not the rounded default sprite
        fillImg.type = UnityEngine.UI.Image.Type.Simple;
        fillImg.color = fillColor;
        fillImg.raycastTarget = false;
        fill.sizeDelta = Vector2.zero;

        slider.fillRect = fill;
        slider.handleRect = null;
        slider.targetGraphic = null;
        slider.direction = UnityEngine.UI.Slider.Direction.LeftToRight;
        slider.minValue = 0;
        slider.maxValue = 1;
        slider.wholeNumbers = false;
        slider.value = 1;
        slider.interactable = false;
        slider.transition = UnityEngine.UI.Selectable.Transition.None;
        var nav = slider.navigation;
        nav.mode = UnityEngine.UI.Navigation.Mode.None;
        slider.navigation = nav;

        if (oldFill != null)
            Object.DestroyImmediate(oldFill.gameObject);
        if (value != null)
        {
            value.gameObject.SetActive(true);
            value.SetAsLastSibling();
        }
    }

    private static System.Collections.Generic.IEnumerable<Transform> EnumChildren(Transform t)
    {
        foreach (Transform c in t)
            yield return c;
    }

    // a container with the first child's rect; the children keep their on-screen place
    private static void Wrap(RectTransform parent, string name, RectTransform[] children, string[] names)
    {
        RectTransform first = children[0];
        var go = Node(name, parent);
        var rt = (RectTransform)go.transform;
        rt.anchorMin = first.anchorMin;
        rt.anchorMax = first.anchorMax;
        rt.pivot = first.pivot;
        rt.sizeDelta = first.sizeDelta;
        rt.anchoredPosition = first.anchoredPosition;
        rt.SetAsLastSibling();
        for (int i = 0; i < children.Length; i++)
        {
            if (children[i] == null)
                continue;
            children[i].SetParent(rt, true);
            if (names != null)
                children[i].name = names[i];
        }
    }

    // paintTouchControl draws imgButton2 over the potion button while it is pressed (keyTouch 10 / 11): Sprite Swap
    public static void SetupPotionButton(GameObject go)
    {
        var img = go.GetComponent<UnityEngine.UI.Image>();
        img.raycastTarget = true;
        var b = go.GetComponent<UnityEngine.UI.Button>() ?? go.AddComponent<UnityEngine.UI.Button>();
        b.targetGraphic = img;
        b.transition = UnityEngine.UI.Selectable.Transition.SpriteSwap;
        var s = b.spriteState;
        s.pressedSprite = HdSprite("button2");
        s.highlightedSprite = null;
        s.selectedSprite = null;
        s.disabledSprite = null;
        b.spriteState = s;
        var nav = b.navigation;
        nav.mode = UnityEngine.UI.Navigation.Mode.None; // no lingering Selected state after a click
        b.navigation = nav;
    }

    // a text whose drawn line is vertically centred on y; align 0 left, 1 right, 2 centre at x
    private static GameObject HudText(RectTransform prt, TMP_FontAsset[] fonts, string name, string text, int font, Color color, float x, float y, int align)
    {
        const float bw = 100;
        float size = FontPx[font] * 0.875f;
        float bh = size * 1.6f;
        var go = Node(name, prt);
        float left = align == 0 ? x : align == 1 ? x - bw : x - bw / 2;
        Place(go, left, y - bh / 2f, bw, bh);
        var t = go.AddComponent<TextMeshProUGUI>();
        t.font = fonts[font];
        t.fontSize = size;
        t.text = text;
        t.color = new Color(color.r, color.g, color.b, 1);
        t.raycastTarget = false;
        t.textWrappingMode = TextWrappingModes.NoWrap;
        t.overflowMode = TextOverflowModes.Overflow;
        t.margin = Vector4.zero;
        t.alignment = align == 0 ? TextAlignmentOptions.MidlineLeft : align == 1 ? TextAlignmentOptions.MidlineRight : TextAlignmentOptions.Midline;
        return go;
    }

    private static GameObject HudImage(RectTransform prt, string name, string file, float x, float y, float w, float h)
    {
        var go = Node(name, prt);
        Place(go, x, y, w, h);
        var img = go.AddComponent<UnityEngine.UI.Image>();
        img.sprite = HdSprite(file);
        img.raycastTarget = false;
        return go;
    }

    // res/x3/hd/<name>.png (the x3 art the capture was drawn with), copied next to the captured sprites as a Sprite
    private static UnityEngine.Sprite HdSprite(string name)
    {
        string src = "Assets/Sprites/Resources/res/x3/hd/" + name + ".png";
        string dst = Root + "/Sprites/hd_" + name + ".png";
        if (!File.Exists(dst))
        {
            if (!File.Exists(src))
                return null;
            File.Copy(src, dst);
            AssetDatabase.ImportAsset(dst);
            var imp = (TextureImporter)AssetImporter.GetAtPath(dst);
            imp.textureType = TextureImporterType.Sprite;
            imp.spriteImportMode = SpriteImportMode.Single;
            imp.spritePixelsPerUnit = 100;
            imp.mipmapEnabled = false;
            imp.alphaIsTransparency = true;
            imp.filterMode = FilterMode.Point; // pixel art: no blending between texels
            imp.textureCompression = TextureImporterCompression.Uncompressed;
            imp.SaveAndReimport();
        }

        return AssetDatabase.LoadAssetAtPath<UnityEngine.Sprite>(dst);
    }

    // The part of a tile that stays visible when the next piece overlaps it: the first `fraction` of it on screen
    // (for a mirrored piece that is the other end of the source image). Saved next to the sprite and imported.
    private static UnityEngine.Sprite Strip(string file, UnityEngine.Sprite source, bool horizontal, float fraction, bool mirrored)
    {
        string src = Root + "/Sprites/" + file;
        var tex = new Texture2D(2, 2);
        tex.LoadImage(File.ReadAllBytes(src));
        int w = tex.width, h = tex.height;
        int n = Mathf.Clamp(Mathf.RoundToInt(fraction * (horizontal ? w : h)), 1, horizontal ? w : h);
        int x = horizontal && mirrored ? w - n : 0;
        int y = !horizontal && !mirrored ? h - n : 0; // texture rows start at the bottom; screen top = top rows
        int sw = horizontal ? n : w, sh = horizontal ? h : n;
        var cut = new Texture2D(sw, sh, TextureFormat.RGBA32, false);
        cut.SetPixels(tex.GetPixels(x, y, sw, sh));
        cut.Apply();
        string name = Path.GetFileNameWithoutExtension(file) + "_" + (horizontal ? "h" : "v") + n + (mirrored ? "m" : "") + ".png";
        string path = Root + "/Sprites/" + name;
        if (!File.Exists(path))
        {
            File.WriteAllBytes(path, cut.EncodeToPNG());
            AssetDatabase.ImportAsset(path);
            var imp = (TextureImporter)AssetImporter.GetAtPath(path);
            imp.textureType = TextureImporterType.Sprite;
            imp.spriteImportMode = SpriteImportMode.Single;
            imp.spritePixelsPerUnit = 100;
            imp.mipmapEnabled = false;
            imp.alphaIsTransparency = true;
            imp.filterMode = FilterMode.Point; // pixel art: no blending between texels
            imp.textureCompression = TextureImporterCompression.Uncompressed;
            imp.wrapMode = TextureWrapMode.Repeat;
            imp.SaveAndReimport();
        }

        Object.DestroyImmediate(tex);
        Object.DestroyImmediate(cut);
        return AssetDatabase.LoadAssetAtPath<UnityEngine.Sprite>(path) ?? source;
    }

    // a text op followed (within a few draws) by the same text 1 px away is its shadow
    private static bool IsShadowOf(DrawRecorder.Op op, System.Collections.Generic.List<DrawRecorder.Op> ops, int k)
    {
        for (int j = k + 1; j < ops.Count && j <= k + 3; j++)
        {
            DrawRecorder.Op n = ops[j];
            if (n.kind == "text" && n.text == op.text && Mathf.Abs(n.x - op.x) <= 1.01f && Mathf.Abs(n.y - op.y) <= 1.01f)
                return true;
        }

        return false;
    }

    private static bool SameTile(DrawRecorder.Op a, DrawRecorder.Op b)
    {
        return b.kind == "image" && b.sprite == a.sprite && b.rotation == 0 && a.rotation == 0
            && Mathf.Abs(b.w - a.w) < 0.01f && Mathf.Abs(b.h - a.h) < 0.01f && b.color == a.color;
    }

    private static GameObject Node(string name, Transform parent)
    {
        var go = new GameObject(name, typeof(RectTransform));
        go.layer = 5;
        go.transform.SetParent(parent, false);
        return go;
    }

    // game rect (top-left x, y, size w x h; negative size = mirrored) inside the panel
    private static void Place(GameObject go, float x, float y, float w, float h)
    {
        var rt = (RectTransform)go.transform;
        float aw = Mathf.Abs(w), ah = Mathf.Abs(h);
        float left = w < 0 ? x + w : x, top = h < 0 ? y + h : y;
        rt.anchorMin = rt.anchorMax = new Vector2(0, 1);
        rt.pivot = new Vector2(0.5f, 0.5f);
        rt.sizeDelta = new Vector2(aw, ah);
        rt.anchoredPosition = new Vector2(left + aw / 2f, -(top + ah / 2f));
        rt.localScale = new Vector3(w < 0 ? -1 : 1, h < 0 ? -1 : 1, 1);
    }

    private static string Short(string s)
    {
        s = s.Replace('\n', ' ');
        return s.Length > 24 ? s.Substring(0, 24) + "…" : s;
    }
}

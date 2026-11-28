using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

// Opens one panel of each type in the running game, records one frame of its drawing with DrawRecorder and saves
// it to Assets/UI/Panels/Captures/<name>.json, with every image region cropped to Assets/UI/Panels/Sprites/.
// The editor builder (Tools > Ninja School > Build Panel Scenes) turns those captures into Canvas scenes.
// Start it in Play mode, once the world has loaded: new GameObject("Panel Capture").AddComponent<PanelCapture>();
public class PanelCapture : MonoBehaviour
{
    [Serializable]
    public class Capture
    {
        public string name;
        public string section;
        public int gameW, gameH;
        public List<DrawRecorder.Op> ops = new List<DrawRecorder.Op>();
    }

    public static string Status = "";
    public float waitSeconds = 2.5f;
    public bool skipExisting = true; // capture only panels that have no capture file yet

    private const string Root = "Assets/UI/Panels";
    private readonly Dictionary<Texture, Texture2D> readable = new Dictionary<Texture, Texture2D>();
    private readonly HashSet<string> written = new HashSet<string>();
    private readonly Dictionary<Texture2D, Color32[]> pixels = new Dictionary<Texture2D, Color32[]>();

    private struct Step
    {
        public string name, section;
        public Action open;
        public Step(string name, string section, Action open) { this.name = name; this.section = section; this.open = open; }
    }

    private static void Call(object target, string method, params object[] args)
    {
        MethodInfo m = typeof(GameScr).GetMethod(method, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
        m.Invoke(target, args);
    }

    private static void InfoMe(int tab)
    {
        GameScr.gI().showViewInfo();
        GameScr.indexMenu = tab;
        GameScr.indexTitle = 0;
    }

    private static List<Step> Steps()
    {
        GameScr gs = GameScr.gI();
        List<Step> s = new List<Step>();
        s.Add(new Step("HUD", "hud", () => { }));
        string[] tabs = { "Hanh Trang", "Ky Nang", "Tiem Nang", "Thong Tin", "Trang Bi", "Thu Cuoi", "Vi Thu" };
        for (int i = 0; i < tabs.Length; i++)
        {
            int tab = i;
            s.Add(new Step(tabs[i], "panel", () => InfoMe(tab)));
        }

        // doOpenUI type codes: one per panel type (all the clothing / jewellery / potion shops share "Item Shop")
        int[] ui = { 20, 14, 35, 38, 10, 33, 13, 11, 46, 47, 48, 49, 36, 37, 40 };
        string[] uiNames = { "Item Shop", "Store", "Elite Shop", "Lucky Spin", "Upgrade", "Convert", "Split", "Up Pearl", "Luyen Ngoc", "Kham Ngoc", "Giao Do", "Got Ngoc", "Auction Sale", "Auction Buy", "Auto" };
        for (int i = 0; i < ui.Length; i++)
        {
            int code = ui[i];
            s.Add(new Step(uiNames[i], "panel", () => gs.doOpenUI(code)));
        }

        s.Add(new Step("Team", "panel", () => gs.doShowTeamUI()));
        s.Add(new Step("Find Team", "panel", () => gs.doShowFindTeamUI()));
        s.Add(new Step("Friends", "panel", () => gs.doShowFriendUI()));
        s.Add(new Step("List", "panel", () => gs.doShowListUI()));
        s.Add(new Step("Ranked List", "panel", () => gs.doShowRankedListUI()));
        s.Add(new Step("Clan", "panel", () => Call(gs, "doShowClan")));
        s.Add(new Step("Task", "panel", () => gs.doShowTaskUI()));
        s.Add(new Step("Chars In Map", "panel", () => gs.doShowCharInMap()));
        s.Add(new Step("Trade", "panel", () => gs.openUITrade()));
        s.Add(new Step("Zone", "panel", () => Service.gI().openUIZone()));
        s.Add(new Step("Alert", "panel", () => gs.showAlert("Thông báo", "Nội dung thông báo dòng 1\nNội dung thông báo dòng 2", false)));
        s.Add(new Step("Item Info", "panel", () =>
        {
            InfoMe(0);
            Item[] bag = Char.getMyChar().arrItemBag;
            Item first = null;
            if (bag != null)
                foreach (Item it in bag)
                    if (it != null) { first = it; break; }
            gs.updateItemInfo(3, first);
        }));
        return s;
    }

    private void Start()
    {
        StartCoroutine(Run());
    }

    private IEnumerator Run()
    {
        Directory.CreateDirectory(Root + "/Captures");
        Directory.CreateDirectory(Root + "/Sprites");
        List<Step> steps = Steps();
        for (int i = 0; i < steps.Count; i++)
        {
            Step step = steps[i];
            if (skipExisting && File.Exists(Root + "/Captures/" + step.name + ".json"))
                continue;
            Status = (i + 1) + "/" + steps.Count + " " + step.name;
            string error = null;
            try
            {
                GameScr.gI().resetButton();
                InfoDlg.hide();
                GameCanvas.currentDialog = null;
                GameCanvas.menu.showMenu = false;
                step.open();
            }
            catch (Exception e)
            {
                error = e.GetBaseException().Message;
            }

            yield return new WaitForSecondsRealtime(waitSeconds);
            GameCanvas.currentDialog = null; // a server reply may have opened a message box over the panel
            yield return null;
            DrawRecorder.Begin();
            yield return new WaitForEndOfFrame();
            DrawRecorder.End();
            try
            {
                Save(step.name, step.section);
            }
            catch (Exception e)
            {
                error = (error == null ? "" : error + "; ") + e.Message;
            }

            if (error != null)
                Debug.LogWarning("[PanelCapture] " + step.name + ": " + error);
        }

        try { GameScr.gI().resetButton(); } catch (Exception) { }
        foreach (Texture2D t in readable.Values)
            if (t != null)
                Destroy(t);
        Status = "done " + steps.Count;
        Debug.Log("[PanelCapture] done: " + steps.Count + " panels in " + Root);
        Destroy(gameObject);
    }

    private void Save(string name, string section)
    {
        Capture cap = new Capture { name = name, section = section, gameW = GameCanvas.w, gameH = GameCanvas.h };
        foreach (DrawRecorder.Op op in DrawRecorder.Ops)
        {
            if (op.section != section)
                continue;
            if (op.kind == "image")
            {
                op.sprite = Crop(op.texture, op.uv);
                if (op.sprite == null)
                    continue;
            }

            cap.ops.Add(op);
        }

        File.WriteAllText(Root + "/Captures/" + name + ".json", JsonUtility.ToJson(cap, true));
    }

    // exports the region of the texture as a PNG named by its content hash; returns the file name
    private string Crop(Texture tex, Rect uv)
    {
        Texture2D src = Readable(tex);
        if (src == null)
            return null;
        int x = Mathf.RoundToInt(uv.x * src.width), y = Mathf.RoundToInt(uv.y * src.height);
        int w = Mathf.RoundToInt(uv.width * src.width), h = Mathf.RoundToInt(uv.height * src.height);
        x = Mathf.Clamp(x, 0, src.width - 1);
        y = Mathf.Clamp(y, 0, src.height - 1);
        w = Mathf.Clamp(w, 1, src.width - x);
        h = Mathf.Clamp(h, 1, src.height - y);
        Color32[] px;
        if (!pixels.TryGetValue(src, out px))
            pixels[src] = px = src.GetPixels32();
        Color32[] cut = new Color32[w * h];
        for (int row = 0; row < h; row++)
            Array.Copy(px, (y + row) * src.width + x, cut, row * w, w);
        Hash128 hash = new Hash128();
        hash.Append(w);
        hash.Append(h);
        hash.Append(cut);
        string file = hash.ToString() + ".png";
        if (written.Add(file) && !File.Exists(Root + "/Sprites/" + file))
        {
            Texture2D outTex = new Texture2D(w, h, TextureFormat.RGBA32, false);
            outTex.SetPixels32(cut);
            outTex.Apply();
            File.WriteAllBytes(Root + "/Sprites/" + file, outTex.EncodeToPNG());
            Destroy(outTex);
        }

        return file;
    }

    // a CPU copy of any texture (GPU-only ones are read back through a render texture), cached per capture run
    private Texture2D Readable(Texture tex)
    {
        Texture2D copy;
        if (readable.TryGetValue(tex, out copy))
            return copy;
        RenderTexture rt = RenderTexture.GetTemporary(tex.width, tex.height, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.sRGB);
        Graphics.Blit(tex, rt);
        RenderTexture prev = RenderTexture.active;
        RenderTexture.active = rt;
        copy = new Texture2D(tex.width, tex.height, TextureFormat.RGBA32, false);
        copy.ReadPixels(new Rect(0, 0, tex.width, tex.height), 0, 0);
        copy.Apply();
        RenderTexture.active = prev;
        RenderTexture.ReleaseTemporary(rt);
        readable[tex] = copy;
        return copy;
    }
}

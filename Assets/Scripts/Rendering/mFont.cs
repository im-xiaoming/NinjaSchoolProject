using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mFont
{
    public static int LEFT;
    public static int RIGHT = 1;
    public static int CENTER = 2;
    public static int RED;
    public static int YELLOW = 1;
    public static int GREEN = 2;
    public static int FATAL = 3;
    public static int MISS = 4;
    public static int ORANGE = 5;
    public static int ADDMONEY = 6;
    public static int MISS_ME = 7;
    public static int FATAL_ME = 8;
    private int space;
    private Image imgFont;
    private string strFont;
    private int[][] fImages;
    private BitmapFontAtlas bitmapAtlas;
    private static readonly Dictionary<string, BitmapFontAtlas> bitmapAtlases = new Dictionary<string, BitmapFontAtlas>(); // key: "font@size"
    // pixel size, ascent and line height of NsoNormalFont0..5 per zoom level (x1..x4): the sizes of the bitmap fonts they replace
    private static readonly int[,] TtfSize = { { 5, 9, 11, 12, 13, 13 }, { 14, 18, 20, 21, 22, 24 }, { 24, 30, 32, 30, 25, 35 }, { 28, 30, 32, 32, 35, 40 } };
    private static readonly int[,] TtfAscent = { { 5, 9, 11, 12, 13, 12 }, { 14, 18, 20, 21, 22, 23 }, { 24, 31, 33, 31, 25, 34 }, { 29, 31, 33, 33, 36, 39 } };
    private static readonly int[,] TtfLineHeight = { { 6, 10, 13, 14, 15, 16 }, { 17, 21, 24, 25, 26, 29 }, { 29, 36, 39, 36, 30, 43 }, { 34, 36, 39, 39, 42, 49 } };
    // gI: G i.
    public static mFont gI;
    public static mFont tahoma_7b_red;
    public static mFont tahoma_7b_blue;
    public static mFont tahoma_7b_purple;
    public static mFont tahoma_7b_yellow;
    public static mFont tahoma_7b_green;
    public static mFont tahoma_7b_white;
    public static mFont tahoma_7;
    public static mFont tahoma_7_blue1;
    public static mFont tahoma_7_white;
    public static mFont tahoma_7_yellow;
    public static mFont tahoma_7_grey;
    public static mFont tahoma_7_red;
    public static mFont tahoma_7_blue;
    public static mFont tahoma_7_green;
    public static mFont tahoma_8b;
    public static mFont number_yellow;
    public static mFont number_red;
    public static mFont number_green;
    public static mFont number_white;
    public static mFont number_orange;
    public static mFont numberb_yellow;
    public static mFont numberb_red;
    public static mFont numberb_green;
    public static mFont numberb_white;
    public static mFont numberb_orange;
    public static mFont tahoma_7_white_b;
    public static mFont number_fly_txt;
    public static mFont bigNumber_red;
    public static mFont bigNumber_While;
    public static mFont bigNumber_yellow;
    public static mFont bigNumber_green;
    public static mFont bigNumber_orange;
    public static mFont bigNumber_blue;
    public Font myFont;
    private int fontPx = 20;
    private FontStyle fontStyle = FontStyle.Normal;
    private int height;
    // wO: W o.
    private int wO;
    public Color color1 = Color.white;
    public Color color2 = Color.gray;
    // id: Identifier.
    public sbyte id;
    public int fstyle;
    public string st1 = "áàảãạăắằẳẵặâấầẩẫậéèẻẽẹêếềểễệíìỉĩịóòỏõọôốồổỗộơớờởỡợúùủũụưứừửữựýỳỷỹỵđÁÀẢÃẠĂẮẰẲẴẶÂẤẦẨẪẬÉÈẺẼẸÊẾỀỂỄỆÍÌỈĨỊÓÒỎÕỌÔỐỒỔỖỘƠỚỜỞỠỢÚÙỦŨỤƯỨỪỬỮỰÝỲỶỸỴĐ";
    public string st2 = "\u00b8µ¶·¹\u00a8¾»¼½Æ©ÊÇÈÉËÐÌÎÏÑªÕÒÓÔÖÝ×ØÜÞãßáâä«èåæçé¬íêëìîóïñòô\u00adøõö÷ùýúûüþ®\u00b8µ¶·¹¡¾»¼½Æ¢ÊÇÈÉËÐÌÎÏÑ£ÕÒÓÔÖÝ×ØÜÞãßáâä¤èåæçé¥íêëìîóïñòô¦øõö÷ùýúûüþ§";
    /// <summary>Font (NsoNormalFont0..5) an mFont id draws with.</summary>
    /// <param name="id">mFont identifier.</param>
    private static int FontIndex(int id)
    {
        int index = 2;
        if (id > 0 && id < 8)
        {
            index = 3;
        }
        else if (id >= 17 && id < 22)
        {
            index = 0;
        }
        else if (id >= 8 && id <= 15)
        {
            index = 1;
        }
        else if (id >= 22)
        {
            index = 4;
        }

        if (id >= 28)
        {
            index = 5;
        }

        if (id == 16)
        {
            index = 3;
        }

        return index;
    }

    /// <summary>Initialize m font state and its supplied dependencies.</summary>
    /// <param name="id">Identifier.</param>
    public mFont(sbyte id)
    {
        this.id = id;
        // the game's fonts are the TTFs in Assets/Fonts/Resources/TTF, drawn at the pixel size of the zoom level's old bitmap font
        int fontIndex = FontIndex(id);
        int zoom = Mathf.Clamp(mGraphics.zoomLevel, 1, 4) - 1;
        Font asset = Resources.Load<Font>("TTF/NsoNormalFont" + fontIndex);
        if (asset != null)
        {
            fontPx = TtfSize[zoom, fontIndex];
            string key = fontIndex + "@" + fontPx;
            if (!bitmapAtlases.TryGetValue(key, out bitmapAtlas))
            {
                bitmapAtlas = BitmapFontAtlas.Create(asset, fontPx, TtfAscent[zoom, fontIndex], TtfLineHeight[zoom, fontIndex], OriginalAdvances(zoom + 1, fontIndex));
                if (bitmapAtlas != null)
                {
                    bitmapAtlases[key] = bitmapAtlas;
                }
            }

            myFont = asset;
            height = TtfLineHeight[zoom, fontIndex];
        }
        else
        {
            fontPx = 10 * mGraphics.zoomLevel;
        }

        if (bitmapAtlas == null)
        {
            fontStyle = FontStyle.Bold;
            myFont = Font.CreateDynamicFontFromOSFont(new string[] { "Tahoma", "Arial", "Segoe UI" }, fontPx);
        }

        color1 = setColorFont(id);
        color2 = setColorFont(id);
        wO = bitmapAtlas != null ? bitmapAtlas.GetAdvance('o') : getWidthExactOf("o") * mGraphics.zoomLevel;
    }

    private static Dictionary<string, Dictionary<int, int>> originalAdvances;

    // letter advances of the game's original bitmap fonts at one zoom level (Assets/Fonts/Resources/TTF/NsoFontAdvances.txt):
    // the TTFs round some advances differently at small sizes, these keep every text line its original width
    private static Dictionary<int, int> OriginalAdvances(int zoom, int fontIndex)
    {
        if (originalAdvances == null)
        {
            originalAdvances = new Dictionary<string, Dictionary<int, int>>();
            TextAsset file = Resources.Load<TextAsset>("TTF/NsoFontAdvances");
            if (file != null)
            {
                string[] lines = file.text.Split('\n');
                for (int i = 0; i + 1 < lines.Length; i++)
                {
                    string header = lines[i].Trim();
                    if (header.Length == 0 || header[0] != 'x')
                    {
                        continue;
                    }

                    Dictionary<int, int> map = new Dictionary<int, int>();
                    foreach (string pair in lines[i + 1].Trim().Split(' '))
                    {
                        int colon = pair.IndexOf(':');
                        int code, advance;
                        if (colon > 0 && int.TryParse(pair.Substring(0, colon), out code) && int.TryParse(pair.Substring(colon + 1), out advance))
                        {
                            map[code] = advance;
                        }
                    }

                    originalAdvances[header] = map;
                    i++;
                }
            }
        }

        Dictionary<int, int> result;
        return originalAdvances.TryGetValue("x" + zoom + " " + fontIndex, out result) ? result : null;
    }

    private struct BitmapGlyph
    {
        // x: Horizontal coordinate in the caller's coordinate space.
        public int x;
        // y: Vertical coordinate in the caller's coordinate space.
        public int y;
        public int width;
        public int height;
        public int advance;
        public int offX; // glyph bearing: left edge relative to the pen
        public int offY; // top edge relative to the line top, measured from the tallest capital so letters share a baseline
        public bool rotated; // glyph is stored turned 90 degrees in the atlas; width/height are the atlas region size
    }

    private sealed class BitmapFontAtlas
    {
        public readonly Image image;
        public readonly Material material;
        public readonly Dictionary<int, BitmapGlyph> glyphs;
        public readonly int lineHeight;
        /// <summary>Initialize bitmap font atlas state and its supplied dependencies.</summary>
        /// <param name="image">Image.</param>
        /// <param name="material">Material.</param>
        /// <param name="glyphs">Glyphs.</param>
        /// <param name="lineHeight">Line height.</param>
        private BitmapFontAtlas(Image image, Material material, Dictionary<int, BitmapGlyph> glyphs, int lineHeight)
        {
            this.image = image;
            this.material = material;
            this.glyphs = glyphs;
            this.lineHeight = lineHeight;
        }

        /// <summary>Create the glyph atlas of a TTF font at one pixel size.</summary>
        /// <param name="font">TTF font (dynamic).</param>
        /// <param name="size">Pixel size.</param>
        /// <param name="ascent">Baseline distance below the top of the line box.</param>
        /// <param name="lineHeight">Line height.</param>
        /// <param name="advances">Letter advances to use instead of the TTF's (char code to pixels), or null.</param>
        public static BitmapFontAtlas Create(Font font, int size, int ascent, int lineHeight, Dictionary<int, int> advances)
        {
            if (font == null || font.material == null || size <= 0)
            {
                return null;
            }

            string chars = Charset;
            font.RequestCharactersInTexture(chars, size, FontStyle.Normal);
            Texture source = font.material.mainTexture;
            if (source == null)
            {
                return null;
            }

            CharacterInfo[] characters = new CharacterInfo[chars.Length];
            int count = 0;
            for (int i = 0; i < chars.Length; i++)
            {
                CharacterInfo info;
                if (font.GetCharacterInfo(chars[i], out info, size, FontStyle.Normal))
                {
                    characters[count++] = info;
                }
            }

            System.Array.Resize(ref characters, count);
            // the font texture is shared by every size and rebuilt whenever other text asks for new glyphs, which moves them:
            // keep a copy, so these glyph rectangles stay valid
            RenderTexture rt = RenderTexture.GetTemporary(source.width, source.height, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear);
            Graphics.Blit(source, rt);
            RenderTexture previous = RenderTexture.active;
            RenderTexture.active = rt;
            Texture2D texture = new Texture2D(source.width, source.height, TextureFormat.RGBA32, false);
            texture.ReadPixels(new Rect(0, 0, source.width, source.height), 0, 0);
            texture.Apply();
            texture.filterMode = FilterMode.Bilinear;
            RenderTexture.active = previous;
            RenderTexture.ReleaseTemporary(rt);
            Material material = new Material(font.material);
            material.mainTexture = texture;
            Image image = new Image();
            image.texture = texture;
            image.w = texture.width;
            image.h = texture.height;
            int referenceMaxY = ascent; // Unity's own text puts the baseline one font ascent below the top of the line box
            Dictionary<int, BitmapGlyph> glyphs = new Dictionary<int, BitmapGlyph>(characters.Length);
            for (int i = 0; i < characters.Length; i++)
            {
                CharacterInfo character = characters[i];
                Rect uv = character.uv;
                BitmapGlyph glyph = new BitmapGlyph
                {
                    x = Mathf.RoundToInt(uv.x * texture.width),
                    y = texture.height - Mathf.RoundToInt(uv.y * texture.height),
                    width = Mathf.RoundToInt(Mathf.Abs(uv.width) * texture.width),
                    height = Mathf.RoundToInt(Mathf.Abs(uv.height) * texture.height),
                    advance = advances != null && advances.ContainsKey(character.index) ? advances[character.index] : character.advance,
                    offX = character.minX,
                    offY = referenceMaxY - character.maxY
                };
                // Unity can pack a glyph turned 90 degrees: the glyph's top edge then runs along +u instead of -v.
                Vector2 bl = character.uvBottomLeft, tl = character.uvTopLeft, br = character.uvBottomRight;
                if (Mathf.Abs(tl.x - bl.x) > Mathf.Abs(tl.y - bl.y) && tl.x > bl.x && br.y < bl.y)
                {
                    float minU = Mathf.Min(bl.x, tl.x, br.x), maxU = Mathf.Max(bl.x, tl.x, br.x);
                    float minV = Mathf.Min(bl.y, tl.y, br.y), maxV = Mathf.Max(bl.y, tl.y, br.y);
                    glyph.rotated = true;
                    glyph.x = Mathf.RoundToInt(minU * texture.width);
                    glyph.y = texture.height - Mathf.RoundToInt(maxV * texture.height);
                    glyph.width = Mathf.RoundToInt((maxU - minU) * texture.width);
                    glyph.height = Mathf.RoundToInt((maxV - minV) * texture.height);
                }

                if (glyph.width > 0 && glyph.height > 0 && glyph.advance >= 0)
                {
                    glyphs[character.index] = glyph;
                }
            }

            return glyphs.Count == 0 ? null : new BitmapFontAtlas(image, material, glyphs, Mathf.Max(1, lineHeight));
        }

        // the 247 characters of the game's fonts (ASCII, the Vietnamese letters and a few symbols); the TTFs hold exactly these
        private const string Charset = " !\"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~\u00A8\u00A9\u00AE\u00B4\u00B6\u00BC\u00BD\u00BE\u00C0\u00C1\u00C2\u00C3\u00C8\u00C9\u00CA\u00CC\u00CD\u00D0\u00D2\u00D3\u00D4\u00D5\u00D9\u00DA\u00DD\u00E0\u00E1\u00E2\u00E3\u00E8\u00E9\u00EA\u00EC\u00ED\u00F2\u00F3\u00F4\u00F5\u00F7\u00F9\u00FA\u00FD\u0102\u0103\u010C\u010D\u0110\u0111\u0128\u0129\u0168\u0169\u01A0\u01A1\u01AF\u01B0\u1EA0\u1EA1\u1EA2\u1EA3\u1EA4\u1EA5\u1EA6\u1EA7\u1EA8\u1EA9\u1EAA\u1EAB\u1EAC\u1EAD\u1EAE\u1EAF\u1EB0\u1EB1\u1EB2\u1EB3\u1EB4\u1EB5\u1EB6\u1EB7\u1EB8\u1EB9\u1EBA\u1EBB\u1EBC\u1EBD\u1EBE\u1EBF\u1EC0\u1EC1\u1EC2\u1EC3\u1EC4\u1EC5\u1EC6\u1EC7\u1EC8\u1EC9\u1ECA\u1ECB\u1ECC\u1ECD\u1ECE\u1ECF\u1ED0\u1ED1\u1ED2\u1ED3\u1ED4\u1ED5\u1ED6\u1ED7\u1ED8\u1ED9\u1EDA\u1EDB\u1EDC\u1EDD\u1EDE\u1EDF\u1EE0\u1EE1\u1EE2\u1EE3\u1EE4\u1EE5\u1EE6\u1EE7\u1EE8\u1EE9\u1EEA\u1EEB\u1EEC\u1EED\u1EEE\u1EEF\u1EF0\u1EF1\u1EF2\u1EF3\u1EF4\u1EF5\u1EF6\u1EF7\u1EF8\u1EF9\u2010\u2018\u2019\u201C\u201D\u2026";

        /// <summary>Attempt get.</summary>
        /// <param name="value">Client character being inspected or controlled.</param>
        /// <param name="glyph">Glyph.</param>
        public bool TryGet(char value, out BitmapGlyph glyph)
        {
            if (glyphs.TryGetValue(value, out glyph))
            {
                return true;
            }

            if (glyphs.TryGetValue('?', out glyph))
            {
                return true;
            }

            return glyphs.TryGetValue(' ', out glyph);
        }

        /// <summary>Return advance.</summary>
        /// <param name="value">Client character being inspected or controlled.</param>
        public int GetAdvance(char value)
        {
            BitmapGlyph glyph;
            return TryGet(value, out glyph) ? glyph.advance : 0;
        }
    }

    /// <summary>Initialize m font.</summary>
    public static void init()
    {
        gI = new mFont(0);
        tahoma_7b_red = new mFont(1);
        tahoma_7b_blue = new mFont(2);
        tahoma_7b_purple = new mFont(3);
        tahoma_7b_yellow = new mFont(4);
        tahoma_7b_green = new mFont(5);
        tahoma_7b_white = new mFont(7);
        tahoma_7 = new mFont(8);
        tahoma_7_blue1 = new mFont(9);
        tahoma_7_white = new mFont(10);
        tahoma_7_yellow = new mFont(11);
        tahoma_7_grey = new mFont(12);
        tahoma_7_red = new mFont(13);
        tahoma_7_blue = new mFont(14);
        tahoma_7_green = new mFont(15);
        tahoma_8b = new mFont(16);
        number_yellow = new mFont(17);
        number_red = new mFont(18);
        number_green = new mFont(19);
        number_white = new mFont(20);
        number_orange = new mFont(21);
        numberb_yellow = new mFont(22);
        numberb_red = new mFont(23);
        numberb_green = new mFont(24);
        numberb_white = new mFont(25);
        numberb_orange = new mFont(26);
        tahoma_7_white_b = new mFont(27);
        bigNumber_red = new mFont(28);
        bigNumber_While = new mFont(29);
        bigNumber_yellow = new mFont(30);
        bigNumber_green = new mFont(31);
        bigNumber_orange = new mFont(32);
        bigNumber_blue = new mFont(33);
    }

    /// <summary>Set color.</summary>
    /// <param name="rgb">Red/green/blue channels.</param>
    /// <remarks>Locals: num: Numeric intermediate used by this operation; the legacy name is retained; num2: Numeric intermediate used by this operation; the legacy name is retained; num3: Numeric intermediate used by this operation; the legacy name is retained; b: Buffer or intermediate output value; g: Drawing context; r: Result being assembled or returned.</remarks>
    public Color setColor(int rgb)
    {
        int num = rgb & 0xFF;
        int num2 = (rgb >> 8) & 0xFF;
        int num3 = (rgb >> 16) & 0xFF;
        float b = (float)num / 256f;
        float g = (float)num2 / 256f;
        float r = (float)num3 / 256f;
        return new Color(r, g, b);
    }

    /// <summary>Set type paint.</summary>
    /// <param name="g">Legacy drawing context.</param>
    /// <param name="st">St.</param>
    /// <param name="x">Horizontal coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="y">Vertical coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="align">Align.</param>
    /// <param name="idFont">Identifier font.</param>
    /// <remarks>Locals: b: Buffer or intermediate output value.</remarks>
    public void setTypePaint(mGraphics g, string st, int x, int y, int align, sbyte idFont)
    {
        sbyte b = id;
        if (idFont > 0)
        {
            b = idFont;
        }

        // the original also nudged x by -1 to suit its bitmap fonts; the system font is already centred, so only the y nudge stays
        if (mGraphics.zoomLevel == 4)
        {
            y += 2;
        }

        switch (b)
        {
            case 1:
                color1 = Color.red;
                color2 = Color.red;
                _drawString(g, st, x, y, align);
                break;
            case 2:
                color1 = Color.blue;
                color2 = Color.blue;
                _drawString(g, st, x, y, align);
                break;
            case 3:
                color1 = setColor(13500671);
                color2 = setColor(13500671);
                _drawString(g, st, x, y, align);
                break;
            case 4:
                color1 = setColor(16755200);
                color2 = setColor(16755200);
                _drawString(g, st, x, y, align);
                break;
            case 5:
                color1 = Color.green;
                color2 = Color.green;
                _drawString(g, st, x, y, align);
                break;
            case 6:
                color1 = setColor(16776960);
                color2 = setColor(16776960);
                _drawString(g, st, x, y, align);
                break;
            case 7:
            case 27:
                color1 = Color.white;
                color2 = Color.white;
                _drawString(g, st, x, y, align);
                break;
            case 8:
                color1 = Color.black;
                color2 = Color.black;
                _drawString(g, st, x, y, align);
                break;
            case 9:
                color1 = setColor(65535);
                color2 = setColor(65535);
                _drawString(g, st, x, y, align);
                break;
            case 10:
                color1 = Color.white;
                color2 = Color.white;
                _drawString(g, st, x, y, align);
                break;
            case 11:
                color1 = Color.yellow;
                color2 = Color.yellow;
                _drawString(g, st, x, y, align);
                break;
            case 12:
                color1 = setColor(5592405);
                color2 = setColor(5592405);
                _drawString(g, st, x, y, align);
                break;
            case 13:
                color1 = Color.red;
                color2 = Color.red;
                _drawString(g, st, x, y, align);
                break;
            case 14:
                color1 = setColor(33023);
                color2 = setColor(33023);
                _drawString(g, st, x, y, align);
                break;
            case 15:
                color1 = setColor(8701737);
                color2 = setColor(8701737);
                _drawString(g, st, x, y, align);
                break;
            case 16:
                color1 = setColor(15655191);
                color2 = setColor(15655191);
                _drawString(g, st, x, y, align);
                break;
            case 17:
                color1 = setColor(0);
                color2 = setColor(0);
                _drawString(g, st, x - 1, y - 3, align);
                color1 = Color.yellow;
                color2 = Color.yellow;
                _drawString(g, st, x - 2, y - 3, align);
                break;
            case 18:
                color1 = setColor(0);
                color2 = setColor(0);
                _drawString(g, st, x - 1, y - 3, align);
                color1 = Color.red;
                color2 = Color.red;
                _drawString(g, st, x - 2, y - 3, align);
                break;
            case 19:
                color1 = setColor(0);
                color2 = setColor(0);
                _drawString(g, st, x - 1, y - 3, align);
                color1 = Color.green;
                color2 = Color.green;
                _drawString(g, st, x - 2, y - 3, align);
                break;
            case 20:
                color1 = setColor(0);
                color2 = setColor(0);
                _drawString(g, st, x - 1, y - 3, align);
                color1 = Color.white;
                color2 = Color.white;
                _drawString(g, st, x - 2, y - 3, align);
                break;
            case 21:
                color1 = setColor(0);
                color2 = setColor(0);
                _drawString(g, st, x - 1, y - 3, align);
                color1 = setColor(16096312);
                color2 = setColor(16096312);
                _drawString(g, st, x - 2, y - 3, align);
                break;
            case 22:
                color1 = setColor(5592405);
                color2 = setColor(5592405);
                _drawString(g, st, x, y + 1, align);
                color1 = Color.yellow;
                color2 = Color.yellow;
                _drawString(g, st, x, y, align);
                break;
            case 30:
                color1 = Color.black;
                color2 = Color.black;
                _drawString(g, st, x, y + 1, align);
                color1 = Color.yellow;
                color2 = Color.yellow;
                _drawString(g, st, x, y, align);
                break;
            case 23:
            case 28:
                color1 = setColor(5592405);
                color2 = setColor(5592405);
                _drawString(g, st, x, y + 1, align);
                color1 = Color.red;
                color2 = Color.red;
                _drawString(g, st, x, y, align);
                break;
            case 24:
            case 31:
                color1 = setColor(5592405);
                color2 = setColor(5592405);
                _drawString(g, st, x, y + 1, align);
                color1 = Color.green;
                color2 = Color.green;
                _drawString(g, st, x, y, align);
                break;
            case 25:
            case 29:
                color1 = setColor(5592405);
                color1 = setColor(5592405);
                _drawString(g, st, x, y + 1, align);
                color1 = Color.white;
                color2 = Color.white;
                _drawString(g, st, x, y, align);
                break;
            case 26:
            case 32:
                color1 = setColor(5592405);
                color1 = setColor(5592405);
                _drawString(g, st, x, y + 1, align);
                color1 = setColor(16096312);
                color1 = setColor(16096312);
                _drawString(g, st, x, y, align);
                break;
            case 33:
                color1 = setColor(5592405);
                color1 = setColor(5592405);
                _drawString(g, st, x, y + 1, align);
                color1 = Color.blue;
                color1 = Color.blue;
                _drawString(g, st, x, y, align);
                break;
        }
    }

    /// <summary>Set color font.</summary>
    /// <param name="id">Identifier.</param>
    public Color setColorFont(sbyte id)
    {
        switch (id)
        {
            case 1:
                return Color.red;
            case 2:
                return Color.blue;
            case 3:
                return setColor(13500671);
            case 4:
                return setColor(16755200);
            case 5:
                return Color.green;
            case 6:
                return setColor(16776960);
            case 7:
                return Color.white;
            case 8:
                return Color.black;
            case 9:
                return setColor(65535);
            case 10:
                return Color.white;
            case 11:
                return Color.yellow;
            case 12:
                return setColor(5592405);
            case 13:
                return Color.red;
            case 14:
                return setColor(33023);
            case 15:
                return setColor(8701737);
            case 16:
                return setColor(15655191);
            case 17:
                return setColor(0);
            case 18:
                return setColor(0);
            case 19:
                return setColor(0);
            case 20:
                return setColor(0);
            case 21:
                return setColor(0);
            case 22:
            case 30:
                return setColor(5592405);
            case 23:
            case 28:
                return setColor(5592405);
            case 24:
            case 31:
                return setColor(5592405);
            case 25:
            case 29:
                return setColor(5592405);
            case 26:
            case 32:
                return setColor(5592405);
            default:
                return Color.white;
        }
    }

    /// <summary>Draw string in the current graphics context.</summary>
    /// <param name="g">Legacy drawing context.</param>
    /// <param name="st">St.</param>
    /// <param name="x">Horizontal coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="y">Vertical coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="align">Align.</param>
    public void drawString(mGraphics g, string st, int x, int y, int align)
    {
        setTypePaint(g, st, x, y, align, id);
    }

    /// <summary>Draw string in the current graphics context.</summary>
    /// <param name="g">Legacy drawing context.</param>
    /// <param name="st">St.</param>
    /// <param name="x">Horizontal coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="y">Vertical coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="align">Align.</param>
    /// <param name="font">Font.</param>
    public void drawString(mGraphics g, string st, int x, int y, int align, mFont font)
    {
        setTypePaint(g, st, x, y + 1, align, font.id);
        setTypePaint(g, st, x, y, align, 0);
    }

    /// <summary>Split font vector.</summary>
    /// <param name="src">Source.</param>
    /// <param name="lineWidth">Line width.</param>
    /// <remarks>Locals: text: Text fragment assembled or read by this operation; i: Loop/collection index; num: Numeric intermediate used by this operation; the legacy name is retained.</remarks>
    public MyVector splitFontVector(string src, int lineWidth)
    {
        MyVector myVector = new MyVector();
        string text = string.Empty;
        for (int i = 0; i < src.Length; i++)
        {
            if (src[i] == '\n')
            {
                myVector.addElement(text);
                text = string.Empty;
                continue;
            }

            text += src[i];
            if (getWidth(text) > lineWidth)
            {
                int num = 0;
                num = text.Length - 1;
                while (num >= 0 && text[num] != ' ')
                {
                    num--;
                }

                if (num < 0)
                {
                    num = text.Length - 1;
                }

                myVector.addElement(text.Substring(0, num));
                i = i - (text.Length - num) + 1;
                text = string.Empty;
            }

            if (i == src.Length - 1 && !text.Trim().Equals(string.Empty))
            {
                myVector.addElement(text);
            }
        }

        return myVector;
    }

    /// <summary>Split font array.</summary>
    /// <param name="src">Source.</param>
    /// <param name="lineWidth">Line width.</param>
    /// <remarks>Locals: array: Number of entries in the source collection; i: Loop/collection index.</remarks>
    public string[] splitFontArray(string src, int lineWidth)
    {
        MyVector myVector = splitFontVector(src, lineWidth);
        string[] array = new string[myVector.size()];
        for (int i = 0; i < myVector.size(); i++)
        {
            array[i] = (string)myVector.elementAt(i);
        }

        return array;
    }

    /// <summary>Return width.</summary>
    /// <param name="s">Source text or serialized input.</param>
    public int getWidth(string s)
    {
        return getWidthExactOf(s);
    }

    /// <summary>Return width exact of.</summary>
    /// <param name="s">Source text or serialized input.</param>
    public int getWidthExactOf(string s)
    {
        if (bitmapAtlas != null)
        {
            return GetBitmapWidth(s) / Mathf.Max(1, mGraphics.zoomLevel);
        }

        try
        {
            GUIStyle gUIStyle = new GUIStyle();
            gUIStyle.font = myFont;
            gUIStyle.fontSize = fontPx;
            gUIStyle.fontStyle = fontStyle;
            return (int)gUIStyle.CalcSize(new GUIContent(s)).x / mGraphics.zoomLevel;
        }
        catch (Exception ex)
        {
            Out.LogError("GET WIDTH OF " + s + " FAIL.\n" + ex.Message + "\n" + ex.StackTrace);
            return getWidthNotExactOf(s);
        }
    }

    /// <summary>Return width not exact of.</summary>
    /// <param name="s">Source text or serialized input.</param>
    public int getWidthNotExactOf(string s)
    {
        if (bitmapAtlas != null)
        {
            return GetBitmapWidth(s) / Mathf.Max(1, mGraphics.zoomLevel);
        }

        return s.Length * wO / Mathf.Max(1, mGraphics.zoomLevel);
    }

    /// <summary>Return bitmap width.</summary>
    /// <param name="text">Text fragment assembled or read by this operation.</param>
    /// <remarks>Locals: i: Loop/collection index.</remarks>
    private int GetBitmapWidth(string text)
    {
        int maxWidth = 0;
        int lineWidth = 0;
        for (int i = 0; i < text.Length; i++)
        {
            if (text[i] == '\n')
            {
                maxWidth = Mathf.Max(maxWidth, lineWidth);
                lineWidth = 0;
                continue;
            }

            if (text[i] == '\r')
            {
                continue;
            }

            lineWidth += bitmapAtlas.GetAdvance(text[i]);
        }

        return Mathf.Max(maxWidth, lineWidth);
    }

    /// <summary>Return height.</summary>
    public int getHeight()
    {
        if (bitmapAtlas != null)
        {
            return bitmapAtlas.lineHeight / Mathf.Max(1, mGraphics.zoomLevel);
        }

        if (height > 0)
        {
            return height / mGraphics.zoomLevel;
        }

        GUIStyle gUIStyle = new GUIStyle();
        gUIStyle.font = myFont;
        gUIStyle.fontSize = fontPx;
        gUIStyle.fontStyle = fontStyle;
        try
        {
            height = (int)gUIStyle.CalcSize(new GUIContent("Adg")).y + 2;
        }
        catch (Exception ex)
        {
            Out.LogError("FAIL GET HEIGHT " + ex.StackTrace);
            height = 20;
        }

        return height / mGraphics.zoomLevel;
    }

    /// <summary>Draw string.</summary>
    /// <param name="g">Legacy drawing context.</param>
    /// <param name="st">St.</param>
    /// <param name="x0">X0.</param>
    /// <param name="y0">Y0.</param>
    /// <param name="align">Align.</param>
    /// <remarks>Locals: num: Numeric intermediate used by this operation; the legacy name is retained; num2: Numeric intermediate used by this operation; the legacy name is retained; gc: Gc.</remarks>
    public void _drawString(mGraphics g, string st, int x0, int y0, int align)
    {
        if (DrawRecorder.Armed)
        {
            DrawRecorder.TextDepth++;
            try
            {
                float gx = x0 + g.getTranslateX(), gy = y0 + g.getTranslateY();
                if (g.RecorderVisible(gx, gy + 2))
                    DrawRecorder.Text(st, FontIndex(id), color1, gx, gy, align);
                _drawStringCore(g, st, x0, y0, align);
            }
            finally
            {
                DrawRecorder.TextDepth--;
            }

            return;
        }

        _drawStringCore(g, st, x0, y0, align);
    }

    private void _drawStringCore(mGraphics g, string st, int x0, int y0, int align)
    {
        if (bitmapAtlas != null)
        {
            _drawBitmapString(g, st, x0, y0, align);
            return;
        }

        GUIStyle gUIStyle = new GUIStyle(GUI.skin.label);
        gUIStyle.padding = new RectOffset(0, 0, 0, 0);
        gUIStyle.font = myFont;
        gUIStyle.fontSize = fontPx;
        gUIStyle.fontStyle = fontStyle;
        float num = 0f;
        float num2 = 0f;
        switch (align)
        {
            case 0:
                num = x0;
                num2 = y0;
                gUIStyle.alignment = TextAnchor.UpperLeft;
                break;
            case 1:
                num = x0 - ScaleGUI.WIDTH / mGraphics.zoomLevel;
                num2 = y0;
                gUIStyle.alignment = TextAnchor.UpperRight;
                break;
            case 2:
            case 3:
                num = x0 - ScaleGUI.WIDTH / mGraphics.zoomLevel / 2f;
                num2 = y0;
                gUIStyle.alignment = TextAnchor.UpperCenter;
                break;
        }

        gUIStyle.normal.textColor = color1;
        Vector2 gc = TextCalib.Get("global");
        g.drawString(st, num + gc.x + TextCalib.X, num2 + gc.y + TextCalib.Y, gUIStyle);
    }

    /// <summary>Draw bitmap string.</summary>
    /// <param name="g">Legacy drawing context.</param>
    /// <param name="text">Text fragment assembled or read by this operation.</param>
    /// <param name="x0">X0.</param>
    /// <param name="y0">Y0.</param>
    /// <param name="align">Align.</param>
    /// <remarks>Locals: k: Key or nested iteration index; bg: Bg; i: Loop/collection index.</remarks>
    private void _drawBitmapString(mGraphics g, string text, int x0, int y0, int align)
    {
        int zoom = Mathf.Max(1, mGraphics.zoomLevel);
        Vector2 global = TextCalib.Get("global");
        float boxX = x0, boxY = y0;
        if (TextCalib.HasBox && text.IndexOf('\n') < 0)
        {
            // vertical ink extent of the line, so its drawn pixels (not the line box) are centred on the requested point
            float top = float.MaxValue, bottom = float.MinValue;
            for (int k = 0; k < text.Length; k++)
            {
                BitmapGlyph bg;
                if (char.IsWhiteSpace(text[k]) || !bitmapAtlas.TryGet(text[k], out bg))
                    continue;
                top = Mathf.Min(top, bg.offY);
                bottom = Mathf.Max(bottom, bg.offY + bg.height);
            }

            boxX = TextCalib.BoxX;
            boxY = top > bottom ? TextCalib.BoxY : TextCalib.BoxY - (top + bottom) / 2f / zoom;
        }

        float adjustedX = boxX + global.x + TextCalib.X;
        float adjustedY = boxY + global.y + TextCalib.Y;
        float anchorX = adjustedX * zoom;
        float lineStartX = AlignBitmapLine(anchorX, text, 0, align);
        float penX = lineStartX;
        float penY = adjustedY * zoom;
        int lineHeightPx = bitmapAtlas.lineHeight;
        float rectLeft = lineStartX;
        float rectRight = lineStartX + GetBitmapLineWidth(text, 0);
        int lineStart = 0;
        for (int i = 0; i < text.Length; i++)
        {
            char value = text[i];
            if (value == '\r')
            {
                continue;
            }

            if (value == '\n')
            {
                lineStart = i + 1;
                lineStartX = AlignBitmapLine(anchorX, text, lineStart, align);
                penX = lineStartX;
                rectLeft = Mathf.Min(rectLeft, lineStartX);
                rectRight = Mathf.Max(rectRight, lineStartX + GetBitmapLineWidth(text, lineStart));
                penY += lineHeightPx;
                continue;
            }

            BitmapGlyph glyph;
            if (!bitmapAtlas.TryGet(value, out glyph))
            {
                continue;
            }

            if (!char.IsWhiteSpace(value))
            {
                if (glyph.rotated)
                {
                    g._drawRotatedGlyph(bitmapAtlas.image, glyph.x, glyph.y, glyph.width, glyph.height, Mathf.RoundToInt(penX) + glyph.offX, Mathf.RoundToInt(penY) + glyph.offY, bitmapAtlas.material, color1);
                }
                else
                {
                    g._drawRegion(bitmapAtlas.image, glyph.x, glyph.y, glyph.width, glyph.height, mGraphics.TRANS_MIRROR_ROT180, Mathf.RoundToInt(penX) + glyph.offX, Mathf.RoundToInt(penY) + glyph.offY, mGraphics.TOP | mGraphics.LEFT, bitmapAtlas.material, color1);
                }
            }

            penX += glyph.advance;
        }

        TextCalib.RecordBitmap(TextCalib.Key, Mathf.RoundToInt(rectLeft), Mathf.RoundToInt(adjustedY * zoom), Mathf.RoundToInt(rectRight - rectLeft), lineHeightPx * CountLines(text));
    }

    /// <summary>Return bitmap line width.</summary>
    /// <param name="text">Text fragment assembled or read by this operation.</param>
    /// <param name="start">Start.</param>
    /// <remarks>Locals: i: Loop/collection index.</remarks>
    private int GetBitmapLineWidth(string text, int start)
    {
        int width = 0;
        for (int i = start; i < text.Length && text[i] != '\n'; i++)
        {
            if (text[i] != '\r')
                width += bitmapAtlas.GetAdvance(text[i]);
        }

        return width;
    }

    // Text of the tuned items (level, exp, hp, mp, zone numbers, captions...) is aligned by its drawn pixels, not by its advance
    // box: the ink (first glyph's left edge to last glyph's right edge) is centred, or put flush left/right, on the anchor.
    // That makes "centre" mean the visual centre whatever the glyph bearings or italic slant are. Other text keeps the box rule.
    /// <summary>Compute align bitmap line.</summary>
    /// <param name="anchorX">Anchor x.</param>
    /// <param name="text">Text fragment assembled or read by this operation.</param>
    /// <param name="start">Start.</param>
    /// <param name="align">Align.</param>
    /// <remarks>Locals: i: Loop/collection index.</remarks>
    private float AlignBitmapLine(float anchorX, string text, int start, int align)
    {
        int width = GetBitmapLineWidth(text, start);
        if (TextCalib.Key == null)
            return GetAlignedBitmapLineStart(anchorX, width, align);
        float inkLeft = float.MaxValue, inkRight = float.MinValue, pen = 0f;
        for (int i = start; i < text.Length && text[i] != '\n'; i++)
        {
            if (text[i] == '\r')
                continue;
            BitmapGlyph glyph;
            if (!bitmapAtlas.TryGet(text[i], out glyph))
                continue;
            if (!char.IsWhiteSpace(text[i]))
            {
                inkLeft = Mathf.Min(inkLeft, pen + glyph.offX);
                inkRight = Mathf.Max(inkRight, pen + glyph.offX + glyph.width);
            }

            pen += glyph.advance;
        }

        if (inkLeft > inkRight)
            return GetAlignedBitmapLineStart(anchorX, width, align);
        if (align == 1)
            return anchorX - inkRight;
        if (align == 2 || align == 3)
            return anchorX - (inkLeft + inkRight) / 2f;
        return anchorX - inkLeft;
    }

    /// <summary>Return aligned bitmap line start.</summary>
    /// <param name="anchorX">Anchor x.</param>
    /// <param name="width">Width.</param>
    /// <param name="align">Align.</param>
    private static float GetAlignedBitmapLineStart(float anchorX, int width, int align)
    {
        if (align == 1)
            return anchorX - width;
        if (align == 2 || align == 3)
            return anchorX - width / 2f;
        return anchorX;
    }

    /// <summary>Count lines.</summary>
    /// <param name="text">Text fragment assembled or read by this operation.</param>
    /// <remarks>Locals: i: Loop/collection index.</remarks>
    private static int CountLines(string text)
    {
        int lines = 1;
        for (int i = 0; i < text.Length; i++)
        {
            if (text[i] == '\n')
            {
                lines++;
            }
        }

        return lines;
    }

    /// <summary>Split string sv.</summary>
    /// <param name="_text">Text.</param>
    /// <param name="_searchStr">Search text.</param>
    /// <remarks>Locals: num: Numeric intermediate used by this operation; the legacy name is retained; num2: Numeric intermediate used by this operation; the legacy name is retained; array: Entry selected from the source collection; num3: Numeric intermediate used by this operation; the legacy name is retained; num4: Numeric intermediate used by this operation; the legacy name is retained; num5: Numeric intermediate used by this operation; the legacy name is retained.</remarks>
    public static string[] splitStringSv(string _text, string _searchStr)
    {
        int num = 0;
        int startIndex = 0;
        int length = _searchStr.Length;
        int num2 = _text.IndexOf(_searchStr, startIndex);
        while (num2 != -1)
        {
            startIndex = num2 + length;
            num2 = _text.IndexOf(_searchStr, startIndex);
            num++;
        }

        string[] array = new string[num + 1];
        int num3 = _text.IndexOf(_searchStr);
        int num4 = 0;
        int num5 = 0;
        while (num3 != -1)
        {
            array[num5] = _text.Substring(num4, num3 - num4);
            num4 = num3 + length;
            num3 = _text.IndexOf(_searchStr, num4);
            num5++;
        }

        array[num5] = _text.Substring(num4, _text.Length - num4);
        return array;
    }

}

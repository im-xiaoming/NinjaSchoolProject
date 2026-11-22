using System;
using System.Threading;
using UnityEngine;

public class Image
{
    private const int INTERVAL = 5;
    private const int MAXTIME = 500;
    public Texture2D texture = new Texture2D(1, 1);
    public static Image imgTemp;
    public static string filenametemp;
    public static byte[] datatemp;
    public static Image imgSrcTemp;
    public static int xtemp;
    public static int ytemp;
    public static int wtemp;
    public static int htemp;
    public static int transformtemp;
    // w: Width or horizontal span.
    public int w;
    // h: Height or vertical span.
    public int h;
    public static int status;
    /// <summary>Create empty image.</summary>
    public static Image createEmptyImage()
    {
        return __createEmptyImage();
    }

    /// <summary>Create image.</summary>
    /// <param name="filename">Filename.</param>
    public static Image createImage(string filename)
    {
        return __createImage(filename);
    }

    /// <summary>Create image.</summary>
    /// <param name="imageData">Image data.</param>
    public static Image createImage(byte[] imageData)
    {
        return __createImage(imageData);
    }

    /// <summary>Create image.</summary>
    /// <param name="src">Source.</param>
    /// <param name="x">Horizontal coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="y">Vertical coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="w">Width or horizontal span.</param>
    /// <param name="h">Height or vertical span.</param>
    /// <param name="transform">Transform.</param>
    public static Image createImage(Image src, int x, int y, int w, int h, int transform)
    {
        return __createImage(src, x, y, w, h, transform);
    }

    /// <summary>Create image.</summary>
    /// <param name="w">Width or horizontal span.</param>
    /// <param name="h">Height or vertical span.</param>
    public static Image createImage(int w, int h)
    {
        return __createImage(w, h);
    }

    /// <summary>Create image.</summary>
    /// <param name="imageData">Image data.</param>
    /// <param name="offset">Offset.</param>
    /// <param name="lenght">Lenght.</param>
    /// <remarks>Locals: array: Entry selected from the source collection; i: Loop/collection index.</remarks>
    public static Image createImage(sbyte[] imageData, int offset, int lenght)
    {
        if (offset + lenght > imageData.Length)
        {
            return null;
        }

        byte[] array = new byte[lenght];
        for (int i = 0; i < lenght; i++)
        {
            array[i] = convertSbyteToByte(imageData[i + offset]);
        }

        return createImage(array);
    }

    /// <summary>Copy signed protocol bytes into an unsigned byte array without changing their bit patterns.</summary>
    /// <param name="var">Compatibility argument retained under its original legacy name.</param>
    public static byte convertSbyteToByte(sbyte var)
    {
        if (var > 0)
        {
            return (byte)var;
        }

        return (byte)(var + 256);
    }

    /// <summary>Convert arr signed byte to arr byte.</summary>
    /// <param name="var">Compatibility argument retained under its original legacy name.</param>
    /// <remarks>Locals: array: Number of entries in the source collection; i: Loop/collection index.</remarks>
    public static byte[] convertArrSbyteToArrByte(sbyte[] var)
    {
        byte[] array = new byte[var.Length];
        for (int i = 0; i < var.Length; i++)
        {
            if (var[i] > 0)
            {
                array[i] = (byte)var[i];
            }
            else
            {
                array[i] = (byte)(var[i] + 256);
            }
        }

        return array;
    }

    /// <summary>Create red/green/blue channels image.</summary>
    /// <param name="rbg">Rbg.</param>
    /// <param name="w">Width or horizontal span.</param>
    /// <param name="h">Height or vertical span.</param>
    /// <param name="bl">Retained for call compatibility; not read by this implementation.</param>
    /// <remarks>Locals: array: Number of entries in the source collection; i: Loop/collection index.</remarks>
    public static Image createRGBImage(int[] rbg, int w, int h, bool bl)
    {
        Image image = createImage(w, h);
        Color[] array = new Color[rbg.Length];
        for (int i = 0; i < array.Length; i++)
        {
            ref Color reference = ref array[i];
            reference = setColorFromRBG(rbg[i]);
        }

        image.texture.SetPixels(0, 0, w, h, array);
        image.texture.Apply();
        return image;
    }

    /// <summary>Set color from rbg.</summary>
    /// <param name="rgb">Red/green/blue channels.</param>
    /// <remarks>Locals: num: Numeric intermediate used by this operation; the legacy name is retained; num2: Numeric intermediate used by this operation; the legacy name is retained; num3: Numeric intermediate used by this operation; the legacy name is retained; b: Buffer or intermediate output value; g: Drawing context; r: Result being assembled or returned.</remarks>
    public static Color setColorFromRBG(int rgb)
    {
        int num = rgb & 0xFF;
        int num2 = (rgb >> 8) & 0xFF;
        int num3 = (rgb >> 16) & 0xFF;
        float b = (float)num / 256f;
        float g = (float)num2 / 256f;
        float r = (float)num3 / 256f;
        return new Color(r, g, b);
    }

    /// <summary>Advance this object&#x27;s state for the current client logic tick.</summary>
    public static void update()
    {
        if (status == 2)
        {
            status = 1;
            imgTemp = __createEmptyImage();
            status = 0;
        }
        else if (status == 3)
        {
            status = 1;
            imgTemp = __createImage(filenametemp);
            status = 0;
        }
        else if (status == 4)
        {
            status = 1;
            imgTemp = __createImage(datatemp);
            status = 0;
        }
        else if (status == 5)
        {
            status = 1;
            imgTemp = __createImage(imgSrcTemp, xtemp, ytemp, wtemp, htemp, transformtemp);
            status = 0;
        }
        else if (status == 6)
        {
            status = 1;
            imgTemp = __createImage(wtemp, htemp);
            status = 0;
        }
    }

    /// <summary>Create empty image.</summary>
    /// <remarks>Locals: i: Loop/collection index.</remarks>
    private static Image _createEmptyImage()
    {
        if (status != 0)
        {
            Debug.LogError("CANNOT CREATE EMPTY IMAGE WHEN CREATING OTHER IMAGE");
            return null;
        }

        imgTemp = null;
        status = 2;
        int i;
        for (i = 0; i < 500; i++)
        {
            Thread.Sleep(5);
            if (status == 0)
            {
                break;
            }
        }

        if (i == 500)
        {
            Debug.LogError("TOO LONG FOR CREATE EMPTY IMAGE");
            status = 0;
        }

        return imgTemp;
    }

    /// <summary>Create image.</summary>
    /// <param name="filename">Filename.</param>
    /// <remarks>Locals: i: Loop/collection index.</remarks>
    private static Image _createImage(string filename)
    {
        if (status != 0)
        {
            Debug.LogError("CANNOT CREATE IMAGE " + filename + " WHEN CREATING OTHER IMAGE");
            return null;
        }

        imgTemp = null;
        filenametemp = filename;
        status = 3;
        int i;
        for (i = 0; i < 500; i++)
        {
            Thread.Sleep(5);
            if (status == 0)
            {
                break;
            }
        }

        if (i == 500)
        {
            Debug.LogError("TOO LONG FOR CREATE IMAGE " + filename);
            status = 0;
        }

        return imgTemp;
    }

    /// <summary>Create image.</summary>
    /// <param name="imageData">Image data.</param>
    /// <remarks>Locals: i: Loop/collection index.</remarks>
    private static Image _createImage(byte[] imageData)
    {
        if (status != 0)
        {
            Debug.LogError("CANNOT CREATE IMAGE(FromArray) WHEN CREATING OTHER IMAGE");
            return null;
        }

        imgTemp = null;
        datatemp = imageData;
        status = 4;
        int i;
        for (i = 0; i < 500; i++)
        {
            Thread.Sleep(5);
            if (status == 0)
            {
                break;
            }
        }

        if (i == 500)
        {
            Debug.LogError("TOO LONG FOR CREATE IMAGE(FromArray)");
            status = 0;
        }

        return imgTemp;
    }

    /// <summary>Create image.</summary>
    /// <param name="src">Source.</param>
    /// <param name="x">Horizontal coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="y">Vertical coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="w">Width or horizontal span.</param>
    /// <param name="h">Height or vertical span.</param>
    /// <param name="transform">Transform.</param>
    /// <remarks>Locals: i: Loop/collection index.</remarks>
    private static Image _createImage(Image src, int x, int y, int w, int h, int transform)
    {
        if (status != 0)
        {
            Debug.LogError("CANNOT CREATE IMAGE(FromSrcPart) WHEN CREATING OTHER IMAGE");
            return null;
        }

        imgTemp = null;
        imgSrcTemp = src;
        xtemp = x;
        ytemp = y;
        wtemp = w;
        htemp = h;
        transformtemp = transform;
        status = 5;
        int i;
        for (i = 0; i < 500; i++)
        {
            Thread.Sleep(5);
            if (status == 0)
            {
                break;
            }
        }

        if (i == 500)
        {
            Debug.LogError("TOO LONG FOR CREATE IMAGE(FromSrcPart)");
            status = 0;
        }

        return imgTemp;
    }

    /// <summary>Create image.</summary>
    /// <param name="w">Width or horizontal span.</param>
    /// <param name="h">Height or vertical span.</param>
    /// <remarks>Locals: i: Loop/collection index.</remarks>
    private static Image _createImage(int w, int h)
    {
        if (status != 0)
        {
            Debug.LogError("CANNOT CREATE IMAGE(w,h) WHEN CREATING OTHER IMAGE");
            return null;
        }

        imgTemp = null;
        wtemp = w;
        htemp = h;
        status = 6;
        int i;
        for (i = 0; i < 500; i++)
        {
            Thread.Sleep(5);
            if (status == 0)
            {
                break;
            }
        }

        if (i == 500)
        {
            Debug.LogError("TOO LONG FOR CREATE IMAGE(w,h)");
            status = 0;
        }

        return imgTemp;
    }

    /// <summary>Create image.</summary>
    /// <param name="filename">Filename.</param>
    private static Image __createImage(string filename)
    {
        Image image = new Image();
        Texture2D texture2D = Resources.Load(filename) as Texture2D;
        if (texture2D == null)
        {
            return null;
        }

        image.texture = texture2D;
        image.w = image.texture.width;
        image.h = image.texture.height;
        setTextureQuality(image);
        return image;
    }

    /// <summary>Create image.</summary>
    /// <param name="imageData">Image data.</param>
    private static Image __createImage(byte[] imageData)
    {
        if (imageData == null || imageData.Length == 0)
        {
            Debug.LogError("Create Image from byte array fail");
            return null;
        }

        Image image = new Image();
        try
        {
            image.texture.LoadImage(imageData);
            image.w = image.texture.width;
            image.h = image.texture.height;
            setTextureQuality(image);
        }
        catch (Exception)
        {
            Debug.LogError("CREAT IMAGE FROM ARRAY FAIL \n" + Environment.StackTrace);
        }

        return image;
    }

    /// <summary>Create image.</summary>
    /// <param name="src">Source.</param>
    /// <param name="x">Horizontal coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="y">Vertical coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="w">Width or horizontal span.</param>
    /// <param name="h">Height or vertical span.</param>
    /// <param name="transform">Transform.</param>
    /// <remarks>Locals: i: Loop/collection index; j: Nested loop/collection index; num: Numeric intermediate used by this operation; the legacy name is retained; num2: Numeric intermediate used by this operation; the legacy name is retained.</remarks>
    private static Image __createImage(Image src, int x, int y, int w, int h, int transform)
    {
        Image image = new Image();
        image.texture = new Texture2D(w, h);
        y = src.texture.height - y - h;
        for (int i = 0; i < w; i++)
        {
            for (int j = 0; j < h; j++)
            {
                int num = i;
                if (transform == 2)
                {
                    num = w - i;
                }

                int num2 = j;
                image.texture.SetPixel(i, j, src.texture.GetPixel(x + num, y + num2));
            }
        }

        image.texture.Apply();
        image.w = image.texture.width;
        image.h = image.texture.height;
        setTextureQuality(image);
        return image;
    }

    /// <summary>Create empty image.</summary>
    private static Image __createEmptyImage()
    {
        return new Image();
    }

    /// <summary>Create image.</summary>
    /// <param name="w">Width or horizontal span.</param>
    /// <param name="h">Height or vertical span.</param>
    public static Image __createImage(int w, int h)
    {
        Image image = new Image();
        image.texture = new Texture2D(w, h);
        setTextureQuality(image);
        image.w = w;
        image.h = h;
        return image;
    }

    /// <summary>Return image width.</summary>
    /// <param name="image">Image.</param>
    public static int getImageWidth(Image image)
    {
        return image.getWidth();
    }

    /// <summary>Return image height.</summary>
    /// <param name="image">Image.</param>
    public static int getImageHeight(Image image)
    {
        return image.getHeight();
    }

    /// <summary>Return width.</summary>
    public int getWidth()
    {
        return w / mGraphics.zoomLevel;
    }

    /// <summary>Return height.</summary>
    public int getHeight()
    {
        return h / mGraphics.zoomLevel;
    }

    /// <summary>Set texture quality.</summary>
    /// <param name="img">Image.</param>
    private static void setTextureQuality(Image img)
    {
        setTextureQuality(img.texture);
    }

    /// <summary>Set texture quality.</summary>
    /// <param name="texture">Texture.</param>
    private static void setTextureQuality(Texture2D texture)
    {
        texture.anisoLevel = 0;
        texture.filterMode = FilterMode.Point;
        texture.mipMapBias = 0f;
        texture.wrapMode = TextureWrapMode.Clamp;
    }
}

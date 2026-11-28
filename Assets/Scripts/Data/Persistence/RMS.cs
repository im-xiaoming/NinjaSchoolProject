using Functions.Auto;
using System;
using System.Threading;
using UnityEngine;

public class RMS
{
    public static int status;
    public static sbyte[] data;
    public static string filename;
    private const int INTERVAL = 5;
    private const int MAXTIME = 500;
    /// <summary>Save rms image.</summary>
    /// <param name="idImage">Retained for call compatibility; not read by this implementation.</param>
    /// <param name="data">Data.</param>
    public static void saveRMSImage(string idImage, sbyte[] data)
    {
        if (Thread.CurrentThread.Name == Main.mainThreadName)
        {
            __saveRMS(filename, data);
        }
        else
        {
            _saveRMS(filename, data);
        }
    }

    /// <summary>Save rms.</summary>
    /// <param name="filename">Filename.</param>
    /// <param name="data">Data.</param>
    public static void saveRMS(string filename, sbyte[] data)
    {
        if (Thread.CurrentThread.Name == Main.mainThreadName)
        {
            __saveRMS(filename, data);
        }
        else
        {
            _saveRMS(filename, data);
        }
    }

    /// <summary>Load rms.</summary>
    /// <param name="filename">Filename.</param>
    public static sbyte[] loadRMS(string filename)
    {
        if (Thread.CurrentThread.Name == Main.mainThreadName)
        {
            return __loadRMS(filename);
        }

        return _loadRMS(filename);
    }

    /// <summary>Copy signed protocol bytes into an unsigned byte array without changing their bit patterns.</summary>
    /// <param name="var">Compatibility argument retained under its original legacy name.</param>
    /// <remarks>Locals: array: Number of entries in the source collection; i: Loop/collection index.</remarks>
    public static byte[] convertSbyteToByte(sbyte[] var)
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

    /// <summary>Save rms string.</summary>
    /// <param name="filename">Filename.</param>
    /// <param name="data">Data.</param>
    public static void saveRMSString(string filename, string data)
    {
        DataOutputStream dataOutputStream = new DataOutputStream();
        try
        {
            dataOutputStream.writeUTF(data);
            saveRMS(filename, dataOutputStream.toByteArray());
            dataOutputStream.close();
        }
        catch (Exception ex)
        {
            Out.println(ex.StackTrace);
        }
    }

    /// <summary>Save rms.</summary>
    /// <param name="filename">Filename.</param>
    /// <param name="data">Data.</param>
    /// <remarks>Locals: i: Loop/collection index.</remarks>
    private static void _saveRMS(string filename, sbyte[] data)
    {
        if (status != 0)
        {
            Debug.LogError("Cannot save RMS " + filename + " because current is saving " + RMS.filename);
            return;
        }

        RMS.filename = filename;
        RMS.data = data;
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
            Debug.LogError("TOO LONG TO SAVE RMS " + filename);
        }
    }

    /// <summary>Load rms.</summary>
    /// <param name="filename">Filename.</param>
    /// <remarks>Locals: i: Loop/collection index.</remarks>
    private static sbyte[] _loadRMS(string filename)
    {
        if (status != 0)
        {
            Debug.LogError("Cannot load RMS " + filename + " because current is loading " + RMS.filename);
            return null;
        }

        RMS.filename = filename;
        data = null;
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
            Debug.LogError("TOO LONG TO LOAD RMS " + filename);
        }

        return data;
    }

    /// <summary>Advance this object&#x27;s state for the current client logic tick.</summary>
    public static void update()
    {
        if (status == 2)
        {
            status = 1;
            __saveRMS(filename, data);
            status = 0;
        }
        else if (status == 3)
        {
            status = 1;
            data = __loadRMS(filename);
            status = 0;
        }
    }

    /// <summary>Load rms int.</summary>
    /// <param name="file">File.</param>
    /// <remarks>Locals: array: Working array for this operation.</remarks>
    public static int loadRMSInt(string file)
    {
        sbyte[] array = loadRMS(file);
        return (array != null) ? array[0] : (-1);
    }

    /// <summary>Save rms int.</summary>
    /// <param name="file">File.</param>
    /// <param name="x">Horizontal coordinate in the caller&#x27;s coordinate space.</param>
    public static void saveRMSInt(string file, int x)
    {
        try
        {
            saveRMS(file, new sbyte[1] { (sbyte)x });
        }
        catch (Exception)
        {
        }
    }

    /// <summary>Save rms.</summary>
    /// <param name="filename">Filename.</param>
    /// <param name="data">Data.</param>
    private static void __saveRMS(string filename, sbyte[] data)
    {
        string value = ByteArrayToString(ArrayCast.cast(data));
        PlayerPrefs.SetString(filename, value);
    }

    /// <summary>Load rms.</summary>
    /// <param name="filename">Filename.</param>
    /// <remarks>Locals: array: Working array for this operation.</remarks>
    private static sbyte[] __loadRMS(string filename)
    {
        string @string = PlayerPrefs.GetString(filename);
        byte[] array;
        try
        {
            array = StringToByteArray(@string);
        }
        catch (Exception ex)
        {
            Debug.LogError("PARSE RMS STRING FAIL " + ex.StackTrace);
            return null;
        }

        if (array.Length == 0)
        {
            return null;
        }

        return ArrayCast.cast(array);
    }

    /// <summary>Compute byte array to string.</summary>
    /// <param name="ba">Ba.</param>
    /// <remarks>Locals: text: Text fragment assembled or read by this operation.</remarks>
    public static string ByteArrayToString(byte[] ba)
    {
        string text = BitConverter.ToString(ba);
        return text.Replace("-", string.Empty);
    }

    /// <summary>Compute string to byte array.</summary>
    /// <param name="hex">Hex.</param>
    /// <remarks>Locals: array: Entry selected from the source collection; i: Loop/collection index.</remarks>
    public static byte[] StringToByteArray(string hex)
    {
        int length = hex.Length;
        byte[] array = new byte[length / 2];
        for (int i = 0; i < length; i += 2)
        {
            array[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
        }

        return array;
    }

    /// <summary>Delete record.</summary>
    /// <param name="name">Name.</param>
    public static void deleteRecord(string name)
    {
        try
        {
            PlayerPrefs.DeleteKey(name);
        }
        catch (Exception ex)
        {
            Debug.Log("Xoa RMS Error");
            Out.println("loi xoa RMS --------------------------" + ex.ToString());
        }
    }

    /// <summary>Clear rms.</summary>
    public static void clearRMS()
    {
        deleteRecord("nj_arrow");
        deleteRecord("nj_effect");
        deleteRecord("nj_image");
        deleteRecord("nj_part");
        deleteRecord("nj_skill");
        deleteRecord("data");
        deleteRecord("dataVersion");
        deleteRecord("map");
        deleteRecord("mapVersion");
        deleteRecord("skill");
        deleteRecord("killVersion");
        deleteRecord("item");
        deleteRecord("itemVersion");
        deleteRecord("NjAuto");
    }
}

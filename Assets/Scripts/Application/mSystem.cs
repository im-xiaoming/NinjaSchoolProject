using System;
using System.Text;
using UnityEngine;

public class mSystem
{
    /// <summary>Perform arraycopy.</summary>
    /// <param name="scr">Screen.</param>
    /// <param name="scrPos">Screen position.</param>
    /// <param name="dest">Dest.</param>
    /// <param name="destPos">Dest position.</param>
    /// <param name="lenght">Lenght.</param>
    public static void arraycopy(sbyte[] scr, int scrPos, sbyte[] dest, int destPos, int lenght)
    {
        Array.Copy(scr, scrPos, dest, destPos, lenght);
    }

    /// <summary>Perform array replace.</summary>
    /// <param name="scr">Screen.</param>
    /// <param name="scrPos">Screen position.</param>
    /// <param name="dest">Dest.</param>
    /// <param name="destPos">Dest position.</param>
    /// <param name="lenght">Lenght.</param>
    /// <remarks>Locals: array: Number of entries in the source collection; i: Loop/collection index; j: Nested loop/collection index; k: Key or nested iteration index.</remarks>
    public static void arrayReplace(sbyte[] scr, int scrPos, ref sbyte[] dest, int destPos, int lenght)
    {
        if (scr != null && dest != null && scrPos + lenght <= scr.Length)
        {
            sbyte[] array = new sbyte[dest.Length + lenght];
            for (int i = 0; i < destPos; i++)
            {
                array[i] = dest[i];
            }

            for (int j = destPos; j < destPos + lenght; j++)
            {
                array[j] = scr[scrPos + j - destPos];
            }

            for (int k = destPos + lenght; k < array.Length; k++)
            {
                array[k] = dest[destPos + k - lenght];
            }
        }
    }

    /// <summary>Return current time millis.</summary>
    public static long getCurrentTimeMillis()
    {
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        return (DateTime.UtcNow.Ticks - dateTime.Ticks) / 10000;
    }

    /// <summary>Perform free data.</summary>
    public static void freeData()
    {
        Resources.UnloadUnusedAssets();
        GC.Collect();
    }

    /// <summary>Convert to signed byte.</summary>
    /// <param name="scr">Screen.</param>
    /// <remarks>Locals: array: Number of entries in the source collection; i: Loop/collection index.</remarks>
    public static sbyte[] convertToSbyte(byte[] scr)
    {
        sbyte[] array = new sbyte[scr.Length];
        for (int i = 0; i < scr.Length; i++)
        {
            array[i] = (sbyte)scr[i];
        }

        return array;
    }

    /// <summary>Convert to signed byte.</summary>
    /// <param name="scr">Screen.</param>
    public static sbyte[] convertToSbyte(string scr)
    {
        ASCIIEncoding aSCIIEncoding = new ASCIIEncoding();
        byte[] bytes = aSCIIEncoding.GetBytes(scr);
        return convertToSbyte(bytes);
    }

    /// <summary>Compute convet to byte.</summary>
    /// <param name="scr">Screen.</param>
    /// <remarks>Locals: array: Number of entries in the source collection; i: Loop/collection index.</remarks>
    public static byte[] convetToByte(sbyte[] scr)
    {
        byte[] array = new byte[scr.Length];
        for (int i = 0; i < scr.Length; i++)
        {
            if (scr[i] > 0)
            {
                array[i] = (byte)scr[i];
            }
            else
            {
                array[i] = (byte)(scr[i] + 256);
            }
        }

        return array;
    }

    /// <summary>Compute to character array.</summary>
    /// <param name="scr">Screen.</param>
    /// <remarks>Locals: array: Number of entries in the source collection; i: Loop/collection index.</remarks>
    public static char[] ToCharArray(sbyte[] scr)
    {
        char[] array = new char[scr.Length];
        for (int i = 0; i < scr.Length; i++)
        {
            array[i] = (char)scr[i];
        }

        return array;
    }

    /// <summary>Compute current time millis.</summary>
    public static long currentTimeMillis()
    {
        return getCurrentTimeMillis();
    }

    /// <summary>Compute current hour.</summary>
    public static int currentHour()
    {
        return DateTime.Now.Hour;
    }

    /// <summary>Perform println.</summary>
    /// <param name="str">Text.</param>
    public static void println(object str)
    {
        Debug.Log(str);
    }

    /// <summary>Request garbage collection through the compatibility platform layer.</summary>
    public static void gc()
    {
        GC.Collect();
    }

    /// <summary>Retain the set scale compatibility hook without performing work.</summary>
    public static void setScale()
    {
    }
}

using System;
using System.Text;
using UnityEngine;

public class mSystem
{
    /// <summary>Return current time millis.</summary>
    public static long getCurrentTimeMillis()
    {
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        return (DateTime.UtcNow.Ticks - dateTime.Ticks) / 10000;
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

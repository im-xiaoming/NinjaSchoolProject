using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEngine;

internal class GameTcp
{
    /// <summary>Compute binary to string.</summary>
    /// <param name="binary">Binary.</param>
    /// <remarks>Locals: i: Loop/collection index; c: Client character being inspected or controlled.</remarks>
    public static string BinaryToString(string binary)
    {
        StringBuilder resultStringBuilder = new StringBuilder();
        for (int i = 0; i < binary.Length; i += 8)
        {
            string binaryChar = binary.Substring(i, 8);
            char c = (char)Convert.ToInt32(binaryChar, 2);
            resultStringBuilder.Append(c);
        }

        return resultStringBuilder.ToString();
    }

    /// <summary>Compute hex2 string.</summary>
    /// <param name="A_0">A 0.</param>
    /// <param name="A_1">A 1.</param>
    /// <remarks>Locals: array: Number of entries in the source collection; i: Loop/collection index.</remarks>
    public static string Hex2String(string A_0, object A_1)
    {
        A_1 = 0;
        A_0 = Regex.Replace(A_0, "[^0-9A-Fa-f]", "");
        if (A_0.Length % 2 != Convert.ToInt32(A_1))
        {
            A_0 = A_0.Remove(A_0.Length - 1, 1);
        }

        if (A_0.Length <= 0)
        {
            return "";
        }

        byte[] array = new byte[A_0.Length / 2];
        for (int i = 0; i < A_0.Length; i += 2)
        {
            if (!byte.TryParse(A_0.Substring(i, 2), NumberStyles.HexNumber, null, out array[i / 2]))
            {
                array[i / 2] = 0;
            }
        }

        return (Encoding.Default.GetString(array));
    }

    /// <summary>Compute reverse string.</summary>
    /// <param name="input">Input.</param>
    public static string ReverseString(string input)
    {
        char[] charArray = input.ToCharArray();
        Array.Reverse(charArray);
        return new string (charArray);
    }

    /// <summary>Read file text.</summary>
    /// <param name="fileName">File name.</param>
    public static string readFileText(string fileName)
    {
        try
        {
            Debug.Log(Main.res + "/" + fileName);
            TextAsset textAsset = (TextAsset)Resources.Load(Main.res + "/" + fileName, typeof(TextAsset));
            if (textAsset != null)
            {
                StringReader reader = new StringReader(textAsset.text);
                string firstLine = reader.ReadLine();
                return firstLine != null ? firstLine : string.Empty;
            }
            else
            {
                Debug.LogWarning("TextAsset not found: " + fileName);
                return string.Empty;
            }
        }
        catch (IOException)
        {
            return string.Empty;
        }
    }

    /// <summary>Read file text2.</summary>
    /// <param name="fileName">File name.</param>
    public static string readFileText2(string fileName)
    {
        try
        {
            Debug.Log(Main.res + "/" + fileName);
            TextAsset textAsset = (TextAsset)Resources.Load(Main.res + "/" + fileName, typeof(TextAsset));
            if (textAsset != null)
            {
                return textAsset.text;
            }
            else
            {
                Debug.LogWarning("TextAsset not found: " + fileName);
                return string.Empty;
            }
        }
        catch (IOException)
        {
            return string.Empty;
        }
    }
}

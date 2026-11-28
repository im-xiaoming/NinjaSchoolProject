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

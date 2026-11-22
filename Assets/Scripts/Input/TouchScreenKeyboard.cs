using UnityEngine;

public class TouchScreenKeyboard
{
    public static bool hideInput;
    public static bool visible;
    public bool done;
    // text: Text fragment assembled or read by this operation.
    public string text;
    /// <summary>Open touch screen keyboard.</summary>
    /// <param name="text">Text fragment assembled or read by this operation.</param>
    /// <param name="t">Current entry or time sample.</param>
    /// <param name="b1">B1.</param>
    /// <param name="b2">B2.</param>
    /// <param name="type">Type.</param>
    /// <param name="b3">B3.</param>
    /// <param name="caption">Caption.</param>
    public static UnityEngine.TouchScreenKeyboard Open(string text, TouchScreenKeyboardType t, bool b1, bool b2, bool type, bool b3, string caption)
    {
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            TouchScreenKeyboard.hideInput = true;
            return UnityEngine.TouchScreenKeyboard.Open(text, (UnityEngine.TouchScreenKeyboardType)(int)t, b1, b2, type, b3, caption);
        }

        return null;
    }

    /// <summary>Clear touch screen keyboard.</summary>
    public static void Clear()
    {
        visible = false;
    }
}

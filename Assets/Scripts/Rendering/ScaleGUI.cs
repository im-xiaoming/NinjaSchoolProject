using System.Collections.Generic;
using UnityEngine;

public class ScaleGUI
{
    public static bool scaleScreen;
    public static float WIDTH;
    public static float HEIGHT;
    private static List<Matrix4x4> stack = new List<Matrix4x4>();
    /// <summary>Initialize scale GUI.</summary>
    public static void initScaleGUI()
    {
        Res.println("Init Scale GUI: Screen.w=" + Screen.width + " Screen.h=" + Screen.height);
        WIDTH = Screen.width;
        HEIGHT = Screen.height;
        scaleScreen = false;
        if (Screen.width <= 1200)
        {
        }
    }

}

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

    /// <summary>Begin GUI.</summary>
    /// <remarks>Locals: num: Measured horizontal size; num2: Measured vertical size; num3: Numeric intermediate used by this operation; the legacy name is retained; num4: Numeric intermediate used by this operation; the legacy name is retained.</remarks>
    public static void BeginGUI()
    {
        if (scaleScreen)
        {
            stack.Add(GUI.matrix);
            Matrix4x4 matrix4x = default(Matrix4x4);
            float num = Screen.width;
            float num2 = Screen.height;
            float num3 = num / num2;
            float num4 = 1f;
            Vector3 zero = Vector3.zero;
            num4 = ((!(num3 < WIDTH / HEIGHT)) ? ((float)Screen.height / HEIGHT) : ((float)Screen.width / WIDTH));
            matrix4x.SetTRS(zero, Quaternion.identity, Vector3.one * num4);
            GUI.matrix *= matrix4x;
        }
    }

    /// <summary>End GUI.</summary>
    public static void EndGUI()
    {
        if (scaleScreen)
        {
            GUI.matrix = stack[stack.Count - 1];
            stack.RemoveAt(stack.Count - 1);
        }
    }

    /// <summary>Compute scale x.</summary>
    /// <param name="x">Horizontal coordinate in the caller&#x27;s coordinate space.</param>
    public static float scaleX(float x)
    {
        if (!scaleScreen)
        {
            return x;
        }

        x = x * WIDTH / (float)Screen.width;
        return x;
    }

    /// <summary>Compute scale y.</summary>
    /// <param name="y">Vertical coordinate in the caller&#x27;s coordinate space.</param>
    public static float scaleY(float y)
    {
        if (!scaleScreen)
        {
            return y;
        }

        y = y * HEIGHT / (float)Screen.height;
        return y;
    }
}

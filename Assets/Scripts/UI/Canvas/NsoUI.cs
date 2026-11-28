using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Root of the Canvas UI that replaces the game's own UI drawing, one screen at a time (phased migration).
// The Canvas is Screen Space - Overlay with Constant Pixel Size, scaled by the game's zoom level, so
// 1 canvas unit = 1 game pixel and the canvas is GameCanvas.w x GameCanvas.h: views use the game's own layout numbers.
// The views read the game's state every frame and call the game's actions when clicked; the game logic is unchanged.
[RequireComponent(typeof(Canvas), typeof(CanvasScaler))]
public class NsoUI : MonoBehaviour
{
    public static NsoUI Instance;
    private CanvasScaler scaler;
    private static bool pressOnUI;

    private void Awake()
    {
        Instance = this;
        scaler = GetComponent<CanvasScaler>();
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }

    private void LateUpdate()
    {
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ConstantPixelSize;
        scaler.scaleFactor = Mathf.Max(1, mGraphics.zoomLevel);
    }

    // Places a rect in game pixels: (x, y) = its top-left corner from the top-left of the game screen.
    // The pivot stays in the centre, so a scale of -1 flips the image in place (mGraphics mirror transforms).
    public static void Place(RectTransform rt, float x, float y, float w, float h)
    {
        rt.anchorMin = rt.anchorMax = new Vector2(0, 1);
        rt.pivot = new Vector2(0.5f, 0.5f);
        rt.sizeDelta = new Vector2(w, h);
        rt.anchoredPosition = new Vector2(x + w / 2f, -(y + h / 2f));
    }

    // Where paintCmdBar draws a bottom button: its own x / y when set, otherwise the default slot.
    // slot: 0 = left, 1 = centre, 2 = right.
    public static Vector2 CommandPosition(Command cmd, int slot)
    {
        if (cmd.x > 0 && cmd.y > 0)
            return new Vector2(cmd.x, cmd.y);
        float x = slot == 0 ? Paint.CmdLeftX : slot == 1 ? GameCanvas.hw - 35 : Paint.CmdRightX;
        return new Vector2(x, Paint.CmdY);
    }

    // ---- pointer filter (GameCanvas.pointerPressed / Dragged / Released) ----
    // A press that starts on a Canvas element belongs to the UI: the game does not see it, its drag or its release,
    // so one click never triggers both the new button and the old hit test at the same spot.
    public static bool BeginPress()
    {
        pressOnUI = Instance != null && EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();
        return pressOnUI;
    }

    public static bool PressIsOnUI
    {
        get { return pressOnUI; }
    }

    public static bool EndPress()
    {
        bool was = pressOnUI;
        pressOnUI = false;
        return was;
    }
}

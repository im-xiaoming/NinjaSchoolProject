using UnityEngine;

// Canvas version of SplashScr.paint: a black screen with the game's title image (tt) in the centre on PC.
// Shown while the current screen is the splash screen.
public class SplashView : MonoBehaviour
{
    public GameObject content;
    public GameObject logo;

    private void LateUpdate()
    {
        bool on = GameCanvas.currentScreen is SplashScr;
        if (content.activeSelf != on)
            content.SetActive(on);
        if (on && logo.activeSelf != Main.isPC)
            logo.SetActive(Main.isPC);
    }
}

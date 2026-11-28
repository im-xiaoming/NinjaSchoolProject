using UnityEngine;

// The splash screen of the menu scene (SplashScr: black screen with the title image).
// It stays for 6 logic ticks of the game (SplashScr.update switches screens at splashScrStat == 6, 24 ticks a second),
// then hides itself so the language menu under it shows. Turn the object off in the editor to work on the menu.
public class SplashPanel : MonoBehaviour
{
    [Tooltip("Seconds the splash stays on screen (the game: 6 ticks at 24 ticks a second = 0.25 s).")]
    public float seconds = 6f / 24f;

    private float shownAt;

    private void OnEnable()
    {
        shownAt = Time.unscaledTime;
    }

    private void Update()
    {
        if (Time.unscaledTime - shownAt >= seconds)
            gameObject.SetActive(false);
    }
}

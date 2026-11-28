using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Canvas version of a bottom button (Command.paint / Paint.paintCmdBar): the btnl image with a tahoma_7b_yellow caption.
// Show() binds it to a game Command every frame; a click runs the command the same way mScreen.updateKey does.
public class UICommandButton : MonoBehaviour
{
    public UnityEngine.UI.Image image;
    public TMP_Text caption;
    public Button button;
    public UnityEngine.Sprite normal, focused;

    // Set by a view whose buttons are not plain Commands (e.g. the NPC menu's Chọn / Đóng run Menu methods).
    public System.Action clickOverride;

    private Command command;

    private void Awake()
    {
        button.onClick.AddListener(Click);
    }

    // slot: 0 = left, 1 = centre, 2 = right (default positions of paintCmdBar); selected = the keyboard / gamepad choice
    public void Show(Command cmd, int slot, Command selected = null)
    {
        command = cmd;
        bool visible = cmd != null && !string.IsNullOrEmpty(cmd.caption);
        if (gameObject.activeSelf != visible)
            gameObject.SetActive(visible);
        if (!visible)
            return;
        Vector2 p = NsoUI.CommandPosition(cmd, slot);
        NsoUI.Place((RectTransform)transform, p.x, p.y, 70, 22);
        if (caption.text != cmd.caption)
            caption.text = cmd.caption;
        image.sprite = selected == cmd ? focused : normal;
    }

    public void Hide()
    {
        command = null;
        if (gameObject.activeSelf)
            gameObject.SetActive(false);
    }

    private void Click()
    {
        if (command == null)
            return;
        Sound.play(Sound.MBClick, 0.5f);
        if (clickOverride != null)
            clickOverride();
        else
            command.performAction();
    }
}

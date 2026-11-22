using UnityEngine;

namespace Assets.Scripts.Functions
{
    public class CommandFuncs : MonoBehaviour, IActionListener
    {
        public static CommandFuncs instance;
        /// <summary>Initialize this component when Unity starts it.</summary>
        private void Start()
        {
            instance = this;
        }

        /// <summary>Dispatch the selected menu/action identifier to its existing handler.</summary>
        /// <param name="idAction">Selected menu/action identifier.</param>
        /// <param name="p">Retained for call compatibility; not read by this implementation.</param>
        public void perform(int idAction, object p)
        {
            switch (idAction)
            {
                case 8882:
                    InfoDlg.hide();
                    GameCanvas.currentDialog = null;
                    break;
            }
        }

        /// <summary>Close key board.</summary>
        public static void closeKeyBoard()
        {
            bool visible = TouchScreenKeyboard.visible;
            if (visible)
            {
                TField.kb.active = false;
                TField.kb = null;
            }
        }

        /// <summary>Start ok dlg.</summary>
        /// <param name="info">Info.</param>
        /// <param name="str">Text.</param>
        /// <param name="show">Show.</param>
        /// <remarks>Locals: flag: Branch/result flag for the current operation.</remarks>
        public static void startOKDlg(string info, string str, bool show)
        {
            bool flag = GameCanvas.currentScreen == GameScr.instance;
            if (flag)
            {
                GameScr.gI().showAlert(info, str, show);
            }
            else
            {
                closeKeyBoard();
                GameCanvas.msgdlg.setInfo(info, null, new Command(mResources.OK, instance, 8882, null), null);
                GameCanvas.currentDialog = GameCanvas.msgdlg;
            }
        }
    }
}

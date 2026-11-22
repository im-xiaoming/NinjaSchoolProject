public abstract class IPaint
{
    /// <summary>Retain the paint default bg compatibility hook without performing work.</summary>
    /// <param name="g">Retained for call compatibility; not read by this implementation.</param>
    public abstract void paintDefaultBg(mGraphics g);
    /// <summary>Retain the paintfill default bg compatibility hook without performing work.</summary>
    /// <param name="g">Retained for call compatibility; not read by this implementation.</param>
    public abstract void paintfillDefaultBg(mGraphics g);
    /// <summary>Retain the repaint circle bg compatibility hook without performing work.</summary>
    public abstract void repaintCircleBg();
    /// <summary>Retain the paint solid bg compatibility hook without performing work.</summary>
    /// <param name="g">Retained for call compatibility; not read by this implementation.</param>
    public abstract void paintSolidBg(mGraphics g);
    /// <summary>Retain the paint default popup compatibility hook without performing work.</summary>
    /// <param name="g">Retained for call compatibility; not read by this implementation.</param>
    /// <param name="x">Retained for call compatibility; not read by this implementation.</param>
    /// <param name="y">Retained for call compatibility; not read by this implementation.</param>
    /// <param name="w">Retained for call compatibility; not read by this implementation.</param>
    /// <param name="h">Retained for call compatibility; not read by this implementation.</param>
    public abstract void paintDefaultPopup(mGraphics g, int x, int y, int w, int h);
    /// <summary>Retain the paint white popup compatibility hook without performing work.</summary>
    /// <param name="g">Retained for call compatibility; not read by this implementation.</param>
    /// <param name="y">Retained for call compatibility; not read by this implementation.</param>
    /// <param name="x">Retained for call compatibility; not read by this implementation.</param>
    /// <param name="width">Retained for call compatibility; not read by this implementation.</param>
    /// <param name="height">Retained for call compatibility; not read by this implementation.</param>
    public abstract void paintWhitePopup(mGraphics g, int y, int x, int width, int height);
    /// <summary>Retain the paint default popup h compatibility hook without performing work.</summary>
    /// <param name="g">Retained for call compatibility; not read by this implementation.</param>
    /// <param name="h">Retained for call compatibility; not read by this implementation.</param>
    public abstract void paintDefaultPopupH(mGraphics g, int h);
    /// <summary>Retain the paint command bar compatibility hook without performing work.</summary>
    /// <param name="g">Retained for call compatibility; not read by this implementation.</param>
    /// <param name="left">Retained for call compatibility; not read by this implementation.</param>
    /// <param name="center">Retained for call compatibility; not read by this implementation.</param>
    /// <param name="right">Retained for call compatibility; not read by this implementation.</param>
    public abstract void paintCmdBar(mGraphics g, Command left, Command center, Command right);
    /// <summary>Retain the paint select compatibility hook without performing work.</summary>
    /// <param name="g">Retained for call compatibility; not read by this implementation.</param>
    /// <param name="x">Retained for call compatibility; not read by this implementation.</param>
    /// <param name="y">Retained for call compatibility; not read by this implementation.</param>
    /// <param name="w">Retained for call compatibility; not read by this implementation.</param>
    /// <param name="h">Retained for call compatibility; not read by this implementation.</param>
    public abstract void paintSelect(mGraphics g, int x, int y, int w, int h);
    /// <summary>Retain the paint logo compatibility hook without performing work.</summary>
    /// <param name="g">Retained for call compatibility; not read by this implementation.</param>
    /// <param name="x">Retained for call compatibility; not read by this implementation.</param>
    /// <param name="y">Retained for call compatibility; not read by this implementation.</param>
    public abstract void paintLogo(mGraphics g, int x, int y);
    /// <summary>Retain the paint hotline compatibility hook without performing work.</summary>
    /// <param name="g">Retained for call compatibility; not read by this implementation.</param>
    /// <param name="num">Retained for call compatibility; not read by this implementation.</param>
    public abstract void paintHotline(mGraphics g, string num);
    /// <summary>Retain the paint input tf compatibility hook without performing work.</summary>
    /// <param name="g">Retained for call compatibility; not read by this implementation.</param>
    /// <param name="isz">Retained for call compatibility; not read by this implementation.</param>
    /// <param name="x">Retained for call compatibility; not read by this implementation.</param>
    /// <param name="y">Retained for call compatibility; not read by this implementation.</param>
    /// <param name="w">Retained for call compatibility; not read by this implementation.</param>
    /// <param name="h">Retained for call compatibility; not read by this implementation.</param>
    /// <param name="xText">Retained for call compatibility; not read by this implementation.</param>
    /// <param name="yText">Retained for call compatibility; not read by this implementation.</param>
    /// <param name="text">Retained for call compatibility; not read by this implementation.</param>
    public abstract void paintInputTf(mGraphics g, bool isz, int x, int y, int w, int h, int xText, int yText, string text);
    /// <summary>Retain the paint tab soft compatibility hook without performing work.</summary>
    /// <param name="g">Retained for call compatibility; not read by this implementation.</param>
    public abstract void paintTabSoft(mGraphics g);
    /// <summary>Retain the paint back menu compatibility hook without performing work.</summary>
    /// <param name="g">Retained for call compatibility; not read by this implementation.</param>
    /// <param name="x">Retained for call compatibility; not read by this implementation.</param>
    /// <param name="y">Retained for call compatibility; not read by this implementation.</param>
    /// <param name="w">Retained for call compatibility; not read by this implementation.</param>
    /// <param name="h">Retained for call compatibility; not read by this implementation.</param>
    /// <param name="isz">Retained for call compatibility; not read by this implementation.</param>
    public abstract void paintBackMenu(mGraphics g, int x, int y, int w, int h, bool isz);
    /// <summary>Retain the paint message bg compatibility hook without performing work.</summary>
    /// <param name="g">Retained for call compatibility; not read by this implementation.</param>
    /// <param name="x">Retained for call compatibility; not read by this implementation.</param>
    /// <param name="y">Retained for call compatibility; not read by this implementation.</param>
    /// <param name="w">Retained for call compatibility; not read by this implementation.</param>
    /// <param name="h">Retained for call compatibility; not read by this implementation.</param>
    /// <param name="title">Retained for call compatibility; not read by this implementation.</param>
    /// <param name="subTitle">Retained for call compatibility; not read by this implementation.</param>
    /// <param name="check">Retained for call compatibility; not read by this implementation.</param>
    public abstract void paintMsgBG(mGraphics g, int x, int y, int w, int h, string title, string subTitle, string check);
    /// <summary>Retain the paint default screen list compatibility hook without performing work.</summary>
    /// <param name="g">Retained for call compatibility; not read by this implementation.</param>
    /// <param name="title">Retained for call compatibility; not read by this implementation.</param>
    /// <param name="subTitle">Retained for call compatibility; not read by this implementation.</param>
    /// <param name="check">Retained for call compatibility; not read by this implementation.</param>
    public abstract void paintDefaultScrList(mGraphics g, string title, string subTitle, string check);
    /// <summary>Retain the paint check compatibility hook without performing work.</summary>
    /// <param name="g">Retained for call compatibility; not read by this implementation.</param>
    /// <param name="x">Retained for call compatibility; not read by this implementation.</param>
    /// <param name="y">Retained for call compatibility; not read by this implementation.</param>
    /// <param name="index">Retained for call compatibility; not read by this implementation.</param>
    public abstract void paintCheck(mGraphics g, int x, int y, int index);
    /// <summary>Retain the paint image message compatibility hook without performing work.</summary>
    /// <param name="g">Retained for call compatibility; not read by this implementation.</param>
    /// <param name="x">Retained for call compatibility; not read by this implementation.</param>
    /// <param name="y">Retained for call compatibility; not read by this implementation.</param>
    /// <param name="index">Retained for call compatibility; not read by this implementation.</param>
    public abstract void paintImgMsg(mGraphics g, int x, int y, int index);
    /// <summary>Retain the paint title board compatibility hook without performing work.</summary>
    /// <param name="g">Retained for call compatibility; not read by this implementation.</param>
    /// <param name="roomID">Retained for call compatibility; not read by this implementation.</param>
    public abstract void paintTitleBoard(mGraphics g, int roomID);
    /// <summary>Retain the paint check pass compatibility hook without performing work.</summary>
    /// <param name="g">Retained for call compatibility; not read by this implementation.</param>
    /// <param name="x">Retained for call compatibility; not read by this implementation.</param>
    /// <param name="y">Retained for call compatibility; not read by this implementation.</param>
    /// <param name="check">Retained for call compatibility; not read by this implementation.</param>
    /// <param name="focus">Retained for call compatibility; not read by this implementation.</param>
    public abstract void paintCheckPass(mGraphics g, int x, int y, bool check, bool focus);
    /// <summary>Retain the paint input dlg compatibility hook without performing work.</summary>
    /// <param name="g">Retained for call compatibility; not read by this implementation.</param>
    /// <param name="x">Retained for call compatibility; not read by this implementation.</param>
    /// <param name="y">Retained for call compatibility; not read by this implementation.</param>
    /// <param name="w">Retained for call compatibility; not read by this implementation.</param>
    /// <param name="h">Retained for call compatibility; not read by this implementation.</param>
    /// <param name="str">Retained for call compatibility; not read by this implementation.</param>
    public abstract void paintInputDlg(mGraphics g, int x, int y, int w, int h, string[] str);
    /// <summary>Retain the paint icon main menu compatibility hook without performing work.</summary>
    /// <param name="g">Retained for call compatibility; not read by this implementation.</param>
    /// <param name="x">Retained for call compatibility; not read by this implementation.</param>
    /// <param name="y">Retained for call compatibility; not read by this implementation.</param>
    /// <param name="isz">Retained for call compatibility; not read by this implementation.</param>
    /// <param name="isSe">Retained for call compatibility; not read by this implementation.</param>
    /// <param name="i">Retained for call compatibility; not read by this implementation.</param>
    /// <param name="wStr">Retained for call compatibility; not read by this implementation.</param>
    public abstract void paintIconMainMenu(mGraphics g, int x, int y, bool isz, bool isSe, int i, int wStr);
    /// <summary>Retain the paint line room compatibility hook without performing work.</summary>
    /// <param name="g">Retained for call compatibility; not read by this implementation.</param>
    /// <param name="x">Retained for call compatibility; not read by this implementation.</param>
    /// <param name="y">Retained for call compatibility; not read by this implementation.</param>
    /// <param name="xTo">Retained for call compatibility; not read by this implementation.</param>
    /// <param name="yTo">Retained for call compatibility; not read by this implementation.</param>
    public abstract void paintLineRoom(mGraphics g, int x, int y, int xTo, int yTo);
    /// <summary>Retain the paint cell containt compatibility hook without performing work.</summary>
    /// <param name="g">Retained for call compatibility; not read by this implementation.</param>
    /// <param name="x">Retained for call compatibility; not read by this implementation.</param>
    /// <param name="y">Retained for call compatibility; not read by this implementation.</param>
    /// <param name="w">Retained for call compatibility; not read by this implementation.</param>
    /// <param name="h">Retained for call compatibility; not read by this implementation.</param>
    /// <param name="isz">Retained for call compatibility; not read by this implementation.</param>
    public abstract void paintCellContaint(mGraphics g, int x, int y, int w, int h, bool isz);
    /// <summary>Retain the paint scroll compatibility hook without performing work.</summary>
    /// <param name="g">Retained for call compatibility; not read by this implementation.</param>
    /// <param name="x">Retained for call compatibility; not read by this implementation.</param>
    /// <param name="y">Retained for call compatibility; not read by this implementation.</param>
    /// <param name="h">Retained for call compatibility; not read by this implementation.</param>
    public abstract void paintScroll(mGraphics g, int x, int y, int h);
    /// <summary>Retain the get color message compatibility hook without performing work.</summary>
    public abstract int[] getColorMsg();
    /// <summary>Retain the paint logo compatibility hook without performing work.</summary>
    /// <param name="g">Retained for call compatibility; not read by this implementation.</param>
    public abstract void paintLogo(mGraphics g);
    /// <summary>Retain the paint text login compatibility hook without performing work.</summary>
    /// <param name="g">Retained for call compatibility; not read by this implementation.</param>
    /// <param name="isRes">Retained for call compatibility; not read by this implementation.</param>
    public abstract void paintTextLogin(mGraphics g, bool isRes);
    /// <summary>Retain the paint sellect board compatibility hook without performing work.</summary>
    /// <param name="g">Retained for call compatibility; not read by this implementation.</param>
    /// <param name="x">Retained for call compatibility; not read by this implementation.</param>
    /// <param name="y">Retained for call compatibility; not read by this implementation.</param>
    /// <param name="w">Retained for call compatibility; not read by this implementation.</param>
    /// <param name="h">Retained for call compatibility; not read by this implementation.</param>
    public abstract void paintSellectBoard(mGraphics g, int x, int y, int w, int h);
    /// <summary>Retain the is register using wap compatibility hook without performing work.</summary>
    public abstract int isRegisterUsingWAP();
    /// <summary>Retain the get card compatibility hook without performing work.</summary>
    public abstract string getCard();
    /// <summary>Retain the paint sellected shop compatibility hook without performing work.</summary>
    /// <param name="g">Retained for call compatibility; not read by this implementation.</param>
    /// <param name="x">Retained for call compatibility; not read by this implementation.</param>
    /// <param name="y">Retained for call compatibility; not read by this implementation.</param>
    /// <param name="w">Retained for call compatibility; not read by this implementation.</param>
    /// <param name="h">Retained for call compatibility; not read by this implementation.</param>
    public abstract void paintSellectedShop(mGraphics g, int x, int y, int w, int h);
    /// <summary>Retain the do select compatibility hook without performing work.</summary>
    /// <param name="focus">Retained for call compatibility; not read by this implementation.</param>
    public abstract void doSelect(int focus);
}

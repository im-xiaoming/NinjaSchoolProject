public class FrameEff
{
    public MyVector listPartTop = new MyVector();
    public MyVector listPartBottom = new MyVector();
    public sbyte xShadow;
    public sbyte yShadow;
    /// <summary>Initialize frame effect state and its supplied dependencies.</summary>
    /// <param name="listtop">Listtop.</param>
    /// <param name="listbottom">Listbottom.</param>
    public FrameEff(MyVector listtop, MyVector listbottom)
    {
        listPartTop = listtop;
        listPartBottom = listbottom;
    }

}

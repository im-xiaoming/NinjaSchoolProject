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

    /// <summary>Return list part paint.</summary>
    /// <remarks>Locals: i: Loop/collection index; j: Nested loop/collection index.</remarks>
    public MyVector getListPartPaint()
    {
        MyVector myVector = new MyVector();
        for (int i = 0; i < listPartBottom.size(); i++)
        {
            myVector.addElement(listPartBottom.elementAt(i));
        }

        for (int j = 0; j < listPartTop.size(); j++)
        {
            myVector.addElement(listPartTop.elementAt(j));
        }

        return myVector;
    }
}

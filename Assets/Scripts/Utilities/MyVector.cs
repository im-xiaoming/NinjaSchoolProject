using System.Collections;

public class MyVector
{
    // a: First operand or current actor.
    private ArrayList a;
    /// <summary>Initialize my vector state and its supplied dependencies.</summary>
    public MyVector()
    {
        a = new ArrayList();
    }

    /// <summary>Initialize my vector state and its supplied dependencies.</summary>
    /// <param name="a">First operand or current actor.</param>
    public MyVector(ArrayList a)
    {
        this.a = a;
    }

    /// <summary>Add element.</summary>
    /// <param name="o">Object being inspected or converted.</param>
    public void addElement(object o)
    {
        a.Add(o);
    }

    /// <summary>Compute contains.</summary>
    /// <param name="o">Object being inspected or converted.</param>
    public bool contains(object o)
    {
        if (a.Contains(o))
        {
            return true;
        }

        return false;
    }

    /// <summary>Compute size.</summary>
    public int size()
    {
        if (a == null)
        {
            return 0;
        }

        return a.Count;
    }

    /// <summary>Compute element at.</summary>
    /// <param name="index">Index.</param>
    public object elementAt(int index)
    {
        if (index > -1 && index < a.Count)
        {
            return a[index];
        }

        return null;
    }

    /// <summary>Set my vector.</summary>
    /// <param name="index">Index.</param>
    /// <param name="obj">Obj.</param>
    public void set(int index, object obj)
    {
        if (index > -1 && index < a.Count)
        {
            a[index] = obj;
        }
    }

    /// <summary>Set element at.</summary>
    /// <param name="obj">Obj.</param>
    /// <param name="index">Index.</param>
    public void setElementAt(object obj, int index)
    {
        if (index > -1 && index < a.Count)
        {
            a[index] = obj;
        }
    }

    /// <summary>Compute index of.</summary>
    /// <param name="o">Object being inspected or converted.</param>
    public int indexOf(object o)
    {
        return a.IndexOf(o);
    }

    /// <summary>Remove element at.</summary>
    /// <param name="index">Index.</param>
    public void removeElementAt(int index)
    {
        if (index > -1 && index < a.Count)
        {
            a.RemoveAt(index);
        }
    }

    /// <summary>Remove element.</summary>
    /// <param name="o">Object being inspected or converted.</param>
    public void removeElement(object o)
    {
        a.Remove(o);
    }

    /// <summary>Remove all elements.</summary>
    public void removeAllElements()
    {
        a.Clear();
    }

    /// <summary>Perform insert element at.</summary>
    /// <param name="o">Object being inspected or converted.</param>
    /// <param name="i">Loop/collection index.</param>
    public void insertElementAt(object o, int i)
    {
        a.Insert(i, o);
    }

    /// <summary>Compute first element.</summary>
    public object firstElement()
    {
        return a[0];
    }

    /// <summary>Compute last element.</summary>
    public object lastElement()
    {
        return a[a.Count - 1];
    }
}

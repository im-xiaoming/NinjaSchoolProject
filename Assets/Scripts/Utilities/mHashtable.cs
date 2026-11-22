using System.Collections;

public class mHashtable
{
    // h: Height or vertical span.
    public Hashtable h = new Hashtable();
    /// <summary>Return m hashtable.</summary>
    /// <param name="k">Key or nested iteration index.</param>
    public object get(object k)
    {
        return h[k];
    }

    /// <summary>Compute conskey.</summary>
    /// <param name="key">Key.</param>
    public object conskey(string key)
    {
        return h[key];
    }

    /// <summary>Clear m hashtable.</summary>
    public void clear()
    {
        h.Clear();
    }

    /// <summary>Return enumerator.</summary>
    public IDictionaryEnumerator GetEnumerator()
    {
        return h.GetEnumerator();
    }

    /// <summary>Compute size.</summary>
    public int size()
    {
        return h.Count;
    }

    /// <summary>Perform put.</summary>
    /// <param name="k">Key or nested iteration index.</param>
    /// <param name="v">Value being inspected or transformed.</param>
    public void put(object k, object v)
    {
        if (h.ContainsKey(k))
        {
            h.Remove(k);
        }

        h.Add(k, v);
    }

    /// <summary>Remove m hashtable.</summary>
    /// <param name="k">Key or nested iteration index.</param>
    public void remove(object k)
    {
        h.Remove(k);
    }

    /// <summary>Remove m hashtable.</summary>
    /// <param name="key">Key.</param>
    public void Remove(string key)
    {
        h.Remove(key);
    }
}

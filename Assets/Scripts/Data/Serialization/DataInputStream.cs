using System.Threading;
using UnityEngine;

public class DataInputStream
{
    // r: Result being assembled or returned.
    public myReader r;
    private const int INTERVAL = 5;
    private const int MAXTIME = 500;
    public static DataInputStream istemp;
    private static int status;
    private static string filenametemp;
    /// <summary>Initialize data input stream state and its supplied dependencies.</summary>
    /// <param name="filename">Filename.</param>
    public DataInputStream(string filename)
    {
        TextAsset textAsset = (TextAsset)Resources.Load(filename, typeof(TextAsset));
        r = new myReader(ArrayCast.cast(textAsset.bytes));
    }

    /// <summary>Initialize data input stream state and its supplied dependencies.</summary>
    /// <param name="data">Data.</param>
    public DataInputStream(sbyte[] data)
    {
        r = new myReader(data);
    }

    /// <summary>Advance this object&#x27;s state for the current client logic tick.</summary>
    public static void update()
    {
        if (status == 2)
        {
            status = 1;
            istemp = __getResourceAsStream(filenametemp);
            status = 0;
        }
    }

    /// <summary>Return resource as stream.</summary>
    /// <param name="filename">Filename.</param>
    public static DataInputStream getResourceAsStream(string filename)
    {
        return __getResourceAsStream(filename);
    }

    /// <summary>Return resource as stream.</summary>
    /// <param name="filename">Filename.</param>
    /// <remarks>Locals: i: Loop/collection index; j: Nested loop/collection index.</remarks>
    private static DataInputStream _getResourceAsStream(string filename)
    {
        if (status != 0)
        {
            for (int i = 0; i < 500; i++)
            {
                Thread.Sleep(5);
                if (status == 0)
                {
                    break;
                }
            }

            if (status != 0)
            {
                Debug.LogError("CANNOT GET INPUTSTREAM " + filename + " WHEN GETTING " + filenametemp);
                return null;
            }
        }

        istemp = null;
        filenametemp = filename;
        status = 2;
        int j;
        for (j = 0; j < 500; j++)
        {
            Thread.Sleep(5);
            if (status == 0)
            {
                break;
            }
        }

        if (j == 500)
        {
            Debug.LogError("TOO LONG FOR CREATE INPUTSTREAM " + filename);
            status = 0;
            return null;
        }

        return istemp;
    }

    /// <summary>Return resource as stream.</summary>
    /// <param name="filename">Filename.</param>
    private static DataInputStream __getResourceAsStream(string filename)
    {
        return new DataInputStream(filename);
    }

    /// <summary>Read short.</summary>
    public short readShort()
    {
        return r.readShort();
    }

    /// <summary>Read int.</summary>
    public int readInt()
    {
        return r.readInt();
    }

    /// <summary>Read data input stream.</summary>
    public int read()
    {
        return r.readUnsignedByte();
    }

    /// <summary>Read data input stream.</summary>
    /// <param name="data">Data.</param>
    public void read(ref sbyte[] data)
    {
        r.read(ref data);
    }

    /// <summary>Close data input stream.</summary>
    public void close()
    {
        r.Close();
    }

    /// <summary>Close data input stream.</summary>
    public void Close()
    {
        r.Close();
    }

    /// <summary>Read utf.</summary>
    public string readUTF()
    {
        return r.readUTF();
    }

    /// <summary>Read byte.</summary>
    public sbyte readByte()
    {
        return r.readByte();
    }

    /// <summary>Read long.</summary>
    public long readLong()
    {
        return r.readLong();
    }

    /// <summary>Read boolean.</summary>
    public bool readBoolean()
    {
        return r.readBoolean();
    }

    /// <summary>Read unsigned byte.</summary>
    public int readUnsignedByte()
    {
        return (byte)r.readByte();
    }

    /// <summary>Read unsigned short.</summary>
    public int readUnsignedShort()
    {
        return r.readUnsignedShort();
    }

    /// <summary>Read fully.</summary>
    /// <param name="data">Data.</param>
    public void readFully(ref sbyte[] data)
    {
        r.read(ref data);
    }

    /// <summary>Compute available.</summary>
    public int available()
    {
        return r.available();
    }
}

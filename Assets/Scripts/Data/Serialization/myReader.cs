using System.Text;
using UnityEngine;

public class myReader
{
    public sbyte[] buffer;
    private int posRead;
    private int posMark;
    private static string fileName;
    private static int status;
    /// <summary>Initialize my reader state and its supplied dependencies.</summary>
    public myReader()
    {
    }

    /// <summary>Initialize my reader state and its supplied dependencies.</summary>
    /// <param name="data">Data.</param>
    public myReader(sbyte[] data)
    {
        buffer = data;
    }

    /// <summary>Initialize my reader state and its supplied dependencies.</summary>
    /// <param name="filename">Filename.</param>
    public myReader(string filename)
    {
        TextAsset textAsset = (TextAsset)Resources.Load(filename, typeof(TextAsset));
        buffer = mSystem.convertToSbyte(textAsset.bytes);
    }

    /// <summary>Read signed byte.</summary>
    public sbyte readSByte()
    {
        if (posRead < buffer.Length)
        {
            return buffer[posRead++];
        }

        posRead = buffer.Length;
        return 0;
    }

    /// <summary>Read signed byte.</summary>
    public sbyte readsbyte()
    {
        return readSByte();
    }

    /// <summary>Read byte.</summary>
    public sbyte readByte()
    {
        return readSByte();
    }

    /// <summary>Mark my reader.</summary>
    /// <param name="readlimit">Retained for call compatibility; not read by this implementation.</param>
    public void mark(int readlimit)
    {
        posMark = posRead;
    }

    /// <summary>Reset my reader.</summary>
    public void reset()
    {
        posRead = posMark;
    }

    /// <summary>Read unsigned byte.</summary>
    public byte readUnsignedByte()
    {
        return convertSbyteToByte(readSByte());
    }

    /// <summary>Read short.</summary>
    /// <remarks>Locals: num: Numeric intermediate used by this operation; the legacy name is retained; i: Loop/collection index.</remarks>
    public short readShort()
    {
        short num = 0;
        for (int i = 0; i < 2; i++)
        {
            num = (short)(num << 8);
            num = (short)(num | (short)(0xFF & buffer[posRead++]));
        }

        return num;
    }

    /// <summary>Read unsigned short.</summary>
    /// <remarks>Locals: num: Numeric intermediate used by this operation; the legacy name is retained; i: Loop/collection index.</remarks>
    public ushort readUnsignedShort()
    {
        ushort num = 0;
        for (int i = 0; i < 2; i++)
        {
            num = (ushort)(num << 8);
            num = (ushort)(num | (ushort)(0xFFu & (uint)buffer[posRead++]));
        }

        return num;
    }

    /// <summary>Read int.</summary>
    /// <remarks>Locals: num: Numeric intermediate used by this operation; the legacy name is retained; i: Loop/collection index.</remarks>
    public int readInt()
    {
        int num = 0;
        for (int i = 0; i < 4; i++)
        {
            num <<= 8;
            num |= 0xFF & buffer[posRead++];
        }

        return num;
    }

    /// <summary>Read long.</summary>
    /// <remarks>Locals: num: Numeric intermediate used by this operation; the legacy name is retained; i: Loop/collection index.</remarks>
    public long readLong()
    {
        long num = 0L;
        for (int i = 0; i < 8; i++)
        {
            num <<= 8;
            num |= 0xFF & buffer[posRead++];
        }

        return num;
    }

    /// <summary>Read bool.</summary>
    public bool readBool()
    {
        return readSByte() > 0;
    }

    /// <summary>Read boolean.</summary>
    public bool readBoolean()
    {
        return readSByte() > 0;
    }

    /// <summary>Read string.</summary>
    /// <remarks>Locals: num: Field decoded from the incoming binary payload; array: Entry selected from the source collection; i: Loop/collection index.</remarks>
    public string readString()
    {
        short num = readShort();
        byte[] array = new byte[num];
        for (int i = 0; i < num; i++)
        {
            array[i] = convertSbyteToByte(readSByte());
        }

        UTF8Encoding uTF8Encoding = new UTF8Encoding();
        return uTF8Encoding.GetString(array);
    }

    /// <summary>Read string utf.</summary>
    /// <remarks>Locals: num: Field decoded from the incoming binary payload; array: Entry selected from the source collection; i: Loop/collection index.</remarks>
    public string readStringUTF()
    {
        short num = readShort();
        byte[] array = new byte[num];
        for (int i = 0; i < num; i++)
        {
            array[i] = convertSbyteToByte(readSByte());
        }

        UTF8Encoding uTF8Encoding = new UTF8Encoding();
        return uTF8Encoding.GetString(array);
    }

    /// <summary>Read utf.</summary>
    public string readUTF()
    {
        return readStringUTF();
    }

    /// <summary>Read my reader.</summary>
    public int read()
    {
        if (posRead < buffer.Length)
        {
            return readSByte();
        }

        return -1;
    }

    /// <summary>Read my reader.</summary>
    /// <param name="data">Data.</param>
    /// <remarks>Locals: num: Numeric intermediate used by this operation; the legacy name is retained; i: Loop/collection index.</remarks>
    public int read(ref sbyte[] data)
    {
        if (data == null)
        {
            return 0;
        }

        int num = 0;
        for (int i = 0; i < data.Length; i++)
        {
            data[i] = readSByte();
            if (posRead > buffer.Length)
            {
                return -1;
            }

            num++;
        }

        return num;
    }

    /// <summary>Compute readz.</summary>
    /// <param name="data">Data.</param>
    /// <remarks>Locals: num: Numeric intermediate used by this operation; the legacy name is retained; i: Loop/collection index.</remarks>
    public int readz(sbyte[] data)
    {
        if (data == null)
        {
            return 0;
        }

        int num = 0;
        for (int i = 0; i < data.Length; i++)
        {
            data[i] = readSByte();
            if (posRead > buffer.Length)
            {
                return -1;
            }

            num++;
        }

        return num;
    }

    /// <summary>Read fully.</summary>
    /// <param name="data">Data.</param>
    /// <remarks>Locals: i: Loop/collection index.</remarks>
    public void readFully(ref sbyte[] data)
    {
        if (data != null && data.Length + posRead <= buffer.Length)
        {
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = readSByte();
            }
        }
    }

    /// <summary>Compute available.</summary>
    public int available()
    {
        return buffer.Length - posRead;
    }

    /// <summary>Copy signed protocol bytes into an unsigned byte array without changing their bit patterns.</summary>
    /// <param name="var">Compatibility argument retained under its original legacy name.</param>
    public static byte convertSbyteToByte(sbyte var)
    {
        if (var > 0)
        {
            return (byte)var;
        }

        return (byte)(var + 256);
    }

    /// <summary>Copy signed protocol bytes into an unsigned byte array without changing their bit patterns.</summary>
    /// <param name="var">Compatibility argument retained under its original legacy name.</param>
    /// <remarks>Locals: array: Number of entries in the source collection; i: Loop/collection index.</remarks>
    public static byte[] convertSbyteToByte(sbyte[] var)
    {
        byte[] array = new byte[var.Length];
        for (int i = 0; i < var.Length; i++)
        {
            if (var[i] > 0)
            {
                array[i] = (byte)var[i];
            }
            else
            {
                array[i] = (byte)(var[i] + 256);
            }
        }

        return array;
    }

    /// <summary>Close my reader.</summary>
    public void Close()
    {
        buffer = null;
    }

    /// <summary>Close my reader.</summary>
    public void close()
    {
        buffer = null;
    }
}

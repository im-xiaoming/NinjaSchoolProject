using System.IO;
using System.Text;
using UnityEngine;

public class myWriter
{
    public sbyte[] buffer = new sbyte[2048];
    private int posWrite;
    private int lenght = 2048;
    /// <summary>Write signed byte.</summary>
    /// <param name="value">Value.</param>
    public void writeSByte(sbyte value)
    {
        checkLenght(0);
        buffer[posWrite++] = value;
    }

    /// <summary>Write signed byte uncheck.</summary>
    /// <param name="value">Value.</param>
    public void writeSByteUncheck(sbyte value)
    {
        buffer[posWrite++] = value;
    }

    /// <summary>Write byte.</summary>
    /// <param name="value">Value.</param>
    public void writeByte(sbyte value)
    {
        writeSByte(value);
    }

    /// <summary>Write byte.</summary>
    /// <param name="value">Value.</param>
    public void writeByte(int value)
    {
        writeSByte((sbyte)value);
    }

    /// <summary>Write unsigned byte.</summary>
    /// <param name="value">Value.</param>
    public void writeUnsignedByte(byte value)
    {
        writeSByte((sbyte)value);
    }

    /// <summary>Write unsigned byte.</summary>
    /// <param name="value">Value.</param>
    /// <remarks>Locals: i: Loop/collection index.</remarks>
    public void writeUnsignedByte(byte[] value)
    {
        checkLenght(value.Length);
        for (int i = 0; i < value.Length; i++)
        {
            writeSByteUncheck((sbyte)value[i]);
        }
    }

    /// <summary>Write signed byte.</summary>
    /// <param name="value">Value.</param>
    /// <remarks>Locals: i: Loop/collection index.</remarks>
    public void writeSByte(sbyte[] value)
    {
        checkLenght(value.Length);
        for (int i = 0; i < value.Length; i++)
        {
            writeSByteUncheck(value[i]);
        }
    }

    /// <summary>Write short.</summary>
    /// <param name="value">Value.</param>
    /// <remarks>Locals: num: Numeric intermediate used by this operation; the legacy name is retained.</remarks>
    public void writeShort(short value)
    {
        checkLenght(2);
        for (int num = 1; num >= 0; num--)
        {
            writeSByteUncheck((sbyte)(value >> num * 8));
        }
    }

    /// <summary>Write short.</summary>
    /// <param name="value">Value.</param>
    /// <remarks>Locals: num: Numeric intermediate used by this operation; the legacy name is retained; num2: Numeric intermediate used by this operation; the legacy name is retained.</remarks>
    public void writeShort(int value)
    {
        checkLenght(2);
        short num = (short)value;
        for (int num2 = 1; num2 >= 0; num2--)
        {
            writeSByteUncheck((sbyte)(num >> num2 * 8));
        }
    }

    /// <summary>Write unsigned short.</summary>
    /// <param name="value">Value.</param>
    /// <remarks>Locals: num: Numeric intermediate used by this operation; the legacy name is retained.</remarks>
    public void writeUnsignedShort(ushort value)
    {
        checkLenght(2);
        for (int num = 1; num >= 0; num--)
        {
            writeSByteUncheck((sbyte)(value >> num * 8));
        }
    }

    /// <summary>Write int.</summary>
    /// <param name="value">Value.</param>
    /// <remarks>Locals: num: Numeric intermediate used by this operation; the legacy name is retained.</remarks>
    public void writeInt(int value)
    {
        checkLenght(4);
        for (int num = 3; num >= 0; num--)
        {
            writeSByteUncheck((sbyte)(value >> num * 8));
        }
    }

    /// <summary>Write long.</summary>
    /// <param name="value">Value.</param>
    /// <remarks>Locals: num: Numeric intermediate used by this operation; the legacy name is retained.</remarks>
    public void writeLong(long value)
    {
        checkLenght(8);
        for (int num = 7; num >= 0; num--)
        {
            writeSByteUncheck((sbyte)(value >> num * 8));
        }
    }

    /// <summary>Write boolean.</summary>
    /// <param name="value">Value.</param>
    public void writeBoolean(bool value)
    {
        writeSByte((sbyte)(value ? 1 : 0));
    }

    /// <summary>Write bool.</summary>
    /// <param name="value">Value.</param>
    public void writeBool(bool value)
    {
        writeSByte((sbyte)(value ? 1 : 0));
    }

    /// <summary>Write string.</summary>
    /// <param name="value">Value.</param>
    /// <remarks>Locals: array: Working array for this operation; i: Loop/collection index.</remarks>
    public void writeString(string value)
    {
        char[] array = value.ToCharArray();
        writeShort((short)array.Length);
        checkLenght(array.Length);
        for (int i = 0; i < array.Length; i++)
        {
            writeSByteUncheck((sbyte)array[i]);
        }
    }

    /// <summary>Write utf.</summary>
    /// <param name="value">Value.</param>
    /// <remarks>Locals: array: Working array for this operation; i: Loop/collection index.</remarks>
    public void writeUTF(string value)
    {
        Encoding unicode = Encoding.Unicode;
        Encoding encoding = Encoding.GetEncoding(65001);
        byte[] bytes = unicode.GetBytes(value);
        byte[] array = Encoding.Convert(unicode, encoding, bytes);
        writeShort((short)array.Length);
        checkLenght(array.Length);
        for (int i = 0; i < array.Length; i++)
        {
            sbyte value2 = (sbyte)array[i];
            writeSByteUncheck(value2);
        }
    }

    /// <summary>Write my writer.</summary>
    /// <param name="value">Value.</param>
    public void write(sbyte value)
    {
        writeSByte(value);
    }

    /// <summary>Write my writer.</summary>
    /// <param name="value">Value.</param>
    public void write(sbyte[] value)
    {
        writeSByte(value);
    }

    /// <summary>Return data.</summary>
    /// <remarks>Locals: array: Entry selected from the source collection; i: Loop/collection index.</remarks>
    public sbyte[] getData()
    {
        if (posWrite <= 0)
        {
            return null;
        }

        sbyte[] array = new sbyte[posWrite];
        for (int i = 0; i < posWrite; i++)
        {
            array[i] = buffer[i];
        }

        return array;
    }

    /// <summary>Check lenght.</summary>
    /// <param name="ltemp">Ltemp.</param>
    /// <remarks>Locals: array: Entry selected from the source collection; i: Loop/collection index.</remarks>
    public void checkLenght(int ltemp)
    {
        if (posWrite + ltemp > lenght)
        {
            sbyte[] array = new sbyte[lenght + 1024 + ltemp];
            for (int i = 0; i < lenght; i++)
            {
                array[i] = buffer[i];
            }

            buffer = null;
            buffer = array;
            lenght += 1024 + ltemp;
        }
    }

    /// <summary>Convert string.</summary>
    /// <param name="args">Args.</param>
    private static void convertString(string[] args)
    {
        string path = args[0];
        string path2 = args[1];
        using (StreamReader input = new StreamReader(path, Encoding.Unicode))
        {
            using (StreamWriter output = new StreamWriter(path2, false, Encoding.UTF8))
            {
                CopyContents(input, output);
            }
        }
    }

    /// <summary>Copy contents.</summary>
    /// <param name="input">Input.</param>
    /// <param name="output">Output.</param>
    /// <remarks>Locals: array: Entry selected from the source collection.</remarks>
    private static void CopyContents(TextReader input, TextWriter output)
    {
        char[] array = new char[8192];
        int count;
        while ((count = input.Read(array, 0, array.Length)) != 0)
        {
            output.Write(array, 0, count);
        }

        output.Flush();
        string message = output.ToString();
        Debug.Log(message);
    }

    /// <summary>Copy signed protocol bytes into an unsigned byte array without changing their bit patterns.</summary>
    /// <param name="var">Compatibility argument retained under its original legacy name.</param>
    public byte convertSbyteToByte(sbyte var)
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
    public byte[] convertSbyteToByte(sbyte[] var)
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

    /// <summary>Close my writer.</summary>
    public void Close()
    {
        buffer = null;
    }

    /// <summary>Close my writer.</summary>
    public void close()
    {
        buffer = null;
    }
}

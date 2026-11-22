public class ArrayCast
{
    /// <summary>Compute cast.</summary>
    /// <param name="data">Data.</param>
    /// <remarks>Locals: array: Number of entries in the source collection; i: Loop/collection index.</remarks>
    public static sbyte[] cast(byte[] data)
    {
        sbyte[] array = new sbyte[data.Length];
        for (int i = 0; i < array.Length; i++)
        {
            array[i] = (sbyte)data[i];
        }

        return array;
    }

    /// <summary>Compute cast.</summary>
    /// <param name="data">Data.</param>
    /// <remarks>Locals: array: Number of entries in the source collection; i: Loop/collection index.</remarks>
    public static byte[] cast(sbyte[] data)
    {
        byte[] array = new byte[data.Length];
        for (int i = 0; i < array.Length; i++)
        {
            array[i] = (byte)data[i];
        }

        return array;
    }

    /// <summary>Compute to character array.</summary>
    /// <param name="data">Data.</param>
    /// <remarks>Locals: array: Number of entries in the source collection; i: Loop/collection index.</remarks>
    public static char[] ToCharArray(sbyte[] data)
    {
        char[] array = new char[data.Length];
        for (int i = 0; i < array.Length; i++)
        {
            array[i] = (char)data[i];
        }

        return array;
    }
}

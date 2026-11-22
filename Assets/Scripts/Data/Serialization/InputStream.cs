public class InputStream : myReader
{
    /// <summary>Initialize input stream state and its supplied dependencies.</summary>
    public InputStream()
    {
    }

    /// <summary>Initialize input stream state and its supplied dependencies.</summary>
    /// <param name="data">Data.</param>
    public InputStream(sbyte[] data)
    {
        buffer = data;
    }

    /// <summary>Initialize input stream state and its supplied dependencies.</summary>
    /// <param name="filename">Filename.</param>
    public InputStream(string filename) : base(filename)
    {
    }
}

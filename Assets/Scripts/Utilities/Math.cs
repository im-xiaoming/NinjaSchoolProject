public class Math
{
    /// <summary>Compute abs.</summary>
    /// <param name="i">Loop/collection index.</param>
    public static int abs(int i)
    {
        return (i <= 0) ? (-i) : i;
    }

    /// <summary>Compute min.</summary>
    /// <param name="x">Horizontal coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="y">Vertical coordinate in the caller&#x27;s coordinate space.</param>
    public static int min(int x, int y)
    {
        return (x >= y) ? y : x;
    }

    /// <summary>Compute max.</summary>
    /// <param name="x">Horizontal coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="y">Vertical coordinate in the caller&#x27;s coordinate space.</param>
    public static int max(int x, int y)
    {
        return (x <= y) ? y : x;
    }

    /// <summary>Compute pow.</summary>
    /// <param name="data">Data.</param>
    /// <param name="x">Horizontal coordinate in the caller&#x27;s coordinate space.</param>
    /// <remarks>Locals: num: Numeric intermediate used by this operation; the legacy name is retained; i: Loop/collection index.</remarks>
    public static int pow(int data, int x)
    {
        int num = 1;
        for (int i = 0; i < x; i++)
        {
            num *= data;
        }

        return num;
    }
}

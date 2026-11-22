using System;

public class MyRandom
{
    // r: Random-number source.
    public Random r;
    /// <summary>Initialize my random state and its supplied dependencies.</summary>
    public MyRandom()
    {
        r = new Random();
    }

    /// <summary>Compute next int.</summary>
    public int nextInt()
    {
        return r.Next();
    }

    /// <summary>Compute next int.</summary>
    /// <param name="a">First operand or current actor.</param>
    public int nextInt(int a)
    {
        return r.Next(a);
    }

    /// <summary>Compute next int.</summary>
    /// <param name="a">First operand or current actor.</param>
    /// <param name="b">Buffer or intermediate output value.</param>
    public int nextInt(int a, int b)
    {
        return r.Next(a, b);
    }
}

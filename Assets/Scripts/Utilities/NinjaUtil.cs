using System;

public class NinjaUtil
{
    public static MyRandom randomz = new MyRandom();
    /// <summary>Handle load map complete.</summary>
    public static void onLoadMapComplete()
    {
        GameCanvas.endDlg();
    }

    /// <summary>Handle loading.</summary>
    public void onLoading()
    {
        GameCanvas.startWaitDlg("Đang tải dữ liệu");
    }

    /// <summary>Return moneys.</summary>
    /// <param name="m">Current message or map entry.</param>
    /// <remarks>Locals: text: Text fragment assembled or read by this operation; num: Numeric intermediate used by this operation; the legacy name is retained; i: Loop/collection index; num2: Numeric intermediate used by this operation; the legacy name is retained.</remarks>
    public static string getMoneys(long m)
    {
        string text = string.Empty;
        long num = m / 1000 + 1;
        for (int i = 0; i < num; i++)
        {
            if (m >= 1000)
            {
                long num2 = m % 1000;
                text = ((num2 != 0) ? ((num2 >= 10) ? ((num2 >= 100) ? ("." + num2 + text) : (".0" + num2 + text)) : (".00" + num2 + text)) : (".000" + text));
                m /= 1000;
                continue;
            }

            text = m + text;
            break;
        }

        return text;
    }

    /// <summary>Compute random number.</summary>
    /// <param name="max">Max.</param>
    public static int randomNumber(int max)
    {
        return randomz.nextInt(max);
    }

    /// <summary>Compute number to string.</summary>
    /// <param name="number">Number.</param>
    /// <remarks>Locals: text: Text fragment assembled or read by this operation; text2: Text fragment assembled or read by this operation; num: Number of entries in the source collection.</remarks>
    public static string numberToString(string number)
    {
        string text = string.Empty;
        string text2 = string.Empty;
        if (number.Equals(string.Empty))
        {
            return text;
        }

        if (number[0] == '-')
        {
            text2 = "-";
            number = number.Substring(1);
        }

        for (int num = number.Length - 1; num >= 0; num--)
        {
            text = (((number.Length - 1 - num) % 3 != 0 || number.Length - 1 - num <= 0) ? (number[num] + text) : (number[num] + "." + text));
        }

        return text2 + text;
    }

    /// <summary>Compute random.</summary>
    /// <param name="a">First operand or current actor.</param>
    /// <param name="b">Buffer or intermediate output value.</param>
    public static int random(int a, int b)
    {
        return a + randomz.nextInt(b - a);
    }

    /// <summary>Read byte array int.</summary>
    /// <param name="msg">Framed protocol message.</param>
    /// <remarks>Locals: num: Field decoded from the incoming binary payload.</remarks>
    public static sbyte[] readByteArray_Int(Message msg)
    {
        try
        {
            int num = msg.reader().readInt();
            sbyte[] data = new sbyte[num];
            msg.reader().read(ref data);
            return data;
        }
        catch (Exception ex)
        {
            Out.println(ex.Message + ex.StackTrace);
        }

        return null;
    }

    /// <summary>Read byte array.</summary>
    /// <param name="msg">Framed protocol message.</param>
    /// <remarks>Locals: num: Field decoded from the incoming binary payload.</remarks>
    public static sbyte[] readByteArray(Message msg)
    {
        try
        {
            int num = msg.reader().readInt();
            sbyte[] data = new sbyte[num];
            msg.reader().read(ref data);
            return data;
        }
        catch (Exception ex)
        {
            Out.println(ex.Message + ex.StackTrace);
        }

        return null;
    }

    /// <summary>Read byte array.</summary>
    /// <param name="dos">Dos.</param>
    /// <remarks>Locals: num: Field decoded from the incoming binary payload.</remarks>
    public static sbyte[] readByteArray(myReader dos)
    {
        try
        {
            int num = dos.readInt();
            sbyte[] data = new sbyte[num];
            dos.read(ref data);
            return data;
        }
        catch (Exception ex)
        {
            Out.println(ex.Message + ex.StackTrace);
        }

        return null;
    }

    /// <summary>Split ninja util.</summary>
    /// <param name="_text">Text.</param>
    /// <param name="_searchStr">Search text.</param>
    public static string[] split(string _text, string _searchStr)
    {
        return mFont.splitStringSv(_text, _searchStr);
    }

    /// <summary>Compute replace.</summary>
    /// <param name="text">Text fragment assembled or read by this operation.</param>
    /// <param name="regex">Regex.</param>
    /// <param name="replacement">Replacement.</param>
    public static string replace(string text, string regex, string replacement)
    {
        return text.Replace(regex, replacement);
    }

    /// <summary>Return time.</summary>
    /// <param name="timeRemainS">Time remain s.</param>
    /// <remarks>Locals: num: Numeric intermediate used by this operation; the legacy name is retained; num2: Numeric intermediate used by this operation; the legacy name is retained; num3: Numeric intermediate used by this operation; the legacy name is retained.</remarks>
    public static string getTime(int timeRemainS)
    {
        int num = 0;
        if (timeRemainS > 60)
        {
            num = timeRemainS / 60;
            timeRemainS %= 60;
        }

        int num2 = 0;
        if (num > 60)
        {
            num2 = num / 60;
            num %= 60;
        }

        int num3 = 0;
        if (num2 > 24)
        {
            num3 = num2 / 24;
            num2 %= 24;
        }

        string empty = string.Empty;
        if (num3 > 0)
        {
            empty += num3;
            empty += "d";
            return empty + num2 + "h";
        }

        if (num2 > 0)
        {
            empty += num2;
            empty += "h";
            return empty + num + "'";
        }

        empty = ((num <= 9) ? (empty + "0" + num) : (empty + num));
        empty += ":";
        if (timeRemainS > 9)
        {
            return empty + timeRemainS;
        }

        return empty + "0" + timeRemainS;
    }
}

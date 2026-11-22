using Functions.Auto;
using System;
using UnityEngine;

public partial class Service
{
    /// <summary>Send the lucky draw request through the existing client protocol.</summary>
    /// <param name="type">Type.</param>
    /// <param name="money">Money.</param>
    /// <param name="typeLucky">Type lucky.</param>
    public void luckyDraw(short type, string money, sbyte typeLucky)
    {
        Message message = null;
        try
        {
            message = new Message((sbyte)92);
            message.writer().writeShort(type);
            message.writer().writeUTF(money);
            message.writer().writeByte(typeLucky);
            session.sendMessage(message);
        }
        catch (Exception)
        {
        }
        finally
        {
            message.cleanup();
        }
    }
}

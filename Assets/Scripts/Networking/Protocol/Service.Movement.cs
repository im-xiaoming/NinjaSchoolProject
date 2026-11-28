using Functions.Auto;
using System;
using UnityEngine;

public partial class Service
{
    /// <summary>Send the character move request through the existing client protocol.</summary>
    /// <param name="xSend">X send.</param>
    /// <param name="ySend">Y send.</param>
    public void charMove(int xSend, int ySend)
    {
        try
        {
            if (xSend - xx != 0 || ySend - yy != 0)
            {
                Message message = new Message(1);
                message.writer().writeShort(xSend);
                message.writer().writeShort(ySend);
                xx = xSend;
                yy = ySend;
                session.sendMessage(message);
                message.cleanup();
            }
        }
        catch (Exception)
        {
        }
    }

    /// <summary>Compute message not map.</summary>
    /// <param name="command">Command.</param>
    public Message messageNotMap(sbyte command)
    {
        Message message = new Message((sbyte)(-28));
        message.writer().writeByte(command);
        return message;
    }

    /// <summary>Send the admin move request through the existing client protocol.</summary>
    /// <param name="mapId">Map template identifier.</param>
    public void adminMove(int mapId)
    {
        Message message = null;
        try
        {
            message = messageSubCommand(-70);
            message.writer().writeByte(mapId);
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

    /// <summary>Send the character move request through the existing client protocol.</summary>
    /// <param name="pos">Retained for call compatibility; not read by this implementation.</param>
    /// <param name="xSend">X send.</param>
    /// <param name="ySend">Y send.</param>
    public void charMove(string pos, int xSend, int ySend)
    {
        try
        {
            if (xSend - xx != 0 || ySend - yy != 0)
            {
                Message message = new Message((sbyte)1);
                message.writer().writeShort(xSend);
                message.writer().writeShort(ySend);
                xx = xSend;
                yy = ySend;
                session.sendMessage(message);
                message.cleanup();
            }
        }
        catch (Exception)
        {
        }
    }

    /// <summary>Open user interface zone.</summary>
    public void openUIZone()
    {
        Message message = null;
        try
        {
            message = new Message((sbyte)36);
            session.sendMessage(message);
        }
        catch (Exception ex)
        {
            Out.println(ex.Message + ex.StackTrace);
        }
        finally
        {
            message.cleanup();
        }
    }

    /// <summary>Update map.</summary>
    public void updateMap()
    {
        Message message = null;
        try
        {
            message = messageNotMap(-121);
            session.sendMessage(message);
        }
        catch (Exception ex)
        {
            Out.println(ex.Message + ex.StackTrace);
        }
        finally
        {
            message.cleanup();
        }
    }

    /// <summary>Move member.</summary>
    /// <param name="index">Index.</param>
    public void moveMember(int index)
    {
        Message message = null;
        try
        {
            message = messageSubCommand(-86);
            message.writer().writeByte(index);
            session.sendMessage(message);
        }
        catch (Exception ex)
        {
            Out.println(ex.Message + ex.StackTrace);
        }
        finally
        {
            message.cleanup();
        }
    }

    /// <summary>Remove friend.</summary>
    /// <param name="name">Name.</param>
    public void removeFriend(string name)
    {
        Message message = null;
        try
        {
            message = messageSubCommand(-83);
            message.writer().writeUTF(name);
            session.sendMessage(message);
        }
        catch (Exception ex)
        {
            Out.println(ex.Message + ex.StackTrace);
        }
        finally
        {
            message.cleanup();
        }
    }

    /// <summary>Move out clan.</summary>
    /// <param name="name">Name.</param>
    public void moveOutClan(string name)
    {
        Message message = null;
        try
        {
            message = messageNotMap(-93);
            message.writer().writeUTF(name);
            session.sendMessage(message);
        }
        catch (Exception ex)
        {
            Out.println(ex.Message + ex.StackTrace);
        }
        finally
        {
            message.cleanup();
        }
    }

    /// <summary>Remove vithu.</summary>
    /// <param name="id">Identifier.</param>
    public void doRemoveVithu(int id)
    {
        Message message = null;
        try
        {
            message = new Message((sbyte)117);
            message.writer().writeByte((sbyte)0);
            message.writer().writeByte(id);
            session.sendMessage(message);
        }
        catch (Exception)
        {
        }
    }
}

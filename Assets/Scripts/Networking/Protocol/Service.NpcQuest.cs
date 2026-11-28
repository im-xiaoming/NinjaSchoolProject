using Functions.Auto;
using System;
using UnityEngine;

public partial class Service
{
    /// <summary>Clear task.</summary>
    public void clearTask()
    {
        Message message = null;
        try
        {
            message = messageNotMap(-98);
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

    /// <summary>Send the request clan info request through the existing client protocol.</summary>
    public void requestClanInfo()
    {
        Message message = null;
        try
        {
            message = messageNotMap(-113);
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

    /// <summary>Send the request icon request through the existing client protocol.</summary>
    /// <param name="id">Identifier.</param>
    public void requestIcon(int id)
    {
        Message message = null;
        try
        {
            message = messageNotMap(-115);
            message.writer().writeInt(id);
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

    /// <summary>Send the request clan member request through the existing client protocol.</summary>
    public void requestClanMember()
    {
        Message message = null;
        try
        {
            message = messageNotMap(-112);
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

    /// <summary>Send the request change zone request through the existing client protocol.</summary>
    /// <param name="zoneId">Zone identifier.</param>
    /// <param name="indexUI">Item slot in the selected legacy panel.</param>
    public void requestChangeZone(int zoneId, int indexUI)
    {
        Message message = new Message((sbyte)28);
        try
        {
            message.writer().writeByte(zoneId);
            message.writer().writeByte(indexUI);
            session.sendMessage(message);
            message.cleanup();
        }
        catch (Exception)
        {
        }
    }

    /// <summary>Send the request mod template request through the existing client protocol.</summary>
    /// <param name="modTemplateId">Mod template identifier.</param>
    public void requestModTemplate(int modTemplateId)
    {
        Message message = null;
        try
        {
            message = messageNotMap(-108);
            message.writer().writeShort(modTemplateId);
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

    /// <summary>Open menu.</summary>
    /// <param name="npcId">NPC template identifier.</param>
    public void openMenu(int npcId)
    {
        Message message = null;
        try
        {
            message = new Message((sbyte)40);
            message.writer().writeShort(npcId);
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

    /// <summary>Send the menu request through the existing client protocol.</summary>
    /// <param name="typeclose">Typeclose.</param>
    /// <param name="npcId">NPC template identifier.</param>
    /// <param name="menuId">Menu identifier.</param>
    /// <param name="optionId">Option identifier.</param>
    public void menu(sbyte typeclose, int npcId, int menuId, int optionId)
    {
        Message message = null;
        try
        {
            message = new Message((sbyte)29);
            message.writer().writeByte(typeclose);
            message.writer().writeByte(npcId);
            message.writer().writeByte(menuId);
            message.writer().writeByte(optionId);
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

    /// <summary>Send the menu identifier request through the existing client protocol.</summary>
    /// <param name="menuId">Menu identifier.</param>
    public void menuId(short menuId)
    {
        Message message = null;
        try
        {
            message = new Message((sbyte)34);
            message.writer().writeShort(menuId);
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

    /// <summary>Return task.</summary>
    /// <param name="npcTemplateId">Non-player character template identifier.</param>
    /// <param name="menuId">Menu identifier.</param>
    /// <param name="optionId">Option identifier.</param>
    public void getTask(int npcTemplateId, int menuId, int optionId)
    {
        Message message = null;
        try
        {
            message = new Message((sbyte)47);
            message.writer().writeByte(npcTemplateId);
            message.writer().writeByte(menuId);
            if (optionId >= 0)
            {
                message.writer().writeByte(optionId);
            }

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

    /// <summary>Send the request maptemplate request through the existing client protocol.</summary>
    /// <param name="maptemplateId">Maptemplate identifier.</param>
    public void requestMaptemplate(int maptemplateId)
    {
        Message message = null;
        try
        {
            message = messageNotMap(-109);
            message.writer().writeByte(maptemplateId);
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

    /// <summary>Send the request friend request through the existing client protocol.</summary>
    public void requestFriend()
    {
        Message message = null;
        try
        {
            message = messageSubCommand(-85);
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

    /// <summary>Send the request match info request through the existing client protocol.</summary>
    /// <param name="zoneid">Zoneid.</param>
    public void requestMatchInfo(int zoneid)
    {
        Message message = null;
        try
        {
            Out.println("zone id: " + zoneid);
            message = new Message((sbyte)100);
            message.writer().writeByte(zoneid);
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

    /// <summary>Send the request player info request through the existing client protocol.</summary>
    /// <param name="chars">Characters.</param>
    /// <remarks>Locals: i: Loop/collection index.</remarks>
    public void requestPlayerInfo(MyVector chars)
    {
        Message message = null;
        try
        {
            message = new Message((sbyte)25);
            message.writer().writeByte(chars.size());
            for (int i = 0; i < chars.size(); i++)
            {
                Char @char = (Char)chars.elementAt(i);
                message.writer().writeInt(@char.charID);
            }

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

    /// <summary>Send the request enemies request through the existing client protocol.</summary>
    public void requestEnemies()
    {
        Message message = null;
        try
        {
            message = messageSubCommand(-84);
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

    /// <summary>Send the request clan log request through the existing client protocol.</summary>
    public void requestClanLog()
    {
        Message message = null;
        try
        {
            message = messageNotMap(-114);
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

    /// <summary>Send the request ranked request through the existing client protocol.</summary>
    /// <param name="index">Index.</param>
    /// <param name="name">Name.</param>
    public void requestRanked(sbyte index, string name)
    {
        Message message = null;
        try
        {
            message = new Message((sbyte)121);
            message.writer().writeByte(index);
            message.writer().writeUTF(name);
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

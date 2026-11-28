using Functions.Auto;
using System;
using UnityEngine;

public partial class Service
{
    /// <summary>Send the up skill request through the existing client protocol.</summary>
    /// <param name="skillTemplateId">Skill template identifier.</param>
    /// <param name="point">Point.</param>
    public void upSkill(int skillTemplateId, int point)
    {
        Message message = null;
        try
        {
            message = messageSubCommand(-108);
            message.writer().writeShort(skillTemplateId);
            message.writer().writeByte(point);
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

    /// <summary>Select skill.</summary>
    /// <param name="skillTemplateId">Skill template identifier.</param>
    public void selectSkill(int skillTemplateId)
    {
        Message message = null;
        try
        {
            message = new Message((sbyte)41);
            message.writer().writeShort(skillTemplateId);
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

    /// <summary>Send player attack through the existing client protocol.</summary>
    /// <param name="vMob">V monster.</param>
    /// <param name="vChar">V character.</param>
    /// <param name="type">Type.</param>
    /// <remarks>Locals: i: Loop/collection index; mob: Monster being inspected or targeted; j: Nested loop/collection index; k: Key or nested iteration index; l: Length or current list; char2: Client character being inspected or controlled.</remarks>
    public void sendPlayerAttack(MyVector vMob, MyVector vChar, int type)
    {
        try
        {
            Message message = null;
            if (type == 0)
            {
                return;
            }

            if (vMob.size() > 0 && vChar.size() > 0)
            {
                switch (type)
                {
                    case 1:
                        message = new Message((sbyte)4);
                        break;
                    case 2:
                        message = new Message((sbyte)73);
                        break;
                }

                message.writer().writeByte(vMob.size());
                for (int i = 0; i < vMob.size(); i++)
                {
                    Mob mob = (Mob)vMob.elementAt(i);
                    message.writer().writeByte(mob.mobId);
                }

                for (int j = 0; j < vChar.size(); j++)
                {
                    Char @char = (Char)vChar.elementAt(j);
                    if (@char != null)
                    {
                        message.writer().writeInt(@char.charID);
                    }
                    else
                    {
                        message.writer().writeInt(-1);
                    }
                }
            }
            else if (vMob.size() > 0)
            {
                message = new Message((sbyte)60);
                for (int k = 0; k < vMob.size(); k++)
                {
                    Mob mob2 = (Mob)vMob.elementAt(k);
                    message.writer().writeByte(mob2.mobId);
                }
            }
            else if (vChar.size() > 0)
            {
                message = new Message((sbyte)61);
                for (int l = 0; l < vChar.size(); l++)
                {
                    Char char2 = (Char)vChar.elementAt(l);
                    message.writer().writeInt(char2.charID);
                }
            }

            if (message != null)
            {
                session.sendMessage(message);
            }
        }
        catch (Exception)
        {
            Out.println("loi send attt ");
        }
    }

    /// <summary>Update skill.</summary>
    public void updateSkill()
    {
        Message message = null;
        try
        {
            message = messageNotMap(-120);
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

    /// <summary>Send use skill my buff through the existing client protocol.</summary>
    public void sendUseSkillMyBuff()
    {
        Message message = null;
        try
        {
            message = new Message((sbyte)74);
            message.writer().writeByte(Char.getMyChar().cdir);
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

    /// <summary>Send attack monster fast through the existing client protocol.</summary>
    /// <param name="idMonter">Identifier monter.</param>
    public void sendAttackMobFast(short idMonter)
    {
        Message message = null;
        try
        {
            message = new Message((sbyte)119);
            message.writer().writeByte(idMonter);
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

    /// <summary>Send the request image effect auto request through the existing client protocol.</summary>
    /// <param name="type">Type.</param>
    /// <param name="id">Identifier.</param>
    public void request_imgEffAuto(sbyte type, short id)
    {
        Message message = null;
        try
        {
            message = messageNotMap(122);
            message.writer().writeByte(type);
            message.writer().writeByte(id);
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

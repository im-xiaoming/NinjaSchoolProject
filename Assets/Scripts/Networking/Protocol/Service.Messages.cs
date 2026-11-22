using Functions.Auto;
using System;
using UnityEngine;

public partial class Service
{
    /// <summary>Return the shared instance, creating it on first access when needed.</summary>
    public static Service gI()
    {
        if (instance == null)
        {
            instance = new Service();
        }

        return instance;
    }

    /// <summary>Send the clan please request through the existing client protocol.</summary>
    /// <param name="charId">Character identifier.</param>
    public void clanPlease(int charId)
    {
        Message message = null;
        try
        {
            message = messageSubCommand(-61);
            message.writer().writeInt(charId);
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

    /// <summary>Send the accept invite clan request through the existing client protocol.</summary>
    /// <param name="playerMapId">Player map identifier.</param>
    public void acceptInviteClan(int playerMapId)
    {
        Message message = null;
        try
        {
            message = messageSubCommand(-62);
            message.writer().writeInt(playerMapId);
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

    /// <summary>Send the clan invite request through the existing client protocol.</summary>
    /// <param name="charId">Character identifier.</param>
    public void clanInvite(int charId)
    {
        Message message = null;
        try
        {
            message = messageSubCommand(-63);
            message.writer().writeInt(charId);
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

    /// <summary>Compute message not login.</summary>
    /// <param name="command">Command.</param>
    public Message messageNotLogin(sbyte command)
    {
        Message message = new Message((sbyte)(-29));
        message.writer().writeByte(command);
        return message;
    }

    /// <summary>Send the change name request through the existing client protocol.</summary>
    /// <param name="name">Name.</param>
    /// <param name="id">Identifier.</param>
    public void changeName(string name, int id)
    {
        Message message = null;
        try
        {
            message = messageNotMap(-97);
            message.writer().writeInt(id);
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

    /// <summary>Compute message sub command.</summary>
    /// <param name="command">Command.</param>
    public static Message messageSubCommand(sbyte command)
    {
        Message message = new Message((sbyte)(-30));
        message.writer().writeByte(command);
        return message;
    }

    /// <summary>Set client type.</summary>
    public void setClientType()
    {
        try
        {
            Message message = messageNotLogin(-125);
            GameMidlet.CLIENT_TYPE = 4;
            if (mGraphics.zoomLevel == 1)
            {
                GameMidlet.CLIENT_TYPE = 0;
            }

            if (!Main.isPC)
            {
                if (Main.isWp)
                {
                    GameMidlet.CLIENT_TYPE = 6;
                }
                else if (Main.IphoneVersionApp)
                {
                    GameMidlet.CLIENT_TYPE = 5;
                }
                else
                {
                    GameMidlet.CLIENT_TYPE = 3;
                }
            }

            message.writer().writeByte(GameMidlet.CLIENT_TYPE);
            message.writer().writeByte(mGraphics.zoomLevel);
            message.writer().writeBoolean(GameCanvas.isGPRS);
            message.writer().writeInt(GameCanvas.w);
            message.writer().writeInt(GameCanvas.h);
            message.writer().writeBoolean(TField.isQwerty);
            message.writer().writeBoolean(GameCanvas.isTouch);
            message.writer().writeUTF("iphone platform xxx");
            if (!Main.isPC)
            {
                message.writer().writeInt(Main.versionIp);
            }
            else
            {
                message.writer().writeInt(0);
            }

            message.writer().writeByte(0);
            message.writer().writeByte(mResources.languageID);
            message.writer().writeInt(GameMidlet.userProvider);
            message.writer().writeUTF(GameMidlet.clientAgent);
            session.sendMessage(message);
            message.cleanup();
        }
        catch (Exception ex)
        {
            Out.println(ex.Message + ex.StackTrace);
        }
    }

    /// <summary>Select character to play.</summary>
    /// <param name="charname">Charname.</param>
    public void selectCharToPlay(string charname)
    {
        Message message = new Message((sbyte)(-28));
        try
        {
            message.writer().writeByte((sbyte)(-126));
            message.writer().writeUTF(charname);
        }
        catch (Exception ex)
        {
            Out.println(ex.Message + ex.StackTrace);
        }

        session.sendMessage(message);
    }

    /// <summary>Send the up potential request through the existing client protocol.</summary>
    /// <param name="index">Index.</param>
    /// <param name="point">Point.</param>
    public void upPotential(int index, int point)
    {
        Message message = null;
        try
        {
            message = messageSubCommand(-109);
            message.writer().writeByte(index);
            message.writer().writeShort(point);
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

    /// <summary>Open find party.</summary>
    public void openFindParty()
    {
        Message message = null;
        try
        {
            message = messageSubCommand(-77);
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

    // CHAT_MAP (-23): offline the text is a dev terminal command, run by the server (Server/Net/DevTerminal.cs).
    /// <summary>Send the chat request through the existing client protocol.</summary>
    /// <param name="text">Text fragment assembled or read by this operation.</param>
    public void chat(string text)
    {
        Message message = null;
        try
        {
            message = new Message((sbyte)(-23));
            message.writer().writeUTF(text);
            session.sendMessage(message);
        }
        catch (Exception ex)
        {
            Out.println(ex.Message + ex.StackTrace);
        }
        finally
        {
            if (message != null)
            {
                message.cleanup();
            }
        }
    }

    /// <summary>Send the accept invite trade request through the existing client protocol.</summary>
    /// <param name="playerMapId">Player map identifier.</param>
    public void acceptInviteTrade(int playerMapId)
    {
        Message message = null;
        try
        {
            message = new Message((sbyte)44);
            message.writer().writeInt(playerMapId);
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

    /// <summary>Send the accept invite test dungeon request through the existing client protocol.</summary>
    /// <param name="playerMapId">Player map identifier.</param>
    public void acceptInviteTestDun(int playerMapId)
    {
        Message message = null;
        try
        {
            message = new Message((sbyte)99);
            message.writer().writeInt(playerMapId);
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

    /// <summary>Send the accept invite test gt request through the existing client protocol.</summary>
    /// <param name="playerMapId">Player map identifier.</param>
    public void acceptInviteTestGT(int playerMapId)
    {
        Message message = null;
        try
        {
            message = new Message((sbyte)106);
            message.writer().writeInt(playerMapId);
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

    /// <summary>Send user interface confirm identifier through the existing client protocol.</summary>
    /// <param name="Id">Identifier.</param>
    public void sendUIConfirmID(int Id)
    {
        Message message = null;
        try
        {
            message = new Message((sbyte)107);
            message.writer().writeByte(Id);
            session.sendMessage(message);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.StackTrace);
        }
        finally
        {
            message.cleanup();
        }
    }

    /// <summary>Send the accept invite test request through the existing client protocol.</summary>
    /// <param name="playerMapId">Player map identifier.</param>
    public void acceptInviteTest(int playerMapId)
    {
        Message message = null;
        try
        {
            message = new Message((sbyte)66);
            message.writer().writeInt(playerMapId);
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

    /// <summary>Check whether cel invite trade applies to the supplied state.</summary>
    public void cancelInviteTrade()
    {
        Message message = null;
        try
        {
            message = new Message((sbyte)56);
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

    /// <summary>Check whether cel trade applies to the supplied state.</summary>
    public void cancelTrade()
    {
        Message message = null;
        try
        {
            message = new Message((sbyte)57);
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

    /// <summary>Send the trade accept request through the existing client protocol.</summary>
    public void tradeAccept()
    {
        Message message = null;
        try
        {
            message = new Message((sbyte)46);
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

    /// <summary>Return town from dead.</summary>
    public void returnTownFromDead()
    {
        Message message = null;
        try
        {
            message = new Message((sbyte)(-9));
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

    /// <summary>Send the wake up from dead request through the existing client protocol.</summary>
    public void wakeUpFromDead()
    {
        Message message = null;
        try
        {
            message = new Message((sbyte)(-10));
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

    /// <summary>Send the client ok request through the existing client protocol.</summary>
    public void clientOk()
    {
        Message message = null;
        try
        {
            message = messageNotMap(-101);
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

    /// <summary>Send the trade invite request through the existing client protocol.</summary>
    /// <param name="charId">Character identifier.</param>
    public void tradeInvite(int charId)
    {
        Message message = null;
        try
        {
            message = new Message((sbyte)43);
            message.writer().writeInt(charId);
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

    /// <summary>Add friend.</summary>
    /// <param name="name">Name.</param>
    public void addFriend(string name)
    {
        Message message = null;
        try
        {
            message = new Message((sbyte)59);
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

    /// <summary>Add party accept.</summary>
    /// <param name="charId">Character identifier.</param>
    public void addPartyAccept(int charId)
    {
        Message message = null;
        try
        {
            message = new Message((sbyte)80);
            message.writer().writeInt(charId);
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

    /// <summary>Add party cancel.</summary>
    /// <param name="charId">Character identifier.</param>
    public void addPartyCancel(int charId)
    {
        Message message = null;
        try
        {
            message = new Message((sbyte)81);
            message.writer().writeInt(charId);
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

    /// <summary>Send the test invite request through the existing client protocol.</summary>
    /// <param name="charId">Character identifier.</param>
    public void testInvite(int charId)
    {
        Message message = null;
        try
        {
            message = new Message((sbyte)65);
            message.writer().writeInt(charId);
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

    /// <summary>Add cuu sat.</summary>
    /// <param name="charId">Character identifier.</param>
    public void addCuuSat(int charId)
    {
        Message message = null;
        try
        {
            message = new Message((sbyte)68);
            message.writer().writeInt(charId);
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

    /// <summary>Send the buff live request through the existing client protocol.</summary>
    /// <param name="charId">Character identifier.</param>
    public void buffLive(int charId)
    {
        Message message = null;
        try
        {
            message = messageSubCommand(-79);
            message.writer().writeInt(charId);
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

    /// <summary>Add party.</summary>
    /// <param name="name">Name.</param>
    public void addParty(string name)
    {
        Message message = null;
        try
        {
            message = new Message((sbyte)79);
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

    /// <summary>Send the change player killing request through the existing client protocol.</summary>
    /// <param name="typePk">Type player killing.</param>
    public void changePk(int typePk)
    {
        Message message = null;
        try
        {
            message = messageSubCommand(-93);
            message.writer().writeByte(typePk);
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

    /// <summary>Send the change team leader request through the existing client protocol.</summary>
    /// <param name="index">Index.</param>
    public void changeTeamLeader(int index)
    {
        Message message = null;
        try
        {
            message = messageSubCommand(-87);
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

    /// <summary>Create party.</summary>
    public void createParty()
    {
        Message message = null;
        try
        {
            message = messageSubCommand(-88);
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

    /// <summary>Lock party.</summary>
    /// <param name="isLock">Is lock.</param>
    public void lockParty(bool isLock)
    {
        Message message = null;
        try
        {
            message = messageSubCommand(-76);
            message.writer().writeBoolean(isLock);
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

    /// <summary>Send the out party request through the existing client protocol.</summary>
    public void outParty()
    {
        Message message = null;
        try
        {
            message = new Message((sbyte)83);
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

    /// <summary>Send the please input party request through the existing client protocol.</summary>
    /// <param name="str">Text.</param>
    public void pleaseInputParty(string str)
    {
        Message message = null;
        try
        {
            message = new Message((sbyte)23);
            message.writer().writeUTF(str);
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

    /// <summary>Send the accept please party request through the existing client protocol.</summary>
    /// <param name="str">Text.</param>
    public void acceptPleaseParty(string str)
    {
        Message message = null;
        try
        {
            message = new Message((sbyte)24);
            message.writer().writeUTF(str);
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

    /// <summary>Save rms.</summary>
    /// <param name="key">Key.</param>
    /// <param name="data">Data.</param>
    /// <param name="type">Type.</param>
    public void saveRms(string key, sbyte[] data, sbyte type)
    {
        Message message = null;
        try
        {
            message = messageSubCommand(-67);
            message.writer().writeUTF(key);
            message.writer().writeInt(data.Length);
            message.writer().write(data);
            message.writer().writeByte(type);
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

    /// <summary>Load rms.</summary>
    /// <param name="key">Key.</param>
    public void loadRMS(string key)
    {
        Message message = null;
        try
        {
            message = messageSubCommand(-65);
            message.writer().writeUTF(key);
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

    /// <summary>Send the accept please clan request through the existing client protocol.</summary>
    /// <param name="playerMapId">Player map identifier.</param>
    public void acceptPleaseClan(int playerMapId)
    {
        Message message = null;
        try
        {
            message = messageSubCommand(-60);
            message.writer().writeInt(playerMapId);
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

    /// <summary>Send the change clan type request through the existing client protocol.</summary>
    /// <param name="name">Name.</param>
    /// <param name="type">Type.</param>
    public void changeClanType(string name, int type)
    {
        Message message = null;
        try
        {
            message = messageNotMap(-94);
            message.writer().writeUTF(name);
            message.writer().writeByte(type);
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

    /// <summary>Send the change clan alert request through the existing client protocol.</summary>
    /// <param name="str">Text.</param>
    public void changeClanAlert(string str)
    {
        Message message = null;
        try
        {
            message = messageNotMap(-95);
            message.writer().writeUTF(str);
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

    /// <summary>Send the input currency clan request through the existing client protocol.</summary>
    /// <param name="coin">Currency.</param>
    public void inputCoinClan(int coin)
    {
        Message message = null;
        try
        {
            message = messageNotMap(-90);
            message.writer().writeInt(coin);
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

    /// <summary>Send the output currency clan request through the existing client protocol.</summary>
    /// <param name="coin">Currency.</param>
    public void outputCoinClan(int coin)
    {
        Message message = null;
        try
        {
            message = messageNotMap(-89);
            message.writer().writeInt(coin);
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

    /// <summary>Send the out clan request through the existing client protocol.</summary>
    public void outClan()
    {
        Message message = null;
        try
        {
            message = messageNotMap(-92);
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

    /// <summary>Send the clan up level request through the existing client protocol.</summary>
    public void clanUpLevel()
    {
        Message message = null;
        try
        {
            message = messageNotMap(-91);
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

    /// <summary>Send the view info request through the existing client protocol.</summary>
    /// <param name="cName">C name.</param>
    public void viewInfo(string cName)
    {
        Message message = null;
        try
        {
            message = new Message((sbyte)93);
            message.writer().writeUTF(cName);
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

    /// <summary>Send the invite clan dungeon request through the existing client protocol.</summary>
    /// <param name="name">Name.</param>
    public void inviteClanDun(string name)
    {
        Message message = null;
        try
        {
            message = messageNotMap(-87);
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

    /// <summary>Open lock acc protect.</summary>
    /// <param name="pass2">Pass2.</param>
    public void openLockAccProtect(int pass2)
    {
        Message message = null;
        try
        {
            message = messageNotMap(-103);
            message.writer().writeInt(pass2);
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

    /// <summary>Send the invite clan battlefield request through the existing client protocol.</summary>
    /// <param name="name">Name.</param>
    public void inviteClanBattlefield(string name)
    {
        Message message = null;
        try
        {
            message = messageNotMap(-70);
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

    /// <summary>Send the accept clan battlefield request through the existing client protocol.</summary>
    public void acceptClanBattlefield()
    {
        Message message = null;
        try
        {
            message = messageNotMap(-68);
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

    /// <summary>Send the invite clan battlefield all request through the existing client protocol.</summary>
    public void inviteClanBattlefieldAll()
    {
        Message message = null;
        try
        {
            message = messageNotMap(-69);
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

    /// <summary>Send the active acc protect request through the existing client protocol.</summary>
    /// <param name="pass">Pass.</param>
    public void activeAccProtect(int pass)
    {
        Message message = null;
        try
        {
            message = messageNotMap(-105);
            message.writer().writeInt(pass);
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

    /// <summary>Clear acc protect.</summary>
    /// <param name="pass">Pass.</param>
    public void clearAccProtect(int pass)
    {
        Message message = null;
        try
        {
            message = messageNotMap(-102);
            message.writer().writeInt(pass);
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

    /// <summary>Send the reward pb request through the existing client protocol.</summary>
    public void rewardPB()
    {
        Message message = null;
        try
        {
            message = messageNotMap(-82);
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

    /// <summary>Send the reward ct request through the existing client protocol.</summary>
    public void rewardCT()
    {
        Message message = null;
        try
        {
            message = messageNotMap(-79);
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

    /// <summary>Select card.</summary>
    public void selectCard()
    {
        Message message = null;
        try
        {
            message = messageNotMap(-72);
            message.writer().writeByte(GameScr.indexSelect);
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

    /// <summary>Send the stone refinement request through the existing client protocol.</summary>
    /// <param name="items">Items (milliseconds).</param>
    /// <remarks>Locals: i: Loop/collection index; j: Nested loop/collection index.</remarks>
    public void luyenthach(Item[] items)
    {
        GameCanvas.msgdlg.pleasewait();
        Message message = null;
        try
        {
            message = new Message((sbyte)110);
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i] != null && (items[i].template.id == 10 || items[i].template.id == 11))
                {
                    message.writer().writeByte(items[i].indexUI);
                    break;
                }
            }

            for (int j = 0; j < items.Length; j++)
            {
                if (items[j] != null && (items[j].template.id == 455 || items[j].template.id == 456))
                {
                    message.writer().writeByte(items[j].indexUI);
                }
            }

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

    /// <summary>Send the tinhluyen request through the existing client protocol.</summary>
    /// <param name="it">Item instance being inspected or transferred.</param>
    /// <param name="items">Items (milliseconds).</param>
    /// <remarks>Locals: i: Loop/collection index.</remarks>
    public void tinhluyen(Item it, Item[] items)
    {
        GameCanvas.msgdlg.pleasewait();
        Message message = null;
        try
        {
            message = new Message((sbyte)111);
            message.writer().writeByte(it.indexUI);
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i] != null && (items[i].template.id == 455 || items[i].template.id == 456 || items[i].template.id == 457))
                {
                    message.writer().writeByte(items[i].indexUI);
                }
            }

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

    /// <summary>Send the dichchuyen request through the existing client protocol.</summary>
    /// <param name="it">Item instance being inspected or transferred.</param>
    /// <param name="items">Items (milliseconds).</param>
    /// <remarks>Locals: num: Numeric intermediate used by this operation; the legacy name is retained; i: Loop/collection index.</remarks>
    public void dichchuyen(Item it, Item[] items)
    {
        GameCanvas.msgdlg.pleasewait();
        Message message = null;
        try
        {
            message = new Message((sbyte)112);
            message.writer().writeByte(it.indexUI);
            int num = 0;
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i] != null && items[i].template.id == 454)
                {
                    message.writer().writeByte(items[i].indexUI);
                    num++;
                }
            }

            if (num < 20)
            {
                GameCanvas.startOKDlg(mResources.NOT_ENOUGH_DICHCHUYEN);
            }
            else
            {
                session.sendMessage(message);
            }
        }
        catch (Exception)
        {
        }
        finally
        {
            message.cleanup();
        }
    }

    /// <summary>Send catkeo through the existing client protocol.</summary>
    /// <param name="id">Identifier.</param>
    public void sendCatkeo(int id)
    {
        Message message = null;
        try
        {
            message = new Message((sbyte)113);
            message.writer().writeShort(id);
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

    /// <summary>Send the chuc tet request through the existing client protocol.</summary>
    /// <param name="to">To.</param>
    /// <param name="loichuc">Loichuc.</param>
    /// <param name="type">Type.</param>
    public void ChucTet(string to, string loichuc, byte type)
    {
        Message message = null;
        string value = to + " " + loichuc;
        try
        {
            message = new Message((sbyte)(-21));
            message.writer().writeUTF(value);
            message.writer().writeByte(type);
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

    /// <summary>Send capcha through the existing client protocol.</summary>
    /// <param name="idRe">Identifier re.</param>
    /// <param name="capcha">Capcha.</param>
    public void SendCapcha(short idRe, string capcha)
    {
        Message message = null;
        try
        {
            message = new Message((sbyte)118);
            message.writer().writeUTF(capcha);
            message.writer().writeShort(idRe);
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

    /// <summary>Send the info kiemduyet request through the existing client protocol.</summary>
    /// <param name="name">Name.</param>
    /// <param name="ngaysinh">Ngaysinh.</param>
    /// <param name="diachi">Diachi.</param>
    /// <param name="cmnd">Cmnd.</param>
    /// <param name="ngaycap">Ngaycap.</param>
    /// <param name="noicap">Noicap.</param>
    /// <param name="email">Email.</param>
    public void info_Kiemduyet(string name, string ngaysinh, string diachi, string cmnd, string ngaycap, string noicap, string email)
    {
        Message message = null;
        try
        {
            message = new Message((sbyte)123);
            message.writer().writeUTF(name);
            message.writer().writeUTF(ngaysinh);
            message.writer().writeUTF(diachi);
            message.writer().writeUTF(cmnd);
            message.writer().writeUTF(ngaycap);
            message.writer().writeUTF(noicap);
            message.writer().writeUTF(email);
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

    /// <summary>Send captcha through the existing client protocol.</summary>
    /// <param name="type">Type.</param>
    public void send_Captcha(sbyte type)
    {
        Message message = null;
        try
        {
            message = messageNotMap(122);
            message.writer().writeByte(4);
            message.writer().writeByte(type);
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

    /// <summary>Send the ngockham request through the existing client protocol.</summary>
    /// <param name="type">Type.</param>
    /// <param name="item">Item instance being inspected or transferred.</param>
    /// <param name="ngoc">Item instance being inspected or transferred.</param>
    /// <param name="items">Items (milliseconds).</param>
    /// <remarks>Locals: i: Loop/collection index; j: Nested loop/collection index.</remarks>
    public void ngockham(sbyte type, Item item, Item ngoc, Item[] items)
    {
        GameCanvas.msgdlg.pleasewait();
        Message message = null;
        try
        {
            message = new Message((sbyte)124);
            message.writer().writeByte(type);
            if (type == 0)
            {
                message.writer().writeByte(item.indexUI);
                message.writer().writeByte(ngoc.indexUI);
                for (int i = 0; i < items.Length; i++)
                {
                    if (items[i] != null)
                    {
                        message.writer().writeByte(items[i].indexUI);
                    }
                }
            }
            else if (type == 1)
            {
                message.writer().writeByte(ngoc.indexUI);
                for (int j = 0; j < items.Length; j++)
                {
                    if (items[j] != null)
                    {
                        message.writer().writeByte(items[j].indexUI);
                    }
                }
            }
            else if (type == 2 || type == 3)
            {
                message.writer().writeByte(ngoc.indexUI);
            }

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

    /// <summary>Send the giaodo request through the existing client protocol.</summary>
    /// <param name="items">Items (milliseconds).</param>
    /// <remarks>Locals: i: Loop/collection index.</remarks>
    public void giaodo(Item[] items)
    {
        GameCanvas.msgdlg.pleasewait();
        Message message = null;
        try
        {
            message = new Message((sbyte)126);
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i] != null)
                {
                    message.writer().writeByte(items[i].indexUI);
                }
            }

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

    /// <summary>Send gps through the existing client protocol.</summary>
    public void sendGPS()
    {
        Message message = null;
        try
        {
            message = messageNotMap(-59);
            message.writer().writeUTF(GameMidlet.longitude);
            message.writer().writeUTF(GameMidlet.latitude);
            session.sendMessage(message);
        }
        catch (Exception)
        {
        }
    }
}

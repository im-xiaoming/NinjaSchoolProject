using Functions.Auto;
using System;
using UnityEngine;

public partial class Service
{
    /// <summary>Send the request change map request through the existing client protocol.</summary>
    public void requestChangeMap()
    {
        Message message = new Message((sbyte)(-17));
        session.sendMessage(message);
        message.cleanup();
    }

    /// <summary>Send the request item info request through the existing client protocol.</summary>
    /// <param name="typeUI">Legacy item-panel/shop category code.</param>
    /// <param name="indexUI">Item slot in the selected legacy panel.</param>
    public void requestItemInfo(int typeUI, int indexUI)
    {
        Message message = null;
        try
        {
            message = new Message((sbyte)42);
            message.writer().writeByte(typeUI);
            message.writer().writeByte(indexUI);
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

    /// <summary>Send the request item player request through the existing client protocol.</summary>
    /// <param name="charId">Character identifier.</param>
    /// <param name="indexUI">Item slot in the selected legacy panel.</param>
    public void requestItemPlayer(int charId, int indexUI)
    {
        Message message = null;
        try
        {
            message = new Message((sbyte)94);
            message.writer().writeInt(charId);
            message.writer().writeByte(indexUI);
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

    /// <summary>Send the item body to bag request through the existing client protocol.</summary>
    /// <param name="indexUI">Item slot in the selected legacy panel.</param>
    public void itemBodyToBag(int indexUI)
    {
        Message message = null;
        try
        {
            message = new Message((sbyte)15);
            message.writer().writeByte(indexUI);
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

    /// <summary>Send the item mon to bag request through the existing client protocol.</summary>
    /// <param name="indexUI">Item slot in the selected legacy panel.</param>
    public void itemMonToBag(int indexUI)
    {
        Message message = null;
        try
        {
            message = new Message((sbyte)108);
            message.writer().writeByte(indexUI);
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

    /// <summary>Send the item box to bag request through the existing client protocol.</summary>
    /// <param name="indexUI">Item slot in the selected legacy panel.</param>
    public void itemBoxToBag(int indexUI)
    {
        Message message = null;
        try
        {
            message = new Message((sbyte)16);
            message.writer().writeByte(indexUI);
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

    /// <summary>Send the item bag to box request through the existing client protocol.</summary>
    /// <param name="indexUI">Item slot in the selected legacy panel.</param>
    public void itemBagToBox(int indexUI)
    {
        Message message = null;
        try
        {
            message = new Message((sbyte)17);
            message.writer().writeByte(indexUI);
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

    /// <summary>Use item.</summary>
    /// <param name="indexUI">Item slot in the selected legacy panel.</param>
    public void useItem(int indexUI)
    {
        Message message = null;
        try
        {
            message = new Message((sbyte)11);
            message.writer().writeByte(indexUI);
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

        if (Char.getMyChar().arrItemBag[indexUI] != null && Char.getMyChar().arrItemBag[indexUI].template.type == 24)
        {
            GameScr.gI().resetButton();
            InfoDlg.showWait(string.Empty);
        }
    }

    /// <summary>Sell item.</summary>
    /// <param name="indexUI">Item slot in the selected legacy panel.</param>
    /// <param name="quantity">Quantity.</param>
    public void saleItem(int indexUI, int quantity)
    {
        GameCanvas.msgdlg.pleasewait();
        Message message = null;
        try
        {
            message = new Message((sbyte)14);
            message.writer().writeByte(indexUI);
            if (quantity > 1)
            {
                message.writer().writeShort(quantity);
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

    /// <summary>Sell item2.</summary>
    /// <param name="indexUI">Item slot in the selected legacy panel.</param>
    /// <param name="quantity">Quantity.</param>
    public void saleItem2(int indexUI, int quantity)
    {
        Message message = null;
        try
        {
            message = new Message((sbyte)14);
            message.writer().writeByte(indexUI);
            if (quantity > 1)
            {
                message.writer().writeShort(quantity);
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

    /// <summary>Buy item.</summary>
    /// <param name="typeUI">Legacy item-panel/shop category code.</param>
    /// <param name="indexUI">Item slot in the selected legacy panel.</param>
    /// <param name="quantity">Quantity.</param>
    public void buyItem(int typeUI, int indexUI, int quantity)
    {
        GameCanvas.msgdlg.pleasewait();
        Message message = null;
        try
        {
            message = new Message((sbyte)13);
            message.writer().writeByte(typeUI);
            message.writer().writeByte(indexUI);
            if (quantity > 1)
            {
                message.writer().writeShort(quantity);
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

    /// <summary>Use item change map.</summary>
    /// <param name="indexUI">Item slot in the selected legacy panel.</param>
    /// <param name="indexMenu">Index menu.</param>
    public void useItemChangeMap(int indexUI, int indexMenu)
    {
        Message message = null;
        try
        {
            message = new Message((sbyte)12);
            message.writer().writeByte(indexUI);
            message.writer().writeByte(indexMenu);
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

    /// <summary>Send the text box identifier request through the existing client protocol.</summary>
    /// <param name="menuId">Menu identifier.</param>
    /// <param name="str">Text.</param>
    public void textBoxId(short menuId, string str)
    {
        Message message = null;
        try
        {
            message = new Message((sbyte)92);
            message.writer().writeShort(menuId);
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

    /// <summary>Send the request item request through the existing client protocol.</summary>
    /// <param name="typeUI">Legacy item-panel/shop category code.</param>
    public void requestItem(int typeUI)
    {
        Message message = null;
        try
        {
            message = messageSubCommand(-103);
            message.writer().writeByte(typeUI);
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

    /// <summary>Send the bag sort request through the existing client protocol.</summary>
    public void bagSort()
    {
        Handle.bagshort = true;
        Message message = null;
        try
        {
            message = messageSubCommand(-107);
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

    /// <summary>Send the box sort request through the existing client protocol.</summary>
    public void boxSort()
    {
        Message message = null;
        try
        {
            message = messageSubCommand(-106);
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

    /// <summary>Send the box currency in request through the existing client protocol.</summary>
    /// <param name="coinIn">Currency in.</param>
    public void boxCoinIn(int coinIn)
    {
        Message message = null;
        try
        {
            message = messageSubCommand(-105);
            message.writer().writeInt(coinIn);
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

    /// <summary>Send the box currency out request through the existing client protocol.</summary>
    /// <param name="coinOut">Currency out.</param>
    public void boxCoinOut(int coinOut)
    {
        Message message = null;
        try
        {
            message = messageSubCommand(-104);
            message.writer().writeInt(coinOut);
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

    /// <summary>Send the crystal collect request through the existing client protocol.</summary>
    /// <param name="items">Items (milliseconds).</param>
    /// <remarks>Locals: i: Loop/collection index.</remarks>
    public void crystalCollect(Item[] items)
    {
        GameCanvas.msgdlg.pleasewait();
        Message message = null;
        try
        {
            message = new Message((sbyte)19);
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i] != null)
                {
                    message.writer().writeByte(items[i].indexUI);
                }
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

    /// <summary>Send the upgrade item request through the existing client protocol.</summary>
    /// <param name="item">Item instance being inspected or transferred.</param>
    /// <param name="items">Items (milliseconds).</param>
    /// <param name="isGold">Is gold.</param>
    /// <remarks>Locals: i: Loop/collection index.</remarks>
    public void upgradeItem(Item item, Item[] items, bool isGold)
    {
        GameCanvas.msgdlg.pleasewait();
        Message message = null;
        try
        {
            message = new Message((sbyte)21);
            message.writer().writeBoolean(isGold);
            message.writer().writeByte(item.indexUI);
            Debug.Log(item.template.name);
            Debug.Log(item.indexUI);
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i] != null)
                {
                    message.writer().writeByte(items[i].indexUI);
                }
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

    /// <summary>Send the crystal collect lock request through the existing client protocol.</summary>
    /// <param name="items">Items (milliseconds).</param>
    /// <remarks>Locals: i: Loop/collection index.</remarks>
    public void crystalCollectLock(Item[] items)
    {
        GameCanvas.msgdlg.pleasewait();
        Message message = null;
        try
        {
            message = new Message((sbyte)20);
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i] != null)
                {
                    message.writer().writeByte(items[i].indexUI);
                }
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

    /// <summary>Split item.</summary>
    /// <param name="item">Item instance being inspected or transferred.</param>
    public void splitItem(Item item)
    {
        GameCanvas.msgdlg.pleasewait();
        Message message = null;
        try
        {
            message = new Message((sbyte)22);
            message.writer().writeByte(item.indexUI);
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

    /// <summary>Send the trade item lock request through the existing client protocol.</summary>
    /// <param name="coin">Currency.</param>
    /// <param name="items">Items (milliseconds).</param>
    /// <remarks>Locals: num: Numeric intermediate used by this operation; the legacy name is retained; i: Loop/collection index; j: Nested loop/collection index.</remarks>
    public void tradeItemLock(int coin, Item[] items)
    {
        Message message = null;
        try
        {
            message = new Message((sbyte)45);
            message.writer().writeInt(coin);
            int num = 0;
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i] != null)
                {
                    num++;
                }
            }

            message.writer().writeByte(num);
            for (int j = 0; j < items.Length; j++)
            {
                if (items[j] != null)
                {
                    message.writer().writeByte(items[j].indexUI);
                }
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

    /// <summary>Pick up item.</summary>
    /// <param name="itemMapId">Item map identifier.</param>
    /// <remarks>Locals: i: Loop/collection index.</remarks>
    public void pickItem(int itemMapId)
    {
        Message message = null;
        ItemMap itemMap = null;
        for (int i = 0; i < GameScr.vItemMap.size(); i++)
        {
            itemMap = (ItemMap)GameScr.vItemMap.elementAt(i);
        }

        try
        {
            message = new Message((sbyte)(-14));
            message.writer().writeShort(itemMapId);
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

    /// <summary>Send the throw item request through the existing client protocol.</summary>
    /// <param name="index">Index.</param>
    public void throwItem(int index)
    {
        Message message = null;
        try
        {
            message = new Message((sbyte)(-12));
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

    /// <summary>Update item.</summary>
    public void updateItem()
    {
        Message message = null;
        try
        {
            message = messageNotMap(-119);
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

    /// <summary>Send the request clan item request through the existing client protocol.</summary>
    public void requestClanItem()
    {
        Message message = null;
        try
        {
            message = messageNotMap(-111);
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

    /// <summary>Convert upgrade.</summary>
    /// <param name="index1">Index1.</param>
    /// <param name="index2">Index2.</param>
    /// <param name="index3">Index3.</param>
    public void doConvertUpgrade(int index1, int index2, int index3)
    {
        Message message = null;
        try
        {
            message = messageNotMap(-88);
            message.writer().writeByte(index1);
            message.writer().writeByte(index2);
            message.writer().writeByte(index3);
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

    /// <summary>Unlock clan item.</summary>
    public void unlockClanItem()
    {
        Message message = null;
        try
        {
            message = messageNotMap(-62);
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

    /// <summary>Send the input num split request through the existing client protocol.</summary>
    /// <param name="indexItem">Index item.</param>
    /// <param name="numSplit">Num split.</param>
    /// <param name="box">Box.</param>
    public void inputNumSplit(int indexItem, int numSplit, bool box = false)
    {
        Message message = null;
        try
        {
            message = messageNotMap(-85);
            message.writer().writeByte(indexItem);
            message.writer().writeInt(numSplit);
            message.writer().writeBoolean(box);
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

    /// <summary>Send to sale item through the existing client protocol.</summary>
    /// <param name="item">Item instance being inspected or transferred.</param>
    /// <param name="value">Value.</param>
    public void sendToSaleItem(Item item, int value)
    {
        Message message = null;
        try
        {
            message = new Message((sbyte)102);
            message.writer().writeByte(item.indexUI);
            message.writer().writeInt(value);
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

    /// <summary>Send the request item auction request through the existing client protocol.</summary>
    /// <param name="itemId">Item identifier.</param>
    public void requestItemAuction(int itemId)
    {
        Message message = null;
        try
        {
            message = new Message((sbyte)104);
            message.writer().writeInt(itemId);
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

    /// <summary>Buy item auction.</summary>
    /// <param name="itemId">Item identifier.</param>
    public void buyItemAuction(int itemId)
    {
        Message message = null;
        try
        {
            message = new Message((sbyte)105);
            message.writer().writeInt(itemId);
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

    /// <summary>Send clan item through the existing client protocol.</summary>
    /// <param name="name">Name.</param>
    public void sendClanItem(string name)
    {
        Message message = null;
        try
        {
            message = messageNotMap(-61);
            message.writer().writeByte(GameScr.indexSelect);
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

    /// <summary>Use clan item.</summary>
    public void useClanItem()
    {
        Message message = null;
        try
        {
            message = messageNotMap(-60);
            message.writer().writeByte(GameScr.indexSelect);
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

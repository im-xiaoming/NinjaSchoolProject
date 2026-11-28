using System;
using System.IO;
using System.Text;
using System.Threading;
using UnityEngine;

namespace Functions.Auto
{
    public class Handle
    {
        public static int dem;
        public static int count;
        public static Thread GetThread;
        public static ThreadStart GetthreadStart;
        // T: T.
        public static Auto T;
        public static bool DkChay;
        public static TanSat tanSat;
        public static long timeAutouse;
        // w: Width or horizontal span.
        public static long w;
        // s: Source text or serialized input.
        public static MyVector s;
        public static long timeChangeZone;
        public static int demTask;
        public static AutoTask autoTask;
        public static AutoNVHN autoNVHN;
        public static Buff buff;
        public static int ccc;
        public static int CharIDSave;
        public static bool isHackGiay = true;
        public static int HackGiay = 8;
        public static MyVector vCharInMap;
        public static MyVector vItemMap;
        public static bool FirstZone;
        public static bool bagshort;
        public static int test;
        public static MyVector Item;
        public static long timeDo;
        public static bool acUse;
        public static long lastimeAutoDel = 0;
        public static int SpNextMap;
        public static short[] arrItemDell = new short[120];
        public static int NextMinMaxInt => new System.Random().Next(int.MinValue, int.MaxValue);

        /// <summary>Perform sleep.</summary>
        /// <param name="milisSeconds">Milis seconds (seconds).</param>
        public static void Sleep(int milisSeconds)
        {
            try
            {
                Thread.Sleep(milisSeconds);
            }
            catch (ArgumentOutOfRangeException)
            {
            }
        }

        /// <summary>Draw handle.</summary>
        /// <param name="text">Text fragment assembled or read by this operation.</param>
        public static void Paint(string text)
        {
            ChatPopup.addChatPopupMultiLine("[Xin chờ] " + text, 1000, Char.getMyChar());
        }

        /// <summary>Add item dell.</summary>
        /// <param name="var0">Compatibility argument retained under its original legacy name.</param>
        /// <remarks>Locals: var1: Intermediate value retained under its original legacy name; var2: Intermediate value retained under its original legacy name; var4: Intermediate value retained under its original legacy name; var3: Number of entries in the source collection.</remarks>
        public static void addItemDell(short var0)
        {
            int var1;
            for (var1 = 0; var1 < arrItemDell.Length; ++var1)
            {
                if (arrItemDell[var1] == var0)
                {
                    return;
                }
            }

            var1 = -1;
            for (int var2 = 0; var2 < arrItemDell.Length; ++var2)
            {
                if (arrItemDell[var2] < 0)
                {
                    var1 = var2;
                    break;
                }
            }

            if (var1 == -1)
            {
                var1 = arrItemDell.Length;
                short[] var4;
                Array.Copy(var4 = new short[arrItemDell.Length + 10], 0, arrItemDell, 0, arrItemDell.Length);
                for (int var3 = arrItemDell.Length; var3 < var4.Length; ++var3)
                {
                    var4[var3] = -1;
                }

                arrItemDell = var4;
            }

            arrItemDell[var1] = var0;
        }

        /// <summary>Remove item dell.</summary>
        /// <param name="var0">Compatibility argument retained under its original legacy name.</param>
        /// <remarks>Locals: var1: Intermediate value retained under its original legacy name.</remarks>
        public static void removeItemDell(short var0)
        {
            for (int var1 = 0; var1 < arrItemDell.Length; ++var1)
            {
                if (arrItemDell[var1] == var0)
                {
                    arrItemDell[var1] = -1;
                }
            }
        }

        /// <summary>Sort item del.</summary>
        /// <remarks>Locals: var0: Intermediate value retained under its original legacy name; var1: Intermediate value retained under its original legacy name.</remarks>
        public static void sortItemDel()
        {
            for (int var0 = 0; var0 < arrItemDell.Length; ++var0)
            {
                if (arrItemDell[var0] > 0)
                {
                    for (int var1 = 0; var1 <= var0; ++var1)
                    {
                        if (arrItemDell[var1] == -1)
                        {
                            arrItemDell[var1] = arrItemDell[var0];
                            arrItemDell[var0] = -1;
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>Perform go map.</summary>
        /// <param name="mapID">Map identifier.</param>
        public static void GoMap(int mapID)
        {
            new Thread((ThreadStart)delegate
            {
                GameCanvas.startWaitDlgWithoutCancel();
                try
                {
                    TileMap.GoMap(mapID);
                }
                catch (Exception value)
                {
                    Console.WriteLine(value);
                }

                GC.Collect();
                if (Session_ME.connecting)
                {
                    GameScr.gI().switchToMe();
                }

                GameCanvas.endDlg();
                GameCanvas.isLoading = false;
            }).Start();
        }

        /// <summary>Perform fixed.</summary>
        public static void Fixed()
        {
            dem = new System.Random().Next(0, 7);
            if (Auto.isDie(Char.getMyChar()) && (TileMap.IsDangGoMap || TileMap.MapMod != TileMap.mapID))
            {
                TileMap.HuyLockMap();
            }
        }

        /// <summary>Initialize handle state and its supplied dependencies.</summary>
        /// <remarks>Locals: i: Loop/collection index.</remarks>
        static Handle()
        {
            Item = new MyVector();
            SpNextMap = 500;
            test = 23123213;
            buff = new Buff();
            autoTask = new AutoTask();
            autoNVHN = new AutoNVHN();
            tanSat = new TanSat();
            s = new MyVector();
            for (int i = 0; i < arrItemDell.Length; ++i)
            {
                arrItemDell[i] = -1;
            }
        }

        /// <summary>Perform next map.</summary>
        /// <param name="way">Way.</param>
        public static void NextMap(int way)
        {
            new Thread((ThreadStart)delegate
            {
                GameCanvas.startWaitDlgWithoutCancel();
                try
                {
                    TileMap.NextMap(way);
                }
                catch (Exception value)
                {
                    Console.WriteLine(value);
                }

                GC.Collect();
                if (Session_ME.connecting)
                {
                    GameScr.gI().switchToMe();
                }

                GameCanvas.endDlg();
                GameCanvas.isLoading = false;
            }).Start();
        }

        /// <summary>Run handle.</summary>
        /// <remarks>Locals: num: Numeric intermediate used by this operation; the legacy name is retained; var3: Client character being inspected or controlled; var9: Intermediate value retained under its original legacy name; var7: Intermediate value retained under its original legacy name; var5: Intermediate value retained under its original legacy name; var10: Intermediate value retained under its original legacy name; var27: Intermediate value retained under its original legacy name; num2: Numeric intermediate used by this operation; the legacy name is retained.</remarks>
        public static void Run()
        {
            try
            {
                while (DkChay)
                {
                    long num = mSystem.currentTimeMillis();
                    try
                    {
                        Char var3 = Char.getMyChar();
                        if (T != null)
                        {
                            T.Run();
                        }

                        if (mSystem.currentTimeMillis() - GameScr.lastimeAutouse > 2000)
                        {
                            int var9;
                            int var7;
                            int var5;
                            Effect var10;
                            for (var9 = 0; var9 < GameScr.ItemAutoUse.size(); ++var9)
                            {
                                var7 = (int)GameScr.ItemAutoUse.elementAt(var9);
                                ItemTemplate var27 = ItemTemplates.get((short)(var7));
                                if (Char.checkItem(var7))
                                {
                                    bool effectExists = false;
                                    for (var5 = 0; var5 < var3.vEff.size(); ++var5)
                                    {
                                        if ((var10 = (Effect)var3.vEff.elementAt(var5)) != null && var10.template.iconId == var27.iconID)
                                        {
                                            effectExists = true;
                                            break;
                                        }
                                    }

                                    if (!effectExists && (var5 = Char.checkIndexBag(var7)) >= 0)
                                    {
                                        Service.gI().useItem(var5);
                                        continue;
                                    }
                                }
                                else
                                {
                                    GameScr.ItemAutoUse.removeElementAt(var9);
                                    --var9;
                                }
                            }

                            GameScr.lastimeAutouse = mSystem.currentTimeMillis();
                        }
                    }
                    catch (Exception)
                    {
                    }

                    long num2 = mSystem.currentTimeMillis() - num;
                    if (mSystem.currentTimeMillis() - num < 100)
                    {
                        Thread.Sleep((int)(100 - num2));
                    }
                    else
                    {
                        Thread.Sleep(0);
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>Perform switch to me.</summary>
        public static void SwitchToMe()
        {
            if (!DkChay)
            {
                DkChay = true;
                GetthreadStart = Run;
                GetThread = new Thread(GetthreadStart);
                GetThread.Start();
            }

            CharIDSave = Char.getMyChar().charID;
        }

        /// <summary>Abort handle.</summary>
        public static void Abort()
        {
            if (T != null)
            {
                T = null;
            }

            if (GetThread != null)
            {
                GetThread.Abort();
                GetThread = null;
                if (GetthreadStart != null)
                {
                    GetthreadStart = null;
                }
            }

            GetthreadStart = Run;
            GetThread = new Thread(GetthreadStart);
            GetThread.Start();
        }

        /// <summary>Stop handle.</summary>
        /// <remarks>Locals: t: Current entry or time sample.</remarks>
        public static void Stop()
        {
            Auto t = T;
            DkChay = false;
            if (T != null)
            {
                T = null;
            }

            if (GetThread != null)
            {
                GetThread.Abort();
                GetThread = null;
                if (GetthreadStart != null)
                {
                    GetthreadStart = null;
                }
            }

            if (GetThread != null)
            {
                GetThread.Abort();
                GetThread = null;
                if (GetthreadStart != null)
                {
                    GetthreadStart = null;
                }
            }

            T = t;
        }

        /// <summary>Activate the supplied automation mode while retaining the previous mode.</summary>
        /// <param name="auto">Auto.</param>
        public static void S(Auto auto)
        {
            auto.T = T;
            T = auto;
        }

        /// <summary>Compute bool item bag identifier.</summary>
        /// <param name="paramInt">Compatibility argument retained under its original legacy name.</param>
        /// <remarks>Locals: i: Loop/collection index.</remarks>
        public static bool boolItemBagId(int paramInt)
        {
            Item[] arrItemBag = Char.getMyChar().arrItemBag;
            for (int i = 0; i < arrItemBag.Length; i++)
            {
                if (arrItemBag[i] != null && arrItemBag[i].template.id == paramInt)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>Perform die.</summary>
        /// <remarks>Locals: npc: Non-player character.</remarks>
        public static void Die()
        {
            Npc npc = Char.FindNpc(13);
            if (TileMap.mapID == 20)
            {
                Char.Move(TileMap.pxw, TileMap.pxh);
            }
            else if (npc == null)
            {
                if (!boolItemBagId(37) && !boolItemBagId(35))
                {
                    Char.Move(Char.getMyChar().cx, YDirt(Char.getMyChar().cx, Char.getMyChar().cy));
                    Service.gI().openUIZone();
                }
                else
                {
                    Char.Move(TileMap.pxw, TileMap.pxh);
                }
            }
            else if (Math.abs(npc.cx - Char.getMyChar().cx) <= 200 && Math.abs(npc.cy - Char.getMyChar().cy) <= 200)
            {
                if (!boolItemBagId(37) && !boolItemBagId(35))
                {
                    if (npc.cx < Char.getMyChar().cx)
                    {
                        Char.Move((Char.getMyChar().cx + 200 >= TileMap.pxw) ? (Char.getMyChar().cx - 200 - Math.abs(Char.getMyChar().cx - npc.cx)) : (Char.getMyChar().cx + 200), YDirt(Char.getMyChar().cx + 200, Char.getMyChar().cy));
                    }
                    else
                    {
                        Char.Move((Char.getMyChar().cx - 200 <= 0) ? (Char.getMyChar().cx + 200 + Math.abs(Char.getMyChar().cx - npc.cx)) : (Char.getMyChar().cx - 200), YDirt(Char.getMyChar().cx + 200, Char.getMyChar().cy));
                    }

                    Char.Move(Char.getMyChar().cx, YDirt(Char.getMyChar().cx, Char.getMyChar().cy));
                    Service.gI().openUIZone();
                }
                else
                {
                    Char.Move(Char.getMyChar().cx, TileMap.pxh);
                }
            }
            else if (!boolItemBagId(37) && !boolItemBagId(35))
            {
                Char.Move(Char.getMyChar().cx, YDirt(Char.getMyChar().cx, Char.getMyChar().cy));
                Service.gI().openUIZone();
            }
            else
            {
                Char.Move(Char.getMyChar().cx, TileMap.pxh);
            }
        }

        /// <summary>Compute y dirt.</summary>
        /// <param name="cx">Character horizontal position in map pixels.</param>
        /// <param name="cy">Character vertical position in map pixels.</param>
        /// <remarks>Locals: i: Loop/collection index; num: Numeric intermediate used by this operation; the legacy name is retained; j: Nested loop/collection index; num2: Numeric intermediate used by this operation; the legacy name is retained.</remarks>
        public static int YDirt(int cx, int cy)
        {
            cy = TileMap.tileYofPixel(cy);
            for (int i = 0; i < 240; i++)
            {
                int num = cy + i * 12;
                if (TileMap.tileTypeAt(cx, num, 2))
                {
                    return num;
                }
            }

            for (int j = 0; j < 240; j++)
            {
                int num2 = cy + j * 12;
                if (TileMap.tileTypeAt(cx, num2, 2))
                {
                    return num2;
                }
            }

            return cy;
        }

        /// <summary>Retain the update compatibility hook without performing work.</summary>
        public static void Update()
        {
        }

        /// <summary>Configure the monster-hunting mode and make it the active automation.</summary>
        /// <param name="templateId">Static template identifier, not an item-instance identifier.</param>
        /// <param name="mobId">Monster identifier in the current zone.</param>
        public static void TS(int templateId, int mobId = -1)
        {
            tanSat.update(templateId, mobId);
            S(tanSat);
        }

        /// <summary>Perform atnvhn.</summary>
        public static void ATNVHN()
        {
            autoNVHN.update();
            S(autoNVHN);
        }

        /// <summary>Compute to boolean.</summary>
        /// <param name="value">Value.</param>
        public static bool ToBoolean(int value)
        {
            return value.Equals(0);
        }

        /// <summary>Compute to int32.</summary>
        /// <param name="value">Value.</param>
        public static int ToInt32(bool value)
        {
            if (value.Equals(obj: true))
            {
                return 0;
            }

            return 1;
        }

        /// <summary>Load auto.</summary>
        /// <remarks>Locals: i: Loop/collection index.</remarks>
        public static void LoadAuto()
        {
            sbyte[] autoData = RMS.loadRMS("NjAuto");
            if (autoData == null || autoData.Length == 0)
                return; // first run: nothing saved yet
            DataInputStream dataInputStream = new DataInputStream(autoData);
            if (dataInputStream != null)
            {
                try
                {
                    myReader dis = dataInputStream.r;
                    Char.isAHP = dis.readBoolean();
                    Char.aHpValue = dis.readInt();
                    Char.isAMP = dis.readBoolean();
                    Char.aMpValue = dis.readInt();
                    Char.isAFood = dis.readBoolean();
                    Char.aFoodValue = dis.readInt();
                    Char.isABuff = dis.readBoolean();
                    int lent = dis.readInt();
                    int i;
                    if (arrItemDell.Length < lent)
                    {
                        arrItemDell = new short[10 * (lent / 10 + 1)];
                    }

                    for (i = 0; i < lent; ++i)
                    {
                        arrItemDell[i] = dis.readShort();
                    }
                }
                catch (Exception e)
                {
                    Debug.Log("Load auto err " + e.StackTrace);
                }
                finally
                {
                    try
                    {
                        dataInputStream.close();
                    }
                    catch (Exception ex2)
                    {
                        Out.println(ex2.Message + ex2.StackTrace);
                    }
                }
            }

            bagshort = false;
        }

        /// <summary>Save auto.</summary>
        /// <remarks>Locals: i: Loop/collection index.</remarks>
        public static void SaveAuto()
        {
            timeChangeZone = 0L;
            DataOutputStream dos = new DataOutputStream();
            try
            {
                dos.writeBoolean(Char.isAHP);
                dos.writeInt(Char.aHpValue);
                dos.writeBoolean(Char.isAMP);
                dos.writeInt(Char.aMpValue);
                dos.writeBoolean(Char.isAFood);
                dos.writeInt(Char.aFoodValue);
                dos.writeBoolean(Char.isABuff);
                int lent = 0;
                int i;
                for (i = 0; i < arrItemDell.Length; ++i)
                {
                    if (arrItemDell[i] >= 0)
                    {
                        ++lent;
                    }
                }

                dos.writeInt(lent);
                for (i = 0; i < arrItemDell.Length; ++i)
                {
                    if (arrItemDell[i] >= 0)
                    {
                        dos.writeShort(arrItemDell[i]);
                    }
                }

                RMS.saveRMS("NjAuto", dos.toByteArray());
            }
            catch (Exception e)
            {
                Debug.LogError("Save auto err " + e.Message);
            }
            finally
            {
                try
                {
                    dos.close();
                }
                catch (Exception ex2)
                {
                    Out.println(ex2.Message + ex2.StackTrace);
                }
            }

            string path = GameStorage.PersistentRoot + "/buffmap";
            string path2 = GameStorage.PersistentRoot + "/buffzone";
            File.WriteAllText(path, string.Concat(-2));
            File.WriteAllText(path2, string.Concat(-2));
        }

        /// <summary>Perform atc auto use.</summary>
        /// <param name="item">Item instance being inspected or transferred.</param>
        /// <remarks>Locals: var1: Intermediate value retained under its original legacy name.</remarks>
        public static void atcAutoUse(Item item)
        {
            try
            {
                int var1 = 0;
                do
                {
                    if (!(GameCanvas.currentScreen is GameScr))
                    {
                        return;
                    }

                    if (mSystem.currentTimeMillis() - timeAutouse >= 50)
                    {
                        Service.gI().useItem(item.indexUI);
                        Thread.Sleep(50);
                        timeAutouse = mSystem.currentTimeMillis();
                        var1++;
                        if ((var1 >= item.quantity))
                        {
                            break;
                        }
                    }
                }
                while (Char.solg_hanhtrang() > 0);
            }
            catch (Exception e)
            {
            }
        }
    }
}

using System;
using System.Threading;

namespace Functions.Auto
{
    public class AutoNVHN : Auto
    {
        public static bool start;
        private static String[] textNpc;
        private static int countNv;
        private TaskOrder taskOder;
        public static object objTask;
        public static bool isTask;
        public static object objSleep;
        /// <summary>Initialize auto nvhn state and its supplied dependencies.</summary>
        static AutoNVHN()
        {
            objTask = new object ();
            objSleep = new object ();
            init();
        }

        /// <summary>Initialize auto nvhn.</summary>
        public static void init()
        {
            start = false;
            textNpc = new String[]
            {
                "Hôm nay con đã làm hết nhiệm vụ ta giao. Hãy quay lại vào ngày hôm sau.",
                "Đây là lần nhận nhiệm vụ thứ ",
                " trong ngày hôm nay. Mỗi ngày được nhận tối đa 20 lần con nhé."
            };
        }

        /// <summary>Check text.</summary>
        /// <param name="var0">Compatibility argument retained under its original legacy name.</param>
        /// <remarks>Locals: var1: Entry selected from the source collection; var2: Entry selected from the source collection.</remarks>
        public static void checkText(String var0)
        {
            UnityEngine.Debug.LogError("Check text " + var0);
            // Kiểm tra chuỗi đầu tiên
            if (var0.Equals(textNpc[0]))
            {
                countNv = 21;
                Clock.CancelLock();
            }
            else
            {
                // Tìm vị trí bắt đầu của phần chuỗi "Đây là lần nhận nhiệm vụ thứ "
                int var1 = var0.IndexOf(textNpc[1]);
                if (var1 >= 0)
                {
                    // Tìm vị trí kết thúc của phần chuỗi " trong ngày hôm nay."
                    int var2 = var0.IndexOf(textNpc[2], var1);
                    if (var2 >= 0)
                    {
                        // Lấy chuỗi số lần nhận nhiệm vụ
                        string taskCountStr = var0.Substring(var1 + textNpc[1].Length, var2 - (var1 + textNpc[1].Length)).Trim();
                        try
                        {
                            // Chuyển chuỗi thành số nguyên
                            countNv = int.Parse(taskCountStr);
                            return;
                        }
                        catch (Exception e)
                        {
                            UnityEngine.Debug.LogError("Error parsing task count: " + e.Message);
                        }
                    }
                }
            }
        }

        /// <summary>Advance this object&#x27;s state for the current client logic tick.</summary>
        public void update()
        {
            countNv = 0;
            this.taskOder = Char.FindTask(0);
            this.Update();
        }

        /// <summary>Return task.</summary>
        public void getTask()
        {
            this.taskOder = Char.FindTask(0);
        }

        /// <summary>Pick up auto nvhn.</summary>
        /// <param name="npcId">NPC template identifier.</param>
        /// <param name="menuId">Menu identifier.</param>
        /// <param name="optionId">Option identifier.</param>
        /// <remarks>Locals: npc: Non-player character.</remarks>
        public void Pick(int npcId, int menuId, int optionId = 0)
        {
            Npc npc = Char.FindNpc(npcId);
            if (npc != null)
            {
                Char.Move(npc.cx, npc.cy);
                Service.gI().menu(-1, npcId, menuId, optionId);
            }
        }

        /// <summary>Run auto nvhn.</summary>
        public override void Run()
        {
            if (Auto.isDie(Char.getMyChar()))
            {
                Hoisinh(isWait: true);
                Sleep(500);
            }
            else
            {
                Default();
                Start();
            }
        }

        /// <summary>Initialize this component when Unity starts it.</summary>
        public void Start()
        {
            if (countNv <= 20)
            {
                if (TileMap.isTruong(TileMap.mapID))
                {
                    if (this.taskOder == null)
                    {
                        Handle.Paint("Nhận NV " + (countNv + 1) + "/20");
                        Pick(25, 1, 0);
                        Clock.Lock();
                        this.taskOder = Char.FindTask(0);
                        return;
                    }

                    if (this.taskOder.count >= this.taskOder.maxCount)
                    {
                        if (Char.checkBag() <= 0)
                        {
                            Handle.Paint("Hành trang đầy");
                            return;
                        }

                        Handle.Paint("Hoàn thành NV " + countNv + "/20");
                        Pick(25, 1, 2);
                        this.taskOder = null;
                        return;
                    }

                    Handle.Paint("Đi làm NV " + countNv + "/20");
                    Pick(25, 1, 3);
                    TileMap.LockMap();
                    this.Go(this.zoneID);
                    return;
                }

                if (this.taskOder != null)
                {
                    if (TileMap.mapID == this.taskOder.mapId)
                    {
                        if (this.taskOder.count >= this.taskOder.maxCount)
                        {
                            Handle.Die();
                            return;
                        }

                        this.Attack(this.taskOder.killId, -1);
                        if (start)
                        {
                            Handle.Paint("Nhiệm vụ " + countNv + "/20:" + this.taskOder.count + "/" + this.taskOder.maxCount + " " + Mob.arrMobTemplate[this.taskOder.killId].name);
                            start = false;
                        }
                    }
                }
            }
            else
            {
                Handle.Paint("Hoàn thành!");
                Handle.Abort();
            }
        }

        /// <summary>Perform sleep.</summary>
        /// <param name="milisSeconds">Milis seconds (seconds).</param>
        public static void Sleep(int milisSeconds = 1000)
        {
            lock (objSleep)
            {
                Monitor.Wait(objSleep, milisSeconds);
            }
        }

        /// <summary>Return the text representation used by this type.</summary>
        public override string ToString()
        {
            return "Auto Nhiệm vụ hằng ngày: " + countNv + "/20";
        }
    }
}

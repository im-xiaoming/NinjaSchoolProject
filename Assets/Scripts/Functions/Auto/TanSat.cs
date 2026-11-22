namespace Functions.Auto
{
    public class TanSat : Auto
    {
        public int templateId;
        public int mobId;
        /// <summary>Run tan sat.</summary>
        public override void Run()
        {
            if (Auto.isDie(Char.getMyChar()))
            {
                Hoisinh(isWait: true);
                return;
            }

            if (TileMap.mapID != mapID || TileMap.zoneID != zoneID) // reMap removed; was always on
            {
                Go(mapID, zoneID);
                return;
            }

            Default();
            PickDa();
            PickHpMp();
            PickNlsk();
            Attack(templateId, mobId);
        }

        /// <summary>Return the text representation used by this type.</summary>
        public override string ToString()
        {
            if (templateId == -1)
            {
                return "Tàn sát + HS lượng: " + (GameScr.HoiSinhLuong ? "Bật" : "Tắt");
            }

            return "Tàn sát: " + Mob.MobName(templateId) + " + HS lượng: " + (GameScr.HoiSinhLuong ? "Bật" : "Tắt");
        }

        /// <summary>Advance this object&#x27;s state for the current client logic tick.</summary>
        /// <param name="temlpateId">Temlpate identifier.</param>
        /// <param name="mobId">Monster identifier in the current zone.</param>
        public void update(int temlpateId, int mobId = -1)
        {
            Update();
            zoneID = -1;
            mapID = TileMap.mapID;
            templateId = temlpateId;
            this.mobId = mobId;
        }
    }
}

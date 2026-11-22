using System;
using NsoServer.World;

namespace NsoServer.Net
{
    using Math = System.Math; // the client has its own global Math class
    using Zone = NsoServer.World.Zone;

    /// <summary>
    /// Characters seeing each other in a zone (the multiplayer part of Zone / AbsService that the single-player port left
    /// out): when a character joins a zone the others get PLAYER_ADD (AbsService.playerAdd + charInfo) and it gets theirs;
    /// moves (PLAYER_MOVE), deaths (PLAYER_DIE), revivals and leaving (PLAYER_REMOVE) are sent to the rest of the zone.
    /// Added for the former offline AI players (removed 2026-11-23); kept because it ports Java Zone behaviour.
    /// Call under GameWorld.SyncRoot (every caller is a message handler or the server tick).
    /// </summary>
    public static class Presence
    {
        /// <summary>Zone.join: tell the zone about the newcomer and the newcomer about the zone.</summary>
        /// <param name="me">Authoritative runtime character state.</param>
        /// <remarks>Locals: p: Authoritative runtime character state.</remarks>
        public static void Joined(PlayerState me)
        {
            if (me == null || me.Zone == null || me.Session == null)
                return;
            var info = CharInfo(me);
            foreach (PlayerState p in me.Zone.Players)
            {
                if (p == me || p.Session == null)
                    continue;
                p.Session.Send(info);
                me.Session.Send(CharInfo(p));
            }
        }

        /// <summary>Zone.out: the character leaves <paramref name = "zone"/> (map change, logout).</summary>
        /// <param name="me">Authoritative runtime character state.</param>
        /// <param name="zone">Map zone containing the operation&#x27;s actors.</param>
        /// <remarks>Locals: rm: Outgoing protocol message.</remarks>
        public static void Left(PlayerState me, Zone zone)
        {
            if (me == null || zone == null || me.Record == null)
                return;
            var rm = new Message(Cmd.PLAYER_REMOVE);
            rm.Writer.WriteInt(me.Record.Id);
            ToOthers(me, zone, rm);
        }

        /// <summary>Zone.move: the new position for everyone else in the zone.</summary>
        /// <param name="me">Authoritative runtime character state.</param>
        /// <remarks>Locals: m: Outgoing protocol message.</remarks>
        public static void Moved(PlayerState me)
        {
            if (me == null || me.Zone == null || me.Zone.Players.Count < 2)
                return;
            var m = new Message(Cmd.PLAYER_MOVE);
            m.Writer.WriteInt(me.Record.Id);
            m.Writer.WriteShort(me.X);
            m.Writer.WriteShort(me.Y);
            ToOthers(me, me.Zone, m);
        }

        /// <summary>MapService.waitToDie seen by the others: the character lies on the ground.</summary>
        /// <param name="me">Authoritative runtime character state.</param>
        /// <remarks>Locals: m: Outgoing protocol message.</remarks>
        public static void Died(PlayerState me)
        {
            if (me == null || me.Zone == null || me.Zone.Players.Count < 2)
                return;
            var m = new Message(Cmd.PLAYER_DIE);
            m.Writer.WriteInt(me.Record.Id);
            m.Writer.WriteByte(me.TypePk);
            m.Writer.WriteShort(me.X);
            m.Writer.WriteShort(me.Y);
            ToOthers(me, me.Zone, m);
        }

        /// <summary>Revived on the spot: the others redraw the character standing (remove + add).</summary>
        /// <param name="me">Authoritative runtime character state.</param>
        public static void Revived(PlayerState me)
        {
            if (me == null || me.Zone == null || me.Zone.Players.Count < 2)
                return;
            Left(me, me.Zone);
            var info = CharInfo(me);
            ToOthers(me, me.Zone, info);
        }

        /// <summary>MapService.chat (CHAT_MAP): a speech bubble over the character for the others in the zone.</summary>
        /// <param name="me">Authoritative runtime character state.</param>
        /// <param name="text">Text fragment assembled or read by this operation.</param>
        /// <remarks>Locals: m: Outgoing protocol message.</remarks>
        public static void Say(PlayerState me, string text)
        {
            if (me == null || me.Zone == null || string.IsNullOrEmpty(text))
                return;
            text = text.Replace('\n', ' ').Trim();
            if (text.Length > 80)
                text = text.Substring(0, 80);
            var m = new Message(Cmd.CHAT_MAP);
            m.Writer.WriteInt(me.Record.Id);
            m.Writer.WriteUTF(text);
            ToOthers(me, me.Zone, m);
        }

        /// <summary>Perform to others.</summary>
        /// <param name="me">Authoritative runtime character state.</param>
        /// <param name="zone">Map zone containing the operation&#x27;s actors.</param>
        /// <param name="m">Framed protocol message.</param>
        /// <remarks>Locals: p: Authoritative runtime character state.</remarks>
        private static void ToOthers(PlayerState me, Zone zone, Message m)
        {
            foreach (PlayerState p in zone.Players)
                if (p != me && p.Session != null)
                    p.Session.Send(m);
        }

        /// <summary>AbsService.playerAdd + Char.charInfo: who the character is and how it looks
        /// (the parts are worked out as in EnterWorld.LoadAll).</summary>
        /// <param name="p">Authoritative runtime character state.</param>
        /// <remarks>Locals: t: Identifier taken from the selected record; m: Outgoing protocol message; w: Width or horizontal span; i: Loop/collection index.</remarks>
        public static Message CharInfo(PlayerState p)
        {
            var world = p.Session != null ? p.Session.Server.World : null;
            var rec = p.Record;
            short head = rec.Head, weapon = 15, body = (short)(rec.Gender == 0 ? 10 : 1), leg = (short)(rec.Gender == 0 ? 9 : 0);
            if (world != null && p.Inventory != null)
                foreach (var item in p.Inventory.Equipment)
                {
                    if (item == null)
                        continue;
                    var t = world.FindItem(item.Id);
                    if (t == null)
                        continue;
                    switch (t.Type)
                    {
                        case 0:
                            head = t.Part;
                            break;
                        case 1:
                            weapon = t.Fashion >= 0 ? (short)-1 : t.Part;
                            break;
                        case 2:
                            body = t.Part;
                            break;
                        case 6:
                            leg = t.Part;
                            break;
                    }
                }

            var m = new Message(Cmd.PLAYER_ADD);
            var w = m.Writer;
            w.WriteInt(rec.Id);
            w.WriteUTF(""); // no clan
            w.WriteBoolean(p.IsInvisible);
            w.WriteByte(p.TypePk);
            w.WriteByte(rec.ClassId);
            w.WriteByte(rec.Gender);
            w.WriteShort(rec.Head);
            w.WriteUTF(rec.Name);
            w.WriteInt(p.IsDead ? 0 : Math.Max(1, p.Hp));
            w.WriteInt(p.MaxHp);
            w.WriteByte(Math.Min(127, p.Level));
            w.WriteShort(weapon);
            w.WriteShort(body);
            w.WriteShort(leg);
            w.WriteByte(-1); // mobMe: no pets offline
            w.WriteShort(p.X);
            w.WriteShort(p.Y);
            w.WriteShort(0); // eff5buffhp
            w.WriteShort(0); // eff5buffmp
            w.WriteByte(0); // effects (not shown to others offline)
            w.WriteBoolean(true); // isHuman
            w.WriteBoolean(false); // isNhanBan
            w.WriteShort(head);
            w.WriteShort(weapon);
            w.WriteShort(body);
            w.WriteShort(leg);
            for (int i = 0; i < 10; i++)
                w.WriteShort(-1); // fashion
            return m;
        }
    }
}

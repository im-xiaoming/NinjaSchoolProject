using System;
using NsoServer.World;

namespace NsoServer.Net
{
    using Math = System.Math; // the client has its own global Math class

    // Buff skills (skill type 2) used on oneself: port of Char.useSkillBuff (cmd USE_SKILL_MY_BUFF 74) for the buffs a
    // character can learn up to level 39, and of the EffectManager flags they set (isEffBong, isShieldMana, isMiss,
    // isInvisible, isHide, incrDame2), applied where Java reads them (Char.attackMonster, Mob.attack / target choice).
    //
    // Not ported:
    //   - Aoiame (22, summons a dummy) and the clone (67-72) are not ported: a message says so.
    public static class PlayerBuff
    {
        public const int CmdUseSkillMyBuff = 74;
        // effect types (EffectTemplate.type) read by combat
        public const int TypeBurnHit = 5, TypeManaShield = 6, TypeDodge = 7, TypeInvisible = 11, TypeHide = 12, TypeDamageUp = 13;
        /// <summary>Return the current timestamp used by this module&#x27;s timers.</summary>
        private static long Now()
        {
            return DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        }

        /// <summary>Compute param through the authoritative player rules.</summary>
        /// <param name="s">Source text or serialized input.</param>
        /// <param name="i">Loop/collection index.</param>
        private static int Param(SkillData s, int i)
        {
            return s.OptionParams != null && i < s.OptionParams.Length ? s.OptionParams[i] : 0;
        }

        // Char.useSkillBuff(Message): direction, then the selected skill
        /// <summary>Use player buff through the authoritative player rules.</summary>
        /// <param name="m">Framed protocol message.</param>
        /// <param name="me">Authoritative runtime character state.</param>
        /// <param name="world">Authoritative game world.</param>
        /// <remarks>Locals: tpl: Static template record.</remarks>
        public static void Use(Message m, PlayerState me, GameWorld world)
        {
            int dir = m.Reader.ReadSByte();
            var skill = me.SelectedSkill;
            if (skill == null || me.IsDead)
                return;
            var tpl = world.FindSkillTemplate(skill.TemplateId);
            if (tpl == null || tpl.Type != PlayerSkills.TypeBuff)
                return;
            long now = Now();
            if (now - me.LastTimeUseSkill < skill.CoolDown || me.Mp < skill.ManaUse)
                return;
            switch (skill.TemplateId)
            {
                case 6:
                    Pay(me, skill, now);
                    PlayerEffects.SetEffect(me, world, 15, Param(skill, 0) * 1000L, 0);
                    break; // invisible
                case 15:
                    Pay(me, skill, now);
                    PlayerEffects.SetEffect(me, world, 16, 5000, Param(skill, 0));
                    break; // hide
                case 13:
                    Pay(me, skill, now);
                    PlayerEffects.SetEffect(me, world, 9, 30000, me.Damage * Param(skill, 0) / 100);
                    break; // fire hits
                case 31:
                    Pay(me, skill, now);
                    PlayerEffects.SetEffect(me, world, 10, 90000, Param(skill, 0));
                    break; // mana shield
                case 33:
                    Pay(me, skill, now);
                    PlayerEffects.SetEffect(me, world, 17, 5000, Param(skill, 0));
                    PlayerCombat.SetAbility(me, world);
                    break; // incrDame2
                case 58:
                    Pay(me, skill, now);
                    PlayerEffects.SetEffect(me, world, 11, Param(skill, 0), 15000);
                    break; // dodge (Java: param as time)
                default:
                    PlayerService.ServerMessage(me, "Bản offline: chiêu này chưa được hỗ trợ.");
                    break;
            }
        }

        /// <summary>Perform pay through the authoritative player rules.</summary>
        /// <param name="me">Authoritative runtime character state.</param>
        /// <param name="skill">Skill.</param>
        /// <param name="now">Current timestamp used by this module&#x27;s timers.</param>
        private static void Pay(PlayerState me, SkillData skill, long now)
        {
            me.LastTimeUseSkill = now;
            me.Mp -= skill.ManaUse;
            PlayerService.UpdateMp(me);
        }

        /// <summary>Find player buff through the authoritative player rules.</summary>
        /// <param name="me">Authoritative runtime character state.</param>
        /// <param name="type">Type.</param>
        /// <remarks>Locals: e: Event/error or current entry.</remarks>
        public static PlayerEffect Find(PlayerState me, int type)
        {
            foreach (var e in me.Effects)
                if (e.Type == type)
                    return e;
            return null;
        }

        /// <summary>Remove player buff through the authoritative player rules.</summary>
        /// <param name="me">Authoritative runtime character state.</param>
        /// <param name="e">Event/error or current entry.</param>
        public static void Remove(PlayerState me, PlayerEffect e)
        {
            me.Effects.Remove(e);
            PlayerService.RemoveEffect(me, e);
        }

        /// <summary>EffectManager type 13: Char.incrDame2 while the effect lasts.</summary>
        /// <param name="me">Authoritative runtime character state.</param>
        /// <remarks>Locals: e: Event/error or current entry.</remarks>
        public static int IncrDame2(PlayerState me)
        {
            var e = Find(me, TypeDamageUp);
            return e == null ? 0 : e.Param;
        }

        /// <summary>Char.isInvisible(): isInvisible || isHide. Monsters do not pick such a player (Mob target choice).</summary>
        /// <param name="me">Authoritative runtime character state.</param>
        public static bool IsHidden(PlayerState me)
        {
            return me.IsInvisible || Find(me, TypeInvisible) != null || Find(me, TypeHide) != null;
        }
    }
}

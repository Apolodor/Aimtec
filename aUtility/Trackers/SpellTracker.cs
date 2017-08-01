namespace aUtility
{
    using System;
    using System.Drawing;
    using System.Linq;

    using Aimtec;

    internal class SpellTracker
    {
        private bool TrackAllies
        {
            get { return MenuClass.SpellTracker["allies"].Enabled; }
        }

        private bool TrackEnemies
        {
            get { return MenuClass.SpellTracker["enemies"].Enabled; }
        }

        public static SpellSlot[] SpellSlots =
        {
            SpellSlot.Q, SpellSlot.W, SpellSlot.E, SpellSlot.R
        };

        private static readonly SpellSlot[] SummonerSpellSlots =
        {
            SpellSlot.Summoner1, SpellSlot.Summoner2
        };

        public static int SpellXAdjustment(Obj_AI_Hero target)
        {
            return target.IsMe ? 55 : 10;
        }

        public static int SpellYAdjustment(Obj_AI_Hero target)
        {

            return target.IsMe ? 25 : 35;
        }

        public static int SummonerSpellXAdjustment(Obj_AI_Hero target)
        {
            return target.IsMe ? 2 : 10;
        }

        public static int SummonerSpellYAdjustment(Obj_AI_Hero target)
        {
            return target.IsMe ? -24 : -6;
        }

        public static string GetUnitSpellCooldown(Obj_AI_Hero unit, int spell)
        {
            var unitSpell = unit.SpellBook.GetSpell(SpellSlots[spell]);
            var cooldownRemaining = unitSpell.CooldownEnd - Game.ClockTime;
            if (cooldownRemaining > 0)
            {
                return ((int)cooldownRemaining).ToString();
            }
            if (unitSpell.State.HasFlag(SpellState.Disabled) ||
                unit.IsAlly && unitSpell.State.HasFlag(SpellState.Surpressed))
            {
                return "X";
            }
            if (unitSpell.State.HasFlag(SpellState.Unknown))
            {
                return "?";

            }

            return SpellSlots[spell].ToString();
        }

        public static string GetUnitSummonerSpellCooldown(Obj_AI_Hero unit, int summonerSpell)
        {
            var cooldownRemaining = unit.SpellBook.GetSpell(SummonerSpellSlots[summonerSpell]).CooldownEnd - Game.ClockTime;
            return cooldownRemaining > 0 ? ((int)cooldownRemaining).ToString() : "READY";
        }

        public static string GetUnitSummonerSpellFixedName(Obj_AI_Hero unit, int summonerSpell)
        {
            switch (unit.SpellBook.GetSpell(SummonerSpellSlots[summonerSpell]).Name.ToLower())
            {
                case "summonerflash": return "Flash";
                case "summonerdot": return "Ignite";
                case "summonerheal": return "Heal";
                case "summonerteleport": return "Teleport";
                case "summonerexhaust": return "Exhaust";
                case "summonerhaste": return "Ghost";
                case "summonerbarrier": return "Barrier";
                case "summonerboost": return "Cleanse";
                case "summonermana": return "Clarity";
                case "summonerclairvoyance": return "Clairvoyance";
                case "summonersnowball": return "Mark";
                default:
                    return "Smite";
            }
        }

        public SpellTracker()
        {
            foreach (var unit in
                ObjectManager.Get<Obj_AI_Hero>().Where(
                    e => Math.Abs(e.FloatingHealthBarPosition.X) > 0 && !e.IsDead && e.IsVisible &&
                         (e.IsAlly && !e.IsMe && TrackAllies ||
                          e.IsEnemy && TrackEnemies)))
            {
                if (unit.Name.Equals("Target Dummy"))
                {
                    return;
                }

                for (var spell = 0; spell < SpellSlots.Length; spell++)
                {
                    var xSpellOffset = (int)unit.FloatingHealthBarPosition.X + SpellXAdjustment(unit) + spell * 25;
                    var ySpellOffset = (int)unit.FloatingHealthBarPosition.Y + SpellYAdjustment(unit);

                    var spellCooldown = GetUnitSpellCooldown(unit, spell);

                    Render.Text(xSpellOffset, ySpellOffset, Color.White, spellCooldown);
                }

                for (var summonerSpell = 0; summonerSpell < SummonerSpellSlots.Length; summonerSpell++)
                {
                    var xSummonerSpellOffset = (int)unit.FloatingHealthBarPosition.X + SummonerSpellXAdjustment(unit) + summonerSpell * 88;
                    var ySummonerSpellOffset = (int)unit.FloatingHealthBarPosition.Y + SummonerSpellYAdjustment(unit);
                    var summonerSpellCooldown = GetUnitSummonerSpellFixedName(unit, summonerSpell) + ": " + GetUnitSummonerSpellCooldown(unit, summonerSpell);

                    Render.Text(xSummonerSpellOffset, ySummonerSpellOffset, Color.White, summonerSpellCooldown);
                }
            }
        }
    }
}








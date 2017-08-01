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

        private static readonly SpellSlot[] SummonersSlots =
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
            }
        }
    }
}
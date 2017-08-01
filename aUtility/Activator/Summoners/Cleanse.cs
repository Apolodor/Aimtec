namespace aUtility
{
    using System.Linq;

    using Aimtec;
    using Aimtec.SDK.Extensions;
    using Spell = Aimtec.SDK.Spell;

    internal class Cleanse
    {
        public static Obj_AI_Hero Player => ObjectManager.GetLocalPlayer();

        public Cleanse()
        {
            Game.OnUpdate += OnUpdate;
        }

        public static void OnUpdate()
        {
            if (Player.IsDead)
            {
                return;
            }

            var SummonerCleanse = Player.SpellBook.Spells.Where(o => o != null && o.SpellData != null).FirstOrDefault(o => o.SpellData.Name == "SummonerBoost");
            if (SummonerCleanse != null)
            {
                Spell Cleanse = new Spell(SummonerCleanse.Slot);
                if (Cleanse.Slot != SpellSlot.Unknown && MenuClass.Cleanse["usecleanse"].Enabled)
                {
                    if (Player.HasBuffOfType(BuffType.Stun) && MenuClass.WhitelistCleanse["BuffType.Stun"].Enabled ||
                        Player.HasBuffOfType(BuffType.Fear) && MenuClass.WhitelistCleanse["BuffType.Fear"].Enabled ||
                        Player.HasBuffOfType(BuffType.Flee) && MenuClass.WhitelistCleanse["BuffType.Flee"].Enabled ||
                        Player.HasBuffOfType(BuffType.Snare) && MenuClass.WhitelistCleanse["BuffType.Snare"].Enabled ||
                        Player.HasBuffOfType(BuffType.Taunt) && MenuClass.WhitelistCleanse["BuffType.Taunt"].Enabled ||
                        Player.HasBuffOfType(BuffType.Charm) && MenuClass.WhitelistCleanse["BuffType.Charm"].Enabled)
                    {
                        Cleanse.Cast();
                    }
                }
            }
        }
    }
}









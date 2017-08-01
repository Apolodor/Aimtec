namespace aUtility
{
    using System.Linq;

    using Aimtec;
    using Aimtec.SDK.Prediction.Health;
    using Spell = Aimtec.SDK.Spell;
    using Aimtec.SDK.Menu.Components;
    using Aimtec.SDK.Util.Cache;
    using Aimtec.SDK.Extensions;

    internal class Heal
    {
        public static Obj_AI_Hero Player => ObjectManager.GetLocalPlayer();

        public Heal()
        {
            Game.OnUpdate += Game_OnUpdate;
        }

        private void Game_OnUpdate()
        {
            if (Player.IsDead)
            {
                return;
            }

            var SummonerHeal = Player.SpellBook.Spells.Where(o => o != null && o.SpellData != null).FirstOrDefault(o => o.SpellData.Name == "SummonerHeal");
            if (SummonerHeal != null)
            {
                Spell Heal = new Spell(SummonerHeal.Slot);
                if (SummonerHeal.Slot != SpellSlot.Unknown &&
                    MenuClass.Heal["useheal"].Enabled)
                {
                    if (HealthPrediction.Implementation.GetPrediction(
                        Player, 250 + Game.Ping) <= Player.MaxHealth /
                        100 * MenuClass.Heal["healpercent"].Value)
                    {
                        Heal.Cast();
                    }
                }

                var Allies = GameObjects.AllyHeroes.Where(t => !t.IsMe && t.IsValidTarget(Heal.Range, true));
                foreach (var ally in Allies.Where(a =>
                MenuClass.WhitelistHealAllies["healwhitelist"][a.ChampionName.ToLower()].As<MenuBool>().Enabled &&
                MenuClass.Heal["usehealallies"].Enabled &&
                        a.CountEnemyHeroesInRange(Player.AttackRange) >= 1 &&
                        a.Health <= a.MaxHealth / 100 * MenuClass.Heal["healcustom"]["healpercent"].Value))
                {
                    Heal.Cast();
                }
            }
        }
    }
}

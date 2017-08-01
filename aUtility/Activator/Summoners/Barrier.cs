namespace aUtility
{
    using System.Linq;

    using Aimtec;
    using Aimtec.SDK.Prediction.Health;
    using Spell = Aimtec.SDK.Spell;

    internal class Barrier
    {
 
        private static Obj_AI_Hero Player => ObjectManager.GetLocalPlayer();

        public Barrier()
        {
            Game.OnUpdate += Game_OnUpdate;
        }

        public static void Game_OnUpdate()
        {
            if (Player.IsDead)
            {
                return;
            }

            var SummonerBarrier = Player.SpellBook.Spells.Where(o => o != null && o.SpellData != null).FirstOrDefault(o => o.SpellData.Name == "SummonerBarrier");
            if (SummonerBarrier != null)
            {
                Spell Barrier = new Spell(SummonerBarrier.Slot);
                if (Barrier.Slot != SpellSlot.Unknown && MenuClass.Barrier["usebarrier"].Enabled)
                {
                    if (HealthPrediction.Implementation.GetPrediction(Player, 250 + Game.Ping) <= Player.MaxHealth / 100 * MenuClass.Barrier["hpbarrier"].Value)
                    {
                        Barrier.Cast();
                    }
                }
            }
        }
    }
}
                    
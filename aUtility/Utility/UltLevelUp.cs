namespace aUtility
{
    using System;

    using Aimtec;
    using Aimtec.SDK.Util;

    internal class UltLevelUp
    {
        private bool EnableRLevelUp
        {
            get { return MenuClass.Menu["lvlupult"].Enabled; }
        }

        internal static Random Rand;

        public UltLevelUp()
        {

            Obj_AI_Base.OnLevelUp += Obj_AI_Base_OnLevelUp;
        }

        private void Obj_AI_Base_OnLevelUp(Obj_AI_Base sender, EventArgs args)
        {
            if (!EnableRLevelUp)
            {
                return;
            }

            var hero = sender as Obj_AI_Hero;
            if (hero == null
                || !hero.IsMe)
            {
                return;
            }

            if (hero.ChampionName == "Jayce"
                || hero.ChampionName == "Udyr"
                || hero.ChampionName == "Elise")
            {
                return;
            }

            switch (hero.Level)
            {
                case 6:
                    DelayAction.Queue(Rand.Next(250, 950) + Math.Max(30, Game.Ping),
                        () => { hero.SpellBook.LevelSpell(SpellSlot.R); });
                    break;
            }
        }
    }
}

          
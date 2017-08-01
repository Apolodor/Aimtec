namespace aUtility
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Aimtec;
    using Aimtec.SDK;
    using Aimtec.SDK.Prediction.Health;
    using Spell = Aimtec.SDK.Spell;
    using Aimtec.SDK.Extensions;
    using System.Drawing;
    using System.Net;
    using Aimtec.SDK.Damage;
    using Aimtec.SDK.Events;

    class Smite
    {
        public static Obj_AI_Hero Player => ObjectManager.GetLocalPlayer();

        private static int SmiteDamages
        {
            get
            {
                int[] Dmg = new int[] { 390, 410, 430, 450, 480, 510, 540, 570, 600, 640, 680, 720, 760, 800, 850, 900, 950, 1000 };

                return Dmg[Player.Level - 1];
            }
        }

        private static string[] pMobs = new string[] { "SRU_Baron", "SRU_RiftHerald" };
        private static string[] small = new string[] { "SRU_Blue", "SRU_Red" };

        public Smite()
        {
            Game.OnUpdate += OnUpdate;
        }
    
        private void OnUpdate()
        {
            var SummonerSmite = Player.SpellBook.Spells.Where(o => o != null && o.SpellData != null).FirstOrDefault(o => o.SpellData.Name.Contains("Smite"));
            if (SummonerSmite != null)
            {
                Spell Smite = new Spell(SummonerSmite.Slot, 560);
                if (Smite.Slot != SpellSlot.Unknown && MenuClass.Smite["usesmite"].Enabled)
                {
                    if (!MenuClass.Smite["smiteactive"].Enabled)
                    {
                        if (Render.WorldToScreen(Player.Position, out Vector2 coord) && MenuClass.Smite["statusdrawing"].Enabled)
                        {
                            coord.Y -= -30;
                            coord.X -= +35;
                            Render.Text(coord.X, coord.Y, Color.Red, "SMITE DISABLED.");
                        }
                        if (MenuClass.Smite["rangedrawing"].Enabled)
                        {
                            Render.Circle(Player.Position, Smite.Range, 30, Color.Red);
                        }
                    }
                    else
                    {
                        if (Smite.Ready)
                        {
                            if (Render.WorldToScreen(Player.Position, out Vector2 coord) && MenuClass.Smite["statusdrawing"].Enabled)
                            {
                                coord.Y -= -30;
                                coord.X -= +35;
                                Render.Text(coord.X, coord.Y, Color.Lime, "SMITE READY.");
                            }
                            if (MenuClass.Smite["rangedrawing"].Enabled)
                            {
                                Render.Circle(Player.Position, Smite.Range, 30, Color.Lime);
                            }
                            foreach (var Obj in ObjectManager.Get<Obj_AI_Minion>().Where(t => t.IsValidTarget(Smite.Range) && SmiteDamages >= t.Health))
                            {
                                if (Obj.UnitSkinName.Contains("Dragon"))
                                {
                                    if (MenuClass.Dragons[Obj.UnitSkinName].Enabled)
                                    {
                                        Smite.Cast(Obj);
                                    }
                                }
                                if (pMobs.Contains(Obj.UnitSkinName))
                                {
                                    if (MenuClass.EpicMonsters[Obj.UnitSkinName].Enabled)
                                    {
                                        Smite.Cast(Obj);
                                    }
                                }
                                if (small.Contains(Obj.UnitSkinName))
                                {
                                    if (MenuClass.Monsters[Obj.UnitSkinName].Enabled)
                                    {
                                        Smite.Cast(Obj);
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (Render.WorldToScreen(Player.Position, out Vector2 coord) && MenuClass.Smite["statusdrawing"].Enabled)
                            {
                                coord.Y -= -30;
                                coord.X -= +55;
                                Render.Text(coord.X, coord.Y, Color.DarkViolet, "SMITE ON COOLDOWN.");
                            }
                            if (MenuClass.Smite["rangedrawing"].Enabled)
                            {
                                Render.Circle(Player.Position, Smite.Range, 30, Color.DarkViolet);
                            }
                        }
                    }
                }
            }
        }
    }
}
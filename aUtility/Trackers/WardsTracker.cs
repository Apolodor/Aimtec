namespace aUtility
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;

    using Aimtec;
    using Aimtec.SDK.Extensions;

    internal class WardsTracker
    {

        private bool WardTrackerActive
        {
            get { return MenuClass.Menu["wardtracker"].Enabled; }
        }

        private float lastTick;

        private readonly Color greenWard = Color.LawnGreen;
        private readonly Color pinkWard = Color.Magenta;
        private readonly Color minimapUnderlay = Color.FromArgb(175, 0, 0, 0);

        private readonly HashSet<string> wardNames =
            new HashSet<string> { "SightWard", "VisionWard", "JammerDevice" };

        private readonly HashSet<string> trapNames =
            new HashSet<string> { "Noxious Trap", "Cupcake Trap", "Jack In The Box" };

        private readonly HashSet<string> wardSpells =
            new HashSet<string> { "TrinketTotemLvl1", "ItemGhostWard", "JammerDevice", "TrinketOrbLvl3" };

        private readonly List<Obj_AI_Minion> wards =
            new List<Obj_AI_Minion>();

        private readonly Dictionary<string, Func<Obj_AI_Base, int>> wardSpellToTimeResolveFunc =
            new Dictionary<string, Func<Obj_AI_Base, int>>
        {
            { "TrinketTotemLvl1", hero => (int)((hero.Level - 1) * 3.5f + 60.5f) },
            { "ItemGhostWard", hero => 150 },
            { "JammerDevice", hero => ushort.MaxValue },
        };

        private readonly List<ward> calculatedWard = new List<ward>();

        public WardsTracker()
        {
            GameObject.OnCreate += OnGameObjectCreated;
            GameObject.OnDestroy += OnGameObjectDestroyed;
            Obj_AI_Base.OnProcessSpellCast += OnProcessSpell;
            Render.OnRender += OnRender;
            Render.OnPresent += OnPresent;            
            Game.OnUpdate += OnUpdate;
        }

        private void OnPresent()
        {
            if (WardTrackerActive)
            {
                foreach (var ward in wards)
                    if (Render.WorldToMinimap(ward.Position, out Vector2 screenCoord))
                    {
                        Render.Line(screenCoord.X - 5, screenCoord.Y + 5, screenCoord.X + 5, screenCoord.Y + 5, 10, false, minimapUnderlay);
                        Render.Text(screenCoord.X - 2, screenCoord.Y - 2, ward.Name == "JammerDevice" ? pinkWard : greenWard, "x");
                    }

                foreach (var ward in calculatedWard )
                    if (Render.WorldToMinimap(ward.Position, out Vector2 screenCoord))
                    {
                        Render.Line(screenCoord.X - 5, screenCoord.Y + 5, screenCoord.X + 5, screenCoord.Y + 5, 10, false, minimapUnderlay);
                        Render.Text(screenCoord.X - 2, screenCoord.Y - 2, ward.Color, "x");
                    }
            }
        }

        private void OnUpdate()
        {
            var clockTime = Game.ClockTime;
            if (clockTime - lastTick > 1f)
            {
                lastTick = clockTime;

                for (var i = 0; i < calculatedWard.Count; i++)
                    if (calculatedWard[i].EndTime < clockTime)
                        calculatedWard.RemoveAt(i--);
            }
        }

        private void OnGameObjectDestroyed(GameObject sender)
        {
            if (!wardNames.Contains(sender.Name))
                return;

            wards.RemoveAll(item => item.NetworkId == sender.NetworkId);
            calculatedWard.RemoveAll(item => item.Position.DistanceSquared(sender.Position) < 25f * 25f);
        }

        private void OnProcessSpell(Obj_AI_Base sender, Obj_AI_BaseMissileClientDataEventArgs args)
        {
            if (args.Sender.IsAlly || args.Sender.Type != GameObjectType.obj_AI_Hero)
                return;

            if (wardSpells.Contains(args.SpellData.Name))
            {
                var time = wardSpellToTimeResolveFunc[args.SpellData.Name](args.Sender);
                calculatedWard.Add(new ward(Game.ClockTime + time, args.End, args.SpellData.Name == "JammerDevice" ? pinkWard : greenWard));
                EliminateDuplicates();
            }
        }

        private void OnRender()
        {
            if (WardTrackerActive)
            {
                wards.RemoveAll(it => !it.IsValid);
                foreach (var ward in wards)
                    Render.Circle(ward.Position, 75, 16, ward.Name == "JammerDevice" ? pinkWard : greenWard);
                foreach (var ward in calculatedWard)
                    Render.Circle(ward.Position, 75, 16, ward.Color);
            }

            if (WardTrackerActive)
                foreach (var calculatedWard in calculatedWard)
                {
                    if (calculatedWard.EndTime - Game.ClockTime > 300)
                        continue;

                    if (Render.WorldToScreen(calculatedWard.Position, out Vector2 screenCoord))
                    {
                        screenCoord.Y -= 5;
                        if (screenCoord.X > 0 && screenCoord.Y > 0 && screenCoord.X < Render.Width && screenCoord.Y < Render.Height)
                            Render.Text(screenCoord.X, screenCoord.Y, calculatedWard.Color, $"{calculatedWard.EndTime - Game.ClockTime:F0} sec");
                    }
                }
        }

        private void EliminateDuplicates()
        {
            foreach (var calculatedWard in calculatedWard)
                for (var index = 0; index < wards.Count; index++)
                    if (wards[index].Position.DistanceSquared(calculatedWard.Position) < 25 * 25)
                        wards.RemoveAt(index--);
        }


        private void OnGameObjectCreated(GameObject sender)
        {
            if (wardNames.Contains(sender.Name) && !sender.IsAlly)
            {
                wards.Add((Obj_AI_Minion)sender);
                EliminateDuplicates();
            }
        }
    }
}


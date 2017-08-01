namespace aUtility
{
    using System.Drawing;

    using Aimtec;
    using Aimtec.SDK.Menu.Components;
    using Aimtec.SDK.Util.Cache;

    internal class HealthTracker
    {

        private bool HealthTrackerActive
        {
            get { return MenuClass.HealthTracker["healthtrackeronoff"].Enabled; }
        }

        private int HudOffsetRight
        {
            get { return MenuClass.HealthTracker["xpos"].As<MenuSlider>().Value; }
        }

        private int HudOffsetTop
        {
            get { return MenuClass.HealthTracker["ypos"].As<MenuSlider>().Value; }
        }

        public HealthTracker()
        {
            if (!HealthTrackerActive)
            {
                return;
            }

            float i = 0;
            foreach (var hero in GameObjects.EnemyHeroes)
            {
                var champion = hero.ChampionName;
                var healthPercent = (int)(hero.Health / hero.MaxHealth * 100);
                var championInfo = $"{champion} ({healthPercent}%)";
                const int Height = 15;

                Render.Text((int)((Render.Width - HudOffsetRight) + 10f), (int)
                 (HudOffsetTop + i + 13),
                 (int)(hero.Health / hero.MaxHealth * 100) > 0 ? Color.AliceBlue : Color.Red, championInfo,
                 RenderTextFlags.VerticalCenter | RenderTextFlags.VerticalCenter);

                i += 20f + 5;
            }
        }
    }
}

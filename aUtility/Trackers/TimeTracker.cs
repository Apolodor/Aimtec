namespace aUtility
{
    using System;
    using System.Drawing;

    using Aimtec;
    using Aimtec.SDK.Menu.Components;

    internal class TimeTracker
    {

        private bool HudActive
        {
            get { return MenuClass.TimeTracker["timetrackeronoff"].Enabled; }
        }

        private int HudOffsetRight
        {
            get { return MenuClass.TimeTracker["xpos"].As<MenuSlider>().Value; }
        }

        private int HudOffsetTop
        {
            get { return MenuClass.TimeTracker["ypos"].As<MenuSlider>().Value; }
        }

        public TimeTracker()
        {
            if (!HudActive)
            {
                return;
            }

            Render.Text((int)((Render.Width - HudOffsetRight) + 25f),
           (int)(HudOffsetTop), Color.Green, DateTime.Now.ToShortTimeString());
        }
    }
}
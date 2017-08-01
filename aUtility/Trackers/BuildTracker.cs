namespace aUtility
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;

    using Aimtec;
    using Aimtec.SDK.Extensions;
    using Aimtec.SDK.Menu.Components;

    public class BuildTracker
    {
        private bool DrawTower
        {
            get { return MenuClass.Menu["recalltracker"].Enabled; }
        }
    }
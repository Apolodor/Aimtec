namespace aUtility
{
    using System.Collections.Generic;
    using System.Drawing;

    using Aimtec;
    using Aimtec.SDK.Menu.Components;


    internal class RecallTracker
    {
        private bool _RecallTrackerActive
        {
            get { return MenuClass.RecallTracker["recalltracker"].Enabled; }
        }

        private bool _RecallTrackerDebug
        {
            get { return MenuClass.RecallTracker["recalltrackerdebug"].Enabled; }
        }

        private int startX
        {
            get { return MenuClass.RecallTracker["xpos"].As<MenuSlider>().Value; }
        }

        private int startY
        {
            get { return MenuClass.RecallTracker["ypos"].As<MenuSlider>().Value; }
        }

        private float barWidth = 400f;
        private float barHeight = 30f;


        private readonly Color bgColor = Color.Green;
        private readonly Color fgColor = Color.White;

        private readonly List<Recall> _recalls = new List<Recall>();

        int barCount = 1;

        public RecallTracker()
        {
            Render.OnPresent += Render_OnPresent;
            Obj_AI_Base.OnTeleport += OnTeleport;

        }

        private void OnTeleport(Obj_AI_Base sender, Obj_AI_BaseTeleportEventArgs args)
           {
                if (args.Name == "recall" && !sender.IsAlly)
                {
                    _recalls.Add(new Recall(Game.ClockTime, Game.ClockTime + 8, sender as Obj_AI_Hero));
                }
                else if (args.Name == "SuperRecall" && !sender.IsAlly)
                {
                    _recalls.Add(new Recall(Game.ClockTime, Game.ClockTime + 4, sender as Obj_AI_Hero));
                }
                else if (string.IsNullOrWhiteSpace(args.Name))
                {
                    _recalls.RemoveAll(it => it?.Caster?.NetworkId == sender?.NetworkId);
                }
            } 

        private void Render_OnPresent()
        {
            if (!_RecallTrackerActive)
            {
                return;
            }

            {
                for (var index = 0; index < _recalls.Count; index++)
                {
                    var recall = _recalls[index];
                    var percent = (recall.EndTime - Game.ClockTime) / (recall.EndTime - recall.StartTime);

                    if (percent < 0) 
                        continue;

                    Render.Line(startX, startY + index * barHeight,
                        startX + (barWidth * percent),
                        startY + index * barHeight, 22f, false, bgColor);

                    Render.Line((float)startX,
                        startY + barHeight * (barCount / 2f) - barHeight / 2f,
                        startX + barWidth,
                        startY + barHeight * (barCount / 2f) - barHeight / 2f,
                        barHeight * barCount,
                        false, Color.FromArgb(150, Color.DarkGreen));

                    Render.Text(startX + (barWidth / 2f) - 15f,
                        startY + index * barHeight - 5f,
                        fgColor, recall.Caster?.ChampionName ?? "Unknown");

                }
                if (_RecallTrackerDebug)
                {
                    Render.Line((float)startX,
                        startY + barHeight * (barCount / 2f) - barHeight / 2f,
                        startX + barWidth,
                        startY + barHeight * (barCount / 2f) - barHeight / 2f,
                        barHeight * barCount,
                        false, Color.FromArgb(150, Color.DarkGreen));
                }
            }
        }
    }
}
  



  
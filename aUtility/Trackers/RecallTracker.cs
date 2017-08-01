namespace aUtility
{
    using System.Collections.Generic;

    using Aimtec;
    using System.Drawing;

    internal class RecallTracker
    {
        private bool _RecallTrackerActive
        {
            get { return MenuClass.Menu["recalltracker"].Enabled; }
        }

        private int startX = 440, startY = 512;

        private float barWidth = 400f;
        private float barHeight = 30f;

        private readonly Color bgColor = Color.Green;
        private readonly Color fgColor = Color.White;

        private readonly List<Recall> _recalls = new List<Recall>();

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

                    Render.Line(startX, startY + index * barHeight, startX + (barWidth * percent), startY + index * barHeight, 22f, false, bgColor);

                    Render.Text(startX + (barWidth / 2f) - 15f, startY + index * barHeight - 5f, fgColor, recall.Caster?.ChampionName ?? "Unknown");

               }
            }
        }
    }
}
  



  
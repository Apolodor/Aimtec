namespace aUtility
{
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;

    using Aimtec;
    using Aimtec.SDK.Extensions;

    internal class RangeTracker
    {
        private bool aTowerRange
        {
            get { return MenuClass.RangeTracker["alliestower"].Enabled; }
        }

        private bool eTowerRange
        {
            get { return MenuClass.RangeTracker["enemiestower"].Enabled; }
        }

        private readonly List<Obj_AI_Minion> _wards = new List<Obj_AI_Minion>();

        public RangeTracker()
        {
            var player = ObjectManager.GetLocalPlayer();
            foreach (var tower in
            ObjectManager.Get<Obj_AI_Turret>().Where(
                e => !e.IsDead && e.IsVisible &&
                (e.IsEnemy && eTowerRange || e.IsAlly && aTowerRange)))

            {
                var towerAutoAttackRange = 775f + tower.BoundingRadius + player.BoundingRadius - 10f;
                Render.Circle(tower.ServerPosition, towerAutoAttackRange, 30, tower.IsEnemy && player.Distance(tower) <= towerAutoAttackRange
                    ? Color.Red
                    : Color.LightGreen);
            }
        }
    }
}
















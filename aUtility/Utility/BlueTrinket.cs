/* Soon 
 * namespace aUtility
{
    using System;
    using Aimtec;

    internal class BlueTrinket
    {
        private bool buyBlueTrinket
        {
            get { return MenuClass.Menu["trinket"].Enabled; }
        }
        private const float CheckInterval = 333f;
        private static Random random;
        private float lastCheck = Environment.TickCount;
        private static Obj_AI_Hero Player => ObjectManager.GetLocalPlayer();

        public BlueTrinket()
        {
            random = new Random(Environment.TickCount);

            Game.OnUpdate += OnUpdate;
        }

        private void OnUpdate()
        {
            if (!buyBlueTrinket)
            {
                return;
            }

            if (lastCheck + CheckInterval > Environment.TickCount)
            {
                return;
            }

            lastCheck = Environment.TickCount;

            if (Player.IsDead)
            {
                if (Player.Level >= 9)
                {

                }
            }
        }
    }
}
*/
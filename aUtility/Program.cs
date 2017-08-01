namespace aUtility
{
    using Aimtec;
    using Aimtec.SDK.Events;

    internal class Program
    {

        private static void Main()
        {
            GameEvents.GameStart += OnStart;
        }

        private static void OnPresent()
        {
            var SpellTracker = new SpellTracker();
            var HealthTracker = new HealthTracker();
            var TimeTracker = new TimeTracker();          
            var RangeTracker = new RangeTracker();         
        }

        private static void OnStart()
        {
            Menus.Initialize();

            var RecallTracker = new RecallTracker();
            var WardsTracker = new WardsTracker();
            var UltLevelUp = new UltLevelUp();

            var Barrier = new Barrier();
            var Heal = new Heal();
            var Cleanse = new Cleanse();
            var Smite = new Smite();

            Render.OnPresent += OnPresent;
        }
    }
}

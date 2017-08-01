namespace aUtility
{
    using System.Linq;

    using Aimtec.SDK.Menu;
    using Aimtec.SDK.Menu.Components;
    using Aimtec.SDK.Util.Cache;

    internal class Menus
    {
        public static void Initialize()
        {
            MenuClass.Menu = new Menu("aUtility", "aUtility", true);
            {
                MenuClass.SpellTracker = new Menu("spelltracker", "Cooldowns Tracker");
                {
                    MenuClass.SpellTracker.Add(new MenuBool("allies", "Allies"));
                    MenuClass.SpellTracker.Add(new MenuBool("enemies", "Enemies"));
                }
                MenuClass.Menu.Add(MenuClass.SpellTracker);

                MenuClass.HealthTracker = new Menu("HealthTracker", "Health Tracker");
                {
                    MenuClass.HealthTracker.Add(new MenuBool("healthtrackeronoff", "Status"));
                    MenuClass.HealthTracker.Add(new MenuSlider("xpos", "X Position", 250, 0, 2000));
                    MenuClass.HealthTracker.Add(new MenuSlider("ypos", "Y Position", 150, 0, 2000));
                }
                MenuClass.Menu.Add(MenuClass.HealthTracker);

                MenuClass.TimeTracker = new Menu("timetracker", "Time Tracker");
                {
                    MenuClass.TimeTracker.Add(new MenuBool("timetrackeronoff", "Status", true));
                    MenuClass.TimeTracker.Add(new MenuSlider("xpos", "X Position", 250, 0, 2000));
                    MenuClass.TimeTracker.Add(new MenuSlider("ypos", "Y Position", 150, 0, 2000));
                }
                MenuClass.Menu.Add(MenuClass.TimeTracker);

                MenuClass.RangeTracker = new Menu("rangetracker", "Range Tracker");
                {
                    MenuClass.RangeTracker.Add(new MenuBool("alliestower", "Allies Tower Range"));
                    MenuClass.RangeTracker.Add(new MenuBool("enemiestower", "Enemies Tower Range"));
                }
                MenuClass.Menu.Add(MenuClass.RangeTracker);

                MenuClass.SummonerSpells = new Menu("summonerspells", "Summoner Spells");
                {
                    MenuClass.Heal = new Menu("healmenu", "Heal");
                    {
                        MenuClass.Heal.Add(new MenuBool("useheal", "Enable Heal?"));
                        MenuClass.Heal.Add(new MenuBool("usehealallies", "Heal Allies?"));

                        MenuClass.WhitelistHealAllies = new Menu("healwhitelist", "Heal Whitelist:");
                        MenuClass.Heal.Add(MenuClass.WhitelistHealAllies);
                        if (GameObjects.AllyHeroes.Any(t => !t.IsMe))
                        {
                            foreach (var ally in GameObjects.AllyHeroes.Where(t => !t.IsMe))
                            {
                                MenuClass.WhitelistHealAllies.Add(new MenuBool(ally.ChampionName.ToLower(), "Use for: " + ally.ChampionName));
                            }
                        }
                        else
                        {
                            MenuClass.WhitelistHealAllies.Add(new MenuSeperator("separator", "No allies found."));
                        }

                        MenuClass.Heal.Add(new MenuSeperator("healcustom", "Heal Settings"));
                        MenuClass.Heal.Add(new MenuSlider("healpercent", "use Heal When Hp% <", 25, 0, 75));
                    }
                    MenuClass.SummonerSpells.Add(MenuClass.Heal);

                    MenuClass.Barrier = new Menu("barrierMenu", "Barrier");
                    {
                        MenuClass.Barrier.Add(new MenuBool("usebarrier", "Enable Barrier ?"));
                        MenuClass.Barrier.Add(new MenuSeperator("BarrierSettings", "Barrier Settings"));
                        MenuClass.Barrier.Add(new MenuSlider("hpbarrier","Use Barrier When HP% <", 25, 0, 75));
                    }
                    MenuClass.SummonerSpells.Add(MenuClass.Barrier);

                    MenuClass.Cleanse = new Menu("cleanseMenu", "Cleanse");
                    {
                        MenuClass.Cleanse.Add(new MenuBool("useCleanse", "Enable Cleanse?"));
                        MenuClass.Cleanse.Add(new MenuSeperator("CleanseSettings", "Cleanse Settings"));
                        MenuClass.WhitelistCleanse = new Menu("whitelistcleanseMenu", "Use Clense On :");
                        {
                            MenuClass.WhitelistCleanse.Add(new MenuBool("BuffType.Stun", "Stun"));
                            MenuClass.WhitelistCleanse.Add(new MenuBool("BuffType.Fear", "Fear"));
                            MenuClass.WhitelistCleanse.Add(new MenuBool("BuffType.Flee", "Flee"));
                            MenuClass.WhitelistCleanse.Add(new MenuBool("BuffType.Snare", "Snare"));
                            MenuClass.WhitelistCleanse.Add(new MenuBool("BuffType.Taunt", "Taunt"));
                            MenuClass.WhitelistCleanse.Add(new MenuBool("BuffType.Charm", "Charm"));
                        }
                        MenuClass.Cleanse.Add(MenuClass.WhitelistCleanse);
                    }
                    MenuClass.SummonerSpells.Add(MenuClass.Cleanse);

                    MenuClass.Smite = new Menu("smitemenu", "Smite");
                    {
                        MenuClass.Smite.Add(new MenuBool("usesmite", "Use Smite?"));
                        MenuClass.Smite.Add(new MenuKeyBind("smiteactive", "Auto Smite", Aimtec.SDK.Util.KeyCode.M, KeybindType.Toggle, true));

                        MenuClass.Smite.Add(new MenuSeperator("SmiteSettings", "Smite Settings"));

                        MenuClass.Smite.Add(new MenuBool("rangedrawing", "Draw Smite Range"));
                        MenuClass.Smite.Add(new MenuBool("statusdrawing", "Draw Smite Status"));

                        MenuClass.Smite.Add(new MenuSeperator("SmiteCastSettings", "Smite Cast Settings"));

                        MenuClass.EpicMonsters = new Menu("epicmonsters", "Use Smite on:");
                        {
                            MenuClass.EpicMonsters.Add(new MenuBool("SRU_Baron", "Baron"));
                            MenuClass.EpicMonsters.Add(new MenuBool("SRU_RiftHerald", "Rift Herald"));
                        }
                        MenuClass.Smite.Add(MenuClass.EpicMonsters);

                        MenuClass.Dragons = new Menu("dragons", "Use Smite on:");
                        {
                            MenuClass.Dragons.Add(new MenuBool("SRU_Dragon_Elder", "Elder Draker"));
                            MenuClass.Dragons.Add(new MenuBool("SRU_Dragon_Fire", "Fire Drake"));
                            MenuClass.Dragons.Add(new MenuBool("SRU_Dragon_Water", "Water Drake"));
                            MenuClass.Dragons.Add(new MenuBool("SRU_Dragon_Air", "Cloud Drake"));
                            MenuClass.Dragons.Add(new MenuBool("SRU_Dragon_Earth", "Earth Drake"));
                        }
                        MenuClass.Smite.Add(MenuClass.Dragons);

                        MenuClass.Monsters = new Menu("monsters", "Use Smite on:");
                        {
                            MenuClass.Monsters.Add(new MenuBool("SRU_Blue", "Blue"));
                            MenuClass.Monsters.Add(new MenuBool("SRU_Red", "Red"));
                        }
                        MenuClass.Smite.Add(MenuClass.Monsters);
                    }
                    MenuClass.SummonerSpells.Add(MenuClass.Smite);

                }         
                MenuClass.Menu.Add(MenuClass.SummonerSpells);

                MenuClass.RecallTracker = new Menu("RecallTracker", "Recall Tracker");
                {
                    MenuClass.RecallTracker.Add(new MenuBool("recalltracker", "Status"));
                    MenuClass.RecallTracker.Add(new MenuSlider("xpos", "X Position", 440, 0, 2000));
                    MenuClass.RecallTracker.Add(new MenuSlider("ypos", "Y Position", 512, 0, 2000));
                    MenuClass.RecallTracker.Add(new MenuSeperator("separatorrctracker", "Recall Tracker Debugger"));
                    MenuClass.RecallTracker.Add(new MenuBool("recalltrackerdebug", "Debugger",false));
                   
                }
                MenuClass.Menu.Add(MenuClass.RecallTracker);

                MenuClass.Menu.Add(new MenuSeperator("separator", "Quick Toggles"));

                //MenuClass.Menu.Add(new MenuBool("ganktracker", "Gank Tracker"));

                MenuClass.Menu.Add(new MenuBool("wardtracker", "Wards Tracker"));
                //   MenuClass.Menu.Add(new MenuBool("trinket", "Buy Blue Trinket"));
                MenuClass.Menu.Add(new MenuBool("lvlupult", "Auto R Lvl Up"));

                MenuClass.Menu.Attach();
            }
        }
    }
}

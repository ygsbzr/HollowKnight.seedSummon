using Satchel.BetterMenus;
namespace SeedSummon
{
    public class ModMenu
    {
        private static Menu MenuRef;
        public static MenuScreen GetMenu(MenuScreen lastmenu)
        {
            if(MenuRef == null)
            {
                MenuRef = Prepare();
            }
            return MenuRef.GetMenuScreen(lastmenu);

        }
        public static Menu Prepare()
        {
            return new("Seed Summon", new Element[]
            {
                new KeyBind("Summon Key",SeedSummon.setting.keyBinds.summon)
            });
        }
    }
}

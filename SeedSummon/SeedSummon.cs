using Modding;
using UnityEngine;
using MonoMod.Cil;
using UObject = UnityEngine.Object;
using Mono.Cecil.Cil;

namespace SeedSummon
{
    public class SeedSummon:Mod,IGlobalSettings<Setting>,ICustomMenuMod
    {
        public static GameObject origseed;
        public override string GetVersion() => "1.0";
        public override List<(string,string)> GetPreloadNames()
        {
            return new List<(string, string)>
            {
                ("Tutorial_01","_Props/Health Cocoon")
            };
        }
        public static Setting setting = new();
        public Setting OnSaveGlobal() => setting;
        public void OnLoadGlobal(Setting s) => setting = s;
        public override void Initialize(Dictionary<string, Dictionary<string, GameObject>> preloadedObjects)
        {
             origseed = preloadedObjects["Tutorial_01"]["_Props/Health Cocoon"].GetComponent<HealthCocoon>().flingPrefabs.First(x => x.prefab.name.Contains("Scuttler")).prefab;
            IL.ScuttlerControl.Start += Modifyseed;
            ModHooks.HeroUpdateHook += Summon;
            
        }

        private void Summon()
        {
           if(setting.keyBinds.wasPressed())
            {
                if (origseed != null)
                {
                    GameObject cloneseed = UObject.Instantiate(origseed);
                    cloneseed.transform.SetPosition2D(HeroController.instance.transform.position);
                    cloneseed.SetActive(true);
                    LogDebug("Summon a seed");
                }
            }
        }

        private void Modifyseed(MonoMod.Cil.ILContext il)
        {
            Log("StartModifyseed");
            ILCursor cursor = new ILCursor(il);
            cursor.GotoNext(i => i.MatchStfld<ScuttlerControl>("reverseRun"));
            cursor.GotoPrev(MoveType.After,i => i.MatchLdarg(0));
            cursor.RemoveRange(4);
            cursor.GotoNext(i => i.MatchStfld<ScuttlerControl>("reverseRun"));
            cursor.Emit(OpCodes.Ldc_I4_1);
            cursor.GotoNext();
            cursor.GotoNext();
            cursor.EmitDelegate<Action>(() => Log("Finish mod seed"));

        }
        public bool ToggleButtonInsideMenu => true;
        public MenuScreen GetMenuScreen(MenuScreen modmenu,ModToggleDelegates? modToggle)
        {
            return ModMenu.GetMenu(modmenu);
        }
    }
}

using InControl;
using Modding.Converters;
using Newtonsoft.Json;
namespace SeedSummon
{
    public class Setting
    {
        [JsonConverter(typeof(PlayerActionSetConverter))]
        public KeyBinds keyBinds = new KeyBinds();
    }
    public class KeyBinds:PlayerActionSet
    {
        public PlayerAction summon;
        public KeyBinds()
        {
            summon = CreatePlayerAction("Summon Blue");
        }
        public bool wasPressed()
        {
            return summon.WasPressed;
        }
    }
}

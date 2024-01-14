using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace HellWeather
{
    [BepInPlugin(ModGUID, ModName, ModVersion)]
    public class HellWeatherBase : BaseUnityPlugin
    {
        public const string ModGUID = "stormytuna.HellWeather";
        public const string ModName = "HellWeather";
        public const string ModVersion = "1.0.0";

        public static ManualLogSource Log = BepInEx.Logging.Logger.CreateLogSource(ModGUID);
        public static HellWeatherBase Instance;

        private readonly Harmony harmony = new Harmony(ModGUID);

        public static LevelWeatherType HellWeather => (LevelWeatherType)6; // Shitty hardcode but I can't find a way around it - will need to update if another weather is added

        private void Awake() {
            if (Instance is null) {
                Instance = this;
            }

            Log.LogInfo("Hell Weather has awoken!");

            harmony.PatchAll();
        }
    }
}

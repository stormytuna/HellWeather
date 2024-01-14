using HarmonyLib;

namespace HellWeather.Patches
{
    [HarmonyPatch(typeof(StartOfRound))]
    public class StartOfRoundPatch
    {
        [HarmonyPostfix, HarmonyPatch("Awake")]
        public static void Awake(SelectableLevel[] ___levels) {
            foreach (SelectableLevel level in ___levels) {
                level.randomWeathers = level.randomWeathers.AddToArray(new RandomWeatherWithVariables() {
                    weatherType = HellWeatherBase.HellWeather
                });
            }
        }

        [HarmonyPrefix, HarmonyPatch(nameof(StartOfRound.SetPlanetsWeather))]
        public static bool MakeAllPlanetsHell(SelectableLevel[] ___levels) {
            foreach (SelectableLevel level in ___levels) {
                level.currentWeather = HellWeatherBase.HellWeather;
                HellWeatherBase.Log.LogInfo($"Current weather: {level.currentWeather}");
            }

            return false;
        }

        [HarmonyPostfix, HarmonyPatch("OnShipLandedMiscEvents")]
        public static void DisplayHellWeatherPopup() {
            HellWeatherBase.Log.LogInfo($"Uhhhhhhhhhh display popup moment!!!!. weather is {TimeOfDay.Instance.currentLevelWeather}");
            if (TimeOfDay.Instance.currentLevelWeather == HellWeatherBase.HellWeather) {
                HellWeatherBase.Log.LogInfo($"thingy!!");
                HUDManager.Instance.DisplayTip("Weather alert!", "You have landed in some severe weather anomolies. Good luck.", true, true, "LC_EclipseTip");
            }
        }
    }
}

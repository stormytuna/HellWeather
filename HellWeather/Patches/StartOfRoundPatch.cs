using System.Collections.Generic;
using System.Linq;
using HarmonyLib;

namespace HellWeather.Patches
{
	[HarmonyPatch(typeof(StartOfRound))]
	public class StartOfRoundPatch
	{
		[HarmonyPostfix]
		[HarmonyPatch("Awake")]
		public static void Awake(SelectableLevel[] ___levels) {
			foreach (SelectableLevel level in ___levels) {
				level.randomWeathers = level.randomWeathers.AddToArray(new RandomWeatherWithVariables {
					weatherType = HellWeatherBase.HellWeather
				});
			}
		}

		[HarmonyPrefix]
		[HarmonyPatch(nameof(StartOfRound.SetPlanetsWeather))]
		public static bool MakeAllPlanetsHell(SelectableLevel[] ___levels) {
			if (!HellWeatherBase.AlwaysHellWeather.Value) {
				return true;
			}

			foreach (SelectableLevel level in ___levels) {
				level.currentWeather = HellWeatherBase.HellWeather;
				HellWeatherBase.Log.LogInfo($"Current weather: {level.currentWeather}");
			}

			return false;
		}

		[HarmonyPostfix]
		[HarmonyPatch("OnShipLandedMiscEvents")]
		public static void DisplayHellWeatherPopup() {
			if (TimeOfDay.Instance.currentLevelWeather != HellWeatherBase.HellWeather) {
				return;
			}

			IEnumerable<RandomWeatherWithVariables> randomWeathersNotIncludingHell = TimeOfDay.Instance.currentLevel.randomWeathers.Where(rw => rw.weatherType != HellWeatherBase.HellWeather && HellWeatherBase.CanApplyChangesToWeather(rw.weatherType));
			IEnumerable<LevelWeatherType> allPossibleWeatherStrings = randomWeathersNotIncludingHell.Select(rw => rw.weatherType);
			string weathersString = string.Join(", ", allPossibleWeatherStrings);
			HUDManager.Instance.DisplayTip("Weather alert!", $"You have landed in some severe weather anomalies. Good luck.\nWeathers active: {weathersString}", true);
		}
	}
}

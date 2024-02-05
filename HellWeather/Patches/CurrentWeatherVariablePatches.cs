using HarmonyLib;
using HellWeather.Helpers;

namespace HellWeather.Patches
{
	[HarmonyPatch]
	public class CurrentWeatherVariablePatches
	{
		private static float[] currentWeatherVariables = new float[7];
		private static float[] currentWeatherVariables2 = new float[7];

		private static LevelWeatherType? weatherTypeToUnfuck;

		private static void FuckCurrentWeatherVariable(LevelWeatherType weatherType) {
			if (TimeOfDay.Instance.currentLevelWeather == HellWeatherBase.HellWeather && HellWeatherBase.CanApplyChangesToWeather(weatherType)) {
				TimeOfDay.Instance.currentWeatherVariable = currentWeatherVariables[(int)weatherType];
				weatherTypeToUnfuck = weatherType;
			}
		}

		private static void FuckCurrentWeatherVariable2(LevelWeatherType weatherType) {
			if (TimeOfDay.Instance.currentLevelWeather == HellWeatherBase.HellWeather && HellWeatherBase.CanApplyChangesToWeather(weatherType)) {
				TimeOfDay.Instance.currentWeatherVariable2 = currentWeatherVariables2[(int)weatherType];
				weatherTypeToUnfuck = weatherType;
			}
		}

		private static void UnfuckCurrentWeatherVariable() {
			if (weatherTypeToUnfuck != null) {
				currentWeatherVariables[(int)weatherTypeToUnfuck] = TimeOfDay.Instance.currentWeatherVariable;
				weatherTypeToUnfuck = null;
			}
		}

		private static void UnfuckCurrentWeatherVariable2() {
			if (weatherTypeToUnfuck != null) {
				currentWeatherVariables2[(int)weatherTypeToUnfuck] = TimeOfDay.Instance.currentWeatherVariable2;
				weatherTypeToUnfuck = null;
			}
		}

		[HarmonyPostfix]
		[HarmonyPatch(typeof(RoundManager), nameof(RoundManager.SetToCurrentLevelWeather))]
		public static void SetupCurrentWeatherVariables(RoundManager __instance) {
			foreach (RandomWeatherWithVariables possibleWeather in WeatherHelpers.GetVanillaRandomWeathersWithEffects(__instance.currentLevel)) {
				currentWeatherVariables[(int)possibleWeather.weatherType] = possibleWeather.weatherVariable;
				currentWeatherVariables2[(int)possibleWeather.weatherType] = possibleWeather.weatherVariable2;
			}
		}

		[HarmonyPostfix]
		[HarmonyPatch(typeof(StartOfRound), nameof(StartOfRound.EndOfGame))]
		public static void ResetCurrentWeatherVariables() {
			currentWeatherVariables = new float[7];
			currentWeatherVariables2 = new float[7];
		}

		[HarmonyPrefix]
		[HarmonyPatch(typeof(EclipseWeather), nameof(EclipseWeather.OnEnable))]
		public static void FuckEclipsedWeatherVariables() {
			FuckCurrentWeatherVariable(LevelWeatherType.Eclipsed);
			FuckCurrentWeatherVariable2(LevelWeatherType.Eclipsed);
		}

		[HarmonyPostfix]
		[HarmonyPatch(typeof(EclipseWeather), nameof(EclipseWeather.OnEnable))]
		public static void UnfuckEclipsedWeatherVariables() {
			UnfuckCurrentWeatherVariable();
			UnfuckCurrentWeatherVariable2();
		}

		[HarmonyPrefix]
		[HarmonyPatch(typeof(FloodWeather), nameof(FloodWeather.OnEnable))]
		[HarmonyPatch(typeof(FloodWeather), nameof(FloodWeather.Update))]
		[HarmonyPatch(typeof(FloodWeather), nameof(FloodWeather.OnGlobalTimeSync))]
		public static void FuckFloodedWeatherVariables() {
			FuckCurrentWeatherVariable(LevelWeatherType.Flooded);
			FuckCurrentWeatherVariable2(LevelWeatherType.Flooded);
		}

		[HarmonyPostfix]
		[HarmonyPatch(typeof(FloodWeather), nameof(FloodWeather.OnEnable))]
		[HarmonyPatch(typeof(FloodWeather), nameof(FloodWeather.Update))]
		[HarmonyPatch(typeof(FloodWeather), nameof(FloodWeather.OnGlobalTimeSync))]
		public static void UnfuckFloodedWeatherVariables() {
			UnfuckCurrentWeatherVariable();
			UnfuckCurrentWeatherVariable2();
		}

		[HarmonyPrefix]
		[HarmonyPatch(typeof(StormyWeather), nameof(StormyWeather.DetermineNextStrikeInterval))]
		[HarmonyPatch(typeof(StormyWeather), nameof(StormyWeather.LightningStrikeRandom))]
		public static void FuckStormyWeatherVariables() {
			FuckCurrentWeatherVariable(LevelWeatherType.Stormy);
			FuckCurrentWeatherVariable2(LevelWeatherType.Stormy);
		}

		[HarmonyPrefix]
		[HarmonyPatch(typeof(StormyWeather), nameof(StormyWeather.DetermineNextStrikeInterval))]
		[HarmonyPatch(typeof(StormyWeather), nameof(StormyWeather.LightningStrikeRandom))]
		public static void UnfuckStormyWeatherVariables() {
			FuckCurrentWeatherVariable(LevelWeatherType.Stormy);
			FuckCurrentWeatherVariable2(LevelWeatherType.Stormy);
		}
	}
}

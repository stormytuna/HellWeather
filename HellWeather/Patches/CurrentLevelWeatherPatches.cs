using HarmonyLib;

namespace HellWeather.Patches
{
	[HarmonyPatch]
	public class CurrentLevelWeatherPatches
	{
		private static bool changeBackToHellWeather;

		private static void FuckCurrentLevelWeatherType(LevelWeatherType newWeatherType) {
			if (TimeOfDay.Instance.currentLevelWeather == HellWeatherBase.HellWeather && HellWeatherBase.CanApplyChangesToWeather(newWeatherType)) {
				TimeOfDay.Instance.currentLevelWeather = newWeatherType;
				changeBackToHellWeather = true;
			}
		}

		private static void UnfuckCurrentLevelWeatherType() {
			if (changeBackToHellWeather) {
				TimeOfDay.Instance.currentLevelWeather = HellWeatherBase.HellWeather;
				changeBackToHellWeather = false;
			}
		}

		[HarmonyPrefix]
		[HarmonyPatch(typeof(EnemyAI), nameof(EnemyAI.CheckLineOfSightForPlayer))]
		[HarmonyPatch(typeof(EnemyAI), nameof(EnemyAI.CheckLineOfSightForClosestPlayer))]
		[HarmonyPatch(typeof(EnemyAI), nameof(EnemyAI.GetAllPlayersInLineOfSight))]
		public static void PreCheckLineOfSight() => FuckCurrentLevelWeatherType(LevelWeatherType.Foggy);

		[HarmonyPrefix]
		[HarmonyPatch(typeof(EnemyAI), nameof(EnemyAI.CheckLineOfSightForPlayer))]
		[HarmonyPatch(typeof(EnemyAI), nameof(EnemyAI.CheckLineOfSightForClosestPlayer))]
		[HarmonyPatch(typeof(EnemyAI), nameof(EnemyAI.GetAllPlayersInLineOfSight))]
		public static void PostCheckLineOfSight() => UnfuckCurrentLevelWeatherType();

		[HarmonyPrefix]
		[HarmonyPatch(typeof(RoundManager), nameof(RoundManager.SpawnOutsideHazards))]
		public static void PreSpawnOutsideHazards() => FuckCurrentLevelWeatherType(LevelWeatherType.Rainy);

		[HarmonyPostfix]
		[HarmonyPatch(typeof(RoundManager), nameof(RoundManager.SpawnOutsideHazards))]
		public static void PostSpawnOutsideHazards() => UnfuckCurrentLevelWeatherType();
	}
}

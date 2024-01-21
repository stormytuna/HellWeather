using HarmonyLib;

namespace HellWeather.Patches
{
	[HarmonyPatch]
	public class EnableDisableWeatherEffectsPatches
	{
		private static void EnablePossibleWeatherEffects() {
			foreach (RandomWeatherWithVariables possibleWeather in StartOfRound.Instance.currentLevel.randomWeathers) {
				if (!HellWeatherBase.CanApplyChangesToWeather(possibleWeather.weatherType)) {
					continue;
				}

				WeatherEffect weatherEffect = TimeOfDay.Instance.effects[(int)possibleWeather.weatherType];
				weatherEffect.effectEnabled = true;
				if (weatherEffect.effectPermanentObject != null) {
					weatherEffect.effectPermanentObject.SetActive(true);
				}
			}
		}

		private static void DisableWeatherEffects() {
			foreach (WeatherEffect weatherEffect in TimeOfDay.Instance.effects) {
				weatherEffect.effectEnabled = false;
				if (weatherEffect.effectPermanentObject != null) {
					weatherEffect.effectPermanentObject.SetActive(false);
				}
			}
		}

		[HarmonyPostfix]
		[HarmonyPatch(typeof(StartOfRound), nameof(StartOfRound.openingDoorsSequence))]
		public static void EnableWeathersAtStartOfGame() {
			if (StartOfRound.Instance.currentLevel.currentWeather != HellWeatherBase.HellWeather) {
				return;
			}

			EnablePossibleWeatherEffects();
		}

		[HarmonyPostfix]
		[HarmonyPatch(typeof(AudioReverbTrigger), nameof(AudioReverbTrigger.ChangeAudioReverbForPlayer))]
		public static void EnableWeathersOnChangeAudioReverb(AudioReverbTrigger __instance) {
			if (!__instance.enableCurrentLevelWeather || TimeOfDay.Instance.currentLevelWeather != HellWeatherBase.HellWeather) {
				return;
			}

			EnablePossibleWeatherEffects();
		}

		[HarmonyPostfix]
		[HarmonyPatch(typeof(StartOfRound), nameof(StartOfRound.EndOfGame))]
		public static void DisableWeathers() {
			if (StartOfRound.Instance.currentLevel.currentWeather != HellWeatherBase.HellWeather) {
				return;
			}

			DisableWeatherEffects();
		}
	}
}

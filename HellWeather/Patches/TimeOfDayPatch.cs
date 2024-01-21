using HarmonyLib;

namespace HellWeather.Patches
{
	[HarmonyPatch(typeof(TimeOfDay))]
	public class TimeOfDayPatch
	{
		[HarmonyPostfix]
		[HarmonyPatch("Awake")]
		public static void Awake(ref WeatherEffect[] ___effects) {
			___effects = ___effects.AddToArray(new WeatherEffect {
				name = "hell",
				sunAnimatorBool = HellWeatherBase.AllowEclipsedWeather.Value ? "eclipse" : ""
			});
		}
	}
}

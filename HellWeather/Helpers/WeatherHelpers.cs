using System.Collections.Generic;
using System.Linq;

namespace HellWeather.Helpers
{
	public static class WeatherHelpers
	{
		public static bool IsVanillaWeather(LevelWeatherType weatherType) => weatherType >= LevelWeatherType.None && weatherType <= LevelWeatherType.Eclipsed;

		public static bool IsVanillaWeatherWithEffect(LevelWeatherType weatherType) => weatherType > LevelWeatherType.None && weatherType <= LevelWeatherType.Eclipsed;

		public static IEnumerable<RandomWeatherWithVariables> GetVanillaRandomWeathersWithEffects(SelectableLevel level) {
			return level.randomWeathers.Where(rw => IsVanillaWeatherWithEffect(rw.weatherType));
		}

		public static IEnumerable<WeatherEffect> GetVanillaWeatherEffects() {
			for (int weatherType = 0; weatherType <= 5; weatherType++) {
				yield return TimeOfDay.Instance.effects[weatherType];
			}
		}
	}
}

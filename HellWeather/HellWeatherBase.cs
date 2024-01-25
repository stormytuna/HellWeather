using System;
using System.Linq;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;

namespace HellWeather
{
	[BepInPlugin(ModGUID, ModName, ModVersion)]
	public class HellWeatherBase : BaseUnityPlugin
	{
		public const string ModGUID = "stormytuna.HellWeather";
		public const string ModName = "HellWeather";
		public const string ModVersion = "1.0.2";

		public static ManualLogSource Log = BepInEx.Logging.Logger.CreateLogSource(ModGUID);
		public static HellWeatherBase Instance;

		public static LevelWeatherType HellWeather;

		private readonly Harmony harmony = new Harmony(ModGUID);

		private void Awake() {
			if (Instance is null) {
				Instance = this;
			}

			HellWeather = Enum.GetValues(typeof(LevelWeatherType)).Cast<LevelWeatherType>().Max() + 1;

			Log.LogInfo("Hell Weather has awoken!");

			LoadConfigs();

			harmony.PatchAll();
		}

		#region Config

		public static ConfigEntry<bool> AllowRainyWeather;
		public static ConfigEntry<bool> AllowFoggyWeather;
		public static ConfigEntry<bool> AllowFloodedWeather;
		public static ConfigEntry<bool> AllowStormyWeather;
		public static ConfigEntry<bool> AllowEclipsedWeather;
		public static ConfigEntry<bool> AlwaysHellWeather;

		public void LoadConfigs() {
			AllowRainyWeather = Config.Bind("AllowedWeathers", nameof(AllowRainyWeather), true, "Whether or not to allow the Rainy weather type to appear in Hell weather planets");
			AllowFoggyWeather = Config.Bind("AllowedWeathers", nameof(AllowFoggyWeather), true, "Whether or not to allow the Foggy weather type to appear in Hell weather planets");
			AllowFloodedWeather = Config.Bind("AllowedWeathers", nameof(AllowFloodedWeather), true, "Whether or not to allow the Flooded weather type to appear in Hell weather planets");
			AllowStormyWeather = Config.Bind("AllowedWeathers", nameof(AllowStormyWeather), true, "Whether or not to allow the Stormy weather type to appear in Hell weather planets");
			AllowEclipsedWeather = Config.Bind("AllowedWeathers", nameof(AllowEclipsedWeather), true, "Whether or not to allow the Eclipsed weather type to appear in Hell weather planets");
			AlwaysHellWeather = Config.Bind("HellWeatherRarity", nameof(AlwaysHellWeather), false, "Whether all planets will always have Hell weather");
		}

		public static bool CanApplyChangesToWeather(LevelWeatherType weather) {
			switch (weather) {
				case LevelWeatherType.Rainy: return AllowRainyWeather.Value;
				case LevelWeatherType.Foggy: return AllowFoggyWeather.Value;
				case LevelWeatherType.Flooded: return AllowFloodedWeather.Value;
				case LevelWeatherType.Stormy: return AllowStormyWeather.Value;
				case LevelWeatherType.Eclipsed: return AllowEclipsedWeather.Value;
				default: return false;
			}
		}

		#endregion
	}
}

using HarmonyLib;
using UnityEngine;

namespace HellWeather.Patches
{
    [HarmonyPatch(typeof(TimeOfDay))]
    public class TimeOfDayPatch
    {
        [HarmonyPostfix, HarmonyPatch("Awake")]
        public static void Awake(ref WeatherEffect[] ___effects) {
            GameObject hellObject = Object.Instantiate(___effects[(int)LevelWeatherType.Eclipsed].effectObject);
            Object.Destroy(hellObject.GetComponent<EclipseWeather>());
            hellObject.AddComponent<HellWeatherEffect>();

            ___effects = ___effects.AddToArray(new WeatherEffect {
                name = "hell",
                sunAnimatorBool = "eclipse",
                effectObject = hellObject
            });
        }
    }
}

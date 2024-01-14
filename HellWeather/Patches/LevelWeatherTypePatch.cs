using System;
using HarmonyLib;

namespace HellWeather.Patches
{
    [HarmonyPatch(typeof(Enum))]
    public class LevelWeatherTypePatch
    {
        [HarmonyPrefix, HarmonyPatch("ToString", new Type[] { })]
        public static bool GetHellWeatherString(ref string __result, Enum __instance) {
            if (__instance is LevelWeatherType levelWeatherType && levelWeatherType == HellWeatherBase.HellWeather) {
                __result = "Hell";
                return false;
            }

            return true;
        }
    }
}

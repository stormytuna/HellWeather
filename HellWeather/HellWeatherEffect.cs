using System.Linq;
using UnityEngine;

namespace HellWeather
{
    public class HellWeatherEffect : MonoBehaviour
    {
        private void OnEnable() {
            HellWeatherBase.Log.LogInfo("Uhhhhh done the eclipse thingy!!");
            RoundManager.Instance.minOutsideEnemiesToSpawn = StartOfRound.Instance.currentLevel.randomWeathers.FirstOrDefault(rw => rw.weatherType == LevelWeatherType.Eclipsed).weatherVariable;
            RoundManager.Instance.minEnemiesToSpawn = StartOfRound.Instance.currentLevel.randomWeathers.FirstOrDefault(rw => rw.weatherType == LevelWeatherType.Eclipsed).weatherVariable;
            HellWeatherBase.Log.LogInfo($"Eclipse thingy: {StartOfRound.Instance.currentLevel.randomWeathers.FirstOrDefault(rw => rw.weatherType == LevelWeatherType.Eclipsed).weatherVariable}");
        }
    }
}

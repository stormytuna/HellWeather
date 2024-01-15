using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;

namespace HellWeather.Patches
{
	[HarmonyPatch(typeof(RoundManager))]
	public class RoundManagerPatch
	{
		[HarmonyTranspiler]
		[HarmonyPatch("SpawnOutsideHazards")]
		public static IEnumerable<CodeInstruction> MakeHellWeatherSpawnQuicksand(IEnumerable<CodeInstruction> instructions, ILGenerator generator) {
			CodeMatcher codeMatcher = new CodeMatcher(instructions, generator);

			codeMatcher.MatchStartForward(new CodeMatch(i => i.opcode == OpCodes.Ldloc_0));
			codeMatcher.CreateLabel(out Label rainyWeatherCheckLabel); // First line of if (TimeOfDay.Instance.currentLevelWeather == LevelWeatherType.Rainy) body

			codeMatcher.Start();

			MethodInfo timeOfDayInstanceGetter = typeof(TimeOfDay).GetProperty(nameof(TimeOfDay.Instance)).GetGetMethod();
			codeMatcher.MatchStartForward(new CodeMatch(i => i.Calls(timeOfDayInstanceGetter)));

			FieldInfo currentLevelWeatherFieldInfo = typeof(TimeOfDay).GetField(nameof(TimeOfDay.currentLevelWeather));
			codeMatcher.Insert(
				new CodeInstruction(OpCodes.Call, timeOfDayInstanceGetter),
				new CodeInstruction(OpCodes.Ldfld, currentLevelWeatherFieldInfo),
				new CodeInstruction(OpCodes.Ldc_I4, (int)HellWeatherBase.HellWeather),
				new CodeInstruction(OpCodes.Beq_S, rainyWeatherCheckLabel)
			);

			// debug!!
			codeMatcher.Start();
			foreach (CodeInstruction instruction in codeMatcher.InstructionEnumeration()) {
				HellWeatherBase.Log.LogInfo(instruction);
			}

			return codeMatcher.Instructions();
		}
	}
}

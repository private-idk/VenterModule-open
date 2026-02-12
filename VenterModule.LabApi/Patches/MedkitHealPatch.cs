using System.Collections.Generic;
using System.Reflection.Emit;
using HarmonyLib;
using InventorySystem.Items.ThrowableProjectiles;
using InventorySystem.Items.Usables;
using NorthwoodLib.Pools;
using PlayerStatsSystem;

namespace VenterModuleLabApi.Patches
{
    [HarmonyPatch(typeof(Medkit), nameof(Medkit.OnEffectsActivated))]
    public class MedkitHealPatch
    {
        [HarmonyTranspiler]
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instruction, ILGenerator generator)
        {
            var newInstruction = ListPool<CodeInstruction>.Shared.Rent(instruction);
            var index = newInstruction.FindIndex(i => 
                i.opcode == OpCodes.Callvirt 
                && i.operand == AccessTools.Method(typeof(HealthStat), nameof(HealthStat.ServerHeal)));

            var returnLabel = generator.DefineLabel();
            
            newInstruction.InsertRange(index, new CodeInstruction[]
            {
                new(OpCodes.Pop),
                new(OpCodes.Ldc_R4, 0f)
            });

            newInstruction[newInstruction.Count - 1].WithLabels(returnLabel);
            for (int z = 0; z < newInstruction.Count; z++)
                yield return newInstruction[z];
            
            ListPool<CodeInstruction>.Shared.Return(newInstruction);
        }
    }
}
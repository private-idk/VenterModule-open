using System.Collections.Generic;
using System.Reflection.Emit;
using HarmonyLib;
using InventorySystem.Items;
using InventorySystem.Items.Scp1509;
using NorthwoodLib.Pools;
using PlayerStatsSystem;

namespace VenterModuleLabApi.Patches
{
    //[HarmonyPatch(typeof(Scp1509Item), nameof(Scp1509Item.ServerApplyResurrectEffects))]
    public class ServerApplyResurrectEffectsPatch
    {
        /*[HarmonyTranspiler]
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instruction, ILGenerator generator)
        {
            var newInstruction = ListPool<CodeInstruction>.Shared.Rent(instruction);
            var index = newInstruction.FindIndex(i =>
                i.opcode == OpCodes.Ldfld
                && i.operand == AccessTools.Field(typeof(ItemBase), "get_Owner"));

            index -= 2;

            newInstruction[newInstruction.Count - 1].WithLabels(returnLabel);

            for (int z = 0; z < newInstruction.Count; z++)
                yield return newInstruction[z];
            
            ListPool<CodeInstruction>.Shared.Return(newInstruction);
        }*/
    }
}
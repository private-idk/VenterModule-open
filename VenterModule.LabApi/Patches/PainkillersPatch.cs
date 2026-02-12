using HarmonyLib;
using InventorySystem.Items.Usables;

namespace VenterModuleLabApi.Patches
{
    [HarmonyPatch(typeof(Painkillers), "OnEffectsActivated")]
    internal class PainkillersPatch
    {
        [HarmonyPrefix]
        static bool Prefix() => false;
    }
}

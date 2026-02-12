using HarmonyLib;
using LabApi.Features.Console;
using LabApi.Features.Wrappers;
using PlayerRoles.PlayableScps.Scp079;

namespace VenterModuleLabApi.Patches
{
    [HarmonyPatch(typeof(Scp079Recontainer), nameof(Scp079Recontainer.Start))]
    public class Scp079RecontainerPatch
    {
        internal static Scp079Recontainer RecontainerInstance;

        [HarmonyPostfix]
        static void Postfix(Scp079Recontainer __instance)
        {
            RecontainerInstance = __instance;
        }
    }
}
using InventorySystem;
using LabApi.Features.Wrappers;
using Mirror;
using PlayerRoles.FirstPersonControl;
using PlayerRoles.Ragdolls;
using PlayerStatsSystem;
using System.Linq;
using UnityEngine;
using UserSettings.ServerSpecific;
using VenterModuleLabApi.API.Extensions;

namespace VenterModuleLabApi.API.Features.ServerSpecific.Keybindings
{
    public class DeathPretendSpecific
    {       
        private void ProcessInput(ReferenceHub hub, ServerSpecificSettingBase settings)
        {
            if (!ServerSpecificExtensions.CheckKeybinding(settings, 23)) return;

            if (!hub.gameObject.TryGetComponent<DeathPretendController>(out var controller)) return;
            controller.ChangePretendCondition();
        }

        public void RegisterSS()
        {
            ServerSpecificSettingsSync.ServerOnSettingValueReceived += ProcessInput;
        }

        public void UnregisterSS()
        {
            ServerSpecificSettingsSync.ServerOnSettingValueReceived -= ProcessInput;
        }
    }
}

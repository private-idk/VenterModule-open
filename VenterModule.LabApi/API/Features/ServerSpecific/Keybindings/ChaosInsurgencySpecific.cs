using InventorySystem;
using LabApi.Features.Wrappers;
using UserSettings.ServerSpecific;
using VenterModuleLabApi.API.Extensions;
using VenterModuleLabApi.API.Features;

namespace VenterModuleLabApi.API.Features.ServerSpecific.Keybindings
{
    public class ChaosInsurgencySpecific
    {
        private void ProcessInput(ReferenceHub hub, ServerSpecificSettingBase settings)
        {
            if (!ServerSpecificExtensions.CheckKeybinding(settings, 26)) return;
            if (!Player.TryGet(hub.PlayerId, out Player player))
                return;

            if (!player.GameObject.TryGetComponent<ChaosInsurgencyKeycardController>(out var controller)
                || !player.Inventory.TryGetInventoryItem(ItemType.KeycardChaosInsurgency, out _))
                return;
        
            controller.RemindKeycardPermissions();
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
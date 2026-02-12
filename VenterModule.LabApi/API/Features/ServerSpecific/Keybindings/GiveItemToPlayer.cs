using LabApi.Features.Wrappers;
using UnityEngine;
using UserSettings.ServerSpecific;
using VenterModuleLabApi.API.Extensions;

namespace VenterModuleLabApi.API.Features.ServerSpecific.Keybindings
{
    public class GiveItemToPlayer
    {
        private void ProcessInput(ReferenceHub hub, ServerSpecificSettingBase settings)
        {
            if (!ServerSpecificExtensions.CheckKeybinding(settings, 24)) return;

            Player player = Player.Get(hub);

            if (player.IsSCP) return;
            
            if (!Physics.Raycast(player.Camera.position, player.Camera.forward, out var hit, 1.5f,
                    ~(1 << 1 | 1 << 13 | 1 << 16 | 1 << 28))
                || !Player.TryGet(hit.collider.gameObject, out var target)
                || target.IsInventoryFull
                || player.CurrentItem == null
                || target.CurrentItem != null) return;

            if (target.IsSCP) return;
            
            var pickup = player.DropItem(player.CurrentItem);
            target.CurrentItem = target.AddItem(pickup);
            pickup.Destroy();
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
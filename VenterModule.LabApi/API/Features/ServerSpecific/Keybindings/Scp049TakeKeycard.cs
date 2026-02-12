using InventorySystem.Items.Pickups;
using LabApi.Features.Wrappers;
using PlayerRoles;
using UnityEngine;
using UserSettings.ServerSpecific;
using VenterModuleLabApi.API.Extensions;

namespace VenterModuleLabApi.API.Features.ServerSpecific.Keybindings
{
    public class Scp049TakeKeycard
    {
        private void ProcessInput(ReferenceHub hub, ServerSpecificSettingBase settings)
        {
            if (!ServerSpecificExtensions.CheckKeybinding(settings, 18)) return;

            if (!IsScp049(hub)) return;

            Player player = Player.Get(hub);

            if (!Physics.Raycast(player.Camera.position, player.Camera.forward, out var hit, 1.8f, ~(1 << 1 | 1 << 13 | 1 << 16 | 1 << 28))) return;

            ItemPickupBase pickupBase = hit.collider.gameObject.GetComponentInParent<ItemPickupBase>();

            if (pickupBase == null) return;

            Pickup pickup = Pickup.Get(pickupBase);

            if (pickup.Category != ItemCategory.Keycard) return;

            if (player.CurrentItem != null) player.DropItem(player.CurrentItem);

            player.CurrentItem = player.AddItem(pickup);
            pickup.Destroy();
        }

        private bool IsScp049(ReferenceHub hub) => hub.roleManager.CurrentRole.RoleTypeId == RoleTypeId.Scp049;

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

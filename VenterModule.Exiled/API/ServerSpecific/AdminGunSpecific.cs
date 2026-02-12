using System;
using Exiled.API.Features;
using UnityEngine;
using UserSettings.ServerSpecific;

namespace VenterModuleExiled.Subroles.ServerSpecific
{
    public class AdminGunSpecific
    {
        private void Keybind(ReferenceHub referenceHub, ServerSpecificSettingBase settingBase)
        {
            if (settingBase is not SSKeybindSetting keybindSetting ||!keybindSetting.SyncIsPressed)
                return;
            if (!Player.TryGet(referenceHub, out Player player))
                return;
            
            if (!player.GameObject.TryGetComponent<AdminGunController>(out var controller)
                || !Plugin.Instance.EventHandlers.adminGun.Check(player.CurrentItem) || !player.RemoteAdminAccess) return;
            
            if (keybindSetting.SettingId == 51)
            {
                if (!Physics.Raycast(player.CameraTransform.position, player.CameraTransform.forward, out var raycastHit,
                        10f, ~(1 << 1 | 1 << 13 | 1 << 16 | 1 << 28)) 
                    || !Player.TryGet(raycastHit.collider.gameObject, out var target)) return;
                
                MakePlayer(controller, target);
            }
            else if (keybindSetting.SettingId == 52)
            {
                MakePlayer(controller, player);
            }
        }

        private void MakePlayer(AdminGunController controller, Player target)
        {
            string taskForce = controller.CustomTaskForce == String.Empty ? null : controller.CustomTaskForce;
            
            SubrolesManager.TryGiveSubrole(controller.Service, controller.Subrole, target, taskForce);
        }

        internal void RegisterSS()
        {
            ServerSpecificSettingsSync.ServerOnSettingValueReceived += Keybind;
        }

        internal void UnregisterSS()
        {
            ServerSpecificSettingsSync.ServerOnSettingValueReceived -= Keybind;
        }
    }
}
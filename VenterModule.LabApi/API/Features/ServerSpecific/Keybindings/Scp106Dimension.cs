using PlayerRoles;
using PlayerRoles.FirstPersonControl;
using PlayerRoles.PlayableScps.Scp106;
using UnityEngine;
using UserSettings.ServerSpecific;
using VenterModuleLabApi.API.Extensions;

namespace VenterModuleLabApi.API.Features.ServerSpecific.Keybindings
{
    public class Scp106Dimension
    {
        private Vector3 _scp106OldPosition = Vector3.zero;

        private void ProcessInput(ReferenceHub hub, ServerSpecificSettingBase settings)
        {
            if (!ServerSpecificExtensions.CheckKeybinding(settings, 17)) return;

            if (!IsScp106(hub)) return;
            if (!IsStalks(hub)) return;

            if (_scp106OldPosition == Vector3.zero)
            {
                _scp106OldPosition = (hub.roleManager.CurrentRole as IFpcRole).FpcModule.Position;
                hub.TryOverridePosition(new(0, -299, 0));
            }
            else
            {
                hub.TryOverridePosition(_scp106OldPosition);
                _scp106OldPosition = Vector3.zero;
            }
        }

        private bool IsScp106(ReferenceHub hub) => hub.roleManager.CurrentRole.RoleTypeId == RoleTypeId.Scp106;
        private bool IsStalks(ReferenceHub hub) => (hub.roleManager.CurrentRole as Scp106Role).IsStalking;

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

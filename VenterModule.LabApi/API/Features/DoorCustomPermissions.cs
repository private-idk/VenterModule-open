using System.Collections.Generic;
using Interactables.Interobjects.DoorUtils;
using LabApi.Features.Enums;
using LabApi.Features.Wrappers;

namespace VenterModuleLabApi.API.Features
{
    public static class DoorCustomPermissions
    {
        private static readonly Dictionary<DoorName, DoorPermissionFlags> _customDoorPermissions = new()
        {
            { DoorName.LczGr18Gate, DoorPermissionFlags.Intercom },
            { DoorName.Hcz096, DoorPermissionFlags.Intercom }
        };
        
        public static bool CheckPermissions(Door door, KeycardItem keycard, out bool isContains)
        {
            if (!_customDoorPermissions.ContainsKey(door.DoorName))
            {
                isContains = false;
                return false;
            }
            
            isContains = true;
            
            if (keycard == null || door.IsLocked) return false;
            
            if (keycard.Permissions.HasFlag(_customDoorPermissions[door.DoorName])) return true;
            else return false;
        }
    }
}
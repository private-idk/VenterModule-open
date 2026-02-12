using System.Collections.Generic;
using Interactables.Interobjects.DoorUtils;
using PlayerRoles;
using UnityEngine;
using VenterModuleExiled.Subroles.Enums;

namespace VenterModuleExiled.Subroles
{
    internal abstract class ServiceBase
    {
        internal virtual string Name { get; set; }
        internal virtual List<string> SubroleName { get; set; }
        internal virtual string CustomInfo { get; set; }
        internal virtual RoleTypeId Role { get; set; }
        internal virtual List<ItemType[]> Items { get; set; }
        internal virtual List<bool> IsUnique { get; set; }
        internal virtual NicknameTypeEnum NicknameType { get; set; }
        internal virtual RoleTypeId SpawnType { get; set; }
        internal virtual ServiceEnum Service { get; set; }
        internal virtual KeycardManager.KeycardData Keycard { get; set; }
        internal virtual bool HasKeycard { get; set; } = true;
        internal virtual List<DoorPermissionFlags> Permissions { get; set; }
    }
}

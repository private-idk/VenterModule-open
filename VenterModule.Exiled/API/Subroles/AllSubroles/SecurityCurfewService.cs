using System.Collections.Generic;
using Interactables.Interobjects.DoorUtils;
using PlayerRoles;
using UnityEngine;
using VenterModuleExiled.Subroles.Enums;

namespace VenterModuleExiled.Subroles.AllSubroles
{
    internal class SecurityCurfewService : ServiceBase
    {
        internal override string Name { get; set; } = "Служба безопасности | КО";
        internal override List<string> SubroleName { get; set; } = new() { "Мл. Офицер СБ | КО", "Офицер СБ | КО", "Ст. Офицер СБ | КО", "Капитан СБ | КО" };
        internal override string CustomInfo { get; set; } = "<size=14><color=#FFFFFF>Человек в <color=#727472>серой</color> униформе | На плече патч с надписью <color=#727472>\"%subrole%\"</color></color></size>";
        internal override RoleTypeId Role { get; set; } = RoleTypeId.FacilityGuard;
        internal override List<ItemType[]> Items { get; set; } = new()
        {
            new ItemType[] { ItemType.Medkit, ItemType.Flashlight, ItemType.Radio, ItemType.GunFSP9, ItemType.ArmorCombat, ItemType.GrenadeFlash },
            new ItemType[] { ItemType.Medkit, ItemType.Flashlight, ItemType.Radio, ItemType.GunFSP9, ItemType.ArmorCombat, ItemType.GrenadeFlash },
            new ItemType[] { ItemType.Medkit, ItemType.Flashlight, ItemType.Radio, ItemType.GunCrossvec, ItemType.ArmorCombat, ItemType.GrenadeFlash },
            new ItemType[] { ItemType.Medkit, ItemType.Flashlight, ItemType.Radio, ItemType.GunCrossvec, ItemType.ArmorCombat, ItemType.GrenadeFlash }
        };
        internal override List<bool> IsUnique { get; set; } = new() { false, false, false, true };
        internal override NicknameTypeEnum NicknameType { get; set; } = NicknameTypeEnum.Mtf;
        internal override RoleTypeId SpawnType { get; set; } = RoleTypeId.FacilityGuard;
        internal override ServiceEnum Service { get; set; } = ServiceEnum.SecurityCurfew;
        internal override KeycardManager.KeycardData Keycard { get; set; } = new()
        {
            ItemName = "Ключ-карта\n%replace%",
            KeycardColor = Color.cyan,
            LabelColor = Color.black,
            PermissionsColor = Color.black
        };
        internal override List<DoorPermissionFlags> Permissions { get; set; } = new()
        {
            (DoorPermissionFlags.Checkpoints | DoorPermissionFlags.ArmoryLevelOne | DoorPermissionFlags.ContainmentLevelOne),
            (DoorPermissionFlags.ExitGates | DoorPermissionFlags.ArmoryLevelTwo | DoorPermissionFlags.ContainmentLevelOne),
            (DoorPermissionFlags.ExitGates | DoorPermissionFlags.ArmoryLevelTwo | DoorPermissionFlags.ContainmentLevelTwo),
            (DoorPermissionFlags.ExitGates | DoorPermissionFlags.ArmoryLevelThree | DoorPermissionFlags.ContainmentLevelThree)
        };
    }
}

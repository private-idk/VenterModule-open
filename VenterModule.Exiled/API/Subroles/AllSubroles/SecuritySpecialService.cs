using System.Collections.Generic;
using Interactables.Interobjects.DoorUtils;
using PlayerRoles;
using UnityEngine;
using VenterModuleExiled.Subroles.Enums;

namespace VenterModuleExiled.Subroles.AllSubroles
{
    internal class SecuritySpecialService : ServiceBase
    {
        internal override string Name { get; set; } = "Служба безопасности | ОСН";
        internal override List<string> SubroleName { get; set; } = new() { "Мл. Офицер СБ | ОСН", "Офицер СБ | ОСН", "Ст. Офицер СБ | ОСН", "Капитан СБ | ОСН" };
        internal override string CustomInfo { get; set; } = "<size=14><color=#FFFFFF>Человек в <color=#727472>серой</color> униформе | На плече патч с надписью <color=#727472>\"%subrole%\"</color></color></size>";
        internal override RoleTypeId Role { get; set; } = RoleTypeId.FacilityGuard;
        internal override List<ItemType[]> Items { get; set; } = new()
        {
            new ItemType[] { ItemType.Medkit, ItemType.Flashlight, ItemType.Radio, ItemType.GunE11SR, ItemType.ArmorHeavy, ItemType.GrenadeFlash },
            new ItemType[] { ItemType.Medkit, ItemType.Flashlight, ItemType.Radio, ItemType.GunE11SR, ItemType.ArmorHeavy, ItemType.GrenadeFlash },
            new ItemType[] { ItemType.Medkit, ItemType.Flashlight, ItemType.Radio, ItemType.GunE11SR, ItemType.ArmorHeavy, ItemType.GrenadeFlash },
            new ItemType[] { ItemType.Medkit, ItemType.Flashlight, ItemType.Radio, ItemType.GunE11SR, ItemType.ArmorHeavy, ItemType.GrenadeFlash },
            new ItemType[] { ItemType.Medkit, ItemType.Flashlight, ItemType.Radio, ItemType.GunE11SR, ItemType.ArmorHeavy, ItemType.GrenadeFlash }
        };
        internal override List<bool> IsUnique { get; set; } = new() { false, false, false, true };
        internal override NicknameTypeEnum NicknameType { get; set; } = NicknameTypeEnum.Mtf;
        internal override RoleTypeId SpawnType { get; set; } = RoleTypeId.FacilityGuard;
        internal override ServiceEnum Service { get; set; } = ServiceEnum.SecuritySpecial;
        internal override KeycardManager.KeycardData Keycard { get; set; } = new()
        {
            ItemName = "Ключ-карта\n%replace%",
            KeycardColor = Color.blue,
            LabelColor = Color.black,
            PermissionsColor = Color.black
        };
        internal override List<DoorPermissionFlags> Permissions { get; set; } = new()
        {
            (DoorPermissionFlags.ExitGates | DoorPermissionFlags.ArmoryLevelTwo | DoorPermissionFlags.ContainmentLevelTwo),
            (DoorPermissionFlags.ExitGates | DoorPermissionFlags.ArmoryLevelTwo | DoorPermissionFlags.ContainmentLevelTwo),
            (DoorPermissionFlags.ExitGates | DoorPermissionFlags.ArmoryLevelThree | DoorPermissionFlags.ContainmentLevelThree),
            (DoorPermissionFlags.ExitGates | DoorPermissionFlags.ArmoryLevelThree | DoorPermissionFlags.ContainmentLevelThree)
        };
    }
}

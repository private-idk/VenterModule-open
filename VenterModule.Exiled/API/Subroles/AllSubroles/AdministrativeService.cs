using System.Collections.Generic;
using Interactables.Interobjects.DoorUtils;
using PlayerRoles;
using UnityEngine;
using VenterModuleExiled.Subroles.Enums;

namespace VenterModuleExiled.Subroles.AllSubroles
{
    internal class AdministrativeService : ServiceBase
    {
        internal override string Name { get; set; } = "Административная служба";
        internal override List<string> SubroleName { get; set; } = new() { "Бухгалтер", "Менеджер АЗ", "Архивариус", "Менеджер ЛЗС", "Менеджер ТЗС", "Руководитель Зоны 77-Д" };
        internal override string CustomInfo { get; set; } = "<size=14><color=#FFFFFF>Человек в <color=#FF6448>деловом</color> костюме | На груди бейджик с надписью <color=#FF6448>\"%subrole%\"</color></color></size>";
        internal override RoleTypeId Role { get; set; } = RoleTypeId.Scientist;
        internal override List<ItemType[]> Items { get; set; } = new()
        {
            new ItemType[] { ItemType.Medkit, ItemType.Radio, ItemType.Flashlight },
            new ItemType[] { ItemType.Medkit, ItemType.Radio, ItemType.Flashlight },
            new ItemType[] { ItemType.Medkit, ItemType.Radio, ItemType.Flashlight },
            new ItemType[] { ItemType.Medkit, ItemType.Radio, ItemType.Flashlight },
            new ItemType[] { ItemType.Medkit, ItemType.Radio, ItemType.Flashlight, ItemType.ArmorLight },
            new ItemType[] { ItemType.Medkit, ItemType.Radio, ItemType.Flashlight, ItemType.ArmorLight, ItemType.GunCOM18 }
        };
        internal override List<bool> IsUnique { get; set; } = new() { false, true, true, true, true, true };
        internal override NicknameTypeEnum NicknameType { get; set; } = NicknameTypeEnum.FacilityPersonnel;
        internal override RoleTypeId SpawnType { get; set; } = RoleTypeId.FacilityGuard;
        internal override ServiceEnum Service { get; set; } = ServiceEnum.Administrative;
        internal override KeycardManager.KeycardData Keycard { get; set; } = new()
        {
            ItemName = "Ключ-карта\n%replace%",
            KeycardColor = Color.green,
            LabelColor = Color.black,
            PermissionsColor = Color.black
        };
        internal override List<DoorPermissionFlags> Permissions { get; set; } = new()
        {
            (DoorPermissionFlags.Intercom),
            (DoorPermissionFlags.ExitGates | DoorPermissionFlags.ContainmentLevelOne),
            (DoorPermissionFlags.Checkpoints),
            (DoorPermissionFlags.ExitGates | DoorPermissionFlags.ContainmentLevelTwo),
            (DoorPermissionFlags.ExitGates | DoorPermissionFlags.ContainmentLevelThree),
            (DoorPermissionFlags.All),
        };
    }
}

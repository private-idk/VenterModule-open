using System.Collections.Generic;
using Interactables.Interobjects.DoorUtils;
using PlayerRoles;
using UnityEngine;
using VenterModuleExiled.Subroles.Enums;

namespace VenterModuleExiled.Subroles.AllSubroles
{
    internal class ScientistService : ServiceBase
    {
        internal override string Name { get; set; } = "Научная служба";
        internal override List<string> SubroleName { get; set; } = new() { "Мл. Научный сотрудник", "Научный сотрудник", "Ст. Научный сотрудник", "Глава научной службы" };
        internal override string CustomInfo  { get; set; } = "<size=14><color=#FFFFFF>Человек в <color=#FAFF86>белом</color> халате | На груди бейджик с надписью <color=#FAFF86>\"%subrole%\"</color></color></size>";
        internal override RoleTypeId Role { get; set; } = RoleTypeId.Scientist;
        internal override List<ItemType[]> Items { get; set; } = new()
        {
            new ItemType[] { ItemType.Medkit, ItemType.Radio, ItemType.Flashlight },
            new ItemType[] { ItemType.Medkit, ItemType.Radio, ItemType.Flashlight },
            new ItemType[] { ItemType.Medkit, ItemType.Radio, ItemType.Flashlight },
            new ItemType[] { ItemType.Medkit, ItemType.Radio, ItemType.Flashlight, ItemType.ArmorLight }
        };
        internal override List<bool> IsUnique { get; set; } = new() { false, false, false, true };
        internal override NicknameTypeEnum NicknameType { get; set; } = NicknameTypeEnum.Scientist;
        internal override RoleTypeId SpawnType { get; set; } = RoleTypeId.Scientist;
        internal override ServiceEnum Service { get; set; } = ServiceEnum.Scientist;

        internal override KeycardManager.KeycardData Keycard { get; set; } = new()
        {
            ItemName = "Ключ-карта\n%replace%",
            KeycardColor = Color.yellow,
            LabelColor = Color.black,
            PermissionsColor = Color.black
        };

        internal override List<DoorPermissionFlags> Permissions { get; set; } = new()
        {
            (DoorPermissionFlags.Checkpoints),
            (DoorPermissionFlags.Checkpoints | DoorPermissionFlags.ContainmentLevelOne),
            (DoorPermissionFlags.ExitGates | DoorPermissionFlags.ContainmentLevelTwo),
            (DoorPermissionFlags.ExitGates | DoorPermissionFlags.ContainmentLevelThree)
        };
    }
}

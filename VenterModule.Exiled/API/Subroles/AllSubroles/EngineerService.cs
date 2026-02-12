using System.Collections.Generic;
using Interactables.Interobjects.DoorUtils;
using PlayerRoles;
using UnityEngine;
using VenterModuleExiled.Subroles.Enums;

namespace VenterModuleExiled.Subroles.AllSubroles
{
    internal class EngineerService : ServiceBase
    {
        internal override string Name { get; set; } = "Инженерно-техническая служба";
        internal override List<string> SubroleName { get; set; } = new() { "Мл. Инженер", "Инженер", "Инженер камер содержания", "Начальник И-ТС" };
        internal override string CustomInfo { get; set; } = "<size=14><color=#FFFFFF>Человек в <color=#EE7600>оранжевом</color> комбинезоне | На груди бейджик с надписью <color=#EE7600>\"%subrole%\"</color></color></size>";
        internal override RoleTypeId Role { get; set; } = RoleTypeId.Tutorial;
        internal override List<ItemType[]> Items { get; set; } = new()
        {
            new ItemType[] { ItemType.Medkit, ItemType.Radio, ItemType.Flashlight },
            new ItemType[] { ItemType.Medkit, ItemType.Radio, ItemType.Flashlight },
            new ItemType[] { ItemType.Medkit, ItemType.Radio, ItemType.Flashlight },
            new ItemType[] { ItemType.Medkit, ItemType.Radio, ItemType.Flashlight, ItemType.ArmorLight }
        };
        internal override List<bool> IsUnique { get; set; } = new() { false, false, false, true };
        internal override NicknameTypeEnum NicknameType { get; set; } = NicknameTypeEnum.FacilityPersonnel;
        internal override RoleTypeId SpawnType { get; set; } = RoleTypeId.FacilityGuard;
        internal override ServiceEnum Service { get; set; } = ServiceEnum.Administrative;
        internal override KeycardManager.KeycardData Keycard { get; set; } = new()
        {
            ItemName = "Ключ-карта\n%replace%",
            KeycardColor = Color.grey,
            LabelColor = Color.black,
            PermissionsColor = Color.black
        };
        internal override List<DoorPermissionFlags> Permissions { get; set; } = new()
        {
            (DoorPermissionFlags.Checkpoints),
            (DoorPermissionFlags.ExitGates | DoorPermissionFlags.ContainmentLevelOne),
            (DoorPermissionFlags.ExitGates | DoorPermissionFlags.ContainmentLevelTwo),
            (DoorPermissionFlags.ExitGates | DoorPermissionFlags.ContainmentLevelThree)
        };
    }
}

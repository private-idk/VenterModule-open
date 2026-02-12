using System.Collections.Generic;
using Interactables.Interobjects.DoorUtils;
using PlayerRoles;
using UnityEngine;
using VenterModuleExiled.Subroles.Enums;

namespace VenterModuleExiled.Subroles.AllSubroles
{
    internal class MedicalService : ServiceBase
    {
        internal override string Name { get; set; } = "Медицинская служба";
        internal override List<string> SubroleName { get; set; } = new() { "Ассистент", "Штатный врач", "Врач-специалист", "Глава медицинской службы" };
        internal override string CustomInfo { get; set; } = "<size=14><color=#FFFFFF>Человек в <color=#DC143C>медицинском</color> халате | На груди бейджик с надписью <color=#DC143C>\"%subrole%\"</color></color></size>";
        internal override RoleTypeId Role { get; set; } = RoleTypeId.Scientist;
        internal override List<ItemType[]> Items { get; set; } = new()
        {
            new ItemType[] { ItemType.Medkit, ItemType.Radio, ItemType.Flashlight },
            new ItemType[] { ItemType.Medkit, ItemType.Medkit, ItemType.Radio, ItemType.Flashlight },
            new ItemType[] { ItemType.Medkit, ItemType.Medkit, ItemType.Radio, ItemType.Flashlight },
            new ItemType[] { ItemType.Medkit, ItemType.Radio, ItemType.Flashlight, ItemType.ArmorLight }
        };
        internal override List<bool> IsUnique { get; set; } = new() { false, false, false, true };
        internal override NicknameTypeEnum NicknameType { get; set; } = NicknameTypeEnum.FacilityPersonnel;
        internal override RoleTypeId SpawnType { get; set; } = RoleTypeId.FacilityGuard;
        internal override ServiceEnum Service { get; set; } = ServiceEnum.Medical;
        internal override KeycardManager.KeycardData Keycard { get; set; } = new()
        {
            ItemName = "Ключ-карта\n%replace%",
            KeycardColor = Color.red,
            LabelColor = Color.black,
            PermissionsColor = Color.black
        };
        internal override List<DoorPermissionFlags> Permissions { get; set; } = new()
        {
            (DoorPermissionFlags.Intercom),
            (DoorPermissionFlags.Checkpoints),
            (DoorPermissionFlags.ExitGates),
            (DoorPermissionFlags.ExitGates | DoorPermissionFlags.ContainmentLevelOne)
        };
    }
}

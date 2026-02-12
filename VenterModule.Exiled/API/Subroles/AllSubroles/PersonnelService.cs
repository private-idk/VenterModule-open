using System.Collections.Generic;
using Interactables.Interobjects.DoorUtils;
using PlayerRoles;
using UnityEngine;
using VenterModuleExiled.Subroles.Enums;

namespace VenterModuleExiled.Subroles.AllSubroles
{
    internal class PersonnelService : ServiceBase
    {
        internal override string Name { get; set; } = "Обслуживащий персонал";
        internal override List<string> SubroleName { get; set; } = new() { "Уборщик", "Повар" };
        internal override string CustomInfo { get; set; } = "<size=14><color=#FFFFFF>Человек в <color=#727472>рабочем</color> костюме | На груди бейджик с надписью <color=#727472>\"%subrole%\"</color></color></size>";
        internal override RoleTypeId Role { get; set; } = RoleTypeId.ClassD;
        internal override List<ItemType[]> Items { get; set; } = new()
        {
            new ItemType[] { ItemType.Medkit, ItemType.Radio, ItemType.Flashlight },
            new ItemType[] { ItemType.Medkit, ItemType.Radio, ItemType.Flashlight },
        };
        internal override List<bool> IsUnique { get; set; } = new() { false, false };
        internal override NicknameTypeEnum NicknameType { get; set; } = NicknameTypeEnum.FacilityPersonnel;
        internal override RoleTypeId SpawnType { get; set; } = RoleTypeId.FacilityGuard;
        internal override ServiceEnum Service { get; set; } = ServiceEnum.ServicePersonnel;
        internal override KeycardManager.KeycardData Keycard { get; set; } = new()
        {
            ItemName = "Ключ-карта\n%replace%",
            KeycardColor = new(224, 196, 112),
            LabelColor = Color.black,
            PermissionsColor = Color.black
        };
        internal override List<DoorPermissionFlags> Permissions { get; set; } = new()
        {
            (DoorPermissionFlags.Intercom),
            (DoorPermissionFlags.Intercom)
        };
    }
}

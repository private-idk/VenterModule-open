using System.Collections.Generic;
using Interactables.Interobjects.DoorUtils;
using PlayerRoles;
using UnityEngine;
using VenterModuleExiled.Subroles.Enums;

namespace VenterModuleExiled.Subroles.AllSubroles
{
    internal class ClassDService : ServiceBase
    {
        internal override string Name { get; set; } = "Класс-Д";
        internal override List<string> SubroleName { get; set; } = new() { "Категория-III", "Категория-II" };
        internal override string CustomInfo { get; set; } = "<size=14><color=#FFFFFF>Человек в <color=#EE7600>оражевом</color> комбинезоне | На запястье татуировка <color=#EE7600>\"%subrole%\"</color></color></size>";
        internal override RoleTypeId Role { get; set; } = RoleTypeId.ClassD;
        internal override List<ItemType[]> Items { get; set; } = new()
        {
            new ItemType[] { ItemType.Coin },
            new ItemType[] { ItemType.Coin },
        };
        internal override List<bool> IsUnique { get; set; } = new() { false, false };
        internal override NicknameTypeEnum NicknameType { get; set; } = NicknameTypeEnum.ClassD;
        internal override RoleTypeId SpawnType { get; set; } = RoleTypeId.ClassD;
        internal override ServiceEnum Service { get; set; } = ServiceEnum.ClassD;
        internal override bool HasKeycard { get; set; } = false;
    }
}

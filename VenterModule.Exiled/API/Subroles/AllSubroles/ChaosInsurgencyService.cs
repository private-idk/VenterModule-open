using System.Collections.Generic;
using PlayerRoles;
using UnityEngine;
using VenterModuleExiled.Subroles;
using VenterModuleExiled.Subroles.Enums;

namespace VenterModuleExiled.API.Subroles.AllSubroles
{
    internal class ChaosInsurgencyService : ServiceBase
    {
        internal override string Name { get; set; } = "Повстанцы хаоса";
        internal override List<string> SubroleName { get; set; } = new() { "Альфа-класс", "Бета-класс" };
        internal override string CustomInfo { get; set; } = "<size=14><color=#FFFFFF>Человек в <color=#228B22>зеленой</color> униформе | На лице противогаз M50 | На плече патч с буквой <color=#228B22>\"%subrole%\"</color></color></size>";
        internal override RoleTypeId Role { get; set; } = RoleTypeId.ChaosRepressor;
        internal override List<ItemType[]> Items { get; set; } = new()
        {
            new ItemType[] { ItemType.KeycardChaosInsurgency, ItemType.Medkit, ItemType.Radio, ItemType.GunAK, ItemType.ArmorCombat },
            new ItemType[] { ItemType.KeycardChaosInsurgency, ItemType.Medkit, ItemType.Radio, ItemType.GunLogicer, ItemType.ArmorHeavy, ItemType.GrenadeFlash },
        };
        internal override List<bool> IsUnique { get; set; } = new() { false, false };
        internal override NicknameTypeEnum NicknameType { get; set; } = NicknameTypeEnum.Mtf;
        internal override RoleTypeId SpawnType { get; set; } = RoleTypeId.ChaosRepressor;
        internal override ServiceEnum Service { get; set; } = ServiceEnum.ChaosInsurgency;
        internal override bool HasKeycard { get; set; } = false;
    }
}

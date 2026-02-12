using System.Collections.Generic;
using Interactables.Interobjects.DoorUtils;
using PlayerRoles;
using UnityEngine;
using VenterModuleExiled.Subroles;
using VenterModuleExiled.Subroles.Enums;

namespace VenterModuleExiled.API.Subroles.AllSubroles
{
    internal class CustomTaskForce : ServiceBase
    {
        internal override string Name { get; set; } = "Кастомная МОГ (ctf)";
        internal override List<string> SubroleName { get; set; } = new() { "Оперативник | ", "Парамедик | ", "Инженер | ", "Штурмовик | ", "Специалист | ", "Ст. Оперативник | ", "Капитан | " };
        internal override string CustomInfo { get; set; } = "<size=14><color=#FFFFFF>Человек в <color=#727472>серой</color> униформе | На плече патч с надписью <color=#727472>\"%subrole%\"</color></color></size>";
        internal override RoleTypeId Role { get; set; } = RoleTypeId.NtfSergeant;
        internal override List<ItemType[]> Items { get; set; } = new()
        {
            new ItemType[] { ItemType.Medkit, ItemType.Flashlight, ItemType.Radio, ItemType.GunE11SR, ItemType.ArmorHeavy, ItemType.GrenadeFlash },
            new ItemType[] { ItemType.Medkit, ItemType.Painkillers, ItemType.Flashlight, ItemType.Radio, ItemType.GunE11SR, ItemType.ArmorHeavy, ItemType.GrenadeFlash },
            new ItemType[] { ItemType.Medkit, ItemType.Flashlight, ItemType.Radio, ItemType.GunE11SR, ItemType.ArmorHeavy, ItemType.GrenadeFlash },
            new ItemType[] { ItemType.Medkit, ItemType.Flashlight, ItemType.Radio, ItemType.GunFRMG0, ItemType.ArmorHeavy, ItemType.GrenadeFlash },
            new ItemType[] { ItemType.Medkit, ItemType.Flashlight, ItemType.Radio, ItemType.GunE11SR, ItemType.ArmorHeavy, ItemType.GrenadeFlash, ItemType.GrenadeHE },
            new ItemType[] { ItemType.Medkit, ItemType.Flashlight, ItemType.Radio, ItemType.GunE11SR, ItemType.ArmorHeavy, ItemType.GrenadeFlash, ItemType.GrenadeHE },
            new ItemType[] { ItemType.Medkit, ItemType.Flashlight, ItemType.Radio, ItemType.GunE11SR, ItemType.ArmorHeavy, ItemType.GrenadeFlash, ItemType.GrenadeHE }
        };
        internal override List<bool> IsUnique { get; set; } = new() { false, false, false, false, false, false, true };
        internal override NicknameTypeEnum NicknameType { get; set; } = NicknameTypeEnum.Mtf;
        internal override RoleTypeId SpawnType { get; set; } = RoleTypeId.NtfSergeant;
        internal override ServiceEnum Service { get; set; } = ServiceEnum.CustomTaskForce;
        internal override KeycardManager.KeycardData Keycard { get; set; } = new()
        {
            ItemName = "Ключ-карта\n%replace%",
            KeycardColor = Color.cyan,
            LabelColor = Color.black,
            PermissionsColor = Color.black
        };
        internal override List<DoorPermissionFlags> Permissions { get; set; } = new()
        {
            (DoorPermissionFlags.All),
            (DoorPermissionFlags.All),
            (DoorPermissionFlags.All),
            (DoorPermissionFlags.All),
            (DoorPermissionFlags.All),
            (DoorPermissionFlags.All),
            (DoorPermissionFlags.All)
        };
    }
}
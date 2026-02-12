using System;
using System.Collections.Generic;
using System.Linq;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.CustomItems.API.Features;
using Interactables.Interobjects.DoorUtils;
using UnityEngine;
using VenterModuleExiled.API.Subroles.AllSubroles;
using VenterModuleExiled.CustomItems;
using VenterModuleExiled.Subroles.AllSubroles;
using VenterModuleExiled.Subroles.Enums;

namespace VenterModuleExiled.Subroles
{
    internal static class SubrolesManager
    {
        internal static Dictionary<ServiceEnum, Type> ServicesDictionary = new()
        {
            { ServiceEnum.Scientist, typeof(ScientistService) },
            { ServiceEnum.SecurityCurfew, typeof(SecurityCurfewService) },
            { ServiceEnum.SecuritySpecial, typeof(SecuritySpecialService) },
            { ServiceEnum.Administrative, typeof(AdministrativeService) },
            { ServiceEnum.Medical, typeof(MedicalService) },
            { ServiceEnum.Engineer, typeof(EngineerService) },
            { ServiceEnum.ServicePersonnel, typeof(PersonnelService) },
            { ServiceEnum.ClassD, typeof(ClassDService) },
            { ServiceEnum.ChaosInsurgency, typeof(ChaosInsurgencyService) },
            { ServiceEnum.CustomTaskForce, typeof(CustomTaskForce) }
        };

        internal static Dictionary<int, string> UniqueSubroles = new();
        internal static Dictionary<int, string> PlayerSubroles = new();

        private static Dictionary<ServiceEnum, Type> _serviceCustomItems = new()
        {
            { ServiceEnum.SecurityCurfew, typeof(Taser) },
            { ServiceEnum.SecuritySpecial, typeof(Taser) }
        };

        internal static bool TryGiveSubrole(ServiceEnum service, int subrole, Player player, string taskForce)
        {
            RemoveExists(player);

            ServiceBase serviceBase = GetServiceBase(ServicesDictionary[service]);

            if (serviceBase.IsUnique[subrole])
            {
                if (UniqueSubroles.ContainsValue(serviceBase.SubroleName[subrole])) return false;

                UniqueSubroles.Add(player.Id, serviceBase.SubroleName[subrole]);
            }

            SetPlayer(player, serviceBase, subrole, taskForce);

            return true;
        }

        internal static ServiceBase GetServiceBase(Type type)
        {
            return (ServiceBase)Activator.CreateInstance(type);
        }

        internal static void RemoveExists(Player player)
        {
            if (PlayerSubroles.ContainsKey(player.Id))
            {
                if (UniqueSubroles.ContainsKey(player.Id))
                {
                    UniqueSubroles.Remove(player.Id);
                }
                PlayerSubroles.Remove(player.Id);
            }
        }

        private static void SetPlayer(Player player, ServiceBase serviceBase, int subrole, string customForceName)
        {
            if (serviceBase.Service == ServiceEnum.CustomTaskForce)
            {
                if (customForceName == null) return;
                serviceBase = CustomTaskForceJob(serviceBase, customForceName);
            }
            
            player.Role.Set(serviceBase.Role, PlayerRoles.RoleSpawnFlags.None);
            player.CustomInfo = serviceBase.CustomInfo.Replace("%subrole%", serviceBase.SubroleName[subrole]);

            GiveItems(player, serviceBase.Items[subrole]);
            SpecialItems(serviceBase, player);
            SetNickname(player, serviceBase.NicknameType, serviceBase);
            KeycardManager.CreateKeycard(player, serviceBase, subrole);

            PlayerSubroles.Add(player.Id, serviceBase.SubroleName[subrole]);
        }

        private static ServiceBase CustomTaskForceJob(ServiceBase serviceBase, string customForceName)
        {
            serviceBase.Name = customForceName;

            List<string> Names = new();
                
            foreach (var subroleName in serviceBase.SubroleName)
            {
                Names.Add(subroleName + serviceBase.Name);
            }

            serviceBase.SubroleName = Names;
            
            return serviceBase;
        }

        private static void GiveItems(Player player, ItemType[] items)
        {
            player.ClearInventory();
            foreach (var item in items)
            {
                player.AddItem(item);
            }

            if (player.Role.Type == PlayerRoles.RoleTypeId.NtfSergeant || player.Role.Type == PlayerRoles.RoleTypeId.FacilityGuard)
            {
                player.AddAmmo(Exiled.API.Enums.AmmoType.Nato556, 120);
                player.AddAmmo(Exiled.API.Enums.AmmoType.Nato9, 90);
            }
            else if (player.Role.Type == PlayerRoles.RoleTypeId.ChaosRepressor)
            {
                player.AddAmmo(Exiled.API.Enums.AmmoType.Nato762, 180);
            }
        }

        private static void SpecialItems(ServiceBase serviceBase, Player player)
        {
            var service = serviceBase.Service;

            if (!_serviceCustomItems.ContainsKey(service)) return;

            var type = _serviceCustomItems[service];
            var item = (CustomItem) Activator.CreateInstance(type);

            CustomItem.TryGive(player, item.Id);
        }

        private static void SetNickname(Player player, NicknameTypeEnum type, ServiceBase serviceBase)
        {
            switch(type) 
            {
                case NicknameTypeEnum.ClassD:
                    player.DisplayNickname = $"«{player.Id}» D-{UnityEngine.Random.Range(1000, 9999)}";
                    break;
                case NicknameTypeEnum.Scientist:
                    player.DisplayNickname = $"«{player.Id}» Д-р {Plugin.Instance.Config.SnamesList.RandomItem()}";
                    break;
                case NicknameTypeEnum.FacilityPersonnel:
                    player.DisplayNickname = $"«{player.Id}» {Plugin.Instance.Config.NamesList.RandomItem()} {Plugin.Instance.Config.SnamesList.RandomItem()}";
                    break;
                case NicknameTypeEnum.Mtf:
                    player.DisplayNickname = $"«{player.Id}» \"{Plugin.Instance.Config.CallSigns.RandomItem()}\"";
                    break;
            }
        }
    }
}

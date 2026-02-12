using System.Collections.Generic;
using System.Linq;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Doors;
using RueI.API;
using RueI.API.Elements;

namespace VenterModuleExiled.Extensions
{
    internal static class WorkdayExtensions
    {
        internal static bool IsStarted { get; set; } = false;

        internal static void StartWorkday()
        {
            if (IsStarted) return;

            Cassie.Message(Plugin.Instance.Config.RoundStartCassie, true, true, true);

            Plugin.Instance.CassieFunctions = true;

            UnlockAllDoors();
            LockCurrentDoors(Plugin.Instance.Config.LockDoors);
            TogglePrisonDoors(true);
            ScpWork();

            IsStarted = true;
        }

        private static void UnlockAllDoors()
        {
            foreach (var door in Door.List)
            {
                door.Unlock();
            }
        }

        private static void ScpWork()
        {
            foreach (var player in Player.List.Where(p => p.IsScp))
            {
                var el = new BasicElement(200f, "<b>Вы <color=red>объект</color> ожидайте НОУС'а</b>");
                
                RueDisplay.Get(player).Show(el, 10f);
                
                player.Role.Set(player.Role.Type, PlayerRoles.RoleSpawnFlags.All);
                player.EnableEffect(EffectType.Invisible);
                player.EnableEffect(EffectType.Ensnared);
            }
        }

        private static void LockCurrentDoors(HashSet<DoorType> doors)
        {
            foreach(var door in doors)
            {
                Door.List.First(d => d.Type == door).ChangeLock(DoorLockType.AdminCommand);
            }
        }

        internal static void TogglePrisonDoors(bool isLock)
        {
            if (isLock)
            {
                foreach (var door in Door.List)
                {
                    if (door.Type == DoorType.PrisonDoor)
                    {
                        door.ChangeLock(DoorLockType.AdminCommand);
                    }
                }
            }
            else
            {
                foreach (var door in Door.List)
                {
                    if (door.Type == DoorType.PrisonDoor)
                    {
                        door.Unlock();
                    }
                }
            }
        }
    }
}

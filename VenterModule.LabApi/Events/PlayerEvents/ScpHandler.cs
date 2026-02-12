using System.Linq;
using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.Arguments.Scp049Events;
using LabApi.Events.Arguments.Scp106Events;
using LabApi.Events.Arguments.Scp3114Events;
using LabApi.Events.Arguments.Scp939Events;
using LabApi.Features.Wrappers;
using PlayerRoles;
using PlayerStatsSystem;
using RueI.API;
using RueI.API.Elements;
using UnityEngine;
using VenterModuleLabApi.API.Extensions;
using VenterModuleLabApi.API.Features;

namespace VenterModuleLabApi.Events.PlayerEvents
{
    public class ScpHandler
    {
        private void OnChangingVigor(Scp106ChangingVigorEventArgs ev) => ev.Value = 9999f;

        private void OnInteractingDoor(PlayerInteractingDoorEventArgs ev)
        {
            if (ev.Player.Role == RoleTypeId.Scp173)
            {
                bool isFound = false;

                foreach (var p in Player.List.Where(p => !p.IsSCP && p.IsAlive && p != ev.Player))
                {
                    if (p.Room == ev.Player.Room || ev.Player.Room.ConnectedRooms.Contains(p.Room.Base))
                    {
                        isFound = true;
                        break;
                    }
                }
                
                if (isFound)
                {
                    ev.IsAllowed = false;
                    RueDisplay.Get(ev.Player).Show(new BasicElement(200f, "<b>Вы не можете <color=yellow>открыть дверь</color>\nВ комнате поблизости <color=yellow>находится игрок</color></b>"), 1f);
                }
                
                return;
            }
            
            if (!ev.Player.IsSCP) return;
            ev.IsAllowed = VenterModule.Instance.Config.IntercationRoles.Contains(ev.Player.Role);
        }
        private void OnInteractingElevator(PlayerInteractingElevatorEventArgs ev)
        {
            if (!ev.Player.IsSCP) return;
            ev.IsAllowed = VenterModule.Instance.Config.IntercationRoles.Contains(ev.Player.Role);
        }

        private void OnDisguising(Scp3114DisguisingEventArgs ev)
        {
            var pretending = Player.List.FirstOrDefault(p =>
                p.GameObject.TryGetComponent<DeathPretendController>(out var controller) &&
                controller.currentCondition &&
                Ragdoll.Get(controller.ragdoll) == ev.Ragdoll);
            
            if (pretending != null)
            {
                pretending.GameObject.GetComponent<DeathPretendController>().ChangePretendCondition();
                ev.IsAllowed = false;
            }
        }

        private void OnDisguised(Scp3114DisguisedEventArgs ev)
        {
            if (!ev.Ragdoll.Base.TryGetComponent<RagdollAdditionalInformation>(out var information)) return;

            ev.Player.CustomInfo = information.customInfo;
            Object.Destroy(information);
        }
        
        private void OnLeavingPocketDimension(PlayerLeavingPocketDimensionEventArgs ev)
        {
            if (ev.Player.Role != PlayerRoles.RoleTypeId.Scp106) return;

            ev.IsAllowed = false;
            ev.Player.Position = new(0, -299f, 0);
        }
        
        private void OnLungedEventArgs(Scp939LungedEventArgs ev) => Scp939Extensions.TryDamageDoor(ev.LungeState, ev.Player);

        private void OnSpawned(PlayerSpawnedEventArgs ev)
        {
            if (!ev.Player.IsSCP) return;

            ev.Player.MaxHumeShield = 0;
            ev.Player.HumeShield = 0;

            if (VenterModule.Instance.Config.ScpHealth.TryGetValue(ev.Player.Role, out var health))
            {
                ev.Player.MaxHealth = health;
                ev.Player.Health = health;
            }
        }

        private void OnScp049Attacked(Scp049AttackedEventArgs ev)
        {
            ev.Target.Damage(new Scp049DamageHandler(ev.Player.ReferenceHub, -1, Scp049DamageHandler.AttackType.Instakill));
        }
        
        public void RegisterEvents()
        {
            LabApi.Events.Handlers.Scp106Events.ChangingVigor += OnChangingVigor;
            LabApi.Events.Handlers.PlayerEvents.InteractingDoor += OnInteractingDoor;
            LabApi.Events.Handlers.PlayerEvents.InteractingElevator += OnInteractingElevator;
            LabApi.Events.Handlers.PlayerEvents.LeavingPocketDimension += OnLeavingPocketDimension;
            LabApi.Events.Handlers.Scp939Events.Lunged += OnLungedEventArgs;
            LabApi.Events.Handlers.Scp3114Events.Disguising += OnDisguising;
            LabApi.Events.Handlers.Scp3114Events.Disguised += OnDisguised;
            LabApi.Events.Handlers.PlayerEvents.Spawned += OnSpawned;
        }

        public void UnregisterEvents()
        {
            LabApi.Events.Handlers.Scp106Events.ChangingVigor -= OnChangingVigor;
            LabApi.Events.Handlers.PlayerEvents.InteractingDoor -= OnInteractingDoor;
            LabApi.Events.Handlers.PlayerEvents.InteractingElevator -= OnInteractingElevator;
            LabApi.Events.Handlers.PlayerEvents.LeavingPocketDimension -= OnLeavingPocketDimension;
            LabApi.Events.Handlers.Scp939Events.Lunged -= OnLungedEventArgs;
            LabApi.Events.Handlers.Scp3114Events.Disguising -= OnDisguising;
            LabApi.Events.Handlers.Scp3114Events.Disguised -= OnDisguised;
            LabApi.Events.Handlers.PlayerEvents.Spawned -= OnSpawned;
        }
    }
}

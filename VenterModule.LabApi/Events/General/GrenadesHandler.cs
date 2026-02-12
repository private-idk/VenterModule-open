using System.Linq;
using CustomPlayerEffects;
using Interactables.Interobjects.DoorUtils;
using InventorySystem.Items.Armor;
using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.Arguments.ServerEvents;
using LabApi.Features.Wrappers;
using MEC;
using PlayerRoles;
using PlayerRoles.PlayableScps.Scp106;
using UnityEngine;

//using Logger = LabApi.Features.Console.Logger;

namespace VenterModuleLabApi.Events.ServerEvents
{
    public class GrenadesHandler
    {
        private void OnThrewProjectile(PlayerThrewProjectileEventArgs ev)
        {
            var grenade = ev.Projectile as TimedGrenadeProjectile;

            if (grenade != null)
            {
                grenade.RemainingTime = 2.5f;
            }
        }
        
        private void OnDoorDamaging(DoorDamagingEventArgs ev)
        {
            if (ev.DamageType != DoorDamageType.Grenade) return;

            ev.IsAllowed = false;
        }
        
        private void OnProjectileExploded(ProjectileExplodedEventArgs ev)
        {
            foreach (var player in Player.List)
            {
                if (player.IsSCP)
                {
                    if (player.RoleBase is Scp106Role role
                        && Vector3.Distance(player.Position, ev.TimedGrenade.Position) <= 5f
                        && role.SubroutineModule.TryGetSubroutine<Scp106StalkAbility>(out var stalkAbility)
                        && !stalkAbility.StalkActive)
                    {
                        stalkAbility.ServerSetStalk(true);
                        return;
                    }
                }
            }
            
            float coefficient = ev.TimedGrenade.Type == ItemType.GrenadeFlash ? 4 : 2;

            foreach (var p in Player.List.Where(x => Vector3.Distance(x.Position, ev.Position) < 15f 
                                                     && (!x.ReferenceHub.inventory.TryGetBodyArmor(out var armor) || armor.ItemTypeId != ItemType.ArmorHeavy)))
            {
                float distance = Vector3.Distance(ev.Position, p.Position);
                
                float multiplier = distance < 1f ? 1f : distance;

                if (ev.TimedGrenade.Room != p.Room) continue;
                
                p.ReferenceHub.playerEffectsController.TryGetEffect("Blurred", out var blur);
                p.ReferenceHub.playerEffectsController.TryGetEffect("Deafened", out var deaf);
                p.ReferenceHub.playerEffectsController.TryGetEffect("Flashed", out var flash);

                if (flash.IsEnabled)
                {
                    Timing.CallDelayed(flash.TimeLeft, () =>
                    {
                        SetEffects(new[] { blur, deaf }, coefficient, multiplier);
                    });
                }
                else
                {
                    SetEffects(new[] { blur, deaf }, coefficient, multiplier);
                }
            }
        }

        private void SetEffects(StatusEffectBase[] effects, float coeff, float mult)
        {
            foreach (var effect in effects) effect.ServerSetState(1, (15 - mult) * coeff, true);
        }
        
        public void RegisterEvents()
        {
            LabApi.Events.Handlers.ServerEvents.ProjectileExploded += OnProjectileExploded;
            LabApi.Events.Handlers.ServerEvents.DoorDamaging += OnDoorDamaging;
            LabApi.Events.Handlers.PlayerEvents.ThrewProjectile += OnThrewProjectile;
        }

        public void UnregisterEvents()
        {
            LabApi.Events.Handlers.ServerEvents.ProjectileExploded -= OnProjectileExploded;
            LabApi.Events.Handlers.ServerEvents.DoorDamaging -= OnDoorDamaging;
            LabApi.Events.Handlers.PlayerEvents.ThrewProjectile -= OnThrewProjectile;
        }
    }
}
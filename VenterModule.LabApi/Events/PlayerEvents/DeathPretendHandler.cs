using System;
using System.Linq;
using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Features.Wrappers;
using PlayerRoles.Ragdolls;
using UnityEngine;
using VenterModuleLabApi.API.Features;

namespace VenterModuleLabApi.Events.PlayerEvents
{
    public class DeathPretendHandler
    {
        private void OnShooting(PlayerShootingWeaponEventArgs ev)
        {
            if (ev.FirearmItem.Type == ItemType.GunRevolver) return;
            
            if (!Physics.Raycast(ev.Player.Camera.position, ev.Player.Camera.forward, out var hit, 10f, ~(1 << 1 | 1 << 13 | 1 << 16 | 1 << 28))) return;

            var ragdoll = hit.collider.transform.root.GetComponentInChildren<BasicRagdoll>();

            if (ragdoll == null) return;

            var player = Player.List.FirstOrDefault(p => p.GameObject.TryGetComponent<DeathPretendController>(out var controller)
            && controller.ragdoll == ragdoll && controller.currentCondition);

            if (player == null || !player.IsAlive) return;

            if (player.Health <= player.MaxHealth / 2) ProcessDeath(player);

            player.Damage(player.MaxHealth / 2, String.Empty);
        }

        private void OnDying(PlayerDyingEventArgs ev)
        {
            if (ev.Player.GameObject.TryGetComponent<DeathPretendController>(out var controller) && controller.currentCondition)
                ProcessDeath(ev.Player);
        }

        private void ProcessDeath(Player player) => player.GameObject.GetComponent<DeathPretendController>()?.ProcessDeath();

        public void RegisterEvents()
        {
            LabApi.Events.Handlers.PlayerEvents.ShootingWeapon += OnShooting;
            LabApi.Events.Handlers.PlayerEvents.Dying += OnDying;
        }
        public void UnregisterEvents()
        {
            LabApi.Events.Handlers.PlayerEvents.ShootingWeapon -= OnShooting;
            LabApi.Events.Handlers.PlayerEvents.Dying -= OnDying;
        }
    }
}

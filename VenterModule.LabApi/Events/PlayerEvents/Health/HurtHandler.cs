using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using InventorySystem;
using InventorySystem.Items.Armor;
using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.CustomHandlers;
using LabApi.Features.Console;
using LabApi.Features.Wrappers;
using MEC;
using PlayerRoles;
using PlayerRoles.PlayableScps.Scp3114;
using PlayerRoles.PlayableScps.Scp939;
using PlayerStatsSystem;
using VenterModuleLabApi.API.Features.Behaviours;

namespace VenterModuleLabApi.Events.PlayerEvents
{
    public class HurtHandler : CustomEventsHandler
    {
        public override void OnPlayerHurting(PlayerHurtingEventArgs ev)
        {
            if (ev.Player == null
                || ev.Attacker == null
                || ev.Player.IsSCP)
                return;

            bool isBleed = ev.Player.ArtificialHealth <= 0;
            int bleedTime = 0;
            
            if (ev.DamageHandler is FirearmDamageHandler firearmDamageHandler)
            {
                if (firearmDamageHandler.Damage == 0)
                    return;
                
                if (firearmDamageHandler.Hitbox == HitboxType.Headshot)
                {
                    isBleed = false;

                    float requiredDamage;

                    if (ev.Player.Inventory.TryGetBodyArmor(out var bodyArmor))
                    {
                        if (bodyArmor.HelmetEfficacy >= 80)
                        {
                            requiredDamage = 80f;

                            ev.Player.ReferenceHub.playerEffectsController.TryGetEffect("Slowness", out var slow);
                            ev.Player.ReferenceHub.playerEffectsController.TryGetEffect("Flashed", out var flash);
                            slow.ServerSetState(30, 120, true);
                            flash.ServerSetState(1, 3);

                            int lifeId = ev.Player.LifeId;
                            Timing.CallDelayed(3f, () =>
                            {
                                if (lifeId == ev.Player.LifeId)
                                    Timing.RunCoroutine(ProcessBlindness(ev.Player));
                            });
                        }
                        else
                        {
                            requiredDamage = 100f;
                        }
                    }
                    else
                        requiredDamage = 150f;

                    if (requiredDamage != 0f)
                    {
                        float dealDamage = requiredDamage;

                        if (ev.Player.Inventory.TryGetBodyArmor(out var armor))
                            dealDamage = (armor.HelmetEfficacy / 100 + 1) * requiredDamage;

                        firearmDamageHandler.Damage = dealDamage;
                    }
                }
                else if (firearmDamageHandler.Hitbox == HitboxType.Limb)
                {
                    ev.Player.ReferenceHub.playerEffectsController.TryGetEffect("Slowness", out var slow);
                    slow.ServerSetState((byte)(firearmDamageHandler.Damage / 10), 15, true);
                }
            }
            else if (ev.DamageHandler is Scp939DamageHandler scp939DamageHandler)
            {
                if (scp939DamageHandler.Damage == 0)
                    return;

                if (scp939DamageHandler.Hitbox == HitboxType.Limb)
                {
                    isBleed = true;
                    bleedTime = 30;
                }
            }
            else if (ev.DamageHandler is Scp3114DamageHandler scp3114DamageHandler)
            {
                if (scp3114DamageHandler.Damage == 0)
                    return;
                
                if (scp3114DamageHandler.Hitbox == HitboxType.Limb)
                {
                    isBleed = true;
                    bleedTime = 10;
                }
            }
            else if (ev.DamageHandler is ScpDamageHandler scpDamageHandler &&
                     scpDamageHandler.Attacker.Role == RoleTypeId.Scp0492)
            {
                if (scpDamageHandler.Damage == 0)
                    return;
                
                if (scpDamageHandler.Hitbox == HitboxType.Limb)
                {
                    isBleed = true;
                    bleedTime = 15;
                }
            }
            
            if (isBleed)
            {
                if (!ev.Player.GameObject.TryGetComponent<BleedingController>(out var controller))
                    return;
                
                if (controller.isBleeding) controller.intensity += 1;
                else controller.ChangeBleeding(bleedTime, true);
            }
            
            base.OnPlayerHurting(ev);
        }

        private IEnumerator<float> ProcessBlindness(Player player)
        {
            int lifeId = player.LifeId;
            
            player.ReferenceHub.playerEffectsController.TryGetEffect("Blindness", out var blind);

            float elapsed = 0f;
            while (elapsed < 8f)
            {
                if (lifeId != player.LifeId) yield break;
                
                blind.ServerSetState((byte)(elapsed * 10f), 0.5f);

                elapsed += Timing.DeltaTime;
                yield return Timing.WaitForOneFrame;
            }
            
            if (lifeId == player.LifeId)
                blind.ServerSetState(80, 90f);
        }
    }
}
using Interactables.Interobjects;
using LabApi.Features.Wrappers;
using PlayerRoles.PlayableScps.Scp939;
using System;
using UnityEngine;

namespace VenterModuleLabApi.API.Extensions
{
    public class Scp939Extensions
    {
        public static void TryDamageDoor(Scp939LungeState state, Player player)
        {
            if (state == Scp939LungeState.Triggered)
            {
                if (!Physics.Raycast(player.Camera.position, player.Camera.forward, out var hit, 5f, ~(1 << 1 | 1 << 13 | 1 << 16 | 1 << 28)))
                {
                    return;
                }

                BasicDoor basicDoor = hit.collider.gameObject.GetComponentInParent<BasicDoor>();

                if (basicDoor == null || basicDoor is not Interactables.Interobjects.BreakableDoor door || door == null)
                {
                    return;
                }

                door.ServerDamage((float)Math.Ceiling(door.MaxHealth / 2), Interactables.Interobjects.DoorUtils.DoorDamageType.Scp096); ;
            }
        }
    }
}

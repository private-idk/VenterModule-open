using System.Collections.Generic;
using InventorySystem.Items.Armor;
using LabApi.Features.Wrappers;
using MEC;
using PlayerRoles;
using PlayerRoles.FirstPersonControl;
using PlayerRoles.FirstPersonControl.Thirdperson;
using PlayerRoles.FirstPersonControl.Thirdperson.Subcontrollers.OverlayAnims;
using RueI.API;
using RueI.API.Elements;
using UnityEngine;

namespace VenterModuleLabApi.API.Features
{
    public class PushTracker : MonoBehaviour
    {
        public float Cooldown => VenterModule.Instance.Config.PushCooldown;
        public Dictionary<ItemType, float> ArmorCoefficient => VenterModule.Instance.Config.ArmorCoefficient;
        
        public ReferenceHub hub => GetComponent<ReferenceHub>();
        public Player player => Player.Get(hub);

        public float lastPushTime { get; set; }

        private void Awake()
        {
            lastPushTime = Time.time;
        }

        public void PressPush()
        {
            if (Time.time - lastPushTime < Cooldown) return;
            lastPushTime = Time.time;
            
            var target = GetLookedPlayer();

            if (target == null
                || Vector3.Distance(target.Position, player.Position) <= 0.1f) return;
            
            var fpcRole = target.RoleBase as IFpcRole;
            
            float coefficient = 1;

            if (target.IsSCP) coefficient = 0.5f;
            
            if (target.ReferenceHub.inventory.TryGetBodyArmor(out var armor)) coefficient = ArmorCoefficient[armor.ItemTypeId];

            Vector3 forceDirection = hub.PlayerCameraReference.forward.NormalizeIgnoreY();
            float force = coefficient;

            WaypointToy waypoint = WaypointToy.Create(target.Position);
            waypoint.BoundsSize = new(0.2f, 0.1f, 0.2f);
            Timing.RunCoroutine(DragCoroutine(waypoint, target.Position + forceDirection * force, 0.3f));
            
            OverlayAnimationsSubcontroller subcontroller;
            if (!(hub.roleManager.CurrentRole is IFpcRole currentRole) ||
                !(currentRole.FpcModule.CharacterModelInstance is AnimatedCharacterModel
                    characterModelInstance) ||
                !characterModelInstance.TryGetSubcontroller<OverlayAnimationsSubcontroller>(out subcontroller))
            {
                return;
            }
            subcontroller._overlayAnimations[1].OnStarted();
            subcontroller._overlayAnimations[1].SendRpc();

            RueDisplay.Get(target)
                .Show(new BasicElement(200f, $"<b>Вас толкнул <color=yellow>{player.DisplayName}</color></b>"), 0.5f);
        }

        private IEnumerator<float> DragCoroutine(WaypointToy waypoint, Vector3 targetPosition, float duration)
        {
            Vector3 startPosition = waypoint.Position;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                waypoint.Position = Vector3.Lerp(startPosition, targetPosition, elapsed / duration);
                elapsed += Timing.DeltaTime;
                yield return Timing.WaitForOneFrame;
            }

            waypoint.Position = targetPosition;
            waypoint.Destroy();
        }
        
        private Player GetLookedPlayer()
        {
            if (!Physics.Raycast(player.Camera.position, player.Camera.forward, out var hit, 1.5f,
                    ~(1 << 1 | 1 << 13 | 1 << 16 | 1 << 28))
                || !Player.TryGet(hit.collider.gameObject, out var target)) return null;
            
            return target;
        }
    }
}
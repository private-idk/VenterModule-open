using Interactables.Interobjects.DoorUtils;
using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Features.Wrappers;
using PlayerRoles.FirstPersonControl;
using PlayerRoles.FirstPersonControl.Thirdperson;
using PlayerRoles.FirstPersonControl.Thirdperson.Subcontrollers.OverlayAnims;
using VenterModuleLabApi.API.Features;

namespace VenterModuleLabApi.Events.PlayerEvents
{
    public class DoorsInteractionHandler
    {
        private void OnInteractingDoor(PlayerInteractingDoorEventArgs ev)
        {
            if (ev.Door.Permissions == DoorPermissionFlags.None)
                PlayGrabAnimation(ev.Player);

            if (ev.Player.CurrentItem == null)
                return;
            
            if (ev.Player.CurrentItem != null && ev.Player.CurrentItem.Type == ItemType.KeycardChaosInsurgency)
            {
                if (ev.Player.GameObject.TryGetComponent<ChaosInsurgencyKeycardController>(out var controller))
                {
                    ev.IsAllowed = false;
                    if (!controller.TryOpenDoor(ev.Door))
                    {
                        controller.SelectMagneticFieldStrenght(ev.Door);
                        return;
                    }
                }

                return;
            }
            
            if (ev.Player.CurrentItem is KeycardItem keycard)
            {
                bool result = DoorCustomPermissions.CheckPermissions(ev.Door, keycard, out bool isContains);

                if (isContains) ev.CanOpen = result;
            }
        }
        
        private void PlayGrabAnimation(Player player)
        {
            OverlayAnimationsSubcontroller subcontroller;
            if (!(player.ReferenceHub.roleManager.CurrentRole is IFpcRole currentRole) ||
                !(currentRole.FpcModule.CharacterModelInstance is AnimatedCharacterModel
                    characterModelInstance) ||
                !characterModelInstance.TryGetSubcontroller<OverlayAnimationsSubcontroller>(out subcontroller))
            {
                return;
            }
            subcontroller._overlayAnimations[1].OnStarted();
            subcontroller._overlayAnimations[1].SendRpc();
        }

        public void RegisterEvents()
        {
            LabApi.Events.Handlers.PlayerEvents.InteractingDoor += OnInteractingDoor;
        }
        
        public void UnregisterEvents()
        {
            LabApi.Events.Handlers.PlayerEvents.InteractingDoor -= OnInteractingDoor;
        }
    }
}
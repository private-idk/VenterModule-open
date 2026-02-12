using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.CustomHandlers;
using LabApi.Features.Console;

namespace VenterModuleLabApi.Events.PlayerEvents
{
    public class FlashlightAttachmentHandler : CustomEventsHandler
    {
        public override void OnPlayerToggledWeaponFlashlight(PlayerToggledWeaponFlashlightEventArgs ev)
        {
            if (!AudioPlayer.TryGet($"Flashlight {ev.Player.UserId}", out var player))
            {
                return;
            }
            
            player.AddClip("flashlight");
            
            base.OnPlayerToggledWeaponFlashlight(ev);
        }
    }
}
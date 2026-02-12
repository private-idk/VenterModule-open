using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.CustomHandlers;

namespace VenterModuleLabApi.Events.PlayerEvents
{
    public class CustomHitmarkerHandler : CustomEventsHandler
    {
        public override void OnPlayerSendingHitmarker(PlayerSendingHitmarkerEventArgs ev)
        {
            ev.IsAllowed = false;
            ev.PlayAudio = false;
        }
    }
}
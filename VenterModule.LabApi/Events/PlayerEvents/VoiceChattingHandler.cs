using LabApi.Events.Arguments.PlayerEvents;
using PlayerRoles.Voice;
using Talky;

namespace VenterModuleLabApi.Events.PlayerEvents
{
    public class VoiceChattingHandler
    {
        private void OnNewVoiceSending(PlayerSendingVoiceMessageEventArgs ev)
        {
            if (ev.Player.VoiceModule is HumanVoiceModule humanVoiceModule)
            {
                if (ev.Player.RoleBase is IVoiceRole role)
                {
                    if (!ev.Player.ReferenceHub.gameObject.TryGetComponent(out SpeechTracker tracker))
                    {
                        return;
                    }
                    tracker.VoiceMessageReceived(ev.Message.Data, ev.Message.DataLength);
                }
            }
        }

        public void RegisterEvents()
        {
            LabApi.Events.Handlers.PlayerEvents.SendingVoiceMessage += OnNewVoiceSending;
        }

        public void UnregisterEvents()
        {
            LabApi.Events.Handlers.PlayerEvents.SendingVoiceMessage -= OnNewVoiceSending;
        }
    }
}
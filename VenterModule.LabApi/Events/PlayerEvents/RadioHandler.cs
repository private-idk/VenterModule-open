using System.Collections.Generic;
using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Features.Wrappers;
using MEC;

namespace VenterModuleLabApi.Events.PlayerEvents
{
    public class RadioHandler
    {
        private Dictionary<int, string> _nicknames = new();

        private void OnUsingRadio(PlayerUsingRadioEventArgs ev)
        {
            ev.Drain = 0f;
            ev.RadioItem.BatteryPercent = 255;
        }

        private void OnSendingVoiceMessage(PlayerSendingVoiceMessageEventArgs ev)
        {
            if (!ev.Player.IsUsingRadio) return;

            Timing.RunCoroutine(HideNicknameOnUsingRadio(ev.Player, _nicknames));
        }

        private IEnumerator<float> HideNicknameOnUsingRadio(Player player, Dictionary<int, string> nicknames)
        {
            nicknames.Add(player.PlayerId, player.ReferenceHub.nicknameSync.Network_displayName);
            player.DisplayName = "???";

            while (player.IsUsingRadio)
            {
                yield return Timing.WaitForSeconds(0.01f);
            }

            yield return Timing.WaitForSeconds(0.01f);

            player.DisplayName = nicknames[player.PlayerId];
            nicknames.Remove(player.PlayerId);
        }

        public void RegisterEvents()
        {
            LabApi.Events.Handlers.PlayerEvents.UsingRadio += OnUsingRadio;
            LabApi.Events.Handlers.PlayerEvents.SendingVoiceMessage += OnSendingVoiceMessage;
        }

        public void UnregisterEvents()
        {
            LabApi.Events.Handlers.PlayerEvents.UsingRadio -= OnUsingRadio;
            LabApi.Events.Handlers.PlayerEvents.SendingVoiceMessage -= OnSendingVoiceMessage;
        }
    }
}

using LabApi.Events.Arguments.PlayerEvents;
using VenterModuleLabApi.API.Extensions;

namespace VenterModuleLabApi.Events.PlayerEvents
{
    public class OtherPlayerHandler
    {
        private void OnCuffing(PlayerCuffingEventArgs ev)
        {
            ev.IsAllowed = false;
        }

        private void OnEscaping(PlayerEscapingEventArgs ev)
        {
            ev.IsAllowed = false;
        }

        private void OnJoined(PlayerJoinedEventArgs ev)
        {
            ev.Player.TrySpawnInTower();
        }

        public void RegisterEvents()
        {
            LabApi.Events.Handlers.PlayerEvents.Cuffing += OnCuffing;
            LabApi.Events.Handlers.PlayerEvents.Escaping += OnEscaping;
            LabApi.Events.Handlers.PlayerEvents.Joined += OnJoined;
        }

        public void UnregisterEvents()
        {
            LabApi.Events.Handlers.PlayerEvents.Cuffing -= OnCuffing;
            LabApi.Events.Handlers.PlayerEvents.Escaping -= OnEscaping;
            LabApi.Events.Handlers.PlayerEvents.Joined -= OnJoined;
        }
    }
}

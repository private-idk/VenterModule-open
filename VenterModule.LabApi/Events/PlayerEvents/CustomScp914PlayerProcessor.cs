using LabApi.Events.Arguments.Scp914Events;
using LabApi.Events.CustomHandlers;

namespace VenterModuleLabApi.Events.PlayerEvents
{
    public class CustomScp914PlayerProcessor : CustomEventsHandler
    {
        public override void OnScp914ProcessedPlayer(Scp914ProcessedPlayerEventArgs ev)
        {
            ev.Player.Kill("Тело изуродовано SCP-914");
            base.OnScp914ProcessedPlayer(ev);
        }
    }
}
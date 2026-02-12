using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.CustomHandlers;

namespace VenterModuleLabApi.Events.PlayerEvents
{
    public class CustomTeslaHandler : CustomEventsHandler
    {
        public static bool CurrentTeslaCondition = false;

        public override void OnPlayerIdlingTesla(PlayerIdlingTeslaEventArgs ev)
        {
            ev.IsAllowed = CurrentTeslaCondition;
            base.OnPlayerIdlingTesla(ev);
        }

        public override void OnPlayerTriggeringTesla(PlayerTriggeringTeslaEventArgs ev)
        {
            ev.IsAllowed = CurrentTeslaCondition;
            base.OnPlayerTriggeringTesla(ev);
        }
    }
}

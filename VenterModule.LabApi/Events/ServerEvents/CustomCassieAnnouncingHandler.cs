using LabApi.Events.Arguments.ServerEvents;
using LabApi.Events.CustomHandlers;

namespace VenterModuleLabApi.Events.ServerEvents
{
    public class CustomCassieAnnouncingHandler : CustomEventsHandler
    {
        public override void OnServerCassieQueuingScpTermination(CassieQueuingScpTerminationEventArgs ev) => ev.IsAllowed = false;
    }
}
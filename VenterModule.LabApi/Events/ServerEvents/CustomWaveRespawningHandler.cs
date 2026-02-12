using LabApi.Events.Arguments.ServerEvents;
using LabApi.Events.CustomHandlers;

namespace VenterModuleLabApi.Events.ServerEvents
{
    public class CustomWaveRespawningHandler : CustomEventsHandler
    {
        public override void OnServerWaveRespawning(WaveRespawningEventArgs ev) => ev.IsAllowed = false;
    }
}
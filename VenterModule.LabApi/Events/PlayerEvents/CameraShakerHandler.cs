using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.CustomHandlers;
using PlayerStatsSystem;
using UnityEngine;

namespace VenterModuleLabApi.Events.PlayerEvents
{
    public class CameraShakerHandler : CustomEventsHandler
    {
        public override void OnPlayerHurt(PlayerHurtEventArgs ev)
        {
            base.OnPlayerHurt(ev);
        }
    }
}
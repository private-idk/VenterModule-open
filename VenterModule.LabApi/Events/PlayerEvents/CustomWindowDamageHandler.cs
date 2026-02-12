using System.Collections.Generic;
using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.CustomHandlers;
using LabApi.Features.Wrappers;
using MapGeneration;
using VenterModuleLabApi.Patches;

namespace VenterModuleLabApi.Events.PlayerEvents
{
    public class CustomWindowDamageHandler : CustomEventsHandler
    {
        private readonly HashSet<RoomName> _bulletProofGlassRooms = new()
        {
            RoomName.EzGateA, RoomName.HczCheckpointToEntranceZone, RoomName.Hcz049
        };
        
        public override void OnPlayerDamagingWindow(PlayerDamagingWindowEventArgs ev)
        {
            if (ev.Window == Scp079RecontainerPatch.RecontainerInstance._activatorGlass
                || (Room.TryGetRoomAtPosition(ev.Window.transform.position, out var room) && _bulletProofGlassRooms.Contains(room.Name))) 
                ev.IsAllowed = false;
        }
    }
}
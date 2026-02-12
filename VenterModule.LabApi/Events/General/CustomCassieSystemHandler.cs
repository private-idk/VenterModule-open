using System.Collections.Generic;
using System.Linq;
using CustomPlayerEffects;
using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.CustomHandlers;
using LabApi.Features.Wrappers;
using MEC;
using UnityEngine;
using VenterModuleLabApi.Commands.Admin;
using VenterModuleLabApi.Events.ServerEvents;

namespace VenterModuleLabApi.Events.PlayerEvents
{
    public class CustomCassieSystemHandler : CustomEventsHandler
    {
        private readonly List<ushort> _scpItems = new();
        
        public override void OnServerRoundStarted()
        {
            foreach (var pickup in Pickup.List.Where(p => p.Category == ItemCategory.SCPItem))
                _scpItems.Add(pickup.Serial);
            
            base.OnServerRoundStarted();
        }

        public override void OnPlayerPickedUpItem(PlayerPickedUpItemEventArgs ev)
        {
            if (_scpItems.Contains(ev.Item.Serial) && CassieSystemCommand.IsCassieSystem)
            {
                _scpItems.Remove(ev.Item.Serial);

                foreach (var door in ev.Player.Room.Doors)
                {
                    door.IsOpened = false;
                    door.IsLocked = true;
                }
            
                ev.Player.Room.LightController.FlickerLights(180f);
                Timing.RunCoroutine(ProcessSleep(ev.Player.Room));
                
                Cassie.Message(VenterModule.Instance.Config.StealScpCassie);

                int roundId = RoundHandler.UniqueRoundId;
            
                Timing.CallDelayed(180f, () =>
                {
                    if (roundId == RoundHandler.UniqueRoundId)
                    {
                        foreach (var door in ev.Player.Room.Doors)
                            door.IsLocked = false;
                    }
                });
            }
            
            base.OnPlayerPickedUpItem(ev);
        }

        private IEnumerator<float> ProcessSleep(Room room)
        {
            float elapsed = 0;
            while (elapsed < 10)
            {
                SetEffectToPlayersInRoom(room, (byte)(elapsed * 10), 0.5f, true);

                elapsed += Time.deltaTime;
                yield return Timing.WaitForOneFrame;
            }
            
            SetEffectToPlayersInRoom(room, 255, 180f, false);
        }

        private void SetEffectToPlayersInRoom(Room room, byte intensity, float duration, bool addDuration)
        {
            foreach (var player in room.Players)
            {
                player.ReferenceHub.playerEffectsController.TryGetEffect("Blindness", out var effect);
                player.ReferenceHub.playerEffectsController.TryGetEffect("Slowness", out var effectSlowness);
                effect.ServerSetState(intensity, duration, addDuration);
                effectSlowness.ServerSetState(intensity, duration, addDuration);
            }
        }
    }
}
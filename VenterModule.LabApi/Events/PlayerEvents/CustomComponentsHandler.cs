using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Features.Wrappers;
using Mirror;
using PlayerRoles;
using Talky;
using UnityEngine;
using VenterModuleLabApi.API.Features;
using VenterModuleLabApi.API.Features.Behaviours;
using VenterModuleLabApi.Commands.Client;

namespace VenterModuleLabApi.Events.PlayerEvents
{
    public class CustomComponentsHandler
    {
        private void OnSpawned(PlayerSpawnedEventArgs ev)
        {
            if (EmergencyButtonCommand.PressedEmergencyButton.Contains(ev.Player.UserId))
                EmergencyButtonCommand.PressedEmergencyButton.Remove(ev.Player.UserId);
            
            AudioPlayer.CreateOrGet(name: $"Flashlight {ev.Player.UserId}", onIntialCreation: (p) =>
            {
                p.transform.parent = ev.Player.GameObject.transform;
                
                var speaker = p.AddSpeaker("Main", isSpatial: true, maxDistance: 7.5f);
                
                speaker.transform.parent = ev.Player.GameObject.transform;
                speaker.transform.localPosition = Vector3.zero;
            });

            if (!ev.Player.GameObject.TryGetComponent<PushTracker>(out _))
                ev.Player.GameObject.AddComponent<PushTracker>();
            
            if (!ev.Player.GameObject.TryGetComponent<SpeechTracker>(out _))
                ev.Player.GameObject.AddComponent<SpeechTracker>();
            
            if (!ev.Player.GameObject.TryGetComponent<DeathPretendController>(out _))
                ev.Player.GameObject.AddComponent<DeathPretendController>();
            
            if (!ev.Player.GameObject.TryGetComponent<HurtConsequencesController>(out _))
                ev.Player.GameObject.AddComponent<HurtConsequencesController>();
            
            if (!ev.Player.GameObject.TryGetComponent<ChaosInsurgencyKeycardController>(out _))
                ev.Player.GameObject.AddComponent<ChaosInsurgencyKeycardController>();
            
            if (!ev.Player.GameObject.TryGetComponent<BleedingController>(out _))
                ev.Player.GameObject.AddComponent<BleedingController>();
        }

        private void OnDeath(PlayerDeathEventArgs ev)
        {
            if (!AudioPlayer.TryGet($"Flashlight {ev.Player.UserId}", out var player))
                player.Destroy();
        }
        
        public void RegisterEvents()
        {
            LabApi.Events.Handlers.PlayerEvents.Spawned += OnSpawned;
            LabApi.Events.Handlers.PlayerEvents.Death += OnDeath;
        }

        public void UnregisterEvents()
        {
            LabApi.Events.Handlers.PlayerEvents.Spawned -= OnSpawned;
            LabApi.Events.Handlers.PlayerEvents.Death -= OnDeath;
        }
    }
}
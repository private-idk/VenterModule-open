using System;
using System.Collections.Generic;
using System.Linq;
using AdminToys;
using LabApi.Features.Wrappers;
using MapGeneration;
using MEC;
using UnityEngine;
using VenterModuleLabApi.API.Features.Behaviours;
using VenterModuleLabApi.Commands.Admin;
using PrimitiveObjectToy = LabApi.Features.Wrappers.PrimitiveObjectToy;

namespace VenterModuleLabApi.Events.ServerEvents
{
    public class RoundHandler
    {
        CoroutineHandle _flicker;
        public static int UniqueRoundId;
        
        private void OnWaitingForPlayers()
        {
            Round.IsLobbyLocked = true;
            Round.IsLocked = true;
        }
        private void OnRoundStarted()
        {
            WorkdayCommand.IsDayStarted = false;
            UniqueRoundId = UnityEngine.Random.Range(Int32.MinValue, Int32.MaxValue);

            MetalDetectorsCollider();
            
            DespawnStructures();
            if (_flicker.IsRunning) Timing.KillCoroutines(_flicker);

            _flicker = Timing.RunCoroutine(RoomFlicker());
        }

        private void DespawnStructures()
        {
            foreach (var locker in ExperimentalWeaponLocker.List) locker.Destroy();

            foreach (var pedestal in PedestalLocker.List) 
                foreach (var i in pedestal.GetAllItems())
                    if (VenterModule.Instance.Config.TrashItems.Contains(i.Type))
                    {
                        pedestal.Destroy();
                        break;
                    }

            IEnumerable<Pickup> pickupsToDelete = Pickup.List.Where(p => p != null && p.Room != null &&
                                                                         ((!VenterModule.Instance.Config.ExceptedRooms.Contains(p.Room.Name) && !p.IsLocked) || VenterModule.Instance.Config.TrashItems.Contains(p.Type)));
            
            foreach (var pickup in pickupsToDelete) pickup.Destroy();
        }
        
        private static IEnumerator<float> RoomFlicker()
        {
            while (Round.IsRoundInProgress)
            {
                yield return Timing.WaitForSeconds(120);

                var player = Player.List.Where(p => p.IsAlive).ToList().RandomItem();

                var identifier = player.Room.ConnectedRooms.First();
                var room = Room.Get(identifier);
                
                if (room == null) continue;
                
                if (!room.LightController.LightsEnabled) continue;
                    
                room.LightController.FlickerLights(3f);
            }
        }

        private List<Vector3> _detectors = new()
        {
            new(-1.975f, 1.227f, -0.125f),
            new(-1.9f, 1.242f, 2.478f)
        };
        
        private void MetalDetectorsCollider()
        {
            foreach (var room in Room.List.Where(r => r.Name == RoomName.HczCheckpointToEntranceZone))
            {
                foreach (var position in _detectors)
                {
                    var pos = room.Transform.TransformPoint(position);
                
                    var toy = PrimitiveObjectToy.Create(pos, room.Rotation);
                
                    AudioPlayer audio = AudioPlayer.CreateOrGet(name: $"MetalDetector {DateTime.Now.Ticks}", onIntialCreation: (p) =>
                    {
                        var speaker = p.AddSpeaker("detec", isSpatial: true, maxDistance: 10f);
                        speaker.Position = pos;
                        speaker.transform.rotation = room.Rotation;
                    });
                
                    toy.Flags = PrimitiveFlags.None;
                    if (!toy.Base.gameObject.TryGetComponent<BoxCollider>(out var collider))
                        collider = toy.Base.gameObject.AddComponent<BoxCollider>();

                    toy.Base.gameObject.AddComponent<MetalDetectorController>().audioPlayer = audio;
                
                    collider.isTrigger = true;
                }
            }
        }

        public void RegisterEvents()
        {
            LabApi.Events.Handlers.ServerEvents.WaitingForPlayers += OnWaitingForPlayers;
            LabApi.Events.Handlers.ServerEvents.RoundStarted += OnRoundStarted;
        }

        public void UnregisterEvents()
        {
            LabApi.Events.Handlers.ServerEvents.WaitingForPlayers -= OnWaitingForPlayers;
            LabApi.Events.Handlers.ServerEvents.RoundStarted -= OnRoundStarted;
        }
    }
}

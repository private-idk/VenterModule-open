using System.Collections.Generic;
using Audio;
using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Features.Console;
using LabApi.Features.Wrappers;
using PlayerRoles.Ragdolls;
using UnityEngine;
using VenterModuleLabApi.API.Features;
using VenterModuleLabApi.Commands.Client;
using Logger = LabApi.Features.Console.Logger;

namespace VenterModuleLabApi.Events.PlayerEvents
{
    public class PlayerInfoHandler
    {
        private readonly Dictionary<ReferenceHub, string> _customInfoBuffer = new();
        
        private void OnJoined(PlayerJoinedEventArgs ev) => SetDisplayInformation(ev.Player, true);

        private void OnDeath(PlayerDeathEventArgs ev)
        {
            _customInfoBuffer.Add(ev.Player.ReferenceHub, ev.Player.CustomInfo);
            SetDisplayInformation(ev.Player, false);
        }

        private void OnSpawned(PlayerSpawnedEventArgs ev) => ev.Player.ReferenceHub.nicknameSync.ShownPlayerInfo = PlayerInfoArea.Nickname | PlayerInfoArea.CustomInfo;

        private void SetDisplayInformation(Player player, bool isJoined)
        {
            player.DisplayName = $"«{player.PlayerId}» {player.Nickname}";
            if (!isJoined) player.CustomInfo = string.Empty;
        }

        private void RagdollSpawnedHandler(BasicRagdoll ragdoll)
        {
            if (ragdoll.Info.OwnerHub == null || !_customInfoBuffer.ContainsKey(ragdoll.Info.OwnerHub)) return;
            
            var information = ragdoll.gameObject.AddComponent<RagdollAdditionalInformation>();
            information.hub = ragdoll.Info.OwnerHub;
            information.customInfo = _customInfoBuffer[ragdoll.Info.OwnerHub];
            
            _customInfoBuffer.Remove(ragdoll.Info.OwnerHub);
        }

        public void RegisterEvents()
        {
            LabApi.Events.Handlers.PlayerEvents.Joined += OnJoined;
            LabApi.Events.Handlers.PlayerEvents.Death += OnDeath;
            LabApi.Events.Handlers.PlayerEvents.Spawned += OnSpawned;

            RagdollManager.OnRagdollSpawned += RagdollSpawnedHandler;
        }

        public void UnregisterEvents()
        {
            LabApi.Events.Handlers.PlayerEvents.Joined -= OnJoined;
            LabApi.Events.Handlers.PlayerEvents.Death -= OnDeath;
            LabApi.Events.Handlers.PlayerEvents.Spawned -= OnSpawned;
            
            RagdollManager.OnRagdollSpawned -= RagdollSpawnedHandler;
        }
    }
}

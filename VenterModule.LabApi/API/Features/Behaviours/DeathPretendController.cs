using LabApi.Features.Wrappers;
using Mirror;
using PlayerRoles;
using PlayerRoles.Ragdolls;
using PlayerStatsSystem;
using UnityEngine;

using Log = LabApi.Features.Console.Logger;

namespace VenterModuleLabApi.API.Features
{
    public class DeathPretendController : MonoBehaviour
    {
        public ReferenceHub hub => GetComponent<ReferenceHub>();
        public Player player => Player.Get(hub);
        
        public bool currentCondition { get; set; }
        private float lastCalledTime { get; set; }
        private Vector3 size { get; set; }
        public BasicRagdoll ragdoll { get; set; }
        private bool wasDisarmed { get; set; }
        
        private void SpawnRagdoll(IRagdollRole role)
        {
            GameObject gameObject = Object.Instantiate(role.Ragdoll.gameObject, transform.position, transform.rotation);
            if (gameObject.TryGetComponent(out BasicRagdoll component))
            {
                component.NetworkInfo = new RagdollData(null, new UniversalDamageHandler(0.0f, DeathTranslations.Unknown), player.Role, new(transform.position), transform.rotation, player.DisplayName, NetworkTime.time);
            }

            NetworkServer.Spawn(gameObject);
            ragdoll = component;
        }
        
        private void Awake()
        {
            lastCalledTime = Time.time;
            currentCondition = false;
            size = Vector3.zero;
        }

        public void ProcessDeath()
        {
            currentCondition = false;
            ragdoll = null;
            
            player.Scale = Vector3.one;
            player.Position += Vector3.up;
                
            player.DropEverything();
            player.Position += Vector3.down * 5;
            player.Kill();
        }

        public void ChangePretendCondition()
        {
            if (!player.IsAlive || player.IsSCP || Time.time - lastCalledTime < 2f) return;
            
            lastCalledTime = Time.time;
            
            hub.playerEffectsController.TryGetEffect("Ensnared", out var effectFreeze);
            hub.playerEffectsController.TryGetEffect("Invisible", out var effectInv);
            
            if (currentCondition)
            {
                currentCondition = false;
                
                effectInv.ServerDisable();
                effectFreeze.ServerDisable();

                if (ragdoll != null) player.Position = ragdoll.transform.position;
                player.Position += Vector3.up;
                player.Scale = size;
                
                player.IsDisarmed = wasDisarmed;
                
                if (ragdoll != null)
                {
                    NetworkServer.Destroy(ragdoll.gameObject);
                }

                ragdoll = null;
                
                return;
            }
            
            currentCondition = true;
            
            effectInv.ServerSetState(1);
            effectFreeze.ServerSetState(1);
            
            if (player.CurrentItem != null) player.DropItem(player.CurrentItem);
            
            wasDisarmed = player.IsDisarmed;
            player.IsDisarmed = true;
            
            size = player.Scale;
            player.Scale = Vector3.zero;

            PlayerRoleLoader.TryGetRoleTemplate(player.Role, out PlayerRoleBase role);
            SpawnRagdoll(role as IRagdollRole);
        }
    }
}
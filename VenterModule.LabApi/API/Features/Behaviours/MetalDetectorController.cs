using System.Linq;
using InventorySystem.Items.Pickups;
using LabApi.Features.Wrappers;
using UnityEngine;

namespace VenterModuleLabApi.API.Features.Behaviours
{
    public class MetalDetectorController : MonoBehaviour
    {
        public AudioPlayer audioPlayer { get; set; }
        
        private float _lastPlayedTime { get; set; }

        private void Awake()
        {
            _lastPlayedTime = Time.time;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (Time.time - _lastPlayedTime < 3f)
                return;

            if ((Player.TryGet(other.gameObject, out var player) && TryGetMetallicItems(player))
                || (other.TryGetComponent(out ItemPickupBase pickupBase) &&
                    (VenterModule.Instance.Config.MetallicItems.Contains(pickupBase.Info.ItemId) ||
                     Pickup.Get(pickupBase) is FirearmPickup)))
            {
                audioPlayer.AddClip("detector");
            
                _lastPlayedTime = Time.time; 
            }
        }

        private bool TryGetMetallicItems(Player player)
        {
            foreach (var item in player.Items)
            {
                if (VenterModule.Instance.Config.MetallicItems.Contains(item.Type)
                    || item is FirearmItem) return true;
            }

            return false;
        }
    }
}
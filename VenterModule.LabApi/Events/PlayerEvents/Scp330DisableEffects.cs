using InventorySystem.Items.ThrowableProjectiles;
using InventorySystem.Items.Usables.Scp330;
using LabApi.Events.Arguments.Interfaces;
using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.CustomHandlers;
using LabApi.Features.Wrappers;
using MEC;
using UnityEngine;

namespace VenterModuleLabApi.Events.PlayerEvents
{
    public class Scp330DisableEffects
    {
        public void OnUsingItem(PlayerUsingItemEventArgs ev)
        {
            if (ev.UsableItem is not Scp330Item candy)
            {
                Item.Get(ev.Player.CurrentItem.Base);
            }
            
            Timing.CallDelayed(ev.UsableItem.UseDuration - Time.deltaTime, () => ev.Player.RemoveItem(ev.UsableItem));
        }

        public void RegisterEvents()
        {
            //LabApi.Events.Handlers.PlayerEvents.UsingItem += OnUsingItem;
        }

        public void UnregisterEvents()
        {
            //LabApi.Events.Handlers.PlayerEvents.UsingItem -= OnUsingItem;
        }
    }
}
using System.Collections.Generic;
using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Features.Wrappers;
using MEC;
using RueI.API;
using RueI.API.Elements;
using VenterModuleLabApi.API.Features.Behaviours;
using Logger = LabApi.Features.Console.Logger;

namespace VenterModuleLabApi.Events.PlayerEvents
{
    public class MedicineHandler
    {
        private readonly Dictionary<ushort, int> _medicinesUseTimes = new();
        private readonly Dictionary<int, int> _playerPainkillersUseTimes = new();
        
        private readonly List<ItemType> _medItems = new()
        {
            ItemType.Adrenaline, ItemType.Medkit, ItemType.Painkillers
        };
        
        private void OnUsedItem(PlayerUsedItemEventArgs ev)
        {
            if (_medItems.Contains(ev.UsableItem.Type))
            {
                ev.Player.ReferenceHub.playerEffectsController.TryGetEffect("Blurred", out var blur);
                ev.Player.ReferenceHub.playerEffectsController.TryGetEffect("Deafened", out var deaf);
                
                if (blur.IsEnabled) blur.ServerDisable();
                if (deaf.IsEnabled) deaf.ServerDisable();
                
                if (ev.UsableItem.Type == ItemType.Painkillers)
                {
                    if (!_playerPainkillersUseTimes.TryGetValue(ev.Player.LifeId, out var value))
                    {
                        _playerPainkillersUseTimes.Add(ev.Player.LifeId, 1);
                        value = 1;
                    }
                    
                    if (value >= 3)
                        Timing.RunCoroutine(ProcessOverdose(ev.Player));
                    else
                        _playerPainkillersUseTimes[ev.Player.LifeId]++;
                }
                else if (ev.UsableItem.Type == ItemType.Medkit)
                {
                    if (ev.Player.GameObject.TryGetComponent<BleedingController>(out var controller))
                        controller.isBleeding = false;
                
                    ev.Player.AddRegeneration(0.5f, 60);
                }

                if (ev.UsableItem.Type != ItemType.Adrenaline)
                {
                    if (!_medicinesUseTimes.TryGetValue(ev.UsableItem.Serial, out var count))
                    {
                        _medicinesUseTimes.Add(ev.UsableItem.Serial, 3);
                        count = 3;
                    }

                    if (count > 1)
                    {
                        _medicinesUseTimes[ev.UsableItem.Serial]--;
                        var item = ev.Player.AddItem(ev.UsableItem.Type);
                    
                        _medicinesUseTimes.Add(item.Serial, _medicinesUseTimes[ev.UsableItem.Serial]);
                        _medicinesUseTimes.Remove(ev.UsableItem.Serial);
                    
                        ev.Player.CurrentItem = item;
                    
                        RueDisplay.Get(ev.Player).Show(new BasicElement(200f, $"<b>Осталось <color=yellow>{_medicinesUseTimes[item.Serial]} использований</color></b>"), 2f);
                    }
                }
            }
        }

        private IEnumerator<float> ProcessOverdose(Player player)
        {
            RueDisplay.Get(player).Show(new BasicElement(200f, "<b>У вас <color=red>передозировка</color></b>"), 2f);
            
            while (player.IsAlive)
            {
                yield return Timing.WaitForSeconds(2f);
                player.Damage(5, "Передозировка оксикодоном");
            }
        }

        private void OnRoundStarted()
        {
            _medicinesUseTimes.Clear();
            _playerPainkillersUseTimes.Clear();
        }

        public void RegisterEvents()
        {
            LabApi.Events.Handlers.PlayerEvents.UsedItem += OnUsedItem;
            LabApi.Events.Handlers.ServerEvents.RoundStarted += OnRoundStarted;
        }

        public void UnregisterEvents()
        {
            LabApi.Events.Handlers.PlayerEvents.UsedItem -= OnUsedItem;
            LabApi.Events.Handlers.ServerEvents.RoundStarted -= OnRoundStarted;
        }
    }
}
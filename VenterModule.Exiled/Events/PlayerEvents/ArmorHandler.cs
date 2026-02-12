using System.Collections.Generic;
using Exiled.Events.EventArgs.Player;
using LabApi.Features.Wrappers;
using PlayerStatsSystem;
using Player = Exiled.API.Features.Player;

namespace VenterModuleExiled.Events.PlayerEvents
{
    internal class ArmorHandler
    {
        private readonly Dictionary<ushort, float> _armorHp = new();
        
        private void OnItemAdded(ItemAddedEventArgs ev)
        {
            if (!Plugin.Instance.Config.ArmorAhp.ContainsKey(ev.Item.Type) || _armorHp.ContainsKey(ev.Item.Serial)) return;
            
            SetArmor(ev.Player, ev.Item.Type, ev.Item.Serial);
        }

        private void OnDroppingItem(DroppingItemEventArgs ev)
        {
            if (!Plugin.Instance.Config.ArmorAhp.ContainsKey(ev.Item.Type)) return;
            
            _armorHp[ev.Item.Serial] = ev.Player.HumeShield;
            
            ev.Player.HumeShield = 0f;
            ev.Player.MaxHumeShield = 0f;
        }

        private void OnPickingUpItem(PickingUpItemEventArgs ev)
        {
            if (!Plugin.Instance.Config.ArmorAhp.ContainsKey(ev.Pickup.Type)) return;

            if (_armorHp.ContainsKey(ev.Pickup.Serial))
            {
                float amount = _armorHp[ev.Pickup.Serial];
                ev.Player.MaxHumeShield = Plugin.Instance.Config.ArmorAhp[ev.Pickup.Type];
                ev.Player.HumeShield = amount;
            }
            else
            {
                SetArmor(ev.Player, ev.Pickup.Type, ev.Pickup.Serial);
            }
        }

        private void SetArmor(Player player, ItemType type, ushort serial)
        {
            float amount = Plugin.Instance.Config.ArmorAhp[type];
            
            player.MaxHumeShield = amount;
            player.HumeShield = amount;
            
            _armorHp.Add(serial, amount);
        }

        private void OnRoundStarted()
        {
            _armorHp.Clear();
        }
        
        internal void Register()
        {
            Exiled.Events.Handlers.Player.ItemAdded += OnItemAdded;
            Exiled.Events.Handlers.Player.DroppingItem += OnDroppingItem;
            Exiled.Events.Handlers.Player.PickingUpItem += OnPickingUpItem;
            Exiled.Events.Handlers.Server.RoundStarted += OnRoundStarted;
        }

        internal void Unregister()
        {
            Exiled.Events.Handlers.Player.ItemAdded -= OnItemAdded;
            Exiled.Events.Handlers.Player.DroppingItem -= OnDroppingItem;
            Exiled.Events.Handlers.Player.PickingUpItem -= OnPickingUpItem;
            Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStarted;
        }
    }
}
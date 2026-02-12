using System.Collections.Generic;
using Exiled.Events.EventArgs.Player;
using ProjectMER.Features.Objects;
using UnityEngine;
using VenterModuleExiled;

namespace VenterModule.Exiled.Events.PlayerEvents
{
    public class TaserPickupmodel
    {
        private readonly Dictionary<ushort, SchematicObject> _schematics = new();
        
        public void OnPickedUpItem(PickingUpItemEventArgs ev)
        {
            if (!_schematics.TryGetValue(ev.Pickup.Serial, out var schematic))
                return;
            
            schematic.Destroy();
            _schematics.Remove(ev.Pickup.Serial);
        }

        public void OnDroppingItem(DroppingItemEventArgs ev)
        {
            if (!Plugin.Instance.EventHandlers.Taser.Check(ev.Item))
                return;

            if (!ProjectMER.Features.ObjectSpawner.TrySpawnSchematic("Taser", ev.Item.Base.transform.position,
                    out var schematic))
            {
                LabApi.Features.Console.Logger.Info("Didnt found");
                return;
            }
            schematic.transform.parent = ev.Item.Base.transform;
            schematic.transform.localPosition = Vector3.zero;
            
            _schematics.Add(ev.Item.Serial, schematic);
        }

        public void OnRoundStarted()
        {
            _schematics.Clear();
        }
    }
}
using System.Collections.Generic;
using Exiled.Events.EventArgs.Player;
using UnityEngine;
using VenterModuleExiled.Subroles;

namespace VenterModuleExiled.Events.PlayerEvents.SubrolesEvents
{
    internal class SubrolesHandler
    {
        private void OnDied(DiedEventArgs ev)
        {
            if (ev.Player == null) return;
            
            SubrolesManager.RemoveExists(ev.Player);
        }

        private void OnLeft(LeftEventArgs ev)
        {
            if (ev.Player == null) return;

            SubrolesManager.RemoveExists(ev.Player);
        }

        private void OnSpawning(SpawningEventArgs ev)
        {
            if (ev.Player == null) return;

            SubrolesManager.RemoveExists(ev.Player);
        }

        internal void Register()
        {
            Exiled.Events.Handlers.Player.Died += OnDied;
            Exiled.Events.Handlers.Player.Left += OnLeft;
            Exiled.Events.Handlers.Player.Spawning += OnSpawning;
        }
        internal void Unregister()
        {
            Exiled.Events.Handlers.Player.Died -= OnDied;
            Exiled.Events.Handlers.Player.Left -= OnLeft;
            Exiled.Events.Handlers.Player.Spawning -= OnSpawning;
        }
    }
}

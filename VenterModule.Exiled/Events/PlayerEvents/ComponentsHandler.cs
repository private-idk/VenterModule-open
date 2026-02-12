using Exiled.Events.EventArgs.Player;
using VenterModuleExiled.API;
using VenterModuleExiled.Subroles;

namespace VenterModuleExiled.Events.PlayerEvents
{
    public class ComponentsHandler
    {
        public void OnSpawned(SpawnedEventArgs ev)
        {
            if (!ev.Player.GameObject.TryGetComponent<AdminGunController>(out _))
                ev.Player.GameObject.AddComponent<AdminGunController>();
        }
    }
}
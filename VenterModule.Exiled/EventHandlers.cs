using System;
using Exiled.API.Features;
using Exiled.CustomItems.API;
using HarmonyLib;
using VenterModule.Exiled.Events.PlayerEvents;
using VenterModuleExiled.CustomItems;
using VenterModuleExiled.Events.PlayerEvents;
using VenterModuleExiled.Events.PlayerEvents.SubrolesEvents;
using VenterModuleExiled.Subroles.ServerSpecific;

namespace VenterModuleExiled
{
    public class EventHandlers
    {
        public Taser Taser;
        public AdminGun adminGun;

        PlayerDisplayHandler _playerDisplayHandler;
        SubrolesHandler _subrolesHandler;
        ArmorHandler _armorHandler;
        ComponentsHandler _componentsHandler;
        TaserPickupmodel _taserPickupmodel;
        
        AdminGunSpecific adminGunSpecific;
        GunStockSpecific gunStockSpecific;
        
        Harmony harmony;

        public void Register()
        {
            Taser = new();

            adminGun = new();

            _playerDisplayHandler = new();
            _subrolesHandler = new();
            _armorHandler = new();
            _componentsHandler = new();
            _taserPickupmodel = new();
            
            adminGunSpecific = new();
            gunStockSpecific = new();
            
            try
            {
                harmony = new($"ventermodule.exiled.private.{DateTime.Now.Ticks}");
                harmony.PatchAll();
            }
            catch (Exception e)
            {
                Log.Error($"Patching error: {e}");
            }

            Taser.Register();
            adminGun.Register();

            _subrolesHandler.Register();
            _playerDisplayHandler.Register();
            _armorHandler.Register();

            // Exiled.Events.Handlers.Player.PickingUpItem += _taserPickupmodel.OnPickedUpItem;
            // Exiled.Events.Handlers.Player.DroppingItem += _taserPickupmodel.OnDroppingItem;
            // Exiled.Events.Handlers.Server.RoundStarted += _taserPickupmodel.OnRoundStarted;

            Exiled.Events.Handlers.Player.Spawned += _componentsHandler.OnSpawned;

            adminGunSpecific.RegisterSS();
            gunStockSpecific.RegisterSS();
            
            GenerateSS.GenerateServerSpecific();
        }

        public void Unregister()
        {
            Taser.Unregister();
            adminGun.Unregister();

            _subrolesHandler.Unregister();
            _playerDisplayHandler.Unregister();
            _armorHandler.Unregister();
            
            // Exiled.Events.Handlers.Player.PickingUpItem -= _taserPickupmodel.OnPickedUpItem;
            // Exiled.Events.Handlers.Player.DroppingItem -= _taserPickupmodel.OnDroppingItem;
            // Exiled.Events.Handlers.Server.RoundStarted -= _taserPickupmodel.OnRoundStarted;

            Exiled.Events.Handlers.Player.Spawned -= _componentsHandler.OnSpawned;
            
            adminGunSpecific.UnregisterSS();
            gunStockSpecific.UnregisterSS();

            Taser = null;
            adminGun = null;

            _playerDisplayHandler = null;
            _subrolesHandler = null;
            _armorHandler = null;
            _componentsHandler = null;
            _taserPickupmodel = null;

            adminGunSpecific = null;
            gunStockSpecific = null;
            
            harmony?.UnpatchAll();
            harmony = null;
        }
    }
}

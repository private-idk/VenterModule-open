using HarmonyLib;
using LabApi.Features.Console;
using System;
using System.IO;
using LabApi.Events.CustomHandlers;
using LabApi.Loader.Features.Paths;
using VenterModuleLabApi.API.Features.ServerSpecific.Keybindings;
using VenterModuleLabApi.Events.PlayerEvents;
using VenterModuleLabApi.Events.ServerEvents;

namespace VenterModuleLabApi
{
    public class EventHandlers
    {
        Harmony harmony;
        
        PlayerInfoHandler _playerInfoHandler;
        RoundHandler _roundHandler;
        ScpHandler _scpHandler;
        OtherPlayerHandler _otherPlayerHandler;
        RadioHandler _radioHandler;
        DeathPretendHandler _deathPretendHandler;
        DoorsInteractionHandler _doorsInteractionHandler;
        VoiceChattingHandler _voiceChattingHandler;
        CustomComponentsHandler _customComponentsHandler;
        GrenadesHandler _grenadesHandler;
        MedicineHandler _medicineHandler;
        Scp330DisableEffects _scp330DisableEffects;

        CustomWindowDamageHandler _customWindowDamageHandler;
        CustomHitmarkerHandler _customHitmarkerHandler;
        CustomGeneratorHandler _customGeneratorHandler;        
        CustomWaveRespawningHandler _customWaveRespawningHandler;
        CustomCassieAnnouncingHandler _customCassieAnnouncingHandler;
        CustomTeslaHandler customTeslaHandler;
        CustomCassieSystemHandler customCassieSystemHandler;
        CustomScp914PlayerProcessor _customScp914PlayerProcessor;
        FlashlightAttachmentHandler _flashlightAttachmentHandler;
        CameraShakerHandler _cameraShakerHandler;
        HurtHandler _hurtHandler;

        GenerateSS ss;
        Scp106Dimension dimension;
        Scp049TakeKeycard takeKeycard;
        DeathPretendSpecific _deathPretendSpecific;
        GiveItemToPlayer giveItemSS;
        PushPlayerSpecific pushPlayerSpecific;
        ChaosInsurgencySpecific _chaosInsurgencySpecific;

        public void RegisterAll()
        {
            AudioClipStorage.LoadClip(Path.Combine(PathManager.LabApi.FullName, "audio/detector.ogg"), "detector");
            AudioClipStorage.LoadClip(Path.Combine(PathManager.LabApi.FullName, "audio/flashlight.ogg"), "flashlight");
            
            try
            {
                harmony = new($"ventermodule.private.{DateTime.Now.Ticks}");
                harmony.PatchAll();
            }
            catch (Exception e)
            {
                Logger.Error($"Patching error\n{e}");
            }

            _playerInfoHandler = new();
            _scpHandler = new();
            _roundHandler = new();
            _otherPlayerHandler = new();
            _radioHandler = new();
            _deathPretendHandler = new();
            _doorsInteractionHandler = new();
            _voiceChattingHandler = new();
            _customComponentsHandler = new();
            _grenadesHandler = new();
            _medicineHandler = new();
            _scp330DisableEffects = new();
            
            _customWindowDamageHandler = new();
            _customWaveRespawningHandler = new();
            _customCassieAnnouncingHandler = new();
            _customHitmarkerHandler = new();
            _customGeneratorHandler = new();
            customTeslaHandler = new();
            customCassieSystemHandler = new();
            _customScp914PlayerProcessor = new();
            _flashlightAttachmentHandler = new();
            _cameraShakerHandler = new();
            _hurtHandler = new();

            ss = new();
            dimension = new();
            takeKeycard = new();
            _deathPretendSpecific = new();
            giveItemSS = new();
            pushPlayerSpecific = new();
            _chaosInsurgencySpecific = new();

            _playerInfoHandler.RegisterEvents();
            _roundHandler.RegisterEvents();
            _scpHandler.RegisterEvents();
            _otherPlayerHandler.RegisterEvents();
            _radioHandler.RegisterEvents();
            _deathPretendHandler.RegisterEvents();
            _doorsInteractionHandler.RegisterEvents();
            _voiceChattingHandler.RegisterEvents();
            _customComponentsHandler.RegisterEvents();
            _grenadesHandler.RegisterEvents();
            _medicineHandler.RegisterEvents();
            _scp330DisableEffects.RegisterEvents();
            
            CustomHandlersManager.RegisterEventsHandler(_customWaveRespawningHandler);
            CustomHandlersManager.RegisterEventsHandler(_customCassieAnnouncingHandler);
            CustomHandlersManager.RegisterEventsHandler(_customWindowDamageHandler);
            CustomHandlersManager.RegisterEventsHandler(_customHitmarkerHandler);
            CustomHandlersManager.RegisterEventsHandler(_customGeneratorHandler);
            CustomHandlersManager.RegisterEventsHandler(customTeslaHandler);
            CustomHandlersManager.RegisterEventsHandler(customCassieSystemHandler);
            CustomHandlersManager.RegisterEventsHandler(_customScp914PlayerProcessor);
            CustomHandlersManager.RegisterEventsHandler(_flashlightAttachmentHandler);
            CustomHandlersManager.RegisterEventsHandler(_cameraShakerHandler);
            CustomHandlersManager.RegisterEventsHandler(_hurtHandler);

            ss.Generate();
            dimension.RegisterSS();
            takeKeycard.RegisterSS();
            _deathPretendSpecific.RegisterSS();
            giveItemSS.RegisterSS();
            pushPlayerSpecific.RegisterSS();
            _chaosInsurgencySpecific.RegisterSS();
        }

        public void UnregisterAll()
        {
            harmony?.UnpatchAll();
            harmony = null;
            
            _playerInfoHandler.UnregisterEvents();
            _roundHandler.UnregisterEvents();
            _scpHandler.UnregisterEvents();
            _otherPlayerHandler.UnregisterEvents();
            _radioHandler.UnregisterEvents();
            _deathPretendHandler.UnregisterEvents();
            _doorsInteractionHandler.UnregisterEvents();
            _voiceChattingHandler.UnregisterEvents();
            _customComponentsHandler.UnregisterEvents();
            _grenadesHandler.UnregisterEvents();
            _medicineHandler.UnregisterEvents();
            _scp330DisableEffects.UnregisterEvents();

            dimension.UnregisterSS();
            takeKeycard.UnregisterSS();
            _deathPretendSpecific.UnregisterSS();
            giveItemSS.UnregisterSS();
            pushPlayerSpecific.UnregisterSS();
            _chaosInsurgencySpecific.UnregisterSS();
            
            CustomHandlersManager.UnregisterEventsHandler(_customWaveRespawningHandler);
            CustomHandlersManager.UnregisterEventsHandler(_customCassieAnnouncingHandler);
            CustomHandlersManager.UnregisterEventsHandler(_customWindowDamageHandler);
            CustomHandlersManager.UnregisterEventsHandler(_customHitmarkerHandler);
            CustomHandlersManager.UnregisterEventsHandler(_customGeneratorHandler);
            CustomHandlersManager.UnregisterEventsHandler(customTeslaHandler);
            CustomHandlersManager.UnregisterEventsHandler(customCassieSystemHandler);
            CustomHandlersManager.UnregisterEventsHandler(_customScp914PlayerProcessor);
            CustomHandlersManager.UnregisterEventsHandler(_flashlightAttachmentHandler);
            CustomHandlersManager.UnregisterEventsHandler(_cameraShakerHandler);
            CustomHandlersManager.UnregisterEventsHandler(_hurtHandler);
            
            _playerInfoHandler = null;
            _scpHandler = null;
            _roundHandler = null;
            _otherPlayerHandler = null;
            _radioHandler = null;
            _deathPretendHandler = null;
            _doorsInteractionHandler = null;
            _voiceChattingHandler = null;
            _customComponentsHandler = null;
            _grenadesHandler = null;
            _medicineHandler = null;
            _scp330DisableEffects = null;

            _customWaveRespawningHandler = null;
            _customCassieAnnouncingHandler = null;
            _customWindowDamageHandler = null;
            _customHitmarkerHandler = null;
            _customGeneratorHandler = null;
            customTeslaHandler = null;
            customCassieSystemHandler = null;
            _customScp914PlayerProcessor = null;
            _flashlightAttachmentHandler = null;
            _cameraShakerHandler = null;
            _hurtHandler = null;

            ss = null;
            dimension = null;
            takeKeycard = null;
            _deathPretendSpecific = null;
            giveItemSS = null;
            pushPlayerSpecific = null;
            _chaosInsurgencySpecific = null;
        }
    }
}

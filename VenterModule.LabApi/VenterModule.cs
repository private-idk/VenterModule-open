using LabApi.Loader.Features.Plugins;
using System;
using System.IO;
using LabApi.Loader.Features.Paths;

namespace VenterModuleLabApi
{
    public class VenterModule : Plugin<Config>
    {
        public override string Name => "VenterModule.LabApi";
        public override string Description => string.Empty;
        public override string Author => "$private";
        public override Version Version => new(1, 0, 0);
        public override Version RequiredApiVersion => new(LabApi.Features.LabApiProperties.CompiledVersion);

        public static VenterModule Instance;

        EventHandlers eventHandlers;

        public override void Disable()
        {
            Instance = null;
            eventHandlers = null;

            eventHandlers.UnregisterAll();
        }

        public override void Enable()
        {
            Instance = this;
            eventHandlers = new();

            eventHandlers.RegisterAll();
        }       
    }
}

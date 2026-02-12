using System;
using Exiled.API.Features;
using VenterModuleExiled.Commands.Admin;

namespace VenterModuleExiled
{
    internal class Plugin : Plugin<Config>
    {
        public override string Name => "VenterModule.Exiled";
        public override string Author => "$private";
        public override Version Version => new(0, 0, 1);

        internal static Plugin Instance;

        internal EventHandlers EventHandlers;
        internal bool CassieFunctions;

        internal static bool IsAutoround = true;

        public override void OnEnabled()
        {
            Instance = this;

            EventHandlers = new();
            CassieFunctions = true;

            EventHandlers.Register();
        }

        public override void OnDisabled()
        {
            Instance = null;

            EventHandlers = null;

            EventHandlers.Unregister();
        }
    }
}

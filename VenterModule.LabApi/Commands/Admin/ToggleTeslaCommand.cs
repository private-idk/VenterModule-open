using System;
using CommandSystem;
using VenterModuleLabApi.Events.PlayerEvents;

namespace VenterModuleLabApi.Commands.Admin
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    internal class ToggleTeslaCommand : ICommand
    {
        public string Command { get; } = "tesla";
        public string[] Aliases { get; } = { };
        public string Description { get; } = "Переключает состояние тесла-ворот";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            CustomTeslaHandler.CurrentTeslaCondition = !CustomTeslaHandler.CurrentTeslaCondition;

            response = $"Состояние тесла-ворот {CustomTeslaHandler.CurrentTeslaCondition}";
            return true;
        }
    }
}

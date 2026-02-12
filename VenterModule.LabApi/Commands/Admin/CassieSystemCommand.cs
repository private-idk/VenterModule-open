using System;
using CommandSystem;

namespace VenterModuleLabApi.Commands.Admin
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class CassieSystemCommand : ICommand
    {
        /// <summary>
        /// Отвечает за пускание газа при краже объекта
        /// </summary>
        public static bool IsCassieSystem = false;
        
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            IsCassieSystem = !IsCassieSystem;
            response = $"Состояние {IsCassieSystem}";
            return true;
        }

        public string Command => "cassiesystem";
        public string[] Aliases => new[] { "cs" };
        public string Description => "Переключает функции CASSIE";
    }
}
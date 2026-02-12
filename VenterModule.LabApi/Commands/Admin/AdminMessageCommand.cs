using System;
using System.Linq;
using CommandSystem;
using LabApi.Features.Wrappers;
using VenterModuleLabApi.API.Extensions;

namespace VenterModuleLabApi.Commands.Admin.MessageCommands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class AdminMessageCommand : ICommand, IUsageProvider
    {
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (arguments.Count <= 2)
            {
                response = "Ошибка синтаксиса!\nmsg [id] [sender_id] [sender_name (Вместо пробелов \"-\")] [message]";
                return false;
            }

            if (!Int32.TryParse(arguments.At(0), out var id) || !Player.TryGet(id, out var player))
            {
                response = "Игрок с таким ID не найден";
                return false;
            }

            if (!player.IsAlive)
            {
                response = "Игрок мертв";
                return false;
            }
            
            player.SendPDAMessage(arguments.At(1), arguments.At(2).Replace("-", " "), String.Join(" ", arguments.Skip(3)));

            response = "Сообщение отправлено";
            return true;
        }

        public string Command => "message";
        public string[] Aliases => new[] { "msg" };
        public string Description => string.Empty;
        public string[] Usage => new[] { "[id] [sender_id] [sender_name (Вместо пробелов \"-\")] [message]" };
    }
}
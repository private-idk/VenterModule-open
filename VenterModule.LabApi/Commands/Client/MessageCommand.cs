using System;
using System.Linq;
using System.Text;
using CommandSystem;
using LabApi.Features.Wrappers;
using VenterModuleLabApi.API.Extensions;

namespace VenterModuleLabApi.Commands.Client
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class MessageCommand : ICommand
    {
        public string Command { get; } = "sendmessage";
        public string[] Aliases { get; } = { "sm" };
        public string Description { get; } = "Отправляет сообщение на КПК";
        
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (arguments.Count == 0)
            {
                response = "Отправить сообщение: sm %player_id% %message%";
                return false;
            }

            if (!Int32.TryParse(arguments.At(0), out var playerId) || !Player.TryGet(playerId, out var target) || !target.IsAlive)
            {
                response = "Пользователь с таким ID не найден";
                return false;
            }

            if (arguments.Count < 2)
            {
                response = "Вы должны написать сообщение";
                return false;
            }

            Player player = Player.Get(sender);

            if (!player.IsAlive)
            {
                response = "Вы мертвы";
                return false;
            }
            
            if (player.PlayerId == target.PlayerId)
            {
                response = "Вы не можете отправить сообщение самому себе";
                return false;
            }

            string message = String.Join(" ", arguments.Skip(1));

            target.SendPDAMessage(player, message);

            Server.SendAdminChatMessage($"<size=24>ID Отправителя: {player.PlayerId}\nID получателя: {target.PlayerId}\nСообщение: {message}");
            
            response = "Сообщение отправлено";
            return true;
        }
    }
}
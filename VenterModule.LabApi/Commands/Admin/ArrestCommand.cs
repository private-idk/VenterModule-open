using CommandSystem;
using LabApi.Features.Wrappers;
using System;

namespace VenterModuleLabApi.Commands.Admin
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class ArrestCommand : ICommand
    {
        public string Command { get; } = "arrest";
        public string[] Aliases { get; } = { };
        public string Description { get; } = "Отправляет касси об аресте персонала";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (arguments.Count == 0)
            {
                response = "Ошибка синтаксиса\narrest personnel-id";
                return false;
            }

            try
            {
                int id = Int32.Parse(String.Join(string.Empty, arguments));

                if (!Player.TryGet(id, out Player player))
                {
                    response = "Игрок с таким ID не найден";
                    return false;
                }

                Cassie.Message(VenterModule.Instance.Config.ArrestCassie.Replace("%id%", player.PlayerId.ToString()));

                response = "Касси об аресте отправлено";
                return true;
            }
            catch (Exception ex) 
            {
                response = "Неверный ID игрока";
                return false;
            }
        }
    }
}

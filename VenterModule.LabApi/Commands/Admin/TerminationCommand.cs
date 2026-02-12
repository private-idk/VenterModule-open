using CommandSystem;
using LabApi.Features.Wrappers;
using System;

namespace VenterModuleLabApi.Commands.Admin
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class TerminationCommand : ICommand
    {
        public string Command { get; } = "termination";
        public string[] Aliases { get; } = { "term" };
        public string Description { get; } = "Отправляет касси о ликвидации персонала";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (arguments.Count == 0)
            {
                response = "Ошибка синтаксиса\ntermination personnel-id";
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

                Cassie.Message(VenterModule.Instance.Config.TerminationCassie.Replace("%id%", player.PlayerId.ToString()));

                response = "Касси о ликвидации отправлено";
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

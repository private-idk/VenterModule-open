using System;
using CommandSystem;
using Exiled.API.Features;
using VenterModuleExiled.Subroles;

namespace VenterModuleExiled.Commands.Admin
{
    [CommandHandler((typeof(RemoteAdminCommandHandler)))]
    public class CustomTaskForceCommand : ICommand
    {
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player player = Player.Get(sender);

            if (!player.GameObject.TryGetComponent<AdminGunController>(out var controller))
            {
                response = "Для начала вы должны выдать себе админ-ган";
                return false;
            }
            
            if (arguments.Count == 0)
            {
                response = "Ошибка в названии группы";
                return false;
            }

            string groupName = string.Join(" ", arguments);

            controller.CustomTaskForce = groupName;

            response = $"Кастомная группа: {groupName}";
            return true;
        }

        public string Command => "customtaskforce";
        public string[] Aliases { get; } = { "ctf" };
        public string Description => "Позволяет задать название для кастомной МОГ (админ-ган)";
    }
}
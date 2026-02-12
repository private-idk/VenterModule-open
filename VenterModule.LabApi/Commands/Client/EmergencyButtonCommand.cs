using System;
using System.Collections.Generic;
using System.Linq;
using CommandSystem;
using LabApi.Features.Wrappers;
using PlayerRoles;

namespace VenterModuleLabApi.Commands.Client
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class EmergencyButtonCommand : ICommand
    {
        public static List<string> PressedEmergencyButton = new();
        
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player player = Player.Get(sender);

            if (player.Role != RoleTypeId.FacilityGuard)
            {
                response = "У вас нету аварийной кнопки";
                return false;
            }
            
            if (PressedEmergencyButton.Contains(player.UserId))
            {
                response = "Аварийная кнопка вышла из строя";
                return false;
            }
            
            foreach (var p in Player.List.Where(x => x.Role == RoleTypeId.FacilityGuard && x != player))
            {
                p.SendBroadcast($"<b>Сотрудник с ID {player.PlayerId} <color=red>нажал аварийную кнопку", 10);
            }
            
            PressedEmergencyButton.Add(player.UserId);

            response = "Вы успешно нажали аварийную кнопку";
            return true;
        }

        public string Command => "emergencybutton";
        public string[] Aliases =>  new[] {"em", "eb"};
        public string Description => "Нажимая на аварийную кнопку вы сообщаете всем сотрудникам СБ о том что вы в ЧС";
    }
}
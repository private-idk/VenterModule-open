using System;
using CommandSystem;
using LabApi.Features.Wrappers;
using UnityEngine;

namespace VenterModuleLabApi.Commands.Admin
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class FacilityColorCommand : ICommand, IUsageProvider
    {
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (arguments.Count == 0)
            {
                ChangeRoomsColor(Color.clear);
                
                response = "Цвета по умолчанию установлены";
                return true;
            }
            
            string stringColor = arguments.At(0);
            if (!ColorUtility.TryParseHtmlString(stringColor, out var color))
            {
                response = "HEX-цвет введен неверно";
                return false;
            }
            
            ChangeRoomsColor(color);

            response = $"Цвет {stringColor} установлен";
            return true;
        }

        private void ChangeRoomsColor(Color color)
        {
            foreach (var room in Room.List) room.LightController.OverrideLightsColor = color;
        }

        public string Command => "facilitycolor";
        public string[] Aliases => new[] { "fcolor" };
        public string Description => "Позволяет изменить цвет помещений во всей зоне";
        public string[] Usage => new[] { "{HEXCOLOR}" };
    }
}
using System;
using System.Linq;
using CommandSystem;
using Interactables.Interobjects.DoorUtils;
using InventorySystem;
using LabApi.Features.Wrappers;
using UnityEngine;
using VenterModuleLabApi.API.Extensions;
using KeycardItem = InventorySystem.Items.Keycards.KeycardItem;

namespace VenterModuleLabApi.Commands.Admin
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class SetnickCommand : ICommand, IUsageProvider
    {
        public string Command { get; } = "snick";
        public string[] Aliases { get; } = { };
        public string Description { get; } = "Устанавливает никнейм без нужды вводить ID";
        public string[] Usage { get; } = { "ID", "Никнейм" };
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (arguments.Count < 1)
            {
                response = "Недостаточное кол-во аргументов";
                return false;
            }

            if (!Int32.TryParse(arguments.At(0), out var id) || !Player.TryGet(id, out var player))
            {
                response = "ID введен неверно или такого игрока не существует";
                return false;
            }

            var list = arguments.ToList();
            list.RemoveAt(0);

            player.DisplayName = $"«{player.PlayerId}» {String.Join(" ", list)}";

            if (player.Inventory.TryGetInventoryItem(ItemType.KeycardCustomSite02, out var itemBase))
            {
                var keycard = itemBase as KeycardItem;
                bool isCurrent = false;
                
                if (player.CurrentItem != null)
                    isCurrent = player.CurrentItem.Type == ItemType.KeycardCustomSite02;

                var data = keycard.GetKeycardData();
                player.RemoveItem(itemBase);
                var newKeycard = LabApi.Features.Wrappers.KeycardItem.CreateCustomKeycardSite02(player, data.CustomName, $"<size=1>NAME: {player.DisplayName}\nEUID: {PlayerRoleplayExtensions.GenerateMessageId()}</size>", data.CustomLabelText, 
                    (KeycardLevels)data.KeycardLevels, (Color)data.TintColor, (Color)data.PermissionColor, (Color)data.CustomLabelColor, (byte)data.WearLevel);
                
                if (isCurrent)
                    player.CurrentItem = newKeycard;
            }
            
            response = "Никнейм установлен";
            return true;
        }
    }
}

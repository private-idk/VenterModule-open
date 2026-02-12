using System;
using System.Collections.Generic;
using System.Text;
using CommandSystem;
using InventorySystem.Items;
using LabApi.Features.Wrappers;
using MEC;
using RueI.API;
using RueI.API.Elements;
using UnityEngine;

namespace VenterModuleLabApi.Commands.Client
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class LookupCommand : ICommand
    {
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player player = Player.Get(sender);

            if (player.IsDisarmed)
            {
                response = "Вы связаны";
                return false;
            }

            if (!Physics.Raycast(player.Camera.position, player.Camera.forward, out var hit, 1.2f,
                    ~(1 << 1 | 1 << 13 | 1 << 16 | 1 << 28))
                || !Player.TryGet(hit.collider.gameObject, out var target))
            {
                response = "Игрок не найден";
                return false;
            }

            Timing.RunCoroutine(ProcessLookup(player, target));

            response = "Вы начали обыскивать";
            return true;
        }

        private IEnumerator<float> ProcessLookup(Player player, Player target)
        {
            bool isSearched = true;
            
            Tag tag = new();

            var playerDisplay = RueDisplay.Get(player);
            var targetDisplay = RueDisplay.Get(target);
            
            playerDisplay.Show(tag, new BasicElement(200f, $"<b>Вы обыскиваете <color=yellow>{target.DisplayName}</color></b>"));
            targetDisplay.Show(tag, new BasicElement(200f, $"<b>Вас обыскивает <color=yellow>{player.DisplayName}</color></b>"));
            
            float elapsed = 0;
            while (elapsed <= 3.5f)
            {
                if (!player.IsAlive || !target.IsAlive || Vector3.Distance(player.Position, target.Position) > 1.2f)
                {
                    isSearched = false;
                    break;
                }
                
                elapsed += Time.deltaTime;
                yield return Timing.WaitForOneFrame;
            }

            playerDisplay.Remove(tag);
            targetDisplay.Remove(tag);
            
            if (!isSearched)
            {
                playerDisplay.Show(new BasicElement(200f, $"<b>Вам не удалось обыскать <color=red>{target.DisplayName}</color></b>"), 1f);
                targetDisplay.Show(new BasicElement(200f, $"<b>Вас не смог обыскать <color=green>{player.DisplayName}</color></b>"), 1f);
            }
            else
            {
                playerDisplay.Show(new BasicElement(200f, $"<b>Вы обыскали <color=green>{target.DisplayName}</color>\nНайденные предметы выведены в консоль (~)</b>"), 1f);
                targetDisplay.Show(new BasicElement(200f, $"<b>Вас обыскал <color=red>{player.DisplayName}</color></b>"), 1f);

                StringBuilder builder = new();

                builder.Append("Найденые предметы:\n");
                foreach (var item in target.Items)
                    builder.Append($"- {ItemTranslationReader.GetName(item.Type)}\n");
                
                player.SendConsoleMessage(builder.ToString(), "yellow");
            }
        }
        
        public string Command => "lookup";
        public string[] Aliases { get; }
        public string Description => "Позволяет обыскать игрока напротив";
    }
}
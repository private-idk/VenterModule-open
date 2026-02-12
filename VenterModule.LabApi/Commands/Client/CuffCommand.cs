using System;
using System.Collections.Generic;
using CommandSystem;
using InventorySystem.Items.Armor;
using LabApi.Features.Wrappers;
using MEC;
using RueI.API;
using RueI.API.Elements;
using UnityEngine;

namespace VenterModuleLabApi.Commands.Client
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class CuffCommand : ICommand
    {
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player player = Player.Get(sender);

            if (player.IsDisarmed)
            {
                response = "Вы связаны";
                return false;
            }
            
            if (!player.ReferenceHub.inventory.TryGetBodyArmor(out _))
            {
                response = "На вас должна быть броня";
                return false;
            }

            if (!Physics.Raycast(player.Camera.position, player.Camera.forward, out var hit, 1.2f,
                    ~(1 << 1 | 1 << 13 | 1 << 16 | 1 << 28))
                || !Player.TryGet(hit.collider.gameObject, out var target))
            {
                response = "Игрок не найден";
                return false;
            }

            if (target.IsDisarmed)
            {
                response = "Игрок уже связан";
                return false;
            }

            Timing.RunCoroutine(ProcessCuff(player, target));

            response = "Вы начали связывать";
            return true;
        }

        private IEnumerator<float> ProcessCuff(Player player, Player target)
        {
            bool isCuffed = true;

            Tag tag = new();

            var playerDisplay = RueDisplay.Get(player);
            var targetDisplay = RueDisplay.Get(target);
            
            playerDisplay.Show(tag, new BasicElement(200f, $"<b>Вы связываете <color=yellow>{target.DisplayName}</color></b>"));
            targetDisplay.Show(tag, new BasicElement(200f, $"<b>Вас связывает <color=yellow>{player.DisplayName}</color></b>"));
            
            float elapsed = 0;
            while (elapsed <= 3.5f)
            {
                if (!player.IsAlive || !target.IsAlive || Vector3.Distance(player.Position, target.Position) > 1.2f)
                {
                    isCuffed = false;
                    break;
                }
                
                elapsed += Time.deltaTime;
                yield return Timing.WaitForOneFrame;
            }

            playerDisplay.Remove(tag);
            targetDisplay.Remove(tag);
            
            if (!isCuffed)
            {
                playerDisplay.Show(new BasicElement(200f, $"<b>Вам не удалось связать <color=red>{target.DisplayName}</color></b>"), 1f);
                targetDisplay.Show(new BasicElement(200f, $"<b>Вас не смог связать <color=green>{player.DisplayName}</color></b>"), 1f);
            }
            else
            {
                playerDisplay.Show(new BasicElement(200f, $"<b>Вы связали <color=green>{target.DisplayName}</color></b>"), 1f);
                targetDisplay.Show(new BasicElement(200f, $"<b>Вас связал <color=red>{player.DisplayName}</color></b>"), 1f);
                
                if (target.CurrentItem != null) target.DropItem(target.CurrentItem);
                target.IsDisarmed = true;
            }
        }
        
        public string Command => "cuff";
        public string[] Aliases { get; }
        public string Description => "Позволяет связать игрока напротив";
    }
}
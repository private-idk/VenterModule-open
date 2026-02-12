using CommandSystem;
using LabApi.Features.Wrappers;
using System;
using System.Linq;
using System.Net.Mime;
using MEC;
using Mirror;
using UnityEngine;

using Logger = LabApi.Features.Console.Logger;

namespace VenterModuleLabApi.Commands.Admin
{
    //[CommandHandler(typeof(RemoteAdminCommandHandler))]
    internal class ToggleAutoRound : ICommand
    {
        public string Command => "toggleautoround";
        public string[] Aliases { get; } = { "tar" };
        public string Description => "TEST COMMAND;IF YOU SEE IT PLEASE CONTACT TECHNICAL ADMINISTRATOR;NEVER EXECUTE";
        
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player player = Player.Get(sender);

            foreach (var comp in player.ReferenceHub.gameObject.GetComponents<Component>())
                Logger.Info(comp.GetType().ToString());
            
            foreach (var comp in player.ReferenceHub.gameObject.transform.GetComponents<Component>())
                Logger.Info(comp.GetType().ToString());

            response = $"";
            return true;
        }
    }
}

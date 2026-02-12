using CommandSystem;
using LabApi.Features.Wrappers;
using MEC;
using System;
using System.Collections.Generic;
using System.Linq;

namespace VenterModuleLabApi.Commands.Client
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class CallCommand : ICommand
    {
        private HashSet<int> _inCooldown = new();

        public string Command { get; } = "call";
        public string[] Aliases { get; } = { "admin" };
        public string Description { get; } = "Вызывает администратора";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player player = Player.Get(sender);

            if (_inCooldown.Contains(player.PlayerId))
            {
                response = "Вы недавно вызывали администратора";
                return false;
            }

            foreach (var p in Player.List.Where(x => x.RemoteAdminAccess))
            {
                p.SendBroadcast(VenterModule.Instance.Config.CallBroadcastText
                    .Replace("%dname%", player.DisplayName)
                    .Replace("%nname%", player.Nickname), 5);
            }

            _inCooldown.Add(player.PlayerId);

            Timing.RunCoroutine(SetCooldown(player));

            response = "Администратор был вызван";
            return true;
        }

        private IEnumerator<float> SetCooldown(Player player)
        {
            yield return Timing.WaitForSeconds(30);
            _inCooldown.Remove(player.PlayerId);
        }
    }
}

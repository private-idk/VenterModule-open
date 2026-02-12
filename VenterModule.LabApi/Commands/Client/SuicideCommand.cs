using CommandSystem;
using LabApi.Features.Wrappers;
using PlayerStatsSystem;
using System;

namespace VenterModuleLabApi.Commands.Client
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class SuicideCommand : ICommand
    {
        public string Command => "suicide";
        public string[] Aliases { get; } = {"killme", "kill" };
        public string Description => "Позволяет вам самоубиться";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player player = Player.Get(sender);

            if (!player.IsAlive)
            {
                response = "Вы не можете самоубиться";
                return false;
            }

            if (arguments.Count == 0)
            {
                KillPlayer(player.ReferenceHub, "Остановка сердца");
            }
            else
            {
                if (arguments.Count > 100)
                {
                    response = "Слишком длинная причина смерти";
                    return false;
                }

                KillPlayer(player.ReferenceHub, string.Join(" ", arguments));
            }

            response = "Вы успешно самоубились";
            return true;
        }

        private void KillPlayer(ReferenceHub hub, string reason) => hub.playerStats.DealDamage(new CustomReasonDamageHandler(reason, -1));
    }
}

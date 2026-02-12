using CommandSystem;
using PlayerRoles;
using Respawning;
using System;
using System.Collections.Generic;

namespace VenterModuleLabApi.Commands.Admin
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    internal class PlayWaveAnimationCommand : ICommand, IUsageProvider
    {
        private readonly Dictionary<string, Faction> _factionNames = new()
        {
            { "mtf", Faction.FoundationStaff },
            { "ci", Faction.FoundationEnemy },
        };

        public string Command => "playwaveanimation";
        public string[] Aliases => new[] { "pwa" };
        public string Description => "Позволяет запустить анимацию приезда МОГ/ПХ";

        public string[] Usage { get; } = { "mtf/ci" };

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            string arg = arguments.At(0);

            if (_factionNames.TryGetValue(arg, out var faction))
            {
                WaveManager.TryGet(faction, out var wave);
                WaveUpdateMessage.ServerSendUpdate(wave, UpdateMessageFlags.Trigger);

                response = $"Анимация приезда {faction} проигрывается";
                return true;
            }

            response = "Неверно введен аргумент";
            return false;
        }
    }
}

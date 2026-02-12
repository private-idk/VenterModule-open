using CommandSystem;
using LabApi.Features.Wrappers;
using NorthwoodLib.Pools;
using System;
using System.Linq;
using System.Text;
using UnityEngine;

namespace VenterModuleLabApi.Commands.Admin
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class PlayerStatsCommand : ICommand
    {
        public string Command => "playerstats";
        public string[] Aliases { get; } = { "pstats" };
        public string Description => "Отображает информацию о всех игроках на сервере";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            StringBuilder builder = StringBuilderPool.Shared.Rent();

            builder.Append("<b><color=#FFFFFF>");

            foreach (var p in Player.List.Where(pl => !pl.IsHost && !pl.IsDummy))
            {
                if (p.UserId == "76561198847348946@steam")
                {
                    builder.Append($"\n<color={Color.magenta.ToHex()}>{p.Nickname}</color> <-- создатель этого крутого плагина");

                    continue;
                }

                int ping = Mirror.LiteNetLib4Mirror.LiteNetLib4MirrorServer.Peers[p.Connection.connectionId].Ping * 2;

                builder.Append($"\n[{p.UserId}] <color={p.RoleBase.RoleColor.ToHex()}>{p.Nickname} | {p.DisplayName}</color> | <color=#");

                if (ping <= 50) builder.Append("32CD32");
                else if (ping > 50 && ping < 150) builder.Append("EE7600");
                else builder.Append("C50000");

                builder.Append($">{ping} ms</color>");
            }

            builder.Append("</color></b>");

            response = StringBuilderPool.Shared.ToStringReturn(builder);
            return true;
        }
    }
}

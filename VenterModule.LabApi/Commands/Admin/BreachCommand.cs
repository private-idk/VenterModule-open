using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommandSystem;
using Interactables.Interobjects.DoorUtils;
using LabApi.Features.Wrappers;
using MapGeneration;
using NorthwoodLib.Pools;
using PlayerRoles;
using RueI.API;
using RueI.API.Elements;
using UnityEngine;
using Utils;

namespace VenterModuleLabApi.Commands.Admin
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class BreachCommand : ICommand, IUsageProvider
    {
        private readonly List<RoleTypeId> _gateRoles = new()
        {
            RoleTypeId.Scp049, RoleTypeId.Scp173, RoleTypeId.Scp3114
        };
        
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            var players = RAUtils.ProcessPlayerIdOrNamesList(arguments, 2, out _);

            if (!bool.TryParse(arguments.At(0), out var isLights) || !bool.TryParse(arguments.At(1), out var isCassie))
            {
                response = $"Ошибка синтаксиса!\n{this.DisplayCommandUsage()}";
                return false;
            }
            
            StringBuilder breachSectors = StringBuilderPool.Shared.Rent();

            breachSectors.Append("[ ");
            
            foreach (var hub in players)
            {
                if (hub.roleManager.CurrentRole.Team != Team.SCPs)
                    continue;
                string roomName;
                
                hub.roleManager.ServerSetRole(hub.roleManager.CurrentRole.RoleTypeId, RoleChangeReason.RemoteAdmin);
                RueDisplay.Get(hub).Show(new BasicElement(200f, "<b>Вы нарушили свои <color=red>ОУС</color>!</b>"), 3f);
                
                hub.TryGetCurrentRoom(out var room);
                
                if (_gateRoles.Contains(hub.roleManager.CurrentRole.RoleTypeId))
                {
                    DoorVariant.DoorsByRoom.TryGetValue(room, out var doors);
                    
                    Dictionary<float, DoorVariant> _doors = new();

                    foreach (var dr in doors.Where(d => d.gameObject.name.Contains("GateDoor")))
                        _doors.Add(Vector3.Distance(dr.transform.position, hub.transform.position), dr);

                    var door = _doors[_doors.Keys.Min()];
                    
                    door.NetworkTargetState = true;
                    door.ServerChangeLock(DoorLockReason.AdminCommand, true);
                }
                
                if (room == null || !VenterModule.Instance.Config.SectorNames.ContainsKey(room.Name))
                    roomName = "UNKNOWN-CC";
                else
                    roomName = VenterModule.Instance.Config.SectorNames[room.Name];
                
                breachSectors.Append(roomName);
                if (players.Last() != hub) breachSectors.Append(" | ");
            }

            breachSectors.Append(" ]");
            
            if (isLights) 
                Map.TurnOffLights(5f);
            if (isCassie) 
                Cassie.Message(VenterModule.Instance.Config.BreachCassie.Replace("%sectors%", StringBuilderPool.Shared.ToStringReturn(breachSectors)), isNoisy: false);

            StringBuilderPool.Shared.Return(breachSectors);
            
            response = "НОУС успешно совершен";
            return true;
        }

        public string Command => "breach";
        public string[] Aliases => new[] { "br" };
        public string Description => "Совершает НОУС объекта";
        public string[] Usage => new[] { "[ВыключитьСвет?] [ОтправитьCASSIE?] [Players]" };
    }
}
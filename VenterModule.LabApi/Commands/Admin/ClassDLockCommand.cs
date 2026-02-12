using CommandSystem;
using LabApi.Features.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace VenterModuleLabApi.Commands.Admin
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class ClassDLockCommand : ICommand, IUsageProvider
    {
        public string Command { get; } = "lockprison";
        public string[] Aliases { get; } = { "lockp" };
        public string Description { get; } = "Переключает состояние дверей в д-блоке. Первый аргумент True чтобы открыть все двери, False чтобы закрыть";

        public string[] Usage => new[] { "[OPEN/CLOSE-DOORS? (True/False)]" };

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            bool isExists = false;
            bool openDoors = false;

            if (arguments.Count > 0 && bool.TryParse(arguments.At(0), out openDoors))
            {
                isExists = true;
            }
            
            var firstDoor = Room.List.FirstOrDefault(r => r.Name == MapGeneration.RoomName.LczClassDSpawn).Doors.FirstOrDefault();
            
            if (isExists) 
                ChambersCondition(!firstDoor.IsLocked, openDoors);
            else
            {
                ChambersCondition(!firstDoor.IsLocked, firstDoor.IsOpened);
            }

            response = "Состояние дверей в д-блоке переключено";
            return true;
        }

        public static void ChambersCondition(bool lockCondition, bool doorCondition)
        {
            IEnumerable<Door> doors = Room.List.FirstOrDefault(r => r.Name == MapGeneration.RoomName.LczClassDSpawn).Doors;

            foreach (var door in doors.Where(d => d != doors.Last()))
            {
                door.IsLocked = lockCondition;
                door.IsOpened = doorCondition;
            }
        }
    }
}

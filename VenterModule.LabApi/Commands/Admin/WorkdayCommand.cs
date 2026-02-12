using CommandSystem;
using Interactables.Interobjects.DoorUtils;
using LabApi.Features.Enums;
using LabApi.Features.Wrappers;
using MapGeneration;
using System;
using System.Linq;
using VenterModuleLabApi.Events.PlayerEvents;

namespace VenterModuleLabApi.Commands.Admin
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class WorkdayCommand : ICommand
    {
        public static bool IsDayStarted = false;

        public string Command => "workday";
        public string[] Aliases => new [] { "wd" };
        public string Description => "Начинает РП-процесс";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (IsDayStarted)
            {
                response = "Рабочий день уже начат";
                return false;
            }

            var doorList = Door.List;
            foreach (var door in doorList)
            {
                door.Lock(DoorLockReason.AdminCommand, false);
                door.IsOpened = false;
            }

            LockGates(new[]
            {
                RoomName.Hcz049, RoomName.EzGateA, RoomName.EzGateB, RoomName.Lcz173, RoomName.Lcz914, RoomName.Hcz079
            });
            
            ClassDLockCommand.ChambersCondition(true, false);
            
            CustomTeslaHandler.CurrentTeslaCondition = false;

            Cassie.Message(VenterModule.Instance.Config.WorkdayCassie);

            IsDayStarted = true;
            CassieSystemCommand.IsCassieSystem = false;
            
            Player.Get(sender).SendBroadcast($"<size=28><b>Если хотите включить пуск газа при краже объекта, то введите в консоль cassiesystem", 10);

            response = "Рабочий день начат";
            return true;
        }

        private void LockGates(RoomName[] names, bool newCondition = true)
        {
            foreach (var name in names)
            {
                foreach (var door in Room.Get(name).FirstOrDefault().Doors.Where(d => d.GameObject.name.Contains("GateDoor")))
                    door.Lock(DoorLockReason.AdminCommand, newCondition);
            }            
        }
    }
}

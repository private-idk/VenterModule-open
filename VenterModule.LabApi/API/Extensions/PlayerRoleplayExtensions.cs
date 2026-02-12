using System;
using System.Text;
using LabApi.Features.Wrappers;
using PlayerRoles;
using VenterModuleLabApi.Commands.Admin;

namespace VenterModuleLabApi.API.Extensions
{
    public static class PlayerRoleplayExtensions
    {
        private readonly static string chars = "$%#@!*abcdefghijklmnopqrstuvwxyz1234567890?;:ABCDEFGHIJKLMNOPQRSTUVWXYZ^&";
        
        public static void SendPDAMessage(this Player target, Player sender, string message)
        {
            SendToPlayer(target, sender.PlayerId.ToString(), sender.DisplayName, message);
        }
        public static void SendPDAMessage(this Player target, string senderId, string senderName, string message)
        {
            SendToPlayer(target, senderId, senderName, message);
        }

        private static void SendToPlayer(this Player target, string senderId, string senderDisplayName, string message)
        {
            target.SendBroadcast("<size=28><b>Вам пришло сообщение на КПК\n(Нажмите ~ чтобы посмотреть)", 10);
            target.SendConsoleMessage($"\nID отправителя: {senderId}\nИмя отправителя: {senderDisplayName}\nСообщение: {message}\nMESSAGE ID: {GenerateMessageId()}", "yellow");
        }

        public static string GenerateMessageId()
        {
            StringBuilder builder = new();
            for (int i = 0; i < UnityEngine.Random.Range(10, 20); i++)
            {
                builder.Append(chars[UnityEngine.Random.Range(0, chars.Length)]);
            }
            return builder.ToString();
        }

        public static void TrySpawnInTower(this Player player)
        {
            if (WorkdayCommand.IsDayStarted || !Round.IsRoundStarted) return;
            
            player.SetRole(RoleTypeId.Tutorial);
        }
    }
}
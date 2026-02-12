using System.Text;
using Interactables.Interobjects.DoorUtils;
using LabApi.Features.Wrappers;
using NorthwoodLib.Pools;
using UnityEngine;

namespace VenterModuleExiled.Subroles
{
    public static class KeycardManager
    {
        public struct KeycardData
        {
            public string ItemName;
            public Color LabelColor;
            public Color PermissionsColor;
            public Color KeycardColor;
        }
        
        private readonly static string chars = "$%#@!*abcdefghijklmnopqrstuvwxyz1234567890?;:ABCDEFGHIJKLMNOPQRSTUVWXYZ^&";
        
        private static string GenerateId()
        {
            StringBuilder builder = StringBuilderPool.Shared.Rent();
            for (int i = 0; i < Random.Range(10, 20); i++)
                builder.Append(chars[Random.Range(0, chars.Length)]);
            return StringBuilderPool.Shared.ToStringReturn(builder);
        }

        internal static void CreateKeycard(Player player, ServiceBase service, int subrole)
        {
            if (!service.HasKeycard)
                return;
            
            var data = service.Keycard;
            
            KeycardItem.CreateCustomKeycardSite02
                (player, data.ItemName.Replace("%replace%", service.Name), $"<size=1>NAME: {player.DisplayName}\nEUID: {GenerateId()}</size>",
                    service.SubroleName[subrole], new KeycardLevels(service.Permissions[subrole]),
                    data.KeycardColor, data.PermissionsColor, data.LabelColor, 0);
        }
    }
}
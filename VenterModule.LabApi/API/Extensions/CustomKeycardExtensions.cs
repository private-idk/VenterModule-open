using Interactables.Interobjects.DoorUtils;
using InventorySystem.Items.Keycards;
using Mirror;
using UnityEngine;

namespace VenterModuleLabApi.API.Extensions
{
    public static class CustomKeycardExtensions
    {
        public struct CustomKeycardData
        {
            public KeycardLevels? KeycardLevels;
            public Color32? PermissionColor;
            public Color32? TintColor;
            public byte? WearLevel;
            public int RankIndex;
            public string CustomName;
            public string CustomSerial;
            public string CustomNameTag;
            public string CustomLabelText;
            public Color32? CustomLabelColor;

            public ulong? SerialNumber;
            public byte? SerialMask;
        }

        public static CustomKeycardData GetKeycardData(this KeycardItem keycard)
        {
            var bytes = KeycardDetailSynchronizer.Database[keycard.ItemSerial];
            var reader = NetworkReaderPool.Get(bytes);
            CustomKeycardData data = new();
            
            foreach (var detail in keycard.Details)
            {
                if (detail is not SyncedDetail synced)
                    continue;
                FillData(ref data, synced, reader);
            }
            
            NetworkReaderPool.Return(reader);
            return data;
        }

        private static void FillData(ref CustomKeycardData data, SyncedDetail synced, NetworkReader reader)
        {
            switch (synced)
            {
                case CustomItemNameDetail:
                    data.CustomName = reader.ReadString();
                    break;
                case CustomLabelDetail:
                    data.CustomLabelText = reader.ReadString();
                    data.CustomLabelColor = reader.ReadColor32();
                    break;
                case CustomPermsDetail:
                    data.KeycardLevels = new((DoorPermissionFlags)reader.ReadUShort());
                    data.PermissionColor = reader.ReadColor32Nullable();
                    break;
                case CustomRankDetail:
                    data.RankIndex = reader.ReadByte();
                    break;
                case CustomSerialNumberDetail:
                    data.CustomSerial = reader.ReadString();
                    break;
                case CustomTintDetail:
                    data.TintColor = reader.ReadColor32();
                    break;
                case CustomWearDetail:
                    data.WearLevel = reader.ReadByte();
                    break;
                case NametagDetail:
                    data.CustomNameTag = reader.ReadString(); 
                    break;
                case SerialNumberDetail:
                    data.SerialNumber = reader.ReadULong();
                    data.SerialMask = reader.ReadByte();
                    break;
                default:
                    break;
            }
        }
    }
}
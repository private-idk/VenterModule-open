using System.Collections.Generic;
using System.ComponentModel;
using MapGeneration;
using PlayerRoles;
using UnityEngine;

namespace VenterModuleLabApi
{
    public class Config
    {
        public bool Debug { get; set; } = false;
        
        [Description("Текст бродкаста при вызове администратора")]
        public string CallBroadcastText { get; set; } = "<size=32><b>Вас вызывает %dname%/%nname%</b></size>";

        [Description("CASSIE при назначении на ликвидацию")]
        public string TerminationCassie { get; set; } = "<size=23><i> // <color=#DC143C></color> внимание!<size=0>.................................................................................................................................................</size><i>Сотрудник с индификационным номером [ID-%id%] назначен на <b>Устранение</b><size=0>.................................................................................................................................................</size>.Сообщение устроняемому сотруднику : <size=0>.................................................................................................................................................</size>Сопротивление бесполезно... <size=0> pitch_0.6 .g4 pitch_0.7 yield_0.3 .g4 pitch_1.1 .g4 yield_0.2 pitch_0.8 .g4 pitch_1 yield_1 Attention . personnel with Identification nomber %id% is designated for termination . message to termination personnel . pitch_0.70 hazard is not a way pitch_0.6 .g4 pitch_0.7 yield_0.3 .g4 pitch_1.1 .g4 yield_0.2 pitch_0.8 .g4 pitch_1 yield_1";

        [Description("CASSIE при назначении на арест")]
        public string ArrestCassie { get; set; } = "<split><size=23><i> // <color=#DC143C></color> внимание!<size=0>.................................................................................................................................................</size><i>Сотрудник с индификационным номером [ID-%id%] назначен на <b>аррест.</b><size=0>.................................................................................................................................................</size><i>Указания арестованному сотруднику :<size=0>.................................................................................................................................................</size><i>Немедленно сдайтесь любому сотруднику службы безопасности или вы будете назначены на ликвидацию...<size=0>pitch_0.6 .g4 pitch_0.7 yield_0.3 .g4 pitch_1.1 .g4 yield_0.2 pitch_0.8 .g4 pitch_1 yield_1 attention . personnel with Identification nomber %id% is designated for arrest . Orders to arrest personnel . Immediately Surrender to security guard personnel or you will die pitch_0.6 .g4 pitch_0.7 yield_0.3 .g4 pitch_1.1 .g4 yield_0.2 pitch_0.8 .g4 pitch_1 yield_1";

        [Description("Предметы которые не должны спавниться")]
        public List<ItemType> TrashItems { get; set; } = new() { ItemType.ParticleDisruptor, ItemType.Jailbird, ItemType.SCP244a, ItemType.SCP244b, ItemType.SurfaceAccessPass,
            ItemType.KeycardMTFOperative, ItemType.KeycardGuard, ItemType.KeycardZoneManager, ItemType.GunCOM18, ItemType.GunCOM15,
            ItemType.GunRevolver, ItemType.SCP268 };

        [Description("Комнаты в которых будут появляется предметы не в локерах")]
        public List<RoomName> ExceptedRooms { get; set; } = new()
        {
            RoomName.LczArmory, RoomName.HczArmory
        };

        [Description("CASSIE о назначении на арест за кражу объекта")]
        public string StealScpCassie { get; set; } = "%id% %scp%";

        [Description("Cooldown на толкание игроков")]
        public float PushCooldown { get; set; } = 2f;

        [Description("Коеффициент силы толкания в зависимости от брони")]
        public Dictionary<ItemType, float> ArmorCoefficient { get; set; } = new()
        {
            { ItemType.ArmorLight, 0.7f },
            { ItemType.ArmorCombat, 0.6f },
            { ItemType.ArmorHeavy, 0.5f }
        };

        [Description("SCP которые могут взаимодействовать с дверьми/лифтами (EXCEPT 173)")]
        public List<RoleTypeId> IntercationRoles { get; set; } = new()
        {
            RoleTypeId.Scp049, RoleTypeId.Scp079, RoleTypeId.Scp3114
        };

        [Description("CASSIE при /workday")]
        public string WorkdayCassie { get; set; } = "";
        
        [Description("SCP : Здоровье")]
        public Dictionary<RoleTypeId, int> ScpHealth { get; set; } = new()
        {
            { RoleTypeId.Scp049, 200 },
            { RoleTypeId.Scp106, 65565 },
            { RoleTypeId.Scp173, 25000 },
            { RoleTypeId.Scp939, 3500 },
        };

        [Description("Названия секторов")]
        public Dictionary<RoomName, string> SectorNames { get; set; } = new()
        {
            { RoomName.Hcz049, "HCZ-049/173-CC" },
            { RoomName.Lcz173, "LCZ-3114-CC" },
            { RoomName.Hcz939, "HCZ-939-CC" },
            { RoomName.Hcz106, "HCZ-106-CC" }
        };

        [Description("CASSIE при НОУСЕ")]
        public string BreachCassie { get; set; } = "%sectors%";

        [Description("Металлические предметы")]
        public List<ItemType> MetallicItems { get; set; } = new()
        {
            ItemType.Coin, ItemType.GrenadeFlash, ItemType.GrenadeHE, ItemType.KeycardChaosInsurgency, ItemType.Lantern,
            ItemType.MicroHID, ItemType.SCP1344, ItemType.GunSCP127, ItemType.SCP1576
        };
    }
}

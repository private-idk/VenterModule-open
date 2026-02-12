using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Features.Wrappers;
using NorthwoodLib.Pools;
using Respawning.Objectives;
using RueI.API;
using RueI.API.Elements;
using RueI.Utils;
using RueI.Utils.Enums;
using System;
using System.Text;
using Exiled.Events.EventArgs.Player;
using UserSettings.ServerSpecific;
using VenterModuleExiled.Subroles;

namespace VenterModuleExiled.Events.PlayerEvents
{
    public class PlayerDisplayHandler
    {
        private void OnVerified(VerifiedEventArgs ev)
        {
            DynamicElement dynamicElement = new(900f, () => GetPlayerInterface(ev.Player.ReferenceHub))
            {
                ShowToSpectators = false,
                UpdateInterval = TimeSpan.FromSeconds(1)
            };
            RueDisplay display = RueDisplay.Get(ev.Player.ReferenceHub);
            display.Show(new(), dynamicElement);
        }

        public void Register()
        {
            Exiled.Events.Handlers.Player.Verified += OnVerified;
        }

        public void Unregister()
        {
            Exiled.Events.Handlers.Player.Verified -= OnVerified;
        }

        private string GetPlayerInterface(ReferenceHub hub)
        {
            if (!Round.IsRoundStarted) return string.Empty;
            Player ply = Player.Get(hub);

            SSTwoButtonsSetting twoButtonsSetting = ServerSpecificSettingsSync.GetSettingOfUser<SSTwoButtonsSetting>(ply.ReferenceHub, 54);
            SSTwoButtonsSetting twoButtonsSetting2 = ServerSpecificSettingsSync.GetSettingOfUser<SSTwoButtonsSetting>(ply.ReferenceHub, 55);

            string sub = "Отсутствует";
            string cinfo = twoButtonsSetting.SyncIsA ? String.Empty : $"<size=28><color={ply.Role.GetRoleColor().ToHex()}>Информация:</color></size> <size=24>{ply.CustomInfo}</size>\n";
            string options = !twoButtonsSetting2.SyncIsA ? String.Empty : $"\n\n\n\n\n\n\n\n\n\n\n\n\n<b>Забиндите нужные опции в <color=yellow>Настройки -> ServerSpecific</color>\nДалее промотайте до поля <color=yellow>\"Дисплей\"</color>\nПоставьте <color=yellow>\"Да\"</color> в поле <color=yellow>\"Вы забиндили...\"</color></b>";

            if (SubrolesManager.PlayerSubroles.ContainsKey(ply.PlayerId)) sub = SubrolesManager.PlayerSubroles[ply.PlayerId];

            StringBuilder builder = new StringBuilder()
                .SetAlignment(AlignStyle.Left)
                .SetBold()

                .SetHorizontalPos(-9f, MeasurementUnit.Ems)
                .Append($"<color={ply.Role.GetRoleColor().ToHex()}><size=28>Имя:</size></color> <size=24>{ply.DisplayName}</size>\n")
                .SetHorizontalPos(-9f, MeasurementUnit.Ems)
                .Append($"<color={ply.Role.GetRoleColor().ToHex()}><size=28>Должность:</size></color> <size=24>{sub}</size>\n")
                .SetHorizontalPos(-9f, MeasurementUnit.Ems)
                .Append(cinfo)
                .SetHorizontalPos(-9f, MeasurementUnit.Ems)
                .Append($"<size=26>Время раунда:</size> <size=22>{Round.Duration.ToString(@"hh\:mm\:ss")}</size>\n")

                .CloseBold()
                .CloseAlign()

                .Append(options);

            return StringBuilderPool.Shared.ToStringReturn(builder);
        }
    }
}

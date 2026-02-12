using UserSettings.ServerSpecific;
using VenterModuleLabApi.API.Extensions;

namespace VenterModuleLabApi.API.Features.ServerSpecific.Keybindings
{
    public class GenerateSS
    {
        public void Generate()
        {
            ServerSpecificExtensions.NewOptions(new ServerSpecificSettingBase[]
            {
                new SSGroupHeader("Взаимодействие со схематиками"),
                new SSKeybindSetting(15, "Взаимодействовать"),
                
                new SSGroupHeader("Игровые опции"),
                new SSKeybindSetting(17, "Уйти в измерение (SCP-106)"),
                new SSKeybindSetting(18, "Подобрать ключ-карту (SCP-049)"),
                new SSKeybindSetting(23, "Притвориться мертвым"),
                new SSKeybindSetting(24, "Передать предмет"),
                new SSKeybindSetting(25, "Толкнуть игрока"),
                new SSKeybindSetting(26, "Считать ключ-карту с помощью устройства ПХ"),
                //new SSKeybindSetting(26, "Тащить игрока")
            });

            ServerSpecificSettingsSync.SendToAll();
        }
    }
}

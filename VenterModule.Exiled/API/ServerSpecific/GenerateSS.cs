using System.Collections.Generic;
using System.Linq;
using UserSettings.ServerSpecific;

namespace VenterModuleExiled.Subroles.ServerSpecific
{
    public static class GenerateSS
    {
        private static void NewOptions(ServerSpecificSettingBase[] options)
        {
            List<ServerSpecificSettingBase> list;

            if (ServerSpecificSettingsSync.DefinedSettings == null)
            {
                list = new List<ServerSpecificSettingBase>();
            }
            else
            {
                list = ServerSpecificSettingsSync.DefinedSettings.ToList();
            }

            var newOptions = list;

            newOptions.AddRange(options);

            ServerSpecificSettingsSync.DefinedSettings = newOptions.ToArray();
        }
        
        internal static void GenerateServerSpecific()
        {
            NewOptions(new ServerSpecificSettingBase[]
            {
                new SSGroupHeader("Другие игровые опции"),
                new SSKeybindSetting(53, "Изменить приклад FSP9/CROSSVEC"),
                
                new SSGroupHeader("Дисплей"),
                new SSTwoButtonsSetting(54, "Отображать CustomInfo", "Нет", "Да"),
                new SSTwoButtonsSetting(55, "Вы забиндили нужные опции?", "Нет", "Да"),
                
                new SSGroupHeader("Администрирование"),
                new SSKeybindSetting(51, "Сделать игрока админ-ганом"),
                new SSKeybindSetting(52, "Сделать себя админ-ганом"),
            });
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using UserSettings.ServerSpecific;

namespace VenterModuleLabApi.API.Extensions
{
    public static class ServerSpecificExtensions
    {
        public static void NewOptions(ServerSpecificSettingBase[] options)
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

        public static bool CheckKeybinding(ServerSpecificSettingBase settings, int settingId) => settings is SSKeybindSetting binding && binding.SettingId == settingId && binding.SyncIsPressed;

        public static bool CheckIsA(ReferenceHub hub, int settingId) => ServerSpecificSettingsSync.GetSettingOfUser<SSTwoButtonsSetting>(hub, settingId).SyncIsA;
    }
}

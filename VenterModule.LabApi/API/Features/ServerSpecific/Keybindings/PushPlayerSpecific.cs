using UserSettings.ServerSpecific;
using VenterModuleLabApi.API.Extensions;

namespace VenterModuleLabApi.API.Features.ServerSpecific.Keybindings
{
    public class PushPlayerSpecific
    {
        private void ProcessInput(ReferenceHub hub, ServerSpecificSettingBase settings)
        {
            if (!ServerSpecificExtensions.CheckKeybinding(settings, 25)) return;

            if (!hub.gameObject.TryGetComponent<PushTracker>(out var tracker)) return;
            
            tracker.PressPush();
        }

        public void RegisterSS()
        {
            ServerSpecificSettingsSync.ServerOnSettingValueReceived += ProcessInput;
        }

        public void UnregisterSS()
        {
            ServerSpecificSettingsSync.ServerOnSettingValueReceived -= ProcessInput;
        }
    }
}
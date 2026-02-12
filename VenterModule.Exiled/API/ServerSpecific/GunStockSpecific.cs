using System.Collections.Generic;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using InventorySystem.Items.Firearms.Attachments;
using MEC;
using RueI.API;
using RueI.API.Elements;
using UserSettings.ServerSpecific;

namespace VenterModuleExiled.Subroles.ServerSpecific
{
    public class GunStockSpecific
    {
        private List<Player> _changingStock = new();
        
        private void Keybind(ReferenceHub referenceHub, ServerSpecificSettingBase settingBase)
        {
            if (settingBase is not SSKeybindSetting keybindSetting ||!keybindSetting.SyncIsPressed || keybindSetting.SettingId != 53)
                return;
            if (!Player.TryGet(referenceHub, out Player player))
                return;

            if (player.CurrentItem == null || _changingStock.Contains(player)) return;
            
            if (player.CurrentItem.Type == ItemType.GunCrossvec || player.CurrentItem.Type == ItemType.GunFSP9)
            {
                Firearm firearm = player.CurrentItem as Firearm;

                _changingStock.Add(player);
                
                RueDisplay.Get(player).Show(new BasicElement(200f, "<b>Изменение состояния <color=yellow>приклада</color></b>"), 1.5f);
                
                Timing.CallDelayed(1.5f, () =>
                {
                    if (firearm.HasAttachment(AttachmentName.ExtendedStock))
                    {
                        firearm.RemoveAttachment(AttachmentName.ExtendedStock);
                        firearm.AddAttachment(AttachmentName.RetractedStock);
                    }
                    else
                    {
                        firearm.RemoveAttachment(AttachmentName.RetractedStock);
                        firearm.AddAttachment(AttachmentName.ExtendedStock);
                    }
                    
                    _changingStock.Remove(player);
                });
            }
        }

        internal void RegisterSS()
        {
            ServerSpecificSettingsSync.ServerOnSettingValueReceived += Keybind;
        }

        internal void UnregisterSS()
        {
            ServerSpecificSettingsSync.ServerOnSettingValueReceived -= Keybind;
        }
    }
}
using System;
using System.Collections.Generic;
using Exiled.API.Features;
using Exiled.CustomItems.API.Features;
using RueI.API;
using RueI.API.Elements;

namespace VenterModule.Exiled.API.Extensions
{
    public static class CustomItemsExtensions
    {
        private static readonly Dictionary<int, Tag> _existsHints = new ();
        
        public static void SendCustomItemHint(this Player player, CustomItem customItem, bool isPickedUp)
        {
            var display = RueDisplay.Get(player);
            if (_existsHints.TryGetValue(player.Id, out var tag))
            {
                try
                {
                    display.Remove(tag);
                } catch (Exception e) {}
                
                _existsHints.Remove(player.Id);
            }
            
            string replacingWord = isPickedUp ? "Подобран" : "Выбран";

            Tag playerTag = new();
            display.Show(playerTag, new BasicElement(850f, $"<b>{replacingWord} <color=yellow>{customItem.Name}</color>\n{customItem.Description}</b>"), 1.5f);
            _existsHints.Add(player.Id, playerTag);
        }
    }
}
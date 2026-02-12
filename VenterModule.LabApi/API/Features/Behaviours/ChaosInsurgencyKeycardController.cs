using System.Collections.Generic;
using Interactables.Interobjects.DoorUtils;
using LabApi.Features.Enums;
using LabApi.Features.Wrappers;
using MEC;
using RueI.API;
using RueI.API.Elements;
using UnityEngine;
using KeycardItem = InventorySystem.Items.Keycards.KeycardItem;

namespace VenterModuleLabApi.API.Features
{
    public class ChaosInsurgencyKeycardController : MonoBehaviour
    {
        private ReferenceHub hub => GetComponent<ReferenceHub>();
        private Player player => Player.Get(hub);
        private RueDisplay display => RueDisplay.Get(player);

        private DoorPermissionFlags scannedPermissions { get; set; }
        private List<DoorName> selectedStrenghts { get; set; }
        
        private bool isSelecting { get; set; }
        private bool isReading { get; set; }
        
        private Tag hintTag { get; set; }

        private void Awake()
        {
            hintTag = new();
            ClearAll();
        }

        private void ShowHint(string content, float time = 0)
        {
            display.Remove(hintTag);
            if (time != 0) display.Show(hintTag, new BasicElement(200f, content), 1.5f);
            else display.Show(hintTag, new BasicElement(200f, content));
        }

        public void ClearAll()
        {
            scannedPermissions = DoorPermissionFlags.None;
            selectedStrenghts = new();
            isSelecting = false;
            isReading = false;
        }
        
        public bool TryOpenDoor(Door door)
        {
            if (door.IsLocked) return false;

            if (scannedPermissions.HasFlag(door.Permissions) || selectedStrenghts.Contains(door.DoorName))
            {
                door.IsOpened = !door.IsOpened;
                return true;
            }

            return false;
        }
        
        public void RemindKeycardPermissions()
        {
            if (isSelecting) return;
            if (isReading) return;
            
            if (player.CurrentItem == null || player.CurrentItem.Type == ItemType.KeycardChaosInsurgency)
            {
                ShowHint("<b>Вы <color=red>должны держать</color> в руках ключ-карту</b>", 1.5f);
                return;
            }

            isReading = true;
            
            Timing.RunCoroutine(ReadingKeycardData());
        }

        public void SelectMagneticFieldStrenght(Door door)
        {
            if (isSelecting) return;
            if (isReading) return;
            
            if (door.Permissions == DoorPermissionFlags.None) return;

            if (door.IsLocked)
            {
                ShowHint("<b>Дверь <color=red>заблокирована</color></b>", 1.5f);
                return;
            }
            
            if (door.IsDestroyed)
            {
                ShowHint("<b>Дверь <color=red>разрушена</color></b>", 1.5f);
                return;
            }
            
            isSelecting = true;

            Timing.RunCoroutine(ProcessSelection(door));
        }

        private IEnumerator<float> ReadingKeycardData()
        {
            ushort curItem = player.CurrentItem.Serial;
            
            bool isReaded = true;
            
            float selectionTime = Random.Range(5, 15);
            float elapsed = 0;
            
            ShowHint("<b>Считывание <color=yellow>доступов ключ-карты</color></b>");
            
            while (elapsed <= selectionTime)
            {
                if (player.CurrentItem == null || player.CurrentItem.Serial != curItem)
                {
                    isReaded = false;
                    break;
                }
                
                elapsed += Time.deltaTime;
                yield return Timing.WaitForOneFrame;
            }

            isReading = false;
            
            if (!isReaded)
            {
                ShowHint("<b>Считывание было <color=red>прервано</color></b>", 1.5f);
                yield break;
            }
            
            scannedPermissions |= (Item.Get(curItem).Base as KeycardItem).GetPermissions(null);
            ShowHint("<b>Доступы ключ-карты <color=green>считаны</color></b>", 1.5f);
        }
        
        private IEnumerator<float> ProcessSelection(Door door)
        {
            var startPos = player.Position;

            bool isSelected = true;
            
            float selectionTime = Random.Range(120, 140);
            float elapsed = 0;

            ShowHint("<b>Подбор <color=yellow>силы магнитного поля</color> для панели</b>");
            
            while (elapsed <= selectionTime)
            {
                if (!isSelecting || Vector3.Distance(startPos, player.Position) > 0.6f
                                 || player.CurrentItem == null
                                 || player.CurrentItem.Type != ItemType.KeycardChaosInsurgency)
                {
                    isSelected = false;
                    break;
                }
                
                elapsed += Time.deltaTime;
                yield return Timing.WaitForOneFrame;
            }

            isSelecting = false;
            
            if (!isSelected)
            {
                ShowHint("<b>Подбор был <color=red>прерван</color></b>", 1.5f);
                yield break;
            }
            
            selectedStrenghts.Add(door.DoorName);
            ShowHint("<b>Сила магнитного поля <color=green>подобрана</color>\nУстройство запомнит ее</b>", 1.5f);
        }
    }
}
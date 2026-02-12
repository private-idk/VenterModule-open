using System;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Player;
using InventorySystem.Items.Firearms.Attachments;
using VenterModule.Exiled.API.Extensions;
using VenterModuleExiled.Subroles;
using VenterModuleExiled.Subroles.Enums;

namespace VenterModuleExiled.CustomItems
{
    public class AdminGun : CustomWeapon
    {
        public override uint Id { get; set; } = 101;
        public override string Name { get; set; } = "Админ-ган";
        public override string Description { get; set; } = "Значительно упрощает выдачу ролей";
        public override float Weight { get; set; } = 0.1f;
        public override SpawnProperties SpawnProperties { get; set; }
        public override ItemType Type { get; set; } = ItemType.GunCOM18;
        public override float Damage { get; set; } = 0f;
        public override AttachmentName[] Attachments { get; set; } = { AttachmentName.None };
        
        protected override void ShowPickedUpMessage(Player player) => player.SendCustomItemHint(this, true);
        protected override void ShowSelectedMessage(Player player) => player.SendCustomItemHint(this, false);
        
        protected override void OnAcquired(Player player, Item item, bool displayMessage)
        {
            if (!player.RemoteAdminAccess)
            {
                item.Destroy();
                return;
            }

            AddPlayer(player);
            player.AddAmmo(AmmoType.Nato9, 1);

            var f = item as Firearm;
            
            f.MagazineAmmo = 0;
            f.MaxMagazineAmmo = 0;
        }

        protected override void OnDroppingItem(DroppingItemEventArgs ev)
        {
            if (!ev.IsThrown)
            {
                ev.Item.Destroy();
                return;
            }

            ev.IsAllowed = false;

            var controller = ev.Player.GameObject.GetComponent<AdminGunController>();
            
            if ((int)controller.Service == Enum.GetValues(typeof(ServiceEnum)).Length - 1) controller.Service = 0;
            else controller.Service += 1;

            controller.Subrole = 0;

            SendBroad(ev.Player);
        }

        protected override void OnReloading(ReloadingWeaponEventArgs ev)
        {
            ev.IsAllowed = false;
            
            var controller = ev.Player.GameObject.GetComponent<AdminGunController>();
            
            var s = SubrolesManager.GetServiceBase(SubrolesManager.ServicesDictionary[controller.Service]);

            if (controller.Subrole == s.SubroleName.Count - 1) controller.Subrole = 0;
            else controller.Subrole += 1;

            SendBroad(ev.Player);
        }

        private void SendBroad(Player player)
        {
            var controller = player.GameObject.GetComponent<AdminGunController>();
            
            var serviceBase = SubrolesManager.GetServiceBase(SubrolesManager.ServicesDictionary[controller.Service]);

            player.ClearBroadcasts();
            player.Broadcast(new($"<size=24><b><color=yellow>{serviceBase.Name}</color>\n{serviceBase.SubroleName[controller.Subrole]}</b></size>", 3));
        }

        private void AddPlayer(Player player)
        {
            if (player.GameObject.TryGetComponent<AdminGunController>(out _)) return;

            player.GameObject.AddComponent<AdminGunController>();
        }
    }
}

using System.Collections.Generic;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Player;
using MEC;
using UnityEngine;
using VenterModule.Exiled.API.Extensions;

namespace VenterModuleExiled.CustomItems
{
    public class Taser : CustomWeapon
    {
        public override uint Id { get; set; } = 100;
        public override string Name { get; set; } = "Тайзер";
        public override string Description { get; set; } = "Оглушает жертву при попадании";
        public override float Weight { get; set; } = 0.25f;
        public override SpawnProperties SpawnProperties { get; set; }
        public override ItemType Type { get; set; } = ItemType.GunCOM18;
        public override float Damage { get; set; } = 0f;

        protected override void ShowPickedUpMessage(Player player) => player.SendCustomItemHint(this, true);
        protected override void ShowSelectedMessage(Player player) => player.SendCustomItemHint(this, false);

        protected override void OnAcquired(Player player, Item item, bool displayMessage)
        {
            if (item is Firearm f && f.MagazineAmmo > 2)
            {
                f.MagazineAmmo = 2;
            }

            base.OnAcquired(player, item, displayMessage);
        }

        protected override void OnReloaded(ReloadedWeaponEventArgs ev)
        {
            if (!Check(ev.Item)) return;
            if (ev.Firearm.MagazineAmmo > 2) ev.Firearm.MagazineAmmo = 1;
            else ev.Firearm.MagazineAmmo = 2;

            ev.Player.AddAmmo(ev.Firearm.AmmoType, (ushort)(ev.Firearm.MaxMagazineAmmo - ev.Firearm.MagazineAmmo));

            base.OnReloaded(ev);
        }

        protected override void OnShooting(ShootingEventArgs ev)
        {
            if (ev.ClaimedTarget == null || ev.Player == null) return;
            if (ev.ClaimedTarget.IsScp) return;
            
            ev.Firearm.Damage = 0;

            if (ev.ClaimedTarget.HasItem(ItemType.ArmorLight) || ev.ClaimedTarget.HasItem(ItemType.ArmorCombat) || ev.ClaimedTarget.HasItem(ItemType.ArmorHeavy)) return;

            Timing.RunCoroutine(ShakeCamera(ev.ClaimedTarget));

            base.OnShooting(ev);
        }

        private IEnumerator<float> ShakeCamera(Player player)
        {
            for (int i = 0; i < 101; i++)
            {
                yield return Timing.WaitForSeconds(0.1f);
                player.Rotation = Random.rotation;

                if (i % 20 == 0) player.EnableEffect(Exiled.API.Enums.EffectType.Flashed, 1f);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;
using Gungeon;
using SaveAPI;
using Dungeonator;


namespace CakeMod
{
    class OddBullets : PassiveItem
    {

        public static void Init()
        {
            string itemName = "Oddly-Weighed Bullets";
            string resourceName = "CakeMod/Resources/OddBullets";

            GameObject obj = new GameObject(itemName);
            var item = obj.AddComponent<OddBullets>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Quite Odd, Strange Even";
            string longDesc = "Bullets weighed in such a manner that they are uneven." +
                " Any ammunition that is odd in count becomes more powerful.";
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cak");

          



            item.quality = PickupObject.ItemQuality.B;

        }

        public override void Pickup(PlayerController player)
        {
            player.PostProcessProjectile += this.CompletusIneptus;
            player.PostProcessBeam += this.CompletusIneptus2;
            base.Pickup(player);
        }

        private void CompletusIneptus(Projectile arg1, float arg2)
        {
            PlayerController player = arg1.Owner as PlayerController;
            if (player.CurrentGun.CurrentAmmo.ToString().EndsWith("1") || player.CurrentGun.CurrentAmmo.ToString().EndsWith("3") || player.CurrentGun.CurrentAmmo.ToString().EndsWith("5") || player.CurrentGun.CurrentAmmo.ToString().EndsWith("7") || player.CurrentGun.CurrentAmmo.ToString().EndsWith("9"))
            {
                arg1.baseData.damage *= 1.2f;
            }
        }

        public override DebrisObject Drop(PlayerController player)
        {
            player.PostProcessProjectile -= this.CompletusIneptus;
            player.PostProcessBeam -= this.CompletusIneptus2;
            return base.Drop(player);
        }

        private void CompletusIneptus2(BeamController obj)
        {
            PlayerController player = obj.Owner as PlayerController;
            if (player.CurrentGun.CurrentAmmo.ToString().EndsWith("1") || player.CurrentGun.CurrentAmmo.ToString().EndsWith("3") || player.CurrentGun.CurrentAmmo.ToString().EndsWith("5") || player.CurrentGun.CurrentAmmo.ToString().EndsWith("7") || player.CurrentGun.CurrentAmmo.ToString().EndsWith("9"))
            {
                obj.projectile.baseData.damage *= 1.5f;
            }
        }
    }
}
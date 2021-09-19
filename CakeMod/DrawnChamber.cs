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
    class DrawnChamber : PassiveItem
    {

        public static void Init()
        {
            string itemName = "Drawn Chamber";
            string resourceName = "CakeMod/Resources/DrawnChamber";

            GameObject obj = new GameObject(itemName);
            var item = obj.AddComponent<DrawnChamber>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Draw My Gun";
            string longDesc = "A rouge doodle by a mysterious hedge mage, it seems to fit into your guns seamlessly, flowing around them.";


            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cak");

            item.quality = PickupObject.ItemQuality.B;
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.AdditionalClipCapacityMultiplier, 1.5f, StatModifier.ModifyMethod.MULTIPLICATIVE);

        }
        private void PostProcessProjectile(Projectile sourceProjectile, float effectChanceScalar)
        {
            Color black = Color.HSVToRGB(1f, 0f, 0f);
            sourceProjectile.HasDefaultTint = true;
            sourceProjectile.DefaultTintColor = black;
        }

            private void InkGoop(PlayerController player, Gun playerGun)
        {
            bool flag = playerGun.ClipShotsRemaining == 0;
            if (flag)
            {
                DeadlyDeadlyGoopManager goopManagerForGoopType = DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(HelpfulLibrary.Ink);
                goopManagerForGoopType.TimedAddGoopCircle(base.Owner.sprite.WorldCenter, 5f, 0.35f, false);
            }
           
        }

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.OnReloadedGun += InkGoop;
            player.PostProcessProjectile += this.PostProcessProjectile;
        }

        public override DebrisObject Drop(PlayerController player)
        {
            player.OnReloadedGun -= InkGoop;
            player.PostProcessProjectile -= this.PostProcessProjectile;
            return base.Drop(player);

        }
        protected override void OnDestroy()
        {
            Owner.OnReloadedGun -= InkGoop;
            Owner.PostProcessProjectile -= this.PostProcessProjectile;
            base.OnDestroy();
        }
    }
}

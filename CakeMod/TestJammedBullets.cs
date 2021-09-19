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
    class TestJammedBullets : PassiveItem
    {

        public static void Init()
        {
            string itemName = "Test Jammed Bullets";
            string resourceName = "CakeMod/Resources/RoundKing";

            GameObject obj = new GameObject(itemName);
            var item = obj.AddComponent<TestJammedBullets>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Bullet = Round?";
            string longDesc = "The king of roundness blesses you.\n\n" +
                "As you are absolved of your sins, bullets resist hitting you.\n" +
                "Some call him Cround.";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cak");

           



            item.quality = PickupObject.ItemQuality.EXCLUDED;

        }

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.PostProcessProjectile += this.roundmugthing;
        }

        public override DebrisObject Drop(PlayerController player)
        {
            player.PostProcessProjectile -= this.roundmugthing;
            return base.Drop(player);

        }
        private void roundmugthing(Projectile obj, float effectChanceScalar)
        {
            obj.BlackPhantomDamageMultiplier *= 2f;
        }
    }
}

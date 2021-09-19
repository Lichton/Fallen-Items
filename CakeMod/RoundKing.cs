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
    class CircularKing : PassiveItem
    {

        public static void Init()
        {
            string itemName = "Circular King";
            string resourceName = "CakeMod/Resources/RoundKing";

            GameObject obj = new GameObject(itemName);
            var item = obj.AddComponent<CircularKing>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Bullet = Round?";
            string longDesc = "The king of roundness blesses you.\n\n" +
                "As you are absolved of your sins, bullets resist hitting you.\n" +
                "Some call him Cround.";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cak");

            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.EnemyProjectileSpeedMultiplier, 0.75f, StatModifier.ModifyMethod.MULTIPLICATIVE);
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Curse, -2);
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.DamageToBosses, 1.5f, StatModifier.ModifyMethod.ADDITIVE);
         


            item.quality = PickupObject.ItemQuality.EXCLUDED;

        }

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
        }

        public override DebrisObject Drop(PlayerController player)
        {
            return base.Drop(player);
        }
    }
}

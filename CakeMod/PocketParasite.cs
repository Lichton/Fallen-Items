using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;

namespace CakeMod
{
    class PocketParasite : PassiveItem
    {

        public static void Init()
        {
            string itemName = "PocketParasite";
            string resourceName = "CakeMod/Resources/PocketParasite";

            GameObject obj = new GameObject(itemName);
            var item = obj.AddComponent<PocketParasite>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Carrion";
            string longDesc = "The king of roundness blesses you.\n\n" +
                "As you are absolved of your sins, bullets resist hitting you.\n" +
                "Some call him Cround.";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cak");

            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.EnemyProjectileSpeedMultiplier, 0.75f, StatModifier.ModifyMethod.MULTIPLICATIVE);
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Curse, -2);

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

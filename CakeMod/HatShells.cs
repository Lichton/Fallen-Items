using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;

namespace CakeMod
{
    class HatShells : PassiveItem
    {

        public static void Init()
        {
            string itemName = "Hat Shells";
            string resourceName = "CakeMod/Resources/HatShells";

            GameObject obj = new GameObject(itemName);
            var item = obj.AddComponent<HatShells>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Mad Hatter";
            string longDesc = "Hats are cool.\n\n" +
                "Unfortunatly, they also soften the impact of bullets.\n" +
                "This bullet has joined the wild craze of hat collection.";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cak");

            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Coolness, 5f, StatModifier.ModifyMethod.ADDITIVE);
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Damage, -0.10f, StatModifier.ModifyMethod.ADDITIVE);

            item.quality = PickupObject.ItemQuality.D;

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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;

namespace CakeMod
{
    class CoolAid : PassiveItem
    {

        public static void Init()
        {
            string itemName = "Cool Aid";
            string resourceName = "CakeMod/Resources/CoolAid";

            GameObject obj = new GameObject(itemName);
            var item = obj.AddComponent<CoolAid>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "OHHHH YEAAAHHHH!";
            string longDesc = "With grape powder...\n\n" +
                "Comes grape responsibility.";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cak");

            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.MovementSpeed, 0.50f, StatModifier.ModifyMethod.MULTIPLICATIVE);
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

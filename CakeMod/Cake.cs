using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;

namespace CakeMod
{
    class Cake : PassiveItem
    {

        public static void Init()
        {
            string itemName = "Cake";
            string resourceName = "CakeMod/Resources/Cake";

            GameObject obj = new GameObject(itemName);
            var item = obj.AddComponent<Cake>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Suprisingly Healthy";
            string longDesc = "Imbues the consumer with vitality.\n\n" +
                "Was created by a unknowable entity simply known as Cultthulu.\n" +
                "Screams of the jammed beg you not to eat it.";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cak");

            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Health, 2f, StatModifier.ModifyMethod.ADDITIVE);
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Curse, 1);

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

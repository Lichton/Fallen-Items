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
    class RatRound : BasicStatPickup
    {

        public static void Init()
        {
            string itemName = "Resourceful Round";
            string resourceName = "CakeMod/Resources/RatRound";

            GameObject obj = new GameObject(itemName);
            var item = obj.AddComponent<RatRound>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Ratster Round";
            string longDesc = "A reward for worthy gungeoneers who triumph over the Rat's Lair without being hurt.\n\n" +
                "There appears to be leftover cheese stuck to it, and scratches all over.";
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cak");
            
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Health, 1f, StatModifier.ModifyMethod.ADDITIVE);
            item.PlaceItemInAmmonomiconAfterItemById(467);
            item.quality = PickupObject.ItemQuality.SPECIAL;
            item.IsMasteryToken = true;
            CakeIDs.MasterRat = item.PickupObjectId;
            

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

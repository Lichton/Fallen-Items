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
    class PlatinumJunk : PassiveItem
    {

        public static void Init()
        {
            string itemName = "Platinum Junk";
            string resourceName = "CakeMod/Resources/PlatinumJunk";

            GameObject obj = new GameObject(itemName);
            var item = obj.AddComponent<PlatinumJunk>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "One Man's Treasure";
            string longDesc = "Junk that has been turned into pure platinum via the process of rewardification. \n\n" +
                "Pure money essence radiates off of it in waves. Junk fears you, and hides from you.";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cak");
            
            item.quality = PickupObject.ItemQuality.EXCLUDED;
            CakeIDs.PlatJunk = item.PickupObjectId;
        }

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            if(GiveMun == true)
            {
                Owner.carriedConsumables.Currency = Owner.carriedConsumables.Currency + 1000;
                GiveMun = false;
            }
           
        }

        public override DebrisObject Drop(PlayerController player)
        {
            return base.Drop(player);
        }

        public bool GiveMun = true;
    }
}

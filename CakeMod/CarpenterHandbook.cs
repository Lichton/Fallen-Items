using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;
using Gungeon;
using SaveAPI;
using Dungeonator;
using MonoMod.RuntimeDetour;
using System.Reflection;

namespace CakeMod
{
    class CarpenterHandbook : PassiveItem
    {
        public static Hook chestPreOpenHook;
        public static void Init()
        {
            string itemName = "Carpenter's Handbook";
            string resourceName = "CakeMod/Resources/CarpenterHandbook";

            GameObject obj = new GameObject(itemName);
            var item = obj.AddComponent<CarpenterHandbook>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Handy Dandy";
            string longDesc = "An informational pamphlet  telling the reader about masonry and carpentry.";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cak");


            CakeIDs.CH = item.PickupObjectId;
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

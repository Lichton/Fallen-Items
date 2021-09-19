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
    class ClownShoes : PassiveItem
    {

        public static void Init()
        {
            string itemName = "Clown Shoes";
            string resourceName = "CakeMod/Resources/RoundKing";

            GameObject obj = new GameObject(itemName);
            var item = obj.AddComponent<ClownShoes>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Canned Laughter";
            string longDesc = "Shoes imbued with magical humour.\n\n" +
                "There seems to be a thin slip of ice below the shoes.";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cak");

            



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

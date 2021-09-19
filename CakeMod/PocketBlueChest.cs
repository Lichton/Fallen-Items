using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;
using Dungeonator;

namespace CakeMod
{
    class PocketBlueChest : PlayerItem
    {
        //Call this method from the Start() method of your ETGModule extension class
        public static void Init()
        {
            //The name of the item
            string itemName = "Pocket Blue Chest";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it.
            string resourceName = "CakeMod/Resources/PocketBlueChest";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a ActiveItem component to the object
            var item = obj.AddComponent<PocketBlueChest>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "C Tier";
            string longDesc = "A miniturized version of the classic Blue Chest, it dispenses a clone of itself on use.";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //"kts" here is the item pool. In the console you'd type kts:sweating_bullets
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cak");

            //Set the cooldown type and duration of the cooldown
            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.Damage, 1111f);


            //Set some other fields
            item.consumable = true;
            item.quality = ItemQuality.SPECIAL;
            CakeIDs.PocketBlueChest = item.PickupObjectId;
        }

        //Add the item's functionality down here! I stole most of this from the Stuffed Star active item code!
        float duration = 10f;
        protected override void DoEffect(PlayerController user)
        {

            RoomHandler room = base.LastOwner.CurrentRoom;
            IntVector2 randomVisibleClearSpot5 = base.LastOwner.CurrentRoom.GetRandomVisibleClearSpot(1, 1);
            Chest rainbow_Chest = GameManager.Instance.RewardManager.C_Chest;
            rainbow_Chest.IsLocked = true;
            Chest.Spawn(rainbow_Chest, randomVisibleClearSpot5);
        }

        protected override void OnPreDrop(PlayerController user)
        {

        }

        //Disable or enable the active whenever you need!
        public override bool CanBeUsed(PlayerController user)
        {
            return base.CanBeUsed(user);
        }
    }
}


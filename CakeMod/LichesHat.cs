using System;
using System.Collections;
using Gungeon;
using MonoMod;
using UnityEngine;
using ItemAPI;
using BasicGun;
using System.Collections.Generic;
using Dungeonator;
using System.Reflection;
using MonoMod.RuntimeDetour;

namespace CakeMod
{
    class LichHat2 : PlayerItem
    {
        //Call this method from the Start() method of your ETGModule extension class
        public static void Init()
        {
            //The name of the item
            string itemName = "Lich Hat";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it.
            string resourceName = "CakeMod/Resources/LichHat";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a ActiveItem component to the object
            var item = obj.AddComponent<LichHat2>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "Temp Hire";
            string longDesc = "A hat carrying a fragment of the Lich's power. Temporarily empowers all guns.";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //"kts" here is the item pool. In the console you'd type kts:sweating_bullets
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cak");

            //Set the cooldown type and duration of the cooldown
            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.Damage, 750);


            //Set some other fields
            item.consumable = false;
            item.quality = ItemQuality.B;
        }

        //Add the item's functionality down here! I stole most of this from the Stuffed Star active item code!
        float duration = 10f;
        protected override void DoEffect(PlayerController user)
        {
            LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(815).gameObject, user);
            user.OnEnteredCombat += this.RoomCleared;
        }

        private void RoomCleared()
        {
            LastOwner.RemovePassiveItem(815);
            LastOwner.OnEnteredCombat -= this.RoomCleared;
        }

        protected override void OnPreDrop(PlayerController user)
        {

        }

        
        //Disable or enable the active whenever you need!
        public override bool CanBeUsed(PlayerController user)
        {
            return LastOwner.IsInCombat;
        }
    }
}


using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;

namespace CakeMod
{
    class Lime : PlayerItem
    {
        //Call this method from the Start() method of your ETGModule extension class
        public static void Init()
        {
            //The name of the item
            string itemName = "Lime";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it.
            string resourceName = "CakeMod/Resources/Lime";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a ActiveItem component to the object
            var item = obj.AddComponent<Lime>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "Wait, What?";
            string longDesc = "An orange that has been spraypainted green.\n\n" +
                "The paint on it appears to be leaking off, dropping onto the ground. It burns to the touch.";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //"kts" here is the item pool. In the console you'd type kts:sweating_bullets
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cak");
            
            //Set the cooldown type and duration of the cooldown
            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.Damage, 500);


            //Set some other fields
            item.consumable = false;
            item.quality = ItemQuality.C;
        }

        //Add the item's functionality down here! I stole most of this from the Stuffed Star active item code!
        float duration = 10f;

        
        protected override void DoEffect(PlayerController user)
        {
            DeadlyDeadlyGoopManager goopManagerForGoopType = DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(HelpfulLibrary.LimeGoop);
            goopManagerForGoopType.TimedAddGoopCircle(base.LastOwner.sprite.WorldCenter, 5f, 0.35f, false);
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

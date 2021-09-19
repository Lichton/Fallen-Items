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
using Brave.BulletScript;
using AnimationType = ItemAPI.EnemyBuilder.AnimationType;
using System.Linq;
using CakeMod;
using static CakeMod.DrawnKin;

namespace CakeMod
{
    class Goblet : PlayerItem
    {
        //Call this method from the Start() method of your ETGModule extension class
        public static void Init()
        {
            //The name of the item
            string itemName = "Golden Goblet";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it.
            string resourceName = "CakeMod/Resources/Goblet";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a ActiveItem component to the object
            var item = obj.AddComponent<Goblet>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "False King";
            string longDesc = "A fancy goblet, adorned with green gems.";

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


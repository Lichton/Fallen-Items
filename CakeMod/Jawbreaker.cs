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
    class Jawbreaker : PlayerItem
    {
        //Call this method from the Start() method of your ETGModule extension class
        public static void Init()
        {
            //The name of the item
            string itemName = "Jawbreaker";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it.
            string resourceName = "CakeMod/Resources/jawbreaker";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a ActiveItem component to the object
            var item = obj.AddComponent<Jawbreaker>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "Hard Candy";
            string longDesc = "A strange, opaque white ball studded with gemstones. It would not be wise to attempt consumption.";
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Health, 1f, StatModifier.ModifyMethod.ADDITIVE);

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //"kts" here is the item pool. In the console you'd type kts:sweating_bullets
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cak");

            //Set the cooldown type and duration of the cooldown
            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.Timed, 3);


            //Set some other fields
            item.consumable = false;
            item.quality = ItemQuality.EXCLUDED;
            item.CanBeDropped = false;
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
        public int x = 1;
    }
}


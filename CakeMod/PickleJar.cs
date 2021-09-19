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
    class PickleJar : PlayerItem
    {
        //Call this method from the Start() method of your ETGModule extension class
        public static void Init()
        {
            //The name of the item
            string itemName = "Pickle Jar";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it.
            string resourceName = "CakeMod/Resources/pickle_jar";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a ActiveItem component to the object
            var item = obj.AddComponent<PickleJar>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "Funniest Thing Ever Seen";
            string longDesc = "A jar of poisonous plants remarkably reminicent of pickles. Sour, but juicy.";
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Health, 1f, StatModifier.ModifyMethod.ADDITIVE);

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //"kts" here is the item pool. In the console you'd type kts:sweating_bullets
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cak");

            //Set the cooldown type and duration of the cooldown
            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.Damage, 400);


            //Set some other fields
            item.consumable = false;
            item.quality = ItemQuality.EXCLUDED;
        }

        //Add the item's functionality down here! I stole most of this from the Stuffed Star active item code!
        float duration = 10f;
        protected override void DoEffect(PlayerController user)
        {
            DeadlyDeadlyGoopManager goopManagerForGoopType = DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(EasyGoopDefinitions.PoisonDef);
            goopManagerForGoopType.TimedAddGoopCircle(this.LastOwner.specRigidbody.UnitCenter, 3, 3f);
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


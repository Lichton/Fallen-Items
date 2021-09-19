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
    class LifeLemon : PlayerItem
    {
        //Call this method from the Start() method of your ETGModule extension class
        public static void Init()
        {
            //The name of the item
            string itemName = "Life's Lemon";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it.
            string resourceName = "CakeMod/Resources/Lemon";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a ActiveItem component to the object
            var item = obj.AddComponent<LifeLemon>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "Lemongrabbed";
            string longDesc = "A lemon mentioned in many a turn of phrase, it's as they say : When life gives you lemons, kill someone with them. Has a 10% chance to harm the user, and a 90% chance to instant kill a non boss enemy.";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //"kts" here is the item pool. In the console you'd type kts:sweating_bullets
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cak");

            //Set the cooldown type and duration of the cooldown
            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.Timed, 3);


            //Set some other fields
            item.consumable = false;
            item.quality = ItemQuality.EXCLUDED;
        }

        //Add the item's functionality down here! I stole most of this from the Stuffed Star active item code!
        float duration = 10f;
        protected override void DoEffect(PlayerController user)
        {
            int bighead = UnityEngine.Random.Range(1, 11);
            if (bighead <= 9)
            {
                if (randomActiveEnemy != randomActiveEnemy.healthHaver.IsBoss)
                {


                    RoomHandler absoluteRoom = base.transform.position.GetAbsoluteRoom();
                    randomActiveEnemy = absoluteRoom.GetRandomActiveEnemy(false);
                    LootEngine.DoDefaultPurplePoof(randomActiveEnemy.CenterPosition, true);
                    randomActiveEnemy.EraseFromExistenceWithRewards();
                }
            }
            if(bighead == 10)
            {
                LastOwner.healthHaver.ApplyDamage(1, Vector2.zero, "Life's Lemon", CoreDamageTypes.None, DamageCategory.Normal, true, null, false);
            }
        }

   
        protected override void OnPreDrop(PlayerController user)
        {

        }

        //Disable or enable the active whenever you need!
        public override bool CanBeUsed(PlayerController user)
        {
            return LastOwner.IsInCombat;
        }

        public AIActor randomActiveEnemy;
    }
}


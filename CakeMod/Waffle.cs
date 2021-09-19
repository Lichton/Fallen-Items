using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;
using SaveAPI;

namespace CakeMod
{
    class Waffle : PlayerItem
    {
        //Call this method from the Start() method of your ETGModule extension class
        public static void Init()
        {
            //The name of the item
            string itemName = "Reloading Waffle";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it.
            string resourceName = "CakeMod/Resources/Waffle";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a ActiveItem component to the object
            var item = obj.AddComponent<Waffle>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "Not Red At All";
            string longDesc = "Is suprisingly tasty for a waffle. \n\nAbsorbs the souls of the dead to regenerate.";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //"kts" here is the item pool. In the console you'd type kts:sweating_bullets
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cak");

            //Set the cooldown type and duration of the cooldown
            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.Damage, 1000);


            //Set some other fields
            item.consumable = false;
            item.quality = ItemQuality.EXCLUDED;
            CakeIDs.Waffle = item.PickupObjectId;
            item.SetupUnlockOnCustomFlag(CustomDungeonFlags.EXAMPLE_BLUEPRINTMETA_3, true);
        }

        //Add the item's functionality down here! I stole most of this from the Stuffed Star active item code!
        float duration = 10f;
        protected override void DoEffect(PlayerController user)
        {
            base.LastOwner.PlayEffectOnActor(ResourceCache.Acquire("Global VFX/vfx_healing_sparkles_001") as GameObject, Vector3.zero, true, false, false);
            user.healthHaver.ApplyHealing(1);
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

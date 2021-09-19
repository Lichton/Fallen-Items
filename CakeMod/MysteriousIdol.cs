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
    class MysteriousIdol : PlayerItem
    {
        //Call this method from the Start() method of your ETGModule extension class
        public static void Init()
        {
            //The name of the item
            string itemName = "Mysterious Idol";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it.
            string resourceName = "CakeMod/Resources/MysteriousIdol";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a ActiveItem component to the object
            var item = obj.AddComponent<MysteriousIdol>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "Blessing of the Gungeon";
            string longDesc = "This figurine of the Gungeon's ruler grants 3 items of different types, and then disappears.";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //"kts" here is the item pool. In the console you'd type kts:sweating_bullets
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cak");

            //Set the cooldown type and duration of the cooldown
            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.Damage, 500);


            //Set some other fields
            item.consumable = true;
            item.quality = ItemQuality.EXCLUDED;
        }

        //Add the item's functionality down here! I stole most of this from the Stuffed Star active item code!
        float duration = 10f;
        protected override void DoEffect(PlayerController user)
        {
            PickupObject.ItemQuality itemQuality = ItemQuality.D;
            PickupObject itemOfTypeAndQuality = LootEngine.GetItemOfTypeAndQuality<Gun>(itemQuality, GameManager.Instance.RewardManager.GunsLootTable, false);
            LootEngine.SpawnItem(itemOfTypeAndQuality.gameObject, LastOwner.specRigidbody.UnitCenter, Vector2.up, 1f, false, true, false);
            PickupObject stuffdone = LootEngine.GetItemOfTypeAndQuality<PassiveItem>(itemQuality, GameManager.Instance.RewardManager.ItemsLootTable, false);
            LootEngine.SpawnItem(stuffdone.gameObject, LastOwner.specRigidbody.UnitCenter, Vector2.up, 1f, false, true, false);
            PickupObject dothingitdo = LootEngine.GetItemOfTypeAndQuality<PlayerItem>(itemQuality, GameManager.Instance.RewardManager.ItemsLootTable, false);
            LootEngine.SpawnItem(dothingitdo.gameObject, LastOwner.specRigidbody.UnitCenter, Vector2.up, 1f, false, true, false);

            
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


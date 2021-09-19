using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;

namespace CakeMod
{
    class TheFool : PlayerItem
    {
        //Call this method from the Start() method of your ETGModule extension class
        public static void Init()
        {
            //The name of the item
            string itemName = "The Fool";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it.
            string resourceName = "CakeMod/Resources/TheFool";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a ActiveItem component to the object
            var item = obj.AddComponent<TheFool>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "Jest A Just";
            string longDesc = "Grants the holder an increase to foolishness. \n\nIt offers a powerful prize on consumption.";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //"kts" here is the item pool. In the console you'd type kts:sweating_bullets
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cak");

            //Set the cooldown type and duration of the cooldown
            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.Damage, 500);

            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.MovementSpeed, 1f, StatModifier.ModifyMethod.ADDITIVE);
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Accuracy, -0.5f, StatModifier.ModifyMethod.ADDITIVE);

            //Set some other fields
            item.consumable = true;
            item.quality = ItemQuality.EXCLUDED;
        }

        //Add the item's functionality down here! I stole most of this from the Stuffed Star active item code!
        float duration = 10f;
        protected override void DoEffect(PlayerController user)
        {
            LootEngine.SpawnItem(PickupObjectDatabase.GetById(276).gameObject, user.specRigidbody.UnitCenter, Vector2.left, 1f, false, true, false);
            LootEngine.SpawnItem(PickupObjectDatabase.GetById(276).gameObject, user.specRigidbody.UnitCenter, Vector2.right, 1f, false, true, false);
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

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
    class Notebook : PlayerItem
    {
        //Call this method from the Start() method of your ETGModule extension class
        public static void Init()
        {
            //The name of the item
            string itemName = "Notebook";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it.
            string resourceName = "CakeMod/Resources/Notebook";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a ActiveItem component to the object
            var item = obj.AddComponent<Notebook>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "Pen Pals";
            string longDesc = "A doodle-filled notebook. \n\nIt hums with faint power, animating anything you draw in it.";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //"kts" here is the item pool. In the console you'd type kts:sweating_bullets
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cak");

            //Set the cooldown type and duration of the cooldown
            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.Damage, 500);


            //Set some other fields
            item.consumable = false;
            item.quality = ItemQuality.D;
        }

        //Add the item's functionality down here! I stole most of this from the Stuffed Star active item code!
        float duration = 10f;
        protected override void DoEffect(PlayerController user)
        {
            string guid;
            guid = "206405acad4d4c33aac6717d184dc8d4";
            int bighead = UnityEngine.Random.Range(1, 4);
            if (bighead == 1)
            {
                guid = "drawnkin";
            }
            if (bighead == 2)
            {
                guid = "doodle_book";
            }
            if (bighead == 3)
            {
                guid = "ink_book";
            }


            PlayerController owner = base.LastOwner;
            AIActor orLoadByGuid = EnemyDatabase.GetOrLoadByGuid(guid);
            IntVector2? intVector = new IntVector2?(base.LastOwner.CurrentRoom.GetRandomVisibleClearSpot(2, 2));
            AIActor aiactor = AIActor.Spawn(orLoadByGuid.aiActor, intVector.Value, GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(intVector.Value), true, AIActor.AwakenAnimationType.Default, true);
            aiactor.CanTargetEnemies = true;
            aiactor.CanTargetPlayers = false;
            PhysicsEngine.Instance.RegisterOverlappingGhostCollisionExceptions(aiactor.specRigidbody, null, false);
            aiactor.gameObject.AddComponent<KillOnRoomClear>();
            aiactor.CanDropCurrency = false;
            aiactor.HitByEnemyBullets = true;
            aiactor.CollisionDamage = 0f;
            aiactor.IsHarmlessEnemy = true;
            aiactor.IgnoreForRoomClear = true;
            aiactor.HandleReinforcementFallIntoRoom(0f);
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

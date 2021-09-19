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
    class ConcealedTreasure : PlayerItem
    {
        //Call this method from the Start() method of your ETGModule extension class
        public static void Init()
        {
            //The name of the item
            string itemName = "Concealed Treasure";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it.
            string resourceName = "CakeMod/Resources/MagicHat";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a ActiveItem component to the object
            var item = obj.AddComponent<ConcealedTreasure>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "Rabbit Not Included";
            string longDesc = "This hat radiates energy that transforms anything nearby the user into hat wielding wackos. Conviently, it also transforms rocks into treasure.";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //"kts" here is the item pool. In the console you'd type kts:sweating_bullets
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cak");

            //Set the cooldown type and duration of the cooldown
            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.Damage, 1111f);


            //Set some other fields
            item.consumable = false;
            item.quality = ItemQuality.EXCLUDED;
        }

        //Add the item's functionality down here! I stole most of this from the Stuffed Star active item code!
        float duration = 10f;
        protected override void DoEffect(PlayerController user)
        {
            AIActor orLoadByGuid = EnemyDatabase.GetOrLoadByGuid("5861e5a077244905a8c25c2b7b4d6ebb");
            IntVector2? intVector = new IntVector2?(base.LastOwner.CurrentRoom.GetRandomVisibleClearSpot(2, 2));
            AIActor aiactor = AIActor.Spawn(orLoadByGuid.aiActor, intVector.Value, GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(intVector.Value), true, AIActor.AwakenAnimationType.Default, true);

            AIActor memehatt = EnemyDatabase.GetOrLoadByGuid("ddf12a4881eb43cfba04f36dd6377abb");
            IntVector2? meme = new IntVector2?(base.LastOwner.CurrentRoom.GetRandomVisibleClearSpot(2, 2));
            AIActor TOPPER = AIActor.Spawn(memehatt.aiActor, meme.Value, GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(meme.Value), true, AIActor.AwakenAnimationType.Default, true);

            AIActor ase = EnemyDatabase.GetOrLoadByGuid("86dfc13486ee4f559189de53cfb84107");
            IntVector2? mem = new IntVector2?(base.LastOwner.CurrentRoom.GetRandomVisibleClearSpot(2, 2));
            AIActor poof = AIActor.Spawn(ase.aiActor, mem.Value, GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(mem.Value), true, AIActor.AwakenAnimationType.Default, true);

            AIActor ae = EnemyDatabase.GetOrLoadByGuid("6f818f482a5c47fd8f38cce101f6566c");
            IntVector2? memee = new IntVector2?(base.LastOwner.CurrentRoom.GetRandomVisibleClearSpot(2, 2));
            AIActor dag = AIActor.Spawn(ae.aiActor, memee.Value, GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(memee.Value), true, AIActor.AwakenAnimationType.Default, true);

            AIActor aerr = EnemyDatabase.GetOrLoadByGuid("8bb5578fba374e8aae8e10b754e61d62");
            IntVector2? me = new IntVector2?(base.LastOwner.CurrentRoom.GetRandomVisibleClearSpot(2, 2));
            AIActor dager = AIActor.Spawn(aerr.aiActor, me.Value, GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(me.Value), true, AIActor.AwakenAnimationType.Default, true);

            AIActor aerddr = EnemyDatabase.GetOrLoadByGuid("df4e9fedb8764b5a876517431ca67b86");
            IntVector2? mec = new IntVector2?(base.LastOwner.CurrentRoom.GetRandomVisibleClearSpot(2, 2));
            AIActor dageree = AIActor.Spawn(aerddr.aiActor, mec.Value, GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(mec.Value), true, AIActor.AwakenAnimationType.Default, true);

            AIActor aerddccr = EnemyDatabase.GetOrLoadByGuid("39e6f47a16ab4c86bec4b12984aece4c");
            IntVector2? mecee = new IntVector2?(base.LastOwner.CurrentRoom.GetRandomVisibleClearSpot(2, 2));
            AIActor dagereecc = AIActor.Spawn(aerddccr.aiActor, mecee.Value, GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(mecee.Value), true, AIActor.AwakenAnimationType.Default, true);

            AIActor aerddccrf = EnemyDatabase.GetOrLoadByGuid("3cadf10c489b461f9fb8814abc1a09c1");
            IntVector2? meceerr = new IntVector2?(base.LastOwner.CurrentRoom.GetRandomVisibleClearSpot(2, 2));
            AIActor dagerqreccceecc = AIActor.Spawn(aerddccrf.aiActor, meceerr.Value, GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(meceerr.Value), true, AIActor.AwakenAnimationType.Default, true);

            AIActor aerddccre = EnemyDatabase.GetOrLoadByGuid("95ec774b5a75467a9ab05fa230c0c143");
            IntVector2? meceea = new IntVector2?(base.LastOwner.CurrentRoom.GetRandomVisibleClearSpot(2, 2));
            AIActor dagerqrqeecc = AIActor.Spawn(aerddccre.aiActor, meceea.Value, GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(meceea.Value), true, AIActor.AwakenAnimationType.Default, true);

            AIActor aerfdddccr = EnemyDatabase.GetOrLoadByGuid("47bdfec22e8e4568a619130a267eab5b");
            IntVector2? meceeffffffff = new IntVector2?(base.LastOwner.CurrentRoom.GetRandomVisibleClearSpot(2, 2));
            AIActor dagercceecc = AIActor.Spawn(aerfdddccr.aiActor, meceeffffffff.Value, GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(meceeffffffff.Value), true, AIActor.AwakenAnimationType.Default, true);

            RoomHandler room = base.LastOwner.CurrentRoom;
            IntVector2 randomVisibleClearSpot5 = base.LastOwner.CurrentRoom.GetRandomVisibleClearSpot(1, 1);
            Chest rainbow_Chest = GameManager.Instance.RewardManager.D_Chest;
            rainbow_Chest.IsLocked = false;
            Chest.Spawn(rainbow_Chest, randomVisibleClearSpot5);
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

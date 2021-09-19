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
    class GunslingKingRequest: PlayerItem
    {
        //Call this method from the Start() method of your ETGModule extension class
        public static void Init()
        {
            //The name of the item
            string itemName = "D-Lootbox";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it.
            string resourceName = "CakeMod/Resources/DLootbox";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a ActiveItem component to the object
            var item = obj.AddComponent<GunslingKingRequest>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "Challenge On A Budget";
            string longDesc = "Turns the next room into a challenge room, and gives you a prize.";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //"kts" here is the item pool. In the console you'd type kts:sweating_bullets
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cak");

            //Set the cooldown type and duration of the cooldown
            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.PerRoom, 5);

            

            //Set some other fields
            item.consumable = false;
            item.quality = ItemQuality.C;
            CakeIDs.DiceBox = item.PickupObjectId;
        }

        //Add the item's functionality down here! I stole most of this from the Stuffed Star active item code!
        protected override void DoEffect(PlayerController user)
        {
            if (ChallengeManager.Instance == false)
            {
                ChallengeManager.ChallengeModeType = ChallengeModeType.GunslingKingTemporary;
                challengeLevel += 3;
                RoomHandler absoluteRoom = base.transform.position.GetAbsoluteRoom();
                absoluteRoom.IsGunslingKingChallengeRoom = true;
                int rewards = UnityEngine.Random.Range(1, 9);
                if (rewards == 1)
                {
                    LootEngine.SpawnItem(Gungeon.Game.Items["junk"].gameObject, user.specRigidbody.UnitCenter, Vector2.down, 0.5f, false, true, false);
                }
                if (rewards == 2)
                {
                    LootEngine.SpawnItem(PickupObjectDatabase.GetById(68).gameObject, user.specRigidbody.UnitCenter, Vector2.left, 1f, false, true, false);
                }
                if (rewards == 3)
                {
                    LootEngine.SpawnItem(PickupObjectDatabase.GetById(70).gameObject, user.specRigidbody.UnitCenter, Vector2.left, 1f, false, true, false);
                }
                if (rewards == 4)
                {
                    LootEngine.SpawnItem(PickupObjectDatabase.GetById(297).gameObject, user.specRigidbody.UnitCenter, Vector2.left, 1f, false, true, false);
                }
                if (rewards == 5)
                {
                    LootEngine.SpawnItem(PickupObjectDatabase.GetById(120).gameObject, user.specRigidbody.UnitCenter, Vector2.left, 1f, false, true, false);
                }
                if (rewards == 6)
                {
                    LootEngine.SpawnItem(PickupObjectDatabase.GetById(73).gameObject, user.specRigidbody.UnitCenter, Vector2.left, 1f, false, true, false);
                }
                if (rewards == 7)
                {
                    LootEngine.SpawnItem(PickupObjectDatabase.GetById(224).gameObject, user.specRigidbody.UnitCenter, Vector2.left, 1f, false, true, false);
                }
                if (rewards == 8)
                {
                    LootEngine.SpawnItem(PickupObjectDatabase.GetById(137).gameObject, user.specRigidbody.UnitCenter, Vector2.left, 1f, false, true, false);
                }
                if(ChallengeManager.Instance == false)
                {
                    this.Notify("Consolation Prize", "Good Luck.");
                    int rewards2 = UnityEngine.Random.Range(1, 4);
                    if (rewards2 == 1)
                    {
                        LootEngine.SpawnItem(PickupObjectDatabase.GetById(70).gameObject, user.specRigidbody.UnitCenter, Vector2.left, 1f, false, true, false);
                    }
                    if (rewards2 == 2)
                    {
                        LootEngine.SpawnItem(PickupObjectDatabase.GetById(68).gameObject, user.specRigidbody.UnitCenter, Vector2.left, 1f, false, true, false);
                    }
                    if (rewards2 == 3)
                    {
                        LootEngine.SpawnItem(PickupObjectDatabase.GetById(70).gameObject, user.specRigidbody.UnitCenter, Vector2.left, 1f, false, true, false);
                    }
                }
            }
        }
        protected override void OnPreDrop(PlayerController user)
        {
            
        }
        private void Notify(string header, string text)
        {
            tk2dSpriteCollectionData encounterIconCollection = AmmonomiconController.Instance.EncounterIconCollection;
            int spriteIdByName = encounterIconCollection.GetSpriteIdByName("CakeMod/Resources/StrangePotion");
            GameUIRoot.Instance.notificationController.DoCustomNotification(header, text, null, spriteIdByName, UINotificationController.NotificationColor.PURPLE, false, true);
        }
        //Disable or enable the active whenever you need!
        public override bool CanBeUsed(PlayerController user)
        {
            return base.CanBeUsed(user);
        }
        public ChallengeModeType ChallengeMode;
        private static int challengeLevel = 0;
    }

}
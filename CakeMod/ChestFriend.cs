using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;
using Dungeonator;

namespace CakeMod
{
    class ChestFriend: PassiveItem
    {

        public static void Init()
        {
            string itemName = "Chest Friend";
            string resourceName = "CakeMod/Resources/ChestFriend";

            GameObject obj = new GameObject(itemName);
            var item = obj.AddComponent<ChestFriend>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Baby Good Chest";
            string longDesc = "A mimic that doesn't seem to understand how to mimic chests accurately.\n\n" +
                "It seems to try to bribe you with spare chests.";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cak");

            item.quality = PickupObject.ItemQuality.B;
            CakeIDs.ChestFriend = item.PickupObjectId;
        }



        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.OnRoomClearEvent += this.RollForlootz;
        }

        private void RollForlootz(PlayerController obj)
        {
            int bighead = UnityEngine.Random.Range(1, 101);
            if (bighead == 2)
            {
                LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(CakeIDs.PocketRedChest).gameObject, Owner);
            }
            if (bighead == 3)
            {
                LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(CakeIDs.PocketGreenChest).gameObject, Owner);
            }
            if (bighead == 4)
            {
                LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(CakeIDs.PocketBrownChest).gameObject, Owner);
            }
            if (bighead == 5)
            {
                LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(CakeIDs.PocketBlueChest).gameObject, Owner);
            }
            if (bighead == 1)
            {
                LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(CakeIDs.PocketBlackChest).gameObject, Owner);
            }
        }
        public override DebrisObject Drop(PlayerController player)
        {
            player.OnRoomClearEvent -= this.RollForlootz;
            return base.Drop(player);
        }
    }
}
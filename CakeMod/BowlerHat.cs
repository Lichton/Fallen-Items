using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;
using Dungeonator;
using SaveAPI;

namespace CakeMod
{
    class BowlerHat : PassiveItem
    {

        public static void Init()
        {
            string itemName = "Bowler Hat";
            string resourceName = "CakeMod/Resources/BowlerHat";

            GameObject obj = new GameObject(itemName);
            var item = obj.AddComponent<BowlerHat>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Bowlerlike, But Not A Bowler";
            string longDesc = "A strange top hat that looks oddly like Bowler.\n\n" +
                "Grants a small chance to find Bowler's favoured chests.";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cak");

            item.quality = PickupObject.ItemQuality.A;
            item.SetupUnlockOnFlag(GungeonFlags.BOWLER_ACTIVE_IN_FOYER, true);
        }



        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.OnRoomClearEvent += this.RollForRainbow;
        }

        private void RollForRainbow(PlayerController obj)
        {
            if (UnityEngine.Random.value <= 0.005f)
            {
                RoomHandler room = base.Owner.CurrentRoom;
                IntVector2 randomVisibleClearSpot5 = base.Owner.CurrentRoom.GetRandomVisibleClearSpot(1, 1);
                Chest rainbow_Chest = GameManager.Instance.RewardManager.Rainbow_Chest;
                rainbow_Chest.IsLocked = false;
                Chest.Spawn(rainbow_Chest, randomVisibleClearSpot5);
            }
        }

        public override DebrisObject Drop(PlayerController player)
        {
            player.OnRoomClearEvent -= this.RollForRainbow;
            return base.Drop(player);
        }
    }
}
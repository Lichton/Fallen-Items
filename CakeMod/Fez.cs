using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;
using Gungeon;
using SaveAPI;
using Dungeonator;


namespace CakeMod
{
    class Fez : PassiveItem
    {

        public static void Init()
        {
            string itemName = "Fez";
            string resourceName = "CakeMod/Resources/fez";

            GameObject obj = new GameObject(itemName);
            var item = obj.AddComponent<Fez>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Coolhardy";
            string longDesc = "Enemies nearby start laughing at your atrocious fashion statement. " +
                "It looked cool at first, but now everybody is staring.";
                

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cak");

           

            item.quality = PickupObject.ItemQuality.EXCLUDED;

        }

        private void StunRoom()
        {
            RoomHandler absoluteRoom = base.transform.position.GetAbsoluteRoom();
            List<AIActor> activeEnemies = absoluteRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
            foreach (AIActor aiactor in activeEnemies)
            {
                if (aiactor.IsNormalEnemy)
                {
                    aiactor.behaviorSpeculator.Stun(3f, true);
                }
            }
        }

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.OnEnteredCombat += this.StunRoom;
        }

        public override DebrisObject Drop(PlayerController player)
        {
            player.OnEnteredCombat -= this.StunRoom;
            return base.Drop(player);
        }
    }
}

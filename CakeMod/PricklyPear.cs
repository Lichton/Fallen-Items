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
    class PricklyPear : PassiveItem
    {

        public static void Init()
        {
            string itemName = "Prickly Pear";
            string resourceName = "CakeMod/Resources/PricklyPear";

            GameObject obj = new GameObject(itemName);
            var item = obj.AddComponent<PricklyPear>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Agriculture";
            string longDesc = "A fruit from a cactus found deep within a lost gungeon floor.\n\n" +
                "The deserts may be sandy and desolate, but their fruit is without doubt delectable.\n" +
                "Eating this makes you feel all tingly inside, probably because of all the needles.";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cak");

            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Health, 1f, StatModifier.ModifyMethod.ADDITIVE);
           

            item.quality = PickupObject.ItemQuality.B;

        }
        private void OnDamaged(PlayerController player)
        {
            RoomHandler currentRoom = Owner.CurrentRoom;
            if (currentRoom.HasActiveEnemies(RoomHandler.ActiveEnemyType.All))
            {
                foreach (AIActor aiactor in currentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All))
                {
                    aiactor.healthHaver.ApplyDamage(20, Vector2.zero, "Needle", CoreDamageTypes.None, DamageCategory.Normal, true, null, false);
                }
            }
        }
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            base.Owner.OnReceivedDamage += this.OnDamaged;

        }

        public override DebrisObject Drop(PlayerController player)
        {
            base.Owner.OnReceivedDamage -= this.OnDamaged;
            return base.Drop(player);
            

        }
    }
}

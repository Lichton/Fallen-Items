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
    class Butter : PassiveItem
    {

        public static void Init()
        {
            string itemName = "Butter";
            string resourceName = "CakeMod/Resources/Butter";

            GameObject obj = new GameObject(itemName);
            var item = obj.AddComponent<Butter>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Butter Fingers";
            string longDesc = "Slippery butter. In reality, it's simply a slippery yellow brick, but that's neither here nor there.";
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cak");

            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.MovementSpeed, 1.25f, StatModifier.ModifyMethod.MULTIPLICATIVE);
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Health, 1f, StatModifier.ModifyMethod.ADDITIVE);

            item.quality = PickupObject.ItemQuality.EXCLUDED;

        }
        protected override void Update()
        {
          
        }

       
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.OnUsedPlayerItem += DoTheDrop;
        }

        private void DoTheDrop(PlayerController player, PlayerItem arg2)
        {
            if (player.HasActiveItem(arg2.PickupObjectId))
            {
                if (arg2.consumable == false)
                {
                    player.DropActiveItem(arg2, 1f);
                    player.RemoveActiveItem(arg2.PickupObjectId);
                }
            }
        }

        public override DebrisObject Drop(PlayerController player)
        {
            player.OnUsedPlayerItem -= DoTheDrop;
            return base.Drop(player);
          
        }
        public bool Gamer = true;
    }
}

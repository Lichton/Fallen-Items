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
    class MiniShroom : PassiveItem
    {

        public static void Init()
        {
            string itemName = "Mini Shroom";
            string resourceName = "CakeMod/Resources/MiniShroom";

            GameObject obj = new GameObject(itemName);
            var item = obj.AddComponent<MiniShroom>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "One Makes You Smaller";
            string longDesc = "A poisonous mushroom that weakens the bones and muscles of gungeoneers, shrinking them to a mockery of their former size.\n\n" +
                "Why would you eat this?";
            item.CanBeDropped = false;
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cak");
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.MovementSpeed, 1.25f, StatModifier.ModifyMethod.MULTIPLICATIVE);
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Coolness, 2);



            item.quality = PickupObject.ItemQuality.C;

        }

        public float OrigX;
        public float OrigY = 0;

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            
            var X = player.sprite.scale.x;
            var Y = player.sprite.scale.y;

            if (OrigY == 0)
            {
                OrigX = X;
                OrigY = Y;
            }
            var deminishX = (player.sprite.scale.x * .85f);
            var deminishY = (player.sprite.scale.y * .7f);
            player.sprite.scale = new Vector3(deminishX, deminishY, player.sprite.scale.z);

            player.specRigidbody.UpdateCollidersOnScale = true;
            player.specRigidbody.UpdateColliderPositions();
        }

        public override DebrisObject Drop(PlayerController player)
        {
            var X = player.sprite.scale.x;
            var Y = player.sprite.scale.y;

            if (OrigY == 0)
            {
                OrigX = X;
                OrigY = Y;
            }
            var deminishX = (player.sprite.scale.x * 1.15f);
            var deminishY = (player.sprite.scale.y * 1.3f);
            player.sprite.scale = new Vector3(deminishX, deminishY, player.sprite.scale.z);

            player.specRigidbody.UpdateCollidersOnScale = true;
            player.specRigidbody.UpdateColliderPositions();
            return base.Drop(player);
        }
    }
}

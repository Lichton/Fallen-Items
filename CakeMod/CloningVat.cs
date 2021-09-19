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
    class CloningVat : PassiveItem
    {

        public static void Init()
        {
            string itemName = "Cloning Vat";
            string resourceName = "CakeMod/Resources/CloningVat";

            GameObject obj = new GameObject(itemName);
            var item = obj.AddComponent<CloningVat>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Bullet = Round?";
            string longDesc = "The king of roundness blesses you.\n\n" +
                "As you are absolved of your sins, bullets resist hitting you.\n" +
                "Some call him Cround.";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cak");

            item.quality = PickupObject.ItemQuality.EXCLUDED;

        }

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            AIActor gameObject = player.aiActor;
            IntVector2? intVector = new IntVector2?(player.CurrentRoom.GetRandomVisibleClearSpot(2, 2));
            AIActor aiactor = AIActor.Spawn(gameObject, intVector.Value, GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(intVector.Value), true, AIActor.AwakenAnimationType.Default, true);
        }

        public override DebrisObject Drop(PlayerController player)
        {
            return base.Drop(player);
        }
    }
}

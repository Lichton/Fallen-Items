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
    class TableTechHolographic : PassiveItem
    {

        public static void Init()
        {
            string itemName = "Table Tech Holographic";
            string resourceName = "CakeMod/Resources/TableTechHolo";

            GameObject obj = new GameObject(itemName);
            var item = obj.AddComponent<TableTechHolographic>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Wonders of Technology";
            string longDesc = "A futuristic table tech that seems to have been brought from the future by the gungeon's magic.\n\n" +
                "It flickers in and out of existence, making it hard to read the words on the page.";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cak");

           
            item.quality = PickupObject.ItemQuality.EXCLUDED;

        }

        public override void Pickup(PlayerController player)
        {
            player.OnTableFlipCompleted += this.BloodTable;
            base.Pickup(player);
        }

        private void BloodTable(FlippableCover obj)
        {
            
            obj.specRigidbody.CanBePushed = false;
            obj.specRigidbody.PrimaryPixelCollider.CollisionLayer = CollisionLayer.BulletBlocker;
            foreach(PixelCollider collider in obj.specRigidbody.GetPixelColliders())
            {
                collider.CollisionLayer = CollisionLayer.BulletBlocker;
            }
        }

        public override DebrisObject Drop(PlayerController player)
        {
            player.OnTableFlipped -= this.BloodTable;
            return base.Drop(player);
        }
    }
}

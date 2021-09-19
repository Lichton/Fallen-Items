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
    class Clumsy : PassiveItem
    {

        public static void Init()
        {
            string itemName = "Clumsy";
            string resourceName = "CakeMod/Resources/Keysing";

            GameObject obj = new GameObject(itemName);
            var item = obj.AddComponent<Clumsy>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Slight Mistep";
            string longDesc = "The king of roundness blesses you.\n\n" +
                "As you are absolved of your sins, bullets resist hitting you.\n" +
                "Some call him Cround.";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cak");

     

            item.quality = PickupObject.ItemQuality.EXCLUDED;

        }
        protected override void Update()
        {
            UnityEngine.Random.Range(1, 5);
            bool flag5 = UnityEngine.Random.value == 5;
            if(flag5)
            {
                Owner.ForceStartDodgeRoll();
            }

        }

        private void DodgingMoment()
        {
            Owner.ForceStartDodgeRoll();
        }

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
        }

        public override DebrisObject Drop(PlayerController player)
        {
            return base.Drop(player);
        }
    }
}
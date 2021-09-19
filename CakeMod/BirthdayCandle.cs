using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;

namespace CakeMod
{
    class BirthdayCandle : PassiveItem
    {

        public static void Init()
        {
            string itemName = "Birthday Candle";
            string resourceName = "CakeMod/Resources/BirthdayCandle";

            GameObject obj = new GameObject(itemName);
            var item = obj.AddComponent<BirthdayCandle>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Revelations";
            string longDesc = "Gives you whatever pickup you don't have currently.\n\n" +
                "Cannot be dropped.\n" +
                "This candle marks the birthday of the Keymaster, a lost gungeoneer.";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cak");

            item.CanBeDropped = false;

            item.quality = PickupObject.ItemQuality.D;

        }
       
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            bool didBirthday = true;
            if (Owner.healthHaver.Armor < 1)
            {
                if (didBirthday == true)
                {
                    Owner.healthHaver.Armor = Owner.healthHaver.Armor + 1;
                    didBirthday = false;
                }
            }
                    if (Owner.carriedConsumables.KeyBullets < 2)
                    {
                        if (didBirthday == true)
                        {
                            Owner.carriedConsumables.KeyBullets = Owner.carriedConsumables.KeyBullets + 1;
                            didBirthday = false;
                        }

                    }
            if (Owner.carriedConsumables.Currency < 15)
            {
                if (didBirthday == true)
                {
                    Owner.carriedConsumables.Currency = Owner.carriedConsumables.Currency + 15;
                    didBirthday = false;
                }

            }
            if (Owner.Blanks < 2)
            {
                if (didBirthday == true)
                {
                    Owner.Blanks = Owner.Blanks + 2;
                    didBirthday = false;
                }

            }
        }

        public override DebrisObject Drop(PlayerController player)
        {
            return base.Drop(player);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;
using Gungeon;
using SaveAPI;

namespace CakeMod
{
    class CultistHelm : PassiveItem
    {

        public static void Init()
        {
            string itemName = "Cultist Helm";
            string resourceName = "CakeMod/Resources/CultistHelm";

            GameObject obj = new GameObject(itemName);
            var item = obj.AddComponent<CultistHelm>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Nefarious";
            string longDesc = "A toy helmet made of cardboard and paint.\n\n" +
                "It seems to have been made by an 'apprentice blacksmith'. While shoddily made, it provides slight protection.\n" +
                "Comes with a sick grenade launcher!";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cak");
            item.CanBeDropped = false;

            item.quality = PickupObject.ItemQuality.B;
            CakeIDs.CultistHelm = item.PickupObjectId;
            item.SetupUnlockOnCustomFlag(CustomDungeonFlags.EXAMPLE_BLUEPRINTMETA_1, true);
        }

        public override void Pickup(PlayerController player)
        {
           
            base.Pickup(player);
            Owner.healthHaver.Armor = Owner.healthHaver.Armor + 3;
            LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(CakeIDs.Explodergun).gameObject, player);

        }

        public override DebrisObject Drop(PlayerController player)
        {
            return base.Drop(player);
        }

    
    }
}
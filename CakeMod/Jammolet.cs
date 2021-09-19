using Dungeonator;
using ItemAPI;
using UnityEngine;
using System.Collections;
using Gungeon;
using MonoMod;
using BasicGun;
using System.Collections.Generic;
using System;
using SaveAPI;
using System.Linq;
using System.Reflection;
using GungeonAPI;
using MonoMod.RuntimeDetour;


namespace CakeMod
{
    // Token: 0x0200001E RID: 30
    internal class Jammolet : SpecialBlankModificationItem
    {

        // Token: 0x060000C4 RID: 196 RVA: 0x0000AAD8 File Offset: 0x00008CD8
        public static void Init()
        {
            string itemName = "Jammolet";
            string resourceName = "CakeMod/Resources/Jammolet";

            GameObject obj = new GameObject(itemName);
            var item = obj.AddComponent<Jammolet>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Damned & Jammed";
            string longDesc = "A cursed ammolet forged deep in bullet hell, this ammolet has the power to weaken the Jammed.";
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cak");
            item.quality = PickupObject.ItemQuality.C;
            item.PlaceItemInAmmonomiconAfterItemById(344);
            item.BlankStunTime = 0f;
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.AdditionalBlanksPerFloor, 1f, StatModifier.ModifyMethod.ADDITIVE);
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Curse, 1);
            item.AddToSubShop(ItemBuilder.ShopType.Cursula, 1f);
          
            item.SetupUnlockOnCustomStat(CustomTrackedStats.JAMMOLET_CURSE, 20f, DungeonPrerequisite.PrerequisiteOperation.GREATER_THAN);
        }

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            
        }

        protected override void OnBlank(Vector2 centerPoint, PlayerController user)
        {
            
            foreach (AIActor aiactor in user.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All))
            {
                if (aiactor != null && aiactor.IsBlackPhantom)
                {
                
                    if (!aiactor.healthHaver.IsBoss)
                    {
                        aiactor.UnbecomeBlackPhantom();
                    }
                }
            }
        }




        public override DebrisObject Drop(PlayerController player)
        {
            return base.Drop(player);
        }

    }
}

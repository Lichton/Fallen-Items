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
    class BloomingHeart : PassiveItem
    {

        public static void Init()
        {
            string itemName = "Blooming Heart";
            string resourceName = "CakeMod/Resources/BloomingHeart";

            GameObject obj = new GameObject(itemName);
            var item = obj.AddComponent<BloomingHeart>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Melon-cholic";
            string longDesc = "A beating heart, rife with leaves and earthy bits. It seems to grow with your progress.";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cak");

            item.quality = PickupObject.ItemQuality.A;

        }

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            GameManager.Instance.OnNewLevelFullyLoaded += this.Grow;
        }

        public override DebrisObject Drop(PlayerController player)
        {
            GameManager.Instance.OnNewLevelFullyLoaded -= this.Grow;
            return base.Drop(player);
        }

        protected override void OnDestroy()
        {

            GameManager.Instance.OnNewLevelFullyLoaded -= this.Grow;
            base.OnDestroy();
        }
        private void Grow()
        {
            this.ApplyStat(Owner, PlayerStats.StatType.Health, 1f, StatModifier.ModifyMethod.ADDITIVE);
        }

        private void ApplyStat(PlayerController player, PlayerStats.StatType statType, float amountToApply, StatModifier.ModifyMethod modifyMethod)
        {
            player.stats.RecalculateStats(player, false, false);
            StatModifier item = new StatModifier
            {
                statToBoost = statType,
                amount = amountToApply,
                modifyType = modifyMethod
            };
            player.ownerlessStatModifiers.Add(item);
            player.stats.RecalculateStats(player, false, false);
        }

        public bool HasBeenPickedUp = false;
    }
}

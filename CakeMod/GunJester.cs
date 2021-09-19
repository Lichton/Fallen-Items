using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;

namespace CakeMod
{
    class GunJester : PassiveItem
    {
        private float random;
        private bool generated2;
        private bool generated;
        private StatModifier statModifier;
        private StatModifier statModifier2;

        public static void Init()
        {
            string itemName = "Gun Jester";
            string resourceName = "CakeMod/Resources/GunJester";

            GameObject obj = new GameObject(itemName);
            var item = obj.AddComponent<GunJester>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "I CAN DO ANYTHING!";
            string longDesc = "Chaotic energies form into a twisted figure not entirely logical.\n\n" +
                "It can either grant firerate or damage.";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cak");

            item.quality = PickupObject.ItemQuality.EXCLUDED;

        }

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            DoRandomEffect(player);
        }

        public override DebrisObject Drop(PlayerController player)
        {
            player.ownerlessStatModifiers.Remove(statModifier);
            player.ownerlessStatModifiers.Remove(statModifier2);
            player.stats.RecalculateStats(player, false, false);
            return base.Drop(player);

        }

        private void DoRandomEffect(PlayerController player)
        {
            if (!this.generated)
            {
                this.random = UnityEngine.Random.value;
                this.generated = true;
            }
            if (random <= 0.50f)
            {
                ApplyStat(player, PlayerStats.StatType.RateOfFire, 0.5f, StatModifier.ModifyMethod.ADDITIVE);
            }
            else
            {
                ApplyStat(player, PlayerStats.StatType.Damage, 0.5f, StatModifier.ModifyMethod.ADDITIVE);
            }

            if (!this.generated2)
            {
                this.random = UnityEngine.Random.value;
                this.generated2 = true;
            }
            if (random <= 0.50f)
            {
                ApplyStat2(player, PlayerStats.StatType.Coolness, 3.0f, StatModifier.ModifyMethod.ADDITIVE);
            }
            else
            {
                ApplyStat2(player, PlayerStats.StatType.Curse, 2.0f, StatModifier.ModifyMethod.ADDITIVE);
            }
        }

        private void ApplyStat(PlayerController player, PlayerStats.StatType statType, float amountToApply, StatModifier.ModifyMethod modifyMethod)
        {
            player.stats.RecalculateStats(player, false, false);
            this.statModifier = new StatModifier()
            {
                statToBoost = statType,
                amount = amountToApply,
                modifyType = modifyMethod
            };
            player.ownerlessStatModifiers.Add(statModifier);
            player.stats.RecalculateStats(player, false, false);
        }

        private void ApplyStat2(PlayerController player, PlayerStats.StatType statType, float amountToApply, StatModifier.ModifyMethod modifyMethod)
        {
            player.stats.RecalculateStats(player, false, false);
            this.statModifier2 = new StatModifier()
            {
                statToBoost = statType,
                amount = amountToApply,
                modifyType = modifyMethod
            };
            player.ownerlessStatModifiers.Add(statModifier2);
            player.stats.RecalculateStats(player, false, false);
        }
    }
}
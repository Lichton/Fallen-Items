using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;

namespace CakeMod
{
    class Depresso : PassiveItem
    {

        public static void Init()
        {
            string itemName = "Depresso";
            string resourceName = "CakeMod/Resources/Depresso";

            GameObject obj = new GameObject(itemName);
            var item = obj.AddComponent<Depresso>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Drink Your Tears";
            string longDesc = "Forged from the tears of a lonely bullet kin.\n\n" +
                "Its sadness makes you less cool, but also empowers you. Getting hurt weakens this power.";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cak");

            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.MovementSpeed, 1.25f, StatModifier.ModifyMethod.MULTIPLICATIVE);

            item.quality = PickupObject.ItemQuality.D;
        }

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            float num = 0f;
            num = (GameManager.Instance.PrimaryPlayer.stats.GetStatValue(PlayerStats.StatType.Coolness));
            if (num >= 2)
            {
                this.AddStat(PlayerStats.StatType.Coolness, -2f, StatModifier.ModifyMethod.ADDITIVE);
                player.stats.RecalculateStats(player, true, false);
            }
            this.m_owner.OnReceivedDamage += this.gamertime;
        }

        public override DebrisObject Drop(PlayerController player)
        {
            this.RemoveStat(PlayerStats.StatType.Coolness);
            player.stats.RecalculateStats(player, true, false);
            return base.Drop(player);
        }
        private void gamertime(PlayerController player)
        {
            player.ApplyEffect(gamer, 10f, null);
        }
        public AIActorDebuffEffect gamer = new AIActorDebuffEffect
        {
            SpeedMultiplier = 0.75f,
            KeepHealthPercentage = true,
            duration = 5f
        };

        private void AddStat(PlayerStats.StatType statType, float amount, StatModifier.ModifyMethod method = StatModifier.ModifyMethod.ADDITIVE)
        {
            StatModifier statModifier = new StatModifier
            {
                amount = amount,
                statToBoost = statType,
                modifyType = method
            };
            bool flag = this.passiveStatModifiers == null;
            if (flag)
            {
                this.passiveStatModifiers = new StatModifier[]
                {
                    statModifier
                };
            }
            else
            {
                this.passiveStatModifiers = this.passiveStatModifiers.Concat(new StatModifier[]
                {
                    statModifier
                }).ToArray<StatModifier>();
            }
        }
        private void RemoveStat(PlayerStats.StatType statType)
        {
            List<StatModifier> list = new List<StatModifier>();
            for (int i = 0; i < this.passiveStatModifiers.Length; i++)
            {
                bool flag = this.passiveStatModifiers[i].statToBoost != statType;
                if (flag)
                {
                    list.Add(this.passiveStatModifiers[i]);
                }
            }
            this.passiveStatModifiers = list.ToArray();
        }
    }
}

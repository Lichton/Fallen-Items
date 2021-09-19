using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;
using Gungeon;
using SaveAPI;
using Dungeonator;
using System.Collections;
using SaveAPI;

namespace CakeMod
{
    class SewingKit : PassiveItem
    {

        public static void Init()
        {
            string itemName = "Sewing Kit";
            string resourceName = "CakeMod/Resources/SewingKit";

            GameObject obj = new GameObject(itemName);
            var item = obj.AddComponent<SewingKit>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Accuracy = Damage";
            string longDesc = "As the Bishop of the Gundead once said, thine accuracyeth doth define thine damage. Hitting things with bullets increases damage, but missing after a short period removes it.";
                

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cak");

           

            item.quality = PickupObject.ItemQuality.A;
        }

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
        private void PostProcessProjectile(Projectile projectile, float effectChanceScalar)
        {
            projectile.OnHitEnemy += this.OnHitEnemy;
            projectile.OnDestruction += this.OnDestruction;
        }

        private void OnDestruction(Projectile projectile)
        {
            if (BulletHittingEnemy == false)
            {
                this.RemoveStat(PlayerStats.StatType.Damage);
                Owner.stats.RecalculateStats(Owner, true, false);
            }
            
        }

        private void OnHitEnemy(Projectile bullet, SpeculativeRigidbody enemy, bool fuckingsomethingidk)
        {
            bool flag = bullet != null && bullet.specRigidbody != null;
            if (flag)
            {
                this.AddStat(PlayerStats.StatType.Damage, 1.01f, StatModifier.ModifyMethod.MULTIPLICATIVE);
                Owner.stats.RecalculateStats(Owner, true, false);
                BulletHittingEnemy = true;
                base.StartCoroutine(this.StartCooldown());

            }
          
        }
        private IEnumerator StartCooldown()
        {
            yield return new WaitForSeconds(1f);
            BulletHittingEnemy = false;
            yield break;
        }

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            SaveAPIManager.SetFlag(CustomDungeonFlags.VOODOO_HEART_FLAG, true);
            player.PostProcessProjectile += this.PostProcessProjectile;            

        }

        public override DebrisObject Drop(PlayerController player)
        {
            player.PostProcessProjectile -= this.PostProcessProjectile;
       
            return base.Drop(player);
        }

        public bool BulletHittingEnemy = false;
    }
}
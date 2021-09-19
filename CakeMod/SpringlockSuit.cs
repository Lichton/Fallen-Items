using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;
using Dungeonator;


namespace CakeMod
{
    class SpringlockSuit : PassiveItem
    {

        public static void Init()
        {
            string itemName = "Springlock Suit";
            string resourceName = "CakeMod/Resources/SpringlockSuit";

            GameObject obj = new GameObject(itemName);
            var item = obj.AddComponent<SpringlockSuit>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "He Always Comes Back";
            string longDesc = "A worn machine, a facsimle of a anthromophic bunny.\n\n" +
                "It seems patched in some places, with fading colour in others.\n" +
                "It cannot sustain itself without maintenance, and becomes unusable when dropped.";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cak");
            item.CanBeDropped = false;
     
            item.quality = PickupObject.ItemQuality.EXCLUDED;
        }

        private void ModifyIncomingDamage(HealthHaver source, HealthHaver.ModifyDamageEventArgs args)
        {
            if (didBirthday == true)
            {
                if (args.ModifiedDamage >= source.GetCurrentHealth())
                {
                    AkSoundEngine.PostEvent("Play_VO_lichA_cackle_01", base.gameObject);
                    args.ModifiedDamage = 0f;
                    float CurrentHealth = Owner.stats.GetStatValue(PlayerStats.StatType.Health);
                    StatModifier hpdown = new StatModifier();
                    hpdown.statToBoost = PlayerStats.StatType.Health;
                    hpdown.amount = ((CurrentHealth - 1) * -1);
                    hpdown.modifyType = StatModifier.ModifyMethod.ADDITIVE;
                    Owner.ownerlessStatModifiers.Add(hpdown);
                    Owner.stats.RecalculateStats(Owner, false, false);
                    Owner.healthHaver.ApplyHealing(0.5f);


                    GameManager.Instance.LoadCustomLevel("tt_castle");
                    this.m_fireImmunity = new DamageTypeModifier();
                    this.m_fireImmunity.damageMultiplier = 0f;
                    this.m_fireImmunity.damageType = CoreDamageTypes.Fire;
                    base.Owner.healthHaver.damageTypeModifiers.Add(this.m_fireImmunity);
                    didBirthday = false;
                }
            }
        }
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            HealthHaver healthHaver = player.healthHaver;
            healthHaver.ModifyDamage += this.ModifyIncomingDamage;

            
        }

        public override DebrisObject Drop(PlayerController player)
        {
            HealthHaver healthHaver = player.healthHaver;
            healthHaver.ModifyDamage -= this.ModifyIncomingDamage;

            return base.Drop(player);
        }
        bool didBirthday = true;

        private DamageTypeModifier m_fireImmunity;

        private float MaxHp;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;
using Dungeonator;
using SaveAPI;

namespace CakeMod
{
    class VoodooHeart : PassiveItem
    {

        public static void Init()
        {
            string itemName = "Voodoo Heart";
            string resourceName = "CakeMod/Resources/VoodooHeart";

            GameObject obj = new GameObject(itemName);
            var item = obj.AddComponent<VoodooHeart>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Who Do Voodoo?";
            string longDesc = "Grants a second chance... for a price.\n\n" +
                "A busted up heart, held together via strings and nails. It radiates cursed energies, sapping your lifeforce.";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cak");

            item.quality = PickupObject.ItemQuality.B;
            item.SetupUnlockOnCustomFlag(CustomDungeonFlags.VOODOO_HEART_FLAG, true);
        }

        private void ModifyIncomingDamage(HealthHaver source, HealthHaver.ModifyDamageEventArgs args)
        {
            PlayableCharacters characterIdentity = Owner.characterIdentity;

            if (characterIdentity != PlayableCharacters.Robot)
            {
                if (source.Armor <= 0f)
                {
                    if (args.ModifiedDamage >= source.GetCurrentHealth())
                    {
                        if (source.GetMaxHealth() > 1f)
                        {
                            if (args.ModifiedDamage != 0)
                            {
                                if (source.IsVulnerable == true)
                                {
                                    AkSoundEngine.PostEvent("Play_CHR_major_damage_01", base.gameObject);
                                    args.ModifiedDamage = 0f;
                                    StatModifier hpdown = new StatModifier();
                                    hpdown.statToBoost = PlayerStats.StatType.Health;
                                    hpdown.amount = -1;
                                    hpdown.modifyType = StatModifier.ModifyMethod.ADDITIVE;
                                    Owner.ownerlessStatModifiers.Add(hpdown);
                                    Owner.stats.RecalculateStats(Owner, false, false);
                                    Owner.healthHaver.TriggerInvulnerabilityPeriod();
                                    
                                    if (this.m_owner.PlayerHasActiveSynergy("Terrible Person"))
                                    {
                                        Owner.healthHaver.ApplyHealing(0.5f);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                int bighead = UnityEngine.Random.Range(1, 8);
                if (bighead == 1)
                {
                    args.ModifiedDamage = 0f;
                    Owner.healthHaver.TriggerInvulnerabilityPeriod();
                }
                }
        }

        
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            HealthHaver healthHaver = player.healthHaver;
            healthHaver.ModifyDamage += this.ModifyIncomingDamage;
        }

        private void TheThingie(Vector2 obj)
        {
            healthHaver.ModifyDamage += this.ModifyIncomingDamage;
        }

        public override DebrisObject Drop(PlayerController player)
        {
            HealthHaver healthHaver = player.healthHaver;
            healthHaver.ModifyDamage -= this.ModifyIncomingDamage;

            return base.Drop(player);
        }


        private float MaxHp;
    }
}

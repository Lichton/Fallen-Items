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

namespace CakeMod
{
    class ImmortalSmoke : PassiveItem
    {

        public static void Init()
        {
            string itemName = "Immortal Smoke";
            string resourceName = "CakeMod/Resources/ImmortalSmoke";

            GameObject obj = new GameObject(itemName);
            var item = obj.AddComponent<ImmortalSmoke>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Reach for the Moon";
            string longDesc = "Fire is your friend.\n\n" +
                "A pile of ash and still burning embers left behind by the bearer of a terrible curse.";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cak");
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Curse, 1);

            item.quality = PickupObject.ItemQuality.EXCLUDED;

        }
            private System.Collections.IEnumerator InflictRage(PlayerController player)
        {
            float elapsed = 0f;
            float particleCounter = 0f;
          
            while (elapsed == 0)
            {
                bool flag2 = GameManager.Options.ShaderQuality != GameOptions.GenericHighMedLowOption.LOW && GameManager.Options.ShaderQuality != GameOptions.GenericHighMedLowOption.VERY_LOW && base.Owner && base.Owner.IsVisible && !base.Owner.IsFalling;
                if (flag2)
                {
                    particleCounter += BraveTime.DeltaTime * 40f;
                    
          
                    bool flag4 = particleCounter > 1f;
                    if (flag4)
                    {
                        if (Murder == true)
                        {
                            int num = Mathf.FloorToInt(particleCounter);
                            particleCounter %= 1f;
                            GlobalSparksDoer.DoRandomParticleBurst(num, base.Owner.sprite.WorldBottomLeft.ToVector3ZisY(0f), base.Owner.sprite.WorldTopRight.ToVector3ZisY(0f), Vector3.up, 90f, 0.5f, null, null, null, GlobalSparksDoer.SparksType.BLACK_PHANTOM_SMOKE);
                        }
                    }
                }
                yield return null;
            }
            yield break;
        }

       

        // Token: 0x06000232 RID: 562 RVA: 0x00016E94 File Offset: 0x00015094
       

        private void ThepickerUpper()
        {
            StartCoroutine(this.InflictRage(Owner));
        }
        public override void Pickup(PlayerController player)
        {
            player.OnDodgedProjectile += this.DoBigGoop;
            base.Pickup(player);
            
            this.m_fireImmunity = new DamageTypeModifier();
            this.m_fireImmunity.damageMultiplier = 0f;
            this.m_fireImmunity.damageType = CoreDamageTypes.Fire;
            Owner.healthHaver.damageTypeModifiers.Add(this.m_fireImmunity);
            this.ThepickerUpper();
            Murder = true;

        }

        public override DebrisObject Drop(PlayerController player)
        {
            Murder = false;
            Owner.OnDodgedProjectile -= this.DoBigGoop;
            Owner.healthHaver.damageTypeModifiers.Remove(this.m_fireImmunity);
            return base.Drop(player);
           
        }
        
       
           
               

        public void DoBigGoop(Projectile bullet)
        {
            DeadlyDeadlyGoopManager goopManagerForGoopType = DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(EasyGoopDefinitions.FireDef);
            goopManagerForGoopType.AddGoopCircle(this.m_owner.specRigidbody.UnitCenter, 1f, -4, false, -4);
        }
      
        protected override void OnDestroy()
        {
            Owner.healthHaver.damageTypeModifiers.Remove(this.m_fireImmunity);
            Owner.OnDodgedProjectile -= this.DoBigGoop;
            base.OnDestroy();
        }
        public bool Murder = true;
        private DamageTypeModifier m_fireImmunity;

        private HeatIndicatorController m_indicator;
    }
}
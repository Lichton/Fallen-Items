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
    class ThirdEye : PassiveItem
    {

        public static void Init()
        {
            string itemName = "Third Eye";
            string resourceName = "CakeMod/Resources/ThirdEye";

            GameObject obj = new GameObject(itemName);
            var item = obj.AddComponent<ThirdEye>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Heartfelt Fancy";
            string longDesc = "Makes it easier to convince enemies that fighting you is no use.\n\n" +
                "A symbiotic creature that burrows into the forehead of organics, granting psychic power.";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cak");

            item.quality = PickupObject.ItemQuality.EXCLUDED;

        }


        // Token: 0x06000232 RID: 562 RVA: 0x00016E94 File Offset: 0x00015094


        public override void Pickup(PlayerController player)
        {
            player.OnDodgedProjectile += this.DoBigGoop;
            base.Pickup(player);
          

        }

        public override DebrisObject Drop(PlayerController player)
        {
            
            Owner.OnDodgedProjectile -= this.DoBigGoop;
            
            return base.Drop(player);

        }





        public void DoBigGoop(Projectile bullet)
        {
            if (bullet.Owner.aiActor != null)
            {
              int bighead = UnityEngine.Random.Range(1, 16);
                if (bighead == 1)
                {
                    bullet.Owner.aiActor.ApplyEffect(charmEffect, 5f, null);
                }
            }
        }

        protected override void OnDestroy()
        {
           
            Owner.OnDodgedProjectile -= this.DoBigGoop;
            base.OnDestroy();
        }
        public GameActorCharmEffect charmEffect = Gungeon.Game.Items["charming_rounds"].GetComponent<BulletStatusEffectItem>().CharmModifierEffect;
    }
}
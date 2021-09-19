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
    class BulletKinBullets : PassiveItem
    {

        public static void Init()
        {
            string itemName = "Kin Bullets";
            string resourceName = "CakeMod/Resources/bullet_kin_bullets";

            GameObject obj = new GameObject(itemName);
            var item = obj.AddComponent<BulletKinBullets>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Paradoxical";
            string longDesc = "Which came first, the bullet or the kin?\n\n" +
                "Adds a small chance to transmogrify enemies hit into bullet kin.";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cak");

           



            item.quality = PickupObject.ItemQuality.B;

        }
        private void PostProcessProjectile(Projectile sourceProjectile, float effectChanceScalar)
        {
            sourceProjectile.OnHitEnemy += this.OnHitEnemy;
        }
        private void OnHitEnemy(Projectile bullet, SpeculativeRigidbody enemy, bool fuckingsomethingidk)
        {
            

                int bighead = UnityEngine.Random.Range(1, 11);
                if (bighead == 1)
                {
                if (enemy.aiActor.EnemyGuid != "01972dee89fc4404a5c408d50007dad5")
                {
                    if (enemy.aiActor.healthHaver.IsBoss == false)
                    {
                        enemy.aiActor.Transmogrify(EnemyDatabase.GetOrLoadByGuid("01972dee89fc4404a5c408d50007dad5"), (GameObject)ResourceCache.Acquire("Global VFX/VFX_Item_Spawn_Poof"));
                    }
                }
                }
        }

        public override void Pickup(PlayerController player)
        {
            player.PostProcessProjectile += this.PostProcessProjectile;
            base.Pickup(player);
        }

        public override DebrisObject Drop(PlayerController player)
        {
            player.PostProcessProjectile -= this.PostProcessProjectile;
            return base.Drop(player);
        }
    }
}


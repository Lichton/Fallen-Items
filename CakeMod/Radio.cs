using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;
using Dungeonator;

namespace CakeMod
{
    class Radio : PlayerItem
    {
        //Call this method from the Start() method of your ETGModule extension class
        public static void Init()
        {
            //The name of the item
            string itemName = "Radio";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it.
            string resourceName = "CakeMod/Resources/Radio";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a ActiveItem component to the object
            var item = obj.AddComponent<Radio>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "Picture Show";
            string longDesc = "Cursed to broadcast an eldritch pulse when turned on.";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //"kts" here is the item pool. In the console you'd type kts:sweating_bullets
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cak");

            //Set the cooldown type and duration of the cooldown
            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.Damage, 1000);


            //Set some other fields
            item.consumable = false;
            item.quality = ItemQuality.B;
        }

        protected override void DoEffect(PlayerController user)
        {
            AkSoundEngine.PostEvent("Play_WPN_Gun_Musical_01", gameObject);
            Exploder.DoRadialKnockback(user.CenterPosition, 100f, 15f);
            this.Collide1(15f);
            ETGMod.SayTheMagicWord();
            ETGMod.KeepSinging();
        }
        protected override void OnPreDrop(PlayerController user)
        {

        }

        private void Collide1(float radius)
        {
            List<AIActor> activeEnemies = base.LastOwner.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
            if (activeEnemies != null)
            {
                int count = activeEnemies.Count;
                for (int i = 0; i < count; i++)
                {
                    if (activeEnemies[i] && activeEnemies[i].HasBeenEngaged && activeEnemies[i].healthHaver && activeEnemies[i].IsNormalEnemy && !activeEnemies[i].healthHaver.IsDead && !activeEnemies[i].healthHaver.IsBoss && activeEnemies[i].specRigidbody != null && Vector2.Distance(activeEnemies[i].specRigidbody.UnitCenter, base.LastOwner.CenterPosition) <= radius)
                    {
                        AddCollide(activeEnemies[i]);
                    }
                }
            }
        }

        private void AddCollide(AIActor arg2)
        {
            arg2.specRigidbody.CollideWithOthers = true;
            arg2.specRigidbody.AddCollisionLayerOverride(CollisionMask.LayerToMask(CollisionLayer.EnemyCollider));
            arg2.specRigidbody.AddCollisionLayerIgnoreOverride(CollisionMask.LayerToMask(CollisionLayer.PlayerHitBox));
            arg2.specRigidbody.AddCollisionLayerIgnoreOverride(CollisionMask.LayerToMask(CollisionLayer.PlayerCollider));
            arg2.specRigidbody.OnCollision += Bang;
        }
        private void Bang(CollisionData tileCollision)
        {
            Vector2 point = tileCollision.MyRigidbody.UnitCenter;
            float speed = tileCollision.MyRigidbody.Velocity.magnitude;
            this.Gun(point, speed, tileCollision.MyRigidbody);

        }
        private void Gun(Vector2 position, float speed, SpeculativeRigidbody target)
        {
            target.OnCollision -= Bang;
            PlayerController man = base.LastOwner as PlayerController;
            Projectile projectile2 = ((Gun)global::ETGMod.Databases.Items[541]).DefaultModule.chargeProjectiles[0].Projectile;
            GameObject gameObject = SpawnManager.SpawnProjectile(projectile2.gameObject, position, Quaternion.Euler(0f, 0f, LastOwner.CurrentGun.CurrentAngle), true);
            Projectile component = gameObject.GetComponent<Projectile>();
            bool flag8 = component != null;
            if (flag8)
            {
                component.AdditionalScaleMultiplier = 2f;
                component.baseData.damage = 5f * speed * man.stats.GetStatValue(PlayerStats.StatType.Damage);
                component.baseData.force = 0f;
                component.Owner = man;
                component.Shooter = man.specRigidbody;

            }
        }


        //Disable or enable the active whenever you need!
        public override bool CanBeUsed(PlayerController user)
        {
            return base.CanBeUsed(user);
        }
    }
}
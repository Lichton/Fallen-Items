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
    class BloodTether : PassiveItem
    {

        public static void Init()
        {
            string itemName = "Blood Tether";
            string resourceName = "CakeMod/Resources/OddBullets";

            GameObject obj = new GameObject(itemName);
            var item = obj.AddComponent<BloodTether>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Quite Odd, Strange Even";
            string longDesc = "Bullets weighed in such a manner that they are uneven." +
                " Any ammunition that is odd in count becomes more powerful.";
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cak");





            item.quality = PickupObject.ItemQuality.EXCLUDED;

        }

        public override void Pickup(PlayerController player)
        {
            player.OnEnteredCombat += this.ConnectBlood;
            base.Pickup(player);
        }
        private GameObject LinkVFXPrefab;
        private tk2dTiledSprite extantLink;
        public int attempts = 0;
        private void ConnectBlood()
        {
            RoomHandler absoluteRoom = base.transform.position.GetAbsoluteRoom();
            randomActiveEnemy = absoluteRoom.GetRandomActiveEnemy(false);
            randomActiveEnemy2 = absoluteRoom.GetRandomActiveEnemy(false);
            
            if(randomActiveEnemy2 != randomActiveEnemy && randomActiveEnemy.healthHaver.IsBoss != true && randomActiveEnemy2.healthHaver.IsBoss != true)
            {
                m_cable = randomActiveEnemy.gameObject.AddComponent<ArbitraryCableDrawer>();
                m_cable.Attach1 = randomActiveEnemy.transform;
                m_cable.Attach2 = randomActiveEnemy2.transform;
                m_cable.Attach1Offset = randomActiveEnemy.CenterPosition - randomActiveEnemy.transform.position.XY();
                m_cable.Attach2Offset = randomActiveEnemy2.CenterPosition - randomActiveEnemy2.transform.position.XY();
                m_cable.Initialize(randomActiveEnemy.transform, randomActiveEnemy2.transform);
                MeshRenderer renderer = m_cable.GetComponent<MeshRenderer>();
                renderer.material.SetColor("_OverrideColor", Color.red);
                renderer.material.color = Color.red;
                randomActiveEnemy.healthHaver.OnDeath += this.KillBoth;
                randomActiveEnemy2.healthHaver.OnDeath += this.KillBoth;
            }

        }

        private void KillBoth(Vector2 obj)
        {
            if (randomActiveEnemy.healthHaver.IsAlive)
            {
                randomActiveEnemy.healthHaver.ApplyDamage(100000f, Vector2.zero, "dual death", CoreDamageTypes.None, DamageCategory.Unstoppable, true, null, false);
            }
            if (randomActiveEnemy2.healthHaver.IsAlive)
            {
                randomActiveEnemy2.healthHaver.ApplyDamage(100000f, Vector2.zero, "dual death", CoreDamageTypes.None, DamageCategory.Unstoppable, true, null, false);
            }
        }

        public AIActor randomActiveEnemy;
        public AIActor randomActiveEnemy2;
        private ArbitraryCableDrawer m_cable;
        public override DebrisObject Drop(PlayerController player)
        {
            player.OnEnteredCombat -= this.ConnectBlood;
            return base.Drop(player);
        }
    }

    
}

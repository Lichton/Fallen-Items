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
    class GhostlyBody : PassiveItem
    {

        public static void Init()
        {
            string itemName = "Ghostly Body";
            string resourceName = "CakeMod/Resources/GhostlyBody";

            GameObject obj = new GameObject(itemName);
            var item = obj.AddComponent<GhostlyBody>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "In-Spectral";
            string longDesc = "Allows the bearer to pass through living beings as if they weren't there.";
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cak");



            item.quality = PickupObject.ItemQuality.EXCLUDED;

        }
        protected override void Update()
        {
            if (Gamer == true)
            {
                PlayerController player = GameManager.Instance.PrimaryPlayer;
                player.specRigidbody.AddCollisionLayerIgnoreOverride(CollisionMask.LayerToMask(CollisionLayer.EnemyHitBox, CollisionLayer.EnemyCollider));
            }
            if(Gamer == false)
            {
                PlayerController player = GameManager.Instance.PrimaryPlayer;
                player.specRigidbody.RemoveCollisionLayerIgnoreOverride(CollisionMask.LayerToMask(CollisionLayer.EnemyHitBox, CollisionLayer.EnemyCollider));
            }
        }
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            Gamer = true;
            player.specRigidbody.AddCollisionLayerIgnoreOverride(CollisionMask.LayerToMask(CollisionLayer.EnemyHitBox, CollisionLayer.EnemyCollider));
            LiveAmmoItem liveammo = PickupObjectDatabase.GetById(414).GetComponent<LiveAmmoItem>();

            if (!PassiveItem.ActiveFlagItems.ContainsKey(player))
            {
                PassiveItem.ActiveFlagItems.Add(player, new Dictionary<Type, int>());
            }
            if (!PassiveItem.ActiveFlagItems[player].ContainsKey(liveammo.GetType()))
            {
                PassiveItem.ActiveFlagItems[player].Add(liveammo.GetType(), 1);
            }
            else
            {
                PassiveItem.ActiveFlagItems[player][liveammo.GetType()] = PassiveItem.ActiveFlagItems[player][liveammo.GetType()] + 1;
            }
        }

        public override DebrisObject Drop(PlayerController player)
        {
            Gamer = false;
            LiveAmmoItem liveammo = PickupObjectDatabase.GetById(414).GetComponent<LiveAmmoItem>();
            player.specRigidbody.RemoveCollisionLayerIgnoreOverride(CollisionMask.LayerToMask(CollisionLayer.EnemyHitBox, CollisionLayer.EnemyCollider));
            if (PassiveItem.ActiveFlagItems[player].ContainsKey(liveammo.GetType()))
            {
                PassiveItem.ActiveFlagItems[player][liveammo.GetType()] = Mathf.Max(0, PassiveItem.ActiveFlagItems[player][liveammo.GetType()] - 1);
                if (PassiveItem.ActiveFlagItems[player][liveammo.GetType()] == 0)
                {
                    PassiveItem.ActiveFlagItems[player].Remove(liveammo.GetType());
                }
            }
            return base.Drop(player);
            
        }
        public bool Gamer = true;
    }
}

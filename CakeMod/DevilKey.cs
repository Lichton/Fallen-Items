using System;
using System.Collections;
using Gungeon;
using MonoMod;
using UnityEngine;
using ItemAPI;
using BasicGun;
using System.Collections.Generic;
using Dungeonator;
using System.Reflection;
using MonoMod.RuntimeDetour;
using SaveAPI;

namespace CakeMod
{
    class DevilKey : PlayerItem
    {
        //Call this method from the Start() method of your ETGModule extension class
        public static void Init()
        {
            //The name of the item
            string itemName = "Devil's Key";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it.
            string resourceName = "CakeMod/Resources/DevilKey";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a ActiveItem component to the object
            var item = obj.AddComponent<DevilKey>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "Chests... at a price?";
            string longDesc = "A cursed key binding the demon Beezlebox. Opens chests for free, but at what cost?";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //"kts" here is the item pool. In the console you'd type kts:sweating_bullets
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cak");

            //Set the cooldown type and duration of the cooldown
            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.Damage, 750);
            item.SetupUnlockOnCustomFlag(CustomDungeonFlags.CHEST_DEVIL_UNLOCK, true);

            //Set some other fields
            item.consumable = false;
            item.quality = ItemQuality.B;
        }

        //Add the item's functionality down here! I stole most of this from the Stuffed Star active item code!
        float duration = 10f;
        protected override void DoEffect(PlayerController user)
        {
            IPlayerInteractable nearestInteractable = user.CurrentRoom.GetNearestInteractable(user.CenterPosition, 1f, user);
            if (!(nearestInteractable is Chest)) return;

            Chest rerollChest = nearestInteractable as Chest;
            if (rerollChest.IsGlitched == true)
            {
                Chest.Spawn(GameManager.Instance.RewardManager.S_Chest, rerollChest.sprite.WorldCenter.ToIntVector2());
            }
            else
            {
                if (rerollChest.IsRainbowChest == true)
                {
                    Chest badchest = Chest.Spawn(GameManager.Instance.RewardManager.S_Chest, rerollChest.sprite.WorldCenter.ToIntVector2());
                    badchest.IsLocked = false;
                }
                else
                {
                    if ((GameManager.Instance.RewardManager.GetQualityFromChest(rerollChest) == ItemQuality.D))
                    {
                        LootEngine.SpawnItem(Gungeon.Game.Items["junk"].gameObject, rerollChest.sprite.WorldCenter, Vector2.left, 0);
                    }
                    if ((GameManager.Instance.RewardManager.GetQualityFromChest(rerollChest) == ItemQuality.C))
                    {
                        Chest badchest = Chest.Spawn(GameManager.Instance.RewardManager.D_Chest, rerollChest.sprite.WorldCenter.ToIntVector2());
                        badchest.IsLocked = false;
                    }
                    if ((GameManager.Instance.RewardManager.GetQualityFromChest(rerollChest) == ItemQuality.B))
                    {
                        Chest badchest = Chest.Spawn(GameManager.Instance.RewardManager.C_Chest, rerollChest.sprite.WorldCenter.ToIntVector2());
                        badchest.IsLocked = false;
                    }
                    if ((GameManager.Instance.RewardManager.GetQualityFromChest(rerollChest) == ItemQuality.A))
                    {
                        Chest badchest = Chest.Spawn(GameManager.Instance.RewardManager.B_Chest, rerollChest.sprite.WorldCenter.ToIntVector2());
                        badchest.IsLocked = false;
                    }
                    if ((GameManager.Instance.RewardManager.GetQualityFromChest(rerollChest) == ItemQuality.S))
                    {
                        Chest badchest = Chest.Spawn(GameManager.Instance.RewardManager.A_Chest, rerollChest.sprite.WorldCenter.ToIntVector2());
                        badchest.IsLocked = false;
                    }
                }
                
                user.CurrentRoom.DeregisterInteractable(rerollChest);
            rerollChest.DeregisterChestOnMinimap();
            Destroy(rerollChest.gameObject);
            
            }
           
        }


        protected override void OnPreDrop(PlayerController user)
        {

        }

        //Disable or enable the active whenever you need!
        public override bool CanBeUsed(PlayerController user)
        {
            IPlayerInteractable nearestInteractable = user.CurrentRoom.GetNearestInteractable(user.CenterPosition, 1f, user);
            return nearestInteractable is Chest && (nearestInteractable as Chest).IsLocked;
        }

        public AIActor randomActiveEnemy;
    }
}


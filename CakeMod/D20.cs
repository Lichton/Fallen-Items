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

namespace CakeMod
{
   
    class d20 : PlayerItem
    {
        private AmmoPickup rerollChest;
        //Call this method from the Start() method of your ETGModule extension class
        public static void Init()
        {
            //The name of the item
            string itemName = "Ammonian Device";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it.
            string resourceName = "CakeMod/Resources/AmmoMachine";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a ActiveItem component to the object
            var item = obj.AddComponent<d20>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "Ammo = Stats";
            string longDesc = "A strange device capable of consuming ammo to give the bearer strength.";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //"kts" here is the item pool. In the console you'd type kts:sweating_bullets
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cak");

            //Set the cooldown type and duration of the cooldown
            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.Timed, 3);


            //Set some other fields
            item.consumable = false;
            item.quality = ItemQuality.C;
        }

        //Add the item's functionality down here! I stole most of this from the Stuffed Star active item code!
        float duration = 10f;
        protected override void DoEffect(PlayerController user)
        {
            IPlayerInteractable nearestInteractable = user.CurrentRoom.GetNearestInteractable(user.CenterPosition, 3f, user);
            if (!(nearestInteractable is AmmoPickup)) return;
            AmmoPickup rerollChest = nearestInteractable as AmmoPickup;
            Instantiate<GameObject>(EasyVFXDatabase.BloodiedScarfPoofVFX, rerollChest.sprite.WorldCenter, Quaternion.identity);
            Destroy(rerollChest.gameObject);
            AkSoundEngine.PostEvent("Play_OBJ_ammo_suck_01", gameObject);
            int bighead = UnityEngine.Random.Range(1, 11);
            if (bighead == 1)
            {
                this.ApplyStat(user, PlayerStats.StatType.Damage, 0.15f, StatModifier.ModifyMethod.ADDITIVE);
            }
            if (bighead == 2)
            {
                this.ApplyStat(user, PlayerStats.StatType.Health, 1f, StatModifier.ModifyMethod.ADDITIVE);
            }
            if (bighead == 3)
            {
                this.ApplyStat(user, PlayerStats.StatType.Coolness, 1f, StatModifier.ModifyMethod.ADDITIVE);
            }
            if (bighead == 4)
            {
                this.ApplyStat(user, PlayerStats.StatType.MovementSpeed, 0.15f, StatModifier.ModifyMethod.ADDITIVE);
            }
            if (bighead == 5)
            {
                this.ApplyStat(user, PlayerStats.StatType.AdditionalClipCapacityMultiplier, 0.15f, StatModifier.ModifyMethod.ADDITIVE);
            }
            if (bighead == 6)
            {
                this.ApplyStat(user, PlayerStats.StatType.AmmoCapacityMultiplier, 0.15f, StatModifier.ModifyMethod.ADDITIVE);
            }
            if (bighead == 7)
            {
                this.ApplyStat(user, PlayerStats.StatType.AdditionalBlanksPerFloor, 1f, StatModifier.ModifyMethod.ADDITIVE);
            }
            if (bighead == 8)
            {
                this.ApplyStat(user, PlayerStats.StatType.RateOfFire, 0.15f, StatModifier.ModifyMethod.ADDITIVE);
            }
            if (bighead == 9)
            {
                this.ApplyStat(user, PlayerStats.StatType.ReloadSpeed, 0.15f, StatModifier.ModifyMethod.ADDITIVE);
            }
            if (bighead == 10)
            {
                this.ApplyStat(user, PlayerStats.StatType.RangeMultiplier, 0.15f, StatModifier.ModifyMethod.ADDITIVE);
            }
        }

        

        protected override void OnPreDrop(PlayerController user)
        {

        }


        //Disable or enable the active whenever you need!
        public override bool CanBeUsed(PlayerController user)
        {

            IPlayerInteractable nearestInteractable = user.CurrentRoom.GetNearestInteractable(user.CenterPosition, 2f, user);
            return nearestInteractable is AmmoPickup;
        }

        private void ApplyStat(PlayerController player, PlayerStats.StatType statType, float amountToApply, StatModifier.ModifyMethod modifyMethod)
        {
            player.stats.RecalculateStats(player, false, false);
            StatModifier item = new StatModifier
            {
                statToBoost = statType,
                amount = amountToApply,
                modifyType = modifyMethod
            };
            player.ownerlessStatModifiers.Add(item);
            player.stats.RecalculateStats(player, false, false);
        }
    }
}


using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;

namespace CakeMod
{
    class StrangePotion : PlayerItem
    {
        //Call this method from the Start() method of your ETGModule extension class
        public static void Init()
        {
            //The name of the item
            string itemName = "Elixir";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it.
            string resourceName = "CakeMod/Resources/StrangePotion";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a ActiveItem component to the object
            var item = obj.AddComponent<StrangePotion>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "Poison Apple";
            string longDesc = "An odd potion with an unreadable lable.\n\nWho knows what it's effect could be?";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //"kts" here is the item pool. In the console you'd type kts:sweating_bullets
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cak");

            //Set the cooldown type and duration of the cooldown
            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.Damage, 800);

            //Set some other fields
            item.consumable = false;
            item.quality = ItemQuality.B;
        }

        //Add the item's functionality down here! I stole most of this from the Stuffed Star active item code!
        float duration = 10f;

        protected override void DoEffect(PlayerController user)
        {
            string header = "You feel sick.";
            string text = "You drank a potion.";
            AkSoundEngine.PostEvent("Play_WPN_Bubbler_Drink_01", gameObject);
            int bighead = UnityEngine.Random.Range(1, 19);
            if (bighead == 1)
            {
                header = "Damage up.";
                text = "Lucky you!";
                this.ApplyStat(user, PlayerStats.StatType.Damage, 0.05f, StatModifier.ModifyMethod.ADDITIVE);
            }
            if (bighead == 2)
            {
                header = "Projectile speed up.";
                text = "Lucky you!";
                this.ApplyStat(user, PlayerStats.StatType.ProjectileSpeed, 0.05f, StatModifier.ModifyMethod.ADDITIVE);
            }
            if (bighead == 3)
            {
                header = "Movement speed up.";
                text = "Lucky you!.";
                this.ApplyStat(user, PlayerStats.StatType.MovementSpeed, 0.05f, StatModifier.ModifyMethod.ADDITIVE);
            }
            if (bighead == 4)
            {
                header = "Damage to bosses up.";
                text = "Lucky you!";
                this.ApplyStat(user, PlayerStats.StatType.DamageToBosses, 0.05f, StatModifier.ModifyMethod.ADDITIVE);
            }
            if (bighead == 5)
            {
                header = "Blanks up.";
                text = "Lucky you!";
                LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(224).gameObject, LastOwner);
            }
            if (bighead == 6)
            {
                header = "Curse up.";
                text = "Unlucky.";

                this.ApplyStat(user, PlayerStats.StatType.Curse, 1f, StatModifier.ModifyMethod.ADDITIVE);
            }
            if (bighead == 7)
            {
                header = "Ammo Capacity up.";
                text = "Lucky you!";
                this.ApplyStat(user, PlayerStats.StatType.AmmoCapacityMultiplier, 1.05f, StatModifier.ModifyMethod.MULTIPLICATIVE);
            }
            if (bighead == 8)
            {
                header = "Coolness up.";
                text = "Lucky you!";
                this.ApplyStat(user, PlayerStats.StatType.Coolness, 3f, StatModifier.ModifyMethod.ADDITIVE);
            }
            if (bighead == 9)
            {
                header = "Armour up.";
                text = "Lucky you!";
                LastOwner.healthHaver.Armor = LastOwner.healthHaver.Armor + 1;

            }
            if (bighead == 10)
            {
                header = "Money up.";
                text = "Lucky you!";
                LastOwner.carriedConsumables.Currency = LastOwner.carriedConsumables.Currency + 15;
            }
            if (bighead == 11)
            {
                header = "Damage.";
                text = "Unlucky.";
                LastOwner.healthHaver.ApplyDamage(1, Vector2.zero, "Chaotic Forces", CoreDamageTypes.None, DamageCategory.Normal, true, null, false);

            }
            if (bighead == 12)
            {
                header = "Keys up.";
                text = "Lucky you!";
                LastOwner.carriedConsumables.KeyBullets = LastOwner.carriedConsumables.KeyBullets + 1;
            }
            if (bighead == 13)
            {
                header = "Mapped out.";
                text = "Lucky you!";
                Minimap.Instance.RevealAllRooms(false);
            }
            if (bighead == 14)
            {
                header = "Lead Embrace.";
                text = "Unlucky.";
                AIActor orLoadByGuid = EnemyDatabase.GetOrLoadByGuid("cd4a4b7f612a4ba9a720b9f97c52f38c");
                IntVector2? intVector = new IntVector2?(base.LastOwner.CurrentRoom.GetRandomVisibleClearSpot(2, 2));

                AIActor aiactor = AIActor.Spawn(orLoadByGuid.aiActor, intVector.Value, GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(intVector.Value), true, AIActor.AwakenAnimationType.Default, true);
            }
            if (bighead == 15)
            {
                header = "Glass blessing.";
                text = "Lucky you!";
                LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(565).gameObject, LastOwner);

            }
            if (bighead == 16)
            {
                header = "Green fire.";
                text = "Lucky you!";
                DeadlyDeadlyGoopManager goopManagerForGoopType = DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(EasyGoopDefinitions.GreenFireDef);
                goopManagerForGoopType.TimedAddGoopCircle(LastOwner.sprite.WorldCenter, 5f, 0.35f, false);
            }
            if (bighead == 17)
            {
                header = "Poison!";
                text = "Unlucky.";
                DeadlyDeadlyGoopManager goopManagerForGoopType = DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(EasyGoopDefinitions.PoisonDef);
                goopManagerForGoopType.TimedAddGoopCircle(LastOwner.sprite.WorldCenter, 5f, 0.35f, false);

            }
            if (bighead == 18)
            {
                header = "Fire!";
                text = "Unlucky.";
                DeadlyDeadlyGoopManager goopManagerForGoopType = DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(EasyGoopDefinitions.FireDef);
                goopManagerForGoopType.TimedAddGoopCircle(LastOwner.sprite.WorldCenter, 5f, 0.35f, false);
            }
            this.Notify(header, text);
            
            }

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.OnRoomClearEvent += Pandamoment;
            


        }

        private void Pandamoment(PlayerController user)
        {
            if (LastOwner.PlayerHasActiveSynergy("Agent of Chaos"))

            {
                string header = "You feel sick.";
                string text = "You drank a potion.";
                AkSoundEngine.PostEvent("Play_WPN_Bubbler_Drink_01", gameObject);
                int bighead = UnityEngine.Random.Range(1, 19);
                if (bighead == 1)
                {
                    header = "Damage up.";
                    text = "Lucky you!";
                    this.ApplyStat(user, PlayerStats.StatType.Damage, 0.05f, StatModifier.ModifyMethod.ADDITIVE);
                }
                if (bighead == 2)
                {
                    header = "Projectile speed up.";
                    text = "Lucky you!";
                    this.ApplyStat(user, PlayerStats.StatType.ProjectileSpeed, 0.05f, StatModifier.ModifyMethod.ADDITIVE);
                }
                if (bighead == 3)
                {
                    header = "Movement speed up.";
                    text = "Lucky you!.";
                    this.ApplyStat(user, PlayerStats.StatType.MovementSpeed, 0.05f, StatModifier.ModifyMethod.ADDITIVE);
                }
                if (bighead == 4)
                {
                    header = "Damage to bosses up.";
                    text = "Lucky you!";
                    this.ApplyStat(user, PlayerStats.StatType.DamageToBosses, 0.05f, StatModifier.ModifyMethod.ADDITIVE);
                }
                if (bighead == 5)
                {
                    header = "Blanks up.";
                    text = "Lucky you!";
                    LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(224).gameObject, LastOwner);
                }
                if (bighead == 6)
                {
                    header = "Curse up.";
                    text = "Unlucky.";

                    this.ApplyStat(user, PlayerStats.StatType.Curse, 1f, StatModifier.ModifyMethod.ADDITIVE);
                }
                if (bighead == 7)
                {
                    header = "Ammo Capacity up.";
                    text = "Lucky you!";
                    this.ApplyStat(user, PlayerStats.StatType.AmmoCapacityMultiplier, 1.05f, StatModifier.ModifyMethod.MULTIPLICATIVE);
                }
                if (bighead == 8)
                {
                    header = "Coolness up.";
                    text = "Lucky you!";
                    this.ApplyStat(user, PlayerStats.StatType.Coolness, 3f, StatModifier.ModifyMethod.ADDITIVE);
                }
                if (bighead == 9)
                {
                    header = "Armour up.";
                    text = "Lucky you!";
                    LastOwner.healthHaver.Armor = LastOwner.healthHaver.Armor + 1;

                }
                if (bighead == 10)
                {
                    header = "Money up.";
                    text = "Lucky you!";
                    LastOwner.carriedConsumables.Currency = LastOwner.carriedConsumables.Currency + 15;
                }
                if (bighead == 11)
                {
                    header = "Damage.";
                    text = "Unlucky.";
                    LastOwner.healthHaver.ApplyDamage(1, Vector2.zero, "Chaotic Forces", CoreDamageTypes.None, DamageCategory.Normal, true, null, false);

                }
                if (bighead == 12)
                {
                    header = "Keys up.";
                    text = "Lucky you!";
                    LastOwner.carriedConsumables.KeyBullets = LastOwner.carriedConsumables.KeyBullets + 1;
                }
                if (bighead == 13)
                {
                    header = "Mapped out.";
                    text = "Lucky you!";
                    LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(137).gameObject, LastOwner);
                }
                if (bighead == 14)
                {
                    header = "Lead Embrace.";
                    text = "Unlucky.";
                    AIActor orLoadByGuid = EnemyDatabase.GetOrLoadByGuid("cd4a4b7f612a4ba9a720b9f97c52f38c");
                    IntVector2? intVector = new IntVector2?(base.LastOwner.CurrentRoom.GetRandomVisibleClearSpot(2, 2));

                    AIActor aiactor = AIActor.Spawn(orLoadByGuid.aiActor, intVector.Value, GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(intVector.Value), true, AIActor.AwakenAnimationType.Default, true);
                }
                if (bighead == 15)
                {
                    header = "Glass blessing.";
                    text = "Lucky you!";
                    LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(565).gameObject, LastOwner);

                }
                if (bighead == 16)
                {
                    header = "Green fire.";
                    text = "Lucky you!";
                    DeadlyDeadlyGoopManager goopManagerForGoopType = DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(EasyGoopDefinitions.GreenFireDef);
                    goopManagerForGoopType.TimedAddGoopCircle(LastOwner.sprite.WorldCenter, 5f, 0.35f, false);
                }
                if (bighead == 17)
                {
                    header = "Poison!";
                    text = "Unlucky.";
                    DeadlyDeadlyGoopManager goopManagerForGoopType = DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(EasyGoopDefinitions.PoisonDef);
                    goopManagerForGoopType.TimedAddGoopCircle(LastOwner.sprite.WorldCenter, 5f, 0.35f, false);

                }
                if (bighead == 18)
                {
                    header = "Fire!";
                    text = "Unlucky.";
                    DeadlyDeadlyGoopManager goopManagerForGoopType = DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(EasyGoopDefinitions.FireDef);
                    goopManagerForGoopType.TimedAddGoopCircle(LastOwner.sprite.WorldCenter, 5f, 0.35f, false);
                }
                this.Notify(header, text);
            }
        }
        protected override void OnPreDrop(PlayerController user)
        {

        }

        private void Notify(string header, string text)
        {
            tk2dSpriteCollectionData encounterIconCollection = AmmonomiconController.Instance.EncounterIconCollection;
            int spriteIdByName = encounterIconCollection.GetSpriteIdByName("CakeMod/Resources/StrangePotion");
            GameUIRoot.Instance.notificationController.DoCustomNotification(header, text, null, spriteIdByName, UINotificationController.NotificationColor.PURPLE, false, true);
        }

        //Disable or enable the active whenever you need!
        public override bool CanBeUsed(PlayerController user)
        {
            
            return base.CanBeUsed(user);

        }


        public float OrigX;
        public float OrigY = 0;
        private Shader m_glintShader = ShaderCache.Acquire("Brave/Internal/lootGlint");

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

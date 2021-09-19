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
    class ToxicArmour : PassiveItem
    {

        public static void Init()
        {
            string itemName = "Toxic Armour";
            string resourceName = "CakeMod/Resources/ToxicArmour";

            GameObject obj = new GameObject(itemName);
            var item = obj.AddComponent<ToxicArmour>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Noxious";
            string longDesc = "A foul shield twisted by elemental forces.\n\n" +
                "Grants you a poisonous touch while armoured.";
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cak");
            item.quality = PickupObject.ItemQuality.D;
        }
        private void PoisonEnemy()
        {

        }
        public void DoBigPoison(PlayerController player)
        {
            if (player.healthHaver.Armor > 0)
            {
                DeadlyDeadlyGoopManager goopManagerForGoopType = DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(EasyGoopDefinitions.PoisonDef);
                goopManagerForGoopType.AddGoopCircle(this.m_owner.specRigidbody.UnitCenter, 1f, -4, false, -4);
                RoomHandler absoluteRoom = base.transform.position.GetAbsoluteRoom();
                AIActor randomActiveEnemy = absoluteRoom.GetRandomActiveEnemy(false);
                randomActiveEnemy.ApplyEffect(Gungeon.Game.Items["irradiated_lead"].GetComponent<BulletStatusEffectItem>().HealthModifierEffect);
            }
        }
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.OnReceivedDamage += DoBigPoison;
            this.m_poisonImmunity = new DamageTypeModifier();
            this.m_poisonImmunity.damageMultiplier = 0f;
            this.m_poisonImmunity.damageType = CoreDamageTypes.Poison;
            Owner.healthHaver.damageTypeModifiers.Add(this.m_poisonImmunity);
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
            SpeculativeRigidbody specRigidbody = player.specRigidbody;
            specRigidbody.OnRigidbodyCollision += HandleRigidbodyCollision;
            if (uwu == true)
            {
                player.healthHaver.Armor = player.healthHaver.Armor + 1;
                uwu = false;
            }
        }

        private void HandleRigidbodyCollision(CollisionData rigidbodyCollision)
        {

            if (Owner.healthHaver.Armor > 0)
            {
                if (Owner && rigidbodyCollision.OtherRigidbody)
                {
                    AIActor aiActor = rigidbodyCollision.OtherRigidbody.aiActor;
                    if (aiActor != null && aiActor.healthHaver.CanCurrentlyBeKilled)
                    {
                        aiActor.ApplyEffect(Gungeon.Game.Items["irradiated_lead"].GetComponent<BulletStatusEffectItem>().HealthModifierEffect);
                    }
                }
            }
        }

        public override DebrisObject Drop(PlayerController player)
        {
            LiveAmmoItem liveammo = PickupObjectDatabase.GetById(414).GetComponent<LiveAmmoItem>();
            Owner.healthHaver.damageTypeModifiers.Remove(this.m_poisonImmunity);
            if (PassiveItem.ActiveFlagItems[player].ContainsKey(liveammo.GetType()))
            {
                PassiveItem.ActiveFlagItems[player][liveammo.GetType()] = Mathf.Max(0, PassiveItem.ActiveFlagItems[player][liveammo.GetType()] - 1);
                if (PassiveItem.ActiveFlagItems[player][liveammo.GetType()] == 0)
                {
                    PassiveItem.ActiveFlagItems[player].Remove(liveammo.GetType());
                }
            }
            SpeculativeRigidbody specRigidbody = player.specRigidbody;
            specRigidbody.OnRigidbodyCollision -= HandleRigidbodyCollision;
            return base.Drop(player);

        }
        public bool uwu = true;
        private DamageTypeModifier m_poisonImmunity;
    }
}

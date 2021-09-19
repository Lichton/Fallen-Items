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
    class FrozenFrog : PassiveItem
    {

        public static void Init()
        {
            string itemName = "Frozen Frog";
            string resourceName = "CakeMod/Resources/frozenfrog";

            GameObject obj = new GameObject(itemName);
            var item = obj.AddComponent<FrozenFrog>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Thawed";
            string longDesc = "The result of an angered frog god and a moronic pastime.\n\n" +
                "The crimes of this individual are immortalized.";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cak");

            item.quality = PickupObject.ItemQuality.B;

        }

        private void Freeze()
        {
            base.StartCoroutine(this.StartCooldown());
        }

        private IEnumerator StartCooldown()
        {
            yield return new WaitForSeconds(1f);
            RoomHandler absoluteRoom = base.transform.position.GetAbsoluteRoom();
            AIActor randomActiveEnemy = absoluteRoom.GetRandomActiveEnemy(false);
            randomActiveEnemy.aiActor.ApplyEffect(freezeModifierEffect, 5f, null);
            yield break;
        }
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.OnEnteredCombat += this.Freeze;
        }

        public override DebrisObject Drop(PlayerController player)
        {
            player.OnEnteredCombat -= this.Freeze;
            return base.Drop(player);
        }
        public static GameActorFreezeEffect freezeModifierEffect = PickupObjectDatabase.GetById(278).GetComponent<BulletStatusEffectItem>().FreezeModifierEffect;
    }
}

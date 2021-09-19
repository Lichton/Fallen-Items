using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;
using Gungeon;
using SaveAPI;


namespace CakeMod
{
    public class StrangeEffigy : PassiveItem
    {
        private float random;

        public static void Init()
        {
            string itemName = "Top Hat Effigy";
            string resourceName = "CakeMod/Resources/TopHatEffigy";
            GameObject obj = new GameObject(itemName);
            var item = obj.AddComponent<StrangeEffigy>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
            string shortDesc = "Top Hat Clan";
            string longDesc = "This effigy radiates COOL energy.\n\n" +
                "Enemies killed come back COOLER then before.";
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cak");
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Coolness, 2f, StatModifier.ModifyMethod.ADDITIVE);
            item.quality = PickupObject.ItemQuality.A;
            item.SetupUnlockOnCustomFlag(CustomDungeonFlags.HAT_UNLOCK, true);
        }
        private void OnEnemyDamaged(float damage, bool fatal, HealthHaver enemyHealth)
        {
            this.random = UnityEngine.Random.Range(0.0f, 1.0f);
            if (random <= 0.16f)
            {
                if (enemyHealth.aiActor.EnemyGuid != "5861e5a077244905a8c25c2b7b4d6ebb")
                {
                    bool flag = enemyHealth.aiActor && fatal;
                    if (flag)
                    {
                        this.tophat(enemyHealth.sprite.WorldCenter);
                    }
                }
                else
                {
                    AkSoundEngine.PostEvent("Play_ENM_Hurt", base.gameObject);
                }
            }
        }

        public void tophat(Vector3 position)
        {
            string guid;
            guid = "5861e5a077244905a8c25c2b7b4d6ebb";
            if (Owner.PlayerHasActiveSynergy("Hat's Off to You"))

            {
                guid = "ddf12a4881eb43cfba04f36dd6377abb";
            }
            if (Owner.HasPickupID(Game.Items["cak:hat_shells"].PickupObjectId))
            {

            }

            PlayerController owner = base.Owner;
            AIActor orLoadByGuid = EnemyDatabase.GetOrLoadByGuid(guid);
            IntVector2? intVector = new IntVector2?(base.Owner.CurrentRoom.GetRandomVisibleClearSpot(2, 2));
            AIActor aiactor = AIActor.Spawn(orLoadByGuid.aiActor, intVector.Value, GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(intVector.Value), true, AIActor.AwakenAnimationType.Default, true);
            aiactor.CanTargetEnemies = true;
            aiactor.CanTargetPlayers = false;
            PhysicsEngine.Instance.RegisterOverlappingGhostCollisionExceptions(aiactor.specRigidbody, null, false);
            aiactor.gameObject.AddComponent<KillOnRoomClear>();
            aiactor.IsHarmlessEnemy = true;
            aiactor.IgnoreForRoomClear = true;
            aiactor.HandleReinforcementFallIntoRoom(0f);
        }
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.OnAnyEnemyReceivedDamage = (Action<float, bool, HealthHaver>)Delegate.Combine(player.OnAnyEnemyReceivedDamage, new Action<float, bool, HealthHaver>(this.OnEnemyDamaged));
        }
    }
}

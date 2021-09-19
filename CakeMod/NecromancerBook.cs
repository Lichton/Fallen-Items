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
    class NecromancerBook : PassiveItem
    {
        private float random;

        public static void Init()
        {
            string itemName = "Ammomancer's Book";
            string resourceName = "CakeMod/Resources/NecromancerBook";

            GameObject obj = new GameObject(itemName);
            var item = obj.AddComponent<NecromancerBook>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Necronomicanned";
            string longDesc = "A book detailing dark rituals of ammomancy.\n\n" +
                "Necromancers all over the world use this book to learn their tricks. Doesn't work on ghosts.\n" +
                "The first page details Ammomancy 101. 'Thine reviveth yourst enemies ast allies.'";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cak");
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Curse, 2);

            item.quality = PickupObject.ItemQuality.EXCLUDED;

        }
        
        public void tophat(PlayerController player, HealthHaver enemy)
        {
            int bighead = UnityEngine.Random.Range(1, 6);
            if (bighead == 1)
            {
                if (!enemy.healthHaver.IsBoss)
                {
                    string guid;
                    guid = enemy.aiActor.EnemyGuid;
                    if (guid != "4db03291a12144d69fe940d5a01de376")
                    {

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
                }
            }
        }
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.OnKilledEnemyContext += this.tophat;
        }

        public override DebrisObject Drop(PlayerController player)
        {
            player.OnKilledEnemyContext -= this.tophat;
            return base.Drop(player);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Gungeon;

namespace CakeMod
{
    public class AdvancedRatRewards : ETGModule
    {
        public override void Exit()
        {
        }
        public override void Start()
        {
        }
        public override void Init()
        {
            ETGMod.AIActor.OnPreStart += this.AIActorMods;
        }
        private void AIActorMods(AIActor enemy)
        {
            if (enemy != null && enemy.aiActor != null)
            {
                if (enemy.aiActor.EnemyGuid == "4d164ba3f62648809a4a82c90fc22cae")
                {
                    enemy.healthHaver.OnDeath += this.handleTheRewards;
                }
            }
        }

        private void handleTheRewards(Vector2 direction)
        {
            PlayerController player = GameManager.Instance.PrimaryPlayer;
            if (player.HasTakenDamageThisFloor)
            {
                
            }
            else
            {
                LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(CakeIDs.MasterRat).gameObject, player);
            }
        
        }
    }
}
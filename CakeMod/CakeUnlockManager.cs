using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Gungeon;
using SaveAPI;

namespace CakeMod
{
    public class CakeUnlockManager : ETGModule
    {
        public override void Exit()
        {
        }
        public override void Start()
        {
        }
        public override void Init()
        {
            ETGMod.AIActor.OnPreStart += this.orange;
            ETGMod.AIActor.OnPreStart += this.orange2;
        }
        private void orange(AIActor enemy)
        {
            if (enemy != null && enemy.aiActor != null)
            {
                if (enemy.aiActor.EnemyGuid == "5861e5a077244905a8c25c2b7b4d6ebb")
                {
                    if (enemy.healthHaver.IsDead)
                    {
                        SaveAPIManager.SetFlag(CustomDungeonFlags.HAT_UNLOCK, true);
                    }
                }
                if (enemy.aiActor.EnemyGuid == "7c5d5f09911e49b78ae644d2b50ff3bf")
                {
                    if (enemy.healthHaver.IsDead)
                    {
                        if((GameManager.Instance.PrimaryPlayer.HasPickupID(ETGMod.Databases.Items["Chest Devil's Contract"].PickupObjectId)))
                        {
                            SaveAPIManager.SetFlag(CustomDungeonFlags.DEVILMODE_LICH_UNLOCK, true);
                        }
                    }
                }
            }
        }

        private void orange2(AIActor enemy)
        {
            if (enemy != null && enemy.aiActor != null)
            {
                if (enemy.aiActor.EnemyGuid == "19b420dec96d4e9ea4aebc3398c0ba7a")
                {
                    if (enemy.healthHaver.IsDead)
                    {
                        SaveAPIManager.SetFlag(CustomDungeonFlags.SCREAN_UNLOCK, true);
                    }
                }
            }
        }
    }
}
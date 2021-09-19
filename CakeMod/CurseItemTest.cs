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
    class CurseItemTest : PassiveItem
    {
        public static void Init()
        {
            List<PlayerController> m_cursedPlayers = new List<PlayerController>();
            string itemName = "CurseTestItem";
            string resourceName = "CakeMod/Resources/CorruptedSoul";

            GameObject obj = new GameObject(itemName);
            var item = obj.AddComponent<CurseItemTest>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Corrupted Soul";
            string longDesc = "INSTANTIATING AMMONOMICON?!!!\n\n" +
                "Talk To Brent\n" +
                "compile error";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cak");

            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.EnemyProjectileSpeedMultiplier, 0.75f, StatModifier.ModifyMethod.MULTIPLICATIVE);
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Curse, -2);

            item.quality = PickupObject.ItemQuality.EXCLUDED;

        }
        protected override void Update()
        {
            List<PlayerController> m_cursedPlayers = new List<PlayerController>();
            for (int i = 0; i < this.m_cursedPlayers.Count; i++)
            {
                this.DoCurse(this.m_cursedPlayers[i]);
            }
        }
  
    public override void Pickup(PlayerController player)
        {
            List<PlayerController> m_cursedPlayers = new List<PlayerController>();
            base.Pickup(player);
            PlayerController playerController = player.gameActor as PlayerController;
            playerController.CurseIsDecaying = true;
            this.m_cursedPlayers.Add(playerController);
            player.CurseIsDecaying = true;
        }

        private void DoCurse(PlayerController targetPlayer)
        {
            List<PlayerController> m_cursedPlayers = new List<PlayerController>();
            if (targetPlayer.IsGhost)
            {
                return;
            }
            targetPlayer.CurrentCurseMeterValue += BraveTime.DeltaTime / this.TimeToCursePoint;
            targetPlayer.CurseIsDecaying = false;
            if (targetPlayer.CurrentCurseMeterValue > 1f)
            {
                targetPlayer.CurrentCurseMeterValue = 0f;
                StatModifier statModifier = new StatModifier();
                statModifier.amount = 1f;
                statModifier.modifyType = StatModifier.ModifyMethod.ADDITIVE;
                statModifier.statToBoost = PlayerStats.StatType.Curse;
                targetPlayer.ownerlessStatModifiers.Add(statModifier);
                targetPlayer.stats.RecalculateStats(targetPlayer, false, false);
                base.minorBreakable.Break();
            }
        }
        public override DebrisObject Drop(PlayerController player)
        {
            return base.Drop(player);
        }

      
        public float TimeToCursePoint = 3f;

        private List<PlayerController> m_cursedPlayers;

    }
}

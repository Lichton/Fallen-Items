using System;
using System.Collections.Generic;
using ItemAPI;
using UnityEngine;

namespace CakeMod
{
	// Token: 0x02000085 RID: 133
	public class FleshCake : PassiveItem
	{
		// Token: 0x06000363 RID: 867 RVA: 0x0001B64C File Offset: 0x0001984C
		public static void Init()
		{
			string name = "Lead Cake";
			string resourcePath = "CakeMod/Resources/Lead_Cake";
			GameObject gameObject = new GameObject(name);
			FleshCake kevin = gameObject.AddComponent<FleshCake>();
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject);
			string shortDesc = "Pb 82";
			string longDesc = "This treat looks remarkably like a Lead Cube. Other lead cubes side with you thinking you are a friend.";
			kevin.SetupItem(shortDesc, longDesc, "cak");
			kevin.quality = PickupObject.ItemQuality.D;
			kevin.AddToSubShop(ItemBuilder.ShopType.Cursula, 1f);
			ItemBuilder.AddPassiveStatModifier(kevin, PlayerStats.StatType.Curse, 1);
			List<string> mandatoryConsoleIDs = new List<string>
			{
				"cak:lead_cake",
				"potion_of_lead_skin"
			};
			CustomSynergies.Add("Lead-er", mandatoryConsoleIDs, null, true);
		}

		// Token: 0x06000364 RID: 868 RVA: 0x0001B6E0 File Offset: 0x000198E0
		private void SpawnFlesh()
		{
			bool flag = base.Owner.HasPickupID(64);
			string guid;
			guid = "33b212b856b74ff09252bf4f2e8b8c57";
			PlayerController owner = base.Owner;
			AIActor orLoadByGuid = EnemyDatabase.GetOrLoadByGuid(guid);
			IntVector2? intVector = new IntVector2?(base.Owner.CurrentRoom.GetRandomVisibleClearSpot(2, 2));
			AIActor aiactor = AIActor.Spawn(orLoadByGuid.aiActor, intVector.Value, GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(intVector.Value), true, AIActor.AwakenAnimationType.Default, true);
			aiactor.CanTargetEnemies = true;
			aiactor.CanTargetPlayers = false;
			aiactor.HitByEnemyBullets = true;
			PhysicsEngine.Instance.RegisterOverlappingGhostCollisionExceptions(aiactor.specRigidbody, null, false);
			aiactor.gameObject.AddComponent<KillOnRoomClear>();
			aiactor.IsHarmlessEnemy = true;
			aiActor.CompanionOwner = this.m_owner;
			aiactor.CanDropCurrency = false;
			aiactor.IgnoreForRoomClear = true;
			aiactor.HandleReinforcementFallIntoRoom(0f);
		}

		// Token: 0x06000365 RID: 869 RVA: 0x0001B7C1 File Offset: 0x000199C1
		public override void Pickup(PlayerController player)
		{
			base.Pickup(player);
			PlayerController owner = base.Owner;
			owner.OnEnteredCombat = (Action)Delegate.Combine(owner.OnEnteredCombat, new Action(this.SpawnFlesh));
		}

		// Token: 0x06000366 RID: 870 RVA: 0x0001B7F4 File Offset: 0x000199F4
		public override DebrisObject Drop(PlayerController player)
		{
			DebrisObject result = base.Drop(player);
			PlayerController owner = base.Owner;
			owner.OnEnteredCombat += this.SpawnFlesh;
			return result;
		}

		// Token: 0x06000367 RID: 871 RVA: 0x0001B836 File Offset: 0x00019A36
		protected override void OnDestroy()
		{
			PlayerController owner = base.Owner;
			owner.OnEnteredCombat -= this.SpawnFlesh;
			base.OnDestroy();
		}

		// Token: 0x040000DE RID: 222
		public GameActorCharmEffect charmEffect;
	}
}

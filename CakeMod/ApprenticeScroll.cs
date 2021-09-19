using System;
using System.Collections.Generic;
using ItemAPI;
using UnityEngine;

namespace CakeMod
{
	// Token: 0x02000085 RID: 133
	public class ApprenticeScroll : PassiveItem
	{
		// Token: 0x06000363 RID: 867 RVA: 0x0001B64C File Offset: 0x0001984C
		public static void Init()
		{
			string name = "Apprentice Scroll";
			string resourcePath = "CakeMod/Resources/ApprenticeScroll";
			GameObject gameObject = new GameObject(name);
			ApprenticeScroll kevin = gameObject.AddComponent<ApprenticeScroll>();
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject);
			string shortDesc = "Runic, not Tunic";
			string longDesc = "A worn scroll written in ink. It seems to have numerous damp spots where water has stained it. The apprentice who wrote it seeks approval, if not from it's superiors.";
			kevin.SetupItem(shortDesc, longDesc, "cak");
			kevin.quality = PickupObject.ItemQuality.B;
		}

		// Token: 0x06000364 RID: 868 RVA: 0x0001B6E0 File Offset: 0x000198E0
		private void SpawnFlesh()
		{
			
			string guid;
			
			
				guid = "206405acad4d4c33aac6717d184dc8d4";
		   
			PlayerController owner = base.Owner;
			AIActor orLoadByGuid = EnemyDatabase.GetOrLoadByGuid(guid);
			IntVector2? intVector = new IntVector2?(base.Owner.CurrentRoom.GetRandomVisibleClearSpot(2, 2));
			AIActor aiactor = AIActor.Spawn(orLoadByGuid.aiActor, intVector.Value, GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(intVector.Value), true, AIActor.AwakenAnimationType.Default, true);
			aiactor.CanTargetEnemies = true;
			aiactor.CanTargetPlayers = false;
			PhysicsEngine.Instance.RegisterOverlappingGhostCollisionExceptions(aiactor.specRigidbody, null, false);
			aiactor.gameObject.AddComponent<KillOnRoomClear>();
			aiactor.CanDropCurrency = false;
			aiactor.HitByEnemyBullets = false;
			aiactor.CollisionDamage = 0f;
			aiactor.IsHarmlessEnemy = true;
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
			owner.OnEnteredCombat = (Action)Delegate.Remove(owner.OnEnteredCombat, new Action(this.SpawnFlesh));
			return result;
		}

		// Token: 0x06000367 RID: 871 RVA: 0x0001B836 File Offset: 0x00019A36
		protected override void OnDestroy()
		{
			PlayerController owner = base.Owner;
			owner.OnEnteredCombat = (Action)Delegate.Remove(owner.OnEnteredCombat, new Action(this.SpawnFlesh));
			base.OnDestroy();
		}

		// Token: 0x040000DE RID: 222
		public GameActorCharmEffect charmEffect;
	}
}
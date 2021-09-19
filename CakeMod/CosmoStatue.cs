using System;
using System.Collections;
using System.Collections.Generic;
using Dungeonator;
using ItemAPI;
using UnityEngine;

namespace CakeMod
{
	// Token: 0x020000FC RID: 252
	public class CosmoStatue : CompanionItem
	{
		// Token: 0x06000620 RID: 1
		private IEnumerator Start()
        {
			RoomHandler room = GameManager.Instance.Dungeon.GetRoomFromPosition(this.transform.position.IntXY(VectorConversions.Floor));
			CompanionController companionController = CosmoStatue.prefab.AddComponent<CompanionController>();
			while (companionController.healthHaver.IsAlive)
			{
				this.AttractEnemies(room);
				
			}       
			yield break;
		}
			public static void CosmoBuildPrefab()
		{
			bool flag = CosmoStatue.prefab != null || CompanionBuilder.companionDictionary.ContainsKey(CosmoStatue.guidcosmo);
			bool flag2 = !flag;
			if (flag2)
			{

				CosmoStatue.prefab = CompanionBuilder.BuildPrefab("cosmoman", CosmoStatue.guidcosmo, "CakeMod/Resources/cosmo_idle_001", new IntVector2(1, 0), new IntVector2(9, 9));
				CompanionController companionController = CosmoStatue.prefab.AddComponent<CompanionController>();
				companionController.aiActor.MovementSpeed = 0f;
				CosmoStatue.prefab.AddAnimation("idle_right", "CakeMod/Resources/cosmo_idle_001", 1, CompanionBuilder.AnimationType.Idle, DirectionalAnimation.DirectionType.TwoWayHorizontal, DirectionalAnimation.FlipType.None);
				CosmoStatue.prefab.AddAnimation("idle_left", "CakeMod/Resources/cosmo_idle_001", 1, CompanionBuilder.AnimationType.Idle, DirectionalAnimation.DirectionType.TwoWayHorizontal, DirectionalAnimation.FlipType.None);
				companionController.CanInterceptBullets = true;
				companionController.aiActor.healthHaver.PreventAllDamage = false;
				companionController.aiActor.specRigidbody.CollideWithOthers = true;
				companionController.aiActor.specRigidbody.CollideWithTileMap = true;
				companionController.aiActor.specRigidbody.CollidedX = true;
				companionController.aiActor.specRigidbody.CollidedY = true;
				companionController.aiActor.IsWorthShootingAt = true;
				companionController.aiActor.CollisionDamage = 0f;
				companionController.aiActor.HitByEnemyBullets = true;
				companionController.aiActor.PreventAutoKillOnBossDeath = true;
				companionController.aiActor.healthHaver.ForceSetCurrentHealth(50f);
				companionController.aiActor.healthHaver.SetHealthMaximum(50f, null, false);
				companionController.aiActor.OverrideTarget = companionController.aiActor.specRigidbody;
				
				companionController.aiActor.specRigidbody.PixelColliders.Clear();
				companionController.aiActor.specRigidbody.PixelColliders.Add(new PixelCollider
				
					{
					ColliderGenerationMode = PixelCollider.PixelColliderGeneration.Manual,
					CollisionLayer = CollisionLayer.BulletBlocker,
					IsTrigger = false,
					BagleUseFirstFrameOnly = false,
					SpecifyBagelFrame = string.Empty,
					BagelColliderNumber = 0,
					ManualOffsetX = 0,
					ManualOffsetY = 0,
					ManualWidth = 17,
					ManualHeight = 24,
					ManualDiameter = 0,
					ManualLeftX = 0,
					ManualLeftY = 0,
					ManualRightX = 0,
					ManualRightY = 0
				});
				companionController.aiActor.specRigidbody.PixelColliders.Add(new PixelCollider
				{
					ColliderGenerationMode = PixelCollider.PixelColliderGeneration.Manual,
					CollisionLayer = CollisionLayer.BeamBlocker,
					IsTrigger = false,
					BagleUseFirstFrameOnly = false,
					SpecifyBagelFrame = string.Empty,
					BagelColliderNumber = 0,
					ManualOffsetX = 0,
					ManualOffsetY = 0,
					ManualWidth = 17,
					ManualHeight = 24,
					ManualDiameter = 0,
					ManualLeftX = 0,
					ManualLeftY = 0,
					ManualRightX = 0,
					ManualRightY = 0
				});
				companionController.aiActor.specRigidbody.PixelColliders.Add(new PixelCollider
				{
					ColliderGenerationMode = PixelCollider.PixelColliderGeneration.Manual,
					CollisionLayer = CollisionLayer.EnemyBulletBlocker,
					IsTrigger = false,
					BagleUseFirstFrameOnly = false,
					SpecifyBagelFrame = string.Empty,
					BagelColliderNumber = 0,
					ManualOffsetX = 0,
					ManualOffsetY = 0,
					ManualWidth = 17,
					ManualHeight = 24,
					ManualDiameter = 0,
					ManualLeftX = 0,
					ManualLeftY = 0,
					ManualRightX = 0,
					ManualRightY = 0
				});


			}
		}

		private void ClearOverrides(RoomHandler room)
		{
			List<AIActor> activeEnemies = room.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
			if (activeEnemies != null)
			{
				for (int i = 0; i < activeEnemies.Count; i++)
				{
					if (activeEnemies[i].OverrideTarget == base.specRigidbody)
					{
						activeEnemies[i].OverrideTarget = null;
					}
				}
			}
		}

		private void AttractEnemies(RoomHandler room)
		{
			List<AIActor> activeEnemies = room.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
			if (activeEnemies != null)
			{
				for (int i = 0; i < activeEnemies.Count; i++)
				{
					if (activeEnemies[i].OverrideTarget == null)
					{
						activeEnemies[i].OverrideTarget = base.specRigidbody;
					}
				}
			}
		}


		// Token: 0x04000283 RID: 643
		public static GameObject prefab;

		// Token: 0x04000284 RID: 644
		public static string guidcosmo = "cosmonautisgod";

		private bool m_revealed;


	}
}

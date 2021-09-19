using System;
using UnityEngine;
using ItemAPI;

namespace CakeMod
{
	public class ghost_statusDebuffEffect : GameActorHealthEffect
	{
		public static string vfxNameghost_status = "ghost_statusVFX";
		public static GameObject ghost_statusVFXObject;
		public Color ClearUp = new Color(0f, 0f, 0f, 0f);
		public static void Init()
		{
			ghost_statusVFXObject = SpriteBuilder.SpriteFromResource("CakeMod/Resources/skull_mark", new GameObject("ghost_statusIcon"));
			ghost_statusVFXObject.SetActive(false);
			tk2dBaseSprite vfxSprite = ghost_statusVFXObject.GetComponent<tk2dBaseSprite>();
			vfxSprite.GetCurrentSpriteDef().ConstructOffsetsFromAnchor(tk2dBaseSprite.Anchor.LowerCenter, vfxSprite.GetCurrentSpriteDef().position3);
			FakePrefab.MarkAsFakePrefab(ghost_statusVFXObject);
			UnityEngine.Object.DontDestroyOnLoad(ghost_statusVFXObject);
		}


		public override void ApplyTint(GameActor actor)
		{
			for (int k = 0; k < 1; k++)
			{
				GameObject original;
				original = ghost_statusDebuffEffect.ghost_statusVFXObject;
				tk2dSprite component = UnityEngine.Object.Instantiate<GameObject>(original, actor.transform).GetComponent<tk2dSprite>();
				component.name = ghost_statusDebuffEffect.vfxNameghost_status;
				component.PlaceAtPositionByAnchor(actor.sprite.WorldTopCenter, tk2dBaseSprite.Anchor.LowerCenter);
				component.scale = Vector3.one;
			}
		}
		public override void EffectTick(GameActor actor, RuntimeGameActorEffectData effectData)
		{

			if (this.AffectsEnemies && actor is AIActor && !actor.healthHaver.IsBoss)
			{

				if (actor.healthHaver.IsDead)
				{
					if (ShouldISpawn == true)
					{
						if (actor.aiActor.EnemyGuid != "5861e5a077244905a8c25c2b7b4d6ebb")
						{
							string guid;
							guid = "4db03291a12144d69fe940d5a01de376";
							AIActor orLoadByGuid = EnemyDatabase.GetOrLoadByGuid(guid);
							IntVector2? intVector = new IntVector2?(actor.PlacedPosition);
							AIActor aiactor = AIActor.Spawn(orLoadByGuid.aiActor, actor.CenterPosition, GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(intVector.Value), true, AIActor.AwakenAnimationType.Default, true);
							aiactor.CanTargetEnemies = true;
							aiactor.CanTargetPlayers = false;
							PhysicsEngine.Instance.RegisterOverlappingGhostCollisionExceptions(aiactor.specRigidbody, null, false);
							aiactor.gameObject.AddComponent<KillOnRoomClear>();
							aiactor.CanDropCurrency = false;
							aiactor.HitByEnemyBullets = false;
							aiactor.CollisionDamage = 0f;
							aiactor.IsHarmlessEnemy = true;
							aiactor.IgnoreForRoomClear = true;
							ShouldISpawn = false;
						}
					}
				}
			}
		}
		public Action<Vector2> ghost_statusMark(GameActor actor)
		{
			LootEngine.SpawnItem(PickupObjectDatabase.GetById(297).gameObject, actor.specRigidbody.UnitCenter, actor.specRigidbody.UnitCenter, 1f, false, true, false);
			throw new NotImplementedException();
		}

		public override void OnEffectApplied(GameActor actor, RuntimeGameActorEffectData effectData, float partialAmount = 1)
		{
			actor.healthHaver.OnDeath += effectData.OnActorPreDeath;
			base.OnEffectApplied(actor, effectData, partialAmount);
			ShouldISpawn = true;
		}

		public override void OnEffectRemoved(GameActor actor, RuntimeGameActorEffectData effectData)
		{
			var hand = actor.transform.Find("ghost_statusVFX").gameObject;
			UnityEngine.Object.Destroy(hand);

			actor.DeregisterOverrideColor(vfxNameghost_status);
			base.OnEffectRemoved(actor, effectData);
		}
		public bool ShouldISpawn = true;
	}

}
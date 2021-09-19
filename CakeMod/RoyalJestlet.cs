
using System;
using System.Collections.Generic;
using Gungeon;
using ItemAPI;
using UnityEngine;
using DirectionType = DirectionalAnimation.DirectionType;
using AnimationType = ItemAPI.EnemyBuilder.AnimationType;
using System.Collections;
using Dungeonator;
using System.Linq;
using Brave.BulletScript;

namespace CakeMod
{
	public class RoyalJesterlet : AIActor
	{
		public static GameObject prefab;
		public static readonly string guid = "RoyalJesterlet";
		private static tk2dSpriteCollectionData RoyalJesterletCollection;
		public static GameObject shootpoint;

		public static void Init()
		{

			RoyalJesterlet.BuildPrefab();
		}

		public static void BuildPrefab()
		{
			// source = EnemyDatabase.GetOrLoadByGuid("c50a862d19fc4d30baeba54795e8cb93");
			bool flag = prefab != null || EnemyBuilder.Dictionary.ContainsKey(guid);
			bool flag2 = flag;
			if (!flag2)
			{
				prefab = EnemyBuilder.BuildPrefab("RoyalJesterlet", guid, spritePaths[0], new IntVector2(0, 0), new IntVector2(8, 9), false);
				var companion = prefab.AddComponent<EnemyBehavior>();
				companion.aiActor.knockbackDoer.weight = 200;
				companion.aiActor.MovementSpeed = 2f;
				companion.aiActor.healthHaver.PreventAllDamage = false;
				companion.aiActor.CollisionDamage = 1f;
				companion.aiActor.HasShadow = false;
				companion.aiActor.IgnoreForRoomClear = false;
				companion.aiActor.aiAnimator.HitReactChance = 0.05f;
				companion.aiActor.specRigidbody.CollideWithOthers = true;
				companion.aiActor.specRigidbody.CollideWithTileMap = true;
				companion.aiActor.PreventFallingInPitsEver = true;
				companion.aiActor.healthHaver.ForceSetCurrentHealth(15f);
				companion.aiActor.CollisionKnockbackStrength = 5f;
				companion.aiActor.CanTargetPlayers = true;
				companion.aiActor.healthHaver.SetHealthMaximum(15f, null, false);
				companion.aiActor.specRigidbody.PixelColliders.Clear();
				companion.aiActor.specRigidbody.PixelColliders.Add(new PixelCollider
				{
					ColliderGenerationMode = PixelCollider.PixelColliderGeneration.Manual,
					CollisionLayer = CollisionLayer.EnemyCollider,
					IsTrigger = false,
					BagleUseFirstFrameOnly = false,
					SpecifyBagelFrame = string.Empty,
					BagelColliderNumber = 0,
					ManualOffsetX = 0,
					ManualOffsetY = 0,
					ManualWidth = 12,
					ManualHeight = 26,
					ManualDiameter = 0,
					ManualLeftX = 0,
					ManualLeftY = 0,
					ManualRightX = 0,
					ManualRightY = 0
				});
				companion.aiActor.specRigidbody.PixelColliders.Add(new PixelCollider
				{

					ColliderGenerationMode = PixelCollider.PixelColliderGeneration.Manual,
					CollisionLayer = CollisionLayer.EnemyHitBox,
					IsTrigger = false,
					BagleUseFirstFrameOnly = false,
					SpecifyBagelFrame = string.Empty,
					BagelColliderNumber = 0,
					ManualOffsetX = 0,
					ManualOffsetY = 0,
					ManualWidth = 12,
					ManualHeight = 26,
					ManualDiameter = 0,
					ManualLeftX = 0,
					ManualLeftY = 0,
					ManualRightX = 0,
					ManualRightY = 0,



				});
				companion.aiActor.CorpseObject = EnemyDatabase.GetOrLoadByGuid("c0ff3744760c4a2eb0bb52ac162056e6").CorpseObject;
				companion.aiActor.PreventBlackPhantom = false;
				AIAnimator aiAnimator = companion.aiAnimator;
				aiAnimator.IdleAnimation = new DirectionalAnimation
				{
					Type = DirectionalAnimation.DirectionType.TwoWayHorizontal,
					Flipped = new DirectionalAnimation.FlipType[2],
					AnimNames = new string[]
					{
						"idle",
						"idle_two"
					}
				};
				bool flag3 = RoyalJesterletCollection == null;
				if (flag3)
				{
					RoyalJesterletCollection = SpriteBuilder.ConstructCollection(prefab, "RoyalJesterlet_Collection");
					UnityEngine.Object.DontDestroyOnLoad(RoyalJesterletCollection);
					for (int i = 0; i < spritePaths.Length; i++)
					{
						SpriteBuilder.AddSpriteToCollection(spritePaths[i], RoyalJesterletCollection);
					}
					SpriteBuilder.AddAnimation(companion.spriteAnimator, RoyalJesterletCollection, new List<int>
					{

					0,
					1,

					}, "idle", tk2dSpriteAnimationClip.WrapMode.Loop).fps = 7f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, RoyalJesterletCollection, new List<int>
					{

					2,
					3

					}, "idle_two", tk2dSpriteAnimationClip.WrapMode.Loop).fps = 7f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, RoyalJesterletCollection, new List<int>
					{

					4,
					5,
					6,
					7

					}, "prepare", tk2dSpriteAnimationClip.WrapMode.Once).fps = 4f;
				}
				var bs = prefab.GetComponent<BehaviorSpeculator>();
				BehaviorSpeculator behaviorSpeculator = EnemyDatabase.GetOrLoadByGuid("01972dee89fc4404a5c408d50007dad5").behaviorSpeculator;
				bs.OverrideBehaviors = behaviorSpeculator.OverrideBehaviors;
				bs.OtherBehaviors = behaviorSpeculator.OtherBehaviors;
				bs.TargetBehaviors = new List<TargetBehaviorBase>
			{
				new TargetPlayerBehavior
				{
					Radius = 35f,
					LineOfSight = false,
					ObjectPermanence = true,
					SearchInterval = 0.25f,
					PauseOnTargetSwitch = false,
					PauseTime = 0.25f
				}
			};
				shootpoint = new GameObject("fuck");
				shootpoint.transform.parent = companion.transform;
				shootpoint.transform.position = companion.sprite.WorldCenter;
				GameObject m_CachedGunAttachPoint = companion.transform.Find("fuck").gameObject;
				bs.InstantFirstTick = behaviorSpeculator.InstantFirstTick;
				bs.TickInterval = behaviorSpeculator.TickInterval;
				bs.PostAwakenDelay = behaviorSpeculator.PostAwakenDelay;
				bs.RemoveDelayOnReinforce = behaviorSpeculator.RemoveDelayOnReinforce;
				bs.OverrideStartingFacingDirection = behaviorSpeculator.OverrideStartingFacingDirection;
				bs.StartingFacingDirection = behaviorSpeculator.StartingFacingDirection;
				bs.SkipTimingDifferentiator = behaviorSpeculator.SkipTimingDifferentiator;
				Game.Enemies.Add("cak:royal_jestlet", companion.aiActor);

			}
		}




		private static string[] spritePaths = new string[]
		{
			
			//idles
			"CakeMod/Resources/royaljesterbullet_idle_001",
			"CakeMod/Resources/royaljesterbullet_idle_002",
			"CakeMod/Resources/royaljesterbullet_idle_two_001",
			"CakeMod/Resources/royaljesterbullet_idle_two_002",
		};

		public class EnemyBehavior : BraveBehaviour
		{
			private RoomHandler m_StartRoom;
			private void Update()
			{
				if (!base.aiActor.HasBeenEngaged) { CheckPlayerRoom(); }
			}
			private void CheckPlayerRoom()
			{

				if (GameManager.Instance.PrimaryPlayer.GetAbsoluteParentRoom() != null && GameManager.Instance.PrimaryPlayer.GetAbsoluteParentRoom() == m_StartRoom)
				{
					base.aiActor.HasBeenEngaged = true;
				}

			}
			private void Start()
			{
				m_StartRoom = aiActor.GetAbsoluteParentRoom();
				//base.aiActor.HasBeenEngaged = true;
				base.aiActor.healthHaver.OnPreDeath += (obj) =>
				{
					LootEngine.DoDefaultPurplePoof(aiActor.sprite.WorldBottomCenter);
					AkSoundEngine.PostEvent("Play_AGUNIM_VO_FIGHT_LAUGH_02", base.aiActor.gameObject);
				};
				this.aiActor.knockbackDoer.SetImmobile(true, "laugh");
			}


		}
	}
}

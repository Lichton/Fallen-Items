using System;
using System.Collections.Generic;
using Gungeon;
using ItemAPI;
using UnityEngine;
//using DirectionType = DirectionalAnimation.DirectionType;
using AnimationType = ItemAPI.EnemyBuilder.AnimationType;
using System.Collections;
using Dungeonator;
using System.Linq;
using Brave.BulletScript;

namespace CakeMod
{
	public class Turret : AIActor
	{
		public static GameObject prefab;
		public static readonly string guid = "turret_cak";
		private static tk2dSpriteCollectionData BatCollection;
		public static GameObject shootpoint;
		public static void Init()
		{
			Turret.BuildPrefab();
		}

		public static void BuildPrefab()
		{
			//
			bool flag = prefab != null || EnemyBuilder.Dictionary.ContainsKey(guid);
			bool flag2 = flag;
			if (!flag2)
			{
				//float AttackAnimationThingAMaWhatIts = 0.5f;
				prefab = EnemyBuilder.BuildPrefab("turret_cak", guid, spritePaths[0], new IntVector2(0, 0), new IntVector2(8, 9), false);
				var companion = prefab.AddComponent<EnemyBehavior>();
				companion.aiActor.knockbackDoer.weight = 10000;
			
				companion.aiActor.MovementSpeed = 2.5f;
				companion.aiActor.healthHaver.PreventAllDamage = false;
				companion.aiActor.CollisionDamage = 50f;
				companion.aiActor.HasShadow = false;
				companion.aiActor.IgnoreForRoomClear = true;
				companion.aiActor.aiAnimator.HitReactChance = 0f;
				companion.aiActor.specRigidbody.CollideWithOthers = true;
				companion.aiActor.specRigidbody.CollideWithTileMap = false;
				companion.aiActor.PreventFallingInPitsEver = true;
				companion.aiActor.healthHaver.ForceSetCurrentHealth(20f);
				companion.aiActor.CollisionKnockbackStrength = 1f;
				companion.aiActor.procedurallyOutlined = false;
				companion.aiActor.CanTargetPlayers = false;
				companion.aiActor.healthHaver.SetHealthMaximum(20f, null, false);
				companion.aiActor.specRigidbody.PixelColliders.Clear();
				companion.aiActor.SetIsFlying(true, "Flying Enemy", true, true);
				companion.aiActor.healthHaver.PreventAllDamage= true;
				companion.aiActor.healthHaver.PreventCooldownGainFromDamage = true;
				companion.aiActor.CollisionDamage = 0f;
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
					ManualWidth = 15,
					ManualHeight = 19,
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
					ManualWidth = 15,
					ManualHeight = 19,
					ManualDiameter = 0,
					ManualLeftX = 0,
					ManualLeftY = 0,
					ManualRightX = 0,
					ManualRightY = 0,



				});
				companion.aiActor.specRigidbody.PixelColliders.Add(new PixelCollider
				{

					ColliderGenerationMode = PixelCollider.PixelColliderGeneration.Manual,
					CollisionLayer = CollisionLayer.PlayerCollider,
					IsTrigger = false,
					BagleUseFirstFrameOnly = false,
					SpecifyBagelFrame = string.Empty,
					BagelColliderNumber = 0,
					ManualOffsetX = 0,
					ManualOffsetY = 0,
					ManualWidth = 15,
					ManualHeight = 19,
					ManualDiameter = 0,
					ManualLeftX = 0,
					ManualLeftY = 0,
					ManualRightX = 0,
					ManualRightY = 0,



				});
				companion.aiActor.CorpseObject = EnemyDatabase.GetOrLoadByGuid("2d4f8b5404614e7d8b235006acde427a").CorpseObject;
				companion.aiActor.PreventBlackPhantom = true;

				AIAnimator aiAnimator = companion.aiAnimator;
				aiAnimator.OtherAnimations = new List<AIAnimator.NamedDirectionalAnimation>
				{
					new AIAnimator.NamedDirectionalAnimation
					{
					name = "die",
					anim = new DirectionalAnimation
						{
							Type = DirectionalAnimation.DirectionType.TwoWayHorizontal,
							Flipped = new DirectionalAnimation.FlipType[2],
							AnimNames = new string[]
							{

						   "die_left",
						   "die_right"

							}

						}
					}
				};
				aiAnimator.IdleAnimation = new DirectionalAnimation
				{
					Type = DirectionalAnimation.DirectionType.TwoWayHorizontal,
					Flipped = new DirectionalAnimation.FlipType[2],
					AnimNames = new string[]
					{
						"idle_left",
						"idle_right"
					}
				};
				aiAnimator.MoveAnimation = new DirectionalAnimation
				{
					Type = DirectionalAnimation.DirectionType.TwoWayHorizontal,
					Flipped = new DirectionalAnimation.FlipType[2],
					AnimNames = new string[]
						{
						"run_left",
						"run_right"
						}
				};
				bool flag3 = BatCollection == null;
				if (flag3)
				{
					BatCollection = SpriteBuilder.ConstructCollection(prefab, "Bat_Collection");
					UnityEngine.Object.DontDestroyOnLoad(BatCollection);
					for (int i = 0; i < spritePaths.Length; i++)
					{
						SpriteBuilder.AddSpriteToCollection(spritePaths[i], BatCollection);
					}
					SpriteBuilder.AddAnimation(companion.spriteAnimator, BatCollection, new List<int>
					{
0,
						0
					}, "idle_left", tk2dSpriteAnimationClip.WrapMode.Loop).fps = 7f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, BatCollection, new List<int>
					{

						0
					}, "idle_right", tk2dSpriteAnimationClip.WrapMode.Once).fps = 7f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, BatCollection, new List<int>
					{

					0

					}, "run_left", tk2dSpriteAnimationClip.WrapMode.Once).fps = 7f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, BatCollection, new List<int>
					{

					0
			
					}, "run_right", tk2dSpriteAnimationClip.WrapMode.Once).fps = 7f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, BatCollection, new List<int>
					{
1


					}, "die_right", tk2dSpriteAnimationClip.WrapMode.Once).fps = 8f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, BatCollection, new List<int>
					{


1


					}, "die_left", tk2dSpriteAnimationClip.WrapMode.Once).fps = 8f;

					//companion.aiActor.gameObject.AddComponent<ColliderComponent>();
					var bs = prefab.GetComponent<BehaviorSpeculator>();
					prefab.GetComponent<ObjectVisibilityManager>();
					BehaviorSpeculator behaviorSpeculator = EnemyDatabase.GetOrLoadByGuid("9b4fb8a2a60a457f90dcf285d34143ac").behaviorSpeculator;
					bs.OverrideBehaviors = behaviorSpeculator.OverrideBehaviors;
					bs.OtherBehaviors = behaviorSpeculator.OtherBehaviors;
					shootpoint = new GameObject("turretshoot");
					shootpoint.transform.parent = companion.transform;
					shootpoint.transform.position = companion.sprite.WorldCenter;
					GameObject m_CachedGunAttachPoint = companion.transform.Find("turretshoot").gameObject;
					bs.InstantFirstTick = behaviorSpeculator.InstantFirstTick;
					bs.TickInterval = behaviorSpeculator.TickInterval;
					bs.PostAwakenDelay = behaviorSpeculator.PostAwakenDelay;
					bs.RemoveDelayOnReinforce = behaviorSpeculator.RemoveDelayOnReinforce;
					bs.OverrideStartingFacingDirection = behaviorSpeculator.OverrideStartingFacingDirection;
					bs.StartingFacingDirection = behaviorSpeculator.StartingFacingDirection;
					bs.SkipTimingDifferentiator = behaviorSpeculator.SkipTimingDifferentiator;
					Game.Enemies.Add("cak:turret", companion.aiActor);
					ItemsMod.Strings.Enemies.Set("#TURRET", "Turret");
					companion.aiActor.OverrideDisplayName = "#TURRET";
					companion.aiActor.ActorName = "#TURRET";
					companion.aiActor.name = "#TURRET";

					bs.AttackBehaviors = new List<AttackBehaviorBase>() {
				new ShootBehavior() {
					ShootPoint = m_CachedGunAttachPoint,
					BulletScript = new CustomBulletScriptSelector(typeof(SkellScript)),
					LeadAmount = 0f,
					AttackCooldown = 5f,
					InitialCooldown = 1f,
					RequiresLineOfSight = true,
					StopDuring = ShootBehavior.StopType.Attack,
					Uninterruptible = true,

				}
				};

				}
			}

		}


		private static string[] spritePaths = new string[]
		{

			"CakeMod/Resources/turret/turret_idle_001.png",
				"CakeMod/Resources/turret/turret_die_001.png"


		};

		public class EnemyBehavior : BraveBehaviour
		{

			private RoomHandler m_StartRoom;
			private void Update()
			{
				if (Time.frameCount % 5 == 0)
				{
				}
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
				
				base.aiActor.healthHaver.OnPreDeath += (obj) =>
				{
				};
			}


		}
		public class SkellScript : Script
		{

			protected override IEnumerator Top()
			{
				AkSoundEngine.PostEvent("SND_WPN_ak47_shot_01", this.BulletBank.aiActor.gameObject);
				if (this.BulletBank && this.BulletBank.aiActor && this.BulletBank.aiActor.TargetRigidbody)
				{
					base.BulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("383175a55879441d90933b5c4e60cf6f").bulletBank.GetBullet("bigBullet"));
					base.BulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("1bc2a07ef87741be90c37096910843ab").bulletBank.GetBullet("default"));
					base.BulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("044a9f39712f456597b9762893fbc19c").bulletBank.GetBullet("gross"));

				}
				base.Fire(new Direction(0f, DirectionType.Aim, -1f), new Speed(7f, SpeedType.Sequence), new BasicBullet());
				yield break;
			}
		}
		public class BasicBullet : Bullet
		{
			// Token: 0x06000A91 RID: 2705 RVA: 0x00030B38 File Offset: 0x0002ED38
			public BasicBullet() : base("default", false, false, false)
			{
			}

		}
		public class WallBullet : Bullet
		{
			// Token: 0x06000A91 RID: 2705 RVA: 0x00030B38 File Offset: 0x0002ED38
			public WallBullet() : base("default", false, false, false)
			{

			}
		}
	}
}
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
using GungeonAPI;

namespace CakeMod
{
	public class DrawnKin : AIActor
	{
		public static GameObject prefab;
		public static readonly string guid = "drawnkin";
		private static tk2dSpriteCollectionData DrawnKinCollection;


		public static void Init()
		{
			DrawnKin.BuildPrefab();
		}

		public static void BuildPrefab()
		{
			//
			bool flag = prefab != null || EnemyBuilder.Dictionary.ContainsKey(guid);
			bool flag2 = flag;
			if (!flag2)
			{
				AIActor aIActor = EnemyDatabase.GetOrLoadByGuid("01972dee89fc4404a5c408d50007dad5");
				prefab = EnemyBuilder.BuildPrefab("DrawnKin", guid, spritePaths[0], new IntVector2(0, 0), new IntVector2(8, 9), true);
				var companion = prefab.AddComponent<EnemyBehavior>(); ;
				companion.aiActor.knockbackDoer.weight = 50;
				companion.aiActor.MovementSpeed = 2f;
				companion.aiActor.healthHaver.PreventAllDamage = false;
				companion.aiActor.CollisionDamage = 1f;
				companion.aiActor.HasShadow = false;
				companion.aiActor.IgnoreForRoomClear = false;
				companion.aiActor.aiAnimator.HitReactChance = 0f;
				companion.aiActor.specRigidbody.CollideWithOthers = true;
				companion.aiActor.specRigidbody.CollideWithTileMap = true;
				companion.aiActor.PreventFallingInPitsEver = false;
				companion.aiActor.healthHaver.ForceSetCurrentHealth(30f);
				companion.aiActor.CollisionKnockbackStrength = 5f;
				companion.aiActor.CanTargetPlayers = true;
				companion.aiActor.SetIsFlying(true, "Flying Enemy", true, true);
				companion.aiActor.healthHaver.SetHealthMaximum(30f, null, false);
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
					ManualWidth = 15,
					ManualHeight = 17,
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
					ManualHeight = 17,
					ManualDiameter = 0,
					ManualLeftX = 0,
					ManualLeftY = 0,
					ManualRightX = 0,
					ManualRightY = 0,



				});
				companion.aiActor.CorpseObject = EnemyDatabase.GetOrLoadByGuid("01972dee89fc4404a5c408d50007dad5").CorpseObject;
				companion.aiActor.PreventBlackPhantom = false;
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

					   "die_right",
						   "die_left"

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
						"idle_front_right",
						"idle_front_left",
						"idle_back_right",
						"idle_back_left"
					}
				};
				aiAnimator.MoveAnimation = new DirectionalAnimation
				{
					Type = DirectionalAnimation.DirectionType.FourWay,
					Flipped = new DirectionalAnimation.FlipType[4],
					AnimNames = new string[]
								{
						"run_back_right",
						"run_front_right",
						"run_front_left",
						"run_back_left",
								}
				};

				bool flag3 = DrawnKinCollection == null;
				if (flag3)
				{
					DrawnKinCollection = SpriteBuilder.ConstructCollection(prefab, "DrawnKin_Collection");
					UnityEngine.Object.DontDestroyOnLoad(DrawnKinCollection);
					for (int i = 0; i < spritePaths.Length; i++)
					{
						SpriteBuilder.AddSpriteToCollection(spritePaths[i], DrawnKinCollection);
					}
					SpriteBuilder.AddAnimation(companion.spriteAnimator, DrawnKinCollection, new List<int>
					{
						0,
						3
					}, "idle_back_left", tk2dSpriteAnimationClip.WrapMode.Loop).fps = 5f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, DrawnKinCollection, new List<int>
					{
						1,
						2
					}, "idle_back_right", tk2dSpriteAnimationClip.WrapMode.Loop).fps = 5f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, DrawnKinCollection, new List<int>
					{
				0,
				3
					}, "idle_front_left", tk2dSpriteAnimationClip.WrapMode.Once).fps = 5f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, DrawnKinCollection, new List<int>
					{
						1,
						2
					  }, "idle_front_right", tk2dSpriteAnimationClip.WrapMode.Once).fps = 5f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, DrawnKinCollection, new List<int>
					{
						10,
	11,
	12,
	13,
	14,
	15
					}, "run_back_left", tk2dSpriteAnimationClip.WrapMode.Once).fps = 10f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, DrawnKinCollection, new List<int>
					{
	 22,
 23,
 24,
 25,
 26,
 27

						}, "run_back_right", tk2dSpriteAnimationClip.WrapMode.Once).fps = 10f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, DrawnKinCollection, new List<int>
					{
4,
5,
6,
7,
8,
9

					}, "run_front_left", tk2dSpriteAnimationClip.WrapMode.Loop).fps = 14f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, DrawnKinCollection, new List<int>
					{

 16,
 17,
 18,
 19,
 20,
 21
					}, "run_front_right", tk2dSpriteAnimationClip.WrapMode.Loop).fps = 14f;
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
					LineOfSight = true,
					ObjectPermanence = true,
					SearchInterval = 0.25f,
					PauseOnTargetSwitch = false,
					PauseTime = 0.25f
				}
			};
				bs.AttackBehaviors = new List<AttackBehaviorBase>() {
				new ShootGunBehavior() {
					GroupCooldownVariance = 0.2f,
					LineOfSight = false,
					WeaponType = WeaponType.BulletScript,
					OverrideBulletName = null,
					BulletScript = new CustomBulletScriptSelector(typeof(Drawn47Script)),
					FixTargetDuringAttack = true,
					StopDuringAttack = true,
					LeadAmount = 0,
					LeadChance = 1,
					RespectReload = true,
					MagazineCapacity = 8,
					ReloadSpeed = 5f,
					EmptiesClip = true,
					SuppressReloadAnim = false,
					TimeBetweenShots = -1,
					PreventTargetSwitching = true,
					OverrideAnimation = null,
					OverrideDirectionalAnimation = null,
					HideGun = false,
					UseLaserSight = false,
					UseGreenLaser = false,
					PreFireLaserTime = -1,
					AimAtFacingDirectionWhenSafe = false,
					Cooldown = 0.6f,
					CooldownVariance = 0,
					AttackCooldown = 0,
					GlobalCooldown = 0,
					InitialCooldown = 0,
					InitialCooldownVariance = 0,
					GroupName = null,
					GroupCooldown = 0,
					MinRange = 0,
					Range = 16,
					MinWallDistance = 0,
					MaxEnemiesInRoom = 0,
					MinHealthThreshold = 0,
					MaxHealthThreshold = 1,
					HealthThresholds = new float[0],
					AccumulateHealthThresholds = true,
					targetAreaStyle = null,
					IsBlackPhantom = false,
					resetCooldownOnDamage = null,
					RequiresLineOfSight = true,
					MaxUsages = 0,

				}
			};
				bs.MovementBehaviors = new List<MovementBehaviorBase>
			{
				new SeekTargetBehavior
				{
					StopWhenInRange = true,
					CustomRange = 7f,
					LineOfSight = false,
					ReturnToSpawn = false,
					SpawnTetherDistance = 0f,
					PathInterval = 0.5f,
					SpecifyRange = false,
					MinActiveRange = 0f,
					MaxActiveRange = 0f
				}
			};
				//BehaviorSpeculator load = EnemyDatabase.GetOrLoadByGuid("206405acad4d4c33aac6717d184dc8d4").behaviorSpeculator;
				//Tools.DebugInformation(load);
				bs.InstantFirstTick = behaviorSpeculator.InstantFirstTick;
				bs.TickInterval = behaviorSpeculator.TickInterval;
				bs.RemoveDelayOnReinforce = behaviorSpeculator.RemoveDelayOnReinforce;
				bs.OverrideStartingFacingDirection = behaviorSpeculator.OverrideStartingFacingDirection;
				bs.StartingFacingDirection = behaviorSpeculator.StartingFacingDirection;
				bs.SkipTimingDifferentiator = behaviorSpeculator.SkipTimingDifferentiator;
				GameObject m_CachedGunAttachPoint = companion.transform.Find("GunAttachPoint").gameObject;
				EnemyBuilder.DuplicateAIShooterAndAIBulletBank(prefab, aIActor.aiShooter, aIActor.GetComponent<AIBulletBank>(), CakeIDs.Drawn47, m_CachedGunAttachPoint.transform);
				Game.Enemies.Add("cak:drawnkin", companion.aiActor);
				ItemsMod.Strings.Enemies.Set("#DRAWNKIN", "Drawn Kin");
				companion.aiActor.OverrideDisplayName = "#DRAWNKIN";
				companion.aiActor.ActorName = "#DRAWNKIN";
				companion.aiActor.name = "#DRAWNKIN";


			}
		}

		public class Drawn47Script : Script // This BulletScript is just a modified version of the script BulletManShroomed, which you can find with dnSpy.
		{
			protected override IEnumerator Top() // This is just a simple example, but bullet scripts can do so much more.
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


		public class Drawn47Bullet : Bullet
		{
			public Drawn47Bullet() : base("bigBullet", false, false, false)
			{
			}
				public override void OnBulletDestruction(Bullet.DestroyType destroyType, SpeculativeRigidbody hitRigidbody, bool preventSpawningProjectiles)
			{
				
			}
		}
		public class BasicBullet : Bullet
		{
			// Token: 0x06000A91 RID: 2705 RVA: 0x00030B38 File Offset: 0x0002ED38
			public BasicBullet() : base("default", false, false, false)
			{
			}

		}


		private static string[] spritePaths = new string[]
		{
			
			//idles
			"CakeMod/Resources/drawnkin/drawnkin_idle_front_left_001",
			"CakeMod/Resources/drawnkin/drawnkin_idle_front_right_001",
			"CakeMod/Resources/drawnkin/drawnkin_idle_front_right_002",
			"CakeMod/Resources/drawnkin/drawnkin_idle_front_left_002",
			//move
			"CakeMod/Resources/drawnkin/drawnkin_move_front_left_001",
			"CakeMod/Resources/drawnkin/drawnkin_move_front_left_002",
			"CakeMod/Resources/drawnkin/drawnkin_move_front_left_003",
			"CakeMod/Resources/drawnkin/drawnkin_move_front_left_004",
			"CakeMod/Resources/drawnkin/drawnkin_move_front_left_005",
			"CakeMod/Resources/drawnkin/drawnkin_move_front_left_006",

			"CakeMod/Resources/drawnkin/drawnkin_move_back_left_001",
			"CakeMod/Resources/drawnkin/drawnkin_move_back_left_002",
			"CakeMod/Resources/drawnkin/drawnkin_move_back_left_003",
			"CakeMod/Resources/drawnkin/drawnkin_move_back_left_004",
			"CakeMod/Resources/drawnkin/drawnkin_move_back_left_005",
			"CakeMod/Resources/drawnkin/drawnkin_move_back_left_006",

			"CakeMod/Resources/drawnkin/drawnkin_move_front_right_001",
			"CakeMod/Resources/drawnkin/drawnkin_move_front_right_002",
			"CakeMod/Resources/drawnkin/drawnkin_move_front_right_003",
			"CakeMod/Resources/drawnkin/drawnkin_move_front_right_004",
			"CakeMod/Resources/drawnkin/drawnkin_move_front_right_005",
			"CakeMod/Resources/drawnkin/drawnkin_move_front_right_006",

			"CakeMod/Resources/drawnkin/drawnkin_move_back_right_001",
			"CakeMod/Resources/drawnkin/drawnkin_move_back_right_002",
			"CakeMod/Resources/drawnkin/drawnkin_move_back_right_003",
			"CakeMod/Resources/drawnkin/drawnkin_move_back_right_004",
			"CakeMod/Resources/drawnkin/drawnkin_move_back_right_005",
			"CakeMod/Resources/drawnkin/drawnkin_move_back_right_006",

			//death
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
					GameManager.Instance.StartCoroutine(LateEngage());
				}

			}

			private IEnumerator LateEngage()
			{
				yield return new WaitForSeconds(0.5f);
				base.aiActor.HasBeenEngaged = true;
				yield break;
			}

			
				private void Start()
			{
				m_StartRoom = aiActor.GetAbsoluteParentRoom();
				//base.aiActor.HasBeenEngaged = true;
				base.aiActor.healthHaver.OnPreDeath += (obj) =>
				{
					AkSoundEngine.PostEvent("Play_OBJ_skeleton_collapse_01", base.aiActor.gameObject);
				};
			}


		}


	}
}





using System;
using System.Collections.Generic;
using Gungeon;
using ItemAPI;
using UnityEngine;
using AnimationType = ItemAPI.EnemyBuilder.AnimationType;
using System.Collections;
using Dungeonator;
using System.Linq;
using Brave.BulletScript;
using Pathfinding;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace CakeMod
{
	public class TestEnemy : AIActor
	{
		public static GameObject prefab;
		public static readonly string guid = "waterbulon";
		private static tk2dSpriteCollectionData TestEnemyCollection;
		public static GameObject shootpoint;
		public static void Init()
		{
			TestEnemy.BuildPrefab();
		}

		public static void BuildPrefab()
		{
			//
			bool flag = prefab != null || EnemyBuilder.Dictionary.ContainsKey(guid);
			bool flag2 = flag;
			if (!flag2)
			{
				prefab = EnemyBuilder.BuildPrefab("TestEnemy", guid, spritePaths[0], new IntVector2(0, 0), new IntVector2(8, 9), false);
				var companion = prefab.AddComponent<EnemyBehavior>();
				companion.aiActor.knockbackDoer.weight = 150;
				companion.aiActor.MovementSpeed = 4.5f;
				companion.aiActor.healthHaver.PreventAllDamage = false;
				companion.aiActor.CollisionDamage = 1f;
				companion.aiActor.HasShadow = false;
				companion.aiActor.IgnoreForRoomClear = false;
				companion.aiActor.aiAnimator.HitReactChance = 0f;
				companion.aiActor.specRigidbody.CollideWithOthers = true;
				companion.aiActor.specRigidbody.CollideWithTileMap = false;
				companion.aiActor.PreventFallingInPitsEver = true;
				companion.aiActor.healthHaver.ForceSetCurrentHealth(25f);
				companion.aiActor.CollisionKnockbackStrength = 0f;
				companion.aiActor.procedurallyOutlined = true;
				companion.aiActor.CanTargetPlayers = true;
				companion.aiActor.healthHaver.SetHealthMaximum(25f, null, false);
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
					ManualWidth = 19,
					ManualHeight = 11,
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
					ManualWidth = 19,
					ManualHeight = 11,
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

						   "die_left",
						   "die_right"

							}

						}
					}
				};
				aiAnimator.IdleAnimation = new DirectionalAnimation
				{
					Type = DirectionalAnimation.DirectionType.FourWay,
					Flipped = new DirectionalAnimation.FlipType[4],
					AnimNames = new string[]
					{
						"idle_back_left",
						"idle_front_right",
						"idle_front_left",
						"die",

					}
				};
				aiAnimator.MoveAnimation = new DirectionalAnimation
				{
					Type = DirectionalAnimation.DirectionType.FourWay,
					Flipped = new DirectionalAnimation.FlipType[4],
					AnimNames = new string[]
						{
						"run_back_left",
						"run_front_right",
						"run_front_left",
						"run_back_right",

						}
				};
				bool flag3 = TestEnemyCollection == null;
				if (flag3)
				{
					TestEnemyCollection = SpriteBuilder.ConstructCollection(prefab, "FodderBoi_Collection");
					UnityEngine.Object.DontDestroyOnLoad(TestEnemyCollection);
					for (int i = 0; i < spritePaths.Length; i++)
					{
						SpriteBuilder.AddSpriteToCollection(spritePaths[i], TestEnemyCollection);
					}
					SpriteBuilder.AddAnimation(companion.spriteAnimator, TestEnemyCollection, new List<int>
					{

					0,
					1,
					2,
					3,
					4,
					5,


					}, "idle_back_left", tk2dSpriteAnimationClip.WrapMode.Loop).fps = 13f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, TestEnemyCollection, new List<int>
					{
					6,
					7,
					8,
					9,
					10,


					}, "die", tk2dSpriteAnimationClip.WrapMode.Once).fps = 6f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, TestEnemyCollection, new List<int>
					{

					0,
					1,
					2,
					3,
					4,
					5,

					}, "idle_front_left", tk2dSpriteAnimationClip.WrapMode.Loop).fps = 13f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, TestEnemyCollection, new List<int>
					{
					0,
					1,
					2,
					3,
					4,
					5,


					}, "idle_front_right", tk2dSpriteAnimationClip.WrapMode.Once).fps = 13f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, TestEnemyCollection, new List<int>
					{

					0,
					1,
					2,
					3,
					4,
					5,


					}, "run_front_left", tk2dSpriteAnimationClip.WrapMode.Once).fps = 13f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, TestEnemyCollection, new List<int>
					{

					0,
					1,
					2,
					3,
					4,
					5,


					}, "run_front_right", tk2dSpriteAnimationClip.WrapMode.Once).fps = 13f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, TestEnemyCollection, new List<int>
					{
					0,
					1,
					2,
					3,
					4,
					5,



					}, "run_back_left", tk2dSpriteAnimationClip.WrapMode.Once).fps = 13f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, TestEnemyCollection, new List<int>
					{

					0,
					1,
					2,
					3,
					4,
					5,


					}, "run_back_right", tk2dSpriteAnimationClip.WrapMode.Once).fps = 13f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, TestEnemyCollection, new List<int>
					{

				 6,
					7,
					8,
					9,
					10,




					}, "die_right", tk2dSpriteAnimationClip.WrapMode.Once).fps = 6f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, TestEnemyCollection, new List<int>
					{

				 6,
					7,
					8,
					9,
					10,

					}, "die_left", tk2dSpriteAnimationClip.WrapMode.Once).fps = 6f;

				}
				var bs = prefab.GetComponent<BehaviorSpeculator>();
				prefab.GetComponent<ObjectVisibilityManager>();
				BehaviorSpeculator behaviorSpeculator = EnemyDatabase.GetOrLoadByGuid("01972dee89fc4404a5c408d50007dad5").behaviorSpeculator;
				bs.OverrideBehaviors = behaviorSpeculator.OverrideBehaviors;
				bs.OtherBehaviors = behaviorSpeculator.OtherBehaviors;
				shootpoint = new GameObject("fuck2");
				shootpoint.transform.parent = companion.transform;
				shootpoint.transform.position = companion.sprite.WorldCenter;
				GameObject m_CachedGunAttachPoint = companion.transform.Find("fuck2").gameObject;
				bs.TargetBehaviors = new List<TargetBehaviorBase>
			{
				new TargetPlayerBehavior
				{
					Radius = 35f,
					LineOfSight = true,
					ObjectPermanence = true,
					SearchInterval = 0.2f,
					PauseOnTargetSwitch = false,
					PauseTime = 0.2f,
				}
			};
				bs.MovementBehaviors = new List<MovementBehaviorBase>() {
				new SeekTargetBehavior() {
					StopWhenInRange = false,
					CustomRange = 6,
					LineOfSight = true,
					ReturnToSpawn = true,
					SpawnTetherDistance = 0,
					PathInterval = 0.5f,
					SpecifyRange = false,
					MinActiveRange = -0.25f,
					MaxActiveRange = 0
				}
				};
				bs.InstantFirstTick = behaviorSpeculator.InstantFirstTick;
				bs.TickInterval = behaviorSpeculator.TickInterval;
				bs.PostAwakenDelay = behaviorSpeculator.PostAwakenDelay;
				bs.RemoveDelayOnReinforce = behaviorSpeculator.RemoveDelayOnReinforce;
				bs.OverrideStartingFacingDirection = behaviorSpeculator.OverrideStartingFacingDirection;
				bs.StartingFacingDirection = behaviorSpeculator.StartingFacingDirection;
				bs.SkipTimingDifferentiator = behaviorSpeculator.SkipTimingDifferentiator;
				Game.Enemies.Add("cak:testenemy", companion.aiActor);
				if (companion.GetComponent<EncounterTrackable>() != null)
				{
					UnityEngine.Object.Destroy(companion.GetComponent<EncounterTrackable>());
				}
				companion.encounterTrackable = companion.gameObject.AddComponent<EncounterTrackable>();
				companion.encounterTrackable.journalData = new JournalEntry();
				companion.encounterTrackable.EncounterGuid = "cak:waterbulon";
				
				companion.encounterTrackable.prerequisites = new DungeonPrerequisite[0];
			}
		}



		private static string[] spritePaths = new string[]
		{

			"CakeMod/Resources/Waterbulon/Idle/waterbulon_idle_001.png",
			"CakeMod/Resources/Waterbulon/Idle/waterbulon_idle_002.png",
			"CakeMod/Resources/Waterbulon/Idle/waterbulon_idle_003.png",
			"CakeMod/Resources/Waterbulon/Idle/waterbulon_idle_004.png",
			"CakeMod/Resources/Waterbulon/Idle/waterbulon_idle_005.png",
			"CakeMod/Resources/Waterbulon/Idle/waterbulon_idle_006.png",

			"CakeMod/Resources/Waterbulon/Die/waterbulon_die_001.png",
			"CakeMod/Resources/Waterbulon/Die/waterbulon_die_002.png",
			"CakeMod/Resources/Waterbulon/Die/waterbulon_die_003.png",
			"CakeMod/Resources/Waterbulon/Die/waterbulon_die_004.png",
			"CakeMod/Resources/Waterbulon/Die/waterbulon_die_005.png",
		};

		public class EnemyBehavior : BraveBehaviour
		{

			private RoomHandler m_StartRoom;

			private void Update()
			{
				if (Time.frameCount % 5 == 0)
				{
					DeadlyDeadlyGoopManager goopManagerForGoopType = DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(EasyGoopDefinitions.WaterGoop);
					goopManagerForGoopType.AddGoopCircle(this.aiActor.specRigidbody.UnitCenter, 1.5f, -1, false, -1);
				}
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
				base.aiActor.bulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("796a7ed4ad804984859088fc91672c7f").bulletBank.bulletBank.GetBullet("default"));
				m_StartRoom = aiActor.GetAbsoluteParentRoom();
			}
			private void ProcessEnemy(AIActor target, float distance)
			{
				bool jamnation = target.IsBlackPhantom;
				if (!jamnation)
				{
					target.PlayEffectOnActor(ResourceCache.Acquire("Global VFX/VFX_Curse") as GameObject, Vector3.zero, true, false, false);
					target.BecomeBlackPhantom();
				}
			}

		}
	}
}
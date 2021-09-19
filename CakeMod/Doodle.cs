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
	public class Doodlelet : AIActor
	{
		public static GameObject prefab;
		public static readonly string guid = "doodle_book";
		private static tk2dSpriteCollectionData InkedBook;
		public static GameObject shootpoint;
		public static void Init()
		{
			Doodlelet.BuildPrefab();
		}

		public static void BuildPrefab()
		{
			//
			bool flag = prefab != null || EnemyBuilder.Dictionary.ContainsKey(guid);
			bool flag2 = flag;
			if (!flag2)
			{
				//float AttackAnimationThingAMaWhatIts = 0.5f;
				prefab = EnemyBuilder.BuildPrefab("doodle_book", guid, spritePaths[0], new IntVector2(0, 0), new IntVector2(8, 9), false);
				var companion = prefab.AddComponent<EnemyBehavior>();
				companion.aiActor.knockbackDoer.weight = 100;
				companion.aiActor.MovementSpeed = 2.5f;
				companion.aiActor.healthHaver.PreventAllDamage = false;
				companion.aiActor.CollisionDamage = 50f;
				companion.aiActor.HasShadow = false;
				companion.aiActor.IgnoreForRoomClear = true;
				companion.aiActor.aiAnimator.HitReactChance = 0f;
				companion.aiActor.specRigidbody.CollideWithOthers = true;
				companion.aiActor.specRigidbody.CollideWithTileMap = true;
				companion.aiActor.healthHaver.ForceSetCurrentHealth(20f);
				companion.aiActor.CollisionKnockbackStrength = 1f;
				companion.aiActor.procedurallyOutlined = false;
				companion.aiActor.CanTargetPlayers = true;
				companion.aiActor.healthHaver.SetHealthMaximum(20f, null, false);
				companion.aiActor.specRigidbody.PixelColliders.Clear();
				companion.aiActor.SetIsFlying(true, "Flying Enemy", true, true);
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
					ManualWidth = 17,
					ManualHeight = 18,
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
					ManualWidth = 17,
					ManualHeight = 18,
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
					ManualWidth = 17,
					ManualHeight = 18,
					ManualDiameter = 0,
					ManualLeftX = 0,
					ManualLeftY = 0,
					ManualRightX = 0,
					ManualRightY = 0,



				});
				companion.aiActor.CorpseObject = EnemyDatabase.GetOrLoadByGuid("42be66373a3d4d89b91a35c9ff8adfec").CorpseObject;
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
				bool flag3 = InkedBook == null;
				if (flag3)
				{
					InkedBook = SpriteBuilder.ConstructCollection(prefab, "Doodlelet_Collection");
					UnityEngine.Object.DontDestroyOnLoad(InkedBook);
					for (int i = 0; i < spritePaths.Length; i++)
					{
						SpriteBuilder.AddSpriteToCollection(spritePaths[i], InkedBook);
					}
					SpriteBuilder.AddAnimation(companion.spriteAnimator, InkedBook, new List<int>
					{

					0,
					1,
					2,
					3,
					4
					}, "idle_left", tk2dSpriteAnimationClip.WrapMode.Loop).fps = 5f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, InkedBook, new List<int>
					{

					0,
					1,
					2,
					3,
					4

					}, "idle_right", tk2dSpriteAnimationClip.WrapMode.Once).fps = 5f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, InkedBook, new List<int>
					{
					0,
					1,
					2,
					3,
					4


					}, "run_left", tk2dSpriteAnimationClip.WrapMode.Once).fps = 10f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, InkedBook, new List<int>
					{

					0,
					1,
					2,
					3,
					4

					}, "run_right", tk2dSpriteAnimationClip.WrapMode.Once).fps = 10f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, InkedBook, new List<int>
					{

				 5,
				 6,
				 7




					}, "die_right", tk2dSpriteAnimationClip.WrapMode.Once).fps = 8f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, InkedBook, new List<int>
					{


				 5,
				 6,
				 7


					}, "die_left", tk2dSpriteAnimationClip.WrapMode.Once).fps = 8f;

					//companion.aiActor.gameObject.AddComponent<ColliderComponent>();
					var bs = prefab.GetComponent<BehaviorSpeculator>();
					prefab.GetComponent<ObjectVisibilityManager>();
					BehaviorSpeculator behaviorSpeculator = EnemyDatabase.GetOrLoadByGuid("c0ff3744760c4a2eb0bb52ac162056e6").behaviorSpeculator;
					bs.OverrideBehaviors = behaviorSpeculator.OverrideBehaviors;
					bs.OtherBehaviors = behaviorSpeculator.OtherBehaviors;
					shootpoint = new GameObject("doodleshoot");
					shootpoint.transform.parent = companion.transform;
					shootpoint.transform.position = companion.sprite.WorldCenter;
					GameObject m_CachedGunAttachPoint = companion.transform.Find("doodleshoot").gameObject;
					bs.TargetBehaviors = new List<TargetBehaviorBase>
			{
				new TargetPlayerBehavior
				{
					Radius = 35f,
					LineOfSight = true,
					ObjectPermanence = true,
					SearchInterval = 0.25f,
					PauseOnTargetSwitch = false,
					PauseTime = 0.25f,
				}
			};
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
					bs.MovementBehaviors = new List<MovementBehaviorBase>
			{
				new SeekTargetBehavior
				{
					StopWhenInRange = false,
					CustomRange = 7f,
					LineOfSight = false,
					ReturnToSpawn = true,
					SpawnTetherDistance = 0f,
					PathInterval = 0.5f,
					SpecifyRange = false,
					MinActiveRange = 0f,
					MaxActiveRange = 0f
				}
			};

					bs.InstantFirstTick = behaviorSpeculator.InstantFirstTick;
					bs.TickInterval = behaviorSpeculator.TickInterval;
					bs.PostAwakenDelay = behaviorSpeculator.PostAwakenDelay;
					bs.RemoveDelayOnReinforce = behaviorSpeculator.RemoveDelayOnReinforce;
					bs.OverrideStartingFacingDirection = behaviorSpeculator.OverrideStartingFacingDirection;
					bs.StartingFacingDirection = behaviorSpeculator.StartingFacingDirection;
					bs.SkipTimingDifferentiator = behaviorSpeculator.SkipTimingDifferentiator;
					Game.Enemies.Add("cak:doodle", companion.aiActor);
					ItemsMod.Strings.Enemies.Set("#DOODLE", "Doodle Goopton");
					companion.aiActor.OverrideDisplayName = "#DOODLE";
					companion.aiActor.ActorName = "#DOODLE";
					companion.aiActor.name = "#DOODLE";



				}
			}

		}


		private static string[] spritePaths = new string[]
		{

			"CakeMod/Resources/Doodle/doodle_idle_001.png",
			"CakeMod/Resources/Doodle/doodle_idle_002.png",
			"CakeMod/Resources/Doodle/doodle_idle_003.png",
			"CakeMod/Resources/Doodle/doodle_idle_004.png",
			"CakeMod/Resources/Doodle/doodle_idle_005.png",

			"CakeMod/Resources/Doodle/doodle_death_001.png",
			"CakeMod/Resources/Doodle/doodle_death_002.png",
			"CakeMod/Resources/Doodle/doodle_death_003.png",

		};

		public class EnemyBehavior : BraveBehaviour
		{

			private RoomHandler m_StartRoom;
			private void Update()
			{
				if (Time.frameCount % 5 == 0)
				{
					DeadlyDeadlyGoopManager goopManagerForGoopType = DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(HelpfulLibrary.Ink);
					goopManagerForGoopType.AddGoopCircle(this.aiActor.specRigidbody.UnitCenter, 1.7f, -1, false, -1);
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
				//base.aiActor.HasBeenEngaged = true;
				base.aiActor.healthHaver.OnPreDeath += (obj) =>
				{
					AkSoundEngine.PostEvent("Play_ENM_highpriest_blast_01", base.aiActor.gameObject);
				};
			}


		}
		public class SkellScript : Script
		{
			protected override IEnumerator Top()
			{
				
				yield break;
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

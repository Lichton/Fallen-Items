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
	public class ammomimic : AIActor
	{
		public static GameObject prefab;
		public static readonly string guid = "ammomimic_cak";
		private static tk2dSpriteCollectionData ammomimicCollection;
		public static GameObject shootpoint;
		public static void Init()
		{
			ammomimic.BuildPrefab();
		}

		public static void BuildPrefab()
		{
			//
			bool flag = prefab != null || EnemyBuilder.Dictionary.ContainsKey(guid);
			bool flag2 = flag;
			if (!flag2)
			{
				//float AttackAnimationThingAMaWhatIts = 0.5f;
				prefab = EnemyBuilder.BuildPrefab("ammomimic_cakaaaa", guid, spritePaths[0], new IntVector2(0, 0), new IntVector2(8, 9), false);
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
				companion.aiActor.PreventFallingInPitsEver = true;
				companion.aiActor.healthHaver.ForceSetCurrentHealth(20f);
				companion.aiActor.CollisionKnockbackStrength = 1f;
				companion.aiActor.procedurallyOutlined = false;
				companion.aiActor.CanTargetPlayers = false;
				companion.aiActor.healthHaver.SetHealthMaximum(20f, null, false);
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
					ManualWidth = 13,
					ManualHeight = 13,
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
					ManualWidth = 13,
					ManualHeight = 13,
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
					ManualWidth = 13,
					ManualHeight = 13,
					ManualDiameter = 0,
					ManualLeftX = 0,
					ManualLeftY = 0,
					ManualRightX = 0,
					ManualRightY = 0,



				});
				companion.aiActor.specRigidbody.PixelColliders.Add(new PixelCollider
				{

					ColliderGenerationMode = PixelCollider.PixelColliderGeneration.Manual,
					CollisionLayer = CollisionLayer.PlayerBlocker,
					IsTrigger = false,
					BagleUseFirstFrameOnly = false,
					SpecifyBagelFrame = string.Empty,
					BagelColliderNumber = 0,
					ManualOffsetX = 0,
					ManualOffsetY = 0,
					ManualWidth = 13,
					ManualHeight = 13,
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
				bool flag3 = ammomimicCollection == null;
				if (flag3)
				{
					ammomimicCollection = SpriteBuilder.ConstructCollection(prefab, "ammomimic_Collection");
					UnityEngine.Object.DontDestroyOnLoad(ammomimicCollection);
					for (int i = 0; i < spritePaths.Length; i++)
					{
						SpriteBuilder.AddSpriteToCollection(spritePaths[i], ammomimicCollection);
					}
					SpriteBuilder.AddAnimation(companion.spriteAnimator, ammomimicCollection, new List<int>
					{
					0,
					1,
					2,
					3,
					4,
					5,
					6,
					7,
					8,
					9,
					10,
					11,
					12,
					13,
					14,
					}, "idle_left", tk2dSpriteAnimationClip.WrapMode.Loop).fps = 7f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, ammomimicCollection, new List<int>
					{

					0,
					1,
					2,
					3,
					4,
					5,
					6,
					7,
					8,
					9,
					10,
					11,
					12,
					13,
					14,
					}, "idle_right", tk2dSpriteAnimationClip.WrapMode.Once).fps = 7f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, ammomimicCollection, new List<int>
					{
0,
					1,
					2,
					3,
					4,
					5,
					6,
					7,
					8,
					9,
					10,
					11,
					12,
					13,
					14,


					}, "run_left", tk2dSpriteAnimationClip.WrapMode.Once).fps = 7f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, ammomimicCollection, new List<int>
					{

					0,
					1,
					2,
					3,
					4,
					5,
					6,
					7,
					8,
					9,
					10,
					11,
					12,
					13,
					14,



					}, "run_right", tk2dSpriteAnimationClip.WrapMode.Once).fps = 7f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, ammomimicCollection, new List<int>
					{

				 0



					}, "die_right", tk2dSpriteAnimationClip.WrapMode.Once).fps = 8f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, ammomimicCollection, new List<int>
					{


0


					}, "die_left", tk2dSpriteAnimationClip.WrapMode.Once).fps = 8f;

					//companion.aiActor.gameObject.AddComponent<ColliderComponent>();
					var bs = prefab.GetComponent<BehaviorSpeculator>();
					prefab.GetComponent<ObjectVisibilityManager>();
					BehaviorSpeculator behaviorSpeculator = EnemyDatabase.GetOrLoadByGuid("9b4fb8a2a60a457f90dcf285d34143ac").behaviorSpeculator;
					bs.OverrideBehaviors = behaviorSpeculator.OverrideBehaviors;
					bs.OtherBehaviors = behaviorSpeculator.OtherBehaviors;
					shootpoint = new GameObject("ammosh");
					shootpoint.transform.parent = companion.transform;
					shootpoint.transform.position = companion.sprite.WorldCenter;
					GameObject m_CachedGunAttachPoint = companion.transform.Find("ammosh").gameObject;
					bs.InstantFirstTick = behaviorSpeculator.InstantFirstTick;
					bs.TickInterval = behaviorSpeculator.TickInterval;
					bs.PostAwakenDelay = behaviorSpeculator.PostAwakenDelay;
					bs.RemoveDelayOnReinforce = behaviorSpeculator.RemoveDelayOnReinforce;
					bs.OverrideStartingFacingDirection = behaviorSpeculator.OverrideStartingFacingDirection;
					bs.StartingFacingDirection = behaviorSpeculator.StartingFacingDirection;
					bs.SkipTimingDifferentiator = behaviorSpeculator.SkipTimingDifferentiator;
					Game.Enemies.Add("cak:ammomimic", companion.aiActor);
					ItemsMod.Strings.Enemies.Set("#AMMOMIMIC", "Ammomimic");
					companion.aiActor.OverrideDisplayName = "#AMMOMIMIC";
					companion.aiActor.ActorName = "#AMMOMIMIC";
					companion.aiActor.name = "#AMMOMIMIC";



				}
			}

		}


		private static string[] spritePaths = new string[]
		{

			"CakeMod/Resources/ammomimicFolder/ammobox_pickup_001.png",
			"CakeMod/Resources/ammomimicFolder/ammobox_pickup_002.png",
			"CakeMod/Resources/ammomimicFolder/ammobox_pickup_003.png",
			"CakeMod/Resources/ammomimicFolder/ammobox_pickup_004.png",
			"CakeMod/Resources/ammomimicFolder/ammobox_pickup_005.png",
			"CakeMod/Resources/ammomimicFolder/ammobox_pickup_006.png",
			"CakeMod/Resources/ammomimicFolder/ammobox_pickup_007.png",
			"CakeMod/Resources/ammomimicFolder/ammobox_pickup_008.png",
			"CakeMod/Resources/ammomimicFolder/ammobox_pickup_009.png",
			"CakeMod/Resources/ammomimicFolder/ammobox_pickup_010.png",
			"CakeMod/Resources/ammomimicFolder/ammobox_pickup_011.png",
			"CakeMod/Resources/ammomimicFolder/ammobox_pickup_012.png",
			"CakeMod/Resources/ammomimicFolder/ammobox_pickup_013.png",
			"CakeMod/Resources/ammomimicFolder/ammobox_pickup_014.png",
			"CakeMod/Resources/ammomimicFolder/ammobox_pickup_015.png"


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

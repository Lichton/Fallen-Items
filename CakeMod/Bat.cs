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
	public class Bat : AIActor
	{
		public static GameObject prefab;
		public static readonly string guid = "bat_cak";
		private static tk2dSpriteCollectionData BatCollection;
		public static GameObject shootpoint;
		public static void Init()
		{
			Bat.BuildPrefab();
		}

		public static void BuildPrefab()
		{
			//
			bool flag = prefab != null || EnemyBuilder.Dictionary.ContainsKey(guid);
			bool flag2 = flag;
			if (!flag2)
			{
				//float AttackAnimationThingAMaWhatIts = 0.5f;
				prefab = EnemyBuilder.BuildPrefab("bat_cak", guid, spritePaths[0], new IntVector2(0, 0), new IntVector2(8, 9), false);
				var companion = prefab.AddComponent<EnemyBehavior>();
				companion.aiActor.knockbackDoer.weight = 100;
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
					ManualWidth = 17,
					ManualHeight = 18,
					ManualDiameter = 0,
					ManualLeftX = 0,
					ManualLeftY = 0,
					ManualRightX = 0,
					ManualRightY = 0,



				});
				companion.aiActor.CorpseObject = EnemyDatabase.GetOrLoadByGuid("2d4f8b5404614e7d8b235006acde427a").CorpseObject;
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
					1,
					2,
					3
					}, "idle_left", tk2dSpriteAnimationClip.WrapMode.Loop).fps = 7f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, BatCollection, new List<int>
					{

					0,
					1,
					2,
					3
					}, "idle_right", tk2dSpriteAnimationClip.WrapMode.Once).fps = 7f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, BatCollection, new List<int>
					{

					0,
					1,
					2,
					3



					}, "run_left", tk2dSpriteAnimationClip.WrapMode.Once).fps = 7f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, BatCollection, new List<int>
					{

					0,
					1,
					2,
					3


					}, "run_right", tk2dSpriteAnimationClip.WrapMode.Once).fps = 7f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, BatCollection, new List<int>
					{

				 4,
				 5,
				 6



					}, "die_right", tk2dSpriteAnimationClip.WrapMode.Once).fps = 8f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, BatCollection, new List<int>
					{



				 4,
				 5,
				 6


					}, "die_left", tk2dSpriteAnimationClip.WrapMode.Once).fps = 8f;

					//companion.aiActor.gameObject.AddComponent<ColliderComponent>();
					var bs = prefab.GetComponent<BehaviorSpeculator>();
					prefab.GetComponent<ObjectVisibilityManager>();
					BehaviorSpeculator behaviorSpeculator = EnemyDatabase.GetOrLoadByGuid("9b4fb8a2a60a457f90dcf285d34143ac").behaviorSpeculator;
					bs.OverrideBehaviors = behaviorSpeculator.OverrideBehaviors;
					bs.OtherBehaviors = behaviorSpeculator.OtherBehaviors;
					shootpoint = new GameObject("batshoot");
					shootpoint.transform.parent = companion.transform;
					shootpoint.transform.position = companion.sprite.WorldCenter;
					GameObject m_CachedGunAttachPoint = companion.transform.Find("batshoot").gameObject;
					bs.InstantFirstTick = behaviorSpeculator.InstantFirstTick;
					bs.TickInterval = behaviorSpeculator.TickInterval;
					bs.PostAwakenDelay = behaviorSpeculator.PostAwakenDelay;
					bs.RemoveDelayOnReinforce = behaviorSpeculator.RemoveDelayOnReinforce;
					bs.OverrideStartingFacingDirection = behaviorSpeculator.OverrideStartingFacingDirection;
					bs.StartingFacingDirection = behaviorSpeculator.StartingFacingDirection;
					bs.SkipTimingDifferentiator = behaviorSpeculator.SkipTimingDifferentiator;
					Game.Enemies.Add("cak:bat", companion.aiActor);
					ItemsMod.Strings.Enemies.Set("#BAT", "Bat");
					companion.aiActor.OverrideDisplayName = "#BAT";
					companion.aiActor.ActorName = "#BAT";
					companion.aiActor.name = "#BAT";



				}
			}

		}


		private static string[] spritePaths = new string[]
		{

			"CakeMod/Resources/BatFolder/bat_fly_001.png",
			"CakeMod/Resources/BatFolder/bat_fly_002.png",
			"CakeMod/Resources/BatFolder/bat_fly_003.png",
			"CakeMod/Resources/BatFolder/bat_fly_004.png",

			"CakeMod/Resources/BatFolder/bat_death_explode_001.png",
			"CakeMod/Resources/BatFolder/bat_death_explode_002.png",
			"CakeMod/Resources/BatFolder/bat_death_explode_003.png",

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

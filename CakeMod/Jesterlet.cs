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
	public class Jesterlet : AIActor
	{
		public static GameObject prefab;
		public static readonly string guid = "jesterlet";
		private static tk2dSpriteCollectionData JesterletCollection;
		public static GameObject shootpoint;

		public static void Init()
		{

			Jesterlet.BuildPrefab();
		}

		public static void BuildPrefab()
		{
			// source = EnemyDatabase.GetOrLoadByGuid("c50a862d19fc4d30baeba54795e8cb93");
			bool flag = prefab != null || EnemyBuilder.Dictionary.ContainsKey(guid);
			bool flag2 = flag;
			if (!flag2)
			{
				prefab = EnemyBuilder.BuildPrefab("Jesterlet", guid, spritePaths[0], new IntVector2(0, 0), new IntVector2(8, 9), false);
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
				DirectionalAnimation done3456 = new DirectionalAnimation
				{
					Type = DirectionalAnimation.DirectionType.Single,
					Prefix = "prepare",
					AnimNames = new string[1],
					Flipped = new DirectionalAnimation.FlipType[1]
				};
				aiAnimator.OtherAnimations = new List<AIAnimator.NamedDirectionalAnimation>
				{
					new AIAnimator.NamedDirectionalAnimation
					{
						name = "prepare",
						anim = done3456
					}
				};
				bool flag3 = JesterletCollection == null;
				if (flag3)
				{
					JesterletCollection = SpriteBuilder.ConstructCollection(prefab, "Jesterlet_Collection");
					UnityEngine.Object.DontDestroyOnLoad(JesterletCollection);
					for (int i = 0; i < spritePaths.Length; i++)
					{
						SpriteBuilder.AddSpriteToCollection(spritePaths[i], JesterletCollection);
					}
					SpriteBuilder.AddAnimation(companion.spriteAnimator, JesterletCollection, new List<int>
					{

					2,
					3

					}, "idle", tk2dSpriteAnimationClip.WrapMode.Loop).fps = 7f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, JesterletCollection, new List<int>
					{

					0,
					1

					}, "idle_two", tk2dSpriteAnimationClip.WrapMode.Loop).fps = 7f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, JesterletCollection, new List<int>
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
				bs.AttackBehaviors = new List<AttackBehaviorBase>()
				{
					new ShootBehavior()
					{
						ShootPoint = m_CachedGunAttachPoint,
						BulletScript = new CustomBulletScriptSelector(typeof(TeleportScript)),
						LeadAmount = 0f,
						InitialCooldown = 5f,
						AttackCooldown = 10f,
						RequiresLineOfSight = false,
						ChargeAnimation = "prepare",
						ChargeTime = 1f,
						Uninterruptible = true
						},
					};
				bs.InstantFirstTick = behaviorSpeculator.InstantFirstTick;
				bs.TickInterval = behaviorSpeculator.TickInterval;
				bs.PostAwakenDelay = behaviorSpeculator.PostAwakenDelay;
				bs.RemoveDelayOnReinforce = behaviorSpeculator.RemoveDelayOnReinforce;
				bs.OverrideStartingFacingDirection = behaviorSpeculator.OverrideStartingFacingDirection;
				bs.StartingFacingDirection = behaviorSpeculator.StartingFacingDirection;
				bs.SkipTimingDifferentiator = behaviorSpeculator.SkipTimingDifferentiator;
				Game.Enemies.Add("cak:jestlet", companion.aiActor);

			}
		}




		private static string[] spritePaths = new string[]
		{
			
			//idles
			"CakeMod/Resources/jesterbullet_idle_001",
			"CakeMod/Resources/jesterbullet_idle_002",
			"CakeMod/Resources/jesterbullet_idle_two_001",
			"CakeMod/Resources/jesterbullet_idle_two_002",
			//prepare
			"CakeMod/Resources/jesterbullet_prepare_001",
			"CakeMod/Resources/jesterbullet_prepare_002",
			"CakeMod/Resources/jesterbullet_prepare_003",
			"CakeMod/Resources/jesterbullet_prepare_004",
		};

		public class TeleportScript : Script // This BulletScript is just a modified version of the script BulletManShroomed, which you can find with dnSpy.
		{
			protected override IEnumerator Top()
			{
				PlayerController player = GameManager.Instance.PrimaryPlayer;
				LootEngine.DoDefaultPurplePoof(player.sprite.WorldBottomCenter);
				AkSoundEngine.PostEvent("Play_AGUNIM_VO_FIGHT_LAUGH_02", this.BulletBank.aiActor.gameObject);
				IntVector2 meceea = new IntVector2(this.BulletBank.aiActor.GetAbsoluteParentRoom().GetRandomAvailableCellDumb().x, this.BulletBank.aiActor.GetAbsoluteParentRoom().GetRandomAvailableCellDumb().y);
				this.TeleportToPoint(meceea.ToCenterVector2(), false);
				yield break;
			}

			public void TeleportToPoint(Vector2 targetPoint, bool useDefaultTeleportVFX)
			{
				GameObject gameObject = null;
				if (useDefaultTeleportVFX)
				{
					gameObject = (GameObject)ResourceCache.Acquire("Global VFX/VFX_Teleport_Beam");
				}
				GameManager.Instance.PrimaryPlayer.StartCoroutine(this.HandleTeleportToPoint(targetPoint, gameObject, null, gameObject, GameManager.Instance.PrimaryPlayer));
			}

			// Token: 0x06008129 RID: 33065 RVA: 0x00333610 File Offset: 0x00331810
			private IEnumerator HandleTeleportToPoint(Vector2 targetPoint, GameObject departureVFXPrefab, GameObject arrivalVFX1Prefab, GameObject arrivalVFX2Prefab, PlayerController teleporter)
			{
				teleporter.healthHaver.IsVulnerable = false;
				CameraController cameraController = GameManager.Instance.MainCameraController;
				Vector2 offsetVector = cameraController.transform.position - teleporter.transform.position;
				offsetVector -= cameraController.GetAimContribution();
				Minimap.Instance.ToggleMinimap(false, false);
				cameraController.SetManualControl(true, false);
				cameraController.OverridePosition = cameraController.transform.position;
				teleporter.CurrentInputState = PlayerInputState.NoInput;
				yield return new WaitForSeconds(0.1f);
				teleporter.ToggleRenderer(false, "arbitrary teleporter.");
				teleporter.ToggleGunRenderers(false, "arbitrary teleporter.");
				teleporter.ToggleHandRenderers(false, "arbitrary teleporter.");
				if (departureVFXPrefab != null)
				{
					GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(departureVFXPrefab);
					gameObject.GetComponent<tk2dBaseSprite>().PlaceAtLocalPositionByAnchor(teleporter.specRigidbody.UnitBottomCenter + new Vector2(0f, -0.5f), tk2dBaseSprite.Anchor.LowerCenter);
					gameObject.transform.position = gameObject.transform.position.Quantize(0.0625f);
					gameObject.GetComponent<tk2dBaseSprite>().UpdateZDepth();
				}
				yield return new WaitForSeconds(0.4f);
				Pixelator.Instance.FadeToBlack(0.1f, false, 0f);
				yield return new WaitForSeconds(0.1f);
				teleporter.specRigidbody.Position = new Position(targetPoint);
				if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
				{
					cameraController.OverridePosition = cameraController.GetIdealCameraPosition();
				}
				else
				{
					cameraController.OverridePosition = (targetPoint + offsetVector).ToVector3ZUp(0f);
				}
				Pixelator.Instance.MarkOcclusionDirty();
				if (arrivalVFX1Prefab != null)
				{
					GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(arrivalVFX1Prefab);
					gameObject2.transform.position = targetPoint;
					gameObject2.transform.position = gameObject2.transform.position.Quantize(0.0625f);
				}
				Pixelator.Instance.FadeToBlack(0.1f, true, 0f);
				yield return null;
				cameraController.SetManualControl(false, true);
				yield return new WaitForSeconds(0.75f);
				if (arrivalVFX2Prefab != null)
				{
					GameObject gameObject3 = UnityEngine.Object.Instantiate<GameObject>(arrivalVFX2Prefab);
					gameObject3.GetComponent<tk2dBaseSprite>().PlaceAtLocalPositionByAnchor(teleporter.specRigidbody.UnitBottomCenter + new Vector2(0f, -0.5f), tk2dBaseSprite.Anchor.LowerCenter);
					gameObject3.transform.position = gameObject3.transform.position.Quantize(0.0625f);
					gameObject3.GetComponent<tk2dBaseSprite>().UpdateZDepth();
				}
				teleporter.DoVibration(Vibration.Time.Normal, Vibration.Strength.Medium);
				yield return new WaitForSeconds(0.25f);
				LootEngine.DoDefaultPurplePoof(teleporter.sprite.WorldBottomCenter);
				teleporter.ToggleRenderer(true, "arbitrary teleporter.");
				teleporter.ToggleGunRenderers(true, "arbitrary teleporter.");
				teleporter.ToggleHandRenderers(true, "arbitrary teleporter.");
				PhysicsEngine.Instance.RegisterOverlappingGhostCollisionExceptions(teleporter.specRigidbody, null, false);
				teleporter.CurrentInputState = PlayerInputState.AllInput;
				teleporter.healthHaver.IsVulnerable = true;
				yield break;
			}

			// Token: 0x0600812A RID: 33066 RVA: 0x00333648 File Offset: 0x00331848
			public bool IsPositionObscuredByTopWall(Vector2 newPosition)
			{
				for (int i = 0; i < 2; i++)
				{
					for (int j = 0; j < 2; j++)
					{
						int x = newPosition.ToIntVector2(VectorConversions.Floor).x + i;
						int y = newPosition.ToIntVector2(VectorConversions.Floor).y + j;
						if (GameManager.Instance.Dungeon.data.CheckInBoundsAndValid(newPosition.ToIntVector2(VectorConversions.Floor) + new IntVector2(i, j)) && (GameManager.Instance.Dungeon.data.isTopWall(x, y) || GameManager.Instance.Dungeon.data.isWall(x, y)))
						{
							return true;
						}
					}
				}
				return false;
			}
		}
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

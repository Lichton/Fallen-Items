using System;
using System.Collections.Generic;
using Gungeon;
using ItemAPI;
using UnityEngine;
//using DirectionType = DirectionalAnimation.DirectionType;
using AnimationType = ItemAPI.BossBuilder.AnimationType;
using System.Collections;
using Dungeonator;
using System.Linq;
using Brave.BulletScript;
using GungeonAPI;

namespace CakeMod
{
	public class BulletBishop : AIActor
	{
		public static GameObject prefab;
		public static readonly string guid = "bulletbishop";
		private static tk2dSpriteCollectionData BishopCollection;
		public static GameObject shootpoint;
		private static Texture2D BossCardTexture = ResourceExtractor.GetTextureFromResource("CakeMod/Resources/floorsheet.png");
		public static string TargetVFX;
		public static void Init()
		{

			BulletBishop.BuildPrefab();
		}

		public static void BuildPrefab()
		{
			// source = EnemyDatabase.GetOrLoadByGuid("c50a862d19fc4d30baeba54795e8cb93");
			bool flag = prefab != null || BossBuilder.Dictionary.ContainsKey(guid);
			bool flag2 = flag;
			if (!flag2)
			{
				prefab = BossBuilder.BuildPrefab("Bullet Bishop", guid, spritePaths[0], new IntVector2(0, 0), new IntVector2(8, 9), false, true);
				var companion = prefab.AddComponent<EnemyBehavior>();
				companion.aiActor.knockbackDoer.weight = 200;
				companion.aiActor.MovementSpeed = 2f;
				companion.aiActor.healthHaver.PreventAllDamage = false;
				companion.aiActor.CollisionDamage = 1f;
				companion.aiActor.HasShadow = true;
				companion.aiActor.IgnoreForRoomClear = false;
				companion.aiActor.aiAnimator.HitReactChance = 0.05f;
				companion.aiActor.specRigidbody.CollideWithOthers = true;
				companion.aiActor.specRigidbody.CollideWithTileMap = true;
				companion.aiActor.PreventFallingInPitsEver = true;
				companion.aiActor.healthHaver.ForceSetCurrentHealth(500f);
				companion.aiActor.healthHaver.SetHealthMaximum(500f);
				companion.aiActor.CollisionKnockbackStrength = 5f;
				companion.aiActor.procedurallyOutlined = true;
				companion.aiActor.CanTargetPlayers = true;
				ItemsMod.Strings.Enemies.Set("#BISHOP", "Bullet Bishop");
				ItemsMod.Strings.Enemies.Set("#????", "???");
				ItemsMod.Strings.Enemies.Set("#SUBTITLE", "Ex Cathedra");
				ItemsMod.Strings.Enemies.Set("#QUOTE", "Spraying and praying!");
				companion.aiActor.healthHaver.overrideBossName = "#BISHOP";
				companion.aiActor.OverrideDisplayName = "#BISHOP";
				companion.aiActor.ActorName = "#BISHOP";
				companion.aiActor.name = "#BISHOP";
				prefab.name = companion.aiActor.OverrideDisplayName;

                {
					//SpriteBuilder.AddSpriteToCollection("CakeMod/Resources/BulletBishop/bullet_pope_idle_front_001", SpriteBuilder.ammonomiconCollection);
					//if (companion.GetComponent<EncounterTrackable>() != null)
					//{
						//UnityEngine.Object.Destroy(companion.GetComponent<EncounterTrackable>());
					//}
					//companion.encounterTrackable = companion.gameObject.AddComponent<EncounterTrackable>();
					//companion.encounterTrackable.journalData = new JournalEntry();
					//companion.encounterTrackable.EncounterGuid = "cak:bullet_bishop";
					//companion.encounterTrackable.prerequisites = new DungeonPrerequisite[0];
					//companion.encounterTrackable.journalData.SuppressKnownState = false;
					//companion.encounterTrackable.journalData.IsEnemy = true;
					//companion.encounterTrackable.journalData.SuppressInAmmonomicon = false;
					//companion.encounterTrackable.ProxyEncounterGuid = "";
					//companion.encounterTrackable.journalData.AmmonomiconSprite = "CakeMod/Resources/BulletBishop/bullet_pope_idle_front_001";
					//companion.encounterTrackable.journalData.enemyPortraitSprite = ResourceExtractor.GetTextureFromResource("CakeMod\\Resources\\BulletBishop\\miniboss_photo_bullet_bishop_001.png");
					//ItemsMod.Strings.Enemies.Set("#BULLETBISHOPNAME", "Bullet Bishop");
					//ItemsMod.Strings.Enemies.Set("#BULLETBISHOPSHDES", "Ex Cathedra");
					//ItemsMod.Strings.Enemies.Set("#BULLETBISHOPSLES", "This blessed bullet speaks with the voice of Kaliber herself. \n\n A bishop of the Cult of the Gundead, he serves the Old King in the Abbey.");
					//companion.encounterTrackable.journalData.PrimaryDisplayName = "#BULLETBISHOPNAME";
					//companion.encounterTrackable.journalData.NotificationPanelDescription = "#BULLETBISHOPSHDES";
					//companion.encounterTrackable.journalData.AmmonomiconFullEntry = "#BULLETBISHOPSLES";
					//EnemyBuilder.AddEnemyToDatabase(companion.gameObject, "cak:bullet_bishop");
					//EnemyDatabase.GetEntry("cak:bullet_bishop").ForcedPositionInAmmonomicon = 201;
					//EnemyDatabase.GetEntry("cak:bullet_bishop").isInBossTab = false;
					//EnemyDatabase.GetEntry("cak:bullet_bishop").isNormalEnemy = true;
				}

				GenericIntroDoer miniBossIntroDoer = prefab.AddComponent<GenericIntroDoer>();
				miniBossIntroDoer.triggerType = GenericIntroDoer.TriggerType.PlayerEnteredRoom;
				miniBossIntroDoer.initialDelay = 0.15f;
				miniBossIntroDoer.cameraMoveSpeed = 14;
				miniBossIntroDoer.specifyIntroAiAnimator = null;
				miniBossIntroDoer.BossMusicEvent = "Play_MUS_Boss_Theme_Beholster";
				companion.aiActor.ShadowObject = EnemyDatabase.GetOrLoadByGuid("c00390483f394a849c36143eb878998f").ShadowObject;
				companion.aiActor.HasShadow = true;
				miniBossIntroDoer.PreventBossMusic = false;
				miniBossIntroDoer.InvisibleBeforeIntroAnim = false;
				miniBossIntroDoer.preIntroAnim = string.Empty;
				miniBossIntroDoer.preIntroDirectionalAnim = string.Empty;
				miniBossIntroDoer.introAnim = "intro";
				miniBossIntroDoer.introDirectionalAnim = string.Empty;
				miniBossIntroDoer.continueAnimDuringOutro = false;
				miniBossIntroDoer.cameraFocus = null;
				miniBossIntroDoer.roomPositionCameraFocus = Vector2.zero;
				miniBossIntroDoer.restrictPlayerMotionToRoom = false;
				miniBossIntroDoer.fusebombLock = false;
				miniBossIntroDoer.AdditionalHeightOffset = 0;
				miniBossIntroDoer.portraitSlideSettings = new PortraitSlideSettings();
				miniBossIntroDoer.SkipBossCard = true;
				companion.aiActor.healthHaver.bossHealthBar = HealthHaver.BossBarType.SubbossBar;
				miniBossIntroDoer.SkipFinalizeAnimation = true;
				miniBossIntroDoer.RegenerateCache();
				//BehaviorSpeculator aIActor = EnemyDatabase.GetOrLoadByGuid("465da2bb086a4a88a803f79fe3a27677").behaviorSpeculator;
				//Tools.DebugInformation(aIActor);

				/////







				companion.aiActor.healthHaver.SetHealthMaximum(500f, null, false);
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
					ManualOffsetY = 10,
					ManualWidth = 34,
					ManualHeight = 45,
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
					ManualOffsetY = 10,
					ManualWidth = 34,
					ManualHeight = 45,
					ManualDiameter = 0,
					ManualLeftX = 0,
					ManualLeftY = 0,
					ManualRightX = 0,
					ManualRightY = 0,



				});
				companion.aiActor.CorpseObject = EnemyDatabase.GetOrLoadByGuid("01972dee89fc4404a5c408d50007dad5").CorpseObject;
				companion.aiActor.PreventBlackPhantom = false;
				AIAnimator aiAnimator = companion.aiAnimator;
				aiAnimator.IdleAnimation = new DirectionalAnimation
				{
					Type = DirectionalAnimation.DirectionType.Single,
					Prefix = "idle",
					AnimNames = new string[1],
					Flipped = new DirectionalAnimation.FlipType[1]
				};
				DirectionalAnimation anim3 = new DirectionalAnimation
				{
					Type = DirectionalAnimation.DirectionType.Single,
					AnimNames = new string[]
					{
						"incense",

					},
					Flipped = new DirectionalAnimation.FlipType[1]
				};
				aiAnimator.OtherAnimations = new List<AIAnimator.NamedDirectionalAnimation>
				{
					new AIAnimator.NamedDirectionalAnimation
					{
						name = "incense",
						anim = anim3
					}
				};
				DirectionalAnimation Itworked = new DirectionalAnimation
				{
					Type = DirectionalAnimation.DirectionType.Single,
					Prefix = "summon",
					AnimNames = new string[1],
					Flipped = new DirectionalAnimation.FlipType[1]
				};
				aiAnimator.OtherAnimations = new List<AIAnimator.NamedDirectionalAnimation>
				{
					new AIAnimator.NamedDirectionalAnimation
					{
						name = "summon",
						anim = Itworked
					}
				};
				DirectionalAnimation Hurray = new DirectionalAnimation
				{
					Type = DirectionalAnimation.DirectionType.Single,
					Prefix = "attack",
					AnimNames = new string[1],
					Flipped = new DirectionalAnimation.FlipType[1]
				};
				aiAnimator.OtherAnimations = new List<AIAnimator.NamedDirectionalAnimation>
				{
					new AIAnimator.NamedDirectionalAnimation
					{
						name = "attack",
						anim = Hurray
					}
				};
				DirectionalAnimation almostdone = new DirectionalAnimation
				{
					Type = DirectionalAnimation.DirectionType.Single,
					Prefix = "intro",
					AnimNames = new string[1],
					Flipped = new DirectionalAnimation.FlipType[1]
				};
				aiAnimator.OtherAnimations = new List<AIAnimator.NamedDirectionalAnimation>
				{
					new AIAnimator.NamedDirectionalAnimation
					{
						name = "intro",
						anim = almostdone
					}
				};
				DirectionalAnimation done = new DirectionalAnimation
				{
					Type = DirectionalAnimation.DirectionType.Single,
					Prefix = "die",
					AnimNames = new string[1],
					Flipped = new DirectionalAnimation.FlipType[1]
				};
				aiAnimator.OtherAnimations = new List<AIAnimator.NamedDirectionalAnimation>
				{
					new AIAnimator.NamedDirectionalAnimation
					{
						name = "die",
						anim = done
					}

				};
				DirectionalAnimation done2 = new DirectionalAnimation
				{
					Type = DirectionalAnimation.DirectionType.Single,
					Prefix = "holy",
					AnimNames = new string[1],
					Flipped = new DirectionalAnimation.FlipType[1]
				};
				aiAnimator.OtherAnimations = new List<AIAnimator.NamedDirectionalAnimation>
				{
					new AIAnimator.NamedDirectionalAnimation
					{
						name = "holy",
						anim = done2
					}
				};
				DirectionalAnimation done3 = new DirectionalAnimation
				{
					Type = DirectionalAnimation.DirectionType.Single,
					Prefix = "teleportIn",
					AnimNames = new string[1],
					Flipped = new DirectionalAnimation.FlipType[1]
				};
				aiAnimator.OtherAnimations = new List<AIAnimator.NamedDirectionalAnimation>
				{
					new AIAnimator.NamedDirectionalAnimation
					{
						name = "teleportIn",
						anim = done3
					}
				};
				DirectionalAnimation done34 = new DirectionalAnimation
				{
					Type = DirectionalAnimation.DirectionType.Single,
					Prefix = "teleportOut",
					AnimNames = new string[1],
					Flipped = new DirectionalAnimation.FlipType[1]
				};
				aiAnimator.OtherAnimations = new List<AIAnimator.NamedDirectionalAnimation>
				{
					new AIAnimator.NamedDirectionalAnimation
					{
						name = "teleportOut",
						anim = done34
					}
				};
				DirectionalAnimation done345 = new DirectionalAnimation
				{
					Type = DirectionalAnimation.DirectionType.Single,
					Prefix = "teleport",
					AnimNames = new string[1],
					Flipped = new DirectionalAnimation.FlipType[1]
				};
				aiAnimator.OtherAnimations = new List<AIAnimator.NamedDirectionalAnimation>
				{
					new AIAnimator.NamedDirectionalAnimation
					{
						name = "teleport",
						anim = done345
					}
				};
				DirectionalAnimation done3456 = new DirectionalAnimation
				{
					Type = DirectionalAnimation.DirectionType.Single,
					Prefix = "attack2",
					AnimNames = new string[1],
					Flipped = new DirectionalAnimation.FlipType[1]
				};
				aiAnimator.OtherAnimations = new List<AIAnimator.NamedDirectionalAnimation>
				{
					new AIAnimator.NamedDirectionalAnimation
					{
						name = "attack2",
						anim = done3456
					}
				};
				bool flag3 = BishopCollection == null;
				if (flag3)
				{
					BishopCollection = SpriteBuilder.ConstructCollection(prefab, "BISHOP_Collection");
					UnityEngine.Object.DontDestroyOnLoad(BishopCollection);
					for (int i = 0; i < spritePaths.Length; i++)
					{
						SpriteBuilder.AddSpriteToCollection(spritePaths[i], BishopCollection);
					}
					SpriteBuilder.AddAnimation(companion.spriteAnimator, BishopCollection, new List<int>
					{
0,
					1
					}, "idle", tk2dSpriteAnimationClip.WrapMode.Loop).fps = 7f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, BishopCollection, new List<int>
					{
						12,
						13,
						14,
						15,
						16,
						17,
						18,
						19
					}, "incense", tk2dSpriteAnimationClip.WrapMode.Loop).fps = 9f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, BishopCollection, new List<int>
					{
						20,
						21,
						22,
						23,
						24,
						25
					}, "summon", tk2dSpriteAnimationClip.WrapMode.Once).fps = 6f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, BishopCollection, new List<int>
					{
				2,
				3,
				4,
				5,
				6,
				7,
				8,
				9,
				10,
				11
					}, "attack", tk2dSpriteAnimationClip.WrapMode.Once).fps = 7f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, BishopCollection, new List<int>
					{
				26,
				27,
				28,
				29,
				30,
				31,
				32,
				33,
				34,
				35,
				36,
				37,
				38,
				39
					}, "intro", tk2dSpriteAnimationClip.WrapMode.Once).fps = 11f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, BishopCollection, new List<int>
					{
						40,
						41,
						42,
						43,
						44,
						45
					}, "die", tk2dSpriteAnimationClip.WrapMode.Once).fps = 6f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, BishopCollection, new List<int>
					{
						46,
						47,
						48,
						49,
						50
					}, "holy", tk2dSpriteAnimationClip.WrapMode.Once).fps = 6f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, BishopCollection, new List<int>
					{
						51,
						52,
						53,
						54,
						55,
						56,
						57,
						58,
						59,
					}, "teleportIn", tk2dSpriteAnimationClip.WrapMode.Once).fps = 10f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, BishopCollection, new List<int>
					{
						59,
						58,
						57,
						56,
						55,
						54,
						53,
						52,
						51
					}, "teleportOut", tk2dSpriteAnimationClip.WrapMode.Once).fps = 10f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, BishopCollection, new List<int>
					{
						60,
						61,
						62,
						63,
						64
					}, "teleport", tk2dSpriteAnimationClip.WrapMode.Once).fps = 10f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, BishopCollection, new List<int>
					{
						65,
						66,
						67,
						68,
						69,
						70,
						71,
						72
					}, "attack2", tk2dSpriteAnimationClip.WrapMode.Once).fps = 10f;

				}
				var bs = prefab.GetComponent<BehaviorSpeculator>();
				BehaviorSpeculator behaviorSpeculator = EnemyDatabase.GetOrLoadByGuid("01972dee89fc4404a5c408d50007dad5").behaviorSpeculator;
				bs.OverrideBehaviors = behaviorSpeculator.OverrideBehaviors;
				bs.OtherBehaviors = behaviorSpeculator.OtherBehaviors;
				shootpoint = new GameObject("attach");
				shootpoint.transform.parent = companion.transform;
				shootpoint.transform.position = companion.sprite.WorldCenter;
				GameObject m_CachedGunAttachPoint = companion.transform.Find("attach").gameObject;
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
				bs.AttackBehaviorGroup.AttackBehaviors = new List<AttackBehaviorGroup.AttackGroupItem>
				{
					new AttackBehaviorGroup.AttackGroupItem()
					{
						Probability = 1,
						Behavior = new ShootBehavior{
						ShootPoint = m_CachedGunAttachPoint,
						BulletScript = new CustomBulletScriptSelector(typeof(ChainScript)),
						LeadAmount = 0f,
						AttackCooldown = 3.5f,
						ChargeAnimation = "attack",
						ChargeTime = 1f,
						RequiresLineOfSight = false,
						StopDuring = ShootBehavior.StopType.Attack,
						Uninterruptible = true
						},
						NickName = "swinging duh lanturn"
					},
					new AttackBehaviorGroup.AttackGroupItem()
					{
						Probability = 1,

						Behavior = new ShootBehavior{
						ShootPoint = m_CachedGunAttachPoint,
						ReaimOnFire = true,
						BulletScript = new CustomBulletScriptSelector(typeof(AttackScript)),
						LeadAmount = 0f,
						AttackCooldown = 3.5f,
						FireAnimation = "holy",
						RequiresLineOfSight = false,
						StopDuring = ShootBehavior.StopType.Attack,
						Uninterruptible = true
						},
						NickName = "Bishop, stop whacking people with that bible!"
					},
					new AttackBehaviorGroup.AttackGroupItem()
					{
						Probability = 0,
						Behavior = new ShootBehavior{
						ShootPoint = m_CachedGunAttachPoint,
						BulletScript = new CustomBulletScriptSelector(typeof(Summon2Script)),
						LeadAmount = 0f,
						AttackCooldown = 4.5f,
						PostFireAnimation = "summon",
						RequiresLineOfSight = false,
						StopDuring = ShootBehavior.StopType.Attack,
						Uninterruptible = true
						},
						NickName = "Summonus Fuckyouius2"
					},
					new AttackBehaviorGroup.AttackGroupItem()
					{
						Probability = 1,
						Behavior = new ShootBehavior{
						ShootPoint = m_CachedGunAttachPoint,
						BulletScript = new CustomBulletScriptSelector(typeof(SummonScript)),
						LeadAmount = 0f,
						AttackCooldown = 4.5f,
						PostFireAnimation = "summon",
						RequiresLineOfSight = false,
						StopDuring = ShootBehavior.StopType.Attack,
						Uninterruptible = true
						},
						NickName = "Summonus Fuckyouius"
					},
					new AttackBehaviorGroup.AttackGroupItem()
					{
						Probability = 1,
						Behavior = new ShootBehavior{
						ShootPoint = m_CachedGunAttachPoint,
						BulletScript = new CustomBulletScriptSelector(typeof(DiagonalScript)),
						LeadAmount = 0f,
						AttackCooldown = 4.5f,
						TellAnimation = "holy",
						RequiresLineOfSight = false,
						StopDuring = ShootBehavior.StopType.Attack,
						Uninterruptible = true
						},
						NickName = "Diagonal"
					},
					new AttackBehaviorGroup.AttackGroupItem()
					{

					Probability = 1f,
					Behavior = new TeleportBehavior()
				{

					AttackableDuringAnimation = true,

					AllowCrossRoomTeleportation = false,
					teleportRequiresTransparency = false,
					hasOutlinesDuringAnim = true,
					ManuallyDefineRoom = false,
					StayOnScreen = true,
					AvoidWalls = true,
					GoneTime = 2f,
					OnlyTeleportIfPlayerUnreachable = false,
					MinDistanceFromPlayer = 4f,
					MaxDistanceFromPlayer = -1f,
					teleportOutAnim = "teleport",

					teleportInBulletScript = new CustomBulletScriptSelector(typeof(PoofScript2)),
					teleportOutBulletScript = new CustomBulletScriptSelector(typeof(PoofScript)),
					AttackCooldown = 1f,
					InitialCooldown = 0f,
					RequiresLineOfSight = false,
					roomMax = new Vector2(0,0),
					roomMin = new Vector2(0,0),
					Cooldown = 1f,

					CooldownVariance = 1f,
					InitialCooldownVariance = 0f,
					goneAttackBehavior = null,
					IsBlackPhantom = false,
					GroupName = null,
					GroupCooldown = 0f,
					MinRange = 3,
					Range = 5,
					MinHealthThreshold = 0,
					AccumulateHealthThresholds = true,
					targetAreaStyle = null,
					HealthThresholds = new float[0],
					MinWallDistance = 1,
				},
					NickName = "Incenseivized"
					},

				};


				bs.InstantFirstTick = behaviorSpeculator.InstantFirstTick;
				bs.TickInterval = behaviorSpeculator.TickInterval;
				bs.PostAwakenDelay = behaviorSpeculator.PostAwakenDelay;
				bs.RemoveDelayOnReinforce = behaviorSpeculator.RemoveDelayOnReinforce;
				bs.OverrideStartingFacingDirection = behaviorSpeculator.OverrideStartingFacingDirection;
				bs.StartingFacingDirection = behaviorSpeculator.StartingFacingDirection;
				bs.SkipTimingDifferentiator = behaviorSpeculator.SkipTimingDifferentiator;
				Game.Enemies.Add("cak:bullet_bishop", companion.aiActor);

			}
		}



		public class SummonScript : Script // This BulletScript is just a modified version of the script BulletManShroomed, which you can find with dnSpy.
		{
			protected override IEnumerator Top()
			{
				IntVector2 meceea = new IntVector2(this.BulletBank.aiActor.GetAbsoluteParentRoom().GetRandomVisibleClearSpot(2, 2).x, this.BulletBank.aiActor.GetAbsoluteParentRoom().GetRandomVisibleClearSpot(2, 2).y);
				var Enemy = EnemyDatabase.GetOrLoadByGuid("8bb5578fba374e8aae8e10b754e61d62");
				IntVector2 meceea2 = new IntVector2(this.BulletBank.aiActor.GetAbsoluteParentRoom().GetRandomVisibleClearSpot(2, 2).x, this.BulletBank.aiActor.GetAbsoluteParentRoom().GetRandomVisibleClearSpot(2, 2).y);
				AIActor.Spawn(Enemy.aiActor, meceea, GameManager.Instance.PrimaryPlayer.CurrentRoom, true, AIActor.AwakenAnimationType.Spawn, true);
				yield break;
			}

		}
		public class Summon2Script : Script // This BulletScript is just a modified version of the script BulletManShroomed, which you can find with dnSpy.
		{
			protected override IEnumerator Top()
			{
				IntVector2 meceea = new IntVector2(this.BulletBank.aiActor.GetAbsoluteParentRoom().GetRandomVisibleClearSpot(2, 2).x, this.BulletBank.aiActor.GetAbsoluteParentRoom().GetRandomVisibleClearSpot(2, 2).y);
				var Enemy = EnemyDatabase.GetOrLoadByGuid("8bb5578fba374e8aae8e10b754e61d62");
				IntVector2 meceea2 = new IntVector2(this.BulletBank.aiActor.GetAbsoluteParentRoom().GetRandomVisibleClearSpot(2, 2).x, this.BulletBank.aiActor.GetAbsoluteParentRoom().GetRandomVisibleClearSpot(2, 2).y);
				AIActor fuckyou2 = AIActor.Spawn(Enemy.aiActor, meceea, GameManager.Instance.PrimaryPlayer.CurrentRoom, true, AIActor.AwakenAnimationType.Spawn, true);
				AIActor fuckyou = AIActor.Spawn(Enemy.aiActor, meceea2, GameManager.Instance.PrimaryPlayer.CurrentRoom, true, AIActor.AwakenAnimationType.Spawn, true);
				fuckyou.sprite.renderer.material.SetFloat("_EmissivePower", 23.45f);
				fuckyou2.sprite.renderer.material.SetFloat("_EmissivePower", 23.45f);
				fuckyou.healthHaver.SetHealthMaximum(fuckyou.healthHaver.GetCurrentHealth() * 2);
				fuckyou.healthHaver.ApplyHealing(fuckyou.healthHaver.GetCurrentHealth());
				fuckyou2.healthHaver.SetHealthMaximum(fuckyou.healthHaver.GetCurrentHealth() * 2);
				fuckyou2.healthHaver.ApplyHealing(fuckyou.healthHaver.GetCurrentHealth());
				yield break;
			}

		}
		public class PoofScript : Script // This BulletScript is just a modified version of the script BulletManShroomed, which you can find with dnSpy.
		{
			protected override IEnumerator Top()
			{
				Instantiate<GameObject>(EasyVFXDatabase.IncenseVFX, base.BulletBank.aiActor.sprite.WorldBottomCenter, Quaternion.identity);
				yield break;
			}

		}

		public class PoofScript2 : Script // This BulletScript is just a modified version of the script BulletManShroomed, which you can find with dnSpy.
		{
			protected override IEnumerator Top()
			{
				Instantiate<GameObject>(EasyVFXDatabase.IncenseVFX, (base.BulletBank.aiActor.sprite.WorldBottomCenter + new Vector2(0.3f, 0)), Quaternion.identity);
				yield break;
			}

		}

		public class PoofScript3 : Script // This BulletScript is just a modified version of the script BulletManShroomed, which you can find with dnSpy.
		{
			protected override IEnumerator Top()
			{
				Instantiate<GameObject>(EasyVFXDatabase.ExplodeFirework, (base.BulletBank.aiActor.sprite.WorldBottomCenter + new Vector2(0.3f, 0)), Quaternion.identity);
				yield break;
			}

		}
		public class AttackScript : Script
		{
			private void FireSpinningLine(Vector2 start, Vector2 end, int numBullets)
			{
				start *= 0.5f;
				end *= 0.5f;
				float direction = (this.BulletManager.PlayerPosition() - base.Position).ToAngle();
				for (int i = 0; i < numBullets; i++)
				{
					Vector2 b = Vector2.Lerp(start, end, (float)i / ((float)numBullets - 1f));
					base.Fire(new Direction(direction, DirectionType.Absolute, -1f), new BulletCardinalHat1.SpinningBullet(base.Position, base.Position + b));
                   
				}
			}
			protected override IEnumerator Top()
			{
				if (this.BulletBank && this.BulletBank.aiActor && this.BulletBank.aiActor.TargetRigidbody)
				{
					base.BulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("8bb5578fba374e8aae8e10b754e61d62").bulletBank.GetBullet("hat"));
				}

				this.FireSpinningLine(new Vector2(-2f, 0f), new Vector2(2f, 0f), 8);
				this.FireSpinningLine(new Vector2(0f, -2f), new Vector2(0f, 4f), 8);
				yield return this.Wait(60);
				yield break;
			}
		}


		public class DiagonalScript : Script
		{
			protected override IEnumerator Top()
			{
				base.BulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("da797878d215453abba824ff902e21b4").bulletBank.GetBullet("snakeBullet"));
				float aimDirection = base.GetAimDirection((float)(((double)UnityEngine.Random.value >= 0.5) ? 1 : 0), 11f);
				for (int i = 0; i < 16; i++)
				{
					base.Fire(new Direction(aimDirection, DirectionType.Absolute, -1f), new Speed(11f, SpeedType.Absolute), new BashelliskSnakeBullets1.SnakeBullet(i * 3));
					base.Fire(new Direction(aimDirection + 45, DirectionType.Absolute, -1f), new Speed(11f, SpeedType.Absolute), new BashelliskSnakeBullets1.SnakeBullet(i * 3));
					base.Fire(new Direction(aimDirection - 45, DirectionType.Absolute, -1f), new Speed(11f, SpeedType.Absolute), new BashelliskSnakeBullets1.SnakeBullet(i * 3));
					base.Fire(new Direction(aimDirection + 135, DirectionType.Absolute, -1f), new Speed(11f, SpeedType.Absolute), new BashelliskSnakeBullets1.SnakeBullet(i * 3));
					base.Fire(new Direction(aimDirection - 135, DirectionType.Absolute, -1f), new Speed(11f, SpeedType.Absolute), new BashelliskSnakeBullets1.SnakeBullet(i * 3));
				}
				return null;
			}

			// Token: 0x0400008D RID: 141
			private const int NumBullets = 8;

			// Token: 0x0400008E RID: 142
			private const int BulletSpeed = 11;

			// Token: 0x0400008F RID: 143
			private const float SnakeMagnitude = 0.6f;

			// Token: 0x04000090 RID: 144
			private const float SnakePeriod = 3f;

			// Token: 0x02000026 RID: 38
			public class SnakeBullet : Bullet
			{
				// Token: 0x0600008B RID: 139 RVA: 0x00003EE1 File Offset: 0x000020E1
				public SnakeBullet(int delay) : base("snakeBullet", false, false, false)
				{
					this.delay = delay;
				}

				// Token: 0x0600008C RID: 140 RVA: 0x00003EF8 File Offset: 0x000020F8
				protected override IEnumerator Top()
				{
					this.ManualControl = true;
					yield return this.Wait(this.delay);
					Vector2 truePosition = this.Position;
					for (int i = 0; i < 360; i++)
					{
						float offsetMagnitude = Mathf.SmoothStep(-0.6f, 0.6f, Mathf.PingPong(0.5f + (float)i / 60f * 3f, 1f));
						truePosition += BraveMathCollege.DegreesToVector(this.Direction, this.Speed / 60f);
						this.Position = truePosition + BraveMathCollege.DegreesToVector(this.Direction - 90f, offsetMagnitude);
						yield return this.Wait(1);
					}
					this.Vanish(false);
					yield break;
				}

				// Token: 0x04000091 RID: 145
				private int delay;
			}
	}

		private static string[] spritePaths = new string[]
		{
			
			//idles
			"CakeMod/Resources/BulletBishop/bullet_pope_idle_front_001",
			"CakeMod/Resources/BulletBishop/bullet_pope_idle_front_002",
			//attack
			"CakeMod/Resources/BulletBishop/bullet_pope_attack_front_001",
			"CakeMod/Resources/BulletBishop/bullet_pope_attack_front_002",
			"CakeMod/Resources/BulletBishop/bullet_pope_attack_front_003",
			"CakeMod/Resources/BulletBishop/bullet_pope_attack_front_004",
			"CakeMod/Resources/BulletBishop/bullet_pope_attack_front_005",
			"CakeMod/Resources/BulletBishop/bullet_pope_attack_front_006",
			"CakeMod/Resources/BulletBishop/bullet_pope_attack_front_007",
			"CakeMod/Resources/BulletBishop/bullet_pope_attack_front_008",
			"CakeMod/Resources/BulletBishop/bullet_pope_attack_front_009",
			"CakeMod/Resources/BulletBishop/bullet_pope_attack_front_010",
			//teleport
			"CakeMod/Resources/BulletBishop/incense_teleport_poof_001",
			"CakeMod/Resources/BulletBishop/incense_teleport_poof_002",
			"CakeMod/Resources/BulletBishop/incense_teleport_poof_003",
			"CakeMod/Resources/BulletBishop/incense_teleport_poof_004",
			"CakeMod/Resources/BulletBishop/incense_teleport_poof_005",
			"CakeMod/Resources/BulletBishop/incense_teleport_poof_006",
			"CakeMod/Resources/BulletBishop/incense_teleport_poof_007",
			"CakeMod/Resources/BulletBishop/incense_teleport_poof_008",
			//summon
			"CakeMod/Resources/BulletBishop/bullet_pope_summon_001",
		"CakeMod/Resources/BulletBishop/bullet_pope_summon_002",
		"CakeMod/Resources/BulletBishop/bullet_pope_summon_003",
		"CakeMod/Resources/BulletBishop/bullet_pope_summon_004",
		"CakeMod/Resources/BulletBishop/bullet_pope_summon_005",
		"CakeMod/Resources/BulletBishop/bullet_pope_summon_006",
			//appear
			"CakeMod/Resources/BulletBishop/bullet_pope_appear_001",
			"CakeMod/Resources/BulletBishop/bullet_pope_appear_002",
			"CakeMod/Resources/BulletBishop/bullet_pope_appear_003",
			"CakeMod/Resources/BulletBishop/bullet_pope_appear_004",
			"CakeMod/Resources/BulletBishop/bullet_pope_appear_005",
			"CakeMod/Resources/BulletBishop/bullet_pope_appear_006",
			"CakeMod/Resources/BulletBishop/bullet_pope_appear_007",
			"CakeMod/Resources/BulletBishop/bullet_pope_appear_008",
			"CakeMod/Resources/BulletBishop/bullet_pope_appear_009",
			"CakeMod/Resources/BulletBishop/bullet_pope_appear_010",
			"CakeMod/Resources/BulletBishop/bullet_pope_appear_011",
			"CakeMod/Resources/BulletBishop/bullet_pope_appear_012",
			"CakeMod/Resources/BulletBishop/bullet_pope_appear_013",
			"CakeMod/Resources/BulletBishop/bullet_pope_appear_014",
			//die
			"CakeMod/Resources/BulletBishop/bullet_pope_die_001",
			"CakeMod/Resources/BulletBishop/bullet_pope_die_002",
			"CakeMod/Resources/BulletBishop/bullet_pope_die_003",
			"CakeMod/Resources/BulletBishop/bullet_pope_die_004",
			"CakeMod/Resources/BulletBishop/bullet_pope_die_005",
			"CakeMod/Resources/BulletBishop/bullet_pope_die_006",
			//holyattack
			"CakeMod/Resources/BulletBishop/bullet_pope_holy_001",
			"CakeMod/Resources/BulletBishop/bullet_pope_holy_002",
			"CakeMod/Resources/BulletBishop/bullet_pope_holy_003",
			"CakeMod/Resources/BulletBishop/bullet_pope_holy_004",
			"CakeMod/Resources/BulletBishop/bullet_pope_holy_005",
			//telepoof
			"CakeMod/Resources/BulletBishop/bullet_pope_teleport_001",
			"CakeMod/Resources/BulletBishop/bullet_pope_teleport_002",
			"CakeMod/Resources/BulletBishop/bullet_pope_teleport_003",
			"CakeMod/Resources/BulletBishop/bullet_pope_teleport_004",
			"CakeMod/Resources/BulletBishop/bullet_pope_teleport_005",
			"CakeMod/Resources/BulletBishop/bullet_pope_teleport_006",
			"CakeMod/Resources/BulletBishop/bullet_pope_teleport_007",
			"CakeMod/Resources/BulletBishop/bullet_pope_teleport_008",
			"CakeMod/Resources/BulletBishop/bullet_pope_teleport_009",
			//teleport2
			"CakeMod/Resources/BulletBishop/Telepoof/bullet_pope_teleport_001",
			"CakeMod/Resources/BulletBishop/Telepoof/bullet_pope_teleport_002",
			"CakeMod/Resources/BulletBishop/Telepoof/bullet_pope_teleport_003",
			"CakeMod/Resources/BulletBishop/Telepoof/bullet_pope_teleport_004",
			"CakeMod/Resources/BulletBishop/Telepoof/bullet_pope_teleport_005",
			//attackshort
			"CakeMod/Resources/BulletBishop/bullet_pope_attack_front_001",
			"CakeMod/Resources/BulletBishop/bullet_pope_attack_front_002",
			"CakeMod/Resources/BulletBishop/bullet_pope_attack_front_003",
			"CakeMod/Resources/BulletBishop/bullet_pope_attack_front_004",
			"CakeMod/Resources/BulletBishop/bullet_pope_attack_front_005",
			"CakeMod/Resources/BulletBishop/bullet_pope_attack_front_006",
			"CakeMod/Resources/BulletBishop/bullet_pope_attack_front_007",
			"CakeMod/Resources/BulletBishop/bullet_pope_attack_front_008",
			//moveleft

			//moveright

			//moveup

			//movedown
				};
	}

	// Token: 0x04000C9C RID: 3228

	public class ChainScript : Script
	{
		public float GetPredictDirection(Vector2 position, float leadAmount, float speed)
		{
			Vector2 vector = this.BulletManager.PlayerPosition();
			Vector2 predictedPosition = BraveMathCollege.GetPredictedPosition(vector, this.BulletManager.PlayerVelocity(), position, speed);
			vector = new Vector2(vector.x + (predictedPosition.x - vector.x) * leadAmount, vector.y + (predictedPosition.y - vector.y) * leadAmount);
			return (vector - position).ToAngle();
		}
		protected override IEnumerator Top()
		{
			if (this.BulletBank && this.BulletBank.aiActor && this.BulletBank.aiActor.TargetRigidbody)
			{
				base.BulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("d8a445ea4d944cc1b55a40f22821ae69").bulletBank.GetBullet("default"));
			}
			if (this.BulletBank && this.BulletBank.aiActor && this.BulletBank.aiActor.TargetRigidbody)
			{
				base.BulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("ffca09398635467da3b1f4a54bcfda80").bulletBank.GetBullet("bigBullet"));
			}
			float aimDirection = base.GetAimDirection(1f, 10f);
			base.Fire(new Direction(aimDirection, DirectionType.Absolute, -1f), new Speed(10f, SpeedType.Absolute), new FlareBullet(new Vector2(0.7f, 0f), 30, 15));
			base.Fire(new Direction(aimDirection, DirectionType.Absolute, -1f), new Speed(10f, SpeedType.Absolute), new FlareBullet(new Vector2(0f, 0f), 30, 15));
			base.Fire(new Direction(aimDirection, DirectionType.Absolute, -1f), new Speed(10f, SpeedType.Absolute), new FlareBullet(new Vector2(-0.7f, 0f), 30, 15));
			base.Fire(new Direction(aimDirection, DirectionType.Absolute, -1f), new Speed(10f, SpeedType.Absolute), new FlareBullet(new Vector2(-1.4f, 0f), 30, 15));
			base.Fire(new Direction(aimDirection, DirectionType.Absolute, -1f), new Speed(10f, SpeedType.Absolute), new FlareBullet(new Vector2(0f, 0.7f), 30, 15));
			base.Fire(new Direction(aimDirection, DirectionType.Absolute, -1f), new Speed(10f, SpeedType.Absolute), new FlareBullet(new Vector2(0f, -0.7f), 30, 15));
		
			yield break;
		}

			public class BigBullet : Bullet
		{
			public BigBullet(Vector2 offset, int setupDelay, int setupTime) : base("bigBullet", false, false, false)
			{
				this.m_offset = offset;
				this.m_setupDelay = setupDelay;
				this.m_setupTime = setupTime;
			}

			// Token: 0x06000C38 RID: 3128 RVA: 0x00038700 File Offset: 0x00036900
			protected override IEnumerator Top()
			{
				this.ManualControl = true;
				this.m_offset = this.m_offset.Rotate(this.Direction);
				for (int i = 0; i < 360; i++)
				{
					if (i > this.m_setupDelay && i < this.m_setupDelay + this.m_setupTime)
					{
						this.Position += this.m_offset / (float)this.m_setupTime;
					}
					this.Position += BraveMathCollege.DegreesToVector(this.Direction, this.Speed / 60f);
					yield return this.Wait(1);
				}
				this.Vanish(false);
				yield break;
			}
			private Vector2 m_offset;

			// Token: 0x04000CF1 RID: 3313
			private int m_setupDelay;

			// Token: 0x04000CF2 RID: 3314
			private int m_setupTime;
		}
		// Token: 0x04000C92 RID: 3218
	}
	public class FlareBullet : Bullet
		{
			// Token: 0x06000C37 RID: 3127 RVA: 0x000386DF File Offset: 0x000368DF
			public FlareBullet(Vector2 offset, int setupDelay, int setupTime) : base("default", false, false, false)
			{
				this.m_offset = offset;
				this.m_setupDelay = setupDelay;
				this.m_setupTime = setupTime;
			}

			// Token: 0x06000C38 RID: 3128 RVA: 0x00038700 File Offset: 0x00036900
			protected override IEnumerator Top()
			{
				this.ManualControl = true;
				this.m_offset = this.m_offset.Rotate(this.Direction);
				for (int i = 0; i < 360; i++)
				{
					if (i > this.m_setupDelay && i < this.m_setupDelay + this.m_setupTime)
					{
						this.Position += this.m_offset / (float)this.m_setupTime;
					}
					this.Position += BraveMathCollege.DegreesToVector(this.Direction, this.Speed / 60f);
					yield return this.Wait(1);
				}
				this.Vanish(false);
				yield break;
			}
			private Vector2 m_offset;

			// Token: 0x04000CF1 RID: 3313
			private int m_setupDelay;

			// Token: 0x04000CF2 RID: 3314
			private int m_setupTime;
		}
		// Token: 0x04000C92 RID: 3218



	// Token: 0x0400035E RID: 86

	public class EnemyBehavior : BraveBehaviour
	{
		public bool dothething = true;
	
			public void Update(AttackBehaviorGroup attackGroup)
		{
			for (int i = 0; i < attackGroup.AttackBehaviors.Count; i++)
			{
				if (dothething == true)
				{
					dothething = false;
					AttackBehaviorGroup.AttackGroupItem attackGroupItem = attackGroup.AttackBehaviors[i];
					if (attackGroupItem.Behavior is ShootBehavior && attackGroup != null && attackGroupItem.NickName == "Summonus Fuckyouius")
					{
						attackGroupItem.Probability = 0f;
					}
					else if (attackGroupItem.Behavior is ShootBehavior && attackGroup != null && attackGroupItem.NickName == "Summonus Fuckyouius2")
					{
						attackGroupItem.Probability = 1f;
					}
				}
			}
		}

		public void Start()
		{
			//base.aiActor.HasBeenEngaged = true;
			if(healthHaver.healthHaver.GetCurrentHealth() <= healthHaver.healthHaver.GetMaxHealth() / 2)
            {
				var bs = BulletBishop.prefab.GetComponent<BehaviorSpeculator>();
			}
			base.aiActor.healthHaver.OnPreDeath += (obj) =>
			{
				AkSoundEngine.PostEvent("Play_ENM_beholster_death_01", base.aiActor.gameObject);
				//Chest chest2 = GameManager.Instance.RewardManager.SpawnTotallyRandomChest(spawnspot);
				//chest2.IsLocked = false;

			};
			base.healthHaver.healthHaver.OnDeath += (obj) =>
			{
				

			}; ;
			this.aiActor.knockbackDoer.SetImmobile(true, "fuckshitdeath");
		}

	}
	public class ShotgunExecutionerChain2 : Script
	{
		// Token: 0x06000BC1 RID: 3009 RVA: 0x00036F38 File Offset: 0x00035138
		protected override IEnumerator Top()
		{
			base.BulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("b1770e0f1c744d9d887cc16122882b4f").bulletBank.GetBullet("chain"));
			this.EndOnBlank = true;
			ShotgunExecutionerChain2.HandBullet handBullet = null;
			for (int i = 0; i < 5; i++)
			{
				handBullet = this.FireVolley(i, (float)(20 + i * 5));
				if (i < 4)
				{
					yield return this.Wait(30);
				}
			}
			while (!handBullet.IsEnded && !handBullet.HasStopped)
			{
				yield return this.Wait(1);
			}
			yield return this.Wait(120);
			yield break;
		}

		// Token: 0x06000BC2 RID: 3010 RVA: 0x00036F54 File Offset: 0x00035154
		private ShotgunExecutionerChain2.HandBullet FireVolley(int volleyIndex, float speed)
		{
			ShotgunExecutionerChain2.HandBullet handBullet = new ShotgunExecutionerChain2.HandBullet(this);
			base.Fire(new Direction(base.AimDirection, DirectionType.Absolute, -1f), new Speed(speed, SpeedType.Absolute), handBullet);
			for (int i = 0; i < 20; i++)
			{
				base.Fire(new Direction(base.AimDirection, DirectionType.Absolute, -1f), new ShotgunExecutionerChain2.ArmBullet(this, handBullet, i));
			}
			return handBullet;
		}

		// Token: 0x04000C92 RID: 3218
		private const int NumArmBullets = 20;

		// Token: 0x04000C93 RID: 3219
		private const int NumVolley = 3;

		// Token: 0x04000C94 RID: 3220
		private const int FramesBetweenVolleys = 30;

		// Token: 0x020002F8 RID: 760
		private class ArmBullet : Bullet
		{
			// Token: 0x06000BC3 RID: 3011 RVA: 0x00036FBA File Offset: 0x000351BA
			public ArmBullet(ShotgunExecutionerChain2 parentScript, ShotgunExecutionerChain2.HandBullet handBullet, int index) : base("chain", false, false, false)
			{
				this.m_parentScript = parentScript;
				this.m_handBullet = handBullet;
				this.m_index = index;
			}

			// Token: 0x06000BC4 RID: 3012 RVA: 0x00036FE0 File Offset: 0x000351E0
			protected override IEnumerator Top()
			{
				this.ManualControl = true;
				while (!this.m_parentScript.IsEnded && !this.m_handBullet.IsEnded && !this.m_handBullet.HasStopped && this.BulletBank)
				{
					this.Position = Vector2.Lerp(this.m_parentScript.Position, this.m_handBullet.Position, (float)this.m_index / 20f);
					yield return this.Wait(1);
				}
				if (this.m_parentScript.IsEnded)
				{
					this.Vanish(false);
					yield break;
				}
				int delay = 20 - this.m_index - 5;
				if (delay > 0)
				{
					yield return this.Wait(delay);
				}
				float currentOffset = 0f;
				Vector2 truePosition = this.Position;
				int halfWiggleTime = 10;
				for (int i = 0; i < 30; i++)
				{
					if (i == 0 && delay < 0)
					{
						i = -delay;
					}
					float magnitude = 0.4f;
					magnitude = Mathf.Min(magnitude, Mathf.Lerp(0.2f, 0.4f, (float)this.m_index / 8f));
					magnitude = Mathf.Min(magnitude, Mathf.Lerp(0.2f, 0.4f, (float)(20 - this.m_index - 1) / 3f));
					magnitude = Mathf.Lerp(magnitude, 0f, (float)i / (float)halfWiggleTime - 2f);
					currentOffset = Mathf.SmoothStep(-magnitude, magnitude, Mathf.PingPong(0.5f + (float)i / (float)halfWiggleTime, 1f));
					this.Position = truePosition + BraveMathCollege.DegreesToVector(this.Direction - 90f, currentOffset);
					yield return this.Wait(1);
				}
				yield return this.Wait(this.m_index + 1 + 60);
				this.Vanish(false);
				yield break;
			}

			// Token: 0x04000C95 RID: 3221
			public const int BulletDelay = 60;

			// Token: 0x04000C96 RID: 3222
			private const float WiggleMagnitude = 0.4f;

			// Token: 0x04000C97 RID: 3223
			public const int WiggleTime = 30;

			// Token: 0x04000C98 RID: 3224
			private const int NumBulletsToPreShake = 5;

			// Token: 0x04000C99 RID: 3225
			private ShotgunExecutionerChain2 m_parentScript;

			// Token: 0x04000C9A RID: 3226
			private ShotgunExecutionerChain2 shotgunExecutionerChain1;

			// Token: 0x04000C9B RID: 3227
			private ShotgunExecutionerChain2.HandBullet m_handBullet;

			// Token: 0x04000C9C RID: 3228
			private int m_index;
		}

		// Token: 0x020002FA RID: 762
		private class HandBullet : Bullet
		{
			// Token: 0x06000BCB RID: 3019 RVA: 0x0003738A File Offset: 0x0003558A
			public HandBullet(ShotgunExecutionerChain2 parentScript) : base("chain", false, false, false)
			{
				this.m_parentScript = parentScript;
			}

			// Token: 0x170002C0 RID: 704
			// (get) Token: 0x06000BCC RID: 3020 RVA: 0x000373A1 File Offset: 0x000355A1
			// (set) Token: 0x06000BCD RID: 3021 RVA: 0x000373A9 File Offset: 0x000355A9
			public bool HasStopped { get; set; }

			// Token: 0x06000BCE RID: 3022 RVA: 0x000373B4 File Offset: 0x000355B4
			protected override IEnumerator Top()
			{
				this.Projectile.BulletScriptSettings.surviveRigidbodyCollisions = true;
				this.Projectile.BulletScriptSettings.surviveTileCollisions = true;
				SpeculativeRigidbody specRigidbody = this.Projectile.specRigidbody;
				specRigidbody.OnCollision = (Action<CollisionData>)Delegate.Combine(specRigidbody.OnCollision, new Action<CollisionData>(this.OnCollision));
				while (!this.m_parentScript.IsEnded && !this.HasStopped)
				{
					yield return this.Wait(1);
				}
				if (this.m_parentScript.IsEnded)
				{
					this.Vanish(false);
					yield break;
				}
				yield return this.Wait(111);
				this.Vanish(false);
				yield break;
			}

			// Token: 0x06000BCF RID: 3023 RVA: 0x000373D0 File Offset: 0x000355D0
			private void OnCollision(CollisionData collision)
			{
				bool flag = collision.collisionType == CollisionData.CollisionType.TileMap;
				SpeculativeRigidbody otherRigidbody = collision.OtherRigidbody;
				if (otherRigidbody)
				{
					flag = (otherRigidbody.majorBreakable || otherRigidbody.PreventPiercing || (!otherRigidbody.gameActor && !otherRigidbody.minorBreakable));
				}
				if (flag)
				{
					base.Position = collision.MyRigidbody.UnitCenter + PhysicsEngine.PixelToUnit(collision.NewPixelsToMove);
					this.Speed = 0f;
					this.HasStopped = true;
					PhysicsEngine.PostSliceVelocity = new Vector2?(new Vector2(0f, 0f));
					SpeculativeRigidbody specRigidbody = this.Projectile.specRigidbody;
					specRigidbody.OnCollision = (Action<CollisionData>)Delegate.Remove(specRigidbody.OnCollision, new Action<CollisionData>(this.OnCollision));
				}
				else
				{
					PhysicsEngine.PostSliceVelocity = new Vector2?(collision.MyRigidbody.Velocity);
				}
			}

			// Token: 0x06000BD0 RID: 3024 RVA: 0x000374DC File Offset: 0x000356DC
			public override void OnBulletDestruction(Bullet.DestroyType destroyType, SpeculativeRigidbody hitRigidbody, bool preventSpawningProjectiles)
			{
				if (this.Projectile)
				{
					SpeculativeRigidbody specRigidbody = this.Projectile.specRigidbody;
					specRigidbody.OnCollision = (Action<CollisionData>)Delegate.Remove(specRigidbody.OnCollision, new Action<CollisionData>(this.OnCollision));
				}
				this.HasStopped = true;
			}

			// Token: 0x04000CA8 RID: 3240
			private ShotgunExecutionerChain2 m_parentScript;

		}
	}

}


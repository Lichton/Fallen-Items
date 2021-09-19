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
	public class KillShrine : AIActor
	{
		public static GameObject prefab;
		public static readonly string guid = "killshrine";
		private static tk2dSpriteCollectionData BishopCollection;
		public static GameObject shootpoint;
		private static Texture2D BossCardTexture = ResourceExtractor.GetTextureFromResource("CakeMod/Resources/KillShrine/killshrine_bosscard.png");
		public static string TargetVFX;
		public static void Init()
		{

			KillShrine.BuildPrefab();
		}
		static private DamageTypeModifier m_fireImmunity;

		public static void BuildPrefab()
		{
			// source = EnemyDatabase.GetOrLoadByGuid("c50a862d19fc4d30baeba54795e8cb93");
			bool flag = prefab != null || BossBuilder.Dictionary.ContainsKey(guid);
			bool flag2 = flag;
			if (!flag2)
			{
				prefab = BossBuilder.BuildPrefab("Kill Shrine", guid, spritePaths[0], new IntVector2(0, 0), new IntVector2(8, 9), false, true);
				var companion = prefab.AddComponent<EnemyBehavior23>();

				companion.aiActor.knockbackDoer.weight = 200;
				companion.aiActor.MovementSpeed = 6f;
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

				ItemsMod.Strings.Enemies.Set("#KILLSHRINE", "Sepulchergiest");
				ItemsMod.Strings.Enemies.Set("#????", "???");
				ItemsMod.Strings.Enemies.Set("#KILLSHRINESUBTITLE", "KILL SHRINE");
				ItemsMod.Strings.Enemies.Set("#KILLSHRINEQUOTE", "THE SHRINING");
				companion.aiActor.healthHaver.overrideBossName = "#KILLSHRINE";
				companion.aiActor.OverrideDisplayName = "#KILLSHRINE";
				companion.aiActor.ActorName = "#KILLSHRINE";
				companion.aiActor.name = "#KILLSHRINE";
				companion.aiActor.SetIsFlying(true, "Flying Enemy", true, true);

				prefab.name = companion.aiActor.OverrideDisplayName;

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
				miniBossIntroDoer.portraitSlideSettings = new PortraitSlideSettings()
				{
					bossNameString = "#KILLSHRINE",
					bossSubtitleString = "#KILLSHRINESUBTITLE",
					bossQuoteString = "#KILLSHRINEQUOTE",
					bossSpritePxOffset = IntVector2.Zero,
					topLeftTextPxOffset = IntVector2.Zero,
					bottomRightTextPxOffset = IntVector2.Zero,
					bgColor = Color.cyan
				};
				miniBossIntroDoer.SkipBossCard = false;
				miniBossIntroDoer.portraitSlideSettings.bossArtSprite = BossCardTexture;
				companion.aiActor.healthHaver.bossHealthBar = HealthHaver.BossBarType.MainBar;
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
					ManualOffsetX = 15,
					ManualOffsetY = 10,
					ManualWidth = 60,
					ManualHeight = 80,
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
					ManualOffsetX = 15,
					ManualOffsetY = 10,
					ManualWidth = 60,
					ManualHeight = 80,
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
					Type = DirectionalAnimation.DirectionType.Single,
					Prefix = "idle",
					AnimNames = new string[1],
					Flipped = new DirectionalAnimation.FlipType[1]
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
				bool flag3 = BishopCollection == null;
				if (flag3)
				{
					BishopCollection = SpriteBuilder.ConstructCollection(prefab, "FlameChamber_Collection");
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
				2,
				3,
				4,
				5,
				6,
				7,
				8
					}, "intro", tk2dSpriteAnimationClip.WrapMode.Once).fps = 7f;

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
				float[] angles = { 45, 135, 225, 135 };
				bs.MovementBehaviors = new List<MovementBehaviorBase>
			{
				
			};

				bs.AttackBehaviorGroup.AttackBehaviors = new List<AttackBehaviorGroup.AttackGroupItem>
				{
				
				};


				bs.InstantFirstTick = behaviorSpeculator.InstantFirstTick;
				bs.TickInterval = behaviorSpeculator.TickInterval;
				bs.PostAwakenDelay = behaviorSpeculator.PostAwakenDelay;
				bs.RemoveDelayOnReinforce = behaviorSpeculator.RemoveDelayOnReinforce;
				bs.OverrideStartingFacingDirection = behaviorSpeculator.OverrideStartingFacingDirection;
				bs.StartingFacingDirection = behaviorSpeculator.StartingFacingDirection;
				bs.SkipTimingDifferentiator = behaviorSpeculator.SkipTimingDifferentiator;
				Game.Enemies.Add("cak:killshrine", companion.aiActor);

			}
		}
		public class SummonScript : Script // This BulletScript is just a modified version of the script BulletManShroomed, which you can find with dnSpy.
		{
			protected override IEnumerator Top()
			{

				AkSoundEngine.PostEvent("Play_ENV_oilfire_ignite_01", base.BulletBank.aiActor.gameObject);
				IntVector2 meceea = new IntVector2(this.BulletBank.aiActor.GetAbsoluteParentRoom().GetRandomAvailableCellDumb().x, this.BulletBank.aiActor.GetAbsoluteParentRoom().GetRandomAvailableCellDumb().y);
				var Enemy = EnemyDatabase.GetOrLoadByGuid("flameclone");
				Instantiate<GameObject>(EasyVFXDatabase.FlameVFX, meceea.ToVector3(), Quaternion.identity);
				AIActor.Spawn(Enemy.aiActor, meceea, GameManager.Instance.PrimaryPlayer.CurrentRoom, true, AIActor.AwakenAnimationType.Default, true);
				yield break;
			}

		}
		public class PoofScript : Script // This BulletScript is just a modified version of the script BulletManShroomed, which you can find with dnSpy.
		{
			protected override IEnumerator Top()
			{
				if (this.BulletBank && this.BulletBank.aiActor && this.BulletBank.aiActor.TargetRigidbody)
				{
					base.BulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("d8a445ea4d944cc1b55a40f22821ae69").bulletBank.GetBullet("default"));
				}
				base.Fire(new Direction(0f), new Speed(8f, SpeedType.Absolute), new FlamenBullet());
				base.Fire(new Direction(20f), new Speed(8f, SpeedType.Absolute), new FlamenBullet());
				base.Fire(new Direction(40f), new Speed(8f, SpeedType.Absolute), new FlamenBullet());
				base.Fire(new Direction(60f), new Speed(8f, SpeedType.Absolute), new FlamenBullet());
				base.Fire(new Direction(80f), new Speed(8f, SpeedType.Absolute), new FlamenBullet());
				base.Fire(new Direction(100f), new Speed(8f, SpeedType.Absolute), new FlamenBullet());
				base.Fire(new Direction(120f), new Speed(8f, SpeedType.Absolute), new FlamenBullet());
				base.Fire(new Direction(140f), new Speed(8f, SpeedType.Absolute), new FlamenBullet());
				base.Fire(new Direction(160f), new Speed(8f, SpeedType.Absolute), new FlamenBullet());
				base.Fire(new Direction(180f), new Speed(8f, SpeedType.Absolute), new FlamenBullet());
				base.Fire(new Direction(200f), new Speed(8f, SpeedType.Absolute), new FlamenBullet());
				base.Fire(new Direction(220f), new Speed(8f, SpeedType.Absolute), new FlamenBullet());
				base.Fire(new Direction(240f), new Speed(8f, SpeedType.Absolute), new FlamenBullet());
				base.Fire(new Direction(260f), new Speed(8f, SpeedType.Absolute), new FlamenBullet());
				base.Fire(new Direction(280f), new Speed(8f, SpeedType.Absolute), new FlamenBullet());
				base.Fire(new Direction(300f), new Speed(8f, SpeedType.Absolute), new FlamenBullet());
				base.Fire(new Direction(320f), new Speed(8f, SpeedType.Absolute), new FlamenBullet());
				base.Fire(new Direction(340f), new Speed(8f, SpeedType.Absolute), new FlamenBullet());
				base.Fire(new Direction(360f), new Speed(8f, SpeedType.Absolute), new FlamenBullet());
				Instantiate<GameObject>(EasyVFXDatabase.FlameVFX, base.BulletBank.aiActor.sprite.WorldBottomCenter, Quaternion.identity);
				yield break;
			}
			public class FlamenBullet : Bullet
			{
				public FlamenBullet() : base("default", false, false, false)
				{

				}
			}
		}

		public class ChamberShot : Script // This BulletScript is just a modified version of the script BulletManShroomed, which you can find with dnSpy.
		{
			protected override IEnumerator Top()
			{
				if (this.BulletBank && this.BulletBank.aiActor && this.BulletBank.aiActor.TargetRigidbody)
				{
					base.BulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("d8a445ea4d944cc1b55a40f22821ae69").bulletBank.GetBullet("default"));
					base.BulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("383175a55879441d90933b5c4e60cf6f").bulletBank.GetBullet("bigBullet"));
				}

				base.Fire(new Offset(new Vector2((float)1, (float)0.4)), new Direction(0f, DirectionType.Aim), new Speed(8f, SpeedType.Absolute), new FlamenBullet());
				base.Fire(new Offset(new Vector2((float)1, (float)-0.4)), new Direction(0f, DirectionType.Aim), new Speed(8f, SpeedType.Absolute), new FlamenBullet());
				base.Fire(new Offset(new Vector2((float)-1, (float)0.4)), new Direction(0f, DirectionType.Aim), new Speed(8f, SpeedType.Absolute), new FlamenBullet());
				base.Fire(new Offset(new Vector2((float)-1, (float)-0.4)), new Direction(0f, DirectionType.Aim), new Speed(8f, SpeedType.Absolute), new FlamenBullet());
				base.Fire(new Offset(new Vector2(0, (float)1)), new Direction(0f, DirectionType.Aim), new Speed(8f, SpeedType.Absolute), new FlamenBullet());
				base.Fire(new Offset(new Vector2(0, (float)-1)), new Direction(0f, DirectionType.Aim), new Speed(8f, SpeedType.Absolute), new FlamenBullet());
				base.Fire(new Offset(new Vector2(0, (float)0)), new Direction(0f, DirectionType.Aim), new Speed(8f, SpeedType.Absolute), new ChamberBullet());
				yield break;
			}
			public class FlamenBullet : Bullet
			{
				public FlamenBullet() : base("default", false, false, false)
				{

				}
			}

			public class ChamberBullet : Bullet
			{
				int i = 0;
				public ChamberBullet() : base("bigBullet", false, false, false)
				{

					i++;
					if (i >= 60)
					{
						i = 0;

					}
				}

				protected override IEnumerator Top()
				{
					yield return this.Wait(30);
					GameObject gameobject = base.BulletBank.CreateProjectileFromBank(this.Projectile.sprite.WorldCenter, (this.BulletManager.PlayerPosition() - this.Projectile.sprite.WorldCenter).ToAngle(), "default");
					yield return this.Wait(30);
					GameObject gameobject4 = base.BulletBank.CreateProjectileFromBank(this.Projectile.sprite.WorldCenter, (this.BulletManager.PlayerPosition() - this.Projectile.sprite.WorldCenter).ToAngle(), "default");
					yield return this.Wait(30);
					GameObject gameobject3 = base.BulletBank.CreateProjectileFromBank(this.Projectile.sprite.WorldCenter, (this.BulletManager.PlayerPosition() - this.Projectile.sprite.WorldCenter).ToAngle(), "default");
					yield return this.Wait(30);
					GameObject gameobject2 = base.BulletBank.CreateProjectileFromBank(this.Projectile.sprite.WorldCenter, (this.BulletManager.PlayerPosition() - this.Projectile.sprite.WorldCenter).ToAngle(), "default");
					yield return this.Wait(30);
					GameObject gameobject5 = base.BulletBank.CreateProjectileFromBank(this.Projectile.sprite.WorldCenter, (this.BulletManager.PlayerPosition() - this.Projectile.sprite.WorldCenter).ToAngle(), "default");
					yield return this.Wait(30);
					GameObject gameobject6 = base.BulletBank.CreateProjectileFromBank(this.Projectile.sprite.WorldCenter, (this.BulletManager.PlayerPosition() - this.Projectile.sprite.WorldCenter).ToAngle(), "default");
					yield return this.Wait(30);
					GameObject gameobject7 = base.BulletBank.CreateProjectileFromBank(this.Projectile.sprite.WorldCenter, (this.BulletManager.PlayerPosition() - this.Projectile.sprite.WorldCenter).ToAngle(), "default");

					yield break;
				}
			}
		}

		public class FireLine : Script // This BulletScript is just a modified version of the script BulletManShroomed, which you can find with dnSpy.
		{
			protected override IEnumerator Top()
			{

				DeadlyDeadlyGoopManager goopManagerForGoopType = DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(EasyGoopDefinitions.FireDef);
				goopManagerForGoopType.TimedAddGoopLine(base.BulletBank.aiActor.CenterPosition, this.BulletManager.PlayerPosition(), 2, 0.25f);

				yield break;
			}
		}

		public class ShotgunBall : Script // This BulletScript is just a modified version of the script BulletManShroomed, which you can find with dnSpy.
		{
			protected override IEnumerator Top()
			{
				if (this.BulletBank && this.BulletBank.aiActor && this.BulletBank.aiActor.TargetRigidbody)
				{
					base.BulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("383175a55879441d90933b5c4e60cf6f").bulletBank.GetBullet("bigBullet"));
					base.BulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("d8a445ea4d944cc1b55a40f22821ae69").bulletBank.GetBullet("default"));
				}
				base.Fire(new Direction(0, DirectionType.Absolute), new Speed(8f, SpeedType.Absolute), new BigBullet());
				base.Fire(new Direction(310, DirectionType.Absolute), new Speed(8f, SpeedType.Absolute), new BigBullet());
				base.Fire(new Direction(250, DirectionType.Absolute), new Speed(8f, SpeedType.Absolute), new BigBullet());
				base.Fire(new Direction(70, DirectionType.Absolute), new Speed(8f, SpeedType.Absolute), new BigBullet());
				base.Fire(new Direction(130, DirectionType.Absolute), new Speed(8f, SpeedType.Absolute), new BigBullet());
				base.Fire(new Direction(180, DirectionType.Absolute), new Speed(8f, SpeedType.Absolute), new BigBullet());
				yield break;
			}
			public class BigBullet : Bullet
			{
				public BigBullet() : base("bigBullet", false, false, false)
				{
				}
				public override void OnBulletDestruction(Bullet.DestroyType destroyType, SpeculativeRigidbody hitRigidbody, bool preventSpawningProjectiles)
				{
					for (int i = 0; i < 11; i++)
					{
						GameObject gameobject = base.BulletBank.CreateProjectileFromBank(this.Projectile.sprite.WorldCenter, (this.BulletManager.PlayerPosition() - this.Projectile.sprite.WorldCenter).ToAngle() + UnityEngine.Random.Range(-20, 20), "default");
						Projectile proj = gameobject.GetComponent<Projectile>();
						proj.IgnoreTileCollisionsFor(2f);
					}
				}

			}

			public class FlamenBullet : Bullet
			{
				public FlamenBullet() : base("default", false, false, false)
				{

				}
			}
		}
		private static string[] spritePaths = new string[]
		{
			
			//idles
			"CakeMod/Resources/KillShrine/killshrine_idle_001",
			"CakeMod/Resources/KillShrine/killshrine_idle_002",
			//intro
			"CakeMod/Resources/KillShrine/killshrine_appear_001",
			"CakeMod/Resources/KillShrine/killshrine_appear_002",
			"CakeMod/Resources/KillShrine/killshrine_appear_003",
			"CakeMod/Resources/KillShrine/killshrine_appear_004",
			"CakeMod/Resources/KillShrine/killshrine_appear_005",
			"CakeMod/Resources/KillShrine/killshrine_appear_006",
			"CakeMod/Resources/KillShrine/killshrine_appear_007",
				};
	}

	// Token: 0x04000C9C RID: 3228
	public class EnemyBehavior23 : BraveBehaviour
	{
		public bool dothething = true;

		public void Update()
		{
			if (Time.frameCount % 5 == 0)
			{

			}
		}
		public void Start()
		{
			//base.aiActor.HasBeenEngaged = true;
			if (healthHaver.healthHaver.GetCurrentHealth() <= healthHaver.healthHaver.GetMaxHealth() / 2)
			{
				var bs = FlameChamber.prefab.GetComponent<BehaviorSpeculator>();
			}
			base.aiActor.healthHaver.OnPreDeath += (obj) =>
			{
				
				AkSoundEngine.PostEvent("Play_ENM_highpriest_blast_01", base.aiActor.gameObject);
				//Chest chest2 = GameManager.Instance.RewardManager.SpawnTotallyRandomChest(spawnspot);
				//chest2.IsLocked = false;
				 
			};
			base.healthHaver.healthHaver.OnDeath += (obj) =>
			{


			}; ;
			this.aiActor.knockbackDoer.SetImmobile(true, "fuckshitdeath");
		}

	}
}

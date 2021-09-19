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
	public class Mimekin : AIActor
	{
		public static GameObject prefab;
		public static readonly string guid = "mimekin";
		private static tk2dSpriteCollectionData RoyalJesterletCollection;
		public static GameObject shootpoint;

		public static void Init()
		{

			Mimekin.BuildPrefab();
		}

		public static void BuildPrefab()
		{
			// source = EnemyDatabase.GetOrLoadByGuid("c50a862d19fc4d30baeba54795e8cb93");
			bool flag = prefab != null || EnemyBuilder.Dictionary.ContainsKey(guid);
			bool flag2 = flag;
			if (!flag2)
			{
				prefab = EnemyBuilder.BuildPrefab("mimekin", guid, spritePaths[0], new IntVector2(0, 0), new IntVector2(8, 9), false);
				var companion = prefab.AddComponent<EnemyBehavior>();
				companion.aiActor.knockbackDoer.weight = 200;
				companion.aiActor.MovementSpeed = 2f;
				companion.aiActor.healthHaver.PreventAllDamage = false;
				companion.aiActor.CollisionDamage = 1f;
				companion.aiActor.HasShadow = false;
				companion.aiActor.IgnoreForRoomClear = false;
				companion.aiActor.specRigidbody.CollideWithOthers = true;
				companion.aiActor.specRigidbody.CollideWithTileMap = true;
				companion.aiActor.PreventFallingInPitsEver = true;
				companion.aiActor.healthHaver.ForceSetCurrentHealth(25f);
				companion.aiActor.CollisionKnockbackStrength = 5f;
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

					2,
					3

					}, "idle", tk2dSpriteAnimationClip.WrapMode.Loop).fps = 7f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, RoyalJesterletCollection, new List<int>
					{

					0,
					1

					}, "idle_two", tk2dSpriteAnimationClip.WrapMode.Loop).fps = 7f;

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
						BulletScript = new CustomBulletScriptSelector(typeof(TransparentBulletScript)),
						LeadAmount = 0f,
						InitialCooldown = 3f,
						AttackCooldown = 5f,
						RequiresLineOfSight = false,
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
				Game.Enemies.Add("cak:mimekin", companion.aiActor);
				SpriteBuilder.AddSpriteToCollection("CakeMod/Resources/mimekin/mime_left_001", SpriteBuilder.ammonomiconCollection);
				if (companion.GetComponent<EncounterTrackable>() != null)
				{
					UnityEngine.Object.Destroy(companion.GetComponent<EncounterTrackable>());
				}
				companion.encounterTrackable = companion.gameObject.AddComponent<EncounterTrackable>();
				companion.encounterTrackable.journalData = new JournalEntry();
				companion.encounterTrackable.EncounterGuid = "cak:mimekin";
				companion.encounterTrackable.prerequisites = new DungeonPrerequisite[0];
				companion.encounterTrackable.journalData.SuppressKnownState = false;
				companion.encounterTrackable.journalData.IsEnemy = true;
				companion.encounterTrackable.journalData.SuppressInAmmonomicon = false;
				companion.encounterTrackable.ProxyEncounterGuid = "";
				companion.encounterTrackable.journalData.AmmonomiconSprite = "CakeMod/Resources/mimekin/mime_left_001";
				companion.encounterTrackable.journalData.enemyPortraitSprite = ItemAPI.ResourceExtractor.GetTextureFromResource("CakeMod\\Resources\\mimepic.png");
				ItemsMod.Strings.Enemies.Set("#THE_MIME", "Mime Kin");
				ItemsMod.Strings.Enemies.Set("#THE_MIME_SHORTDESC", "Invisible Man");
				ItemsMod.Strings.Enemies.Set("#THE_MIME_LONGDESC", "Masters of mimicking and mimery, these clever bullets use their connection to a silent force to enchant bullets with a fraction of their power.");
				companion.encounterTrackable.journalData.PrimaryDisplayName = "#THE_MIME";
				companion.encounterTrackable.journalData.NotificationPanelDescription = "#THE_MIME_SHORTDESC";
				companion.encounterTrackable.journalData.AmmonomiconFullEntry = "#THE_MIME_LONGDESC";
				EnemyBuilder.AddEnemyToDatabase(companion.gameObject, "cak:mimekin");
				EnemyDatabase.GetEntry("cak:mimekin").ForcedPositionInAmmonomicon = 32;
				EnemyDatabase.GetEntry("cak:mimekin").isInBossTab = false;
				EnemyDatabase.GetEntry("cak:mimekin").isNormalEnemy = true;
			}
		}
		private void DoTeleport()
		{
		}
		public class PoofScript : Script // This BulletScript is just a modified version of the script BulletManShroomed, which you can find with dnSpy.
		{
			protected override IEnumerator Top()
			{
				Instantiate<GameObject>(EasyVFXDatabase.ExplodeFirework, base.BulletBank.aiActor.sprite.WorldBottomCenter, Quaternion.identity);
				yield break;
			}

		}

		public class TransparentBulletScript : Script // This BulletScript is just a modified version of the script BulletManShroomed, which you can find with dnSpy.
		{
			protected override IEnumerator Top()
			{
				base.BulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("01972dee89fc4404a5c408d50007dad5").bulletBank.bulletBank.GetBullet("default"));
				Vector2 vector = new Vector2(0, 0);
				Vector2 vector2 = new Vector2(0, 0);
				int bighead = UnityEngine.Random.Range(1, 3);
				if (bighead == 1)
				{
					vector = new Vector2(3, 0);
				}
				if (bighead == 2)
				{
					vector = new Vector2(-3, 0);
				}
				int bighead2 = UnityEngine.Random.Range(1, 3);
				if (bighead2 == 1)
				{
					vector2 = new Vector2(0, 3);
				}
				if (bighead2 == 2)
				{
					vector = new Vector2(0, -3);
				}
				Instantiate<GameObject>(EasyVFXDatabase.WhiteCircleVFX, (this.BulletManager.PlayerPosition() + vector + vector2), Quaternion.identity);
				float direction = (this.BulletManager.PlayerPosition() - (this.BulletManager.PlayerPosition() + vector + vector2)).ToAngle();
				GameObject bullet = base.BulletBank.CreateProjectileFromBank(this.BulletManager.PlayerPosition() + vector + vector2, direction, "default");
				Projectile sourceProjectile = bullet.GetComponent<Projectile>();
				Material material = new Material(ShaderCache.Acquire("Brave/Internal/SimpleAlphaFadeUnlit"));
				material.SetTexture("_MainTex", sourceProjectile.sprite.renderer.material.mainTexture);
				material.SetTexture("_MaskTex", sourceProjectile.sprite.renderer.material.mainTexture);
				material.SetFloat("_Fade", 0.33f);
				sourceProjectile.sprite.renderer.material = material;
				return null;
			}

		}
		private static string[] spritePaths = new string[]
		{
			
			//idles
			"CakeMod/Resources/mimekin/mime_left_001",
			"CakeMod/Resources/mimekin/mime_left_002",
			"CakeMod/Resources/mimekin/mime_right_001",
			"CakeMod/Resources/mimekin/mime_right_002",
		};

		public class EnemyBehavior : BraveBehaviour
		{
			private RoomHandler m_StartRoom;
			public void Update()
			{
				if (!base.aiActor.HasBeenEngaged) { CheckPlayerRoom(); }
			}
			public AIActor randomActiveEnemy = null;
			private void CheckPlayerRoom()
			{

				if (GameManager.Instance.PrimaryPlayer.GetAbsoluteParentRoom() != null && GameManager.Instance.PrimaryPlayer.GetAbsoluteParentRoom() == m_StartRoom)
				{
					base.aiActor.HasBeenEngaged = true;
				}

			}
			public  void Start()
			{
				m_StartRoom = aiActor.GetAbsoluteParentRoom();
				//base.aiActor.HasBeenEngaged = true;
				base.aiActor.healthHaver.OnPreDeath += (obj) =>
				{
					LootEngine.DoDefaultPurplePoof(aiActor.sprite.WorldBottomCenter);
				};
				this.aiActor.knockbackDoer.SetImmobile(true, "laugh");
				base.aiActor.healthHaver.OnDamaged += (float resultValue, float maxValue, CoreDamageTypes damageTypes, DamageCategory damageCategory, Vector2 damageDirection) =>
				{


					if ((resultValue > base.aiActor.healthHaver.GetCurrentHealth()))
					{
						Instantiate<GameObject>(EasyVFXDatabase.MachoBraceDustUpVFX, aiActor.sprite.WorldBottomCenter, Quaternion.identity);
					}
				};
				this.aiActor.knockbackDoer.SetImmobile(true, "LAUGH!");
			}


		}

	}
	public class MimeEnhanceBehavior : BraveBehaviour
	{

		public void Update(AttackBehaviorGroup attackGroup)
		{

			RoomHandler absoluteRoom = base.transform.position.GetAbsoluteRoom();
			AIActor randomActiveEnemy = null;
			if (randomActiveEnemy == null)
			{
				randomActiveEnemy = absoluteRoom.GetRandomActiveEnemy(false);
			}
			if(randomActiveEnemy == this.aiActor)
            {
				randomActiveEnemy = absoluteRoom.GetRandomActiveEnemy(false);
			}
			if (randomActiveEnemy.aiActor != this.aiActor)
			{

				randomActiveEnemy.sprite.renderer.material.SetFloat("_EmissivePower", 23.45f);
				randomActiveEnemy.bulletBank.OnProjectileCreated += (Projectile rpoj) =>
				{
					Color c = rpoj.sprite.color;
					c.a = 0.5f;
					rpoj.sprite.color = c;
				};
			}
			if (randomActiveEnemy.healthHaver.IsDead)
            {
				randomActiveEnemy = absoluteRoom.GetRandomActiveEnemy(false);
			}
		}
		
	}
}
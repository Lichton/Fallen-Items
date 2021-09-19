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
	public class FlameClone : AIActor
	{
		public static GameObject prefab;
		public static readonly string guid = "flameclone";
		private static tk2dSpriteCollectionData OphaimCollection;
		public static GameObject shootpoint;
		public static void Init()
		{
			FlameClone.BuildPrefab();
		}

		public static void BuildPrefab()
		{
			//
			bool flag = prefab != null || EnemyBuilder.Dictionary.ContainsKey(guid);
			bool flag2 = flag;
			if (!flag2)
			{

				prefab = EnemyBuilder.BuildPrefab("Flame_Clone", guid, spritePaths[0], new IntVector2(0, 0), new IntVector2(8, 9), false);
				var companion = prefab.AddComponent<EnemyBehavior>();
				companion.aiActor.SetIsFlying(true, "Flying Enemy", true, true);
				companion.aiActor.EffectResistances = new ActorEffectResistance[] { new ActorEffectResistance() { resistAmount = 1, resistType = EffectResistanceType.Fire }, };
				companion.aiActor.knockbackDoer.weight = 800;
				companion.aiActor.MovementSpeed = 5f;
				companion.aiActor.name = "Killinder Clone";
				companion.aiActor.healthHaver.PreventAllDamage = false;
				companion.aiActor.CollisionDamage = 1f;
				companion.aiActor.HasShadow = false;
				companion.aiActor.IgnoreForRoomClear = false;
				companion.aiActor.aiAnimator.HitReactChance = 0f;
				companion.aiActor.specRigidbody.CollideWithOthers = true;
				companion.aiActor.specRigidbody.CollideWithTileMap = true;
				companion.aiActor.PreventFallingInPitsEver = false;
				companion.aiActor.CollisionKnockbackStrength = 5f;
				companion.aiActor.healthHaver.SetHealthMaximum(15f, null, false);
				AIAnimator aiAnimator = companion.aiAnimator;
				aiAnimator.IdleAnimation = new DirectionalAnimation
				{
					Type = DirectionalAnimation.DirectionType.TwoWayHorizontal,
					Flipped = new DirectionalAnimation.FlipType[2],
					AnimNames = new string[]
					{
						"idle"



					}
				};

				bool flag3 = OphaimCollection == null;
				if (flag3)
				{
					OphaimCollection = SpriteBuilder.ConstructCollection(prefab, "Inflamed_Eye_Collection");
					UnityEngine.Object.DontDestroyOnLoad(OphaimCollection);
					for (int i = 0; i < spritePaths.Length; i++)
					{
						SpriteBuilder.AddSpriteToCollection(spritePaths[i], OphaimCollection);
					}
					SpriteBuilder.AddAnimation(companion.spriteAnimator, OphaimCollection, new List<int>
					{

					0,
					1,
					2,
					3




					}, "idle", tk2dSpriteAnimationClip.WrapMode.Loop).fps = 7f;
					
				}
				var bs = prefab.GetComponent<BehaviorSpeculator>();
				float[] angles = { 135, 45, 125, 45 };
				prefab.GetComponent<ObjectVisibilityManager>();
				BehaviorSpeculator behaviorSpeculator = EnemyDatabase.GetOrLoadByGuid("01972dee89fc4404a5c408d50007dad5").behaviorSpeculator;
				bs.OverrideBehaviors = behaviorSpeculator.OverrideBehaviors;
				bs.OtherBehaviors = behaviorSpeculator.OtherBehaviors;
				shootpoint = new GameObject("fuck");
				shootpoint.transform.parent = companion.transform;
				shootpoint.transform.position = companion.sprite.WorldCenter;
				GameObject m_CachedGunAttachPoint = companion.transform.Find("fuck").gameObject;
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
					BulletScript = new CustomBulletScriptSelector(typeof(FlameShooter)),
					LeadAmount = 0f,
					AttackCooldown = 3f,
					RequiresLineOfSight = false,
					Uninterruptible = true
				}
				};
				companion.aiActor.CorpseObject = EnemyDatabase.GetOrLoadByGuid("c0ff3744760c4a2eb0bb52ac162056e6").CorpseObject;
				bs.InstantFirstTick = behaviorSpeculator.InstantFirstTick;
				bs.TickInterval = behaviorSpeculator.TickInterval;
				bs.PostAwakenDelay = behaviorSpeculator.PostAwakenDelay;
				bs.RemoveDelayOnReinforce = behaviorSpeculator.RemoveDelayOnReinforce;
				bs.OverrideStartingFacingDirection = behaviorSpeculator.OverrideStartingFacingDirection;
				bs.StartingFacingDirection = behaviorSpeculator.StartingFacingDirection;
				bs.SkipTimingDifferentiator = behaviorSpeculator.SkipTimingDifferentiator;
				Game.Enemies.Add("cak:flame_clone", companion.aiActor);
			}
		}




		private static string[] spritePaths = new string[]
		{
			
			//idles
			"CakeMod/Resources/FlameChamber/FlameClone/flameclone_idle_001",
			"CakeMod/Resources/FlameChamber/FlameClone/flameclone_idle_002",
					"CakeMod/Resources/FlameChamber/FlameClone/flameclone_idle_003",
			"CakeMod/Resources/FlameChamber/FlameClone/flameclone_idle_004",
				};

		public class EnemyBehavior : BraveBehaviour
		{
			public int o = 0;
			private RoomHandler m_StartRoom;
			private void Update()
			{
				o++;
				if (o == 600)
				{
					base.healthHaver.ApplyDamage(100000f, Vector2.zero, "da powa of da scapegoat", CoreDamageTypes.None, DamageCategory.Unstoppable, true, null, false);
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
				m_StartRoom = aiActor.GetAbsoluteParentRoom();
				//base.aiActor.HasBeenEngaged = true;
				base.aiActor.healthHaver.OnPreDeath += (obj) =>
				{
					Instantiate<GameObject>(EasyVFXDatabase.FlameVFX, base.aiActor.sprite.WorldBottomCenter, Quaternion.identity);
					AkSoundEngine.PostEvent("Play_ENM_highpriest_blast_01", base.aiActor.gameObject);
				};

			}


		}

		public class FlameShooter : Script
		{
			protected override IEnumerator Top()
			{
				if (this.BulletBank && this.BulletBank.aiActor && this.BulletBank.aiActor.TargetRigidbody)
				{
					base.BulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("d8a445ea4d944cc1b55a40f22821ae69").bulletBank.GetBullet("default"));
				}
				base.Fire(new Direction(0f, DirectionType.Aim), new Speed(8f, SpeedType.Absolute), new FlamenBullet());

				yield break;
			}
			public class FlamenBullet : Bullet
			{
				public FlamenBullet() : base("default", false, false, false)
				{

				}
			}

		}
	}
}


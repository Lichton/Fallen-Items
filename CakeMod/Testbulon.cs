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
using GungeonAPI;
using static DirectionalAnimation;
using CakeMod;

namespace CakeMod
{
	public class Testbulon : AIActor
	{
		public static GameObject prefab;
		public static readonly string guid = "testbulon";
		public static Dictionary<string, GameObject> enemyPrefabDictionary = new Dictionary<string, GameObject>();
		public class EnemyBehavior : BraveBehaviour
		{
			private void Update()
			{
				if (Time.frameCount % 5 == 0)
				{
					DeadlyDeadlyGoopManager goopManagerForGoopType = DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(EasyGoopDefinitions.CharmGoopDef);
					goopManagerForGoopType.AddGoopCircle(this.aiActor.specRigidbody.UnitCenter, 1.5f, -1, false, -1);
				}
			}
		}

		public static void Init()
		{

			Testbulon.BuildPrefab();
		}

		public static void BuildPrefab()
		{
			if (prefab != null || CompanionBuilder.companionDictionary.ContainsKey(guid))
				return;

			//Create the prefab with a starting sprite and hitbox offset/size
			prefab = CompanionBuilder.BuildPrefab("testbulon", guid, "CakeMod/Resources/Lovebulon/Idle/lovebulon_idle_001", new IntVector2(1, 0), new IntVector2(9, 9));

			//Add a companion component to the prefab (could be a custom class)
			var companion = prefab.GetComponent<BehaviorSpeculator>();
			BehaviorSpeculator BEHAVIORIAL = EnemyDatabase.GetOrLoadByGuid("42be66373a3d4d89b91a35c9ff8adfec").behaviorSpeculator;
			BehaviorSpeculator spec = companion.behaviorSpeculator;
			spec.OverrideBehaviors = BEHAVIORIAL.OverrideBehaviors;
			spec.OtherBehaviors = BEHAVIORIAL.OtherBehaviors;
			spec.TargetBehaviors = BEHAVIORIAL.TargetBehaviors;

			spec.AttackBehaviors = BEHAVIORIAL.AttackBehaviors;
			spec.MovementBehaviors = BEHAVIORIAL.MovementBehaviors;
			spec.InstantFirstTick = BEHAVIORIAL.InstantFirstTick;
			spec.TickInterval = BEHAVIORIAL.TickInterval;
			spec.PostAwakenDelay = BEHAVIORIAL.PostAwakenDelay;
			spec.RemoveDelayOnReinforce = BEHAVIORIAL.RemoveDelayOnReinforce;
			spec.OverrideStartingFacingDirection = BEHAVIORIAL.OverrideStartingFacingDirection;
			spec.StartingFacingDirection = BEHAVIORIAL.StartingFacingDirection;
			spec.SkipTimingDifferentiator = BEHAVIORIAL.SkipTimingDifferentiator;
			companion.aiActor.MovementSpeed = 9f;
			companion.aiActor.healthHaver.PreventAllDamage = false;
			companion.aiActor.CollisionDamage = 100f;
			companion.aiActor.HasShadow = false;
			companion.aiActor.CanTargetPlayers = true;
			companion.aiActor.specRigidbody.CollideWithOthers = true;
			companion.aiActor.specRigidbody.CollideWithTileMap = true;
			companion.aiActor.healthHaver.ForceSetCurrentHealth(35f);
			companion.aiActor.healthHaver.SetHealthMaximum(35f, null, false);
			companion.aiActor.specRigidbody.PixelColliders.Clear();
			companion.aiActor.specRigidbody.PixelColliders.Add(new PixelCollider
			{
				ColliderGenerationMode = PixelCollider.PixelColliderGeneration.Manual,
				CollisionLayer = CollisionLayer.EnemyCollider,
				IsTrigger = false,
				BagleUseFirstFrameOnly = false,
				SpecifyBagelFrame = string.Empty,
				BagelColliderNumber = 0,
				ManualOffsetX = 20,
				ManualOffsetY = 6,
				ManualWidth = 11,
				ManualHeight = 9,
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
				ManualWidth = 11,
				ManualHeight = 9,
				ManualDiameter = 0,
				ManualLeftX = 0,
				ManualLeftY = 0,
				ManualRightX = 0,
				ManualRightY = 0
			});

			//Add all of the needed animations (most of the animations need to have specific names to be recognized, like idle_right or attack_left)
			prefab.AddAnimation("idle_left", "CakeMod/Resources/Lovebulon/Idle", fps: 7, AnimationType.Idle, DirectionType.TwoWayHorizontal);
			prefab.AddAnimation("idle_right", "CakeMod/Resources/Lovebulon/Idle", fps: 7, AnimationType.Idle, DirectionType.TwoWayHorizontal);
			prefab.AddAnimation("run_left", "CakeMod/Resources/Lovebulon/Run", fps: 7, AnimationType.Move, DirectionType.TwoWayHorizontal);
			prefab.AddAnimation("run_right", "CakeMod/Resources/Lovebulon/Run", fps: 7, AnimationType.Move, DirectionType.TwoWayHorizontal);
			prefab.AddAnimation("pitfall", "CakeMod/Resources/Lovebulon/Fall", fps: 7, AnimationType.Idle, DirectionType.TwoWayHorizontal, tk2dSpriteAnimationClip.WrapMode.Once);
			//Note that the "die" and "attack" animations are only set to Move because they will be overwritten later.
			//tk2dSpriteAnimationClip.WrapMode.Once determines how an animation plays out. If you don't want it to loop, leave it to Once, otherwise you can change it to Loop or something.
			//Assign animation well assigns an animation to an animation type. By default this is on, but since we're overwritting this set this to false.	
			prefab.AddAnimation("attack_left", "CakeMod/Resources/Lovebulon/Attack", fps: 8, AnimationType.Move, DirectionType.TwoWayHorizontal, tk2dSpriteAnimationClip.WrapMode.Once, assignAnimation: false);
			prefab.AddAnimation("attack_right", "CakeMod/Resources/Lovebulon/Attack", fps: 8, AnimationType.Move, DirectionType.TwoWayHorizontal, tk2dSpriteAnimationClip.WrapMode.Once, assignAnimation: false);
			prefab.AddAnimation("die_left", "CakeMod/Resources/Lovebulon/Die", fps: 12, AnimationType.Move, DirectionType.TwoWayHorizontal, tk2dSpriteAnimationClip.WrapMode.Once, assignAnimation: false);
			prefab.AddAnimation("die_right", "CakeMod/Resources/Lovebulon/Die", fps: 12, AnimationType.Move, DirectionType.TwoWayHorizontal, tk2dSpriteAnimationClip.WrapMode.Once, assignAnimation: false);
			//Here we create a new DirectionalAnimation for our enemy to pull from. 
			//Make sure the AnimNames correspong to the AddAnimation names.
			DirectionalAnimation attack = new DirectionalAnimation()
			{
				AnimNames = new string[] { "attack_right", "attack_left" },
				Flipped = new FlipType[] { FlipType.None, FlipType.None },
				Type = DirectionType.TwoWayHorizontal,
				Prefix = string.Empty
			};
			DirectionalAnimation die = new DirectionalAnimation()
			{
				AnimNames = new string[] { "die_right", "die_left" },
				Flipped = new FlipType[] { FlipType.None, FlipType.None },
				Type = DirectionType.TwoWayHorizontal,
				Prefix = string.Empty
			};
			//Because Dodge Roll is Dodge Roll and there is no animation types for attack and death, we have to assign them to the Other category.
			companion.aiAnimator.AssignDirectionalAnimation("attack", attack, AnimationType.Other);
			companion.aiAnimator.AssignDirectionalAnimation("die", die, AnimationType.Other);

			//Add the behavior here, this too can be a custom class that extends AttackBehaviorBase or something like that
			var bs = prefab.GetComponent<BehaviorSpeculator>();
			bs.TargetBehaviors = new List<TargetBehaviorBase>() {
				new TargetPlayerBehavior() {
					Radius = 35,
					LineOfSight = false,
					ObjectPermanence = true,
					SearchInterval = 0.01f,
					PauseOnTargetSwitch = false,
					PauseTime = 0f
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
					MinActiveRange = 0,
					MaxActiveRange = 0
				}
			};
			Game.Enemies.Add("cak:testbulon", companion.aiActor);


		}
	}
}






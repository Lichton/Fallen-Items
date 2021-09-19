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
	public class Lovebulon : AIActor
	{
		private void SetGoopIndex(int index)
		{
			this.goopIndex = index;
			this.currentGoop = Lovebulon.goopDefs[index];
			this.tint = Lovebulon.tints[index];
		}
		private int goopIndex;

		private GoopDefinition currentGoop;

		private Color tint = Color.white;

		private static Color[] tints = new Color[]
		{
			new Color(0.9f, 0.34f, 0.45f),
			new Color(1f, 0.5f, 0.35f),
			new Color(0.7f, 0.9f, 0.7f),
			new Color(0.9f, 0.4f, 0.8f)
		};
		private static string[] goops = new string[]
	   {
			"assets/data/goops/water goop.asset",
	   };
		public static List<GoopDefinition> goopDefs;
		public static GameObject prefab;
		//Always make sure to give your enemy a unique guid. This is essentially the id of your enemy and is integral for many parts of EnemyAPI
		public static readonly string guid = "Lovebulon";
		//This shootpoint gameObject determines well, where the enemy shoots from, I'll explain more when we get to the AttackBehaviors.
		public static GameObject shootpoint;


		public static void Init()
		{
			AssetBundle assetBundle = ResourceManager.LoadAssetBundle("shared_auto_001");
			Lovebulon.goopDefs = new List<GoopDefinition>();
			foreach (string text in Lovebulon.goops)
			{
				GoopDefinition goopDefinition;
				try
				{
					GameObject gameObject = assetBundle.LoadAsset(text) as GameObject;
					goopDefinition = gameObject.GetComponent<GoopDefinition>();
				}
				catch
				{
					goopDefinition = (assetBundle.LoadAsset(text) as GoopDefinition);
				}
				goopDefinition.name = text.Replace("assets/data/goops/", "").Replace(".asset", "");
				Lovebulon.goopDefs.Add(goopDefinition);
			}
			GoopDefinition item;
			item = null;
			List<GoopDefinition> goopDefs = Lovebulon.goopDefs;
			goopDefs.Add(item);
			Lovebulon.goopDefs = new List<GoopDefinition>();
			foreach (string text in Lovebulon.goops)
			{
				GoopDefinition goopDefinition;
				try
				{
					GameObject gameObject2 = assetBundle.LoadAsset(text) as GameObject;
					goopDefinition = gameObject2.GetComponent<GoopDefinition>();
				}
				catch
				{
					goopDefinition = (assetBundle.LoadAsset(text) as GoopDefinition);
				}
				goopDefinition.name = text.Replace("assets/data/goops/", "").Replace(".asset", "");
				Lovebulon.goopDefs.Add(goopDefinition);
			}
			//As always don't forget to initalize your enemy. 
			Lovebulon.BuildPrefab();
		}
		public static void BuildPrefab()
		{

			if (prefab == null || !EnemyBuilder.Dictionary.ContainsKey(guid))
			{
				//Sets up the prefab of the enemy. The spritepath, "CakeMod/Resources/Lovebulon/Idle/milton_idle_001", determines the setup sprite for your enemy. vvvv This bool right here determines whether or not an enemy has an AiShooter or not. AIShooters are necessary if you want your enemy to hold a gun for example. An example of this can be seen in Humphrey.
				prefab = EnemyBuilder.BuildPrefab("Lovebulon", guid, "CakeMod/Resources/Lovebulon/Idle/lovebulon_idle_001", new IntVector2(0, 0), new IntVector2(0, 0), false);
				//This line extends a BraveBehavior called EnemyBehavior, this is a generic behavior I use for setting up things that can't be setup in BuildPrefab.
				var enemy = prefab.AddComponent<EnemyBehavior>();
				//Here you can setup various things like movement speed, weight, and health. There's a lot you can do with the AiActor parameter so feel free to experiment.
				enemy.aiActor.MovementSpeed = 6.7f;
				enemy.aiActor.knockbackDoer.weight = 100;
				enemy.aiActor.IgnoreForRoomClear = false;
				enemy.aiActor.CollisionDamage = 10f;
				enemy.aiActor.healthHaver.ForceSetCurrentHealth(15f);
				enemy.aiActor.healthHaver.SetHealthMaximum(15f, null, false);
				

				//This is where you setup your animations. Most animations need specific frame names to be recognized like idle or die. 
				//The AddAnimation lines gets sprites from the folder specified in second phrase of the this line. At the very least you need an animation that contains the word idle for the idle animations for example.
				//AnimationType determines what kind of animation your making. In Gungeon there are 7 different Animation Types: Move, Idle, Fidget, Flight, Hit, Talk, Other. For a majority of these animations, these play automatically, however specific animations need to be told when to play such as Attack.
				//DirectionType determines the amount of ways an animation can face. You'll have to change your animation names to correspond with the DirectionType. For example if you want an animation to face eight ways you'll have to name your animations something like ""attack_south_west", "attack_north_east",  "attack_east", "attack_south_east",  "attack_north",  "attack_south", "attack_west", "attack_north_west" and change DirectionType to  DirectionType.EightWayOrdinal.
				//I suggest looking at the sprites of base game enemies to determine the names for the different directions.
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
				enemy.aiAnimator.AssignDirectionalAnimation("attack", attack, AnimationType.Other);
				enemy.aiAnimator.AssignDirectionalAnimation("die", die, AnimationType.Other);
				//This is where we get into the meat and potatoes of our enemy. This is where all the behaviors of our enemy are made.
				//This shootpoint block of code determines where our bullets will orginate from. In this case, the center of the enemy.
				shootpoint = new GameObject("lovebuloncenter");
				shootpoint.transform.parent = enemy.transform;
				shootpoint.transform.position = enemy.sprite.WorldCenter;
				GameObject position = enemy.transform.Find("lovebuloncenter").gameObject;
				//this line adds a BehaviorSpeculator to our enemy which is the base for adding behaviors on to.
				var bs = prefab.GetComponent<BehaviorSpeculator>();
				//Here we will add some basic behaviors such as TargetPlayerBehavior and SeekTargetBehavior.
				//You can change many things in these behaviors so feel free to go nuts.
				enemy.aiActor.specRigidbody.PixelColliders.Clear();
				enemy.aiActor.specRigidbody.PixelColliders.Add(new PixelCollider
				{
					ColliderGenerationMode = PixelCollider.PixelColliderGeneration.Manual,
					CollisionLayer = CollisionLayer.EnemyCollider,
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
				enemy.aiActor.specRigidbody.PixelColliders.Add(new PixelCollider
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
				BehaviorSpeculator BEHAVIORIAL = EnemyDatabase.GetOrLoadByGuid("42be66373a3d4d89b91a35c9ff8adfec").behaviorSpeculator;
				BehaviorSpeculator spec = enemy.behaviorSpeculator;
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
				//Now this is one of the most important behaviors because it allows our enemy to shoot.
				bs.AttackBehaviors = new List<AttackBehaviorBase>() {
				new ShootBehavior() {
					ShootPoint = position,
					//This line selects our Bullet Script
					BulletScript = new CustomBulletScriptSelector(typeof(MiltonScript)),
					LeadAmount = 0f,
					AttackCooldown = 4f,
					FireAnimation = "attack",
					RequiresLineOfSight = true,
					Uninterruptible = false,
					
				}
				};
				bs.MovementBehaviors = new List<MovementBehaviorBase>
			{
				new SeekTargetBehavior
				{
					StopWhenInRange = false,
					CustomRange = 15f,
					LineOfSight = false,
					ReturnToSpawn = false,
					SpawnTetherDistance = 0f,
					PathInterval = 0.5f,
					SpecifyRange = false,
					MinActiveRange = 0f,
					MaxActiveRange = 0f
				}
			};
				//Adds the enemy to MTG spawn pool and spawn command
				Game.Enemies.Add("cak:lovebulon", enemy.aiActor);
			}
		}
		public class EnemyBehavior : BraveBehaviour
		{
			//This determines that the enemy is active when a player is in the room
			private RoomHandler m_StartRoom;
			private void Update()
			{
				if (Time.frameCount % 5 == 0)
				{
					DeadlyDeadlyGoopManager goopManagerForGoopType = DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(EasyGoopDefinitions.CharmGoopDef);
					goopManagerForGoopType.AddGoopCircle(this.aiActor.specRigidbody.UnitCenter, 1.5f, -1, false, -1);
				}
				if (!base.aiActor.HasBeenEngaged) { CheckPlayerRoom(); }
			}
			private void CheckPlayerRoom()
			{
				if (GameManager.Instance.PrimaryPlayer.GetAbsoluteParentRoom() != null && GameManager.Instance.PrimaryPlayer.GetAbsoluteParentRoom() == m_StartRoom) { base.aiActor.HasBeenEngaged = true; }
			}
			private void Start()
			{

				m_StartRoom = aiActor.GetAbsoluteParentRoom();
				//This line determines what happens when an enemy dies. For now it's something simple like playing a death sound.
				//A full list of all the sounds can be found in the SFX.txt document that comes with this github.
				base.aiActor.healthHaver.OnPreDeath += (obj) => { AkSoundEngine.PostEvent("SND_CHR_blobulin_death_01", base.aiActor.gameObject); };
				base.aiActor.healthHaver.OnPreDeath += (obj) =>
				{
					DeadlyDeadlyGoopManager goopManagerForGoopType = DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(EasyGoopDefinitions.CharmGoopDef);
					goopManagerForGoopType.AddGoopCircle(this.aiActor.specRigidbody.UnitCenter, 5f, -1, false, -1);
				};
			}
		}


	}
}
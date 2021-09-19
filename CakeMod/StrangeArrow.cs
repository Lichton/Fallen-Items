using Dungeonator;
using ItemAPI;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DirectionType = DirectionalAnimation.DirectionType;
using AnimationType = ItemAPI.CompanionBuilder.AnimationType;

namespace CakeMod
{
	public class StrangeArrow : CompanionItem
	{
		public static void Init()
		{
			string name = "Strange Arrow";
			string resourcePath = "CakeMod/Resources/StrangeArrow";
			GameObject gameObject = new GameObject();
			var arrowStrange = gameObject.AddComponent<StrangeArrow>();

			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject);

			string shortDesc = "Requiem";
			string longDesc = "This arrow radiates a menacing aura.";
			ItemBuilder.SetupItem(arrowStrange, shortDesc, longDesc, "cak");
			arrowStrange.quality = PickupObject.ItemQuality.EXCLUDED;
			arrowStrange.CompanionGuid = StrangeArrow.guid;
			arrowStrange.Synergies = new CompanionTransformSynergy[0];
			StrangeArrow.BuildPrefab();
		}

		public override void Pickup(PlayerController player)
		{

			base.Pickup(player);
		}




		public static void BuildPrefab()
		{

			bool flag = StrangeArrow.arrowPrefab != null || CompanionBuilder.companionDictionary.ContainsKey(StrangeArrow.guid);
			bool flag2 = flag;
			if (!flag2)
			{
				StrangeArrow.arrowPrefab = CompanionBuilder.BuildPrefab("Strange Arrow", StrangeArrow.guid, StrangeArrow.spritePaths[0], new IntVector2(3, 2), new IntVector2(8, 9));
				StrangeArrow.arrowBehavior arrowsBehavior = StrangeArrow.arrowPrefab.AddComponent<StrangeArrow.arrowBehavior>();
				AIAnimator aiAnimator = arrowsBehavior.aiAnimator;
				aiAnimator.MoveAnimation = new DirectionalAnimation
				{
					Type = DirectionalAnimation.DirectionType.TwoWayHorizontal,
					Flipped = new DirectionalAnimation.FlipType[2],
					AnimNames = new string[]
					{
						"run_right",
						"run_left"
					}
				};
				aiAnimator.IdleAnimation = new DirectionalAnimation
				{
					Type = DirectionalAnimation.DirectionType.TwoWayHorizontal,
					Flipped = new DirectionalAnimation.FlipType[2],
					AnimNames = new string[]
					{
						"idle_right",
						"idle_left"
					}
				};
				bool flag3 = StrangeArrow.arrowCollection == null;
				if (flag3)
				{
					StrangeArrow.arrowCollection = SpriteBuilder.ConstructCollection(StrangeArrow.arrowPrefab, "StrangeArrow_Collection");
					UnityEngine.Object.DontDestroyOnLoad(StrangeArrow.arrowCollection);
					for (int i = 0; i < StrangeArrow.spritePaths.Length; i++)
					{
						SpriteBuilder.AddSpriteToCollection(StrangeArrow.spritePaths[i], StrangeArrow.arrowCollection);
					}
					SpriteBuilder.AddAnimation(arrowsBehavior.spriteAnimator, StrangeArrow.arrowCollection, new List<int>
					{
						0,
						1

					}, "idle_left", tk2dSpriteAnimationClip.WrapMode.Loop).fps = 5f;
					SpriteBuilder.AddAnimation(arrowsBehavior.spriteAnimator, StrangeArrow.arrowCollection, new List<int>
					{
						2,
						3
					}, "idle_right", tk2dSpriteAnimationClip.WrapMode.Loop).fps = 5f;
					SpriteBuilder.AddAnimation(arrowsBehavior.spriteAnimator, StrangeArrow.arrowCollection, new List<int>
					{
						4,
						5

					}, "run_left", tk2dSpriteAnimationClip.WrapMode.Loop).fps = 14f;
					SpriteBuilder.AddAnimation(arrowsBehavior.spriteAnimator, StrangeArrow.arrowCollection, new List<int>
					{
						6,
						7
					}, "run_right", tk2dSpriteAnimationClip.WrapMode.Loop).fps = 14f;
				}
				arrowsBehavior.aiActor.MovementSpeed = 7f;
				arrowsBehavior.specRigidbody.Reinitialize();
				arrowsBehavior.specRigidbody.CollideWithTileMap = false;
				arrowsBehavior.aiActor.CanTargetEnemies = true;
				BehaviorSpeculator behaviorSpeculator = arrowsBehavior.behaviorSpeculator;
				behaviorSpeculator.AttackBehaviors.Add(new StrangeArrow.arrowAttackBehavior());
				behaviorSpeculator.MovementBehaviors.Add(new StrangeArrow.ApproachEnemiesBehavior());
				behaviorSpeculator.MovementBehaviors.Add(new CompanionFollowPlayerBehavior
				{
					IdleAnimations = new string[]
					{
						"idle"
					}
				});
				UnityEngine.Object.DontDestroyOnLoad(StrangeArrow.arrowPrefab);
				FakePrefab.MarkAsFakePrefab(StrangeArrow.arrowPrefab);
				StrangeArrow.arrowPrefab.SetActive(false);
			}
		}

		public static GameObject arrowPrefab;

		public static readonly string guid = "arrow";

		private List<CompanionController> companionsSpawned = new List<CompanionController>();


		private static string[] spritePaths = new string[]
		{
			"CakeMod/Resources/StarPlatinum/idleleft/starplatinum_idle_left_001",
			"CakeMod/Resources/StarPlatinum/idleleft/starplatinum_idle_left_002",
			"CakeMod/Resources/StarPlatinum/idleright/starplatinum_idle_right_001",
			"CakeMod/Resources/StarPlatinum/idleright/starplatinum_idle_right_002",
			"CakeMod/Resources/StarPlatinum/runleft/starplatinum_run_left_001",
			"CakeMod/Resources/StarPlatinum/runleft/starplatinum_run_left_002",
			"CakeMod/Resources/StarPlatinum/runright/starplatinum_run_right_001",
			"CakeMod/Resources/StarPlatinum/runright/starplatinum_run_right_002",
		};

		private static tk2dSpriteCollectionData arrowCollection;


		public class arrowBehavior : CompanionController
		{

			private void Start()
			{
				base.spriteAnimator.Play("idle");
				this.Owner = this.m_owner;
			}


			public PlayerController Owner;
		}

		public class arrowAttackBehavior : AttackBehaviorBase
		{


			public override void Destroy()
			{

				base.Destroy();
			}


			public override void Init(GameObject gameObject, AIActor aiActor, AIShooter aiShooter)
			{
				base.Init(gameObject, aiActor, aiShooter);
				this.Owner = this.m_aiActor.GetComponent<StrangeArrow.arrowBehavior>().Owner;
			}

			public override BehaviorResult Update()
			{
				bool flag = this.attackTimer > 0f && this.isAttacking;
				if (flag)
				{
					base.DecrementTimer(ref this.attackTimer, false);
				}
				else
				{

					bool flag2 = this.attackCooldownTimer > 0f && !this.isAttacking;
					if (flag2)
					{

						base.DecrementTimer(ref this.attackCooldownTimer, false);
					}
				}
				bool flag3 = this.IsReady();
				bool flag4 = (!flag3 || this.attackCooldownTimer > 0f || this.attackTimer == 0f || this.m_aiActor.TargetRigidbody == null) && this.isAttacking;
				BehaviorResult result;
				if (flag4)
				{

					this.StopAttacking();
					result = BehaviorResult.Continue;
				}
				else
				{

					bool flag5 = flag3 && this.attackCooldownTimer == 0f && !this.isAttacking;
					if (flag5)
					{

						this.attackTimer = this.attackDuration;
						this.isAttacking = true;
					}
					bool flag6 = this.attackTimer > 0f && flag3;
					if (flag6)
					{

						this.Attack();
						result = BehaviorResult.SkipAllRemainingBehaviors;
					}
					else
					{

						result = BehaviorResult.Continue;
					}
				}
				return result;
			}

			private void StopAttacking()
			{
				this.isAttacking = false;
				this.attackTimer = 0f;
				this.attackCooldownTimer = this.attackCooldown;
			}

			public AIActor GetNearestEnemy(List<AIActor> activeEnemies, Vector2 position, out float nearestDistance, string[] filter)
			{
				AIActor aiactor = null;
				nearestDistance = float.MaxValue;
				bool flag = activeEnemies == null;
				bool flag2 = flag;
				bool flag3 = flag2;
				AIActor result;
				if (flag3)
				{
					result = null;
				}
				else
				{
					for (int i = 0; i < activeEnemies.Count; i++)
					{
						AIActor aiactor2 = activeEnemies[i];
						bool flag4 = aiactor2.healthHaver && aiactor2.healthHaver.IsVulnerable;
						bool flag5 = flag4;
						bool flag6 = flag5;
						if (flag6)
						{
							bool flag7 = !aiactor2.healthHaver.IsDead;
							bool flag8 = flag7;
							bool flag9 = flag8;
							if (flag9)
							{
								bool flag10 = filter == null || !filter.Contains(aiactor2.EnemyGuid);
								bool flag11 = flag10;
								bool flag12 = flag11;
								if (flag12)
								{
									float num = Vector2.Distance(position, aiactor2.CenterPosition);
									bool flag13 = num < nearestDistance;
									bool flag14 = flag13;
									bool flag15 = flag14;
									if (flag15)
									{
										nearestDistance = num;
										aiactor = aiactor2;
									}
								}
							}
						}
					}
					result = aiactor;
				}
				return result;
			}

			private void Attack()
			{

				bool flag = this.Owner == null;
				if (flag)
				{
					this.Owner = this.m_aiActor.GetComponent<StrangeArrow.arrowBehavior>().Owner;
				}
				float num = -1f;

				List<AIActor> activeEnemies = this.Owner.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
				bool flag2 = activeEnemies == null | activeEnemies.Count <= 0;
				if (!flag2)
				{
					AIActor nearestEnemy = this.GetNearestEnemy(activeEnemies, this.m_aiActor.sprite.WorldCenter, out num, null);
					bool flag3 = nearestEnemy && num < 10f;
					if (flag3)
					{
						bool flag4 = this.IsInRange(nearestEnemy);
						if (flag4)
						{
							bool flag5 = !nearestEnemy.IsHarmlessEnemy && nearestEnemy.IsNormalEnemy && !nearestEnemy.healthHaver.IsDead && nearestEnemy != this.m_aiActor;
							if (flag5)
							{
								Vector2 unitCenter = this.m_aiActor.specRigidbody.UnitCenter;
								Vector2 unitCenter2 = nearestEnemy.specRigidbody.HitboxPixelCollider.UnitCenter;
								float z = BraveMathCollege.Atan2Degrees((unitCenter2 - unitCenter).normalized);
								Projectile projectile = ((Gun)ETGMod.Databases.Items[16]).DefaultModule.projectiles[0];
								GameObject gameObject = SpawnManager.SpawnProjectile(projectile.gameObject, this.m_aiActor.sprite.WorldCenter, Quaternion.Euler(0f, 0f, z), true);
								Projectile component = gameObject.GetComponent<Projectile>();
							}
						}
					}
				}
			}

			public override float GetMaxRange()
			{
				return 0.9f;
			}

			public override float GetMinReadyRange()
			{
				return 0.7f;
			}

			public override bool IsReady()
			{
				AIActor aiActor = this.m_aiActor;
				bool flag;
				if (aiActor == null)
				{
					flag = true;
				}
				else
				{
					SpeculativeRigidbody targetRigidbody = aiActor.TargetRigidbody;
					Vector2? vector = (targetRigidbody != null) ? new Vector2?(targetRigidbody.UnitCenter) : null;
					flag = (vector == null);
				}
				bool flag2 = flag;
				return !flag2 && Vector2.Distance(this.m_aiActor.specRigidbody.UnitCenter, this.m_aiActor.TargetRigidbody.UnitCenter) <= this.GetMinReadyRange();
			}


			public bool IsInRange(AIActor enemy)
			{

				bool flag;
				if (enemy == null)
				{
					flag = true;
				}
				else
				{
					SpeculativeRigidbody specRigidbody = enemy.specRigidbody;
					Vector2? vector = (specRigidbody != null) ? new Vector2?(specRigidbody.UnitCenter) : null;
					flag = (vector == null);
				}
				bool flag2 = flag;
				return !flag2 && Vector2.Distance(this.m_aiActor.specRigidbody.UnitCenter, enemy.specRigidbody.UnitCenter) <= this.GetMinReadyRange();
			}

			private bool isAttacking;

	
			private float attackCooldown = 0.01f;

		
			private float attackDuration = 5f;

	
			private float attackTimer;

		
			private float attackCooldownTimer;


			private PlayerController Owner;


			private List<AIActor> roomEnemies = new List<AIActor>();
		}


		public class ApproachEnemiesBehavior : MovementBehaviorBase
		{
			public override void Init(GameObject gameObject, AIActor aiActor, AIShooter aiShooter)
			{
				base.Init(gameObject, aiActor, aiShooter);
			}


			public override void Upkeep()
			{
				base.Upkeep();
				base.DecrementTimer(ref this.repathTimer, false);
			}

			public override BehaviorResult Update()
			{
				SpeculativeRigidbody overrideTarget = this.m_aiActor.OverrideTarget;
				bool flag = this.repathTimer > 0f;
				BehaviorResult result;
				if (flag)
				{
					result = ((overrideTarget == null) ? BehaviorResult.Continue : BehaviorResult.SkipRemainingClassBehaviors);
				}
				else
				{
					bool flag2 = overrideTarget == null;
					if (flag2)
					{
						this.PickNewTarget();
						result = BehaviorResult.Continue;
					}
					else
					{
						this.isInRange = (Vector2.Distance(this.m_aiActor.specRigidbody.UnitCenter, overrideTarget.UnitCenter) <= this.DesiredDistance);
						bool flag3 = overrideTarget != null && !this.isInRange;
						if (flag3)
						{
							this.m_aiActor.PathfindToPosition(overrideTarget.UnitCenter, null, true, null, null, null, false);
							this.repathTimer = this.PathInterval;
							result = BehaviorResult.SkipRemainingClassBehaviors;
						}
						else
						{
							bool flag4 = overrideTarget != null && this.repathTimer >= 0f;
							if (flag4)
							{
								this.m_aiActor.ClearPath();
								this.repathTimer = -1f;
							}
							result = BehaviorResult.Continue;
						}
					}
				}
				return result;
			}

			private void PickNewTarget()
			{

				bool flag = this.m_aiActor == null;
				if (!flag)
				{
					bool flag2 = this.Owner == null;
					if (flag2)
					{
						this.Owner = this.m_aiActor.GetComponent<StrangeArrow.arrowBehavior>().Owner;
					}
					this.Owner.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All, ref this.roomEnemies);
					for (int i = 0; i < this.roomEnemies.Count; i++)
					{
						AIActor aiactor = this.roomEnemies[i];
						bool flag3 = aiactor.IsHarmlessEnemy || !aiactor.IsNormalEnemy || aiactor.healthHaver.IsDead || aiactor == this.m_aiActor || aiactor.EnemyGuid == "ba928393c8ed47819c2c5f593100a5bc";
						if (flag3)
						{

							this.roomEnemies.Remove(aiactor);

						}
					}
					bool flag4 = this.roomEnemies.Count == 0;
					if (flag4)
					{
						this.m_aiActor.OverrideTarget = null;
					}
					else
					{
						AIActor aiActor = this.m_aiActor;
						AIActor aiactor2 = this.roomEnemies[UnityEngine.Random.Range(0, this.roomEnemies.Count)];
						aiActor.OverrideTarget = ((aiactor2 != null) ? aiactor2.specRigidbody : null);
					}
				}
			}


			public float PathInterval = 0.45f;


			public float DesiredDistance = 0.8f;


			private float repathTimer;

			private List<AIActor> roomEnemies = new List<AIActor>();

			private bool isInRange;

			private PlayerController Owner;
		}
	}
}

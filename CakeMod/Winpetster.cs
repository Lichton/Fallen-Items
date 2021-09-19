using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;
using DirectionType = DirectionalAnimation.DirectionType;
using AnimationType = ItemAPI.CompanionBuilder.AnimationType;
using Dungeonator;

namespace CakeMod
{
	public class Winpetster : CompanionItem
	{
		public static GameObject prefab;
		private static readonly string guid = "winpetster252352556"; //give your companion some unique guid

		public static void Init()
		{
			string itemName = "Winpetster";
			string resourceName = "CakeMod/Resources/winpetster";

			GameObject obj = new GameObject();
			var item = obj.AddComponent<Winpetster>();
			ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

			string shortDesc = "Evading Criticism";
			string longDesc = "Famously used by many gungeoneers to laugh off and evade criticism.";

			ItemBuilder.SetupItem(item, shortDesc, longDesc, "cak");
			item.quality = PickupObject.ItemQuality.D;
			item.CompanionGuid = guid; //this will be used by the item later to pull your companion from the enemy database
			item.Synergies = new CompanionTransformSynergy[0]; //this just needs to not be null
			item.AddPassiveStatModifier(PlayerStats.StatType.DodgeRollDistanceMultiplier, 1.3f, StatModifier.ModifyMethod.MULTIPLICATIVE);
			item.AddPassiveStatModifier(PlayerStats.StatType.DodgeRollSpeedMultiplier, 1.1f, StatModifier.ModifyMethod.MULTIPLICATIVE);
			item.AddPassiveStatModifier(PlayerStats.StatType.MovementSpeed, 1.4f, StatModifier.ModifyMethod.MULTIPLICATIVE);
			BuildPrefab();
		}


		public static void BuildPrefab()
		{
			if (prefab != null || CompanionBuilder.companionDictionary.ContainsKey(guid))
				return;

			//Create the prefab with a starting sprite and hitbox offset/size
			prefab = CompanionBuilder.BuildPrefab("Winpetster", guid, "CakeMod/Resources/Winpetster/Idle/son_idle_001", new IntVector2(1, 0), new IntVector2(17, 22));


			//Add a companion component to the prefab (could be a custom class)
			var companion = prefab.AddComponent<WinpetsterBehavior>();
			companion.aiActor.MovementSpeed = 2.5f;

			//Add all of the needed animations (most of the animations need to have specific names to be recognized, like idle_right or attack_left)
			prefab.AddAnimation("idle_right", "CakeMod/Resources/Winpetster/Idle", fps: 10, AnimationType.Idle, DirectionType.TwoWayHorizontal);
			prefab.AddAnimation("idle_left", "CakeMod/Resources/Winpetster/Idle", fps: 10, AnimationType.Idle, DirectionType.TwoWayHorizontal);
			prefab.AddAnimation("run_right", "CakeMod/Resources/Winpetster/MoveRight", fps: 10, AnimationType.Move, DirectionType.TwoWayHorizontal);
			prefab.AddAnimation("run_left", "CakeMod/Resources/Winpetster/MoveLeft", fps: 10, AnimationType.Move, DirectionType.TwoWayHorizontal);

			//Add the behavior here, this too can be a custom class that extends AttackBehaviorBase or something like that
			BehaviorSpeculator bs = companion.behaviorSpeculator;
			companion.aiActor.MovementSpeed = 7f;
			companion.specRigidbody.Reinitialize();
			companion.specRigidbody.CollideWithTileMap = false;
			companion.aiActor.CanTargetEnemies = true;
			bs.MovementBehaviors.Add(new CompanionFollowPlayerBehavior() { IdleAnimations = new string[] { "idle" } });
			bs.AttackBehaviors.Add(new Winpetster.WinpetsterAttackBehavior());
			bs.MovementBehaviors.Add(new Winpetster.ApproachEnemiesBehavior());
		}

		public class WinpetsterBehavior : CompanionController
		{

			// Token: 0x06000107 RID: 263 RVA: 0x0000A125 File Offset: 0x00008325
			// Token: 0x06000107 RID: 263 RVA: 0x0000A125 File Offset: 0x00008325die
			private void Start()
			{

				base.spriteAnimator.Play("idle");
				this.Owner = this.m_owner;
			}

			// Token: 0x0400006A RID: 106
			public PlayerController Owner;
		}
		public class WinpetsterAttackBehavior : AttackBehaviorBase
		{
			// Token: 0x060003C6 RID: 966 RVA: 0x00020F4B File Offset: 0x0001F14B
			public override void Destroy()
			{
				base.Destroy();
			}

			// Token: 0x060003C7 RID: 967 RVA: 0x000232D4 File Offset: 0x000214D4
			public override void Init(GameObject gameObject, AIActor aiActor, AIShooter aiShooter)
			{
				base.Init(gameObject, aiActor, aiShooter);
				this.Owner = this.m_aiActor.GetComponent<Winpetster.WinpetsterBehavior>().Owner;
			}

			// Token: 0x060003C8 RID: 968 RVA: 0x000232F8 File Offset: 0x000214F8
			public override BehaviorResult Update()
			{
				bool flag = this.attackTimer > 0f && this.isAttacking;
				bool flag2 = flag;
				if (flag2)
				{
					base.DecrementTimer(ref this.attackTimer, false);
				}
				else
				{
					bool flag3 = this.attackCooldownTimer > 0f && !this.isAttacking;
					bool flag4 = flag3;
					if (flag4)
					{
						base.DecrementTimer(ref this.attackCooldownTimer, false);
					}
				}
				bool flag5 = this.IsReady();
				bool flag6 = (!flag5 || this.attackCooldownTimer > 0f || this.attackTimer == 0f || this.m_aiActor.TargetRigidbody == null) && this.isAttacking;
				bool flag7 = flag6;
				BehaviorResult result;
				if (flag7)
				{
					this.StopAttacking();
					result = BehaviorResult.Continue;
				}
				else
				{
					bool flag8 = flag5 && this.attackCooldownTimer == 0f && !this.isAttacking;
					bool flag9 = flag8;
					if (flag9)
					{
						this.attackTimer = this.attackDuration;
						this.isAttacking = true;
					}
					bool flag10 = this.attackTimer > 0f && flag5;
					bool flag11 = flag10;
					if (flag11)
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

			// Token: 0x060003C9 RID: 969 RVA: 0x0002342E File Offset: 0x0002162E
			private void StopAttacking()
			{
				this.isAttacking = false;
				this.attackTimer = 0f;
				this.attackCooldownTimer = this.attackCooldown;
			}

			// Token: 0x060003CA RID: 970 RVA: 0x00023450 File Offset: 0x00021650
			public AIActor GetNearestEnemy(List<AIActor> activeEnemies, Vector2 position, out float nearestDistance, string[] filter)
			{
				AIActor aiactor = null;
				nearestDistance = float.MaxValue;
				bool flag = activeEnemies == null;
				bool flag2 = flag;
				bool flag3 = flag2;
				bool flag4 = flag3;
				AIActor result;
				if (flag4)
				{
					result = null;
				}
				else
				{
					for (int i = 0; i < activeEnemies.Count; i++)
					{
						AIActor aiactor2 = activeEnemies[i];
						bool flag5 = aiactor2.healthHaver && aiactor2.healthHaver.IsVulnerable;
						bool flag6 = flag5;
						bool flag7 = flag6;
						bool flag8 = flag7;
						if (flag8)
						{
							bool flag9 = !aiactor2.healthHaver.IsDead;
							bool flag10 = flag9;
							bool flag11 = flag10;
							bool flag12 = flag11;
							if (flag12)
							{
								bool flag13 = filter == null || !filter.Contains(aiactor2.EnemyGuid);
								bool flag14 = flag13;
								bool flag15 = flag14;
								bool flag16 = flag15;
								if (flag16)
								{
									float num = Vector2.Distance(position, aiactor2.CenterPosition);
									bool flag17 = num < nearestDistance;
									bool flag18 = flag17;
									bool flag19 = flag18;
									bool flag20 = flag19;
									if (flag20)
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

			// Token: 0x060003CB RID: 971 RVA: 0x00023570 File Offset: 0x00021770
			private void Attack()
			{
				bool flag = this.Owner == null;
				bool flag2 = flag;
				if (flag2)
				{
					this.Owner = this.m_aiActor.GetComponent<Winpetster.WinpetsterBehavior>().Owner;
				}
				float num = -1f;
				List<AIActor> activeEnemies = this.Owner.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
				bool flag3 = activeEnemies == null | activeEnemies.Count <= 0;
				bool flag4 = !flag3;
				if (flag4)
				{
					AIActor nearestEnemy = this.GetNearestEnemy(activeEnemies, this.m_aiActor.sprite.WorldCenter, out num, null);
					bool flag5 = nearestEnemy && num < 10f;
					bool flag6 = flag5;
					if (flag6)
					{
						bool flag7 = this.IsInRange(nearestEnemy);
						bool flag8 = flag7;
						if (flag8)
						{
							bool flag9 = !nearestEnemy.IsHarmlessEnemy && nearestEnemy.IsNormalEnemy && !nearestEnemy.healthHaver.IsDead && nearestEnemy != this.m_aiActor;
							bool flag10 = flag9;
							if (flag10)
							{
								Vector2 unitCenter = this.m_aiActor.specRigidbody.UnitCenter;
								Vector2 unitCenter2 = nearestEnemy.specRigidbody.HitboxPixelCollider.UnitCenter;
								float z = BraveMathCollege.Atan2Degrees((unitCenter2 - unitCenter).normalized);
								Projectile projectile = ((Gun)ETGMod.Databases.Items[1]).DefaultModule.projectiles[0];
								GameObject gameObject = SpawnManager.SpawnProjectile(projectile.gameObject, this.m_aiActor.sprite.WorldCenter, Quaternion.Euler(0f, 0f, z), true);
								Projectile component = gameObject.GetComponent<Projectile>();
								bool flag11 = component != null;
								bool flag12 = flag11;
								bool flag13 = flag12;
								if (flag13)
								{
									component.Owner = this.Owner;
									component.Shooter = this.m_aiActor.specRigidbody;
									bool flag14 = this.Owner.HasPickupID(402) || this.Owner.HasPickupID(1);
									if (flag14)
									{
										component.baseData.damage = 4f;
										component.AdditionalScaleMultiplier = 2f;
									}
									else
									{
										component.baseData.damage = 2.5f;
									}
									component.baseData.force = 1f;
									component.collidesWithPlayer = false;
								}
							}
						}
					}
				}
			}

			// Token: 0x060003CC RID: 972 RVA: 0x000237D0 File Offset: 0x000219D0
			public override float GetMaxRange()
			{
				return 20f;
			}

			// Token: 0x060003CD RID: 973 RVA: 0x000237E8 File Offset: 0x000219E8
			public override float GetMinReadyRange()
			{
				return 20f;
			}

			// Token: 0x060003CE RID: 974 RVA: 0x00023800 File Offset: 0x00021A00
			public override bool IsReady()
			{
				AIActor aiActor = this.m_aiActor;
				bool flag = aiActor == null;
				bool flag2;
				if (flag)
				{
					flag2 = true;
				}
				else
				{
					SpeculativeRigidbody targetRigidbody = aiActor.TargetRigidbody;
					Vector2? vector = (targetRigidbody != null) ? new Vector2?(targetRigidbody.UnitCenter) : null;
					flag2 = (vector == null);
				}
				return !flag2 && Vector2.Distance(this.m_aiActor.specRigidbody.UnitCenter, this.m_aiActor.TargetRigidbody.UnitCenter) <= this.GetMinReadyRange();
			}

			// Token: 0x060003CF RID: 975 RVA: 0x000238A0 File Offset: 0x00021AA0
			public bool IsInRange(AIActor enemy)
			{
				bool flag = enemy == null;
				bool flag2;
				if (flag)
				{
					flag2 = true;
				}
				else
				{
					SpeculativeRigidbody specRigidbody = enemy.specRigidbody;
					Vector2? vector = (specRigidbody != null) ? new Vector2?(specRigidbody.UnitCenter) : null;
					flag2 = (vector == null);
				}
				return !flag2 && Vector2.Distance(this.m_aiActor.specRigidbody.UnitCenter, enemy.specRigidbody.UnitCenter) <= this.GetMinReadyRange();
			}

			// Token: 0x0400021A RID: 538
			private bool isAttacking;

			// Token: 0x0400021B RID: 539
			private float attackCooldown = 0.9f;

			// Token: 0x0400021C RID: 540
			private float attackDuration = 0.01f;

			// Token: 0x0400021D RID: 541
			private float attackTimer;

			// Token: 0x0400021E RID: 542
			private float attackCooldownTimer;

			// Token: 0x0400021F RID: 543
			private PlayerController Owner;

			// Token: 0x04000220 RID: 544
			private List<AIActor> roomEnemies = new List<AIActor>();

		}


		// Token: 0x020000A2 RID: 162
		public class ApproachEnemiesBehavior : MovementBehaviorBase
		{
			// Token: 0x060003D1 RID: 977 RVA: 0x000215DF File Offset: 0x0001F7DF
			public override void Init(GameObject gameObject, AIActor aiActor, AIShooter aiShooter)
			{
				base.Init(gameObject, aiActor, aiShooter);
			}

			// Token: 0x060003D2 RID: 978 RVA: 0x00023958 File Offset: 0x00021B58
			public override void Upkeep()
			{
				base.Upkeep();
				base.DecrementTimer(ref this.repathTimer, false);
			}

			// Token: 0x060003D3 RID: 979 RVA: 0x00023970 File Offset: 0x00021B70
			public override BehaviorResult Update()
			{
				SpeculativeRigidbody overrideTarget = this.m_aiActor.OverrideTarget;
				bool flag = this.repathTimer > 0f;
				bool flag2 = flag;
				BehaviorResult result;
				if (flag2)
				{
					result = ((overrideTarget == null) ? BehaviorResult.Continue : BehaviorResult.SkipRemainingClassBehaviors);
				}
				else
				{
					bool flag3 = overrideTarget == null;
					bool flag4 = flag3;
					if (flag4)
					{
						this.PickNewTarget();
						result = BehaviorResult.Continue;
					}
					else
					{
						this.isInRange = (Vector2.Distance(this.m_aiActor.specRigidbody.UnitCenter, overrideTarget.UnitCenter) <= this.DesiredDistance);
						bool flag5 = overrideTarget != null && !this.isInRange;
						bool flag6 = flag5;
						if (flag6)
						{
							this.m_aiActor.PathfindToPosition(overrideTarget.UnitCenter, null, true, null, null, null, false);
							this.repathTimer = this.PathInterval;
							result = BehaviorResult.SkipRemainingClassBehaviors;
						}
						else
						{
							bool flag7 = overrideTarget != null && this.repathTimer >= 0f;
							bool flag8 = flag7;
							if (flag8)
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

			// Token: 0x060003D4 RID: 980 RVA: 0x00023AA8 File Offset: 0x00021CA8
			private void PickNewTarget()
			{
				bool flag = this.m_aiActor == null;
				bool flag2 = !flag;
				if (flag2)
				{
					bool flag3 = this.Owner == null;
					bool flag4 = flag3;
					if (flag4)
					{
						this.Owner = this.m_aiActor.GetComponent<Winpetster.WinpetsterBehavior>().Owner;
					}
					this.Owner.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All, ref this.roomEnemies);
					for (int i = 0; i < this.roomEnemies.Count; i++)
					{
						AIActor aiactor = this.roomEnemies[i];
						bool flag5 = aiactor.IsHarmlessEnemy || !aiactor.IsNormalEnemy || aiactor.healthHaver.IsDead || aiactor == this.m_aiActor || aiactor.EnemyGuid == "ba928393c8ed47819c2c5f593100a5bc";
						bool flag6 = flag5;
						if (flag6)
						{
							this.roomEnemies.Remove(aiactor);
						}
					}
					bool flag7 = this.roomEnemies.Count == 0;
					bool flag8 = flag7;
					if (flag8)
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

			// Token: 0x04000221 RID: 545
			public float PathInterval = 0.25f;

			// Token: 0x04000222 RID: 546
			public float DesiredDistance = 5f;

			// Token: 0x04000223 RID: 547
			private float repathTimer;

			// Token: 0x04000224 RID: 548
			private List<AIActor> roomEnemies = new List<AIActor>();

			// Token: 0x04000225 RID: 549
			private bool isInRange;

			// Token: 0x04000226 RID: 550
			private PlayerController Owner;
		}
	}
}
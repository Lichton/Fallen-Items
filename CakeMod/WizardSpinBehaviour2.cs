using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Brave.BulletScript;

namespace CakeMod
{
	// Token: 0x02000D6E RID: 3438
	public class WizardSpinShootBehavior2 : BasicAttackBehavior
	{
		// Token: 0x0600489C RID: 18588 RVA: 0x00178D4E File Offset: 0x00176F4E
		public WizardSpinShootBehavior2()
		{
			this.LineOfSight = true;
			this.m_bulletPositions = new List<Tuple<Projectile, float>>();
		}

		// Token: 0x0600489D RID: 18589 RVA: 0x00178D68 File Offset: 0x00176F68
		public override void Start()
		{
			m_isCharmed = this.m_aiActor.IsHarmlessEnemy;
			base.Start();
			ShootPoint = m_aiActor.transform;
			IntVector2 intVector = PhysicsEngine.UnitToPixel(this.ShootPoint.position - this.m_aiActor.transform.position);
			int num = PhysicsEngine.UnitToPixel(this.BulletCircleRadius);
			this.m_bulletCatcher = new PixelCollider();
			this.m_bulletCatcher.ColliderGenerationMode = PixelCollider.PixelColliderGeneration.Circle;
			this.m_bulletCatcher.CollisionLayer = CollisionLayer.BulletBlocker;
			this.m_bulletCatcher.IsTrigger = true;
			this.m_bulletCatcher.ManualOffsetX = intVector.x - num;
			this.m_bulletCatcher.ManualOffsetY = intVector.y - num;
			this.m_bulletCatcher.ManualDiameter = num * 2;
			this.m_bulletCatcher.Regenerate(this.m_aiActor.transform, true, true);
			this.m_aiActor.specRigidbody.PixelColliders.Add(this.m_bulletCatcher);
			SpeculativeRigidbody specRigidbody = this.m_aiActor.specRigidbody;
			specRigidbody.OnTriggerCollision += this.OnTriggerCollision;
		}

		// Token: 0x0600489E RID: 18590 RVA: 0x00178EB0 File Offset: 0x001770B0
		private void OnTriggerCollision(SpeculativeRigidbody specRigidbody, SpeculativeRigidbody sourceSpecRigidbody, CollisionData collisionData)
		{
			
			if ((this.State == WizardSpinShootBehavior2.SpinShootState.Spawn || this.State == WizardSpinShootBehavior2.SpinShootState.Prefire) && collisionData.MyPixelCollider == this.m_bulletCatcher && collisionData.OtherRigidbody != null && collisionData.OtherRigidbody.projectile != null)
			{
				Projectile projectile = collisionData.OtherRigidbody.projectile;
				bool flag = (!this.m_isCharmed) ? (projectile.Owner is PlayerController) : (projectile.Owner is AIActor);
				if (flag && projectile.CanBeCaught)
				{
					projectile.specRigidbody.DeregisterSpecificCollisionException(projectile.Owner.specRigidbody);
					projectile.Shooter = this.m_aiActor.specRigidbody;
					projectile.Owner = this.m_aiActor;
					projectile.specRigidbody.Velocity = Vector2.zero;
					projectile.ManualControl = true;
					projectile.baseData.SetAll(EnemyDatabase.GetOrLoadByGuid("c4fba8def15e47b297865b18e36cbef8").bulletBank.GetBullet("default").ProjectileData);
					projectile.UpdateSpeed();
					projectile.specRigidbody.CollideWithTileMap = false;
					projectile.ResetDistance();
					projectile.collidesWithEnemies = this.m_isCharmed;
					projectile.collidesWithPlayer = true;
					projectile.UpdateCollisionMask();
					projectile.sprite.color = new Color(1f, 0.1f, 0.1f);
					projectile.MakeLookLikeEnemyBullet(true);
					projectile.RemovePlayerOnlyModifiers();
					float second = BraveMathCollege.ClampAngle360((collisionData.Contact - this.ShootPoint.position.XY()).ToAngle());
					this.m_bulletPositions.Insert(Mathf.Max(0, this.m_bulletPositions.Count - 1), Tuple.Create<Projectile, float>(projectile, second));
				}
			}
		}

		// Token: 0x0600489F RID: 18591 RVA: 0x00179070 File Offset: 0x00177270
		public override void Upkeep()
		{
			base.Upkeep();
			this.m_stateTimer -= this.m_deltaTime;
			if (this.m_isCharmed != this.m_aiActor.CanTargetEnemies)
			{
				this.m_isCharmed = this.m_aiActor.CanTargetEnemies;
				for (int i = 0; i < this.m_bulletPositions.Count; i++)
				{
					Projectile first = this.m_bulletPositions[i].First;
					if (!(first == null))
					{
						first.collidesWithEnemies = this.m_isCharmed;
						first.UpdateCollisionMask();
					}
				}
			}
		}

		// Token: 0x060048A0 RID: 18592 RVA: 0x00179110 File Offset: 0x00177310
		public override BehaviorResult Update()
		{
			base.Update();
			BehaviorResult behaviorResult = base.Update();
			if (behaviorResult != BehaviorResult.Continue)
			{
				return behaviorResult;
			}
			if (!this.IsReady())
			{
				return BehaviorResult.Continue;
			}
			if (!this.m_aiActor.HasLineOfSightToTarget)
			{
				return BehaviorResult.Continue;
			}
			this.bottleowhiskey(SpinShootState.Spawn);
			this.m_updateEveryFrame = true;
			return BehaviorResult.RunContinuous;
		}

		// Token: 0x060048A1 RID: 18593 RVA: 0x00179164 File Offset: 0x00177364
		public override ContinuousBehaviorResult ContinuousUpdate()
		{
			for (int i = this.m_bulletPositions.Count - 1; i >= 0; i--)
			{
				float num = this.m_bulletPositions[i].Second + this.m_deltaTime * (float)this.BulletCircleSpeed;
				this.m_bulletPositions[i].Second = num;
				Projectile first = this.m_bulletPositions[i].First;
				if (!(first == null))
				{
					if (!first)
					{
						this.m_bulletPositions[i] = null;
					}
					else
					{
						Vector2 bulletPosition = this.GetBulletPosition(num);
						first.specRigidbody.Velocity = (bulletPosition.ToVector3XUp() - first.transform.position) / BraveTime.DeltaTime;
						if (first.shouldRotate)
						{
							first.transform.rotation = Quaternion.Euler(0f, 0f, 180f + (Quaternion.Euler(0f, 0f, 90f) * (this.ShootPoint.position.XY() - bulletPosition)).XY().ToAngle());
						}
						first.ResetDistance();
					}
				}
			}
			if (this.State == WizardSpinShootBehavior2.SpinShootState.Spawn)
			{
				while (this.m_stateTimer <= 0f && this.State == WizardSpinShootBehavior2.SpinShootState.Spawn)
				{
					AIBulletBank.Entry bullet = (EnemyDatabase.GetOrLoadByGuid("c4fba8def15e47b297865b18e36cbef8").bulletBank.GetBullet("default"));
					GameObject bulletObject = bullet.BulletObject;
					float num2 = 0f;
					if (this.m_bulletPositions.Count > 0)
					{
						num2 = BraveMathCollege.ClampAngle360(this.m_bulletPositions[this.m_bulletPositions.Count - 1].Second - this.BulletAngleDelta);
					}
					GameObject gameObject = SpawnManager.SpawnProjectile(bulletObject, this.GetBulletPosition(num2), Quaternion.Euler(0f, 0f, 0f), true);
					Projectile component = gameObject.GetComponent<Projectile>();
					if (bullet != null && bullet.OverrideProjectile)
					{
						component.baseData.SetAll(bullet.ProjectileData);
					}
					component.Shooter = this.m_aiActor.specRigidbody;
					component.specRigidbody.Velocity = Vector2.zero;
					component.ManualControl = true;
					component.specRigidbody.CollideWithTileMap = false;
					component.collidesWithEnemies = this.m_isCharmed;
					component.UpdateCollisionMask();
					this.m_bulletPositions.Add(Tuple.Create<Projectile, float>(component, num2));
					this.m_stateTimer += this.SpawnDelay;
					if (this.m_bulletPositions.Count >= this.NumBullets)
					{
						this.bottleowhiskey(SpinShootState.Prefire);
					}
				}
			}
			else if (this.State == WizardSpinShootBehavior2.SpinShootState.Prefire)
			{
				if (this.m_stateTimer <= 0f)
				{
					this.bottleowhiskey(SpinShootState.Fire);
				}
			}
			else if (this.State == WizardSpinShootBehavior2.SpinShootState.Fire)
			{
				if (this.m_behaviorSpeculator.TargetBehaviors != null && this.m_behaviorSpeculator.TargetBehaviors.Count > 0)
				{
					this.m_behaviorSpeculator.TargetBehaviors[0].Update();
				}
				if (this.m_bulletPositions.All((Tuple<Projectile, float> t) => t.First == null))
				{
					return ContinuousBehaviorResult.Finished;
				}
				while (this.m_stateTimer <= 0f)
				{
					Vector2 vector = this.ShootPoint.position.XY();
					Vector2 b = vector + ((!this.m_aiActor.TargetRigidbody) ? Vector2.zero : (this.m_aiActor.TargetRigidbody.UnitCenter - vector)).normalized * this.BulletCircleRadius;
					int num3 = -1;
					float num4 = float.MaxValue;
					for (int j = 0; j < this.m_bulletPositions.Count; j++)
					{
						Projectile first2 = this.m_bulletPositions[j].First;
						if (!(first2 == null))
						{
							float sqrMagnitude = (first2.specRigidbody.UnitCenter - b).sqrMagnitude;
							if (sqrMagnitude < num4)
							{
								num4 = sqrMagnitude;
								num3 = j;
							}
						}
					}
					if (num3 >= 0)
					{
						Projectile first3 = this.m_bulletPositions[num3].First;
						first3.ManualControl = false;
						first3.specRigidbody.CollideWithTileMap = true;
						if (this.m_aiActor.TargetRigidbody)
						{
							Vector2 unitCenter = this.m_aiActor.TargetRigidbody.specRigidbody.GetUnitCenter(ColliderType.HitBox);
							float speed = first3.Speed;
							float d = Vector2.Distance(first3.specRigidbody.UnitCenter, unitCenter) / speed;
							Vector2 b2 = unitCenter + this.m_aiActor.TargetRigidbody.specRigidbody.Velocity * d;
							Vector2 a = Vector2.Lerp(unitCenter, b2, this.LeadAmount);
							first3.SendInDirection(a - first3.specRigidbody.UnitCenter, true, true);
						}
						first3.transform.rotation = Quaternion.Euler(0f, 0f, first3.specRigidbody.Velocity.ToAngle());
						this.m_bulletPositions[num3].First = null;
					}
					else
					{
						Debug.LogError("WizardSpinShootBehaviour.ContinuousUpdate(): This shouldn't happen!");
					}
					this.m_stateTimer += this.FireDelay;
					if (this.m_bulletPositions.All((Tuple<Projectile, float> t) => t.First == null))
					{
						return ContinuousBehaviorResult.Finished;
					}
				}
			}
			return ContinuousBehaviorResult.Continue;
		}

		// Token: 0x060048A2 RID: 18594 RVA: 0x0017971F File Offset: 0x0017791F
		public override void EndContinuousUpdate()
		{
			base.EndContinuousUpdate();
			this.FreeRemainingProjectiles();
			this.bottleowhiskey(SpinShootState.None);
			this.m_aiAnimator.EndAnimationIf("attack");
			this.UpdateCooldowns();
			this.m_updateEveryFrame = false;
		}

		// Token: 0x060048A3 RID: 18595 RVA: 0x00179752 File Offset: 0x00177952
		public override void OnActorPreDeath()
		{
			base.OnActorPreDeath();
			SpeculativeRigidbody specRigidbody = this.m_aiActor.specRigidbody;
			specRigidbody.OnTriggerCollision = (SpeculativeRigidbody.OnTriggerDelegate)Delegate.Remove(specRigidbody.OnTriggerCollision, new SpeculativeRigidbody.OnTriggerDelegate(this.OnTriggerCollision));
			this.FreeRemainingProjectiles();
		}

		// Token: 0x060048A4 RID: 18596 RVA: 0x0017978C File Offset: 0x0017798C
		public override void Destroy()
		{
			base.Destroy();
			this.bottleowhiskey(SpinShootState.None);
		}

		// Token: 0x17000A78 RID: 2680
		// (get) Token: 0x060048A5 RID: 18597 RVA: 0x0017979B File Offset: 0x0017799B
		private float BulletAngleDelta
		{
			get
			{
				return 360f / (float)this.NumBullets;
			}
		}

		// Token: 0x060048A6 RID: 18598 RVA: 0x001797AA File Offset: 0x001779AA
		private Vector2 GetBulletPosition(float angle)
		{
			return this.ShootPoint.position.XY() + new Vector2(Mathf.Cos(angle * 0.0174532924f), Mathf.Sin(angle * 0.0174532924f)) * this.BulletCircleRadius;
		}

		// Token: 0x17000A79 RID: 2681
		// (get) Token: 0x060048A7 RID: 18599 RVA: 0x001797E9 File Offset: 0x001779E9
		// (set) Token: 0x060048A8 RID: 18600 RVA: 0x001797F1 File Offset: 0x001779F1
		private WizardSpinShootBehavior2.SpinShootState State
		{
			get
			{
				return this.m_state;
			}
		}

		public void bottleowhiskey(SpinShootState fuck)
        {
			this.EndState(this.m_state);
			this.m_state = fuck;
			this.BeginState(this.m_state);
		}

		// Token: 0x060048A9 RID: 18601 RVA: 0x00179814 File Offset: 0x00177A14
		private void BeginState(WizardSpinShootBehavior2.SpinShootState state)
		{
			if (state == WizardSpinShootBehavior2.SpinShootState.None)
			{
				this.m_bulletPositions.Clear();
			}
			if (state == WizardSpinShootBehavior2.SpinShootState.Spawn)
			{
				this.m_aiAnimator.PlayUntilCancelled("cast", true, null, -1f, false);
				this.m_stateTimer = this.FirstSpawnDelay;
				if (this.m_aiActor && this.m_aiActor.knockbackDoer)
				{
					this.m_aiActor.knockbackDoer.SetImmobile(true, "WizardSpinShootBehavior2");
				}
				this.m_aiActor.ClearPath();
			}
			else if (state == WizardSpinShootBehavior2.SpinShootState.Prefire)
			{
				this.m_aiAnimator.PlayUntilFinished("attack", true, null, -1f, false);
				this.m_stateTimer = this.PrefireDelay;
				if (this.PrefireUseAnimTime)
				{
					this.m_stateTimer += (float)this.m_aiAnimator.spriteAnimator.CurrentClip.frames.Length / this.m_aiAnimator.spriteAnimator.CurrentClip.fps;
				}
			}
			else if (state == WizardSpinShootBehavior2.SpinShootState.Fire)
			{
				this.m_stateTimer = this.FirstFireDelay;
			}
		}

		// Token: 0x060048AA RID: 18602 RVA: 0x001799A8 File Offset: 0x00177BA8
		private void EndState(WizardSpinShootBehavior2.SpinShootState state)
		{
			if (state == WizardSpinShootBehavior2.SpinShootState.Spawn)
			{
			}
			if (this.m_aiActor && this.m_aiActor.knockbackDoer)
			{
				this.m_aiActor.knockbackDoer.SetImmobile(false, "WizardSpinShootBehavior2");
			}
		}

		// Token: 0x060048AB RID: 18603 RVA: 0x00179A64 File Offset: 0x00177C64
		private void FreeRemainingProjectiles()
		{
			for (int i = 0; i < this.m_bulletPositions.Count; i++)
			{
				Projectile first = this.m_bulletPositions[i].First;
				if (!(first == null))
				{
					first.ManualControl = false;
					first.specRigidbody.CollideWithTileMap = true;
					first.SendInDirection((Quaternion.Euler(0f, 0f, 90f) * (first.specRigidbody.UnitCenter - this.ShootPoint.position.XY())).XY(), true, true);
					first.transform.rotation = Quaternion.Euler(0f, 0f, first.specRigidbody.Velocity.ToAngle());
					this.m_bulletPositions[i].First = null;
				}
			}
		}

		// Token: 0x04003C91 RID: 15505
		public bool LineOfSight;

		// Token: 0x04003C92 RID: 15506
		public string OverrideBulletName = "whydoineedthis";

		// Token: 0x04003C93 RID: 15507
		public bool CanHitEnemies = false;

		// Token: 0x04003C94 RID: 15508
		public Transform ShootPoint;

		// Token: 0x04003C95 RID: 15509
		public int NumBullets;

		// Token: 0x04003C96 RID: 15510
		public int BulletCircleSpeed = 3;

		// Token: 0x04003C97 RID: 15511
		public float BulletCircleRadius = 3f;

		// Token: 0x04003C98 RID: 15512
		public float FirstSpawnDelay = 0f;

		// Token: 0x04003C99 RID: 15513
		public float SpawnDelay = 0f;

		// Token: 0x04003C9A RID: 15514
		public bool PrefireUseAnimTime = true;

		// Token: 0x04003C9B RID: 15515
		public float PrefireDelay = 1f;

		// Token: 0x04003C9C RID: 15516
		public float FirstFireDelay = 1f;

		// Token: 0x04003C9D RID: 15517
		public float FireDelay = 1f;

		// Token: 0x04003C9E RID: 15518
		public float LeadAmount = 0f;

		// Token: 0x04003C9F RID: 15519

		// Token: 0x04003CA0 RID: 15520

		// Token: 0x04003CA1 RID: 15521
		private WizardSpinShootBehavior2.SpinShootState m_state;

		// Token: 0x04003CA2 RID: 15522
		private float m_stateTimer = 5;

		// Token: 0x04003CA3 RID: 15523
		private bool m_isCharmed;

		// Token: 0x04003CA4 RID: 15524
		private List<Tuple<Projectile, float>> m_bulletPositions;

		// Token: 0x04003CA5 RID: 15525
		private PixelCollider m_bulletCatcher;

		// Token: 0x02000D6F RID: 3439
		public enum SpinShootState
		{
			// Token: 0x04003CA9 RID: 15529
			None,
			// Token: 0x04003CAA RID: 15530
			Spawn,
			// Token: 0x04003CAB RID: 15531
			Prefire,
			// Token: 0x04003CAC RID: 15532
			Fire
		}
	}
}
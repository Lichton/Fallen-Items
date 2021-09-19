using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace CakeMod
{
	// Token: 0x02000006 RID: 6
	internal class MindControlNotGamer : MonoBehaviour
	{
		// Token: 0x06000018 RID: 24 RVA: 0x00002E5C File Offset: 0x0000105C
		public MindControlNotGamer()
		{
			this.m_attackedThisCycle = true;
		}

		// Token: 0x06000019 RID: 25 RVA: 0x00002E7C File Offset: 0x0000107C
		private void Start()
		{
			this.m_aiActor = base.GetComponent<AIActor>();
			this.m_behaviorSpeculator = this.m_aiActor.behaviorSpeculator;
			GameObject gameObject = new GameObject("fake target");
			this.m_fakeActor = gameObject.AddComponent<NonActor>();
			this.m_fakeActor.HasShadow = false;
			this.m_fakeTargetRigidbody = gameObject.AddComponent<SpeculativeRigidbody>();
			this.m_fakeTargetRigidbody.PixelColliders = new List<PixelCollider>();
			this.m_fakeTargetRigidbody.CollideWithTileMap = true;
			this.m_fakeTargetRigidbody.CollideWithOthers = true;
			this.m_fakeTargetRigidbody.CanBeCarried = true;
			this.m_fakeTargetRigidbody.CanBePushed = true;
			this.m_fakeTargetRigidbody.CanCarry = true;
			PixelCollider pixelCollider = new PixelCollider();
			pixelCollider.ColliderGenerationMode = PixelCollider.PixelColliderGeneration.Manual;
			pixelCollider.CollisionLayer = CollisionLayer.TileBlocker;
			pixelCollider.ManualWidth = 4;
			pixelCollider.ManualHeight = 4;
			this.m_fakeTargetRigidbody.PixelColliders.Add(pixelCollider);
			
			this.m_aiActor.LocalTimeScale = 1.5f;
			foreach (MovementBehaviorBase movementBehaviorBase in this.m_behaviorSpeculator.MovementBehaviors)
			{
				bool flag = movementBehaviorBase is SeekTargetBehavior;
				if (flag)
				{
					SeekTargetBehavior seekTargetBehavior = movementBehaviorBase as SeekTargetBehavior;
					seekTargetBehavior.PathInterval = 0f;
				}
				bool flag2 = movementBehaviorBase is MoveErraticallyBehavior;
				if (flag2)
				{
					MoveErraticallyBehavior moveErraticallyBehavior = movementBehaviorBase as MoveErraticallyBehavior;
					moveErraticallyBehavior.PathInterval = 0f;
				}
				bool flag3 = movementBehaviorBase is FleeTargetBehavior;
				if (flag3)
				{
					FleeTargetBehavior fleeTargetBehavior = movementBehaviorBase as FleeTargetBehavior;
					fleeTargetBehavior.PathInterval = 0f;
				}
			}
		}

		// Token: 0x0600001A RID: 26 RVA: 0x00003070 File Offset: 0x00001270
		private Vector2 GetPlayerAimPointController(Vector2 aimBase, Vector2 aimDirection)
		{
			Func<SpeculativeRigidbody, bool> rigidbodyExcluder = (SpeculativeRigidbody otherRigidbody) => otherRigidbody.minorBreakable && !otherRigidbody.minorBreakable.stopsBullets;
			Vector2 result = aimBase + aimDirection * 10f;
			CollisionLayer layer = CollisionLayer.EnemyHitBox;
			int rayMask = CollisionMask.LayerToMask(CollisionLayer.HighObstacle, CollisionLayer.BulletBlocker, layer, CollisionLayer.BulletBreakable);
			RaycastResult raycastResult;
			bool flag = PhysicsEngine.Instance.Raycast(aimBase, aimDirection, 50f, out raycastResult, true, true, rayMask, null, false, rigidbodyExcluder, null);
			if (flag)
			{
				result = aimBase + aimDirection * raycastResult.Distance;
			}
			RaycastResult.Pool.Free(ref raycastResult);
			return result;
		}

		// Token: 0x0600001B RID: 27 RVA: 0x00003110 File Offset: 0x00001310
		private void UpdateAimTargetPosition()
		{
			PlayerController playerController = this.owner;
			BraveInput instanceForPlayer = BraveInput.GetInstanceForPlayer(playerController.PlayerIDX);
			GungeonActions activeActions = instanceForPlayer.ActiveActions;
			Vector3 position = this.m_aiActor.CenterPosition + BraveMathCollege.DegreesToVector(this.owner.FacingDirection, 1f) * 5f;
			bool flag = instanceForPlayer.IsKeyboardAndMouse(false);
			if (flag)
			{
				this.m_fakeTargetRigidbody.transform.position = position;
			}
			else
			{
				this.m_fakeTargetRigidbody.transform.position = position;
			}
			this.m_fakeTargetRigidbody.Reinitialize();
		}

		// Token: 0x0600001C RID: 28 RVA: 0x000031B0 File Offset: 0x000013B0
		private void Update()
		{
			this.m_fakeActor.specRigidbody = this.m_fakeTargetRigidbody;
			bool flag = this.m_aiActor;
			if (flag)
			{
				this.m_aiActor.CanTargetEnemies = false;
				this.m_aiActor.CanTargetPlayers = true;
				this.m_aiActor.PlayerTarget = this.m_fakeActor;
				this.m_aiActor.OverrideTarget = null;
				this.UpdateAimTargetPosition();
				bool flag2 = this.m_aiActor.aiShooter;
				if (flag2)
				{
					this.m_aiActor.aiShooter.AimAtPoint(this.m_behaviorSpeculator.PlayerTarget.CenterPosition);
				}
			}
			bool flag3 = this.m_behaviorSpeculator;
			if (flag3)
			{
				PlayerController playerController = this.owner;
				BraveInput instanceForPlayer = BraveInput.GetInstanceForPlayer(playerController.PlayerIDX);
				GungeonActions activeActions = instanceForPlayer.ActiveActions;
				bool flag4 = this.m_behaviorSpeculator.AttackCooldown <= 0f;
				if (flag4)
				{
					bool flag5 = !this.m_attackedThisCycle && this.m_behaviorSpeculator.ActiveContinuousAttackBehavior != null;
					if (flag5)
					{
						this.m_attackedThisCycle = true;
					}
					bool flag6 = this.m_attackedThisCycle && this.m_behaviorSpeculator.ActiveContinuousAttackBehavior == null;
					if (flag6)
					{
						this.m_behaviorSpeculator.AttackCooldown = float.MaxValue;
						bool flag7 = this.dashBehav != null;
						if (flag7)
						{
						}
						bool flag8 = this.TeleBehav != null;
						if (flag8)
						{
							this.TeleBehav.RequiresLineOfSight = true;
							this.TeleBehav.MinRange = 1000f;
							this.TeleBehav.Range = 0.1f;
						}
					}
				}
				else
				{
					bool wasPressed = activeActions.ShootAction.WasPressed;
					if (wasPressed)
					{
						this.m_attackedThisCycle = false;
						this.m_behaviorSpeculator.AttackCooldown = 0f;
					}
					else
					{
						bool wasPressed2 = activeActions.DodgeRollAction.WasPressed;
						if (wasPressed2)
						{
							this.m_attackedThisCycle = false;
							bool flag9 = this.dashBehav != null;
							if (flag9)
							{
								bool flag10 = !this.isDashingOrTPing;
								if (flag10)
								{
									this.dashBehav.RequiresLineOfSight = false;
									this.dashBehav.MinRange = 3f;
									this.dashBehav.Range = 8f;
									base.StartCoroutine(this.DoDash(this.dashBehav.dashTime));
								}
							}
							bool flag11 = this.TeleBehav != null;
							if (flag11)
							{
								bool flag12 = !this.isDashingOrTPing;
								if (flag12)
								{
									this.TeleBehav.RequiresLineOfSight = false;
									this.TeleBehav.MinRange = 3f;
									this.TeleBehav.Range = 17f;
									base.StartCoroutine(this.DoTP());
								}
							}
						}
					}
				}
				bool flag13 = this.m_behaviorSpeculator.TargetBehaviors != null && this.m_behaviorSpeculator.TargetBehaviors.Count > 0;
				if (flag13)
				{
					this.m_behaviorSpeculator.TargetBehaviors.Clear();
				}
				bool flag14 = this.m_behaviorSpeculator.MovementBehaviors != null && this.m_behaviorSpeculator.MovementBehaviors.Count > 0;
				if (flag14)
				{
					this.m_behaviorSpeculator.MovementBehaviors.Clear();
				}
				this.m_aiActor.ImpartedVelocity += activeActions.Move.Value * this.m_aiActor.MovementSpeed * this.m_aiActor.LocalTimeScale;
				bool flag15 = this.m_behaviorSpeculator.AttackBehaviors != null;
				if (flag15)
				{
					for (int i = 0; i < this.m_behaviorSpeculator.AttackBehaviors.Count; i++)
					{
						AttackBehaviorBase attack = this.m_behaviorSpeculator.AttackBehaviors[i];
						this.ProcessAttack(attack);
					}
				}
			}
		}

		// Token: 0x0600001D RID: 29 RVA: 0x0000357A File Offset: 0x0000177A
		private IEnumerator DoTP()
		{
			this.isDashingOrTPing = true;
			bool flag = this.TeleBehav.teleportOutAnim != null;
			if (flag)
			{
				this.m_aiActor.aiAnimator.PlayUntilFinished(this.TeleBehav.teleportOutAnim, false, null, -1f, false);
				while (this.m_aiActor.aiAnimator.IsPlaying(this.TeleBehav.teleportOutAnim))
				{
					yield return null;
				}
			}
			this.DoTeleport.Invoke(this.TeleBehav, null);
			bool flag2 = this.TeleBehav.teleportInAnim != null;
			if (flag2)
			{
				this.m_aiActor.aiAnimator.PlayUntilFinished(this.TeleBehav.teleportInAnim, false, null, -1f, false);
				while (this.m_aiActor.aiAnimator.IsPlaying(this.TeleBehav.teleportInAnim))
				{
					yield return null;
				}
			}
			this.isDashingOrTPing = false;
			yield break;
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00003589 File Offset: 0x00001789
		private IEnumerator DoDash(float dashtime)
		{
			this.isDashingOrTPing = true;
			bool flag = this.dashBehav.chargeAnim != null;
			if (flag)
			{
				this.m_aiActor.aiAnimator.PlayUntilFinished(this.dashBehav.chargeAnim, false, null, -1f, false);
				while (this.m_aiActor.aiAnimator.IsPlaying(this.dashBehav.chargeAnim))
				{
					yield return null;
				}
			}
			this.BeginState.Invoke(this.dashBehav, this.State);
			yield return new WaitForSeconds(dashtime);
			this.EndState.Invoke(this.dashBehav, this.State);
			this.isDashingOrTPing = false;
			yield break;
		}

		// Token: 0x0600001F RID: 31 RVA: 0x000035A0 File Offset: 0x000017A0
		private void ProcessAttack(AttackBehaviorBase attack)
		{
			bool flag = attack == null;
			if (!flag)
			{
				bool flag2 = attack is BasicAttackBehavior;
				if (flag2)
				{
					BasicAttackBehavior basicAttackBehavior = attack as BasicAttackBehavior;
					basicAttackBehavior.Cooldown = 0f;
					basicAttackBehavior.RequiresLineOfSight = false;
					basicAttackBehavior.MinRange = -1f;
					basicAttackBehavior.Range = -1f;
					bool flag3 = attack is TeleportBehavior;
					if (flag3)
					{
						this.TeleBehav = (basicAttackBehavior as TeleportBehavior);
						basicAttackBehavior.RequiresLineOfSight = true;
						basicAttackBehavior.MinRange = 1000f;
						basicAttackBehavior.Range = 0.1f;
						this.DoTeleport = typeof(TeleportBehavior).GetMethod("DoTeleport", BindingFlags.Instance | BindingFlags.NonPublic);
					}
					bool flag4 = attack is DashBehavior;
					if (flag4)
					{
						this.dashBehav = (basicAttackBehavior as DashBehavior);
						basicAttackBehavior.RequiresLineOfSight = true;
						basicAttackBehavior.MinRange = 1000f;
						basicAttackBehavior.Range = 0.1f;
						this.BeginState = typeof(DashBehavior).GetMethod("BeginState", BindingFlags.Instance | BindingFlags.NonPublic);
						this.State[0] = typeof(DashBehavior).GetNestedType("DashState", BindingFlags.NonPublic).GetField("Charge").GetValue(this.dashBehav);
						this.EndState = typeof(DashBehavior).GetMethod("EndState", BindingFlags.Instance | BindingFlags.NonPublic);
					}
					bool flag5 = basicAttackBehavior is ShootGunBehavior;
					if (flag5)
					{
						ShootGunBehavior shootGunBehavior = basicAttackBehavior as ShootGunBehavior;
						shootGunBehavior.LineOfSight = false;
						shootGunBehavior.EmptiesClip = false;
						shootGunBehavior.Cooldown = 0.3f;
						shootGunBehavior.RespectReload = true;
					}
				}
				else
				{
					bool flag6 = attack is AttackBehaviorGroup;
					if (flag6)
					{
						AttackBehaviorGroup attackBehaviorGroup = attack as AttackBehaviorGroup;
						for (int i = 0; i < attackBehaviorGroup.AttackBehaviors.Count; i++)
						{
							this.ProcessAttack(attackBehaviorGroup.AttackBehaviors[i].Behavior);
						}
					}
				}
			}
		}

		// Token: 0x04000008 RID: 8
		private MethodInfo BeginState;

		// Token: 0x04000009 RID: 9
		private MethodInfo EndState;

		// Token: 0x0400000A RID: 10
		private MethodInfo DoTeleport;

		// Token: 0x0400000B RID: 11
		private object[] State = new object[0];

		// Token: 0x0400000C RID: 12
		private DashBehavior dashBehav;

		// Token: 0x0400000D RID: 13
		private TeleportBehavior TeleBehav;

		// Token: 0x0400000E RID: 14
		[NonSerialized]
		public PlayerController owner;

		// Token: 0x0400000F RID: 15
		private AIActor m_aiActor;

		// Token: 0x04000010 RID: 16
		private BehaviorSpeculator m_behaviorSpeculator;

		// Token: 0x04000011 RID: 17
		private bool m_attackedThisCycle;

		// Token: 0x04000012 RID: 18
		private bool isDashingOrTPing;

		// Token: 0x04000013 RID: 19
		private NonActor m_fakeActor;

		// Token: 0x04000014 RID: 20
		private SpeculativeRigidbody m_fakeTargetRigidbody;

		// Token: 0x04000015 RID: 21
	}
}

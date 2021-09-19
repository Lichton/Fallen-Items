using System;
using Dungeonator;
using UnityEngine;

// Token: 0x02000DE7 RID: 3559
public class CompanionFollowCursorBehaviour : MovementBehaviorBase
{
	// Token: 0x06004B5B RID: 19291 RVA: 0x0018ED28 File Offset: 0x0018CF28
	public CompanionFollowCursorBehaviour()
	{
		this.PathInterval = 0.25f;
		this.DisableInCombat = true;
		this.IdealRadius = 1.5f;
		this.CatchUpRadius = 7f;
		this.CatchUpAccelTime = 5f;
		this.CatchUpSpeed = 7f;
		this.CatchUpMaxSpeed = 10f;
		this.RollAnimation = "roll";
		this.m_idleTimer = 2f;
	}

	// Token: 0x06004B5C RID: 19292 RVA: 0x0018ED9A File Offset: 0x0018CF9A
	public override void Start()
	{
		base.Start();
		this.m_companionController = this.m_gameObject.GetComponent<CompanionController>();
	}

	// Token: 0x06004B5D RID: 19293 RVA: 0x0018EDB3 File Offset: 0x0018CFB3
	public override void Upkeep()
	{
		base.Upkeep();
		base.DecrementTimer(ref this.m_repathTimer, false);
	}

	// Token: 0x06004B5E RID: 19294 RVA: 0x0018EDC8 File Offset: 0x0018CFC8
	private void CatchUpMovementModifier(ref Vector2 voluntaryVel, ref Vector2 involuntaryVel)
	{
		if (this.DisableInCombat)
		{
			PlayerController playerController = GameManager.Instance.PrimaryPlayer;
			if (this.m_aiActor && this.m_aiActor.CompanionOwner)
			{
				playerController = this.m_aiActor.CompanionOwner;
			}
			if (playerController && playerController.IsInCombat && Vector2.Distance(playerController.unadjustedAimPoint, this.m_aiActor.AimCenter) < this.CatchUpRadius)
			{
				this.m_isCatchingUp = false;
				if (!string.IsNullOrEmpty(this.CatchUpOutAnimation))
				{
					this.m_aiAnimator.PlayUntilFinished(this.CatchUpOutAnimation, false, null, -1f, false);
				}
				this.m_aiActor.MovementModifiers -= this.CatchUpMovementModifier;
				return;
			}
		}
		this.m_catchUpTime += this.m_aiActor.LocalDeltaTime;
		voluntaryVel = voluntaryVel.normalized * Mathf.Lerp(this.CatchUpSpeed, this.CatchUpMaxSpeed, this.m_catchUpTime / this.CatchUpAccelTime);
	}

	// Token: 0x06004B5F RID: 19295 RVA: 0x0018EEE4 File Offset: 0x0018D0E4
	public override ContinuousBehaviorResult ContinuousUpdate()
	{
		if (!this.m_aiAnimator.IsPlaying(this.RollAnimation))
		{
			return ContinuousBehaviorResult.Finished;
		}
		if (this.m_aiAnimator.CurrentClipProgress > 0.7f)
		{
			this.m_aiActor.FallingProhibited = false;
			this.m_aiActor.BehaviorVelocity = this.m_aiActor.BehaviorVelocity.normalized * 2f;
		}
		return base.ContinuousUpdate();
	}

	// Token: 0x06004B60 RID: 19296 RVA: 0x0018EF55 File Offset: 0x0018D155
	public override void EndContinuousUpdate()
	{
		this.m_updateEveryFrame = false;
		this.m_triedToPathOverPit = false;
		this.m_groundRolling = false;
		this.m_aiActor.FallingProhibited = false;
		this.m_aiActor.BehaviorOverridesVelocity = false;
		base.EndContinuousUpdate();
	}

	// Token: 0x06004B61 RID: 19297 RVA: 0x0018EF8C File Offset: 0x0018D18C
	public override BehaviorResult Update()
	{
		if (!GameManager.HasInstance || GameManager.Instance.IsLoadingLevel)
		{
			return BehaviorResult.SkipAllRemainingBehaviors;
		}
		if (GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.END_TIMES)
		{
			this.m_aiActor.ClearPath();
			return BehaviorResult.SkipAllRemainingBehaviors;
		}
		if (this.TemporarilyDisabled)
		{
			return BehaviorResult.Continue;
		}
		base.DecrementTimer(ref this.m_idleTimer, false);
		this.m_aiActor.DustUpInterval = Mathf.Lerp(0.5f, 0.125f, this.m_aiActor.specRigidbody.Velocity.magnitude / this.CatchUpSpeed);
		PlayerController playerController = GameManager.Instance.PrimaryPlayer;
		if (this.m_aiActor && this.m_aiActor.CompanionOwner)
		{
			playerController = this.m_aiActor.CompanionOwner;
		}
		if (this.CanRollOverPits && this.m_triedToPathOverPit)
		{
			if (this.m_aiActor.IsOverPitAtAll && !this.m_wasOverPit)
			{
				Debug.Log("running continuous");
				this.m_aiActor.FallingProhibited = true;
				this.m_aiAnimator.PlayUntilFinished(this.RollAnimation, false, null, -1f, false);
				Vector2 normalized = this.m_aiActor.specRigidbody.Velocity.normalized;
				this.m_aiActor.BehaviorOverridesVelocity = true;
				this.m_aiActor.BehaviorVelocity = normalized * 7f;
				this.m_aiActor.ClearPath();
				this.m_updateEveryFrame = true;
				return BehaviorResult.RunContinuous;
			}
			this.m_wasOverPit = this.m_aiActor.IsOverPitAtAll;
		}
		IntVector2 intVector = this.m_aiActor.specRigidbody.UnitCenter.ToIntVector2(VectorConversions.Floor);
		CellData cellData = GameManager.Instance.Dungeon.data[intVector];
		if (cellData != null && cellData.IsPlayerInaccessible)
		{
			if (this.m_repathTimer <= 0f)
			{
				this.m_repathTimer = this.PathInterval;
				RoomHandler absoluteRoomFromPosition = GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(intVector);
				if (absoluteRoomFromPosition != null)
				{
					IntVector2? nearestAvailableCell = absoluteRoomFromPosition.GetNearestAvailableCell(intVector.ToCenterVector2(), new IntVector2?(this.m_aiActor.Clearance), new CellTypes?(this.m_aiActor.PathableTiles), false, (IntVector2 pos) => !GameManager.Instance.Dungeon.data[pos].IsPlayerInaccessible);
					if (nearestAvailableCell != null)
					{
						this.m_aiActor.PathfindToPosition(nearestAvailableCell.Value.ToCenterVector2(), null, true, null, null, null, false);
					}
				}
			}
			return BehaviorResult.SkipRemainingClassBehaviors;
		}
		if (!playerController)
		{
			return BehaviorResult.Continue;
		}
		if (!playerController.IsStealthed && playerController.CurrentRoom != null && playerController.CurrentRoom.HasActiveEnemies(RoomHandler.ActiveEnemyType.All) && this.m_aiActor.TargetRigidbody && this.m_aiActor.transform.position.GetAbsoluteRoom() == playerController.CurrentRoom && this.DisableInCombat)
		{
			IntVector2 intVector2 = (!this.m_aiActor.specRigidbody) ? this.m_aiActor.transform.position.IntXY(VectorConversions.Floor) : this.m_aiActor.specRigidbody.UnitCenter.ToIntVector2(VectorConversions.Floor);
			if (GameManager.Instance.Dungeon.data.CheckInBoundsAndValid(intVector2) && !GameManager.Instance.Dungeon.data[intVector2].isExitCell)
			{
				if (this.m_isCatchingUp)
				{
					this.m_isCatchingUp = false;
					if (!string.IsNullOrEmpty(this.CatchUpOutAnimation))
					{
						this.m_aiAnimator.PlayUntilFinished(this.CatchUpOutAnimation, false, null, -1f, false);
					}
					this.m_aiActor.MovementModifiers -= this.CatchUpMovementModifier;
				}
				return BehaviorResult.Continue;
			}
		}
		bool flag = false;
		if (this.m_companionController && this.m_companionController.IsBeingPet)
		{
			flag = true;
		}
		float num = Vector2.Distance(playerController.unadjustedAimPoint, this.m_aiActor.AimCenter);
		if (num <= this.IdealRadius && !flag)
		{
			this.m_aiActor.ClearPath();
			if (this.m_isCatchingUp)
			{
				this.m_isCatchingUp = false;
				if (!string.IsNullOrEmpty(this.CatchUpOutAnimation))
				{
					this.m_aiAnimator.PlayUntilFinished(this.CatchUpOutAnimation, false, null, -1f, false);
				}
				this.m_aiActor.MovementModifiers -= this.CatchUpMovementModifier;
			}
			if (this.m_idleTimer <= 0f && this.IdleAnimations != null && this.IdleAnimations.Length > 0)
			{
				this.m_aiAnimator.PlayUntilFinished(this.IdleAnimations[UnityEngine.Random.Range(0, this.IdleAnimations.Length)], false, null, -1f, false);
				this.m_idleTimer = (float)UnityEngine.Random.Range(3, 10);
			}
			return BehaviorResult.SkipRemainingClassBehaviors;
		}
		if (num > 30f)
		{
			this.m_sequentialPathFails = 0;
			this.m_aiActor.CompanionWarp(this.m_aiActor.CompanionOwner.unadjustedAimPoint);
		}
		else if (!this.m_isCatchingUp && num > this.CatchUpRadius)
		{
			this.m_isCatchingUp = true;
			this.m_catchUpTime = 0f;
			if (!string.IsNullOrEmpty(this.CatchUpAnimation))
			{
				this.m_aiAnimator.PlayUntilFinished(this.CatchUpAnimation, false, null, -1f, false);
			}
			this.m_aiActor.MovementModifiers += this.CatchUpMovementModifier;
		}
		this.m_idleTimer = Mathf.Max(this.m_idleTimer, 2f);
		if (this.m_repathTimer <= 0f && !playerController.IsOverPitAtAll && !playerController.IsInMinecart)
		{
			this.m_repathTimer = this.PathInterval;
			this.m_triedToPathOverPit = false;
			this.m_aiActor.FallingProhibited = false;
			if (flag)
			{
				Vector2 vector = this.m_companionController.m_pettingDoer.specRigidbody.UnitCenter + this.m_companionController.m_petOffset;
				if (Vector2.Distance(vector, this.m_aiActor.specRigidbody.UnitCenter) < 0.08f)
				{
					this.m_aiActor.ClearPath();
				}
				else
				{
					this.m_aiActor.PathfindToPosition(vector, new Vector2?(vector), true, null, null, null, false);
				}
			}
			else
			{
				this.m_aiActor.PathfindToPosition(playerController.unadjustedAimPoint, null, true, null, null, null, false);
			}
			if (this.m_aiActor.Path != null && this.m_aiActor.Path.InaccurateLength > 50f)
			{
				this.m_aiActor.ClearPath();
				this.m_sequentialPathFails = 0;
				this.m_aiActor.CompanionWarp(this.m_aiActor.CompanionOwner.unadjustedAimPoint);
			}
			else if (this.m_aiActor.Path != null && !this.m_aiActor.Path.WillReachFinalGoal)
			{
				bool flag2 = false;
				if (this.CanRollOverPits)
				{
					this.m_aiActor.PathableTiles = (this.m_aiActor.PathableTiles | CellTypes.PIT);
					this.m_aiActor.PathfindToPosition(playerController.unadjustedAimPoint, null, true, null, null, null, false);
					this.m_aiActor.PathableTiles = (this.m_aiActor.PathableTiles & ~CellTypes.PIT);
					if (this.m_aiActor.Path != null && this.m_aiActor.Path.WillReachFinalGoal)
					{
						this.m_triedToPathOverPit = true;
						this.m_aiActor.FallingProhibited = true;
						flag2 = true;
					}
				}
				if (!flag2)
				{
					this.m_sequentialPathFails++;
					IntVector2 key = this.m_aiActor.CompanionOwner.AimCenter.ToIntVector2(VectorConversions.Floor);
					CellData cellData2 = GameManager.Instance.Dungeon.data[key];
					if (this.m_sequentialPathFails > 3 && cellData2 != null && cellData2.IsPassable)
					{
						this.m_sequentialPathFails = 0;
						this.m_aiActor.CompanionWarp(this.m_aiActor.CompanionOwner.unadjustedAimPoint);
					}
				}
			}
			else
			{
				this.m_sequentialPathFails = 0;
			}
		}
		if (!this.m_aiShooter || this.m_aiShooter.EquippedGun)
		{
		}
		return BehaviorResult.SkipRemainingClassBehaviors;
	}

	// Token: 0x040040D8 RID: 16600
	public float PathInterval;

	// Token: 0x040040D9 RID: 16601
	public bool DisableInCombat;

	// Token: 0x040040DA RID: 16602
	public float IdealRadius;

	// Token: 0x040040DB RID: 16603
	public float CatchUpRadius;

	// Token: 0x040040DC RID: 16604
	public float CatchUpAccelTime;

	// Token: 0x040040DD RID: 16605
	public float CatchUpSpeed;

	// Token: 0x040040DE RID: 16606
	public float CatchUpMaxSpeed;

	// Token: 0x040040DF RID: 16607
	public string CatchUpAnimation;

	// Token: 0x040040E0 RID: 16608
	public string CatchUpOutAnimation;

	// Token: 0x040040E1 RID: 16609
	public string[] IdleAnimations;

	// Token: 0x040040E2 RID: 16610
	public bool CanRollOverPits;

	// Token: 0x040040E3 RID: 16611
	public string RollAnimation;

	// Token: 0x040040E4 RID: 16612
	private bool m_isCatchingUp;

	// Token: 0x040040E5 RID: 16613
	private float m_catchUpTime;

	// Token: 0x040040E6 RID: 16614
	[NonSerialized]
	public bool TemporarilyDisabled;

	// Token: 0x040040E7 RID: 16615
	protected bool m_triedToPathOverPit;

	// Token: 0x040040E8 RID: 16616
	protected bool m_wasOverPit;

	// Token: 0x040040E9 RID: 16617
	protected bool m_groundRolling;

	// Token: 0x040040EA RID: 16618
	private int m_sequentialPathFails;

	// Token: 0x040040EB RID: 16619
	private float m_idleTimer;

	// Token: 0x040040EC RID: 16620
	private float m_repathTimer;

	// Token: 0x040040ED RID: 16621
	private CompanionController m_companionController;
}

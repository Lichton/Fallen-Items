using System;
using System.Collections.Generic;
using Dungeonator;
using FullInspector;
using Pathfinding;
using UnityEngine;

public class TeleportToListOfAreasBehavior : BasicAttackBehavior
{
	// Token: 0x0600486A RID: 18538 RVA: 0x001A3F50 File Offset: 0x001A2150
	public TeleportToListOfAreasBehavior()
	{
		this.GoneTime = 1f;
		this.teleportOutAnim = "teleport_out";
		this.teleportInAnim = "teleport_in";
		this.hasOutlinesDuringAnim = true;
	}

	// Token: 0x0600486B RID: 18539 RVA: 0x0002F686 File Offset: 0x0002D886
	private bool ShowShadowAnimationNames()
	{
		return this.shadowSupport == TeleportToListOfAreasBehavior.ShadowSupport.Animate;
	}

	// Token: 0x0600486C RID: 18540 RVA: 0x0002F691 File Offset: 0x0002D891
	public override void Start()
	{
		base.Start();
		tk2dSpriteAnimator spriteAnimator = this.m_aiActor.spriteAnimator;
		spriteAnimator.AnimationEventTriggered = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>)Delegate.Combine(spriteAnimator.AnimationEventTriggered, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>(this.AnimationEventTriggered));
	}

	// Token: 0x0600486D RID: 18541 RVA: 0x0002F6C5 File Offset: 0x0002D8C5
	public override void Upkeep()
	{
		base.Upkeep();
		base.DecrementTimer(ref this.m_timer, false);
		if (this.goneAttackBehavior != null)
		{
			this.goneAttackBehavior.Upkeep();
		}
	}

	// Token: 0x0600486E RID: 18542 RVA: 0x001A3FA8 File Offset: 0x001A21A8
	public override bool IsReady()
	{
		if (this.OnlyTeleportIfPlayerUnreachable)
		{
			bool flag = this.m_aiActor.GetAbsoluteParentRoom() == GameManager.Instance.BestActivePlayer.CurrentRoom;
			if (flag && this.m_aiActor.Path != null && this.m_aiActor.Path.WillReachFinalGoal)
			{
				return false;
			}
		}
		return base.IsReady();
	}
	public override BehaviorResult Update()
	{
		base.Update();
		if (this.m_shadowSprite == null)
		{
			this.m_shadowSprite = this.m_aiActor.ShadowObject.GetComponent<tk2dBaseSprite>();
		}
		if (!this.IsReady())
		{
			return BehaviorResult.Continue;
		}
		if (!this.m_aiActor.TargetRigidbody)
		{
			return BehaviorResult.Continue;
		}
		this.SetState(TeleportToListOfAreasBehavior.TeleportState.TeleportOut);
		this.m_updateEveryFrame = true;
		return BehaviorResult.RunContinuous;
	}

	// Token: 0x06004870 RID: 18544 RVA: 0x001A4080 File Offset: 0x001A2280
	public override ContinuousBehaviorResult ContinuousUpdate()
	{
		base.ContinuousUpdate();
		if (this.State == TeleportToListOfAreasBehavior.TeleportState.TeleportOut)
		{
			if (this.shadowSupport == TeleportToListOfAreasBehavior.ShadowSupport.Fade)
			{
				this.m_shadowSprite.color = this.m_shadowSprite.color.WithAlpha(1f - this.m_aiAnimator.CurrentClipProgress);
			}
			if (!this.m_aiAnimator.IsPlaying(this.teleportOutAnim))
			{
				this.SetState(TeleportToListOfAreasBehavior.TeleportState.Gone);
			}
		}
		else if (this.State == TeleportToListOfAreasBehavior.TeleportState.Gone)
		{
			if (this.m_timer <= 0f)
			{
				this.SetState(TeleportToListOfAreasBehavior.TeleportState.GoneBehavior);
			}
		}
		else if (this.State == TeleportToListOfAreasBehavior.TeleportState.GoneBehavior)
		{
			if (this.goneAttackBehavior.ContinuousUpdate() == ContinuousBehaviorResult.Finished)
			{
				this.SetState(TeleportToListOfAreasBehavior.TeleportState.TeleportIn);
			}
		}
		else if (this.State == TeleportToListOfAreasBehavior.TeleportState.TeleportIn)
		{
			if (this.shadowSupport == TeleportToListOfAreasBehavior.ShadowSupport.Fade)
			{
				this.m_shadowSprite.color = this.m_shadowSprite.color.WithAlpha(this.m_aiAnimator.CurrentClipProgress);
			}
			if (this.m_aiShooter)
			{
				this.m_aiShooter.ToggleGunAndHandRenderers(false, "TeleportToListOfAreasBehavior");
			}
			if (!this.m_aiAnimator.IsPlaying(this.teleportInAnim))
			{
				this.SetState(TeleportToListOfAreasBehavior.TeleportState.None);
				return ContinuousBehaviorResult.Finished;
			}
		}
		return ContinuousBehaviorResult.Continue;
	}

	// Token: 0x06004871 RID: 18545 RVA: 0x001A41CC File Offset: 0x001A23CC
	public override void EndContinuousUpdate()
	{
		base.EndContinuousUpdate();
		if (this.teleportRequiresTransparency && this.m_cachedShader)
		{
			this.m_aiActor.sprite.usesOverrideMaterial = false;
			this.m_aiActor.renderer.material.shader = this.m_cachedShader;
			this.m_cachedShader = null;
		}
		this.m_aiActor.sprite.renderer.enabled = true;
		if (this.m_aiActor.knockbackDoer)
		{
			this.m_aiActor.knockbackDoer.SetImmobile(false, "teleport");
		}
		this.m_aiActor.specRigidbody.CollideWithOthers = true;
		this.m_aiActor.IsGone = false;
		if (this.m_aiShooter)
		{
			this.m_aiShooter.ToggleGunAndHandRenderers(true, "TeleportToListOfAreasBehavior");
		}
		if (!this.hasOutlinesDuringAnim)
		{
			SpriteOutlineManager.ToggleOutlineRenderers(this.m_aiActor.sprite, true);
		}
		if (this.goneAttackBehavior != null && this.State == TeleportToListOfAreasBehavior.TeleportState.GoneBehavior)
		{
			this.goneAttackBehavior.EndContinuousUpdate();
		}
		this.m_aiAnimator.EndAnimationIf(this.teleportOutAnim);
		this.m_aiAnimator.EndAnimationIf(this.teleportInAnim);
		if (this.shadowSupport == TeleportToListOfAreasBehavior.ShadowSupport.Fade)
		{
			this.m_shadowSprite.color = this.m_shadowSprite.color.WithAlpha(1f);
		}
		else if (this.shadowSupport == TeleportToListOfAreasBehavior.ShadowSupport.Animate)
		{
			tk2dSpriteAnimationClip clipByName = this.m_shadowSprite.spriteAnimator.GetClipByName(this.shadowInAnim);
			this.m_shadowSprite.spriteAnimator.Play(clipByName, (float)(clipByName.frames.Length - 1), clipByName.fps, false);
		}
		this.m_state = TeleportToListOfAreasBehavior.TeleportState.None;
		this.m_updateEveryFrame = false;
		this.UpdateCooldowns();
	}

	// Token: 0x06004872 RID: 18546 RVA: 0x0002F6F0 File Offset: 0x0002D8F0
	public override void Init(GameObject gameObject, AIActor aiActor, AIShooter aiShooter)
	{
		base.Init(gameObject, aiActor, aiShooter);
		if (this.goneAttackBehavior != null)
		{
			this.goneAttackBehavior.Init(gameObject, aiActor, aiShooter);
		}
	}

	// Token: 0x06004873 RID: 18547 RVA: 0x0002F714 File Offset: 0x0002D914
	public override void SetDeltaTime(float deltaTime)
	{
		base.SetDeltaTime(deltaTime);
		if (this.goneAttackBehavior != null)
		{
			this.goneAttackBehavior.SetDeltaTime(deltaTime);
		}
	}

	// Token: 0x06004874 RID: 18548 RVA: 0x0002F734 File Offset: 0x0002D934
	public override bool UpdateEveryFrame()
	{
		if (this.goneAttackBehavior != null && this.m_state == TeleportToListOfAreasBehavior.TeleportState.GoneBehavior)
		{
			return this.goneAttackBehavior.UpdateEveryFrame();
		}
		return base.UpdateEveryFrame();
	}

	// Token: 0x06004875 RID: 18549 RVA: 0x001A439C File Offset: 0x001A259C
	public void AnimationEventTriggered(tk2dSpriteAnimator animator, tk2dSpriteAnimationClip clip, int frame)
	{
		if (this.m_shouldFire && clip.GetFrame(frame).eventInfo == "fire")
		{
			if (this.State == TeleportToListOfAreasBehavior.TeleportState.TeleportIn)
			{
				SpawnManager.SpawnBulletScript(this.m_aiActor, this.teleportInBulletScript, null, null, false, null);
			}
			else if (this.State == TeleportToListOfAreasBehavior.TeleportState.TeleportOut)
			{
				SpawnManager.SpawnBulletScript(this.m_aiActor, this.teleportOutBulletScript, null, null, false, null);
			}
			this.m_shouldFire = false;
		}
		else if (this.State == TeleportToListOfAreasBehavior.TeleportState.TeleportOut && clip.GetFrame(frame).eventInfo == "teleport_collider_off")
		{
			this.m_aiActor.specRigidbody.CollideWithOthers = false;
			this.m_aiActor.IsGone = true;
		}
	}

	// Token: 0x17000A75 RID: 2677
	// (get) Token: 0x06004876 RID: 18550 RVA: 0x0002F75F File Offset: 0x0002D95F
	// (set) Token: 0x06004877 RID: 18551 RVA: 0x0002F767 File Offset: 0x0002D967
	private TeleportToListOfAreasBehavior.TeleportState State
	{
		get
		{
			return this.m_state;
		}
	}

	private void SetState(TeleportToListOfAreasBehavior.TeleportState value)
	{
		this.EndState(this.m_state);
		this.m_state = value;
		this.BeginState(this.m_state);
	}

	// Token: 0x06004878 RID: 18552 RVA: 0x001A4488 File Offset: 0x001A2688
	private void BeginState(TeleportToListOfAreasBehavior.TeleportState state)
	{
		if (state == TeleportToListOfAreasBehavior.TeleportState.TeleportOut)
		{
			if (this.teleportOutBulletScript != null && !this.teleportOutBulletScript.IsNull)
			{
				this.m_shouldFire = true;
			}
			if (this.teleportRequiresTransparency)
			{
				this.m_cachedShader = this.m_aiActor.renderer.material.shader;
				this.m_aiActor.sprite.usesOverrideMaterial = true;
				this.m_aiActor.renderer.material.shader = ShaderCache.Acquire("Brave/LitBlendUber");
			}
			this.m_aiAnimator.PlayUntilCancelled(this.teleportOutAnim, true, null, -1f, false);
			if (this.shadowSupport == TeleportToListOfAreasBehavior.ShadowSupport.Animate)
			{
				this.m_shadowSprite.spriteAnimator.PlayAndForceTime(this.shadowOutAnim, this.m_aiAnimator.CurrentClipLength);
			}
			if (this.m_aiActor.knockbackDoer)
			{
				this.m_aiActor.knockbackDoer.SetImmobile(true, "teleport");
			}
			this.m_aiActor.ClearPath();
			if (!this.AttackableDuringAnimation)
			{
				this.m_aiActor.specRigidbody.CollideWithOthers = false;
				this.m_aiActor.IsGone = true;
			}
			if (this.m_aiShooter)
			{
				this.m_aiShooter.ToggleGunAndHandRenderers(false, "TeleportToListOfAreasBehavior");
			}
			if (!this.hasOutlinesDuringAnim)
			{
				SpriteOutlineManager.ToggleOutlineRenderers(this.m_aiActor.sprite, false);
			}
		}
		else if (state == TeleportToListOfAreasBehavior.TeleportState.Gone)
		{
			if (this.GoneTime <= 0f)
			{
				this.SetState(TeleportToListOfAreasBehavior.TeleportState.GoneBehavior);
				return;
			}
			this.m_timer = this.GoneTime;
			this.m_aiActor.specRigidbody.CollideWithOthers = false;
			this.m_aiActor.IsGone = true;
			this.m_aiActor.sprite.renderer.enabled = false;
		}
		else if (this.State == TeleportToListOfAreasBehavior.TeleportState.GoneBehavior)
		{
			if (this.goneAttackBehavior == null)
			{
				this.SetState(TeleportToListOfAreasBehavior.TeleportState.TeleportIn);
				return;
			}
			BehaviorResult behaviorResult = this.goneAttackBehavior.Update();
			if (behaviorResult != BehaviorResult.RunContinuous && behaviorResult != BehaviorResult.RunContinuousInClass)
			{
				this.SetState(TeleportToListOfAreasBehavior.TeleportState.TeleportIn);
			}
		}
		else if (state == TeleportToListOfAreasBehavior.TeleportState.TeleportIn)
		{
			if (this.teleportInBulletScript != null && !this.teleportInBulletScript.IsNull)
			{
				this.m_shouldFire = true;
			}
			this.DoTeleport();
			this.m_aiAnimator.PlayUntilFinished(this.teleportInAnim, true, null, -1f, false);
			if (this.shadowSupport == TeleportToListOfAreasBehavior.ShadowSupport.Animate)
			{
				this.m_shadowSprite.spriteAnimator.PlayAndForceTime(this.shadowInAnim, this.m_aiAnimator.CurrentClipLength);
			}
			this.m_shadowSprite.renderer.enabled = true;
			if (this.AttackableDuringAnimation)
			{
				this.m_aiActor.specRigidbody.CollideWithOthers = true;
				this.m_aiActor.IsGone = false;
			}
			this.m_aiActor.sprite.renderer.enabled = true;
			if (this.m_aiShooter)
			{
				this.m_aiShooter.ToggleGunAndHandRenderers(false, "TeleportToListOfAreasBehavior");
			}
			if (this.hasOutlinesDuringAnim)
			{
				SpriteOutlineManager.ToggleOutlineRenderers(this.m_aiActor.sprite, true);
			}
		}
	}

	// Token: 0x06004879 RID: 18553 RVA: 0x001A47A8 File Offset: 0x001A29A8
	private void EndState(TeleportToListOfAreasBehavior.TeleportState state)
	{
		if (state == TeleportToListOfAreasBehavior.TeleportState.TeleportOut)
		{
			this.m_shadowSprite.renderer.enabled = false;
			if (this.hasOutlinesDuringAnim)
			{
				SpriteOutlineManager.ToggleOutlineRenderers(this.m_aiActor.sprite, false);
			}
			if (this.teleportOutBulletScript != null && !this.teleportOutBulletScript.IsNull && this.m_shouldFire)
			{
				SpawnManager.SpawnBulletScript(this.m_aiActor, this.teleportOutBulletScript, null, null, false, null);
				this.m_shouldFire = false;
			}
		}
		else if (state == TeleportToListOfAreasBehavior.TeleportState.TeleportIn)
		{
			if (this.teleportRequiresTransparency && this.m_cachedShader)
			{
				this.m_aiActor.sprite.usesOverrideMaterial = false;
				this.m_aiActor.renderer.material.shader = this.m_cachedShader;
				this.m_cachedShader = null;
			}
			if (this.shadowSupport == TeleportToListOfAreasBehavior.ShadowSupport.Fade)
			{
				this.m_shadowSprite.color = this.m_shadowSprite.color.WithAlpha(1f);
			}
			if (this.m_aiActor.knockbackDoer)
			{
				this.m_aiActor.knockbackDoer.SetImmobile(false, "teleport");
			}
			this.m_aiActor.specRigidbody.CollideWithOthers = true;
			this.m_aiActor.IsGone = false;
			if (this.m_aiShooter)
			{
				this.m_aiShooter.ToggleGunAndHandRenderers(true, "TeleportToListOfAreasBehavior");
			}
			if (this.teleportInBulletScript != null && !this.teleportInBulletScript.IsNull && this.m_shouldFire)
			{
				SpawnManager.SpawnBulletScript(this.m_aiActor, this.teleportInBulletScript, null, null, false, null);
				this.m_shouldFire = false;
			}
			if (!this.hasOutlinesDuringAnim)
			{
				SpriteOutlineManager.ToggleOutlineRenderers(this.m_aiActor.sprite, true);
			}
		}
	}

	// Token: 0x0600487A RID: 18554 RVA: 0x001A4998 File Offset: 0x001A2B98
	private void DoTeleport()
	{
		Vector2 playerLowerLeft = Vector2.zero;
		Vector2 playerUpperRight = Vector2.zero;
		bool hasOtherPlayer = false;
		Vector2 otherPlayerLowerLeft = Vector2.zero;
		Vector2 otherPlayerUpperRight = Vector2.zero;

		IntVector2 bottomLeft = IntVector2.Zero;
		IntVector2 topRight = IntVector2.Zero;
		CellValidator cellValidator = delegate (IntVector2 c)
		{
			for (int i = 0; i < this.m_aiActor.Clearance.x; i++)
			{
				int num = c.x + i;
				for (int j = 0; j < this.m_aiActor.Clearance.y; j++)
				{
					int num2 = c.y + j;
					if (GameManager.Instance.Dungeon.data.isTopWall(num, num2))
					{
						return false;
					}
					if (this.ManuallyDefineRoom && ((float)num < this.roomMin.x || (float)num > this.roomMax.x || (float)num2 < this.roomMin.y || (float)num2 > this.roomMax.y))
					{
						return false;
					}
				}
			}

			return true;
		};
		Vector2 b = this.m_aiActor.specRigidbody.UnitBottomCenter - this.m_aiActor.transform.position.XY();
		IntVector2? intVector = null;
		List<IntVector2> randomAvailableCell = new List<IntVector2>()
		{
			this.m_aiActor.ParentRoom.area.UnitCenter.ToIntVector2() + new IntVector2(0, 5),
			this.m_aiActor.ParentRoom.area.UnitCenter.ToIntVector2() + new IntVector2(-5,0 ),
			this.m_aiActor.ParentRoom.area.UnitCenter.ToIntVector2() + new IntVector2(5, 0),
		};
       
	    ;//this.m_aiActor.ParentRoom.GetRandomAvailableCell(new IntVector2?(this.m_aiActor.Clearance), new CellTypes?(this.m_aiActor.PathableTiles), false, cellValidator);
		if (randomAvailableCell != null)
		{
			IntVector2 id = BraveUtility.RandomElement<IntVector2>(randomAvailableCell);
			this.m_aiActor.transform.position = Pathfinder.GetClearanceOffset(id, this.m_aiActor.Clearance).WithY((float)id.y) - b;
			this.m_aiActor.specRigidbody.Reinitialize();
		}
		else
		{
			ETGModConsole.Log("TELEPORT FAILED!", this.m_aiActor);
		}
	}

	// Token: 0x04003C34 RID: 15412
	public bool AttackableDuringAnimation;

	// Token: 0x04003C39 RID: 15417
	public float GoneTime;

	// Token: 0x04003C3A RID: 15418
	[InspectorCategory("Conditions")]
	public bool OnlyTeleportIfPlayerUnreachable;

	// Token: 0x04003C3B RID: 15419
	[InspectorCategory("Attack")]
	public BulletScriptSelector teleportOutBulletScript;

	// Token: 0x04003C3C RID: 15420
	[InspectorCategory("Attack")]
	public BulletScriptSelector teleportInBulletScript;

	// Token: 0x04003C3D RID: 15421
	[InspectorCategory("Attack")]
	public AttackBehaviorBase goneAttackBehavior;

	// Token: 0x04003C3E RID: 15422
	[InspectorCategory("Attack")]
	public bool AllowCrossRoomTeleportation;

	// Token: 0x04003C3F RID: 15423
	[InspectorCategory("Visuals")]
	public string teleportOutAnim;

	// Token: 0x04003C40 RID: 15424
	[InspectorCategory("Visuals")]
	public string teleportInAnim;

	// Token: 0x04003C41 RID: 15425
	[InspectorCategory("Visuals")]
	public bool teleportRequiresTransparency;

	// Token: 0x04003C42 RID: 15426
	[InspectorCategory("Visuals")]
	public bool hasOutlinesDuringAnim;

	// Token: 0x04003C43 RID: 15427
	[InspectorCategory("Visuals")]
	public TeleportToListOfAreasBehavior.ShadowSupport shadowSupport;

	// Token: 0x04003C44 RID: 15428
	[InspectorCategory("Visuals")]
	[InspectorShowIf("ShowShadowAnimationNames")]
	public string shadowOutAnim;

	// Token: 0x04003C45 RID: 15429
	[InspectorShowIf("ShowShadowAnimationNames")]
	[InspectorCategory("Visuals")]
	public string shadowInAnim;

	// Token: 0x04003C46 RID: 15430
	public bool ManuallyDefineRoom;

	// Token: 0x04003C47 RID: 15431
	[InspectorShowIf("ManuallyDefineRoom")]
	[InspectorIndent]
	public Vector2 roomMin;

	// Token: 0x04003C48 RID: 15432
	[InspectorShowIf("ManuallyDefineRoom")]
	[InspectorIndent]
	public Vector2 roomMax;

	// Token: 0x04003C49 RID: 15433
	private tk2dBaseSprite m_shadowSprite;

	// Token: 0x04003C4A RID: 15434
	private Shader m_cachedShader;

	// Token: 0x04003C4B RID: 15435
	private float m_timer;

	// Token: 0x04003C4C RID: 15436
	private bool m_shouldFire;

	// Token: 0x04003C4D RID: 15437
	private TeleportToListOfAreasBehavior.TeleportState m_state;

	// Token: 0x02000D66 RID: 3430
	public enum ShadowSupport
	{
		// Token: 0x04003C4F RID: 15439
		None,
		// Token: 0x04003C50 RID: 15440
		Fade,
		// Token: 0x04003C51 RID: 15441
		Animate
	}

	// Token: 0x02000D67 RID: 3431
	private enum TeleportState
	{
		// Token: 0x04003C53 RID: 15443
		None,
		// Token: 0x04003C54 RID: 15444
		TeleportOut,
		// Token: 0x04003C55 RID: 15445
		Gone,
		// Token: 0x04003C56 RID: 15446
		GoneBehavior,
		// Token: 0x04003C57 RID: 15447
		TeleportIn
	}
}
using System;
using System.Collections;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;
using CakeMod;

// Token: 0x02001461 RID: 5217
public class CursonGuonController : BraveBehaviour, CursorOrbitalFloats
{
	// Token: 0x0600768A RID: 30346 RVA: 0x002E3E58 File Offset: 0x002E2058
	public CursonGuonController()
	{
		this.numToShoot = 1;
		this.shootCooldown = 1f;
		this.orbitRadius = 3f;
		this.orbitDegreesPerSecond = 90f;
		this.shouldRotate = true;
		this.DamageToEnemiesOnShot = 10f;
		this.DamageToEnemiesOnShotCooldown = 3f;
		this.SinWavelength = 3f;
		this.SinAmplitude = 1f;
	}

	// Token: 0x170011A5 RID: 4517
	// (get) Token: 0x0600768B RID: 30347 RVA: 0x002E3EC6 File Offset: 0x002E20C6
	public PlayerController Owner
	{
		get
		{
			return this.m_owner;
		}
	}

    public object OverrideTargetPosition { get; private set; }

    // Token: 0x0600768C RID: 30348 RVA: 0x002E3ED0 File Offset: 0x002E20D0
    public static int GetNumberOfOrbitalsInTier(PlayerController owner, int tier)
	{
		int num = 0;
		for (int i = 0; i < owner.orbitals.Count; i++)
		{
			if (owner.orbitals[i].GetOrbitalTier() == tier)
			{
				num++;
			}
		}
		return num;
	}

	// Token: 0x0600768D RID: 30349 RVA: 0x002E3F18 File Offset: 0x002E2118
	public static int CalculateTargetTier(PlayerController owner, CursonGuonOrbital orbital)
	{
		float orbitalRadius = orbital.GetOrbitalRadius();
		float orbitalRotationalSpeed = orbital.GetOrbitalRotationalSpeed();
		int num = -1;
		for (int i = 0; i < owner.orbitals.Count; i++)
		{
			if (owner.orbitals[i] != orbital)
			{
				num = Mathf.Max(num, owner.orbitals[i].GetOrbitalTier());
				float orbitalRadius2 = owner.orbitals[i].GetOrbitalRadius();
				float orbitalRotationalSpeed2 = owner.orbitals[i].GetOrbitalRotationalSpeed();
				if (Mathf.Approximately(orbitalRadius2, orbitalRadius) && Mathf.Approximately(orbitalRotationalSpeed2, orbitalRotationalSpeed))
				{
					return owner.orbitals[i].GetOrbitalTier();
				}
			}
		}
		return num + 1;
	}

	// Token: 0x0600768E RID: 30350 RVA: 0x002E3FD8 File Offset: 0x002E21D8
	public void Initialize(PlayerController owner)
	{
		this.m_initialized = true;
		this.m_owner = owner;
		Debug.LogError(string.Concat(new object[]
		{
			"new orbital tier: ",
			this.GetOrbitalTier(),
			" and index: ",
			this.GetOrbitalTierIndex()
		}));
		owner.orbitals.Add((IPlayerOrbital)this);
		base.sprite = base.GetComponentInChildren<tk2dSprite>();
		base.spriteAnimator = base.GetComponentInChildren<tk2dSpriteAnimator>();
		if (!this.PreventOutline)
		{
			SpriteOutlineManager.AddOutlineToSprite(base.sprite, Color.black);
		}
		this.m_ownerCenterAverage = this.m_owner.CenterPosition;
		if (base.specRigidbody && (this.DamagesEnemiesOnShot || this.TriggersMachoBraceOnShot))
		{
			SpeculativeRigidbody specRigidbody = base.specRigidbody;
			specRigidbody.OnRigidbodyCollision = (SpeculativeRigidbody.OnRigidbodyCollisionDelegate)Delegate.Combine(specRigidbody.OnRigidbodyCollision, new SpeculativeRigidbody.OnRigidbodyCollisionDelegate(this.HandleRigidbodyCollision));
		}
		if (base.specRigidbody && this.ExplodesOnTriggerCollision)
		{
			SpeculativeRigidbody specRigidbody2 = base.specRigidbody;
			specRigidbody2.OnTriggerCollision = (SpeculativeRigidbody.OnTriggerDelegate)Delegate.Combine(specRigidbody2.OnTriggerCollision, new SpeculativeRigidbody.OnTriggerDelegate(this.HandleTriggerCollisionExplosion));
		}
	}

	// Token: 0x0600768F RID: 30351 RVA: 0x002E4130 File Offset: 0x002E2330
	private void HandleTriggerCollisionExplosion(SpeculativeRigidbody otherRigidbody, SpeculativeRigidbody sourceSpecRigidbody, CollisionData collisionData)
	{
		if (otherRigidbody && otherRigidbody.aiActor && Time.time - this.m_lastExplosionTime > 5f)
		{
			this.m_lastExplosionTime = Time.time;
			Exploder.Explode(base.specRigidbody.UnitCenter, this.TriggerExplosionData, Vector2.zero, null, false, CoreDamageTypes.None, false);
			this.Disappear();
		}
	}

	// Token: 0x06007690 RID: 30352 RVA: 0x002E41A3 File Offset: 0x002E23A3
	private void Disappear()
	{
		base.specRigidbody.enabled = false;
		SpriteOutlineManager.ToggleOutlineRenderers(base.sprite, false);
		base.sprite.renderer.enabled = false;
	}

	// Token: 0x06007691 RID: 30353 RVA: 0x002E41D0 File Offset: 0x002E23D0
	private void Reappear()
	{
		base.specRigidbody.enabled = true;
		base.sprite.renderer.enabled = true;
		SpriteOutlineManager.ToggleOutlineRenderers(base.sprite, true);
		base.specRigidbody.Reinitialize();
		PhysicsEngine.Instance.RegisterOverlappingGhostCollisionExceptions(base.specRigidbody, null, false);
	}

	// Token: 0x06007692 RID: 30354 RVA: 0x002E422C File Offset: 0x002E242C
	public void DecoupleBabyDragun()
	{
		this.m_owner.orbitals.Remove((IPlayerOrbital)this);
		if (base.specRigidbody)
		{
			SpeculativeRigidbody specRigidbody = base.specRigidbody;
			specRigidbody.OnRigidbodyCollision = (SpeculativeRigidbody.OnRigidbodyCollisionDelegate)Delegate.Remove(specRigidbody.OnRigidbodyCollision, new SpeculativeRigidbody.OnRigidbodyCollisionDelegate(this.HandleRigidbodyCollision));
		}
		UnityEngine.Object.Destroy(this);
	}

	// Token: 0x06007693 RID: 30355 RVA: 0x002E42BC File Offset: 0x002E24BC
	private void HandleRigidbodyCollision(CollisionData rigidbodyCollision)
	{
		if (rigidbodyCollision.OtherRigidbody.projectile)
		{
			if (this.DamagesEnemiesOnShot && this.m_damageOnShotCooldown <= 0f)
			{
				if (this.m_owner)
				{
					base.StartCoroutine(this.FlashSprite(base.sprite, 1f));
					this.m_owner.CurrentRoom.ApplyActionToNearbyEnemies(this.m_owner.CenterPosition, 100f, delegate (AIActor enemy, float dist)
					{
						if (enemy && enemy.healthHaver)
						{
							enemy.healthHaver.ApplyDamage(this.DamageToEnemiesOnShot, Vector2.zero, string.Empty, CoreDamageTypes.None, DamageCategory.Normal, false, null, false);
						}
					});
				}
				this.m_damageOnShotCooldown = this.DamageToEnemiesOnShotCooldown;
			}
			if (this.TriggersMachoBraceOnShot && this.m_owner)
			{
				for (int i = 0; i < this.m_owner.passiveItems.Count; i++)
				{
					if (this.m_owner.passiveItems[i] is MachoBraceItem)
					{
						(this.m_owner.passiveItems[i] as MachoBraceItem).ForceTrigger(this.m_owner);
						break;
					}
				}
			}
		}
	}

	// Token: 0x06007694 RID: 30356 RVA: 0x002E43D8 File Offset: 0x002E25D8
	private IEnumerator FlashSprite(tk2dBaseSprite targetSprite, float flashTime = 1f)
	{
		Color overrideColor = Color.white;
		overrideColor.a = 1f;
		if (targetSprite)
		{
			targetSprite.usesOverrideMaterial = true;
		}
		Color startColor = targetSprite.renderer.material.GetColor("_OverrideColor");
		Material targetMaterial = targetSprite.renderer.material;
		for (float elapsed = 0f; elapsed < flashTime; elapsed += BraveTime.DeltaTime)
		{
			float t = 1f - elapsed / flashTime;
			targetMaterial.SetColor("_OverrideColor", Color.Lerp(startColor, overrideColor, t));
			targetMaterial.SetFloat("_SaturationModifier", Mathf.Lerp(1f, 5f, t));
			yield return null;
		}
		targetSprite.renderer.material.SetColor("_OverrideColor", startColor);
		targetMaterial.SetFloat("_SaturationModifier", 1f);
		yield break;
	}

	// Token: 0x06007695 RID: 30357 RVA: 0x002E43FC File Offset: 0x002E25FC
	private void Update()
	{
		if (!this.m_initialized)
		{
			return;
		}
		if (this.ExplodesOnTriggerCollision && !base.specRigidbody.enabled && Time.time - this.m_lastExplosionTime > 5f)
		{
			this.Reappear();
		}
		this.HandleMotion();
		this.HandleCombat();
		bool flag = false;
		bool flag2 = false;
		
		if (flag && !flag2 && !string.IsNullOrEmpty(this.IdleAnimation) && !base.spriteAnimator.IsPlaying(this.IdleAnimation))
		{
			base.spriteAnimator.Play(this.IdleAnimation);
		}
		if (this.motionStyle != CursonGuonController.OrbitalMotionStyle.ORBIT_TARGET)
		{
			this.m_retargetTimer -= BraveTime.DeltaTime;
		}
		if (this.shootProjectile && base.specRigidbody)
		{
			if (this.m_hasLuteBuff && (!this.m_owner || !this.m_owner.CurrentGun || !this.m_owner.CurrentGun.LuteCompanionBuffActive))
			{
				if (this.m_luteOverheadVfx)
				{
					UnityEngine.Object.Destroy(this.m_luteOverheadVfx);
					this.m_luteOverheadVfx = null;
				}
				if (base.specRigidbody)
				{
					SpeculativeRigidbody specRigidbody = base.specRigidbody;
					specRigidbody.OnPostRigidbodyMovement = (Action<SpeculativeRigidbody, Vector2, IntVector2>)Delegate.Remove(specRigidbody.OnPostRigidbodyMovement, new Action<SpeculativeRigidbody, Vector2, IntVector2>(this.UpdateVFXOnMovement));
				}
				this.m_hasLuteBuff = false;
			}
			else if (!this.m_hasLuteBuff && this.m_owner && this.m_owner.CurrentGun && this.m_owner.CurrentGun.LuteCompanionBuffActive)
			{
				GameObject prefab = (GameObject)ResourceCache.Acquire("Global VFX/VFX_Buff_Status");
				this.m_luteOverheadVfx = SpawnManager.SpawnVFX(prefab, base.specRigidbody.UnitCenter.ToVector3ZisY(0f).Quantize(0.0625f) + new Vector3(0f, 1f, 0f), Quaternion.identity);
				if (base.specRigidbody)
				{
					SpeculativeRigidbody specRigidbody2 = base.specRigidbody;
					specRigidbody2.OnPostRigidbodyMovement = (Action<SpeculativeRigidbody, Vector2, IntVector2>)Delegate.Combine(specRigidbody2.OnPostRigidbodyMovement, new Action<SpeculativeRigidbody, Vector2, IntVector2>(this.UpdateVFXOnMovement));
				}
				this.m_hasLuteBuff = true;
			}
		}
		this.m_damageOnShotCooldown -= BraveTime.DeltaTime;
		this.m_shootTimer -= BraveTime.DeltaTime;
	}

	// Token: 0x06007696 RID: 30358 RVA: 0x002E4710 File Offset: 0x002E2910
	private void UpdateVFXOnMovement(SpeculativeRigidbody arg1, Vector2 arg2, IntVector2 arg3)
	{
		if (this.m_hasLuteBuff && this.m_luteOverheadVfx)
		{
			this.m_luteOverheadVfx.transform.position = base.specRigidbody.UnitCenter.ToVector3ZisY(0f).Quantize(0.0625f) + new Vector3(0f, 1f, 0f);
		}
	}

	// Token: 0x06007697 RID: 30359 RVA: 0x002E4780 File Offset: 0x002E2980
	protected override void OnDestroy()
	{
		for (int i = 0; i < this.m_owner.orbitals.Count; i++)
		{
			if (this.m_owner.orbitals[i].GetOrbitalTier() == this.GetOrbitalTier() && this.m_owner.orbitals[i].GetOrbitalTierIndex() > this.GetOrbitalTierIndex())
			{
				this.m_owner.orbitals[i].SetOrbitalTierIndex(this.m_owner.orbitals[i].GetOrbitalTierIndex() - 1);
			}
		}
		this.m_owner.orbitals.Remove((IPlayerOrbital)this);
	}

	// Token: 0x06007698 RID: 30360 RVA: 0x002E4830 File Offset: 0x002E2A30
	public void Reinitialize()
	{
		base.specRigidbody.Reinitialize();
		this.m_ownerCenterAverage = this.m_owner.CenterPosition;
	}

	// Token: 0x06007699 RID: 30361 RVA: 0x002E484E File Offset: 0x002E2A4E
	public void ReinitializeWithDelta(Vector2 delta)
	{
		base.specRigidbody.Reinitialize();
		this.m_ownerCenterAverage += delta;
	}

	// Token: 0x0600769A RID: 30362 RVA: 0x002E4870 File Offset: 0x002E2A70

	private void HandleMotion()
	{
		Vector2 centerPosition = this.m_owner.unadjustedAimPoint;
		
	}

	// Token: 0x0600769B RID: 30363 RVA: 0x002E4AAC File Offset: 0x002E2CAC
	private void AcquireTarget()
	{
		this.m_retargetTimer = 0.25f;
		this.m_currentTarget = null;
		if (this.m_owner == null || this.m_owner.CurrentRoom == null)
		{
			return;
		}
		List<AIActor> activeEnemies = this.m_owner.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
		if (activeEnemies != null && activeEnemies.Count > 0)
		{
			AIActor aiactor = null;
			float num = -1f;
			for (int i = 0; i < activeEnemies.Count; i++)
			{
				AIActor aiactor2 = activeEnemies[i];
				if (aiactor2 && aiactor2.HasBeenEngaged && aiactor2.IsWorthShootingAt)
				{
					float num2 = Vector2.Distance(base.transform.position.XY(), aiactor2.specRigidbody.UnitCenter);
					if (aiactor == null || num2 < num)
					{
						aiactor = aiactor2;
						num = num2;
					}
				}
			}
			if (aiactor)
			{
				this.m_currentTarget = aiactor;
			}
		}
	}

	// Token: 0x0600769C RID: 30364 RVA: 0x002E4BB4 File Offset: 0x002E2DB4
	private Projectile GetProjectile()
	{
		Projectile overrideProjectile = this.shootProjectile;
		
		return overrideProjectile;
	}

	// Token: 0x0600769D RID: 30365 RVA: 0x002E4C28 File Offset: 0x002E2E28
	private Vector2 FindPredictedTargetPosition()
	{
		float num = this.GetProjectile().baseData.speed;
		if (num < 0f)
		{
			num = float.MaxValue;
		}
		Vector2 a = base.transform.position.XY();
		Vector2 vector = (this.m_currentTarget.specRigidbody.HitboxPixelCollider == null) ? this.m_currentTarget.specRigidbody.UnitCenter : this.m_currentTarget.specRigidbody.HitboxPixelCollider.UnitCenter;
		float d = Vector2.Distance(a, vector) / num;
		return vector + this.m_currentTarget.specRigidbody.Velocity * d;
	}

	// Token: 0x0600769E RID: 30366 RVA: 0x002E4CD4 File Offset: 0x002E2ED4
	private void Shoot(Vector2 targetPosition, Vector2 startOffset)
	{
		Vector2 vector = base.transform.position.XY() + startOffset;
		Vector2 vector2 = targetPosition - vector;
		float z = Mathf.Atan2(vector2.y, vector2.x) * 57.29578f;
		GameObject gameObject = this.GetProjectile().gameObject;
		GameObject gameObject2 = SpawnManager.SpawnProjectile(gameObject, vector, Quaternion.Euler(0f, 0f, z), true);
		Projectile component = gameObject2.GetComponent<Projectile>();
		component.collidesWithEnemies = true;
		component.collidesWithPlayer = false;
		component.Owner = this.m_owner;
		component.Shooter = this.m_owner.specRigidbody;
		component.TreatedAsNonProjectileForChallenge = true;
		if (this.m_owner)
		{
			if (PassiveItem.IsFlagSetForCharacter(this.m_owner, typeof(BattleStandardItem)))
			{
				component.baseData.damage *= BattleStandardItem.BattleStandardCompanionDamageMultiplier;
			}
			if (this.m_owner.CurrentGun && this.m_owner.CurrentGun.LuteCompanionBuffActive)
			{
				component.baseData.damage *= 2f;
				component.RuntimeUpdateScale(1.75f);
			}
			this.m_owner.DoPostProcessProjectile(component);
		}
	}

	// Token: 0x0600769F RID: 30367 RVA: 0x002E4E22 File Offset: 0x002E3022
	public void ToggleRenderer(bool value)
	{
		base.sprite.renderer.enabled = value;
		if (!this.PreventOutline)
		{
			SpriteOutlineManager.ToggleOutlineRenderers(base.sprite, value);
		}
	}

	// Token: 0x060076A0 RID: 30368 RVA: 0x002E4E4C File Offset: 0x002E304C
	private int GetNumberToFire()
	{
		int num = this.numToShoot;
		
		return num;
	}

	// Token: 0x060076A1 RID: 30369 RVA: 0x002E4ECC File Offset: 0x002E30CC
	private float GetModifiedCooldown()
	{
		float num = this.shootCooldown;
		if (this.m_owner && this.m_owner.CurrentGun && this.m_owner.CurrentGun.LuteCompanionBuffActive)
		{
			num /= 1.5f;
		}
		return num;
	}

	// Token: 0x060076A2 RID: 30370 RVA: 0x002E4F90 File Offset: 0x002E3190
	private void HandleCombat()
	{
		if (GameManager.Instance.IsPaused || !this.m_owner || this.m_owner.CurrentInputState != PlayerInputState.AllInput || this.m_owner.IsInputOverridden)
		{
			return;
		}
		if (this.shootProjectile == null)
		{
			return;
		}
		if (this.m_retargetTimer <= 0f)
		{
			this.m_currentTarget = null;
		}
		if (this.m_currentTarget == null || !this.m_currentTarget || this.m_currentTarget.healthHaver.IsDead)
		{
			this.AcquireTarget();
		}
		if (this.m_currentTarget == null || !this.m_currentTarget)
		{
			return;
		}
		if (this.m_shootTimer <= 0f)
		{
			this.m_shootTimer = this.GetModifiedCooldown();
			Vector2 a = this.FindPredictedTargetPosition();
			if (!this.m_owner.IsStealthed)
			{
				int numberToFire = this.GetNumberToFire();
				for (int i = 0; i < numberToFire; i++)
				{
					Vector2 vector = Vector2.zero;
					if (i > 0)
					{
						vector = UnityEngine.Random.insideUnitCircle.normalized;
					}
					this.Shoot(a + vector, vector);
				}
			}
		}
		if (this.shouldRotate)
		{
			float num = BraveMathCollege.Atan2Degrees(this.m_currentTarget.CenterPosition - base.transform.position.XY());
			base.transform.localRotation = Quaternion.Euler(0f, 0f, num - 90f);
		}
	}

	// Token: 0x060076A3 RID: 30371 RVA: 0x0028E054 File Offset: 0x0028C254
	public Transform GetTransform()
	{
		return base.transform;
	}

	// Token: 0x060076A4 RID: 30372 RVA: 0x002E512E File Offset: 0x002E332E
	public int GetOrbitalTier()
	{
		return this.m_orbitalTier;
	}

	// Token: 0x060076A5 RID: 30373 RVA: 0x002E5136 File Offset: 0x002E3336
	public void SetOrbitalTier(int tier)
	{
		this.m_orbitalTier = tier;
	}

	// Token: 0x060076A6 RID: 30374 RVA: 0x002E513F File Offset: 0x002E333F
	public int GetOrbitalTierIndex()
	{
		return this.m_orbitalTierIndex;
	}

	// Token: 0x060076A7 RID: 30375 RVA: 0x002E5147 File Offset: 0x002E3347
	public void SetOrbitalTierIndex(int tierIndex)
	{
		this.m_orbitalTierIndex = tierIndex;
	}

	// Token: 0x060076A8 RID: 30376 RVA: 0x002E5150 File Offset: 0x002E3350
	public float GetOrbitalRadius()
	{
		return this.orbitRadius;
	}

	// Token: 0x060076A9 RID: 30377 RVA: 0x002E5158 File Offset: 0x002E3358
	public float GetOrbitalRotationalSpeed()
	{
		return this.orbitDegreesPerSecond;
	}

	// Token: 0x04007862 RID: 30818
	public CursonGuonController.SpecialOrbitalIdentifier SpecialID;

	// Token: 0x04007863 RID: 30819
	public CursonGuonController.OrbitalMotionStyle motionStyle;

	// Token: 0x04007864 RID: 30820
	public Projectile shootProjectile;

	// Token: 0x04007865 RID: 30821
	public int numToShoot;

	// Token: 0x04007866 RID: 30822
	public float shootCooldown;

	// Token: 0x04007867 RID: 30823
	public float orbitRadius;

	// Token: 0x04007868 RID: 30824
	public float orbitDegreesPerSecond;

	// Token: 0x04007869 RID: 30825
	public bool shouldRotate;

	// Token: 0x0400786A RID: 30826
	public float perfectOrbitalFactor;

	// Token: 0x0400786B RID: 30827
	public bool DamagesEnemiesOnShot;

	// Token: 0x0400786C RID: 30828
	public float DamageToEnemiesOnShot;

	// Token: 0x0400786D RID: 30829
	public float DamageToEnemiesOnShotCooldown;

	// Token: 0x0400786E RID: 30830
	private float m_damageOnShotCooldown;

	// Token: 0x0400786F RID: 30831
	public bool TriggersMachoBraceOnShot;

	// Token: 0x04007870 RID: 30832
	public bool PreventOutline;

	// Token: 0x04007871 RID: 30833
	public string IdleAnimation;

	// Token: 0x04007872 RID: 30834

	// Token: 0x04007873 RID: 30835
	public bool ExplodesOnTriggerCollision;

	// Token: 0x04007874 RID: 30836
	public ExplosionData TriggerExplosionData;

	// Token: 0x04007875 RID: 30837
	private bool m_initialized;

	// Token: 0x04007876 RID: 30838
	private PlayerController m_owner;

	// Token: 0x04007877 RID: 30839
	private AIActor m_currentTarget;

	// Token: 0x04007878 RID: 30840
	private float m_currentAngle;

	// Token: 0x04007879 RID: 30841
	private float m_shootTimer;

	// Token: 0x0400787A RID: 30842
	private float m_retargetTimer;

	// Token: 0x0400787B RID: 30843
	private int m_orbitalTier;

	// Token: 0x0400787C RID: 30844
	private int m_orbitalTierIndex;

	// Token: 0x0400787D RID: 30845
	private Vector2 m_ownerCenterAverage;

	// Token: 0x0400787E RID: 30846
	private bool m_hasLuteBuff;

	// Token: 0x0400787F RID: 30847
	private GameObject m_luteOverheadVfx;

	// Token: 0x04007880 RID: 30848
	[NonSerialized]

	public bool AlternateTargeting;
	// Token: 0x04007881 RID: 30849
	private float m_lastExplosionTime;

	// Token: 0x04007882 RID: 30850
	public float SinWavelength;

	// Token: 0x04007883 RID: 30851
	public float SinAmplitude;
    private bool OverridePosition;
    private float m_targetAngle;

    // Token: 0x02001462 RID: 5218
    public enum SpecialOrbitalIdentifier
	{
		// Token: 0x04007885 RID: 30853
		NONE,
		// Token: 0x04007886 RID: 30854
		BABY_DRAGUN
	}

	// Token: 0x02001463 RID: 5219
	public enum OrbitalMotionStyle
	{
		// Token: 0x04007888 RID: 30856
		ORBIT_PLAYER_ALWAYS,
		// Token: 0x04007889 RID: 30857
		ORBIT_TARGET
	}


}

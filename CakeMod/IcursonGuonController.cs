using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200141F RID: 5151
public class CursonGuonOrbital : PlayerOrbitalItem
{
	// Token: 0x060074F0 RID: 29936 RVA: 0x002DA10C File Offset: 0x002D830C
	public CursonGuonOrbital()
	{
		this.SlowBulletsDuration = 15f;
		this.SlowBulletsMultiplier = 0.5f;
		this.HealChanceNormal = 0.2f;
		this.HealChanceCritical = 0.5f;
		this.GreenerSynergyMoneyGain = 20;
		this.GreenerChanceCritical = 0.7f;
		this.DodgeRollTimeMultiplier = 0.9f;
		this.DodgeRollDistanceMultiplier = 1.25f;
	}

	// Token: 0x060074F1 RID: 29937 RVA: 0x002DA174 File Offset: 0x002D8374
	public override void Pickup(PlayerController player)
	{
		player.OnReceivedDamage += this.OwnerTookDamage;
		player.OnHitByProjectile = (Action<Projectile, PlayerController>)Delegate.Combine(player.OnHitByProjectile, new Action<Projectile, PlayerController>(this.OwnerHitByProjectile));
		if (this.Identifier == CursonGuonOrbital.IounStoneIdentifier.CLEAR)
		{
			player.PostProcessProjectile += this.HandlePostProcessClearSynergy;
		}
		if (this.ModifiesDodgeRoll)
		{
			player.rollStats.rollDistanceMultiplier *= this.DodgeRollDistanceMultiplier;
			player.rollStats.rollTimeMultiplier *= this.DodgeRollTimeMultiplier;
		}
		base.Pickup(player);
	}

    internal float GetOrbitalRadius()
    {
        throw new NotImplementedException();
    }

    internal float GetOrbitalRotationalSpeed()
    {
        throw new NotImplementedException();
    }

    // Token: 0x060074F2 RID: 29938 RVA: 0x002DA218 File Offset: 0x002D8418
    private void HandlePostProcessClearSynergy(Projectile targetProjectile, float arg2)
	{
		if (this.m_owner && this.m_synergyUpgradeActive && this.m_owner.CurrentGoop)
		{
			if (this.m_owner.CurrentGoop.CanBeIgnited)
			{
				if (!targetProjectile.AppliesFire)
				{
					targetProjectile.AppliesFire = true;
					targetProjectile.FireApplyChance = 1f;
					targetProjectile.fireEffect = this.m_owner.CurrentGoop.fireEffect;
				}
			}
			else if (this.m_owner.CurrentGoop.CanBeFrozen && !targetProjectile.AppliesFreeze)
			{
				targetProjectile.AppliesFreeze = true;
				targetProjectile.FreezeApplyChance = 1f;
				targetProjectile.freezeEffect = this.DefaultFreezeEffect;
			}
		}
	}

	// Token: 0x060074F3 RID: 29939 RVA: 0x002DA2E0 File Offset: 0x002D84E0
	private void OwnerHitByProjectile(Projectile incomingProjectile, PlayerController arg2)
	{
		if (this.SynergyCharmsEnemiesOnDamage && this.m_synergyUpgradeActive && incomingProjectile && incomingProjectile.Owner && incomingProjectile.Owner is AIActor)
		{
			incomingProjectile.Owner.ApplyEffect(this.CharmEffect, 1f, null);
		}
	}

	// Token: 0x060074F4 RID: 29940 RVA: 0x002DA348 File Offset: 0x002D8548
	private void OwnerTookDamage(PlayerController obj)
	{
		if (this.SlowBulletsOnDamage)
		{
			if (!this.m_isSlowingBullets)
			{
				obj.StartCoroutine(this.HandleSlowBullets());
			}
			else
			{
				this.m_slowDurationRemaining = this.SlowBulletsDuration;
			}
		}
		if (this.ChanceToHealOnDamage)
		{
			if (obj.healthHaver.GetCurrentHealth() < 0.5f)
			{
				bool flag = obj.HasActiveBonusSynergy(CustomSynergyType.GUON_UPGRADE_GREEN, false);
				if (UnityEngine.Random.value < ((!flag) ? this.HealChanceCritical : this.GreenerChanceCritical))
				{
					Debug.Log("Green Guon Critical Heal");
					obj.healthHaver.ApplyHealing(0.5f);
					if (flag)
					{
						LootEngine.SpawnCurrency(obj.CenterPosition, this.GreenerSynergyMoneyGain, false);
					}
				}
			}
			else if (UnityEngine.Random.value < this.HealChanceNormal)
			{
				Debug.Log("Green Guon Normal Heal");
				obj.healthHaver.ApplyHealing(0.5f);
				if (obj.HasActiveBonusSynergy(CustomSynergyType.GUON_UPGRADE_GREEN, false))
				{
					LootEngine.SpawnCurrency(obj.CenterPosition, this.GreenerSynergyMoneyGain, false);
				}
			}
		}
	}

	// Token: 0x060074F5 RID: 29941 RVA: 0x002DA45C File Offset: 0x002D865C
	private IEnumerator HandleSlowBullets()
	{
		yield return new WaitForEndOfFrame();
		this.m_isSlowingBullets = true;
		float slowMultiplier = this.SlowBulletsMultiplier;
		Projectile.BaseEnemyBulletSpeedMultiplier *= slowMultiplier;
		this.m_slowDurationRemaining = this.SlowBulletsDuration;
		while (this.m_slowDurationRemaining > 0f)
		{
			yield return null;
			this.m_slowDurationRemaining -= BraveTime.DeltaTime;
			Projectile.BaseEnemyBulletSpeedMultiplier /= slowMultiplier;
			slowMultiplier = Mathf.Lerp(this.SlowBulletsMultiplier, 1f, 1f - this.m_slowDurationRemaining);
			Projectile.BaseEnemyBulletSpeedMultiplier *= slowMultiplier;
		}
		Projectile.BaseEnemyBulletSpeedMultiplier /= slowMultiplier;
		this.m_isSlowingBullets = false;
		yield break;
	}

	// Token: 0x060074F6 RID: 29942 RVA: 0x002DA478 File Offset: 0x002D8678
	public override DebrisObject Drop(PlayerController player)
	{
		player.OnHitByProjectile = (Action<Projectile, PlayerController>)Delegate.Remove(player.OnHitByProjectile, new Action<Projectile, PlayerController>(this.OwnerHitByProjectile));
		player.OnReceivedDamage -= this.OwnerTookDamage;
		player.PostProcessProjectile -= this.HandlePostProcessClearSynergy;
		if (this.ModifiesDodgeRoll)
		{
			player.rollStats.rollDistanceMultiplier /= this.DodgeRollDistanceMultiplier;
			player.rollStats.rollTimeMultiplier /= this.DodgeRollTimeMultiplier;
		}
		return base.Drop(player);
	}

	// Token: 0x060074F7 RID: 29943 RVA: 0x002DA510 File Offset: 0x002D8710
	protected override void OnDestroy()
	{
		if (this.m_owner != null)
		{
			PlayerController owner = this.m_owner;
			owner.OnHitByProjectile = (Action<Projectile, PlayerController>)Delegate.Remove(owner.OnHitByProjectile, new Action<Projectile, PlayerController>(this.OwnerHitByProjectile));
			this.m_owner.OnReceivedDamage -= this.OwnerTookDamage;
			this.m_owner.PostProcessProjectile -= this.HandlePostProcessClearSynergy;
			if (this.ModifiesDodgeRoll)
			{
				this.m_owner.rollStats.rollDistanceMultiplier /= this.DodgeRollDistanceMultiplier;
				this.m_owner.rollStats.rollTimeMultiplier /= this.DodgeRollTimeMultiplier;
			}
		}
		base.OnDestroy();
	}

	// Token: 0x040076BE RID: 30398
	public bool SlowBulletsOnDamage;

	// Token: 0x040076BF RID: 30399
	public float SlowBulletsDuration;

	// Token: 0x040076C0 RID: 30400
	public float SlowBulletsMultiplier;

	// Token: 0x040076C1 RID: 30401
	public bool ChanceToHealOnDamage;

	// Token: 0x040076C2 RID: 30402
	public float HealChanceNormal;

	// Token: 0x040076C3 RID: 30403
	public float HealChanceCritical;

	// Token: 0x040076C4 RID: 30404
	public int GreenerSynergyMoneyGain;

	// Token: 0x040076C5 RID: 30405
	public float GreenerChanceCritical;

	// Token: 0x040076C6 RID: 30406
	public bool ModifiesDodgeRoll;

	// Token: 0x040076C7 RID: 30407
	public float DodgeRollTimeMultiplier;

	// Token: 0x040076C8 RID: 30408
	public float DodgeRollDistanceMultiplier;

	// Token: 0x040076C9 RID: 30409
	public bool SynergyCharmsEnemiesOnDamage;

	// Token: 0x040076CA RID: 30410
	public GameActorCharmEffect CharmEffect;

	// Token: 0x040076CB RID: 30411
	public GameActorFreezeEffect DefaultFreezeEffect;

	// Token: 0x040076CC RID: 30412
	public CursonGuonOrbital.IounStoneIdentifier Identifier;

	// Token: 0x040076CD RID: 30413
	private bool m_isSlowingBullets;

	// Token: 0x040076CE RID: 30414
	private float m_slowDurationRemaining;

	// Token: 0x02001420 RID: 5152
	public enum IounStoneIdentifier
	{
		// Token: 0x040076D0 RID: 30416
		GENERIC,
		// Token: 0x040076D1 RID: 30417
		CLEAR
	}
}

using System;
using UnityEngine;

// Token: 0x02001466 RID: 5222
public class CursorGuonItem : PassiveItem
{
	// Token: 0x060076C0 RID: 30400 RVA: 0x002E6318 File Offset: 0x002E4518
	public static GameObject CreateOrbital(PlayerController owner, GameObject targetCursorPrefab, bool isFollower, CursorGuonItem sourceItem = null)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(targetCursorPrefab, owner.transform.position, Quaternion.identity);
		if (!isFollower)
		{
			CursonGuonController component = gameObject.GetComponent<CursonGuonController>();
			component.Initialize(owner);
		}
		else
		{
			PlayerOrbitalFollower component2 = gameObject.GetComponent<PlayerOrbitalFollower>();
			if (component2)
			{
				component2.Initialize(owner);
			}
		}
		return gameObject;
	}

	// Token: 0x060076C1 RID: 30401 RVA: 0x002E6378 File Offset: 0x002E4578
	private void CreateOrbital(PlayerController owner)
	{
		GameObject targetCursorPrefab = (!(this.CursorPrefab != null)) ? this.OrbitalFollowerPrefab.gameObject : this.CursorPrefab.gameObject;
		if (this.HasUpgradeSynergy && this.m_synergyUpgradeActive)
		{
			targetCursorPrefab = ((!(this.UpgradeCursorPrefab != null)) ? this.UpgradeOrbitalFollowerPrefab.gameObject : this.UpgradeCursorPrefab.gameObject);
		}
		this.m_extantOrbital = CursorGuonItem.CreateOrbital(owner, targetCursorPrefab, this.OrbitalFollowerPrefab != null, this);
		if (this.BreaksUponContact && this.m_extantOrbital)
		{
			SpeculativeRigidbody component = this.m_extantOrbital.GetComponent<SpeculativeRigidbody>();
			if (component)
			{
				SpeculativeRigidbody speculativeRigidbody = component;
				speculativeRigidbody.OnRigidbodyCollision = (SpeculativeRigidbody.OnRigidbodyCollisionDelegate)Delegate.Combine(speculativeRigidbody.OnRigidbodyCollision, new SpeculativeRigidbody.OnRigidbodyCollisionDelegate(this.HandleBreakOnCollision));
			}
		}
		if (this.BreaksUponOwnerDamage && owner)
		{
			owner.OnReceivedDamage += this.HandleBreakOnOwnerDamage;
		}
	}

	// Token: 0x060076C2 RID: 30402 RVA: 0x002E648C File Offset: 0x002E468C
	private void HandleBreakOnOwnerDamage(PlayerController arg1)
	{
		if (!this)
		{
			return;
		}
		if (this.BreakVFX && this.m_extantOrbital && this.m_extantOrbital.GetComponentInChildren<tk2dSprite>())
		{
			SpawnManager.SpawnVFX(this.BreakVFX, this.m_extantOrbital.GetComponentInChildren<tk2dSprite>().WorldCenter.ToVector3ZisY(0f), Quaternion.identity);
		}
		if (this.m_owner)
		{
			this.m_owner.RemovePassiveItem(this.PickupObjectId);
			this.m_owner.OnReceivedDamage -= this.HandleBreakOnOwnerDamage;
		}
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x060076C3 RID: 30403 RVA: 0x002E6548 File Offset: 0x002E4748
	private void HandleBreakOnCollision(CollisionData rigidbodyCollision)
	{
		if (this.m_owner)
		{
			this.m_owner.RemovePassiveItem(this.PickupObjectId);
		}
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x060076C4 RID: 30404 RVA: 0x002E6576 File Offset: 0x002E4776
	public void DecoupleOrbital()
	{
		this.m_extantOrbital = null;
		if (this.BreaksUponOwnerDamage && this.m_owner)
		{
			this.m_owner.OnReceivedDamage -= this.HandleBreakOnOwnerDamage;
		}
	}

	// Token: 0x060076C5 RID: 30405 RVA: 0x002E65B4 File Offset: 0x002E47B4
	private void DestroyOrbital()
	{
		if (!this.m_extantOrbital)
		{
			return;
		}
		if (this.BreaksUponOwnerDamage && this.m_owner)
		{
			this.m_owner.OnReceivedDamage -= this.HandleBreakOnOwnerDamage;
		}
		UnityEngine.Object.Destroy(this.m_extantOrbital.gameObject);
		this.m_extantOrbital = null;
	}

	// Token: 0x060076C6 RID: 30406 RVA: 0x002E661C File Offset: 0x002E481C
	protected override void Update()
	{
		base.Update();
		if (this.HasUpgradeSynergy)
		{
			if (this.m_synergyUpgradeActive && (!this.m_owner || !this.m_owner.HasActiveBonusSynergy(this.UpgradeSynergy, false)))
			{
				if (this.m_owner)
				{
					for (int i = 0; i < this.synergyModifiers.Length; i++)
					{
						this.m_owner.healthHaver.damageTypeModifiers.Remove(this.synergyModifiers[i]);
					}
				}
				this.m_synergyUpgradeActive = false;
				this.DestroyOrbital();
				if (this.m_owner)
				{
					this.CreateOrbital(this.m_owner);
				}
			}
			else if (!this.m_synergyUpgradeActive && this.m_owner && this.m_owner.HasActiveBonusSynergy(this.UpgradeSynergy, false))
			{
				this.m_synergyUpgradeActive = true;
				this.DestroyOrbital();
				if (this.m_owner)
				{
					this.CreateOrbital(this.m_owner);
				}
				for (int j = 0; j < this.synergyModifiers.Length; j++)
				{
					this.m_owner.healthHaver.damageTypeModifiers.Add(this.synergyModifiers[j]);
				}
			}
		}
	}

	// Token: 0x060076C7 RID: 30407 RVA: 0x002E6774 File Offset: 0x002E4974
	public override void Pickup(PlayerController player)
	{
		base.Pickup(player);
		player.OnNewFloorLoaded = (Action<PlayerController>)Delegate.Combine(player.OnNewFloorLoaded, new Action<PlayerController>(this.HandleNewFloor));
		for (int i = 0; i < this.modifiers.Length; i++)
		{
			player.healthHaver.damageTypeModifiers.Add(this.modifiers[i]);
		}
		this.CreateOrbital(player);
	}

	// Token: 0x060076C8 RID: 30408 RVA: 0x002E67E2 File Offset: 0x002E49E2
	private void HandleNewFloor(PlayerController obj)
	{
		this.DestroyOrbital();
		this.CreateOrbital(obj);
	}

	// Token: 0x060076C9 RID: 30409 RVA: 0x002E67F4 File Offset: 0x002E49F4
	public override DebrisObject Drop(PlayerController player)
	{
		this.DestroyOrbital();
		player.OnNewFloorLoaded = (Action<PlayerController>)Delegate.Remove(player.OnNewFloorLoaded, new Action<PlayerController>(this.HandleNewFloor));
		for (int i = 0; i < this.modifiers.Length; i++)
		{
			player.healthHaver.damageTypeModifiers.Remove(this.modifiers[i]);
		}
		for (int j = 0; j < this.synergyModifiers.Length; j++)
		{
			player.healthHaver.damageTypeModifiers.Remove(this.synergyModifiers[j]);
		}
		return base.Drop(player);
	}

	// Token: 0x060076CA RID: 30410 RVA: 0x002E6894 File Offset: 0x002E4A94
	protected override void OnDestroy()
	{
		if (this.m_owner != null)
		{
			PlayerController owner = this.m_owner;
			owner.OnNewFloorLoaded = (Action<PlayerController>)Delegate.Remove(owner.OnNewFloorLoaded, new Action<PlayerController>(this.HandleNewFloor));
			for (int i = 0; i < this.modifiers.Length; i++)
			{
				this.m_owner.healthHaver.damageTypeModifiers.Remove(this.modifiers[i]);
			}
			for (int j = 0; j < this.synergyModifiers.Length; j++)
			{
				this.m_owner.healthHaver.damageTypeModifiers.Remove(this.synergyModifiers[j]);
			}
			this.m_owner.OnReceivedDamage -= this.HandleBreakOnOwnerDamage;
		}
		this.DestroyOrbital();
		base.OnDestroy();
	}

	// Token: 0x040078B2 RID: 30898
	public CursonGuonOrbital CursorPrefab;

	// Token: 0x040078B3 RID: 30899
	public PlayerOrbitalFollower OrbitalFollowerPrefab;

	// Token: 0x040078B4 RID: 30900
	public bool HasUpgradeSynergy;

	// Token: 0x040078B5 RID: 30901
	[LongNumericEnum]
	public CustomSynergyType UpgradeSynergy;

	// Token: 0x040078B6 RID: 30902
	public GameObject UpgradeCursorPrefab;

	// Token: 0x040078B7 RID: 30903
	public GameObject UpgradeOrbitalFollowerPrefab;

	// Token: 0x040078B8 RID: 30904
	public bool CanBeMimicked;

	// Token: 0x040078B9 RID: 30905
	[Header("Random Stuff, probably for Ioun Stones")]
	public DamageTypeModifier[] modifiers;

	// Token: 0x040078BA RID: 30906
	public DamageTypeModifier[] synergyModifiers;

	// Token: 0x040078BB RID: 30907
	public bool BreaksUponContact;

	// Token: 0x040078BC RID: 30908
	public bool BreaksUponOwnerDamage;

	// Token: 0x040078BD RID: 30909
	public GameObject BreakVFX;

	// Token: 0x040078BE RID: 30910
	protected GameObject m_extantOrbital;

	// Token: 0x040078BF RID: 30911
	protected bool m_synergyUpgradeActive;
}

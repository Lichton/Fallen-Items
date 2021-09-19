using System;
using System.Collections;
using System.Collections.Generic;
using ItemAPI;
using UnityEngine;

namespace CakeMod
{
	// Token: 0x02000018 RID: 24
	internal class BladeBullets : GunVolleyModificationItem
	{
		// Token: 0x0600008B RID: 139 RVA: 0x00005B8C File Offset: 0x00003D8C
		public static void Init()
		{
			string name = "Blade Bullets";
			string resourcePath = "CakeMod/Resources/BladeBullets";
			GameObject gameObject = new GameObject(name);
			BladeBullets catSnack = gameObject.AddComponent<BladeBullets>();
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject);
			string shortDesc = "Sharp Wit";
			string longDesc = "Quips may cut the mind, but these bullets cut the flesh.";
			catSnack.SetupItem(shortDesc, longDesc, "cak");
			catSnack.quality = PickupObject.ItemQuality.EXCLUDED;
			List<string> mandatoryConsoleIDs = new List<string>
			{
				"cak:bladebullets",
				"excaliber"
			};
		}

		// Token: 0x0600008C RID: 140 RVA: 0x00005C38 File Offset: 0x00003E38
		private void PostProcessProjectile(Projectile projectile, float Chance)
		{
			PlayerController owner = base.Owner;
			bool flag = owner.HasGun(377);
			float statValue = owner.stats.GetStatValue(PlayerStats.StatType.Coolness);
			Chance = 0.8f - statValue / 100f;
			bool flag2 = flag;
			if (flag2)
			{
				Chance -= 0.05f;
			}
			bool flag3 = UnityEngine.Random.value > Chance;
			bool flag4 = flag3 && !BladeBullets.CoolAsIce;
			if (flag4)
			{
				BladeBullets.CoolAsIce = true;
				base.StartCoroutine(this.StartCooldown());
				float num = 0.97f - statValue / 50f;
				bool flag5 = UnityEngine.Random.value > num;
				bool flag6 = flag5 && !BladeBullets.SharkMFs;
				if (flag6)
				{
					BladeBullets.SharkMFs = true;
					base.StartCoroutine(this.SharkCooldown());
					Projectile projectile2 = ((Gun)ETGMod.Databases.Items[377]).DefaultModule.chargeProjectiles[0].Projectile;
					GameObject gameObject = SpawnManager.SpawnProjectile(projectile2.gameObject, base.Owner.sprite.WorldCenter, Quaternion.Euler(0f, 0f, (base.Owner.CurrentGun == null) ? 0f : base.Owner.CurrentGun.CurrentAngle), true);
					Projectile component = gameObject.GetComponent<Projectile>();
					bool flag7 = component != null;
					bool flag8 = flag7;
					if (flag8)
					{
						component.Owner = base.Owner;
						component.Shooter = base.Owner.specRigidbody;
						component.baseData.speed = 25f;
						component.baseData.damage = 105f;
					}
				}
				else
				{
					Projectile projectile3 = ((Gun)ETGMod.Databases.Items[377]).DefaultModule.projectiles[0];
					GameObject gameObject2 = SpawnManager.SpawnProjectile(projectile3.gameObject, base.Owner.sprite.WorldCenter, Quaternion.Euler(0f, 0f, (base.Owner.CurrentGun == null) ? 0f : base.Owner.CurrentGun.CurrentAngle), true);
					Projectile component2 = gameObject2.GetComponent<Projectile>();
					bool flag9 = component2 != null;
					bool flag10 = flag9;
					if (flag10)
					{
						component2.Owner = base.Owner;
						component2.Shooter = base.Owner.specRigidbody;
						component2.baseData.speed = 20f;
						bool flag11 = flag;
						if (flag11)
						{
							component2.baseData.damage = 20f;
						}
						else
						{
							component2.baseData.damage = 10f;
						}
					}
				}
			}
		}

		// Token: 0x0600008D RID: 141 RVA: 0x00005EEC File Offset: 0x000040EC
		private void PostProcessBeamChanceTick(BeamController beamController)
		{
			PlayerController owner = base.Owner;
			bool flag = owner.HasGun(377);
			float statValue = owner.stats.GetStatValue(PlayerStats.StatType.Coolness);
			float num = 0.94f - statValue / 100f;
			bool flag2 = flag;
			if (flag2)
			{
				num -= 0.05f;
			}
			bool flag3 = UnityEngine.Random.value > num;
			bool flag4 = flag3 && !BladeBullets.CoolAsIce;
			if (flag4)
			{
				BladeBullets.CoolAsIce = true;
				base.StartCoroutine(this.StartCooldown());
				float num2 = 0.97f - statValue / 50f;
				bool flag5 = UnityEngine.Random.value > num2;
				bool flag6 = flag5 && !BladeBullets.SharkMFs;
				if (flag6)
				{
					BladeBullets.SharkMFs = true;
					base.StartCoroutine(this.SharkCooldown());
					Projectile projectile = ((Gun)ETGMod.Databases.Items[377]).DefaultModule.chargeProjectiles[0].Projectile;
					GameObject gameObject = SpawnManager.SpawnProjectile(projectile.gameObject, base.Owner.sprite.WorldCenter, Quaternion.Euler(0f, 0f, (base.Owner.CurrentGun == null) ? 0f : base.Owner.CurrentGun.CurrentAngle), true);
					Projectile component = gameObject.GetComponent<Projectile>();
					bool flag7 = component != null;
					bool flag8 = flag7;
					if (flag8)
					{
						component.Owner = base.Owner;
						component.Shooter = base.Owner.specRigidbody;
						component.baseData.speed = 25f;
						component.baseData.damage = 105f;
					}
				}
				else
				{
					Projectile projectile2 = ((Gun)ETGMod.Databases.Items[377]).DefaultModule.projectiles[0];
					GameObject gameObject2 = SpawnManager.SpawnProjectile(projectile2.gameObject, base.Owner.sprite.WorldCenter, Quaternion.Euler(0f, 0f, (base.Owner.CurrentGun == null) ? 0f : base.Owner.CurrentGun.CurrentAngle), true);
					Projectile component2 = gameObject2.GetComponent<Projectile>();
					bool flag9 = component2 != null;
					bool flag10 = flag9;
					if (flag10)
					{
						component2.Owner = base.Owner;
						component2.Shooter = base.Owner.specRigidbody;
						component2.baseData.speed = 20f;
						bool flag11 = flag;
						if (flag11)
						{
							component2.baseData.damage = 20f;
						}
						else
						{
							component2.baseData.damage = 10f;
						}
					}
				}
			}
		}

		// Token: 0x0600008E RID: 142 RVA: 0x0000619F File Offset: 0x0000439F
		private IEnumerator StartCooldown()
		{
			yield return new WaitForSeconds(0.5f);
			BladeBullets.CoolAsIce = false;
			yield break;
		}

		// Token: 0x0600008F RID: 143 RVA: 0x000061AE File Offset: 0x000043AE
		private IEnumerator SharkCooldown()
		{
			yield return new WaitForSeconds(5f);
			BladeBullets.SharkMFs = false;
			yield break;
		}

		// Token: 0x06000090 RID: 144 RVA: 0x000061BD File Offset: 0x000043BD
		public override void Pickup(PlayerController player)
		{
			base.Pickup(player);
			player.PostProcessProjectile += this.PostProcessProjectile;
			player.PostProcessBeamChanceTick += this.PostProcessBeamChanceTick;
		}

		// Token: 0x06000091 RID: 145 RVA: 0x000061F0 File Offset: 0x000043F0
		public override DebrisObject Drop(PlayerController player)
		{
			player.PostProcessProjectile -= this.PostProcessProjectile;
			player.PostProcessBeamChanceTick -= this.PostProcessBeamChanceTick;
			this.m_pickedUp = false;
			this.m_pickedUpThisRun = true;
			this.HasBeenStatProcessed = true;
			this.m_owner = null;
			DebrisObject debrisObject = LootEngine.DropItemWithoutInstantiating(base.gameObject, player.LockedApproximateSpriteCenter, player.unadjustedAimPoint - player.LockedApproximateSpriteCenter, 4f, true, false, false, false);
			SpriteOutlineManager.AddOutlineToSprite(debrisObject.sprite, Color.black, 0.1f, 0f, SpriteOutlineManager.OutlineType.NORMAL);
			base.RegisterMinimapIcon();
			return debrisObject;
		}

		// Token: 0x0400001B RID: 27
		protected new PlayerController m_owner;

		// Token: 0x0400001C RID: 28
		private static bool CoolAsIce = false;

		// Token: 0x0400001D RID: 29
		private static bool SharkMFs = false;
	}
}

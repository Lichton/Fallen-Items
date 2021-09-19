using System;
using ItemAPI;
using UnityEngine;
using Dungeonator;
using System.Collections;
using Gungeon;
using MonoMod;
using BasicGun;
using System.Collections.Generic;
using SaveAPI;
using GungeonAPI;

namespace CakeMod
{
	// Token: 0x02000040 RID: 64
	internal class Cookie : PassiveItem
	{
		// Token: 0x0600017E RID: 382 RVA: 0x0000C990 File Offset: 0x0000AB90
		public static void Init()
		{
			string name = "Odd Cookie";
			string resourcePath = "CakeMod/Resources/Cookie.png";
			GameObject gameObject = new GameObject(name);
			Cookie wyrmBlood = gameObject.AddComponent<Cookie>();
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject);
			string shortDesc = "Tasty Rainbow";
			string longDesc = "A strange cookie that, when consumed, taints Gungeoneers with the Curse of the Rainbow.";
			wyrmBlood.SetupItem(shortDesc, longDesc, "cak");
			wyrmBlood.quality = PickupObject.ItemQuality.EXCLUDED;
			wyrmBlood.SetupUnlockOnFlag(GungeonFlags.BOWLER_ACTIVE_IN_FOYER, true);
		}

		// Token: 0x0600017F RID: 383 RVA: 0x0000CA00 File Offset: 0x0000AC00

		
		private void PostProcessBeam(BeamController sourceBeam)
		{
			try
			{
				Projectile projectile = sourceBeam.projectile;
				
			}
			catch (Exception ex)
			{
				ETGModConsole.Log(ex.Message, false);
			}
		}
		// Token: 0x06000180 RID: 384 RVA: 0x0000CA60 File Offset: 0x0000AC60
		private void PostProcessProjectile(Projectile sourceProjectile, float effectChanceScalar)
		{
			bighead = UnityEngine.Random.Range(1, 20);
			if (bighead == 1)
			{
				sourceProjectile.HasDefaultTint = true;
				sourceProjectile.DefaultTintColor = ExtendedColours.red;
				
			}
			if (bighead == 2)
			{
				sourceProjectile.HasDefaultTint = true;
				sourceProjectile.DefaultTintColor = ExtendedColours.orange;
				
			}
			if (bighead == 3)
			{
				sourceProjectile.HasDefaultTint = true;
				sourceProjectile.DefaultTintColor = Color.yellow;
				
			}
			if (bighead == 4)
			{
				sourceProjectile.HasDefaultTint = true;
				sourceProjectile.DefaultTintColor = Color.blue;
				
			}
			if (bighead == 5)
			{
				sourceProjectile.HasDefaultTint = true;
				sourceProjectile.DefaultTintColor = Color.green;
				
			}
			if (bighead == 6)
			{
				sourceProjectile.HasDefaultTint = true;
				sourceProjectile.DefaultTintColor = ExtendedColours.purple;
				
				
			}
			if (bighead == 7)
			{
				sourceProjectile.HasDefaultTint = true;
				sourceProjectile.DefaultTintColor = ExtendedColours.pink;
				
			}
			if (bighead == 8)
			{
				sourceProjectile.HasDefaultTint = true;
				sourceProjectile.DefaultTintColor = Color.grey;
				
			}
			sourceProjectile.OnHitEnemy += this.OnHitEnemy;

		}

		// Token: 0x06000181 RID: 385 RVA: 0x0000CAB8 File Offset: 0x0000ACB8
		private void OnHitEnemy(Projectile arg1, SpeculativeRigidbody arg2, bool arg3)
		{
		if(bighead == 6)
            {
				base.StartCoroutine(this.HandleFear(base.Owner, arg2));
			}
			if (bighead == 5)
			{		
                GameActorHealthEffect irradiatedLeadEffect = PickupObjectDatabase.GetById(204).GetComponent<BulletStatusEffectItem>().HealthModifierEffect;
				arg2.aiActor.ApplyEffect(irradiatedLeadEffect, 2f, arg1);
			}
			if (bighead == 2)
			{
				GameActorFireEffect hotLeadEffect = PickupObjectDatabase.GetById(295).GetComponent<BulletStatusEffectItem>().FireModifierEffect;
				arg2.aiActor.ApplyEffect(hotLeadEffect, 2f, arg1);
			}
			if (bighead == 1)
			{
				arg2.aiActor.healthHaver.ApplyDamage(1f * base.Owner.stats.GetStatValue(PlayerStats.StatType.Damage), Vector2.zero, "Erasure", CoreDamageTypes.None, DamageCategory.Normal, true, null, false);
				GlobalSparksDoer.DoRadialParticleBurst(50, arg2.specRigidbody.HitboxPixelCollider.UnitCenter, arg2.specRigidbody.HitboxPixelCollider.UnitCenter, 90f, 2f, 0f, null, null, Color.red, GlobalSparksDoer.SparksType.BLOODY_BLOOD);
			}
			if (bighead == 3)
			{
				arg2.aiActor.ApplyEffect(this.cheeseEffect, 3f, null);
			}
			if (bighead == 7)
			{
			GameActorCharmEffect charmingRoundsEffect = PickupObjectDatabase.GetById(527).GetComponent<BulletStatusEffectItem>().CharmModifierEffect;
		    arg2.aiActor.ApplyEffect(charmingRoundsEffect, 3f, null);
			}
			if (bighead == 4)
			{
				GameActorFreezeEffect frostBulletsEffect = PickupObjectDatabase.GetById(278).GetComponent<BulletStatusEffectItem>().FreezeModifierEffect;
				arg2.aiActor.ApplyEffect(frostBulletsEffect, 3f, null);
			}
			if (bighead == 8)
			{
				GameActorSpeedEffect tripleCrossbowSlowEffect = (PickupObjectDatabase.GetById(381) as Gun).DefaultModule.projectiles[0].speedEffect;
				arg2.aiActor.ApplyEffect(tripleCrossbowSlowEffect, 3f, null);
			}

		}

		private IEnumerator HandleFear(PlayerController user, SpeculativeRigidbody enemy)
		{
			bool flag = this.fleeData == null || this.fleeData.Player != base.Owner;
			if (flag)
			{
				this.fleeData = new FleePlayerData();
				this.fleeData.Player = base.Owner;
				this.fleeData.StartDistance *= 2f;
			}
			bool flag2 = enemy.aiActor.behaviorSpeculator != null;
			if (flag2)
			{
				enemy.aiActor.behaviorSpeculator.FleePlayerData = this.fleeData;
				FleePlayerData fleePlayerData = new FleePlayerData();
				yield return new WaitForSeconds(1.5f);
				enemy.aiActor.behaviorSpeculator.FleePlayerData.Player = null;
			}
			yield break;
		}

		// Token: 0x06000182 RID: 386 RVA: 0x0000CB0A File Offset: 0x0000AD0A
		public override void Pickup(PlayerController player)
		{
			base.Pickup(player);
			player.PostProcessProjectile += this.PostProcessProjectile;
			player.PostProcessBeam += this.PostProcessBeam;
		}

		// Token: 0x06000183 RID: 387 RVA: 0x0000CB3C File Offset: 0x0000AD3C
		public override DebrisObject Drop(PlayerController player)
		{
			DebrisObject debrisObject = base.Drop(player);
			try
			{
				player.PostProcessBeam -= this.PostProcessBeam;
				player.PostProcessProjectile -= this.PostProcessProjectile;
			}
			catch (Exception arg)
			{
				ETGModConsole.Log(string.Format("damn,\n {0}", arg), false);
			}
			debrisObject.GetComponent<Cookie>().m_pickedUpThisRun = true;
			return debrisObject;
		}

		// Token: 0x0400006D RID: 109
		public AIActorDebuffEffect EnemyDebuff = new AIActorDebuffEffect
		{
			HealthMultiplier = 0.5f,
			CooldownMultiplier = 1f,
			SpeedMultiplier = 2f,
			duration = 10f

		};

		public AIActorDebuffEffect EnemyDebuff2 = new AIActorDebuffEffect
		{
			HealthMultiplier = 0.5f,
			CooldownMultiplier = 1f,
			SpeedMultiplier = 1f,
			duration = 10f

		};

		// Token: 0x0400006E RID: 110
		private Shader m_glintShader;

		public static GameActorFireEffect hotLeadEffect = PickupObjectDatabase.GetById(295).GetComponent<BulletStatusEffectItem>().FireModifierEffect;
		public static GameActorFireEffect greenFireEffect = PickupObjectDatabase.GetById(706).GetComponent<Gun>().DefaultModule.projectiles[0].fireEffect;


		//Freezes
		public static GameActorFreezeEffect frostBulletsEffect = PickupObjectDatabase.GetById(278).GetComponent<BulletStatusEffectItem>().FreezeModifierEffect;
		public static GameActorFreezeEffect chaosBulletsFreeze = PickupObjectDatabase.GetById(569).GetComponent<ChaosBulletsItem>().FreezeModifierEffect;

		//Poisons
		public static GameActorHealthEffect irradiatedLeadEffect = PickupObjectDatabase.GetById(204).GetComponent<BulletStatusEffectItem>().HealthModifierEffect;

		//Charms
		public static GameActorCharmEffect charmingRoundsEffect = PickupObjectDatabase.GetById(527).GetComponent<BulletStatusEffectItem>().CharmModifierEffect;

		//Cheeses

		//Speed Changes
		public static GameActorSpeedEffect tripleCrossbowSlowEffect = (PickupObjectDatabase.GetById(381) as Gun).DefaultModule.projectiles[0].speedEffect;
		private FleePlayerData fleeData;

		public GameActorCheeseEffect cheeseEffect = (PickupObjectDatabase.GetById(626) as Gun).DefaultModule.projectiles[0].cheeseEffect;
		public GameActorBleedEffect bleedEffect = (PickupObjectDatabase.GetById(542) as Gun).DefaultModule.projectiles[0].bleedEffect;
		public int bighead = UnityEngine.Random.Range(1, 9);
	}
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;

namespace CakeMod
{
	// Token: 0x02000078 RID: 120
	internal class DumbBullets : BulletStatusEffectItem
	{
		// Token: 0x060002BB RID: 699 RVA: 0x00016278 File Offset: 0x00014478
		public static void Init()
		{
			string name = "Dumb Bullets";
			string resourcePath = "CakeMod/Resources/DumbBullets";
			GameObject gameObject = new GameObject();
			DumbBullets DumbBullets = gameObject.AddComponent<DumbBullets>();
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject);
			string shortDesc = "Bulletvis And Gunhead";
			string longDesc = "With great power comes great stupidity. Very rarely will these bullets use their power, and if so, only on accident.";
			DumbBullets.SetupItem(shortDesc, longDesc, "cak");
			DumbBullets.AppliesFire = true;
			DumbBullets.quality = PickupObject.ItemQuality.EXCLUDED;
		}

		// Token: 0x060002BC RID: 700 RVA: 0x00016370 File Offset: 0x00014570
		public override void Pickup(PlayerController player)
		{
			bool pickedUp = this.m_pickedUp;
			if (!pickedUp)
			{
				base.Pickup(player);
				player.PostProcessProjectile += this.PostProcessProjectile;
				player.PostProcessBeam += this.PostProcessBeam;
				player.PostProcessBeamTick += this.PostProcessBeamTick;

				GameActorFireEffect fireModifierEffect = new GameActorFireEffect
				{
					IsGreenFire = false,
					AffectsEnemies = true,
					DamagePerSecondToEnemies = 50f
				};
				this.chanceOfActivating = 0.01f;
				this.chanceFromBeamPerSecond = 0.08f;
			    this.TintColor = new Color(0f, 0f, 0f);
				this.TintPriority = 5;
				this.FreezeAmountPerDamage = 1f;
				this.TintBeams = true;
				this.FireModifierEffect = fireModifierEffect;
				this.quality = PickupObject.ItemQuality.S;
				this.sprite.IsPerpendicular = true;
			}
		}

		// Token: 0x060002BD RID: 701 RVA: 0x000163CC File Offset: 0x000145CC
		public void PostProcessBeam(BeamController beam)
		{
			bool tintBeams = this.TintBeams;
			if (tintBeams)
			{
				beam.AdjustPlayerBeamTint(this.TintColor.WithAlpha(this.TintColor.a / 2f), this.TintPriority, 0f);
			}
		}

		// Token: 0x060002BE RID: 702 RVA: 0x00016414 File Offset: 0x00014614
		private void PostProcessProjectile(Projectile projectile, float effectChanceScalar)
		{
			bool flag = UnityEngine.Random.value < this.chanceOfActivating * effectChanceScalar;
			if (flag)
			{
				bool appliesFire = this.AppliesFire;
				if (appliesFire)
				{
					projectile.statusEffectsToApply.Add(this.FireModifierEffect);
					projectile.AdjustPlayerProjectileTint(this.TintColor.WithAlpha(this.TintColor.a / 2f), this.TintPriority, 0f);
				}
			}
		}

		// Token: 0x060002BF RID: 703 RVA: 0x00016484 File Offset: 0x00014684
		public void PostProcessBeamTick(BeamController beam, SpeculativeRigidbody hitRigidbody, float tickRate)
		{
			GameActor gameActor = hitRigidbody.gameActor;
			bool flag = !gameActor;
			if (!flag)
			{
				bool flag2 = UnityEngine.Random.value < BraveMathCollege.SliceProbability(this.chanceFromBeamPerSecond, tickRate);
				if (flag2)
				{
					bool appliesFire = this.AppliesFire;
					if (appliesFire)
					{
						gameActor.ApplyEffect(this.FireModifierEffect, 1f, null);
					}
				}
			}
		}

		// Token: 0x060002C0 RID: 704 RVA: 0x000164E4 File Offset: 0x000146E4
		public override DebrisObject Drop(PlayerController player)
		{
			DebrisObject debrisObject = base.Drop(player);
			debrisObject.GetComponent<DumbBullets>().m_pickedUpThisRun = true;
			player.PostProcessProjectile -= this.PostProcessProjectile;
			player.PostProcessBeam -= this.PostProcessBeam;
			player.PostProcessBeamTick -= this.PostProcessBeamTick;
			return debrisObject;
		}
	}
}

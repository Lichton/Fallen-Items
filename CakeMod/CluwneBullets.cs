using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;

namespace CakeMod
{
	// Token: 0x02000078 RID: 120
	internal class CluwneBullets : BulletStatusEffectItem
	{
		// Token: 0x060002BB RID: 699 RVA: 0x00016278 File Offset: 0x00014478
		public static void Init()
		{
			string name = "Cluwne Bullets";
			string resourcePath = "CakeMod/Resources/CluwneBullets";
			GameObject gameObject = new GameObject();
			CluwneBullets cluwneBullets = gameObject.AddComponent<CluwneBullets>();
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject);
			string shortDesc = "Clown's Ugly Brother";
			string longDesc = "A disgusting creation of the warped magics of Gungeon sorcerers.";
			cluwneBullets.SetupItem(shortDesc, longDesc, "cak");
			cluwneBullets.quality = PickupObject.ItemQuality.EXCLUDED;
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

				this.AppliesCharm = true;
				GameActorCharmEffect charmModifierEffect = new GameActorCharmEffect
				{
					AffectsEnemies = true,
				};
				this.chanceOfActivating = 0.2f;
				this.chanceFromBeamPerSecond = 0.08f;
				this.TintColor = new Color(0f, 0f, 0f);
				this.TintPriority = 6;
				this.FreezeAmountPerDamage = 1f;
				this.TintBeams = true;
				this.CharmModifierEffect = charmModifierEffect;

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
private void PostProcessProjectile(Projectile sourceProjectile, float effectChanceScalar)
		{
			bool flag = UnityEngine.Random.Range(0, 1) < this.chanceOfActivating * effectChanceScalar;
			if (flag)
			{
				bool appliesCharm = this.AppliesCharm;
				if (appliesCharm)
				{
					BulletStatusEffectItem charmBullet = PickupObjectDatabase.GetById(527).GetComponent<BulletStatusEffectItem>();
					sourceProjectile.statusEffectsToApply.Add(charmBullet.CharmModifierEffect);
					sourceProjectile.AdjustPlayerProjectileTint(this.TintColor.WithAlpha(this.TintColor.a / 2f), this.TintPriority, 0f);
				}

				float num = 0.2f;
				float num2 = num * effectChanceScalar;
				bool flag3 = UnityEngine.Random.Range(0, 1) > num2;
				if (flag)
				{
					sourceProjectile.OnHitEnemy += this.AddFearEffect;
				}
			}
		}

		private void AddFearEffect(Projectile arg1, SpeculativeRigidbody arg2, bool arg3)
		{
			bool flag = arg2 != null && arg2.aiActor != null && base.Owner != null;
			if (flag)
			{
				bool flag2 = arg2 != null && arg2.healthHaver.IsAlive;
				if (flag2)
				{
					bool flag3 = arg2.aiActor.EnemyGuid != "465da2bb086a4a88a803f79fe3a27677" && arg2.aiActor.EnemyGuid != "05b8afe0b6cc4fffa9dc6036fa24c8ec";
					if (flag3)
					{
						base.StartCoroutine(this.HandleFear(base.Owner, arg2));
					}
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
					bool appliesCharm = this.AppliesCharm;
					if (appliesCharm)
					{
						gameActor.ApplyEffect(this.CharmModifierEffect, 1f, null);
					}
				}
			}
		}

		// Token: 0x060002C0 RID: 704 RVA: 0x000164E4 File Offset: 0x000146E4
		public override DebrisObject Drop(PlayerController player)
		{
			DebrisObject debrisObject = base.Drop(player);
			debrisObject.GetComponent<CluwneBullets>().m_pickedUpThisRun = true;
			player.PostProcessProjectile -= this.PostProcessProjectile;
			player.PostProcessBeam -= this.PostProcessBeam;
			player.PostProcessBeamTick -= this.PostProcessBeamTick;
			return debrisObject;
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
				yield return new WaitForSeconds(7f);
				enemy.aiActor.behaviorSpeculator.FleePlayerData.Player = null;
			}
			yield break;
		}

		private FleePlayerData fleeData;
	}
}
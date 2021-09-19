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

namespace CakeMod
{
	// Token: 0x02000040 RID: 64
	internal class FunnyHat : PassiveItem
	{
		// Token: 0x0600017E RID: 382 RVA: 0x0000C990 File Offset: 0x0000AB90
		public static void Init()
		{
			string name = "Rainbowllets";
			string resourcePath = "CakeMod/Resources/Rainbowllets.png";
			GameObject gameObject = new GameObject(name);
			FunnyHat wyrmBlood = gameObject.AddComponent<FunnyHat>();
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject);
			string shortDesc = "Prismatic";
			string longDesc = "Imposes chaotic energies onto enemies hit.";
			wyrmBlood.SetupItem(shortDesc, longDesc, "cak");
			wyrmBlood.quality = PickupObject.ItemQuality.S;
			CakeIDs.Rainbowllets = wyrmBlood.PickupObjectId;
			wyrmBlood.SetupUnlockOnCustomFlag(CustomDungeonFlags.EXAMPLE_BLUEPRINTMETA_2, true);
		}

		// Token: 0x0600017F RID: 383 RVA: 0x0000CA00 File Offset: 0x0000AC00
		private void PostProcessBeam(BeamController sourceBeam)
		{
			try
			{
					sourceBeam.AdjustPlayerBeamTint(Color.HSVToRGB(UnityEngine.Random.value, 1.0f, 1.0f), 1);
				Projectile projectile = sourceBeam.projectile;
				
				projectile.OnHitEnemy = (Action<Projectile, SpeculativeRigidbody, bool>)Delegate.Combine(projectile.OnHitEnemy, new Action<Projectile, SpeculativeRigidbody, bool>(this.OnHitEnemy));
			}
			catch (Exception ex)
			{
				ETGModConsole.Log(ex.Message, false);
			}
		}

		//make beams and shit change colour in the air you dumbfuck!

      

        // Token: 0x06000180 RID: 384 RVA: 0x0000CA60 File Offset: 0x0000AC60

        private void PostProcessProjectile(Projectile sourceProjectile, float effectChanceScalar)
		{
			try
			{
				sourceProjectile.OnHitEnemy += this.OnHitEnemy;
			}
			catch (Exception ex)
			{
				ETGModConsole.Log(ex.Message, false);
			}
			Color randomColor = Color.HSVToRGB(UnityEngine.Random.value, 1.0f, 1.0f);
			sourceProjectile.HasDefaultTint = true;
			sourceProjectile.DefaultTintColor = randomColor;
		}

		// Token: 0x06000181 RID: 385 RVA: 0x0000CAB8 File Offset: 0x0000ACB8
		private void OnHitEnemy(Projectile arg1, SpeculativeRigidbody arg2, bool arg3)
		{
			if (Owner.PlayerHasActiveSynergy("Painbows & Gunshine"))

			{
				bool flag4 = !arg2.healthHaver.IsDead && !arg2.healthHaver.IsBoss;
				if (flag4)
				{
					bool flag = arg2 != null && arg2.aiActor != null && base.Owner != null;
					if (flag)
					{
						arg2.aiActor.ApplyEffect(this.EnemyDebuff2, 1f, null);
						ProcessGunShader(arg2.aiActor);
					}
				}
			}
			else
			{
				bool flag4 = !arg2.healthHaver.IsDead && !arg2.healthHaver.IsBoss;
				if (flag4)
				{
					bool flag = arg2 != null && arg2.aiActor != null && base.Owner != null;
					if (flag)
					{
						arg2.aiActor.ApplyEffect(this.EnemyDebuff, 1f, null);
						ProcessGunShader(arg2.aiActor);
					}
				}
			}

		}




		private void ProcessGunShader(AIActor g)
		{
			MeshRenderer component = g.GetComponent<MeshRenderer>();
			if (!component)
			{
				return;
			}
			this.m_glintShader = Shader.Find("Brave/Internal/RainbowChestShader");
			Material[] sharedMaterials = component.sharedMaterials;
			for (int i = 0; i < sharedMaterials.Length; i++)
			{
				if (sharedMaterials[i].shader == this.m_glintShader)
				{
					return;
				}
			}
			Array.Resize<Material>(ref sharedMaterials, sharedMaterials.Length + 1);
			Material material = new Material(this.m_glintShader);
			material.SetFloat("_AllColorsToggle", 1f);
			material.SetTexture("_MainTex", sharedMaterials[0].GetTexture("_MainTex"));
			sharedMaterials[sharedMaterials.Length - 1] = material;
			component.sharedMaterials = sharedMaterials;
			sprite.renderer.material.shader = ShaderCache.Acquire("Brave/Internal/RainbowChestShader");
			sprite.usesOverrideMaterial = true;
			MeshRenderer component2 = g.CorpseObject.GetComponent<MeshRenderer>();
			if (!component2)
			{
				return;
			}
			for (int i = 0; i < sharedMaterials.Length; i++)
			{
				if (sharedMaterials[i].shader == this.m_glintShader)
				{
					return;
				}
			}
			Array.Resize<Material>(ref sharedMaterials, sharedMaterials.Length + 1);
			
			material.SetFloat("_AllColorsToggle", 1f);
			material.SetTexture("_MainTex", sharedMaterials[0].GetTexture("_MainTex"));
			sharedMaterials[sharedMaterials.Length - 1] = material;
			component2.sharedMaterials = sharedMaterials;
			sprite.renderer.material.shader = ShaderCache.Acquire("Brave/Internal/RainbowChestShader");
			sprite.usesOverrideMaterial = true;
			return;
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
			debrisObject.GetComponent<FunnyHat>().m_pickedUpThisRun = true;
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
		public int i = 0;
	}
}
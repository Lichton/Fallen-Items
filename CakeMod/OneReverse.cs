using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;
using System.Collections;

namespace CakeMod
{
	// Token: 0x020000BC RID: 188
	internal class OneReverse : PlayerItem
	{
		// Token: 0x060004F1 RID: 1265 RVA: 0x0002A4DC File Offset: 0x000286DC
		public static void Init()
		{
			string name = "One Reverse";
			string resourcePath = "CakeMod/Resources/OneReverse";
			GameObject gameObject = new GameObject(name);
			OneReverse oneReverse = gameObject.AddComponent<OneReverse>();
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject);
			string shortDesc = "No U";
			string longDesc = "Stops enemy bullets in the tracks.";
			oneReverse.SetupItem(shortDesc, longDesc, "cak");
			oneReverse.SetCooldownType(ItemBuilder.CooldownType.Damage, 500f);
			oneReverse.consumable = false;
			oneReverse.quality = PickupObject.ItemQuality.EXCLUDED;
			ItemBuilder.AddPassiveStatModifier(oneReverse, PlayerStats.StatType.Coolness, 2f);
		}

		// Token: 0x060004F2 RID: 1266 RVA: 0x0002A553 File Offset: 0x00028753
		protected override void DoEffect(PlayerController user)
		{
			AkSoundEngine.PostEvent("Play_OBJ_power_up_01", base.gameObject);
			this.StartEffect(user);
			base.StartCoroutine(ItemBuilder.HandleDuration(this, this.duration, user, new Action<PlayerController>(this.EndEffect)));
		}

		// Token: 0x060004F3 RID: 1267 RVA: 0x0002A590 File Offset: 0x00028790
		private void EnableVFX(PlayerController user)
		{
			Material outlineMaterial = SpriteOutlineManager.GetOutlineMaterial(user.sprite);
			outlineMaterial.SetColor("_OverrideColor", new Color(225f, 225f, 225f));
		}

		// Token: 0x060004F4 RID: 1268 RVA: 0x0002A5CC File Offset: 0x000287CC
		private void DisableVFX(PlayerController user)
		{
			Material outlineMaterial = SpriteOutlineManager.GetOutlineMaterial(user.sprite);
			outlineMaterial.SetColor("_OverrideColor", new Color(0, 0f, 0f));
		}

		// Token: 0x060004F5 RID: 1269 RVA: 0x0002A608 File Offset: 0x00028808
		private void StartEffect(PlayerController user)
		{
			float baseStatValue = user.stats.GetBaseStatValue(PlayerStats.StatType.EnemyProjectileSpeedMultiplier);
			user.stats.SetBaseStatValue(PlayerStats.StatType.EnemyProjectileSpeedMultiplier, 0.01f, user);
			this.movementBuff = true;
			this.EnableVFX(user);
			this.activeOutline = false;
		}

		// Token: 0x060004F6 RID: 1270 RVA: 0x0002A654 File Offset: 0x00028854
		private void EndEffect(PlayerController user)
		{

			if (this.movementBuff)
			{
				float baseStatValue = user.stats.GetBaseStatValue(PlayerStats.StatType.EnemyProjectileSpeedMultiplier);
				user.stats.SetBaseStatValue(PlayerStats.StatType.EnemyProjectileSpeedMultiplier, 1f, user);
				this.movementBuff = false;
				this.DisableVFX(user);
				this.activeOutline = false;
			}
		}

		// Token: 0x060004F7 RID: 1271 RVA: 0x0002A6B8 File Offset: 0x000288B8
		private void PlayerTookDamage(float resultValue, float maxValue, CoreDamageTypes damageTypes, DamageCategory damageCategory, Vector2 damageDirection)
		{
			bool flag = this.activeOutline;
			if (flag)
			{
				GameManager.Instance.StartCoroutine(this.GainOutline());
			}
			else
			{
				bool flag2 = !this.activeOutline;
				if (flag2)
				{
					GameManager.Instance.StartCoroutine(this.LoseOutline());
				}
			}
		}

		// Token: 0x060004F8 RID: 1272 RVA: 0x0002A705 File Offset: 0x00028905
		private IEnumerator GainOutline()
		{
			PlayerController user = this.LastOwner;
			yield return new WaitForSeconds(0.05f);
			this.EnableVFX(user);
			yield break;
		}

		// Token: 0x060004F9 RID: 1273 RVA: 0x0002A714 File Offset: 0x00028914
		private IEnumerator LoseOutline()
		{
			PlayerController user = this.LastOwner;
			yield return new WaitForSeconds(0.05f);
			this.DisableVFX(user);
			yield break;
		}

		// Token: 0x060004FA RID: 1274 RVA: 0x0002A723 File Offset: 0x00028923
		public override void Pickup(PlayerController player)
		{
			player.healthHaver.OnDamaged += this.PlayerTookDamage;
			base.Pickup(player);
			this.CanBeDropped = true;
		}

		// Token: 0x060004FB RID: 1275 RVA: 0x0002A750 File Offset: 0x00028950
		protected override void OnPreDrop(PlayerController user)
		{
			bool isCurrentlyActive = base.IsCurrentlyActive;
			if (isCurrentlyActive)
			{
				base.IsCurrentlyActive = true;
				this.EndEffect(user);
			}
		}

		// Token: 0x060004FC RID: 1276 RVA: 0x0002A77C File Offset: 0x0002897C
		public DebrisObject Drop(PlayerController player)
		{
			DebrisObject result = base.Drop(player, 4f);
			player.healthHaver.OnDamaged -= this.PlayerTookDamage;
			base.IsCurrentlyActive = true;
			this.EndEffect(player);
			return result;
		}

		// Token: 0x060004FD RID: 1277 RVA: 0x0002A7C4 File Offset: 0x000289C4
		protected override void OnDestroy()
		{
			this.LastOwner.healthHaver.OnDamaged -= this.PlayerTookDamage;
			base.IsCurrentlyActive = true;
			this.EndEffect(this.LastOwner);
			base.OnDestroy();
		}

		// Token: 0x060004FE RID: 1278 RVA: 0x0002A800 File Offset: 0x00028A00
		public override bool CanBeUsed(PlayerController user)
		{
			return base.CanBeUsed(user);
		}

		// Token: 0x04000145 RID: 325
		private bool movementBuff;

		// Token: 0x04000146 RID: 326
		private float duration = 5f;

		// Token: 0x04000147 RID: 327
		private bool activeOutline = false;
	}
}

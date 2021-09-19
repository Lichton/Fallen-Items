using System;
using System.Collections;
using ItemAPI;
using UnityEngine;

namespace CakeMod
{
	// Token: 0x020000BC RID: 188
	internal class StarNinja : PlayerItem
	{
		// Token: 0x060004F1 RID: 1265 RVA: 0x0002A4DC File Offset: 0x000286DC
		public static void Init()
		{
			string name = "Star Ninja";
			string resourcePath = "CakeMod/Resources/StarNinja";
			GameObject gameObject = new GameObject(name);
			StarNinja ninjaStuff = gameObject.AddComponent<StarNinja>();
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject);
			string shortDesc = "Stealth 100";
			string longDesc = "This star aspiried to become a ninja, but was always a joke due to it's ironic nature.";
			ninjaStuff.SetupItem(shortDesc, longDesc, "cak");
			ninjaStuff.SetCooldownType(ItemBuilder.CooldownType.Damage, 250f);
			ninjaStuff.IgnoredByRat = true;
			ninjaStuff.consumable = false;
			ninjaStuff.canStack = true;
			ninjaStuff.numberOfUses = 3;
			ninjaStuff.m_cachedNumberOfUses = 3;
			ninjaStuff.UsesNumberOfUsesBeforeCooldown = true;
			ninjaStuff.quality = PickupObject.ItemQuality.EXCLUDED;
		}

		// Token: 0x060004F2 RID: 1266 RVA: 0x0002A553 File Offset: 0x00028753
		protected override void DoEffect(PlayerController user)
		{
			AkSoundEngine.PostEvent("Play_OBJ_power_up_01", base.gameObject);
			PlayableCharacters characterIdentity = user.characterIdentity;
			this.StealthEffect();
			base.StartCoroutine(ItemBuilder.HandleDuration(this, 1f, user, new Action<PlayerController>(this.BreakStealth)));

		}

		private void OnDamaged(float resultValue, float maxValue, CoreDamageTypes damageTypes, DamageCategory damageCategory, Vector2 damageDirection)
		{
			this.BreakStealth(this.LastOwner);
		}

		// Token: 0x0600062D RID: 1581 RVA: 0x000373D4 File Offset: 0x000355D4
		private void BreakStealthOnSteal(PlayerController arg1, ShopItemController arg2)
		{
			this.BreakStealth(arg1);
		}

		// Token: 0x0600062E RID: 1582 RVA: 0x000373E0 File Offset: 0x000355E0
		private void BreakStealth(PlayerController player)
		{
			player.ChangeSpecialShaderFlag(1, 0f);
			player.OnItemStolen -= this.BreakStealthOnSteal;
			player.SetIsStealthed(false, "shade");
			player.healthHaver.OnDamaged -= this.OnDamaged;
			player.SetCapableOfStealing(false, "shade", null);
			player.OnDidUnstealthyAction -= this.BreakStealth;
			AkSoundEngine.PostEvent("Play_OBJ_power_up_01", base.gameObject);
		}
		private IEnumerator Unstealthy()
		{
			yield return new WaitForSeconds(0.15f);
			this.LastOwner.OnDidUnstealthyAction += this.BreakStealth;
			yield break;
		}
		private void StealthEffect()
		{
			PlayerController lastOwner = this.LastOwner;
			this.BreakStealth(lastOwner);
			lastOwner.OnItemStolen += this.BreakStealthOnSteal;
			lastOwner.ChangeSpecialShaderFlag(1, 1f);
			lastOwner.healthHaver.OnDamaged += this.OnDamaged;
			lastOwner.SetIsStealthed(true, "shade");
			lastOwner.SetCapableOfStealing(true, "shade", null);
			GameManager.Instance.StartCoroutine(this.Unstealthy());
		}
		// Token: 0x060004F5 RID: 1269 RVA: 0x0002A608 File Offset: 0x0002880



		// Token: 0x060004F6 RID: 1270 RVA: 0x0002A654 File Offset: 0x00028854


		// Token: 0x060004F7 RID: 1271 RVA: 0x0002A6B8 File Offset: 0x000288B8

		// Token: 0x060004F8 RID: 1272 RVA: 0x0002A705 File Offset: 0x00028905

		// Token: 0x060004F9 RID: 1273 RVA: 0x0002A714 File Offset: 0x00028914


		// Token: 0x060004FA RID: 1274 RVA: 0x0002A723 File Offset: 0x00028923
		public override void Pickup(PlayerController player)
		{
			base.Pickup(player);
		}

		// Token: 0x060004FB RID: 1275 RVA: 0x0002A750 File Offset: 0x00028950
		protected override void OnPreDrop(PlayerController user)
		{
			bool isCurrentlyActive = base.IsCurrentlyActive;
			if (isCurrentlyActive)
			{
				base.IsCurrentlyActive = true;
			}
		}

		// Token: 0x060004FC RID: 1276 RVA: 0x0002A77C File Offset: 0x0002897C
		public DebrisObject Drop(PlayerController player)
		{
			DebrisObject result = base.Drop(player, 4f);
			base.IsCurrentlyActive = true;
			return result;
		}

		// Token: 0x060004FD RID: 1277 RVA: 0x0002A7C4 File Offset: 0x000289C4
		protected override void OnDestroy()
		{
			base.IsCurrentlyActive = true;
			base.OnDestroy();
		}

		// Token: 0x060004FE RID: 1278 RVA: 0x0002A800 File Offset: 0x00028A00
		public override bool CanBeUsed(PlayerController user)
		{
			return base.CanBeUsed(user);
		}

		// Token: 0x04000145 RID: 32

		// Token: 0x04000146 RID: 326
		// Token: 0x04000147 RID: 327
	}
}
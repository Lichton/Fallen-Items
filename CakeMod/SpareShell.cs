using System.Collections;
using ItemAPI;
using UnityEngine;
using Gungeon;
using MonoMod;
using BasicGun;
using System.Collections.Generic;

namespace CakeMod
{
	// Token: 0x020000BC RID: 188
	internal class SpareShell : PlayerItem
	{
		// Token: 0x060004F1 RID: 1265 RVA: 0x0002A4DC File Offset: 0x000286DC
		public static void Init()
		{
			string name = "Spare Shell";
			string resourcePath = "CakeMod/Resources/SpareShell";
			GameObject gameObject = new GameObject(name);
			SpareShell item = gameObject.AddComponent<SpareShell>();
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject);
			string shortDesc = "Shell A Ton";
			string longDesc = "A shell of a rare species of Gundead, the Turtle Kin. It is quite hardy.";
			item.SetupItem(shortDesc, longDesc, "cak");
			item.SetCooldownType(ItemBuilder.CooldownType.Damage, 600f);
			item.consumable = false;
			item.quality = PickupObject.ItemQuality.D;
			CakeIDs.TurtItem1 = item.PickupObjectId;
		}

		// Token: 0x060004F2 RID: 1266 RVA: 0x0002A553 File Offset: 0x00028753
		protected override void DoEffect(PlayerController user)
		{
			this.daboomer();
			AkSoundEngine.PostEvent("m_WPN_brickgun_shot_01", base.gameObject);
			this.StartEffect(user);
		}

		private void OnDamaged(float resultValue, float maxValue, CoreDamageTypes damageTypes, DamageCategory damageCategory, Vector2 damageDirection)
		{

		}

		private void daboomer()
		{



			PlayerController lastOwner = this.LastOwner;
			Projectile projectile = ((Gun)ETGMod.Databases.Items[376]).DefaultModule.projectiles[0];
			GameObject gameObject = SpawnManager.SpawnProjectile(projectile.gameObject, lastOwner.sprite.WorldBottomCenter, Quaternion.Euler(0f, 0f, (lastOwner.CurrentGun == null) ? 0f : lastOwner.CurrentGun.CurrentAngle), true);
			Projectile component = gameObject.GetComponent<Projectile>();
			bool flag = component != null;
			if (flag)
			{
				component.Owner = lastOwner;
				component.Shooter = lastOwner.specRigidbody;
				component.baseData.damage *= 1f;
				projectile.AdditionalScaleMultiplier = 1f;
				projectile.collidesWithPlayer = false;

				lastOwner.DoPostProcessProjectile(component);
			}
		
		}

		// Token: 0x0600062D RID: 1581 RVA: 0x000373D4 File Offset: 0x000355D4


		// Token: 0x0600062E RID: 1582 RVA: 0x000373E0 File Offset: 0x000355E0

		// Token: 0x060004F5 RID: 1269 RVA: 0x0002A608 File Offset: 0x00028808
		private void StartEffect(PlayerController user)
		{

		}



		// Token: 0x060004F6 RID: 1270 RVA: 0x0002A654 File Offset: 0x00028854
		private void EndEffect(PlayerController user)
		{

		}

		// Token: 0x060004F7 RID: 1271 RVA: 0x0002A6B8 File Offset: 0x000288B8
		private void PlayerTookDamage(float resultValue, float maxValue, CoreDamageTypes damageTypes, DamageCategory damageCategory, Vector2 damageDirection)
		{

		}

		// Token: 0x060004F8 RID: 1272 RVA: 0x0002A705 File Offset: 0x00028905

		// Token: 0x060004F9 RID: 1273 RVA: 0x0002A714 File Offset: 0x00028914


		// Token: 0x060004FA RID: 1274 RVA: 0x0002A723 File Offset: 0x00028923
		public override void Pickup(PlayerController player)
		{
			if(player.HasActiveItem(CakeIDs.TurtItem1) && player.HasPassiveItem(CakeIDs.TurtItem2) && player.HasPassiveItem(CakeIDs.TurtItem3) && player.HasPassiveItem(CakeIDs.TurtItem4))
            {
				TransformTo(player, "PlayerRogue");
			}
			base.Pickup(player);
			this.CanBeDropped = true;
		}

		private void TransformTo(PlayerController player, string playerName)
		{
			PlayerController controller = ((GameObject)BraveResources.Load(playerName)).GetComponent<PlayerController>();
			Transform playerSprite = controller.transform.Find("PlayerSprite");
			tk2dBaseSprite sprite = playerSprite?.GetComponent<tk2dSprite>();

			var library = Instantiate(sprite.GetComponent<tk2dSpriteAnimator>().Library);
			DontDestroyOnLoad(library);

			var handSkin = controller.primaryHand.sprite.Collection;

			TurtSprites = ItemsMod.list;
			tk2dSpriteCollectionData turts = ItemsMod.HandleAnimations2(sprite, TurtSprites);
			if (turts != null)
			{
				handSkin = turts;
			}
			foreach (var clip in library.clips)
			{

				for (int i = 0; i < clip.frames.Length; i++)
				{
					clip.frames[i].spriteCollection = turts;
				}
			}

			player.OverrideAnimationLibrary = library;
			player.ChangeHandsToCustomType(handSkin, controller.primaryHand.sprite.spriteId);
		}
		// Token: 0x060004FB RID: 1275 RVA: 0x0002A750 File Offset: 0x00028950
		protected override void OnPreDrop(PlayerController user)
		{

		}

		// Token: 0x060004FC RID: 1276 RVA: 0x0002A77C File Offset: 0x0002897C
		public DebrisObject Drop(PlayerController player)
		{
			DebrisObject result = base.Drop(player, 4f);

			return result;
		}

		// Token: 0x060004FD RID: 1277 RVA: 0x0002A7C4 File Offset: 0x000289C4
		protected override void OnDestroy()
		{
			base.OnDestroy();
		}

		// Token: 0x060004FE RID: 1278 RVA: 0x0002A800 File Offset: 0x00028A00
		public override bool CanBeUsed(PlayerController user)
		{
			return base.CanBeUsed(user);
		}
		public List<Texture2D> TurtSprites;
		// Token: 0x04000145 RID: 325

		// Token: 0x04000147 RID: 327
	}
}
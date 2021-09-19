using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;
using Dungeonator;
using System.Reflection;
using Random = System.Random;
using FullSerializer;
using System.Collections;
using Gungeon;
using MonoMod.RuntimeDetour;
using MonoMod;
using System.Collections.ObjectModel;
using UnityEngine.Serialization;
using SaveAPI;

namespace CakeMod
{
	public class CasingBullets : PassiveItem
	{
		public static void Init()
		{
			string itemName = "Casing Bullets";
			string resourceName = "CakeMod/Resources/CasingBullets";
			GameObject obj = new GameObject(itemName);
			var item = obj.AddComponent<CasingBullets>();
			ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
			string shortDesc = "Meta Coin";
			string longDesc = "A prototype of the well known casing currency of the gungeon." +
				"\n\nEnemies hit drop extra casings if inflicted.";
			ItemBuilder.SetupItem(item, shortDesc, longDesc, "cak");
			item.quality = PickupObject.ItemQuality.EXCLUDED;
		}
		private void PostProcessProjectile(Projectile sourceProjectile, float effectChanceScalar)
		{
				sourceProjectile.AppliesPoison = true;
				sourceProjectile.PoisonApplyChance = 0.2f;
				sourceProjectile.healthEffect = HelpfulLibrary.Money;
		}
		public override void Pickup(PlayerController player)
		{
			base.Pickup(player);
			player.PostProcessProjectile += this.PostProcessProjectile;
		}

		protected override void OnDestroy()
		{
			base.Owner.PostProcessProjectile -= this.PostProcessProjectile;
			base.OnDestroy();
		}
	}
}
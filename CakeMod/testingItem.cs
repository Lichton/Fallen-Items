using System;
using ItemAPI;
using UnityEngine;
using Dungeonator;
using System.Collections;
using Gungeon;
using MonoMod;
using BasicGun;
using System.Collections.Generic;

namespace CakeMod
{
	// Token: 0x02000040 RID: 64
	internal class testingitem : PassiveItem
	{
		// Token: 0x0600017E RID: 382 RVA: 0x0000C990 File Offset: 0x0000AB90
		public static void Init()
		{
			string name = "testingitem";
			string resourcePath = "CakeMod/Resources/Rainbowllets.png";
			GameObject gameObject = new GameObject(name);
			testingitem wyrmBlood = gameObject.AddComponent<testingitem>();
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject);
			string shortDesc = "Prismatic";
			string longDesc = "Imposes chaotic energies onto enemies hit.";
			wyrmBlood.SetupItem(shortDesc, longDesc, "cak");
			wyrmBlood.quality = PickupObject.ItemQuality.EXCLUDED;
		}

		private void PostProcessProjectile(Projectile sourceProjectile, float effectChanceScalar)
		{
			Color randomColor = Color.HSVToRGB(UnityEngine.Random.value, 1.0f, 1.0f);
			sourceProjectile.HasDefaultTint = true;
			sourceProjectile.DefaultTintColor = randomColor;
		}


		public override void Pickup(PlayerController player)
		{
			base.Pickup(player);
			player.PostProcessProjectile += this.PostProcessProjectile;
		}

		// Token: 0x06000183 RID: 387 RVA: 0x0000CB3C File Offset: 0x0000AD3C
		public override DebrisObject Drop(PlayerController player)
		{
			DebrisObject debrisObject = base.Drop(player);
			debrisObject.GetComponent<testingitem>().m_pickedUpThisRun = true;
			return debrisObject;
		}
	}
}

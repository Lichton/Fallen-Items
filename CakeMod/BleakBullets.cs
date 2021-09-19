using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;

namespace CakeMod
{

	public class BleakBullets : BulletStatusEffectItem
	{

		public static void Init()
		{
			string name = "Bleak Bullets";
			string resourcePath = "CakeMod/Resources/BleakBullets";
			GameObject gameObject = new GameObject(name);
			BleakBullets bleakBullets = gameObject.AddComponent<BleakBullets>();
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject);
			string shortDesc = "Agunized";
			string longDesc = "Infused with depressive hormones at extremely low temperatures, this bullet makes any that come in contact with it question their very existence and freezes them on the spot.\n\n" +
				"The process is simple, really: The victim's torrent of tears are frozen instantly by the coldness of the bullets, making it up to you to end their misery.";
			bleakBullets.SetupItem(shortDesc, longDesc, "cak");

			bleakBullets.quality = PickupObject.ItemQuality.EXCLUDED;

			bleakBullets.AppliesFreeze = true;

			bleakBullets.FreezeModifierEffect = (PickupObjectDatabase.GetById(278) as BulletStatusEffectItem).FreezeModifierEffect;

			bleakBullets.AppliesSpeedModifier = true;

			bleakBullets.SpeedModifierEffect = (PickupObjectDatabase.GetById(381) as Gun).DefaultModule.projectiles[0].speedEffect;

		}


		public override void Pickup(PlayerController player)
		{
			base.Pickup(player);
		}

		public override DebrisObject Drop(PlayerController player)
		{
			return base.Drop(player);
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
		}
	}
}

using System;
using ItemAPI;
using UnityEngine;

namespace CakeMod
{
	// Token: 0x02000062 RID: 98
	internal class ArmouredKey : PassiveItem
	{
		// Token: 0x06000250 RID: 592 RVA: 0x000135DE File Offset: 0x000117DE


		// Token: 0x06000251 RID: 593 RVA: 0x000135F4 File Offset: 0x000117F4
		public static void Init()
		{
			string name = "Armoured Key";
			string resourcePath = "CakeMod/Resources/ArmouredKey";
			GameObject gameObject = new GameObject(name);
			ArmouredKey petRock = gameObject.AddComponent<ArmouredKey>();
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject);
			string shortDesc = "Chink In The Armour";
			string longDesc = "Keys are fueled by armor. If you have no keys on you, an armour piece is siphoned and turned into a key.";
			petRock.SetupItem(shortDesc, longDesc, "cak");
			petRock.quality = PickupObject.ItemQuality.D;
			petRock.sprite.IsPerpendicular = true;

		}

		// Token: 0x06000252 RID: 594 RVA: 0x00013674 File Offset: 0x00011874
		protected override void Update()
		{
			bool flag = Owner.carriedConsumables.KeyBullets < 1;
			if (flag)
			{
				if (Owner.healthHaver.Armor >= 1)
				{
					AkSoundEngine.PostEvent("Play_OBJ_lock_unlock_01", base.gameObject);
					Owner.healthHaver.Armor = Owner.healthHaver.Armor - 1;
					Owner.carriedConsumables.KeyBullets = Owner.carriedConsumables.KeyBullets + 1;
				}
			}
			base.Update();
		}
		// Token: 0x06000253 RID: 595 RVA: 0x00013784 File Offset: 0x00011984
		public override void Pickup(PlayerController player)
		{
			base.Pickup(player);
		}

		// Token: 0x06000254 RID: 596 RVA: 0x000137BC File Offset: 0x000119BC
		public override DebrisObject Drop(PlayerController player)
		{
			DebrisObject debrisObject = base.Drop(player);
			return debrisObject;
		}

		// Token: 0x06000255 RID: 597 RVA: 0x000137F8 File Offset: 0x000119F8
	}
}
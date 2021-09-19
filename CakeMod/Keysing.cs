using System;
using ItemAPI;
using UnityEngine;

namespace CakeMod
{
	// Token: 0x02000062 RID: 98
	internal class Keysing : PlayerItem
	{
		// Token: 0x06000250 RID: 592 RVA: 0x000135DE File Offset: 0x000117DE


		// Token: 0x06000251 RID: 593 RVA: 0x000135F4 File Offset: 0x000117F4
	public static void Init()
	{
		string name = "Keysing";
		string resourcePath = "CakeMod/Resources/Keysing";
		GameObject gameObject = new GameObject(name);
		Keysing petRock = gameObject.AddComponent<Keysing>();
		ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject);
		string shortDesc = "The Key To Success";
		string longDesc = "This casing has a strange siphon at the bottom of it, absorbing your credits and turning them into keys. Only works while having 50 casings or more.";
		petRock.SetupItem(shortDesc, longDesc, "cak");
		petRock.quality = PickupObject.ItemQuality.D;
		petRock.sprite.IsPerpendicular = true;
			ItemBuilder.SetCooldownType(petRock, ItemBuilder.CooldownType.Timed, 5f);
			petRock.PlaceItemInAmmonomiconAfterItemById(166);
		}

		// Token: 0x06000252 RID: 594 RVA: 0x00013674 File Offset: 0x00011874


		protected override void DoEffect(PlayerController user)
		{
			bool flag = user.carriedConsumables.Currency > 49;
			if (flag)
			{
				user.carriedConsumables.Currency = user.carriedConsumables.Currency - 50;
				user.carriedConsumables.KeyBullets = user.carriedConsumables.KeyBullets + 1;
			}
		}
	// Token: 0x06000253 RID: 595 RVA: 0x00013784 File Offset: 0x00011984
	public override void Pickup(PlayerController player)
	{
		base.Pickup(player);
	}

		// Token: 0x06000254 RID: 596 RVA: 0x000137BC File Offset: 0x000119BC
		protected override void OnPreDrop(PlayerController user)
		{

		}

		// Token: 0x06000255 RID: 597 RVA: 0x000137F8 File Offset: 0x000119F8
	}
}
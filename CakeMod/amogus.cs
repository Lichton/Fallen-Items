using System;
using ItemAPI;
using UnityEngine;

namespace CakeMod
{
	// Token: 0x02000062 RID: 98
	internal class amogus : PassiveItem
	{
		// Token: 0x06000250 RID: 592 RVA: 0x000135DE File Offset: 0x000117DE
	

		// Token: 0x06000251 RID: 593 RVA: 0x000135F4 File Offset: 0x000117F4
		public static void Init()
		{
			string name = "amogus";
			string resourcePath = "CakeMod/Resources/amogus";
			GameObject gameObject = new GameObject(name);
			amogus petRock = gameObject.AddComponent<amogus>();
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject);
			string shortDesc = "amongst whom";
			string longDesc = "haha drip";
			petRock.SetupItem(shortDesc, longDesc, "cak");
			petRock.quality = PickupObject.ItemQuality.EXCLUDED;
			petRock.sprite.IsPerpendicular = true;
		}

		// Token: 0x06000252 RID: 594 RVA: 0x00013674 File Offset: 0x00011874
		private void RoomCleared(PlayerController obj)
		{
				string header = "amogus.";
				string text = "amogus drip";
				this.Notify(header, text);
		}

		// Token: 0x06000253 RID: 595 RVA: 0x00013784 File Offset: 0x00011984
		public override void Pickup(PlayerController player)
		{
			bool pickedUp = this.m_pickedUp;
			if (!pickedUp)
			{
				player.OnRoomClearEvent += this.RoomCleared;
				base.Pickup(player);
			}
		}

		// Token: 0x06000254 RID: 596 RVA: 0x000137BC File Offset: 0x000119BC
		public override DebrisObject Drop(PlayerController player)
		{
			DebrisObject debrisObject = base.Drop(player);
			player.OnRoomClearEvent -= this.RoomCleared;
			debrisObject.GetComponent<amogus>().m_pickedUpThisRun = true;
			return debrisObject;
		}

		// Token: 0x06000255 RID: 597 RVA: 0x000137F8 File Offset: 0x000119F8
		private void Notify(string header, string text)
		{
			tk2dBaseSprite notificationObjectSprite = GameUIRoot.Instance.notificationController.notificationObjectSprite;
			GameUIRoot.Instance.notificationController.DoCustomNotification(header, text, notificationObjectSprite.Collection, notificationObjectSprite.spriteId, UINotificationController.NotificationColor.SILVER, false, false);
		}

	}
}
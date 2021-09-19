using System;
using ItemAPI;
using UnityEngine;

namespace CakeMod
{
	// Token: 0x02000062 RID: 98
	internal class David : PassiveItem
	{
		// Token: 0x06000250 RID: 592 RVA: 0x000135DE File Offset: 0x000117DE
		public David()
		{
			this.MotivateCh = 2f;
		}

		// Token: 0x06000251 RID: 593 RVA: 0x000135F4 File Offset: 0x000117F4
		public static void Init()
		{
			string name = "David";
			string resourcePath = "CakeMod/Resources/David";
			GameObject gameObject = new GameObject(name);
			David petRock = gameObject.AddComponent<David>();
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject);
			string shortDesc = "Punapple";
			string longDesc = "Tells funny puns.\n\nJust being near it increases your coolness insanely.";
			petRock.SetupItem(shortDesc, longDesc, "cak");
			petRock.AddPassiveStatModifier(PlayerStats.StatType.Coolness, 10f, StatModifier.ModifyMethod.ADDITIVE);
			petRock.quality = PickupObject.ItemQuality.EXCLUDED;
			petRock.sprite.IsPerpendicular = true;
		}

		// Token: 0x06000252 RID: 594 RVA: 0x00013674 File Offset: 0x00011874
		private void RoomCleared(PlayerController obj)
		{
			int num = UnityEngine.Random.Range(1, 6);
			bool flag = (float)num < this.MotivateCh;
			if (flag)
			{
				string header = "Jokes are punny.";
				string text = "But YOKES are even better!";
				int num2 = UnityEngine.Random.Range(0, 5);
				{
					int num3 = UnityEngine.Random.Range(0, 5);
					bool flag3 = num3 == 0;
					if (flag3)
					{
						header = "Who lives in a gunapple?";
						text = "Sprunbob Squarepants!";
					}
					bool flag4 = num3 == 1;
					if (flag4)
					{
						header = "woood fired piizza?";
						text = "hows piizza going to get a joob now?!?!";
					}
					bool flag5 = num3 == 2;
					if (flag5)
					{
						header = "What makes you suppressed?";
						text = "The Great Suppression!";
					}
					bool flag6 = num3 == 3;
					if (flag6)
					{
						header = "Why am I still with you?";
						text = "Because the world revolvers around you!";
					}
					bool flag7 = num3 == 4;
					if (flag7)
					{
						header = "A Gun + an Octopus is...";
						text = "A glocktopus!";
					}
				}
				bool flag8 = num2 == 5;
				if (flag8)
				{
					header = "What do you call when you need help?";
					text = "I need lemonaid!";
				}
				this.Notify(header, text);
			}
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
			debrisObject.GetComponent<David>().m_pickedUpThisRun = true;
			return debrisObject;
		}

		// Token: 0x06000255 RID: 597 RVA: 0x000137F8 File Offset: 0x000119F8
		private void Notify(string header, string text)
		{
			tk2dBaseSprite notificationObjectSprite = GameUIRoot.Instance.notificationController.notificationObjectSprite;
			GameUIRoot.Instance.notificationController.DoCustomNotification(header, text, notificationObjectSprite.Collection, notificationObjectSprite.spriteId, UINotificationController.NotificationColor.SILVER, false, false);
		}

		// Token: 0x040000F9 RID: 249
		private float MotivateCh;
	}
}

using Gungeon;
using ItemAPI;
using UnityEngine;
using Dungeonator;
using HutongGames.PlayMaker.Actions;
using System.Collections;

namespace CakeMod
{
	// Token: 0x02000034 RID: 52
	internal class HeartyLocket : PassiveItem
	{
		// Token: 0x06000164 RID: 356 RVA: 0x0000D45C File Offset: 0x0000B65C
		public static void Init()
		{
			string name = "Hearty Locket";
			string resourcePath = "CakeMod/Resources/HeartyLocket";
			GameObject gameObject = new GameObject(name);
			HeartyLocket blankKey = gameObject.AddComponent<HeartyLocket>();
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject);
			string shortDesc = "Key To My Heart";
			string longDesc = "Spending a key spawns a chest.\n\nEven mimics have hearts, even if all that's in them is greed.";
			blankKey.SetupItem(shortDesc, longDesc, "cak");
			blankKey.quality = PickupObject.ItemQuality.S;
		}

		// Token: 0x06000165 RID: 357 RVA: 0x0000D4B4 File Offset: 0x0000B6B4
		protected override void Update()
		{
			bool flag = base.Owner;
			if (flag)
			{
				this.CalculateKeys(base.Owner);
			}
		}

		// Token: 0x06000166 RID: 358 RVA: 0x0000D4E8 File Offset: 0x0000B6E8
		private void CalculateKeys(PlayerController player)
		{
			this.currentKeys = (float)player.carriedConsumables.KeyBullets;
			bool flag = this.currentKeys < this.lastKeys;
			if (flag)
			{
				bool flag2 = base.Owner.HasPickupID(Game.Items["cak:hearty_locket"].PickupObjectId);
				if (flag2)
				{
					GameManager.Instance.StartCoroutine(TheCountdown());
					RoomHandler roomd = Owner.CurrentRoom;
					IntVector2 randomVisibleClearSpot5 = Owner.CurrentRoom.GetRandomVisibleClearSpot(1, 1);
					Chest rainbow_Chest = GameManager.Instance.RewardManager.D_Chest;
					rainbow_Chest.IsLocked = false;
					Chest.Spawn(rainbow_Chest, randomVisibleClearSpot5);
				}
				else
				{
					GameManager.Instance.StartCoroutine(TheCountdown());
					RoomHandler roomd = Owner.CurrentRoom;
					IntVector2 randomVisibleClearSpot5 = Owner.CurrentRoom.GetRandomVisibleClearSpot(1, 1);
					Chest rainbow_Chest = GameManager.Instance.RewardManager.D_Chest;
					rainbow_Chest.IsLocked = false;
					Chest.Spawn(rainbow_Chest, randomVisibleClearSpot5);
				}
			}
			this.lastKeys = this.currentKeys;
		}

		// Token: 0x06000167 RID: 359 RVA: 0x0000D58C File Offset: 0x0000B78C
		public override void Pickup(PlayerController player)
		{
			bool flag = !this.m_pickedUpThisRun;
			base.Pickup(player);
		}

		// Token: 0x06000168 RID: 360 RVA: 0x0000D5DC File Offset: 0x0000B7DC
		public override DebrisObject Drop(PlayerController player)
		{
			return base.Drop(player);
		}

		// Token: 0x06000169 RID: 361 RVA: 0x0000D5F7 File Offset: 0x0000B7F7
		protected override void OnDestroy()
		{
			base.OnDestroy();
		}


		// Token: 0x04000073 RID: 115
		private float currentKeys;

		// Token: 0x04000074 RID: 116
		private float lastKeys;

		private IEnumerator TheCountdown()
		{
			yield return new WaitForSeconds(0.01f);
			{
				RoomHandler room = Owner.CurrentRoom;
				IntVector2? pos = new IntVector2?(room.GetRandomVisibleClearSpot(2, 2));
				RewardManager rm = GameManager.Instance.RewardManager;
				Chest chest = GameManager.Instance.RewardManager.D_Chest;
				chest.IsLocked = false;
				chest.overrideMimicChance = 1;

				RoomHandler roomd = Owner.CurrentRoom;
				IntVector2 randomVisibleClearSpot5 = Owner.CurrentRoom.GetRandomVisibleClearSpot(1, 1);
				Chest rainbow_Chest = GameManager.Instance.RewardManager.D_Chest;
				rainbow_Chest.IsLocked = false;
				Chest.Spawn(rainbow_Chest, randomVisibleClearSpot5);
			}
			yield break;
		}
	}
}
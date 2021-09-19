using System;
using System.Collections.Generic;
using Gungeon;
using GungeonAPI;
using UnityEngine;
using ItemAPI;
using static PickupObject;

namespace CakeMod
{
	// Token: 0x0200000E RID: 14
	public static class OldManBreach
	{
		// Token: 0x06000059 RID: 89 RVA: 0x0000480C File Offset: 0x00002A0C
		public static void Add()
		{
			try
			{
				ShrineFactory ShrineFactory = new ShrineFactory
				{
					name = "OldManBreach",
					modID = "cak",
					spritePath = "CakeMod/Resources/OldManBreach/oldman_idle_001.png",
					shadowSpritePath = "CakeMod/Resources/OldManBreach/oldman_shadow_001.png",
					acceptText = "It's a deal.",
					declineText = "I don't think I want what you're offering.",
					OnAccept = new Action<PlayerController, GameObject>(OldManBreach.Accept),
					OnDecline = null,
					CanUse = new Func<PlayerController, GameObject, bool>(OldManBreach.CanUse),
					offset = new Vector3(35f, 45f, 51.3f),
				talkPointOffset = new Vector3(1f, 2f, 0f),
					isToggle = false,
					isBreachShrine = true,
					interactableComponent = typeof(OldManBreachInteractable)
				};
				GameObject gameObject = ShrineFactory.Build();
				gameObject.AddAnimation("idle", "CakeMod/Resources/OldManBreach/oldman_idle", 4, NPCBuilder.AnimationType.Idle, DirectionalAnimation.DirectionType.None, DirectionalAnimation.FlipType.None);
				gameObject.AddAnimation("talk", "CakeMod/Resources/OldManBreach/oldman_idle", 4, NPCBuilder.AnimationType.Talk, DirectionalAnimation.DirectionType.None, DirectionalAnimation.FlipType.None);
				gameObject.AddAnimation("talk_start", "CakeMod/Resources/OldManBreach/oldman_idle", 4, NPCBuilder.AnimationType.Other, DirectionalAnimation.DirectionType.None, DirectionalAnimation.FlipType.None);
				gameObject.AddAnimation("do_effect", "CakeMod/Resources/OldManBreach/oldman_talk", 3, NPCBuilder.AnimationType.Other, DirectionalAnimation.DirectionType.None, DirectionalAnimation.FlipType.None);
				OldManBreachInteractable component = gameObject.GetComponent<OldManBreachInteractable>();
				component.conversation = new List<string>
			{
				"Hey, you!",
				"It's dangerous to go alone!",
				"Take this!"
			};
				gameObject.SetActive(false);
			}
			catch (Exception e)
			{
				Tools.Print(e);
			}
		}

		// Token: 0x060006D3 RID: 1747 RVA: 0x0003ACFC File Offset: 0x00038EFC

		private static bool CanUse(PlayerController player, GameObject npc)
		{
			return player != OldManBreach.storedPlayer;
		}

		// Token: 0x060006D4 RID: 1748 RVA: 0x0003AD10 File Offset: 0x00038F10
		public static void Accept(PlayerController player, GameObject npc)
		{
			PickupObject.ItemQuality itemQuality = ItemQuality.D;
			PickupObject itemOfTypeAndQuality = LootEngine.GetItemOfTypeAndQuality<Gun>(itemQuality, GameManager.Instance.RewardManager.GunsLootTable, false);
			LootEngine.SpawnItem(itemOfTypeAndQuality.gameObject, player.specRigidbody.UnitCenter, Vector2.down, 1f, false, true, false);
			OldManBreach.storedPlayer = player;
		}



		// Token: 0x04000015 RID: 21
		private static PlayerController storedPlayer;
	}
}


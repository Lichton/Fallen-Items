using System;
using System.Collections;
using System.Collections.Generic;
using Gungeon;
using GungeonAPI;
using ItemAPI;
using UnityEngine;


namespace CakeMod
{
	// Token: 0x0200000E RID: 14
	public static class Chester
	{
		// Token: 0x06000059 RID: 89 RVA: 0x0000480C File Offset: 0x00002A0C
		public static void Add()
		{
			try
			{
				ShrineFactory ShrineFactory = new ShrineFactory
				{
					name = "Chester",
					modID = "cak",
					spritePath = "CakeMod/Resources/Chester/Chester_idle_001.png",
					shadowSpritePath = "CakeMod/Resources/Chester/Chester_shadow_001.png",
					acceptText = "It's a deal.",
					declineText = "I don't think I want what you're offering.",
					OnAccept = new Action<PlayerController, GameObject>(Chester.Accept),
					OnDecline = null,
					CanUse = new Func<PlayerController, GameObject, bool>(Chester.CanUse),
					offset = new Vector3(60f, 35f, 51.3f),
					talkPointOffset = new Vector3(2f, 2f, 0f),
					isToggle = false,
					isBreachShrine = true,
					interactableComponent = typeof(ChesterInteractible)
				};
				GameObject gameObject = ShrineFactory.Build();
				gameObject.AddAnimation("idle", "CakeMod/Resources/Chester/Chester_idle", 4, NPCBuilder.AnimationType.Idle, DirectionalAnimation.DirectionType.None, DirectionalAnimation.FlipType.None);
				gameObject.AddAnimation("talk", "CakeMod/Resources/Chester/Chester_idle", 4, NPCBuilder.AnimationType.Talk, DirectionalAnimation.DirectionType.None, DirectionalAnimation.FlipType.None);
				gameObject.AddAnimation("talk_start", "CakeMod/Resources/Chester/Chester_talk", 4, NPCBuilder.AnimationType.Other, DirectionalAnimation.DirectionType.None, DirectionalAnimation.FlipType.None);
				gameObject.AddAnimation("do_effect", "CakeMod/Resources/Chester/Chester_doeffect", 3, NPCBuilder.AnimationType.Other, DirectionalAnimation.DirectionType.None, DirectionalAnimation.FlipType.None);
				ChesterInteractible component = gameObject.GetComponent<ChesterInteractible>();
				component.conversation = new List<string>
			{
				"Heya, kid!",
				"You look like you need a hand!",
				"Say, pal, I've got a deal for ya.",
				"Let's make this a bit easier, yeah?"
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
			return player != Chester.storedPlayer;
		}

		// Token: 0x060006D4 RID: 1748 RVA: 0x0003AD10 File Offset: 0x00038F10
		public static void Accept(PlayerController player, GameObject npc)
		{
			Chester.storedPlayer = player;
			player.ownerlessStatModifiers.Add(Toolbox.SetupStatModifier(PlayerStats.StatType.AdditionalBlanksPerFloor, 0, StatModifier.ModifyMethod.MULTIPLICATIVE));
			player.carriedConsumables.InfiniteKeys = true;
			
			LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(CakeIDs.DevilContract).gameObject, player);
			return;
		}
		

		// Token: 0x04000015 RID: 21
		private static PlayerController storedPlayer;

	}
}


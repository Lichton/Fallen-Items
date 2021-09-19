using System;
using System.Collections.Generic;
using Gungeon;
using GungeonAPI;
using ItemAPI;
using UnityEngine;


namespace CakeMod
{
	// Token: 0x0200000E RID: 14
	public static class TinyBullet
	{
		// Token: 0x06000059 RID: 89 RVA: 0x0000480C File Offset: 0x00002A0C
		public static void Add()
		{
			try
			{
				ShrineFactory ShrineFactory = new ShrineFactory
				{
					name = "Shrap And Nel",
					modID = "cak",
					spritePath = "CakeMod/Resources/TinyBullet/Idle/tinybullet_idle_001.png",
					acceptText = "Let's get explodey!",
					declineText = "I'm fine.",
					OnAccept = new Action<PlayerController, GameObject>(TinyBullet.Accept),
					OnDecline = null,
					CanUse = new Func<PlayerController, GameObject, bool>(TinyBullet.CanUse),
					offset = new Vector3(40f, 40f, 51.3f),
					talkPointOffset = new Vector3(1f, 1f, 0f),
					isToggle = false,
					isBreachShrine = true,
					interactableComponent = typeof(TinyBulletInteractable)
				};
				GameObject gameObject = ShrineFactory.Build();
				gameObject.AddAnimation("idle", "CakeMod/Resources/TinyBullet/Idle/", 2, NPCBuilder.AnimationType.Idle, DirectionalAnimation.DirectionType.None, DirectionalAnimation.FlipType.None);
				gameObject.AddAnimation("talk", "CakeMod/Resources/TinyBullet/Talk/", 6, NPCBuilder.AnimationType.Talk, DirectionalAnimation.DirectionType.None, DirectionalAnimation.FlipType.None);
				gameObject.AddAnimation("talk_start", "CakeMod/Resources/TinyBullet/Talk/", 6, NPCBuilder.AnimationType.Other, DirectionalAnimation.DirectionType.None, DirectionalAnimation.FlipType.None);
				gameObject.AddAnimation("do_effect", "CakeMod/Resources/TinyBullet/Talk/", 5, NPCBuilder.AnimationType.Other, DirectionalAnimation.DirectionType.None, DirectionalAnimation.FlipType.None);
				TinyBulletInteractable component = gameObject.GetComponent<TinyBulletInteractable>();
				component.conversation = new List<string>
			{
				"Yoooo!",
				"Got out of that dungeon crawling stuff.",
				"Not that much to do up here.",
				"Wanna get booming?"
			};
				gameObject.SetActive(false);
			}
			catch(Exception e)
            {
				Tools.Print(e);
            }
		}

		// Token: 0x060006D3 RID: 1747 RVA: 0x0003ACFC File Offset: 0x00038EFC

		private static bool CanUse(PlayerController player, GameObject npc)
		{
			return true;
		}

		// Token: 0x060006D4 RID: 1748 RVA: 0x0003AD10 File Offset: 0x00038F10
		public static void Accept(PlayerController player, GameObject npc)
		{
			//ArtifactMonger.HandleLoadout(player);
		}
		


		// Token: 0x04000015 RID: 21
		//private static PlayerController storedPlayer;
	}
}


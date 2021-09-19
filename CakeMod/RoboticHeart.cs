using System;
using System.Collections;
using Dungeonator;
using ItemAPI;
using UnityEngine;
using Gungeon;

namespace CakeMod
{
	public class RoboticHeart : PlayerItem
	{

	
		public static void Init()
		{
			string name = "Robotic Heart";
			string resourcePath = "CakeMod/Resources/RobotHeart";
			GameObject gameObject = new GameObject(name);
			RoboticHeart rh = gameObject.AddComponent<RoboticHeart>();
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject);
			string shortDesc = "Magnetic Charm";
			string longDesc = "A heart forged of simple magnetized iron with the inexplicable ability to draw creatures to its wielder. Its said they feel a certain attraction to you. Hopefully its positive!";
			rh.SetupItem(shortDesc, longDesc, "cak");
			rh.SetCooldownType(ItemBuilder.CooldownType.PerRoom, 5f);
			rh.consumable = false;
			rh.quality = PickupObject.ItemQuality.B;
		}

		// Token: 0x0600013D RID: 317 RVA: 0x0000DF55 File Offset: 0x0000C155
		public override void Pickup(PlayerController player)
		{
			base.Pickup(player);
		}

		// Token: 0x0600013E RID: 318 RVA: 0x0000DF60 File Offset: 0x0000C160
		protected override void DoEffect(PlayerController user)
		{
			AkSoundEngine.PostEvent("Play_OBJ_charmhorn_use_01", base.gameObject);
			RoomHandler currentRoom = user.CurrentRoom;
			bool flag = currentRoom.HasActiveEnemies(RoomHandler.ActiveEnemyType.All);
			RoomHandler absoluteRoom = base.transform.position.GetAbsoluteRoom();
			AIActor randomActiveEnemy = absoluteRoom.GetRandomActiveEnemy(false);
			bool flag2 = flag;
			if (flag2)
            {
				randomActiveEnemy.aiActor.ApplyEffect(GameManager.Instance.Dungeon.sharedSettingsPrefab.DefaultPermanentCharmEffect, 1f, null);
			}
		}

	}
}
using System;
using System.Collections;
using Dungeonator;
using ItemAPI;
using UnityEngine;

namespace CakeMod
{
	// Token: 0x02000033 RID: 51
	public class Honkhorn : PlayerItem
	{
		private static FleePlayerData fleeData;

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000138 RID: 312 RVA: 0x0000DE5F File Offset: 0x0000C05F
		// (set) Token: 0x06000139 RID: 313 RVA: 0x0000DE67 File Offset: 0x0000C067
		public float Random { get; private set; }

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600013A RID: 314 RVA: 0x0000DE70 File Offset: 0x0000C070
		// (set) Token: 0x0600013B RID: 315 RVA: 0x0000DE78 File Offset: 0x0000C078
		public object FreezeModifierEffect { get; private set; }

		// Token: 0x0600013C RID: 316 RVA: 0x0000DE84 File Offset: 0x0000C084
		public static void Init()
		{
			string name = "Honkhorn";
			string resourcePath = "CakeMod/Resources/Honkhorn";
			GameObject gameObject = new GameObject(name);
			Honkhorn freezeLighter = gameObject.AddComponent<Honkhorn>();
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject);
			string shortDesc = "HONK!";
			string longDesc = "Upon honking this horn, it emits a truly horrendous sound that strikes fear into any listener. It seems contact with the item prevents you from being affected.";
			freezeLighter.SetupItem(shortDesc, longDesc, "cak");
			freezeLighter.SetCooldownType(ItemBuilder.CooldownType.Damage, 200f);
			freezeLighter.consumable = false;
			freezeLighter.quality = PickupObject.ItemQuality.B;
		}

		// Token: 0x0600013D RID: 317 RVA: 0x0000DF55 File Offset: 0x0000C155
		public override void Pickup(PlayerController player)
		{
			base.Pickup(player);
			Honkhorn.fleeData = new FleePlayerData();
			Honkhorn.fleeData.Player = player;
			Honkhorn.fleeData.StartDistance = 100f;
		}

		// Token: 0x0600013E RID: 318 RVA: 0x0000DF60 File Offset: 0x0000C160F
		protected override void DoEffect(PlayerController user)
		{
			AkSoundEngine.PostEvent("Play_OBJ_charmhorn_use_01", base.gameObject);
			RoomHandler currentRoom = user.CurrentRoom;
			bool flag = currentRoom.HasActiveEnemies(RoomHandler.ActiveEnemyType.All);
			bool flag2 = flag;
			if (flag2)
				foreach (AIActor aiactor in currentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All))
				{
					bool flag3 = aiactor.behaviorSpeculator != null;
					if (flag3)
					{
						aiactor.behaviorSpeculator.FleePlayerData = Honkhorn.fleeData;
						FleePlayerData fleePlayerData = new FleePlayerData();
						GameManager.Instance.StartCoroutine(Honkhorn.ohshitthatsnottheslayer(aiactor));
					}
				}
		}
		private static IEnumerator ohshitthatsnottheslayer(AIActor aiactor)
		{
			yield return new WaitForSeconds(4f);
			aiactor.behaviorSpeculator.FleePlayerData = null;
			yield break;
			{

			}
		}

	}
}
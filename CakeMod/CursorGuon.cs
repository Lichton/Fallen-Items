using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;
using MonoMod.RuntimeDetour;
using Dungeonator;
using GungeonAPI;

namespace CakeMod
{
	// Token: 0x02000040 RID: 64
	internal class ArrowGuonStone : CursonGuonOrbital
	{
		// Token: 0x060001A9 RID: 425 RVA: 0x0000EDB4 File Offset: 0x0000CFB4
		public static void Init()
		{
			string name = "Arrow Guon Stone";
			string resourcePath = "CakeMod/Resources/ArrowGuonStone";
			GameObject gameObject = new GameObject();
			ArrowGuonStone arrowGuonStone = gameObject.AddComponent<ArrowGuonStone>();
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject);
			string shortDesc = "Curse-or";
			string longDesc = "This arrow seems to point to wherever you are not aiming. Very helpful for you, but not so much for enemies.";
			arrowGuonStone.SetupItem(shortDesc, longDesc, "cak");
			arrowGuonStone.quality = PickupObject.ItemQuality.EXCLUDED;
			ArrowGuonStone.BuildPrefab();
			arrowGuonStone.CursorPrefab = ArrowGuonStone.orbitalPrefab;
			arrowGuonStone.Identifier = CursonGuonOrbital.IounStoneIdentifier.GENERIC;
		}

		// Token: 0x060001AA RID: 426 RVA: 0x0000EE24 File Offset: 0x0000D024
		public static void BuildPrefab()
		{
			bool flag = ArrowGuonStone.orbitalPrefab != null;
			if (!flag)
			{
				GameObject gameObject = SpriteBuilder.SpriteFromResource("CakeMod/Resources/ArrowGuonStone", null, true);
				gameObject.name = "Arrow Guon Orbital";
				SpeculativeRigidbody speculativeRigidbody = gameObject.GetComponent<tk2dSprite>().SetUpSpeculativeRigidbody(IntVector2.Zero, new IntVector2(7, 13));
				speculativeRigidbody.CollideWithTileMap = false;
				speculativeRigidbody.CollideWithOthers = true;
				speculativeRigidbody.PrimaryPixelCollider.CollisionLayer = CollisionLayer.EnemyBulletBlocker;
				ArrowGuonStone.orbitalPrefab = gameObject.AddComponent<CursonGuonController>();
				ArrowGuonStone.orbitalPrefab.shouldRotate = true;
				ArrowGuonStone.orbitalPrefab.orbitRadius = 2f;
				ArrowGuonStone.orbitalPrefab.orbitDegreesPerSecond = 60f;
				ArrowGuonStone.orbitalPrefab.SetOrbitalTier(0);
				UnityEngine.Object.DontDestroyOnLoad(gameObject);
				FakePrefab.MarkAsFakePrefab(gameObject);
				gameObject.SetActive(false);
			}
		}

		// Token: 0x060001AB RID: 427 RVA: 0x0000EEF4 File Offset: 0x0000D0F4
		public override void Pickup(PlayerController player)
		{
			ArrowGuonStone.guonHook = new Hook(typeof(CursorGuonItem).GetMethod("Initialize"), typeof(ArrowGuonStone).GetMethod("GuonInit"));
			base.Pickup(player);
		}

		// Token: 0x060001AC RID: 428 RVA: 0x0000EF64 File Offset: 0x0000D164
		public override DebrisObject Drop(PlayerController player)
		{
			ArrowGuonStone.guonHook.Dispose();
			return base.Drop(player);
		}

		// Token: 0x060001AD RID: 429 RVA: 0x0000EF9F File Offset: 0x0000D19F
		protected override void OnDestroy()
		{
			ArrowGuonStone.guonHook.Dispose();
			base.OnDestroy();
		}


		// Token: 0x060001AF RID: 431 RVA: 0x0000EFD8 File Offset: 0x0000D1D8
		public static void GuonInit(Action<CursorGuonItem, PlayerController> orig, CursorGuonItem self, PlayerController player)
		{
			orig(self, player);
		}

		// Token: 0x060001B0 RID: 432 RVA: 0x0000F00C File Offset: 0x0000D20C


		// Token: 0x0400007A RID: 122
		public static Hook guonHook;

		// Token: 0x0400007B RID: 123
		public static bool speedUp = false;

		public CursonGuonController CursorPrefab;
		// Token: 0x0400007C RID: 124
		public static CursonGuonController orbitalPrefab;

		// Token: 0x0400007D RID: 125
		public static CursonGuonController rookGuonOrbital;

		

		
	}
}

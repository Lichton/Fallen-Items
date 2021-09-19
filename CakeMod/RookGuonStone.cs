using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;
using MonoMod.RuntimeDetour;
using Dungeonator;

namespace CakeMod
{
	// Token: 0x02000040 RID: 64
	internal class RookGuonStone : IounStoneOrbitalItem
	{
		// Token: 0x060001A9 RID: 425 RVA: 0x0000EDB4 File Offset: 0x0000CFB4
		public static void Init()
		{
			string name = "Rook Guon Stone";
			string resourcePath = "CakeMod/Resources/RookGuonStone";
			GameObject gameObject = new GameObject();
			RookGuonStone brownGuonStone = gameObject.AddComponent<RookGuonStone>();
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject);
			string shortDesc = "Only A Pawn In My Game";
			string longDesc = "This chess piece has somehow absorbed copius amounts of magical power that allows sentience. It believes you to be it's King.";
			brownGuonStone.SetupItem(shortDesc, longDesc, "cak");
			brownGuonStone.quality = PickupObject.ItemQuality.B;
			RookGuonStone.BuildPrefab();
			brownGuonStone.OrbitalPrefab = RookGuonStone.orbitalPrefab;
			brownGuonStone.Identifier = IounStoneOrbitalItem.IounStoneIdentifier.GENERIC;
		}

		// Token: 0x060001AA RID: 426 RVA: 0x0000EE24 File Offset: 0x0000D024
		public static void BuildPrefab()
		{
			bool flag = RookGuonStone.orbitalPrefab != null;
			if (!flag)
			{
				GameObject gameObject = SpriteBuilder.SpriteFromResource("CakeMod/Resources/RookGuonStone", null, true);
				gameObject.name = "Rook Guon Orbital";
				SpeculativeRigidbody speculativeRigidbody = gameObject.GetComponent<tk2dSprite>().SetUpSpeculativeRigidbody(IntVector2.Zero, new IntVector2(7, 13));
				speculativeRigidbody.CollideWithTileMap = false;
				speculativeRigidbody.CollideWithOthers = true;
				speculativeRigidbody.PrimaryPixelCollider.CollisionLayer = CollisionLayer.EnemyBulletBlocker;
				RookGuonStone.orbitalPrefab = gameObject.AddComponent<PlayerOrbital>();
				RookGuonStone.orbitalPrefab.motionStyle = PlayerOrbital.OrbitalMotionStyle.ORBIT_PLAYER_ALWAYS;
				RookGuonStone.orbitalPrefab.shouldRotate = false;
				RookGuonStone.orbitalPrefab.orbitRadius = 2f;
				RookGuonStone.orbitalPrefab.orbitDegreesPerSecond = 30f;
				RookGuonStone.orbitalPrefab.SetOrbitalTier(0);
				UnityEngine.Object.DontDestroyOnLoad(gameObject);
				FakePrefab.MarkAsFakePrefab(gameObject);
				gameObject.SetActive(false);
			}
		}

		// Token: 0x060001AB RID: 427 RVA: 0x0000EEF4 File Offset: 0x0000D0F4
		public override void Pickup(PlayerController player)
		{
			RookGuonStone.guonHook = new Hook(typeof(PlayerOrbital).GetMethod("Initialize"), typeof(RookGuonStone).GetMethod("GuonInit"));
			base.Pickup(player);
		}

		// Token: 0x060001AC RID: 428 RVA: 0x0000EF64 File Offset: 0x0000D164
		public override DebrisObject Drop(PlayerController player)
		{
			RookGuonStone.guonHook.Dispose();
			return base.Drop(player);
		}

		// Token: 0x060001AD RID: 429 RVA: 0x0000EF9F File Offset: 0x0000D19F
		protected override void OnDestroy()
		{
			RookGuonStone.guonHook.Dispose();
			base.OnDestroy();
		}


		// Token: 0x060001AF RID: 431 RVA: 0x0000EFD8 File Offset: 0x0000D1D8
		public static void GuonInit(Action<PlayerOrbital, PlayerController> orig, PlayerOrbital self, PlayerController player)
		{
			orig(self, player);
			bool flag = self.name == "Rook Guon Orbital(Clone)";
			if (flag)
			{
				RookGuonStone.rookGuonOrbital = self;
			}
		}

		// Token: 0x060001B0 RID: 432 RVA: 0x0000F00C File Offset: 0x0000D20C


		// Token: 0x0400007A RID: 122
		public static Hook guonHook;

		// Token: 0x0400007B RID: 123
		public static bool speedUp = false;

		// Token: 0x0400007C RID: 124
		public static PlayerOrbital orbitalPrefab;

		// Token: 0x0400007D RID: 125
		public static PlayerOrbital rookGuonOrbital;

		// Token: 0x0400007E RID: 126
		private int currentItems;

		// Token: 0x0400007F RID: 127
		private int lastItems;

		// Token: 0x04000080 RID: 128
		private int currentGuns;

		// Token: 0x04000081 RID: 129
		private int lastGuns;
	}
}

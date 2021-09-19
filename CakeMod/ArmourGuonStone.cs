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
	internal class ArmourGuonStone : AdvancedPlayerOrbitalItem
	{
		// Token: 0x060001A9 RID: 425 RVA: 0x0000EDB4 File Offset: 0x0000CFB4
		public static void Init()
		{
			string name = "Armour Guon Stone";
			string resourcePath = "CakeMod/Resources/ArmourGuonStone";
			GameObject gameObject = new GameObject();
			ArmourGuonStone brownGuonStone = gameObject.AddComponent<ArmourGuonStone>();
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject);
			string shortDesc = "Offense Is The Best Defense";
			string longDesc = "Armour forged by the Guonsmith, his fundemental lack of any knowledge in any field other than guon stones is apparent.";
			brownGuonStone.SetupItem(shortDesc, longDesc, "cak");
			brownGuonStone.quality = PickupObject.ItemQuality.EXCLUDED;
			ArmourGuonStone.BuildPrefab();
			brownGuonStone.OrbitalPrefab = ArmourGuonStone.orbitalPrefab;
			brownGuonStone.ArmorToGainOnInitialPickup = 1;
		}

		// Token: 0x060001AA RID: 426 RVA: 0x0000EE24 File Offset: 0x0000D024
		public static void BuildPrefab()
		{
			bool flag = ArmourGuonStone.orbitalPrefab != null;
			if (!flag)
			{
				GameObject gameObject = SpriteBuilder.SpriteFromResource("CakeMod/Resources/ArmourGuonStone", null, true);
				gameObject.name = "Armour Guon Orbital";
				SpeculativeRigidbody speculativeRigidbody = gameObject.GetComponent<tk2dSprite>().SetUpSpeculativeRigidbody(IntVector2.Zero, new IntVector2(7, 13));
				speculativeRigidbody.CollideWithTileMap = false;
				speculativeRigidbody.CollideWithOthers = true;
				speculativeRigidbody.PrimaryPixelCollider.CollisionLayer = CollisionLayer.EnemyBulletBlocker;
				ArmourGuonStone.orbitalPrefab = gameObject.AddComponent<PlayerOrbital>();
				ArmourGuonStone.orbitalPrefab.motionStyle = PlayerOrbital.OrbitalMotionStyle.ORBIT_PLAYER_ALWAYS;
				ArmourGuonStone.orbitalPrefab.shouldRotate = false;
				ArmourGuonStone.orbitalPrefab.orbitRadius = 3f;
				ArmourGuonStone.orbitalPrefab.orbitDegreesPerSecond = 40f;
				ArmourGuonStone.orbitalPrefab.SetOrbitalTier(0);
				UnityEngine.Object.DontDestroyOnLoad(gameObject);
				FakePrefab.MarkAsFakePrefab(gameObject);
				gameObject.SetActive(false);
			}
		}

		public override void OnOrbitalCreated(GameObject orbital)
		{
			SpeculativeRigidbody orbBody = orbital.GetComponent<SpeculativeRigidbody>();
			if (orbBody)
			{
				orbBody.specRigidbody.OnPreRigidbodyCollision += this.DoBlankDisappear;
			}
			base.OnOrbitalCreated(orbital);
		}

		private void DoBlankDisappear(SpeculativeRigidbody myRigidbody, PixelCollider myCollider, SpeculativeRigidbody other, PixelCollider otherCollider)
		{
			PlayerController owner = base.Owner;
			Projectile component = other.GetComponent<Projectile>();
			if (component != null && !(component.Owner is PlayerController))
			{
				GameObject silencerVFX = (GameObject)ResourceCache.Acquire("Global VFX/BlankVFX_Ghost");
				AkSoundEngine.PostEvent("Play_OBJ_silenceblank_small_01", base.gameObject);
				GameObject gameObject = new GameObject("silencer");
				SilencerInstance silencerInstance = gameObject.AddComponent<SilencerInstance>();
				float additionalTimeAtMaxRadius = 0.25f;
				silencerInstance.TriggerSilencer(myRigidbody.UnitCenter, 25f, 5f, silencerVFX, 0f, 3f, 3f, 3f, 250f, 5f, additionalTimeAtMaxRadius, owner, false, false);
			}
			ArmourGuonStone.guonHook.Dispose();
		}
		// Token: 0x060001AB RID: 427 RVA: 0x0000EEF4 File Offset: 0x0000D0F4
		public override void Pickup(PlayerController player)
		{
			ArmourGuonStone.guonHook = new Hook(typeof(PlayerOrbital).GetMethod("Initialize"), typeof(ArmourGuonStone).GetMethod("GuonInit"));
			base.Pickup(player);
		}

		// Token: 0x060001AC RID: 428 RVA: 0x0000EF64 File Offset: 0x0000D164
		public override DebrisObject Drop(PlayerController player)
		{
			ArmourGuonStone.guonHook.Dispose();
			return base.Drop(player);
		}

		// Token: 0x060001AD RID: 429 RVA: 0x0000EF9F File Offset: 0x0000D19F
		protected override void OnDestroy()
		{
			ArmourGuonStone.guonHook.Dispose();
			base.OnDestroy();
		}


		// Token: 0x060001AF RID: 431 RVA: 0x0000EFD8 File Offset: 0x0000D1D8
		public static void GuonInit(Action<PlayerOrbital, PlayerController> orig, PlayerOrbital self, PlayerController player)
		{
			orig(self, player);
			bool flag = self.name == "Rook Guon Orbital(Clone)";
			if (flag)
			{
				ArmourGuonStone.rookGuonOrbital = self;
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
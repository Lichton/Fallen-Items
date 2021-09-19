using System;
using System.Collections.Generic;
using System.Linq;
using GungeonAPI;
using ItemAPI;
using SaveAPI;
using UnityEngine;
using Dungeonator;
using GungeonAPI;
using ItemAPI;
using MonoMod.RuntimeDetour;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
namespace CakeMod
{
	// Token: 0x020000D4 RID: 212
	public class ItemsMod : ETGModule
	{
		public static AssetBundle ModAssets;
		// Token: 0x06000596 RID: 1430 RVA: 0x0003014F File Offset: 0x0002E34F
		public override void Exit()
		{
		}
		// Token: 0x06000597 RID: 1431 RVA: 0x00030154 File Offset: 0x0002E354
		public string path;
		public Assembly assembly;
		public static List<Texture2D> list;

		private void LoadFloor(string[] obj)
		{
			GameManager.Instance.LoadCustomLevel(DoodleDungeon.FloorNameDefinition.dungeonSceneName);;
		}
		
		public override void Start()
		{
			try
			{
				
				ZipFilePath = this.Metadata.Archive;
				metadata = this.Metadata.Directory;
				metadataARCHIVE = this.Metadata.Archive;
				FilePath = this.Metadata.Directory + "/rooms";
				
				path = "CakeMod.Resources.TurtSprites.";

				assembly = Assembly.GetExecutingAssembly();
				list = new List<Texture2D>();
				foreach (var item in assembly.GetManifestResourceNames())
				{
					if (!item.EndsWith(".png")) continue;

					if (item.StartsWith(path))
					{
						list.Add(ResourceExtractor.GetTextureFromResource(item));
					}
				}

				ETGModConsole.Log(list.Count.ToString());
				cosmoSpriteSheet = ResourceExtractor.GetTextureFromResource(cosmoSpriteSheetName);
				FloorSheet = ResourceExtractor.GetTextureFromResource(StringFloorSheet);
				
				
				FakePrefabHooks.Init();
				ItemBuilder.Init();
				EnemyTools.Init();
				EnemyBuilder.Init();
				Hooks.Init();
				BossBuilder.Init();
				SpecialBlankModificationItem.InitHooks();
				ItemsMod.Strings = new AdvancedStringDB();
				EasyGoopDefinitions.DefineDefaultGoops();

				Cake.Init();
				CircularKing.Init();
				KinglyBullets.Init();
				LockOfTheJammed.Init();
				CakeBullets.Init();
				BleakBullets.Init();
				Jankan.Init();
				Robohead.Init();
				HatShells.Init();
				DumbBullets.Init();
				ControlledBullets.Init();
				David.Init();
				HeadCrab.Init();
				GunJester.Init();
				Waffle.Init();
				BladeBullets.Init();
				TheFool.Init();
				BowlerHat.Init();
				Depresso.Init();
				FleshCake.Init();
				BountyPoster.Init();
				OneReverse.Init();
				StrangeEffigy.Init();
				ConcealedTreasure.Init();
				Winpetster.Init();
				RookGuonStone.Init();
				IceOgreHead.Init();
				MysteriousIdol.Init();
				GlowingWomb.Init();
				Honkhorn.Init();
				HeartyLocket.Init();
				FunnyHat.Init();
				Radio.Init();
				ChocolateBar.Init();
				CluwneBullets.Init();
				BowlerRainbowDust.Init();
				StrangeArrow.Init();
				testingitem.Init();
				TestItem.Init();
				glitchammolet.Init();
				testbarrel.Init();
				BlobHeart.Init();
				WebAmmolet.Init();
				StarNinja.Init();
				PoisonBomb.Init();
				RoboticHeart.Init();
				ApprenticeScroll.Init();
				amogus.Init();
				ArmouredKey.Init();
				BabyGoodChamber.Init();
				SpringlockSuit.Init();
				BirthdayCandle.Init();
				DeadlyInsanity.Init();
				Keysing.Init();
				BabyGoodLovebulon.Init();
				FreddyPizza.Init();
				CultistHelm.Init();
				Lime.Init();
				CurseItemTest.Init();
				GoopCurse.Init();
				Jammolet.Init();
				HoloProjection.Init();
				Clumsy.Init();
				MiniShroom.Init();
				DrawnChamber.Init();
				BabyGoodMagnum.Init();
				SewingKit.Init();
				PocketRedChest.Init();
				PocketBlackChest.Init();
				PocketGreenChest.Init();
				PocketBlueChest.Init();
				PocketBrownChest.Init();
				ChestFriend.Init();
				PricklyPear.Init();
				StrangePotion.Init();
				DevilContract.Init();
				Hellfire.Init();
				BloomingHeart.Init();
				RatRound.Init();
				FrozenFrog.Init();
				BabyGoodApiary.Init();
				ImmortalSmoke.Init();
				UFO.Init();
				VoodooHeart.Init();
				GunslingKingRequest.Init();
				HappyLad.Init();
				Fez.Init();
				NecromancerBook.Init();
				PlatinumJunk.Init();
				ToxicArmour.Init();
				Notebook.Init();
				TestJammedBullets.Init();
				BulletKinBullets.Init();
				LichHat.Init();
				Skelebot.Init();
				CosmoStatue.CosmoBuildPrefab();
				LichHat2.Init();
				PickleJar.Init();
				Jawbreaker.Init();
				Rift.Init();
				SaveDisk.Init();
				d20.Init();
				ThirdEye.Init();
				GhostlyBody.Init();
				Butter.Init();
				Cookie.Init();
				JunkChestHandler.TheHooks();
				CarpenterHandbook.Init();
				Ipecac.Init();
				Taurus.Init();
				LifeLemon.Init();
				ArmourGuonStone.Init();
				JammedGunParts.Init();
				CloningVat.Init();
				DevilKey.Init();
				TurtsMelon.Init();
				SpareShell.Init();
				FungalTurtle.Init();
				PBullets.Init();
				OddBullets.Init();
				BloodTether.Init();
				TableTechHolographic.Init();
				NavySealCopypasta.Init();
				RuneChalk.Init();
				//RuneChalk2.Init();

				Synergun.Add();
				JackpotOfGreed.Add();
				NoSpriteFound.Add();
				boneblaster.Add();
				KingGun.Add();
				GungeonGun.Add();
				Glockamole.Add();
				CursedKatana.Add();
				Blox.Add();
				Terragun.Add();
				SpongeGun.Add();
				Horn.Add();
				IHateGuns.Add();
				gamefreeze.Add();
				Bananastaff.Add();
				Blackpistol.Add();
				boomstick.Add();
				SydneySleeper.Add();
				Explodergun.Add();
				Scrapgun.Add();
				tinyshotgun.Add();
				GunResource.Add();
				Scrapper.Add();
				Megaphone.Add();
				MissGun.Add();
				Drawn47.Add();
				PirateShotgunKinGun.Add();
				AnArrowkin.Add();
				Timerase.Add();
				Knife.Add();
				firegun.Add();
				Television.Add();
				//Dupligun.Add();
				//OneShot.Add();
				MoneyDebuffEffect.Init();
				money22DebuffEffect.Init();
				hegemonyDebuffEffect.Init();
				moneydebuff3DebuffEffect.Init();
				DemonBuff.Init();
				CasingBullets.Init();
				UnlockHookInators.Init();
				InkBooklet.Init();
				Doodlelet.Init();
				KoolAidMan.Init();
				ammomimic.Init();
				BulletBishop.Init();
				FlameChamber.Init();
				KillShrine.Init();
				InflamedEye.Init();
				FlameClone.Init();
				Jesterlet.Init();
				Mimekin.Init();
				RoyalJesterlet.Init();
				DrawnKin.Init();
				HoveringGunsAdder.AddHovers();
				CakeGunMods.Init();
				BreachShopTool.AddBaseMetaShopTier(ETGMod.Databases.Items["Cultist Helm"].PickupObjectId, 80, ETGMod.Databases.Items["Rainbowllets"].PickupObjectId, 25, ETGMod.Databases.Items["Reloading Waffle"].PickupObjectId, 75, null);
				GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[]
				{
				new SyngergraceThisMod.HatSynergy(),
				new SyngergraceThisMod.IceSynergy(),
				new SyngergraceThisMod.RainbowSynergy(),
				new SyngergraceThisMod.ChaosSynergy(),
				new SyngergraceThisMod.VoodooSynergy(),
				//new SyngergraceThisMod.OldSynergy(),
				new SyngergraceThisMod.LoveSynergy(),
				new SyngergraceThisMod.SoundSynergy(),
				}).ToArray<AdvancedSynergyEntry>();
				
				//TinyBullet.Add();
				//OldManBreach.Add();
	

				//ArtistShrine.Add();
				Chester.Add();
				//LesGo.Add();
				StaticReferences.Init();
				StaticReferences2.Init();
				DungeonHandlerTrueForm.Init();
				ShrineFakePrefabHooks.Init();
				ShrineFactory.Init();
				OldShrineFactory.Init();
				SaveAPIManager.Setup("cak");
				ShrineFactory.PlaceBreachShrines();
				ETGModConsole.Log("King's Items is up and running.");
				SyngergraceThisMod.Synergies();
				SynergyFormInitialiser.AddSynergyForms();
				ModPrefabs.InitCustomPrefabs();
			
				ModRoomPrefabs.InitCustomRooms();

				FloorNameDungeonFlows.InitDungeonFlows();

				DoodleDungeon.InitCustomDungeon();

				ETGModConsole.Commands.AddUnit("daflow", (args) =>
				{
					DungeonHandlerTrueForm.debugFlow = !DungeonHandlerTrueForm.debugFlow;
					string status = DungeonHandlerTrueForm.debugFlow ? "enabled" : "disabled";
					string color = DungeonHandlerTrueForm.debugFlow ? "00FF00" : "FF0000";
					ETGModConsole.Log($"da flow is {status}", false);
				});
				List<string> SpareVFXPaths = new List<string>()
			{
				"CakeMod/Resources/BulletBishop/incense_teleport_poof_001",
				"CakeMod/Resources/BulletBishop/incense_teleport_poof_002",
				"CakeMod/Resources/BulletBishop/incense_teleport_poof_003",
				"CakeMod/Resources/BulletBishop/incense_teleport_poof_004",
				"CakeMod/Resources/BulletBishop/incense_teleport_poof_005",
				"CakeMod/Resources/BulletBishop/incense_teleport_poof_006",
				"CakeMod/Resources/BulletBishop/incense_teleport_poof_007",
				"CakeMod/Resources/BulletBishop/incense_teleport_poof_008",
			};
				GameObject spareVFX = VFXLibrary.CreateVFX("IncensePoof", SpareVFXPaths, 1, new IntVector2(10, 1), tk2dBaseSprite.Anchor.LowerCenter, true, 0.18f, 0, null);
				EasyVFXDatabase.IncenseVFX = spareVFX;
				List<string> SpareVFXPaths3 = new List<string>()
			{
				"CakeMod/Resources/FlameChamber/flamepoof_001",
			"CakeMod/Resources/FlameChamber/flamepoof_002",
			"CakeMod/Resources/FlameChamber/flamepoof_003",
			"CakeMod/Resources/FlameChamber/flamepoof_004",
			"CakeMod/Resources/FlameChamber/flamepoof_005",
			};
				GameObject spareVFX3 = VFXLibrary.CreateVFX("FlamePoof", SpareVFXPaths3, 7, new IntVector2(10, 1), tk2dBaseSprite.Anchor.LowerCenter, true, 0.18f, 0, null);
				EasyVFXDatabase.FlameVFX = spareVFX3;
				List<string> SpareVFXPaths2 = new List<string>()
			{
			"CakeMod/Resources/BloodyLightning/blood1",
			"CakeMod/Resources/BloodyLightning/blood2",
			"CakeMod/Resources/BloodyLightning/blood3",
			"CakeMod/Resources/BloodyLightning/blood4",
			"CakeMod/Resources/BloodyLightning/blood5",
			"CakeMod/Resources/BloodyLightning/blood6",
			"CakeMod/Resources/BloodyLightning/blood7",
			"CakeMod/Resources/BloodyLightning/blood8",
			"CakeMod/Resources/BloodyLightning/blood9",
			"CakeMod/Resources/BloodyLightning/blood10",
			};
				GameObject spareVFX2 = VFXLibrary.CreateVFX("String", SpareVFXPaths2, 16, new IntVector2(10, 1), tk2dBaseSprite.Anchor.LowerCenter, true, 0.18f, 0, null);
				EasyVFXDatabase.StringVFX = spareVFX2;
				//DoodleDungeon.InitCustomDungeon();
			}
			catch(Exception e)
            {
				ETGModConsole.Log(e.ToString(), false);
			}

			
		}
		public static string ZipFilePath;
		public static string metadata;
		public static string metadataARCHIVE;
		public static string FilePath;

		public void DelayedStart()
		{
		}

		
		public override void Init()
		{
		}

		
		public static GameObject vfx;

		
		public static AdvancedStringDB Strings;

		
		public static List<GameUIAmmoType> addedAmmoTypes = new List<GameUIAmmoType>();
		public static tk2dSpriteCollectionData HandleAnimations(tk2dBaseSprite sprite, Texture2D playerSheet)
		{
			tk2dSpriteCollectionData orig = sprite.Collection;

			var copyCollection = GameObject.Instantiate(orig);
			GameObject.DontDestroyOnLoad(copyCollection);

			tk2dSpriteDefinition[] copyDefinitions = new tk2dSpriteDefinition[orig.spriteDefinitions.Length];
			for (int i = 0; i < copyCollection.spriteDefinitions.Length; i++)
			{
				copyDefinitions[i] = Copy(orig.spriteDefinitions[i]);
			}
			copyCollection.spriteDefinitions = copyDefinitions;

			if (playerSheet != null)
			{
				var materialsToCopy = orig.materials;
				copyCollection.materials = new Material[orig.materials.Length];
				for (int i = 0; i < copyCollection.materials.Length; i++)
				{
					if (materialsToCopy[i] == null) continue;
					var mat = new Material(materialsToCopy[i]);
					GameObject.DontDestroyOnLoad(mat);
					mat.mainTexture = playerSheet;
					mat.name = materialsToCopy[i].name;
					copyCollection.materials[i] = mat;
				}

				for (int i = 0; i < copyCollection.spriteDefinitions.Length; i++)
				{
					foreach (var mat in copyCollection.materials)
					{
						if (mat != null && copyDefinitions[i].material.name.Equals(mat.name))
						{
							copyDefinitions[i].material = mat;
							copyDefinitions[i].materialInst = new Material(mat);
						}
					}
				}
			}

			return copyCollection;
		}

		// mostly taken from the custom character mod itsel

		// taken from the custom character mod itself
		public static tk2dSpriteDefinition Copy(tk2dSpriteDefinition orig)
		{

			tk2dSpriteDefinition copy = new tk2dSpriteDefinition()
			{
				material = orig.material != null ? new Material(orig.material) : null,
				materialInst = orig.materialInst != null ? new Material(orig.materialInst) : null,
				boundsDataCenter = orig.boundsDataCenter,
				boundsDataExtents = orig.boundsDataExtents,
				colliderConvex = orig.colliderConvex,
				colliderSmoothSphereCollisions = orig.colliderSmoothSphereCollisions,
				colliderType = orig.colliderType,
				colliderVertices = orig.colliderVertices,
				collisionLayer = orig.collisionLayer,
				complexGeometry = orig.complexGeometry,
				extractRegion = orig.extractRegion,
				flipped = orig.flipped,
				indices = orig.indices,
				
				materialId = orig.materialId,
				
				metadata = orig.metadata,
				name = orig.name,
				normals = orig.normals,
				physicsEngine = orig.physicsEngine,
				position0 = orig.position0,
				position1 = orig.position1,
				position2 = orig.position2,
				position3 = orig.position3,
				regionH = orig.regionH,
				regionW = orig.regionW,
				regionX = orig.regionX,
				regionY = orig.regionY,
				tangents = orig.tangents,
				texelSize = orig.texelSize,
				untrimmedBoundsDataCenter = orig.untrimmedBoundsDataCenter,
				untrimmedBoundsDataExtents = orig.untrimmedBoundsDataExtents,
				uvs = orig.uvs
			};

			return copy;
		}

		


		public PlayableCharacters baseCharacter = PlayableCharacters.Cosmonaut;
		public static void ApplyTexture(PlayerController player, tk2dSpriteCollectionData copyCollection)
		{
			// TODO why is this even instantiated before changing? so you can DDOL it? (I added the oldLibrary destroy)
			var oldLibrary = player.spriteAnimator.Library;
			player.spriteAnimator.Library = GameObject.Instantiate(oldLibrary);
			GameObject.DontDestroyOnLoad(player.spriteAnimator.Library);
			UnityEngine.Object.Destroy(oldLibrary);

			foreach (var clip in player.spriteAnimator.Library.clips)
			{
				for (int i = 0; i < clip.frames.Length; i++)
				{
					clip.frames[i].spriteCollection = copyCollection;
				}
			}

			copyCollection.name = player.OverrideDisplayName;

			player.primaryHand.sprite.Collection = copyCollection;
			player.secondaryHand.sprite.Collection = copyCollection;
			player.sprite.Collection = copyCollection;
		}






		public static tk2dSpriteCollectionData HandleAnimations2(tk2dBaseSprite sprite, List<Texture2D> playerSheet)
		{
			tk2dSpriteCollectionData orig = sprite.Collection;
			var copyCollection = GameObject.Instantiate(orig);
			GameObject.DontDestroyOnLoad(copyCollection);

			tk2dSpriteDefinition[] copyDefinitions = new tk2dSpriteDefinition[orig.spriteDefinitions.Length];
			for (int i = 0; i < copyCollection.spriteDefinitions.Length; i++)
			{
				copyDefinitions[i] = Copy(orig.spriteDefinitions[i]);
			}
			copyCollection.spriteDefinitions = copyDefinitions;
			{


				RuntimeAtlasPage page = new RuntimeAtlasPage();
				for (int i = 0; i < playerSheet.Count; i++)
				{
					var tex = playerSheet[i];

					float nw = (tex.width) / 16f;
					float nh = (tex.height) / 16f;

					var def = copyCollection.GetSpriteDefinition(tex.name);
					if (def != null)
					{

						if (def.boundsDataCenter != Vector3.zero)
						{
							var ras = page.Pack(tex);
							def.materialInst.mainTexture = ras.texture;
							def.uvs = ras.uvs;
							def.extractRegion = true;
							def.position0 = new Vector3(0, 0, 0);
							def.position1 = new Vector3(nw, 0, 0);
							def.position2 = new Vector3(0, nh, 0);
							def.position3 = new Vector3(nw, nh, 0);

							def.boundsDataCenter = new Vector2(nw / 2, nh / 2);
							def.untrimmedBoundsDataCenter = def.boundsDataCenter;

							def.boundsDataExtents = new Vector2(nw, nh);
							def.untrimmedBoundsDataExtents = def.boundsDataExtents;
						}
						else
						{
							def.ReplaceTexture(tex);
						}
					}
				}
				page.Apply();
			}
			return copyCollection;
		}












		public readonly string cosmoSpriteSheetName = "CakeMod/Resources/cosmosheet.png";
		public static Texture2D cosmoSpriteSheet;
		public static Texture2D PulpitTexture;
		public readonly string StringFloorSheet = "CakeMod/Resources/floorsheet.png";
		public static Texture2D FloorSheet;
		public List<Texture2D> TurtSprites;


		// call this when you want to change the player sprite, add an int variable if you want more than 2 sprites
	}


}

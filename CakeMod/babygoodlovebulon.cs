using System;
using System.Collections.Generic;
using ItemAPI;
using Dungeonator;
using UnityEngine;

namespace CakeMod
{
	
	public class BabyGoodLovebulon : CompanionItem
	{
		
		public static void Init()
		{
			string name = "Baby Good Lovebulon";
			string resourcePath = "CakeMod/Resources/Lovebulon/Idle/lovebulon_idle_003";
			GameObject gameObject = new GameObject();
			BabyGoodLovebulon babyGoodBlob = gameObject.AddComponent<BabyGoodLovebulon>();
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject);
			string shortDesc = "Charismatic";
			string longDesc = "A genetically modified blobulin that is now able to feel love. It simply wishes to spread it's joy via it's odd goop.";
			babyGoodBlob.SetupItem(shortDesc, longDesc, "cak");
			babyGoodBlob.quality = PickupObject.ItemQuality.A;
			babyGoodBlob.CompanionGuid = BabyGoodLovebulon.guid;
			babyGoodBlob.Synergies = new CompanionTransformSynergy[0];
			BabyGoodLovebulon.BuildPrefab();
		}

		
		public static void BuildPrefab()
		{
			
			bool flag = BabyGoodLovebulon.blobPrefab != null || CompanionBuilder.companionDictionary.ContainsKey(BabyGoodLovebulon.guid);
			if (flag)
			{
				ETGModConsole.Log("shit. this mod has fallen comrades, we must seek Round King", false);
			}
			else
			{
				BabyGoodLovebulon.blobPrefab = CompanionBuilder.BuildPrefab("blobuloveman", BabyGoodLovebulon.guid, BabyGoodLovebulon.spritePaths[0], new IntVector2(1, 0), new IntVector2(9, 9));
				BabyGoodLovebulon.RandomGoopTrailBehaviour randomGoopTrailBehaviour = BabyGoodLovebulon.blobPrefab.AddComponent<BabyGoodLovebulon.RandomGoopTrailBehaviour>();
				AIAnimator component = BabyGoodLovebulon.blobPrefab.GetComponent<AIAnimator>();
				component.MoveAnimation = new DirectionalAnimation
				{
					AnimNames = new string[]
					{
						"idle"
					},
					Type = DirectionalAnimation.DirectionType.None
				};
				component.IdleAnimation = component.MoveAnimation;
				bool flag2 = BabyGoodLovebulon.blobCollection == null;
				if (flag2)
				{
					BabyGoodLovebulon.blobCollection = SpriteBuilder.ConstructCollection(BabyGoodLovebulon.blobPrefab, "Lovebulon_Baby_Collection");
					UnityEngine.Object.DontDestroyOnLoad(BabyGoodLovebulon.blobCollection);
					for (int i = 0; i < BabyGoodLovebulon.spritePaths.Length; i++)
					{
						SpriteBuilder.AddSpriteToCollection(BabyGoodLovebulon.spritePaths[i], BabyGoodLovebulon.blobCollection);
					}
					SpriteBuilder.AddAnimation(randomGoopTrailBehaviour.spriteAnimator, BabyGoodLovebulon.blobCollection, new List<int>
					{
						0,
						1,
						2,
						3,
						4
					}, "idle", tk2dSpriteAnimationClip.WrapMode.Loop).fps = 8f;
				}
				BehaviorSpeculator component2 = BabyGoodLovebulon.blobPrefab.GetComponent<BehaviorSpeculator>();
				component2.MovementBehaviors.Add(new CompanionFollowPlayerBehavior
				{
					IdleAnimations = new string[]
					{
						"idle"
					}
				});
				component2.MovementBehaviors.Add(new SeekTargetBehavior
				{
					LineOfSight = false,
					StopWhenInRange = true,
					CustomRange = 1f
				});
				randomGoopTrailBehaviour.aiActor.MovementSpeed = 7f;
				UnityEngine.Object.DontDestroyOnLoad(BabyGoodLovebulon.blobPrefab);
				FakePrefab.MarkAsFakePrefab(BabyGoodLovebulon.blobPrefab);
				BabyGoodLovebulon.blobPrefab.SetActive(false);
				
			}
		}

		
		public static GameObject blobPrefab;

		
		private static readonly string guid = "baby_good_blob";

		private static string[] spritePaths = new string[]
		{
			"CakeMod/Resources/Lovebulon/Idle/lovebulon_idle_001",
			"CakeMod/Resources/Lovebulon/Idle/lovebulon_idle_002",
			"CakeMod/Resources/Lovebulon/Idle/lovebulon_idle_003",
			"CakeMod/Resources/Lovebulon/Idle/lovebulon_idle_004",
			"CakeMod/Resources/Lovebulon/Idle/lovebulon_idle_005",
		};

		// Token: 0x0400005E RID: 94
		private static tk2dSpriteCollectionData blobCollection;

		// Token: 0x0400005F RID: 95
		private static string[] goops = new string[]
		{
			"assets/data/goops/blobulongoop.asset",
			"assets/data/goops/napalmgoopthatworks.asset",
			"assets/data/goops/poison goop.asset"
		};

		// Token: 0x04000060 RID: 96
		private static Color[] tints = new Color[]
		{
			new Color(0.9f, 0.34f, 0.45f),
			new Color(1f, 0.5f, 0.35f),
			new Color(0.7f, 0.9f, 0.7f),
			new Color(0.9f, 0.4f, 0.8f)
		};

		// Token: 0x04000061 RID: 97
		private static List<GoopDefinition> goopDefs;

		// Token: 0x02000136 RID: 310
		public class RandomGoopTrailBehaviour : CompanionController
		{
			// Token: 0x06000758 RID: 1880 RVA: 0x0003EC88 File Offset: 0x0003CE88
			private void Start()
			{
				AssetBundle assetBundle = ResourceManager.LoadAssetBundle("shared_auto_001");
				BabyGoodLovebulon.goopDefs = new List<GoopDefinition>();
				foreach (string text in BabyGoodLovebulon.goops)
				{
					GoopDefinition goopDefinition;
					try
					{
						GameObject gameObject = assetBundle.LoadAsset(text) as GameObject;
						goopDefinition = gameObject.GetComponent<GoopDefinition>();
					}
					catch
					{
						goopDefinition = (assetBundle.LoadAsset(text) as GoopDefinition);
					}
					goopDefinition.name = text.Replace("assets/data/goops/", "").Replace(".asset", "");
					BabyGoodLovebulon.goopDefs.Add(goopDefinition);
				}
				List<GoopDefinition> goopDefs = BabyGoodLovebulon.goopDefs;
				PickupObject byId = PickupObjectDatabase.GetById(310);
				GoopDefinition item;
				if (byId == null)
				{
					item = null;
				}
				else
				{
					WingsItem component = byId.GetComponent<WingsItem>();
					item = ((component != null) ? component.RollGoop : null);
				}
				goopDefs.Add(item);
				this.SetGoopIndex(0);
				base.spriteAnimator.Play("idle");
			}

			
			private void FixedUpdate()
			{
				bool flag = Time.time - this.lastSwitch > 10f;
				if (flag)
				{
					this.SetGoopIndex(UnityEngine.Random.Range(1, BabyGoodLovebulon.goopDefs.Count));
					this.lastSwitch = Time.time;
					AIActor aiActor = base.aiActor;
					PlayerController owner = this.m_owner;
					SpeculativeRigidbody overrideTarget;
					if (owner == null)
					{
						overrideTarget = null;
					}
					else
					{
						RoomHandler currentRoom = owner.CurrentRoom;
						if (currentRoom == null)
						{
							overrideTarget = null;
						}
						else
						{
							AIActor randomActiveEnemy = currentRoom.GetRandomActiveEnemy(false);
							overrideTarget = ((randomActiveEnemy != null) ? randomActiveEnemy.specRigidbody : null);
						}
					}
					aiActor.OverrideTarget = overrideTarget;
				}
				bool flag2 = !this.m_owner.IsInCombat;
				if (flag2)
				{
					this.SetGoopIndex(0);
				}

				float num = 1.2f;
				bool flag3 = PassiveItem.IsFlagSetForCharacter(this.m_owner, typeof(BattleStandardItem));
				if (flag3)
				{
					num *= 2f;
				}
				if (this.m_owner.PlayerHasActiveSynergy("Bubble Love"))

				{
					num *= 2f;
				}
				base.aiActor.ApplyEffect(charmEffect, 0.1f, null);
				DeadlyDeadlyGoopManager goopManagerForGoopType = DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(EasyGoopDefinitions.CharmGoopDef);
				goopManagerForGoopType.AddGoopCircle(base.sprite.WorldCenter, num, -1, false, -1);
			}

			
			private void SetGoopIndex(int index)
			{
				this.goopIndex = index;
				this.currentGoop = BabyGoodLovebulon.goopDefs[index];
				this.tint = BabyGoodLovebulon.tints[index];
			}

			
			private int goopIndex;

			
			private float lastSwitch = 0f;

			
			private const float switchTime = 10f;

			
			private GoopDefinition currentGoop;

			
			private Color tint = Color.white;
		}
		public static GameActorCharmEffect charmEffect;
	}
}
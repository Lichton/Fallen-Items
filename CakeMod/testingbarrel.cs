using Dungeonator;
using ItemAPI;
using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;


namespace CakeMod
{
	class testbarrel : PlayerItem
	{
		public static void Init()
		{
			string itemName = "Barrel O' Barrels";
			string resourcePath = "CakeMod/Resources/barrelbarrel";
			GameObject gameObject = new GameObject(itemName);
			testbarrel item = gameObject.AddComponent<testbarrel>();
			ItemBuilder.AddSpriteToObject(itemName, resourcePath, gameObject);
			string shortDesc = "Barrel To The Knee";
			string longDesc = "This barrel is fueled internally by the tortured souls of the Jammed. Despite having unlimited power, it can only create barrels.";
			item.SetupItem(shortDesc, longDesc, "cak");
			item.SetCooldownType(ItemBuilder.CooldownType.Timed, 1f);
			item.quality = ItemQuality.D;
			List<string> mandatoryConsoleIDs = new List<string>
			{
				"cak:barrel_o'_barrels",
				"lil_bomber",
			};
			CustomSynergies.Add("Mad Bomber", mandatoryConsoleIDs, null, true);
		}


		protected override void DoEffect(PlayerController user)
		{
			base.DoEffect(user);
			AkSoundEngine.PostEvent("m_ENM_amulet_conjure_01", base.gameObject);
			if (user.CurrentRoom != null)
			{
				float roomPosX = user.transform.position.x - user.CurrentRoom.area.basePosition.x;
				float roomPosY = user.transform.position.y - user.CurrentRoom.area.basePosition.y;
				float xOffSet = 0;
				float yOffSet = 0;
				float thexOffSet = 0;
				float theyOffSet = 0;
				float offsetAmount = 2f;
				float gunCurrentAngle = BraveMathCollege.Atan2Degrees(this.LastOwner.unadjustedAimPoint.XY() - this.LastOwner.CenterPosition);
				if (gunCurrentAngle > 45f && gunCurrentAngle <= 135f)
				{
					yOffSet = offsetAmount;//up
				}
				else if ((gunCurrentAngle > 0 && gunCurrentAngle > 135f) || (gunCurrentAngle < 0 && gunCurrentAngle <= -135f))
				{
					xOffSet = -offsetAmount;//left
				}
				else if (gunCurrentAngle > -135f && gunCurrentAngle <= -45f)
				{
					yOffSet = -offsetAmount;//bottom
				}
				else
				{
					xOffSet = offsetAmount;//right
				}

				if (gunCurrentAngle > 45f && gunCurrentAngle <= 135f)
				{
					theyOffSet = -offsetAmount;//up
				}
				else if ((gunCurrentAngle > 0 && gunCurrentAngle > 135f) || (gunCurrentAngle < 0 && gunCurrentAngle <= -135f))
				{
					thexOffSet = offsetAmount;//left
				}
				else if (gunCurrentAngle > -135f && gunCurrentAngle <= -45f)
				{
					theyOffSet = offsetAmount;//bottom
				}
				else
				{
					thexOffSet = -offsetAmount;//right
				}


				Vector2 posInCurrentRoom = new Vector2(roomPosX + xOffSet, roomPosY + yOffSet);
				Vector2 posInCurrentRoom2 = new Vector2(roomPosX + thexOffSet, roomPosY + theyOffSet);
				Vector2 posInMap = new Vector2(user.transform.position.x + xOffSet, user.transform.position.y + yOffSet).ToIntVector2().ToVector2();
				Vector2 posInMap2 = new Vector2(user.transform.position.x + thexOffSet, user.transform.position.y + theyOffSet).ToIntVector2().ToVector2();
				if (user.IsValidPlayerPosition(posInMap))
				{
					AssetBundle sharedAssets2d = ResourceManager.LoadAssetBundle("shared_auto_002");
					AssetBundle sharedAssets2 = ResourceManager.LoadAssetBundle("shared_auto_001");
					if (user.HasPickupID(332))
					{
						DungeonPlaceable ExplodyBarreld = sharedAssets2d.LoadAsset<DungeonPlaceable>("ExplodyBarrel_Maybe");
						GameObject spawnedDrumd = ExplodyBarreld.InstantiateObject(user.CurrentRoom, posInCurrentRoom2.ToIntVector2());
					};

					DungeonPlaceable ExplodyBarrel = sharedAssets2.LoadAsset<DungeonPlaceable>("Barrel_collection");
					GameObject spawnedDrum = ExplodyBarrel.InstantiateObject(user.CurrentRoom, posInCurrentRoom.ToIntVector2());
					}
				}
			}

		public override void Update()
		{
			base.Update();
		}
	}
}
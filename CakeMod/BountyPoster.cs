using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Dungeonator;
using ItemAPI;
using SaveAPI;

namespace CakeMod
{
    class BountyPoster : PassiveItem
	{
	
		public static void Init()
		{
			string name = "Bounty Poster";
			string resourcePath = "CakeMod/Resources/BountyPoster";
			GameObject gameObject = new GameObject(name);
			BountyPoster item = gameObject.AddComponent<BountyPoster>();
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject);
			string shortDesc = "Dead or Alive";
			string longDesc = "A special request from Kaliber herself...\n\n" +
				"Inflicts random enemies with rankings. The higer the ranking, the higher the reward. \n" +
				"Whoever wields this poster will gain a thirst for hunting.";
			item.SetupItem(shortDesc, longDesc, "cak");
			item.quality = PickupObject.ItemQuality.B;
			item.PlaceItemInAmmonomiconAfterItemById(465);
			item.AddItemToTrorcMetaShop(100, 100);
			item.SetupUnlockOnCustomFlag(CustomDungeonFlags.EXAMPLE_BLUEPRINTTRUCK, true);
			item.AddToSubShop(ItemBuilder.ShopType.Trorc, 1f);


		}


		private void Bounty()
		{
			int bighead = UnityEngine.Random.Range(1, 5);
			if (bighead == 1)
			{
				RoomHandler absoluteRoom = base.transform.position.GetAbsoluteRoom();
				AIActor randomActiveEnemy = absoluteRoom.GetRandomActiveEnemy(false);
				randomActiveEnemy.aiActor.ApplyEffect(HelpfulLibrary.Money, 9999f, null);
			}
			if (bighead == 2)
			{
				RoomHandler absoluteRoom = base.transform.position.GetAbsoluteRoom();
				AIActor randomActiveEnemy = absoluteRoom.GetRandomActiveEnemy(false);
				randomActiveEnemy.aiActor.ApplyEffect(HelpfulLibrary.Money2, 9999f, null);
			}
			if(bighead == 3)
            {
				RoomHandler absoluteRoom = base.transform.position.GetAbsoluteRoom();
				AIActor randomActiveEnemy = absoluteRoom.GetRandomActiveEnemy(false);
				randomActiveEnemy.aiActor.ApplyEffect(HelpfulLibrary.hegymoney, 9999f, null);
			}
			if (bighead == 4)
			{
				RoomHandler absoluteRoom = base.transform.position.GetAbsoluteRoom();
				AIActor randomActiveEnemy = absoluteRoom.GetRandomActiveEnemy(false);
				randomActiveEnemy.aiActor.ApplyEffect(HelpfulLibrary.moneydebuff3, 9999f, null);
			}
		}

		
		public override void Pickup(PlayerController player)
		{
			base.Pickup(player);
			player.OnEnteredCombat += this.Bounty;
			PlayerController owner = base.Owner;
			
		}

		
		public override DebrisObject Drop(PlayerController player)
		{
			DebrisObject result = base.Drop(player);
			player.OnEnteredCombat -= this.Bounty;
			PlayerController owner = base.Owner;
			
			return result;
		}

		public AIActorDebuffEffect BountyWorth = new AIActorDebuffEffect
		{
			HealthMultiplier = 1.5f,
			CooldownMultiplier = 0.5f,
			SpeedMultiplier = 0.7f,
			duration = 10f

		};
	}
}

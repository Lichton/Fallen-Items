using System;
using ItemAPI;
using UnityEngine;
using Dungeonator;
using System.Collections;

namespace CakeMod
{

	internal class DevilContract : PassiveItem
	{
		


	
		public static void Init()
		{
			string name = "Chest Devil's Contract";
			string resourcePath = "CakeMod/Resources/DevilContract";
			GameObject gameObject = new GameObject(name);
			DevilContract petRock = gameObject.AddComponent<DevilContract>();
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject);
			string shortDesc = "Fine Print";
			string longDesc = "A devilish contract with Beezlebox, it offers infinite keys in exchange for no money, no blanks on the start of floors, limited armour, and a devilish surprise..";
			petRock.SetupItem(shortDesc, longDesc, "cak");
			petRock.quality = PickupObject.ItemQuality.EXCLUDED;
			petRock.sprite.IsPerpendicular = true;
			petRock.CanBeDropped = false;
			CakeIDs.DevilContract = petRock.PickupObjectId;
			petRock.PreventStartingOwnerFromDropping = true;
		}
		protected override void Update()
		{
			if (Owner.characterIdentity == PlayableCharacters.Robot)
			{
				Owner.carriedConsumables.KeyBullets = 0;
				Owner.carriedConsumables.Currency = 0;
			}
            else
            {
				
				Owner.carriedConsumables.KeyBullets = 0;
				Owner.carriedConsumables.Currency = 0;
				if (Owner.healthHaver.Armor > 1)
				{
					Owner.healthHaver.Armor = 1;
				}
				
			}
			base.Update();
		}

		private void Hackerman()
		{
			int bighead = UnityEngine.Random.Range(1, 26);
			if (bighead == 1)
			{
				AIActor orLoadByGuid = EnemyDatabase.GetOrLoadByGuid("5f3abc2d561b4b9c9e72b879c6f10c7e");
				IntVector2? intVector = new IntVector2?(base.Owner.CurrentRoom.GetRandomVisibleClearSpot(2, 2));
				AIActor aiactor = AIActor.Spawn(orLoadByGuid.aiActor, intVector.Value, GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(intVector.Value), true, AIActor.AwakenAnimationType.Default, true);
				aiactor.BecomeBlackPhantom();
			}
			if (bighead == 2)
			{
				AIActor orLoadByGuid = EnemyDatabase.GetOrLoadByGuid("88f037c3f93b4362a040a87b30770407");
				IntVector2? intVector = new IntVector2?(base.Owner.CurrentRoom.GetRandomVisibleClearSpot(2, 2));
				
				AIActor aiactor = AIActor.Spawn(orLoadByGuid.aiActor, intVector.Value, GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(intVector.Value), true, AIActor.AwakenAnimationType.Default, true);
				aiactor.BecomeBlackPhantom();
			}
			if (bighead == 3)
			{
				AIActor orLoadByGuid = EnemyDatabase.GetOrLoadByGuid("e21ac9492110493baef6df02a2682a0d");
				IntVector2? intVector = new IntVector2?(base.Owner.CurrentRoom.GetRandomVisibleClearSpot(2, 2));

				AIActor aiactor = AIActor.Spawn(orLoadByGuid.aiActor, intVector.Value, GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(intVector.Value), true, AIActor.AwakenAnimationType.Default, true);
				aiactor.BecomeBlackPhantom();
			}
			if (bighead == 4)
			{
				AIActor orLoadByGuid = EnemyDatabase.GetOrLoadByGuid("249db525a9464e5282d02162c88e0357");
				IntVector2? intVector = new IntVector2?(base.Owner.CurrentRoom.GetRandomVisibleClearSpot(2, 2));
			
				AIActor aiactor = AIActor.Spawn(orLoadByGuid.aiActor, intVector.Value, GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(intVector.Value), true, AIActor.AwakenAnimationType.Default, true);
				aiactor.BecomeBlackPhantom();
			}
		}

		private IEnumerator Grow(AIActor randomActiveEnemy)
		{
			Vector2 startScale = randomActiveEnemy.EnemyScale;
			randomActiveEnemy.EnemyScale = Vector2.Lerp(startScale, this.TargetScale, 1f);

			yield break;
		}

		private IEnumerator Shrink(AIActor randomActiveEnemy)
		{
			Vector2 startScale = randomActiveEnemy.EnemyScale;
			randomActiveEnemy.EnemyScale = Vector2.Lerp(startScale, this.TargetScale2, 1f);

			yield break;
		}
		public override void Pickup(PlayerController player)
		{
			GameManager.Instance.OnNewLevelFullyLoaded += this.DeviledEgg;
			player.OnEnteredCombat += this.BulbaQuake;
			base.Pickup(player);
			
		}

        private void DeviledEgg()
        {
			Owner.Blanks = 0;
			if (GameManager.Instance.Dungeon.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.MINEGEON)
            {
				PlayerController player = GameManager.Instance.PrimaryPlayer;
				LootEngine.TryGivePrefabToPlayer(PickupObjectDatabase.GetById(316).gameObject, player, true);
            }
			
		}

        private void BulbaQuake()
		{
			int bighead = UnityEngine.Random.Range(1, 6);
			if (bighead == 1)
			{
				RoomHandler absoluteRoom = base.transform.position.GetAbsoluteRoom();
				AIActor randomActiveEnemy = absoluteRoom.GetRandomActiveEnemy(false);
				if (randomActiveEnemy.healthHaver.IsBoss != true)
				{
					randomActiveEnemy.aiActor.ApplyEffect(HelpfulLibrary.Demon, 9999f, null);
				}
			}
		}

        public override DebrisObject Drop(PlayerController player)
		{
			player.OnEnteredCombat -= this.BulbaQuake;
			Owner.carriedConsumables.InfiniteKeys = true;
			return base.Drop(player);
		}
		public Vector2 TargetScale = new Vector2(1.5f, 1.5f);
		public Vector2 TargetScale2 = new Vector2(0.5f, 0.5f);
	}
}
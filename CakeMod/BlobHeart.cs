using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;
using Dungeonator;
using SaveAPI;
using Gungeon;

namespace CakeMod
{

    class BlobHeart : PassiveItem
    {

		public static void Init()
		{

			string itemName = "Blob Heart";
			string resourceName = "CakeMod/Resources/BlobHeart";

			GameObject obj = new GameObject(itemName);
			var item = obj.AddComponent<BlobHeart>();

			ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

			string shortDesc = "Blobulove";
			string longDesc = "The heart of the blobulord. Some could say red is the color of love.\n" +
				"The blobulord thought otherwise.";
			ItemBuilder.SetupItem(item, shortDesc, longDesc, "cak");
			item.AddItemToGooptonMetaShop(30, 50);
			item.SetupUnlockOnCustomFlag(CustomDungeonFlags.EXAMPLE_BLUEPRINTGOOP, true);
			ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Health, 1f, StatModifier.ModifyMethod.ADDITIVE);
			item.AddToSubShop(ItemBuilder.ShopType.Goopton, 1f);
			item.quality = PickupObject.ItemQuality.C;
			item.PlaceItemInAmmonomiconAfterItemById(425);
			BlobHeart.goopDefs = new List<GoopDefinition>();
			AssetBundle assetBundle = ResourceManager.LoadAssetBundle("shared_auto_001");
			foreach (string text in BlobHeart.goops)
			{
				GoopDefinition goopDefinition;
				try
				{
					GameObject gameObject2 = assetBundle.LoadAsset(text) as GameObject;
					goopDefinition = gameObject2.GetComponent<GoopDefinition>();
				}
				catch
				{
					goopDefinition = (assetBundle.LoadAsset(text) as GoopDefinition);
				}
				goopDefinition.name = text.Replace("assets/data/goops/", "").Replace(".asset", "");
				BlobHeart.goopDefs.Add(goopDefinition);
			}
		}

        private static string[] goops = new string[]
        {
            "assets/data/goops/blobulongoop.asset",
        };
		private static List<GoopDefinition> goopDefs;

        protected override void Update()
		{
			base.Update();
			this.DoGoop();
        }
		private void Start()
		{
			this.SetGoopIndex(UnityEngine.Random.Range(1, BlobHeart.goopDefs.Count));
			AssetBundle assetBundle = ResourceManager.LoadAssetBundle("shared_auto_001");
			BlobHeart.goopDefs = new List<GoopDefinition>();
			foreach (string text in BlobHeart.goops)
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
				BlobHeart.goopDefs.Add(goopDefinition);
			}
			GoopDefinition item;
			item = null;
			List<GoopDefinition> goopDefs = BlobHeart.goopDefs;
			goopDefs.Add(item);
			this.SetGoopIndex(0);
			base.sprite.color = BlobHeart.tints[0];
			base.sprite.color = Color.Lerp(base.sprite.color, this.tint, 0.1f);
			base.spriteAnimator.Play("idle");
		}

		public override void Pickup(PlayerController player)
        {
			base.Pickup(player);
			base.Owner.OnReceivedDamage += this.DoBigGoop;
				this.DoGoop();
		}

		private void DoGoop()
		{
			float time = 0;
			if (time < .005f)
			{
				time += BraveTime.DeltaTime;
			}
			if (time > .005f)
			{
				DeadlyDeadlyGoopManager goopManagerForGoopType = DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(goopDefs[0]);
				goopManagerForGoopType.TimedAddGoopCircle(this.m_owner.specRigidbody.UnitCenter, 1, .2f);
			}
		}

		private static Color[] tints = new Color[]
		{
			new Color(0.9f, 0.34f, 0.45f),
			new Color(1f, 0.5f, 0.35f),
			new Color(0.7f, 0.9f, 0.7f),
			new Color(0.9f, 0.4f, 0.8f)
		};
		private void SetGoopIndex(int index)
		{
			this.goopIndex = index;
			this.currentGoop = BlobHeart.goopDefs[index];
			this.tint = BlobHeart.tints[index];
		}

		public override DebrisObject Drop(PlayerController player)
        {
			base.Owner.OnReceivedDamage -= this.DoBigGoop;
			return base.Drop(player);
			
		}
		private int goopIndex;

		private GoopDefinition currentGoop;

		private Color tint = Color.white;

		public void DoBigGoop(PlayerController user)
		{
			DeadlyDeadlyGoopManager goopManagerForGoopType = DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(goopDefs[0]);
			goopManagerForGoopType.AddGoopCircle(this.m_owner.specRigidbody.UnitCenter, 4f, -4, false, -4);
		}
	}
}


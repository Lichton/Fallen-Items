using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;
using Dungeonator;
using SaveAPI;
using Gungeon;
using System.Collections;
using System.Reflection;
using MonoMod.RuntimeDetour;


namespace CakeMod
{

	class WebAmmolet : BlankModificationItem
	{

		public static void Init()
		{

			string itemName = "Web Ammolet";
			string resourceName = "CakeMod/Resources/Webbed_Ammolet";

			GameObject obj = new GameObject(itemName);
			var item = obj.AddComponent<WebAmmolet>();

			ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

			string shortDesc = "Rusted & Dusted";
			string longDesc = "An old, creaky wooden prototype of the modern ammolet.\n" +
				"It seems to be carved hollow, filled with some sort of thick webbing.";
			ItemBuilder.SetupItem(item, shortDesc, longDesc, "cak");
			item.quality = PickupObject.ItemQuality.EXCLUDED;

			WebAmmolet.goopDefs = new List<GoopDefinition>();
			AssetBundle assetBundle = ResourceManager.LoadAssetBundle("shared_auto_001");
			foreach (string text in WebAmmolet.goops)
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
				WebAmmolet.goopDefs.Add(goopDefinition);
			}
			WebAmmolet.roundID = item.PickupObjectId;
		}

		private static string[] goops = new string[]
		{
			"assets/data/goops/phasewebgoop.asset",
		};
		private static List<GoopDefinition> goopDefs;

		protected override void Update()
		{
			bool flag = base.Owner;
			base.Update();

		}
		private void Start()
		{
			this.SetGoopIndex(UnityEngine.Random.Range(1, WebAmmolet.goopDefs.Count));
			AssetBundle assetBundle = ResourceManager.LoadAssetBundle("shared_auto_001");
			WebAmmolet.goopDefs = new List<GoopDefinition>();
			foreach (string text in WebAmmolet.goops)
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
				WebAmmolet.goopDefs.Add(goopDefinition);
			}
			GoopDefinition item;
			item = null;
			List<GoopDefinition> goopDefs = WebAmmolet.goopDefs;
			goopDefs.Add(item);
			this.SetGoopIndex(0);
			base.sprite.color = WebAmmolet.tints[0];
			base.sprite.color = Color.Lerp(base.sprite.color, this.tint, 0.1f);
			base.spriteAnimator.Play("idle");
		}

		public override void Pickup(PlayerController player)
		{
			base.Pickup(player);
		}
		public static void BlankModHook(Action<SilencerInstance, BlankModificationItem, Vector2, PlayerController> orig, SilencerInstance silencer, BlankModificationItem bmi, Vector2 centerPoint, PlayerController user)
		{

			orig(silencer, bmi, centerPoint, user);
			bool flag = user.HasPickupID(WebAmmolet.roundID);
			if (flag)
			{
				DeadlyDeadlyGoopManager goopManagerForGoopType = DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(goopDefs[0]);
				goopManagerForGoopType.TimedAddGoopCircle(user.sprite.WorldCenter, 3f, 0.35f, false);
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
			this.currentGoop = WebAmmolet.goopDefs[index];
			this.tint = WebAmmolet.tints[index];
		}

		public override DebrisObject Drop(PlayerController player)
		{
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
		private static int roundID;
		private DamageTypeModifier slowImmune;
		private static Hook BlankHook = new Hook(typeof(SilencerInstance).GetMethod("ProcessBlankModificationItemAdditionalEffects", BindingFlags.Instance | BindingFlags.NonPublic), typeof(WebAmmolet).GetMethod("BlankModHook"));

	}

}

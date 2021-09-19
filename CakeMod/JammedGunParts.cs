using Dungeonator;
using ItemAPI;
using UnityEngine;
using System.Collections;
using Gungeon;
using MonoMod;
using BasicGun;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Reflection;
using MonoMod.RuntimeDetour;

namespace CakeMod
{
	class JammedGunParts : PassiveItem
	{

		public static void Init()
		{
			string itemName = "Gold Paint";
			string resourceName = "CakeMod/Resources/GoldPaint";

			GameObject obj = new GameObject(itemName);
			var item = obj.AddComponent<JammedGunParts>();

			ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

			string shortDesc = "All That Glitters Is Gold";
			string longDesc = "Makes guns golden as well as higher quality.";

			ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Damage, 1.2f, StatModifier.ModifyMethod.MULTIPLICATIVE);
			ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.ReloadSpeed, 1.2f, StatModifier.ModifyMethod.MULTIPLICATIVE);
			ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.RateOfFire, 1.2f, StatModifier.ModifyMethod.MULTIPLICATIVE);




			ItemBuilder.SetupItem(item, shortDesc, longDesc, "cak");

			item.quality = PickupObject.ItemQuality.EXCLUDED;

		}

		protected override void Update()
		{
		
		}

       

        public override void Pickup(PlayerController player)
		{
			
			Aguny = Diebuster;
			base.Pickup(player);
			player.GunChanged += this.HandleGunChanged;
			if (player.CurrentGun)
			{
				this.ProcessGunShader(player.CurrentGun);
			}
		}

    

        public override DebrisObject Drop(PlayerController player)
		{
			player.GunChanged -= this.HandleGunChanged;
			player.ClearOverrideShader();
			this.RemoveGunShader(player.CurrentGun);
			return base.Drop(player);
		}

   

	

		
		

		private Shader m_glintShader = ShaderCache.Acquire("Brave/ItemSpecific/LootGlintAdditivePass");

		public bool BreakCheck = true;

		private void HandleGunChanged(Gun oldGun, Gun newGun, bool arg3)
		{
			this.RemoveGunShader(oldGun);
			this.ProcessGunShader(newGun);
		}

		private void RemoveGunShader(Gun g)
		{
			if (!g)
			{
				return;
			}
			MeshRenderer component = g.GetComponent<MeshRenderer>();
			if (!component)
			{
				return;
			}
			Material[] sharedMaterials = component.sharedMaterials;
			List<Material> list = new List<Material>();
			for (int i = 0; i < sharedMaterials.Length; i++)
			{
				if (sharedMaterials[i].shader != this.m_glintShader)
				{
					list.Add(sharedMaterials[i]);
				}
			}
			component.sharedMaterials = list.ToArray();
		}

		// Token: 0x0600733D RID: 29501 RVA: 0x002CEDAC File Offset: 0x002CCFAC
		private void ProcessGunShader(Gun g)
		{
			MeshRenderer component = g.GetComponent<MeshRenderer>();
			if (!component)
			{
				return;
			}
			Material[] sharedMaterials = component.sharedMaterials;
			for (int i = 0; i < sharedMaterials.Length; i++)
			{
				if (sharedMaterials[i].shader == this.m_glintShader)
				{
					return;
				}
			}
			Array.Resize<Material>(ref sharedMaterials, sharedMaterials.Length + 1);
			Material material = new Material(this.m_glintShader);
			material.SetTexture("_MainTex", sharedMaterials[0].GetTexture("_MainTex"));
			sharedMaterials[sharedMaterials.Length - 1] = material;
			component.sharedMaterials = sharedMaterials;
			material.DisableKeyword("TINTING_OFF");
			material.EnableKeyword("TINTING_ON");
			material.SetColor("_OverrideColor", new Color(1f, 0.768f, 0f));
			material.DisableKeyword("EMISSIVE_OFF");
			material.EnableKeyword("EMISSIVE_ON");
			material.SetFloat("_EmissivePower", 1.75f);
			material.SetFloat("_EmissiveColorPower", 1f);
		}
		public bool ShouldIMoney = true;
		public static PlayerController player = GameManager.Instance.PrimaryPlayer;
		public int Diebuster;
		public int Aguny;
	}
}
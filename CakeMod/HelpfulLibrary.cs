using System;
using ItemAPI;
using UnityEngine;
using CakeMod;
using GungeonAPI;

namespace CakeMod
{
	// Token: 0x02000059 RID: 89
	public static class HelpfulLibrary
	{
		public static GameObject LimeVFXObject;
		public static void BuildPrefab()
		{
			LimeVFXObject = SpriteBuilder.SpriteFromResource("CakeMod/Resources/Lime", new GameObject("FrailtyIcon"));
			LimeVFXObject.SetActive(false);
			tk2dBaseSprite vfxSprite = LimeVFXObject.GetComponent<tk2dBaseSprite>();
			vfxSprite.GetCurrentSpriteDef().ConstructOffsetsFromAnchor(tk2dBaseSprite.Anchor.LowerCenter, vfxSprite.GetCurrentSpriteDef().position3);
			FakePrefab.MarkAsFakePrefab(LimeVFXObject);
			UnityEngine.Object.DontDestroyOnLoad(LimeVFXObject);
		}

		// Token: 0x0600025F RID: 607 RVA: 0x00018B90 File Offset: 0x00016D90

		// Token: 0x04000117 RID: 279
		public static AppliedPiss appliedPiss = new AppliedPiss
		{
			duration = 7.5f,
			effectIdentifier = "AppliedPiss",
			
			AffectsEnemies = true,
			TintColor = new Color(0.89803921568f, 0.7058824f, 0.42352941176f),
			AppliesDeathTint = true,
			DeathTintColor = new Color(0.89803921568f, 0.94117647058f, 0.42352941176f),
			AppliesTint = true
		};
		public static Color pissYellow = new Color(0.89803921568f, 0.94117647058f, 0.42352941176f);

		public static GameActorHealthEffect Lime = new GameActorHealthEffect
		{
			TintColor = new Color(0.152f, 1.086f, 0.054f),
			DeathTintColor = new Color(0.152f, 1.086f, 0.0549f),
			AppliesTint = true,
		
		    AppliesDeathTint = true,
			AffectsEnemies = true,
			AffectsPlayers = false,
			DamagePerSecondToEnemies = 5f,
			duration = 6f,
			effectIdentifier = "Lime"
			
		};
		public static AIActorDebuffEffect Stoned = new AIActorDebuffEffect
		{
			TintColor = Color.grey,
			DeathTintColor = Color.grey,
			AppliesTint = true,
			SpeedMultiplier = 0.7f,
			AppliesDeathTint = true,
			AffectsEnemies = true,
			AffectsPlayers = false,
			OutlineTintColor = Color.grey,
			duration = 6f,
			effectIdentifier = "Stoned"

		};

		public static MoneyDebuffEffect Money = new MoneyDebuffEffect
		{
			DamagePerSecondToEnemies = 0f,
			effectIdentifier = "Money",
			AppliesTint = false,
			AffectsEnemies = true,	
			duration = 99999,
			PlaysVFXOnActor = true

		};

		public static DemonBuff Demon = new DemonBuff
		{
			TintColor = Color.red,
			DeathTintColor = Color.red,
			effectIdentifier = "Demon",
			AppliesTint = true,
			AffectsEnemies = true,
			duration = 999999,
			PlaysVFXOnActor = true
			

		};
		public static money22DebuffEffect Money2 = new money22DebuffEffect
		{
			DamagePerSecondToEnemies = 0f,
			effectIdentifier = "Money2",
			AppliesTint = false,
			AffectsEnemies = true,
			duration = 99999,
			PlaysVFXOnActor = true

		};
		public static hegemonyDebuffEffect hegymoney = new hegemonyDebuffEffect
		{
			DamagePerSecondToEnemies = 0f,
			effectIdentifier = "hegymony",
			AppliesTint = false,
			AffectsEnemies = true,
			duration = 99999,
			PlaysVFXOnActor = true

		};

		public static ghost_statusDebuffEffect ghoststatuseffect = new ghost_statusDebuffEffect
		{
			DamagePerSecondToEnemies = 0f,
			effectIdentifier = "ghosteffect",
			AppliesTint = false,
			AffectsEnemies = true,
			duration = 99999,
			PlaysVFXOnActor = true

		};

		public static moneydebuff3DebuffEffect moneydebuff3 = new moneydebuff3DebuffEffect
		{
			DamagePerSecondToEnemies = 0f,
			effectIdentifier = "moneydebuff3",
			AppliesTint = false,
			AffectsEnemies = true,
			duration = 99999,
			PlaysVFXOnActor = true

		};

		// Token: 0x04000114 RID: 276
		public static GoopDefinition LimeGoop = new GoopDefinition
		{
			CanBeIgnited = false,
			damagesEnemies = false,
			damagesPlayers = false,
			
			baseColor32 = new Color32(2, 255, 51, 200),
			goopTexture = ResourceExtractor.GetTextureFromResource("CakeMod/Resources/goop_standard_base_001.png"),
			AppliesDamageOverTime = true,
			HealthModifierEffect = HelpfulLibrary.Lime,
			lifespan = 10f,

		};

		public static inkDebuffEffect Inked = new inkDebuffEffect
		{
			TintColor = new Color(0f, 0f, 0f),
			DeathTintColor = new Color(0f, 0f, 0f),
			AppliesTint = true,
			AffectsPlayers = false,
			AppliesDeathTint = true,
			AffectsEnemies = true,
			DamagePerSecondToEnemies = 0.5f,
			duration = 15f,
			effectIdentifier = "Inked"
		};


		public static GoopDefinition Ink = new GoopDefinition
		{
			CanBeIgnited = false,
			damagesEnemies = false,
			damagesPlayers = false,
			baseColor32 = new Color32(1, 1, 1, 200),
			goopTexture = ResourceExtractor.GetTextureFromResource("CakeMod/Resources/goop_standard_base_001.png"),
			AppliesDamageOverTime = true,
			HealthModifierEffect = HelpfulLibrary.Inked,
			lifespan = 5f,
			isOily = true,
			CanBeFrozen = true
		};

		




		public static GoopDefinition CurseGoop = new GoopDefinition
		{
			CanBeIgnited = true,
			damagesEnemies = false,
			damagesPlayers = false,
			baseColor32 = new Color32(235, 235, 235, 150),
			goopTexture = ResourceExtractor.GetTextureFromResource("CakeMod/Resources/goop_standard_base_001.png"),
			AppliesDamageOverTime = true,
			HealthModifierEffect = HelpfulLibrary.Cursed,



		};

		public static GameActorHealthEffect Cursed = new GameActorHealthEffect
		{
			TintColor = new Color(0.15294117647f, 1.0862745098f, 0.05490196078f),
			DeathTintColor = new Color(0.15294117647f, 1.0862745098f, 0.05490196078f),
			AppliesTint = true,
			AppliesDeathTint = true,
			AffectsEnemies = true,
			AffectsPlayers = true,
			DamagePerSecondToEnemies = 5f,
			duration = 6f,
			effectIdentifier = "Cursed"
			
		};

        public static GameObject Vfxprefab { get; private set; }
    }


}


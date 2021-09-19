using System;
using UnityEngine;
using ItemAPI;
using System.Collections;

namespace CakeMod
{
	public class MoneyDebuffEffect : GameActorHealthEffect
	{
		public static string vfxNamemoney = "MoneyVFX";
		public static GameObject moneyVFXObject;
		public Color ClearUp = new Color(0f, 0f, 0f, 0f);
		public static void Init()
		{
			moneyVFXObject = SpriteBuilder.SpriteFromResource("CakeMod/Resources/MoneyDebuff", new GameObject("MoneyIcon"));
			moneyVFXObject.SetActive(false);
			tk2dBaseSprite vfxSprite = moneyVFXObject.GetComponent<tk2dBaseSprite>();
			vfxSprite.GetCurrentSpriteDef().ConstructOffsetsFromAnchor(tk2dBaseSprite.Anchor.LowerCenter, vfxSprite.GetCurrentSpriteDef().position3);
			FakePrefab.MarkAsFakePrefab(moneyVFXObject);
			UnityEngine.Object.DontDestroyOnLoad(moneyVFXObject);
		}


		public override void ApplyTint(GameActor actor)
		{
			for (int k = 0; k < 1; k++)
			{
				GameObject original;
				original = MoneyDebuffEffect.moneyVFXObject;
				tk2dSprite component = UnityEngine.Object.Instantiate<GameObject>(original, actor.transform).GetComponent<tk2dSprite>();
				component.name = MoneyDebuffEffect.vfxNamemoney;
				component.PlaceAtPositionByAnchor(actor.sprite.WorldTopCenter, tk2dBaseSprite.Anchor.LowerCenter);
				component.scale = Vector3.one;
				component.transform.position.WithZ(component.transform.position.z + 99999);
			}
		}
		public override void EffectTick(GameActor actor, RuntimeGameActorEffectData effectData)
		{

			if (this.AffectsEnemies && actor is AIActor && !actor.healthHaver.IsBoss)
			{
				actor.healthHaver.HasRatchetHealthBar = true;
				if (actor.healthHaver.IsDead)
				{
					if (ShouldISpawn == true)
					{
						LootEngine.SpawnItem(PickupObjectDatabase.GetById(70).gameObject, actor.specRigidbody.UnitCenter, actor.specRigidbody.UnitCenter, 1f, false, true, false);
						LootEngine.SpawnItem(PickupObjectDatabase.GetById(70).gameObject, actor.specRigidbody.UnitCenter, actor.specRigidbody.UnitCenter, 1f, false, true, false);
						ShouldISpawn = false;
				

					}
				}
			}
		}

		
		public Action<Vector2> MoneyMark(GameActor actor)
        {
			LootEngine.SpawnItem(PickupObjectDatabase.GetById(68).gameObject, actor.specRigidbody.UnitCenter, actor.specRigidbody.UnitCenter, 1f, false, true, false);
			throw new NotImplementedException();
		}
		
        public override void OnEffectApplied(GameActor actor, RuntimeGameActorEffectData effectData, float partialAmount = 1)
        {
			actor.healthHaver.OnDeath += effectData.OnActorPreDeath;
			actor.healthHaver.AllDamageMultiplier += -0.5f;
			base.OnEffectApplied(actor, effectData, partialAmount);
			ShouldISpawn = true;

		}
		
        public override void OnEffectRemoved(GameActor actor, RuntimeGameActorEffectData effectData)
		{
			var hand = actor.transform.Find("MoneyVFX").gameObject;
			UnityEngine.Object.Destroy(hand);

			actor.DeregisterOverrideColor(vfxNamemoney);
			base.OnEffectRemoved(actor, effectData);
			actor.healthHaver.AllDamageMultiplier -= -0.5f;
		}
		public bool ShouldISpawn = true;
	}
	
}

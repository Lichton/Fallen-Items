using System;
using UnityEngine;
using ItemAPI;

namespace CakeMod
{
	public class money22DebuffEffect : GameActorHealthEffect
	{
		public static string vfxNamemoney2 = "money22VFX";
		public static GameObject money2VFXObject;
		public Color ClearUp = new Color(0f, 0f, 0f, 0f);
		public static void Init()
		{
			money2VFXObject = SpriteBuilder.SpriteFromResource("CakeMod/Resources/money22Debuff", new GameObject("money22Icon"));
			money2VFXObject.SetActive(false);
			tk2dBaseSprite vfxSprite = money2VFXObject.GetComponent<tk2dBaseSprite>();
			vfxSprite.GetCurrentSpriteDef().ConstructOffsetsFromAnchor(tk2dBaseSprite.Anchor.LowerCenter, vfxSprite.GetCurrentSpriteDef().position3);
			FakePrefab.MarkAsFakePrefab(money2VFXObject);
			UnityEngine.Object.DontDestroyOnLoad(money2VFXObject);
		}


		public override void ApplyTint(GameActor actor)
		{
			for (int k = 0; k < 1; k++)
			{
				GameObject original;
				original = money22DebuffEffect.money2VFXObject;
				tk2dSprite component = UnityEngine.Object.Instantiate<GameObject>(original, actor.transform).GetComponent<tk2dSprite>();
				component.name = money22DebuffEffect.vfxNamemoney2;
				component.PlaceAtPositionByAnchor(actor.sprite.WorldTopCenter, tk2dBaseSprite.Anchor.LowerCenter);
				component.scale = Vector3.one;
				component.transform.position.WithZ(component.transform.position.z + 99999);
			}
		}
		public override void EffectTick(GameActor actor, RuntimeGameActorEffectData effectData)
		{

			if (this.AffectsEnemies && actor is AIActor && !actor.healthHaver.IsBoss)
			{
				if (actor.healthHaver.IsDead)
				{
					if (ShouldISpawn == true)
					{
						LootEngine.SpawnItem(PickupObjectDatabase.GetById(70).gameObject, actor.specRigidbody.UnitCenter, actor.specRigidbody.UnitCenter, 1f, false, true, false);
						ShouldISpawn = false;
					}
				}
			}
		}
		public Action<Vector2> money22Mark(GameActor actor)
		{
			LootEngine.SpawnItem(PickupObjectDatabase.GetById(68).gameObject, actor.specRigidbody.UnitCenter, actor.specRigidbody.UnitCenter, 1f, false, true, false);
			throw new NotImplementedException();
		}

		public override void OnEffectApplied(GameActor actor, RuntimeGameActorEffectData effectData, float partialAmount = 1)
		{
			actor.healthHaver.OnDeath += effectData.OnActorPreDeath;
			actor.healthHaver.AllDamageMultiplier += -0.3f;
			base.OnEffectApplied(actor, effectData, partialAmount);
			ShouldISpawn = true;
		}

		public override void OnEffectRemoved(GameActor actor, RuntimeGameActorEffectData effectData)
		{
			var hand = actor.transform.Find("money22VFX").gameObject;
			UnityEngine.Object.Destroy(hand);

			actor.DeregisterOverrideColor(vfxNamemoney2);
			base.OnEffectRemoved(actor, effectData);
			actor.healthHaver.AllDamageMultiplier -= -0.3f;
		}
		public bool ShouldISpawn = true;
	}

}
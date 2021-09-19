using System;
using UnityEngine;
using ItemAPI;

namespace CakeMod
{
	public class hegemonyDebuffEffect : GameActorHealthEffect
	{
		public static string vfxNamehegemony = "hegemonyVFX";
		public static GameObject hegemonyVFXObject;
		public Color ClearUp = new Color(0f, 0f, 0f, 0f);
		public static void Init()
		{
			hegemonyVFXObject = SpriteBuilder.SpriteFromResource("CakeMod/Resources/hegemoneybounty", new GameObject("hegemonyIcon"));
			hegemonyVFXObject.SetActive(false);
			tk2dBaseSprite vfxSprite = hegemonyVFXObject.GetComponent<tk2dBaseSprite>();
			vfxSprite.GetCurrentSpriteDef().ConstructOffsetsFromAnchor(tk2dBaseSprite.Anchor.LowerCenter, vfxSprite.GetCurrentSpriteDef().position3);
			FakePrefab.MarkAsFakePrefab(hegemonyVFXObject);
			UnityEngine.Object.DontDestroyOnLoad(hegemonyVFXObject);
		}


		public override void ApplyTint(GameActor actor)
		{
			for (int k = 0; k < 1; k++)
			{
				GameObject original;
				original = hegemonyDebuffEffect.hegemonyVFXObject;
				tk2dSprite component = UnityEngine.Object.Instantiate<GameObject>(original, actor.transform).GetComponent<tk2dSprite>();
				component.name = hegemonyDebuffEffect.vfxNamehegemony;
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
						LootEngine.SpawnItem(PickupObjectDatabase.GetById(297).gameObject, actor.specRigidbody.UnitCenter, actor.specRigidbody.UnitCenter, 1f, false, true, false);
						ShouldISpawn = false;
					}
				}
			}
		}
		public Action<Vector2> hegemonyMark(GameActor actor)
		{
			LootEngine.SpawnItem(PickupObjectDatabase.GetById(297).gameObject, actor.specRigidbody.UnitCenter, actor.specRigidbody.UnitCenter, 1f, false, true, false);
			throw new NotImplementedException();
		}

		public override void OnEffectApplied(GameActor actor, RuntimeGameActorEffectData effectData, float partialAmount = 1)
		{
			actor.healthHaver.OnDeath += effectData.OnActorPreDeath;
			actor.healthHaver.AllDamageMultiplier += -0.7f;
			base.OnEffectApplied(actor, effectData, partialAmount);
			ShouldISpawn = true;
		}

		public override void OnEffectRemoved(GameActor actor, RuntimeGameActorEffectData effectData)
		{
			var hand = actor.transform.Find("hegemonyVFX").gameObject;
			UnityEngine.Object.Destroy(hand);

			actor.DeregisterOverrideColor(vfxNamehegemony);
			base.OnEffectRemoved(actor, effectData);
			actor.healthHaver.AllDamageMultiplier -= -0.7f;
		}
		public bool ShouldISpawn = true;
	}

}
using System;
using UnityEngine;
using ItemAPI;

namespace CakeMod
{
	public class moneydebuff3DebuffEffect : GameActorHealthEffect
	{
		public static string vfxNamemoneydebuff3 = "moneydebuff3VFX";
		public static GameObject moneydebuff3VFXObject;
		public Color ClearUp = new Color(0f, 0f, 0f, 0f);
		public static void Init()
		{
			moneydebuff3VFXObject = SpriteBuilder.SpriteFromResource("CakeMod/Resources/moneydebuff3", new GameObject("moneydebuff3Icon"));
			moneydebuff3VFXObject.SetActive(false);
			tk2dBaseSprite vfxSprite = moneydebuff3VFXObject.GetComponent<tk2dBaseSprite>();
			vfxSprite.GetCurrentSpriteDef().ConstructOffsetsFromAnchor(tk2dBaseSprite.Anchor.LowerCenter, vfxSprite.GetCurrentSpriteDef().position3);
			FakePrefab.MarkAsFakePrefab(moneydebuff3VFXObject);
			UnityEngine.Object.DontDestroyOnLoad(moneydebuff3VFXObject);
		}


		public override void ApplyTint(GameActor actor)
		{
			for (int k = 0; k < 1; k++)
			{
				GameObject original;
				original = moneydebuff3DebuffEffect.moneydebuff3VFXObject;
				tk2dSprite component = UnityEngine.Object.Instantiate<GameObject>(original, actor.transform).GetComponent<tk2dSprite>();
				component.name = moneydebuff3DebuffEffect.vfxNamemoneydebuff3;
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
						LootEngine.SpawnItem(PickupObjectDatabase.GetById(68).gameObject, actor.specRigidbody.UnitCenter, actor.specRigidbody.UnitCenter, 1f, false, true, false);
						LootEngine.SpawnItem(PickupObjectDatabase.GetById(68).gameObject, actor.specRigidbody.UnitCenter, actor.specRigidbody.UnitCenter, 1f, false, true, false);
						LootEngine.SpawnItem(PickupObjectDatabase.GetById(68).gameObject, actor.specRigidbody.UnitCenter, actor.specRigidbody.UnitCenter, 1f, false, true, false);
						ShouldISpawn = false;
					}
				}
			}
		}
		public Action<Vector2> moneydebuff3Mark(GameActor actor)
		{
			LootEngine.SpawnItem(PickupObjectDatabase.GetById(68).gameObject, actor.specRigidbody.UnitCenter, actor.specRigidbody.UnitCenter, 1f, false, true, false);
			LootEngine.SpawnItem(PickupObjectDatabase.GetById(68).gameObject, actor.specRigidbody.UnitCenter, actor.specRigidbody.UnitCenter, 1f, false, true, false);
			LootEngine.SpawnItem(PickupObjectDatabase.GetById(68).gameObject, actor.specRigidbody.UnitCenter, actor.specRigidbody.UnitCenter, 1f, false, true, false);
			throw new NotImplementedException();
		}

		public override void OnEffectApplied(GameActor actor, RuntimeGameActorEffectData effectData, float partialAmount = 1)
		{
			actor.healthHaver.OnDeath += effectData.OnActorPreDeath;
			actor.healthHaver.AllDamageMultiplier += -0.2f;
			base.OnEffectApplied(actor, effectData, partialAmount);
			ShouldISpawn = true;
		}

		public override void OnEffectRemoved(GameActor actor, RuntimeGameActorEffectData effectData)
		{
			var hand = actor.transform.Find("moneydebuff3VFX").gameObject;
			UnityEngine.Object.Destroy(hand);

			actor.DeregisterOverrideColor(vfxNamemoneydebuff3);
			base.OnEffectRemoved(actor, effectData);
			actor.healthHaver.AllDamageMultiplier -= -0.2f;
		}
		public bool ShouldISpawn = true;
	}

}
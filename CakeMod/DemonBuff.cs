using System;
using UnityEngine;
using ItemAPI;
using System.Collections;

namespace CakeMod
{
	public class DemonBuff : AIActorBuffEffect
	{
		public static string vfxNamedemon = "DemonVFX";
		public static GameObject demonVFXObject;
		public Color ClearUp = new Color(0f, 0f, 0f, 0f);
		public static void Init()
		{
			demonVFXObject = SpriteBuilder.SpriteFromResource("CakeMod/Resources/DemonOverhead", new GameObject("DemonOverhead"));
			demonVFXObject.SetActive(false);
			tk2dBaseSprite vfxSprite = demonVFXObject.GetComponent<tk2dBaseSprite>();
			vfxSprite.GetCurrentSpriteDef().ConstructOffsetsFromAnchor(tk2dBaseSprite.Anchor.LowerCenter, vfxSprite.GetCurrentSpriteDef().position3);
			FakePrefab.MarkAsFakePrefab(demonVFXObject);
			UnityEngine.Object.DontDestroyOnLoad(demonVFXObject);
		}


		public override void ApplyTint(GameActor actor)
		{
			for (int k = 0; k < 1; k++)
			{
				GameObject original;
				original = DemonBuff.demonVFXObject;
				tk2dSprite component = UnityEngine.Object.Instantiate<GameObject>(original, actor.transform).GetComponent<tk2dSprite>();
				component.name = DemonBuff.vfxNamedemon;
				component.transform.position.WithZ(component.transform.position.z + 99999);
				component.PlaceAtPositionByAnchor(actor.sprite.WorldTopCenter, tk2dBaseSprite.Anchor.LowerCenter);
				component.scale = Vector3.one;
			}
		}
		public override void EffectTick(GameActor actor, RuntimeGameActorEffectData effectData)
		{
			i++;
			if(i == 180)
            {
				i = 0;
				num = actor.healthHaver.GetCurrentHealth();
				actor.healthHaver.ApplyHealing(num / 5);
				ETGModConsole.Log((num / 5).ToString());
            }
		}


		

		public override void OnEffectApplied(GameActor actor, RuntimeGameActorEffectData effectData, float partialAmount = 1)
		{
			
			base.OnEffectApplied(actor, effectData, partialAmount);

		}

		public override void OnEffectRemoved(GameActor actor, RuntimeGameActorEffectData effectData)
		{
			var hand = actor.transform.Find("DemonVFX").gameObject;
			UnityEngine.Object.Destroy(hand);
			actor.DeregisterOverrideColor(vfxNamedemon);
			base.OnEffectRemoved(actor, effectData);
			
		}
		int i = 0;
		float num;
	}

}

using System;
using UnityEngine;
using ItemAPI;

namespace CakeMod
{
	public class inkDebuffEffect : GameActorHealthEffect
	{
		public override void OnEffectApplied(GameActor actor, RuntimeGameActorEffectData effectData, float partialAmount = 1f)
		{
			base.OnEffectApplied(actor, effectData, partialAmount);
			actor.gameActor.ApplyEffect(this.gamer, 1f, null);

		}

		public AIActorDebuffEffect gamer = new AIActorDebuffEffect
		{
			SpeedMultiplier = 0.7f,
			KeepHealthPercentage = true,
			AppliesTint = true,
			TintColor = Color.black,
			duration = 5f
		};
		// Token: 0x0600019F RID: 415 RVA: 0x0001018E File Offset: 0x0000E38E
		public override void OnEffectRemoved(GameActor actor, RuntimeGameActorEffectData effectData)
		{
			base.OnEffectRemoved(actor, effectData);
			
		}
	}

}
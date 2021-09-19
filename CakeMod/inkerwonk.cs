using System;

namespace CakeMod
{
	// Token: 0x02000035 RID: 53
	public class inkerwonk : GameActorEffect
	{
		// Token: 0x0600019E RID: 414 RVA: 0x0001016A File Offset: 0x0000E36A
		public override void OnEffectApplied(GameActor actor, RuntimeGameActorEffectData effectData, float partialAmount = 1f)
		{
			base.OnEffectApplied(actor, effectData, partialAmount);
			actor.healthHaver.NextShotKills = true;
		}

		// Token: 0x0600019F RID: 415 RVA: 0x0001018E File Offset: 0x0000E38E
		public override void OnEffectRemoved(GameActor actor, RuntimeGameActorEffectData effectData)
		{
			base.OnEffectRemoved(actor, effectData);
			actor.healthHaver.AllDamageMultiplier -= 0.3f;
		}
	}
}
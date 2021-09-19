using System;
using UnityEngine;

// Token: 0x02001422 RID: 5154
public interface CursorOrbitalFloats
{
	// Token: 0x060074FE RID: 29950
	void Reinitialize();

	// Token: 0x060074FF RID: 29951
	Transform GetTransform();

	// Token: 0x06007500 RID: 29952
	void ToggleRenderer(bool visible);

	// Token: 0x06007501 RID: 29953
	int GetOrbitalTier();

	// Token: 0x06007502 RID: 29954
	void SetOrbitalTier(int tier);

	// Token: 0x06007503 RID: 29955
	int GetOrbitalTierIndex();

	// Token: 0x06007504 RID: 29956
	void SetOrbitalTierIndex(int tierIndex);

	// Token: 0x06007505 RID: 29957
	float GetOrbitalRadius();

	// Token: 0x06007506 RID: 29958
	float GetOrbitalRotationalSpeed();
}

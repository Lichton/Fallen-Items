using System;
using System.Collections;
using System.Collections.Generic;
using Gungeon;
using GungeonAPI;
using ItemAPI;
using UnityEngine;


namespace CakeMod
{
	// Token: 0x0200000E RID: 14
	public static class Pulpit
	{
		// Token: 0x06000059 RID: 89 RVA: 0x0000480C File Offset: 0x00002A0C
		public static void Add()
		{
			try
			{
				ShrineFactory ShrineFactory = new ShrineFactory
				{
					name = "Pulpit",
					modID = "cak",
					spritePath = "CakeMod/Resources/ModRooms/Pulpit",
					shadowSpritePath = "CakeMod/Resources/PulpitShadow",
				};
				GameObject gameObject = ShrineFactory.Build();
			}
			catch (Exception e)
			{
				Tools.Print(e);
			}
		}

		// Token: 0x060006D3 RID: 1747 RVA: 0x0003ACFC File Offset: 0x00038
	}
}


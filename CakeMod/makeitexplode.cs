using System;
using UnityEngine;
using CakeMod;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;

namespace CakeMod
{
	// Token: 0x0200008B RID: 139
	public class makeitexplode : MonoBehaviour
	{
		// Token: 0x0600030E RID: 782 RVA: 0x0001CE85 File Offset: 0x0001B085
		private void Awake()
		{
			this.m_projectile = base.GetComponent<Projectile>();
			this.m_projectile.OnDestruction += this.OnDestroy;
		}

		// Token: 0x0600030F RID: 783 RVA: 0x0001CEAC File Offset: 0x0001B0AC
		private void OnDestroy(Projectile projectile)
		{
			Exploder.DoDefaultExplosion(projectile.specRigidbody.UnitTopCenter, default(Vector2), null, false, CoreDamageTypes.None, true);
			DeadlyDeadlyGoopManager goopManagerForGoopType = DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(EasyGoopDefinitions.FireDef);
			goopManagerForGoopType.TimedAddGoopCircle(projectile.specRigidbody.UnitTopCenter, 5f, 0.35f, false);

		}

		// Token: 0x06000310 RID: 784 RVA: 0x0001CEDD File Offset: 0x0001B0DD
		private void Update()
		{
		}

		// Token: 0x04000119 RID: 281
		private Projectile m_projectile;
	}
}

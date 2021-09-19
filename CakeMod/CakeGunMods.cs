using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ItemAPI;
using UnityEngine;

namespace CakeMod
{
    class CakeGunMods
    {
        public static void Init()
        {
            (PickupObjectDatabase.GetById(60) as Gun).gameObject.AddComponent<DemonModifier>();

        }
    }

    public class DemonModifier : GunBehaviour
    {
        public override void PostProcessProjectile(Projectile projectile)
        {
            PlayerController player = gun.CurrentOwner as PlayerController;
            if (player.PlayerHasActiveSynergy("Rainbow Brim"))
            {
                projectile.ignoreDamageCaps = true;
                projectile.baseData.damage *= 2;
            }
        }
    }  
}
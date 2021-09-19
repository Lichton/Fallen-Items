using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CakeMod
{
    class HoveringGunsAdder
    {
        public static void AddHovers()
        {
            AdvancedHoveringGunProcessor PrimeSawHover = ETGMod.Databases.Items["Baby Good Magnum"].gameObject.AddComponent<AdvancedHoveringGunProcessor>();
            PrimeSawHover.Activate = true;
            PrimeSawHover.ConsumesTargetGunAmmo = false;
            PrimeSawHover.AimType = HoveringGunController.AimType.PLAYER_AIM;
            PrimeSawHover.PositionType = HoveringGunController.HoverPosition.CIRCULATE;
            PrimeSawHover.FireType = HoveringGunController.FireType.ON_COOLDOWN;
            PrimeSawHover.UsesMultipleGuns = false;
            PrimeSawHover.TargetGunID = ETGMod.Databases.Items["babygun"].PickupObjectId;
            PrimeSawHover.FireCooldown = 1f;
            PrimeSawHover.FireDuration = 0;

        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;
using Dungeonator;

namespace CakeMod
{
    class BowlerRainbowDust : PassiveItem
    {

        public static void Init()
        {
            string itemName = "Bowler's Rainbow Dust";
            string resourceName = "CakeMod/Resources/BowlerRainbowDust";

            GameObject obj = new GameObject(itemName);
            var item = obj.AddComponent<BowlerRainbowDust>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Pyrovision";
            string longDesc = "A strange powder made from plants native to Bowler's homeworld.\n\n" +
                "It's probably not a good itea to eat this.";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cak");

            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Damage, 1.2f, StatModifier.ModifyMethod.MULTIPLICATIVE);
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.RateOfFire, 1.2f, StatModifier.ModifyMethod.MULTIPLICATIVE);
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.MovementSpeed, 1.2f, StatModifier.ModifyMethod.MULTIPLICATIVE);
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.PlayerBulletScale, 1.2f, StatModifier.ModifyMethod.MULTIPLICATIVE);
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.ProjectileSpeed, 1.2f, StatModifier.ModifyMethod.MULTIPLICATIVE);
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.ReloadSpeed, 1.2f, StatModifier.ModifyMethod.MULTIPLICATIVE);

            item.quality = PickupObject.ItemQuality.EXCLUDED;
        }

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            Pixelator.Instance.AdditionalCoreStackRenderPass = new Material(ShaderCache.Acquire("Brave/Internal/RainbowChestShader"));
            Pixelator.Instance.AdditionalCoreStackRenderPass.SetFloat("_AllColorsToggle",1f);
        }

        public override DebrisObject Drop(PlayerController player)
        {
            Pixelator.Instance.AdditionalCoreStackRenderPass = null;
            return base.Drop(player);
        }


    }
}

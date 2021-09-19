using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;
using Gungeon;
using SaveAPI;
using Dungeonator;
using MonoMod.RuntimeDetour;
using System.Reflection;

namespace CakeMod
{
    class GlitchedHeart : PassiveItem
    {

        public static void Init()
        {
            string itemName = "Glitched Heart";
            string resourceName = "CakeMod/Resources/GlitchedHeart";

            GameObject obj = new GameObject(itemName);
            var item = obj.AddComponent<GlitchedHeart>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Null Reference Exception";
            string longDesc = "A heart infected by a poorly constructed virus, scrambling its data.\n" +
                "Picking up hearts gives you armour or damage.";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cak");


            CakeIDs.Glitchedheart = item.PickupObjectId;
            item.quality = PickupObject.ItemQuality.EXCLUDED;

        }
        Hook healthPickupHook = new Hook(
               typeof(HealthPickup).GetMethod("Pickup", BindingFlags.Instance | BindingFlags.Public),
               typeof(GlitchedHeart).GetMethod("heartPickupHookMethod")
           );

        public static void heartPickupHookMethod(Action<HealthPickup, PlayerController> orig, HealthPickup self, PlayerController player)
        {
            orig(self, player);
            if (player.HasPickupID(CakeIDs.Glitchedheart))
            {
                int bighead = UnityEngine.Random.Range(1, 3);
                if (bighead == 1)
                {
                    player.healthHaver.Armor = player.healthHaver.Armor + 1;
                }
                if (bighead == 2)
                {
                    player.ownerlessStatModifiers.Add(Toolbox.SetupStatModifier(PlayerStats.StatType.Damage, 0.1f, StatModifier.ModifyMethod.ADDITIVE, false));
                    player.stats.RecalculateStats(player, false, false);
                }

            }
        }

        private void ApplyStat(PlayerController player, PlayerStats.StatType statType, float amountToApply, StatModifier.ModifyMethod modifyMethod)
        {
            player.ownerlessStatModifiers.Add(Toolbox.SetupStatModifier(PlayerStats.StatType.Damage, 0.1f, StatModifier.ModifyMethod.ADDITIVE, false));
            player.stats.RecalculateStats(player, false, false);
        }
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
        }

        public override DebrisObject Drop(PlayerController player)
        {
            return base.Drop(player);
        }
    }
}

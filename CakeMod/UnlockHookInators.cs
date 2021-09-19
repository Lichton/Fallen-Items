using Dungeonator;
using MonoMod.RuntimeDetour;
using System;
using System.Reflection;
using SaveAPI;

namespace ItemAPI
{
    class UnlockHookInators
    {
        public static void Init()
        {
            Hook hook = new Hook(typeof(PlayerStats).GetMethod("RecalculateStatsInternal", BindingFlags.Public | BindingFlags.Instance), typeof(UnlockHookInators).GetMethod("MaxHealthStatAdder"));
        }
        public static void MaxHealthStatAdder(Action<PlayerStats, PlayerController> action, PlayerStats origStats, PlayerController owner)
        {
            action(origStats, owner);
            SaveAPIManager.UpdateMaximum(CustomTrackedMaximums.MAXIMUM_JAMMO, owner.stats.GetStatValue(PlayerStats.StatType.Curse));
            
        }
    }
}
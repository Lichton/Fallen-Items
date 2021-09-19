using Dungeonator;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using MonoMod.RuntimeDetour;

// Example Hook setup:
namespace CakeMod
{ // Namespace = namespace of the mod you are using this in

    public class RatMastery : DungeonDatabase
    {

        // Execute this method from Start() or Init() of mod's main CS file.
        public static void InitDungeonHook()
        {

            Hook GetOrLoadByNameHook = new Hook(
                typeof(DungeonDatabase).GetMethod("GetOrLoadByName", BindingFlags.Static | BindingFlags.Public),
                typeof(RatMastery).GetMethod("GetOrLoadByNameHook", BindingFlags.Static | BindingFlags.Public)
            );

        }
        public static Dungeon GetOrLoadByNameHook(Func<string, Dungeon> orig, string name)
        {
            Dungeon dungeon = null;
            if (name.ToLower() == "base_resourcefulrat")
            {
                dungeon = RatDungeonMods(GetOrLoadByName_Orig(name));
            }

            if (dungeon)
            {
                DebugTime.RecordStartTime();
                DebugTime.Log("AssetBundle.LoadAsset<Dungeon>({0})", new object[] { name });
                return dungeon;
            }
            else
            {
                return orig(name);
            }
        }

        public static Dungeon GetOrLoadByName_Orig(string name)
        {
            AssetBundle assetBundle = ResourceManager.LoadAssetBundle("dungeons/" + name.ToLower());
            DebugTime.RecordStartTime();
            Dungeon component = assetBundle.LoadAsset<GameObject>(name).GetComponent<Dungeon>();
            DebugTime.Log("AssetBundle.LoadAsset<Dungeon>({0})", new object[] { name });
            return component;
        }

        
        public static Dungeon RatDungeonMods(Dungeon dungeon)
        {
            // Here is where you'll do your mods to existing Dungeon prefab	
            var finalMasteryRewardRat = BraveUtility.RandomElement(ratMasteryRewards);
            dungeon.BossMasteryTokenItemId = finalMasteryRewardRat; // Item ID for Third Floor Master Round. Replace with Item ID of your choosing. 
            
            return dungeon;
        }
        public static List<int> ratMasteryRewards = new List<int>()
        {
            CakeIDs.MasterRat
        };
    }
}
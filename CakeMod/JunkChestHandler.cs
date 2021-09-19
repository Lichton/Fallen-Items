using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using Gungeon;
using UnityEngine;
using ItemAPI;
using static PickupObject;

namespace CakeMod
{
    class JunkChestHandler
    {
        public static void TheHooks()
        {
            chestPreOpenHook = new Hook(
                typeof(Chest).GetMethod("Open", BindingFlags.Instance | BindingFlags.NonPublic),
                typeof(JunkChestHandler).GetMethod("ChestPreOpen", BindingFlags.Static | BindingFlags.Public)
            );

        }
        public static Hook chestPreOpenHook;

        public static void ChestPreOpen(Action<Chest, PlayerController> orig, Chest self, PlayerController opener)
        {
            if (opener.HasPickupID(CakeIDs.PlatJunk))
            {
                self.PredictContents(opener);
                int bighead = UnityEngine.Random.Range(1, 101);
                if (bighead == 1)
                {
                    PickupObject obj = PickupObjectDatabase.GetById(580);
                    self.contents.Add(obj);
                }
                if (bighead == 2)
                {
                    PickupObject obj1 = PickupObjectDatabase.GetById(641);
                    self.contents.Add(obj1);
                }
                if (bighead == 3)
                {
                    PickupObject obj11 = PickupObjectDatabase.GetById(CakeIDs.PlatJunk);
                    self.contents.Add(obj11);
                }
                PickupObject obj2 = PickupObjectDatabase.GetById(127);
                self.contents.Add(obj2);
            }
          
            if (opener.HasPickupID(CakeIDs.CH))
            {
                self.PredictContents(opener);
                int bighead = UnityEngine.Random.Range(1, 31);
                if (bighead == 1)
                {

                    List<int> excludedGunsIds = new List<int>()
                    {

                    };
                    
                    foreach (Gun gun in opener.inventory.AllGuns) { excludedGunsIds.Add(gun.PickupObjectId); }
                    PickupObject obj2 = PickupObjectDatabase.GetRandomGunOfQualities(new System.Random(), excludedGunsIds, ItemQuality.D, ItemQuality.C);
                    self.contents.Add(obj2);
                }
                int lootID = BraveUtility.RandomElement(ExtendedLoots);
                PickupObject obj3 = PickupObjectDatabase.GetById(lootID);
                self.contents.Add(obj3);
                
            }



            orig(self, opener);
        }
        

        private static List<int> ExtendedLoots = new List<int>()
        {
            78, //Ammo
            600, //Spread Ammo
            565, //Glass Guon Stone
            73, //Half Heart
            85, //Heart
            120, //Armour
            224, //Blank
            67, //Key
             68, //Bronze Casing
             70, //Silver Casing
    };

       


}
}

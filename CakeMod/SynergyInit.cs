using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ItemAPI;
using UnityEngine;

namespace CakeMod
{
    class SynergyFormInitialiser
    {
        public static void AddSynergyForms()
        {
            //------------------------------------------------------SYNERGY FORMES
            #region PreBigUpdate
            //Pirate - Ye' Olden Ages Synergy
            AdvancedTransformGunSynergyProcessor PirateSynergy = (PickupObjectDatabase.GetById(CakeIDs.PirateID) as Gun).gameObject.AddComponent<AdvancedTransformGunSynergyProcessor>();
            PirateSynergy.NonSynergyGunId = CakeIDs.PirateID;
            PirateSynergy.SynergyGunId = CakeIDs.ArrowKinSynergyFormID;
            PirateSynergy.SynergyToCheck = "Ye Olden Ages";
            
            AdvancedDualWieldSynergyProcessor StunTranqDualSTUN = (PickupObjectDatabase.GetById(CakeIDs.TimeGun) as Gun).gameObject.AddComponent<AdvancedDualWieldSynergyProcessor>();
            StunTranqDualSTUN.PartnerGunID = 169;
            StunTranqDualSTUN.SynergyNameToCheck = "Time Paradox";

            AdvancedDualWieldSynergyProcessor StunTranqDualSTUN2 = (PickupObjectDatabase.GetById(CakeIDs.PissGun) as Gun).gameObject.AddComponent<AdvancedDualWieldSynergyProcessor>();
            StunTranqDualSTUN2.PartnerGunID = 5;
            StunTranqDualSTUN2.SynergyNameToCheck = "A. W. Piss";

            AdvancedTransformGunSynergyProcessor PirateSynerg2y = (PickupObjectDatabase.GetById(197) as Gun).gameObject.AddComponent<AdvancedTransformGunSynergyProcessor>();
            PirateSynerg2y.NonSynergyGunId = 197;
            PirateSynerg2y.SynergyGunId = CakeIDs.FirePea;
            PirateSynerg2y.SynergyToCheck = "Burn Baby Burn";

            AdvancedTransformGunSynergyProcessor PirateSynerg2y2 = (PickupObjectDatabase.GetById(197) as Gun).gameObject.AddComponent<AdvancedTransformGunSynergyProcessor>();
            PirateSynerg2y2.NonSynergyGunId = 647;
            PirateSynerg2y2.SynergyGunId = 808;
            PirateSynerg2y2.SynergyToCheck = "Rat Mastery";
            #endregion
        }
    }
}
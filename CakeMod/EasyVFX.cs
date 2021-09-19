using ItemAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Dungeonator;

namespace CakeMod
{

    public class EasyVFXDatabase
    {



        //Basegame VFX Objects
        public static GameObject WeakenedStatusEffectOverheadVFX = ResourceCache.Acquire("Global VFX/VFX_Debuff_Status") as GameObject;
        public static GameObject SpiratTeleportVFX;
        public static GameObject TeleporterPrototypeTelefragVFX = PickupObjectDatabase.GetById(449).GetComponent<TeleporterPrototypeItem>().TelefragVFXPrefab.gameObject;
        public static GameObject CrisisStoneVFX = PickupObjectDatabase.GetById(634).GetComponent<CrisisStoneItem>().WallVFX.gameObject;
        public static GameObject BloodiedScarfPoofVFX = PickupObjectDatabase.GetById(436).GetComponent<BlinkPassiveItem>().BlinkpoofVfx.gameObject;
        public static GameObject ChestTeleporterTimeWarp = (PickupObjectDatabase.GetById(573) as ChestTeleporterItem).TeleportVFX;
        public static GameObject MachoBraceDustUpVFX = PickupObjectDatabase.GetById(665).GetComponent<MachoBraceItem>().DustUpVFX;
        public static GameObject MachoBraceBurstVFX = PickupObjectDatabase.GetById(665).GetComponent<MachoBraceItem>().BurstVFX;
        public static GameObject MachoBraceOverheadVFX = PickupObjectDatabase.GetById(665).GetComponent<MachoBraceItem>().OverheadVFX;
        public static GameObject CheeseVFX = PickupObjectDatabase.GetById(662).GetComponent<CheeseWheelItem>().TransformationVFX;
        public static GameObject YellowChamberVFX = PickupObjectDatabase.GetById(570).GetComponent<YellowChamberItem>().EraseVFX;
        //Projectile Death Effects
        public static GameObject IncenseVFX;
        public static GameObject FlameVFX;
        public static GameObject StringVFX;
        public static GameObject GreenLaserCircleVFX = (PickupObjectDatabase.GetById(89) as Gun).DefaultModule.projectiles[0].hitEffects.overrideMidairDeathVFX;
        public static GameObject YellowLaserCircleVFX = (PickupObjectDatabase.GetById(651) as Gun).DefaultModule.projectiles[0].hitEffects.overrideMidairDeathVFX;
        public static GameObject RedLaserCircleVFX = (PickupObjectDatabase.GetById(32) as Gun).DefaultModule.projectiles[0].hitEffects.overrideMidairDeathVFX;
        public static GameObject BlueLaserCircleVFX = (PickupObjectDatabase.GetById(59) as Gun).DefaultModule.projectiles[0].hitEffects.overrideMidairDeathVFX;
        public static GameObject SmoothLightBlueLaserCircleVFX = (PickupObjectDatabase.GetById(576) as Gun).DefaultModule.projectiles[0].hitEffects.overrideMidairDeathVFX;
        public static GameObject SmoothLightGreenLaserCircleVFX = (PickupObjectDatabase.GetById(360) as Gun).DefaultModule.projectiles[0].hitEffects.overrideMidairDeathVFX;
        public static GameObject WhiteCircleVFX = (PickupObjectDatabase.GetById(330) as Gun).DefaultModule.projectiles[0].hitEffects.overrideMidairDeathVFX;
        public static GameObject BlueFrostBlastVFX = (PickupObjectDatabase.GetById(225) as Gun).DefaultModule.projectiles[0].hitEffects.overrideMidairDeathVFX;
        public static GameObject RedFireBlastVFX = (PickupObjectDatabase.GetById(125) as Gun).DefaultModule.projectiles[0].hitEffects.overrideMidairDeathVFX;
        public static GameObject SmallMagicPuffVFX = (PickupObjectDatabase.GetById(338) as Gun).DefaultModule.projectiles[0].hitEffects.overrideMidairDeathVFX;
        public static HellDragZoneController hellDrag = DungeonDatabase.GetOrLoadByName("Base_Forge").PatternSettings.flows[0].AllNodes.Where(node => node.overrideExactRoom != null && node.overrideExactRoom.name.Contains("EndTimes")).First().overrideExactRoom.placedObjects.Where(ppod => ppod != null && ppod.nonenemyBehaviour != null).First().nonenemyBehaviour.gameObject.GetComponentsInChildren<HellDragZoneController>()[0];
        public static GameObject ExplodeFirework = ResourceManager.LoadAssetBundle("shared_auto_001").LoadAsset<GameObject>("VFX_Explosion_Firework");
        //Basegame VFX Pools
        public static VFXPool SpiratTeleportVFXPool;
        public static void Init()
        {
            //Spirat Teleportation VFX
            #region SpiratTP

            GameObject teleportBullet = EnemyDatabase.GetOrLoadByGuid("7ec3e8146f634c559a7d58b19191cd43").bulletBank.GetBullet("self").BulletObject;
            Projectile proj = teleportBullet.GetComponent<Projectile>();
            if (proj != null)
            {
                TeleportProjModifier tp = proj.GetComponent<TeleportProjModifier>();
                if (tp != null)
                {
                    SpiratTeleportVFXPool = tp.teleportVfx;
                    SpiratTeleportVFX = tp.teleportVfx.effects[0].effects[0].effect;
                }
            }
            #endregion
        }
        public static List<string> Mod_VFXList;
        public static List<IntVector2> Mod_Vector2List;

    }
}
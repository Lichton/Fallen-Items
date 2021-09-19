using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Gungeon;
using MonoMod;
using UnityEngine;
using ItemAPI;
using BasicGun;


namespace CakeMod
{
    public class Television : AdvancedGunBehavior
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("TV", "tv");
            Game.Items.Rename("outdated_gun_mods:tv", "cak:tv");
            var behav = gun.gameObject.AddComponent<Television>();
            //behav.overrideNormalFireAudio = "Play_ENM_shelleton_beam_01";
            gun.SetShortDescription("Television Head");
            gun.SetLongDescription("A scratchy and old television that switches between multiple near-useless channels.");

            gun.SetupSprite(null, "tv_idle_001", 8);

            gun.SetAnimationFPS(gun.shootAnimation, 8);
            gun.SetAnimationFPS(gun.idleAnimation, 8);
            gun.SetAnimationFPS(gun.reloadAnimation, 8);
            gun.isAudioLoop = true;

            //int iterator = 0;
                ProjectileModule mod = gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(86) as Gun, true, false);
                
                //if (iterator == 1) mod.angleFromAim = 30;
                //if (iterator == 2) mod.angleFromAim = -30;
                //iterator++;
                mod.ammoCost = 10;
                if (mod != gun.DefaultModule) { mod.ammoCost = 0; }
                mod.shootStyle = ProjectileModule.ShootStyle.Beam;
                mod.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
                mod.cooldownTime = 0.001f;
                mod.numberOfShotsInClip = 400;
                mod.ammoType = GameUIAmmoType.AmmoType.BEAM;

                List<string> BeamAnimPaths = new List<string>()
            {
                "CakeMod/Resources/BeamStatic/static_middle_001",
                "CakeMod/Resources/BeamStatic/static_middle_002",

            };
                List<string> StartAnimPaths = new List<string>()
            {
                "CakeMod/Resources/BeamStatic/static_start_001",
                "CakeMod/Resources/BeamStatic/static_start_002",


            };

                List<string> ImpactAnimPaths = new List<string>()
            {
                "CakeMod/Resources/BeamStatic/static_impact_001",
                "CakeMod/Resources/BeamStatic/static_impact_002",
                "CakeMod/Resources/BeamStatic/static_impact_003",
                "CakeMod/Resources/BeamStatic/static_impact_004",
            };

                List<string> End = new List<string>()
            {
                "CakeMod/Resources/BeamStatic/static_end_001",
                "CakeMod/Resources/BeamStatic/static_end_002",

            };

                Projectile projectile = UnityEngine.Object.Instantiate<Projectile>((PickupObjectDatabase.GetById(86) as Gun).DefaultModule.projectiles[0]);

                BasicBeamController beamComp = projectile.GenerateBeamPrefab(
                    "CakeMod/Resources/BeamStatic/static_middle_001",
                    new Vector2(10, 2),
                    new Vector2(0, 4),
                    BeamAnimPaths,
                    12,
                    //Beam Impact
                    ImpactAnimPaths,
                    12,
                    new Vector2(4, 4),
                    new Vector2(7, 7),
                    //End of the Beam
                    null,
                    -1,
                    null,
                    null,
                    //Start of the Beam
                    StartAnimPaths,
                    12,
                    new Vector2(10, 2),
                    new Vector2(0, 4)
                    );

                projectile.gameObject.SetActive(false);
                FakePrefab.MarkAsFakePrefab(projectile.gameObject);
                UnityEngine.Object.DontDestroyOnLoad(projectile);
                projectile.baseData.damage = 0.5f;
                projectile.baseData.force *= 1f;
                projectile.baseData.range *= 5;
                projectile.baseData.speed *= 5f;
                projectile.specRigidbody.CollideWithOthers = false;

                

                beamComp.boneType = BasicBeamController.BeamBoneType.Projectile;

                beamComp.startAudioEvent = "Play_ENM_deathray_shot_01";
                beamComp.projectile.baseData.damage = 120;
                beamComp.endAudioEvent = "Stop_ENM_deathray_loop_01";
                beamComp.penetration = 0;
                beamComp.reflections = 0;
                beamComp.IsReflectedBeam = false;
                //beamComp.specRigidbody.CollideWithOthers = false;

                //beamComp.ReflectedFromRigidbody = true;
                //beamComp.interpolateStretchedBones = false;
                //beamComp.HitsEnemies = true;
                //beamComp.HitsPlayers = false;
                mod.projectiles[0] = projectile;


            //GUN STATS
            gun.doesScreenShake = false;
            gun.reloadTime = 1.2f;
            gun.muzzleFlashEffects.type = VFXPoolType.None;
            gun.barrelOffset.transform.localPosition = new Vector3(0.5f, 0.35f, 0f);
            gun.SetBaseMaxAmmo(400);
            gun.ammo = 400;
            gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.shootAnimation).wrapMode = tk2dSpriteAnimationClip.WrapMode.LoopSection;
            gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.shootAnimation).loopStart = 0;
            gun.quality = PickupObject.ItemQuality.EXCLUDED;
            ETGMod.Databases.Items.Add(gun, null, "ANY");
            Television.TelevisionID = gun.PickupObjectId;
        }
        public static int TelevisionID;
        
       
        public override void OnReloadPressed(PlayerController player, Gun gun, bool bSOMETHING)
        {
            if (gun.IsReloading && this.HasReloaded)
            {
                AkSoundEngine.PostEvent("Stop_WPN_All", base.gameObject);
                HasReloaded = false;
                AkSoundEngine.PostEvent("Play_OBJ_rock_break", base.gameObject);
                int bighead = UnityEngine.Random.Range(1, 4);
                if (bighead == 1)
                {
                    isCartoon = true;
                    isTelemarket = false;
                    isWrassle = false;

                }
                if (bighead == 2)
                {
                    isCartoon = false;
                    isTelemarket = true;
                    isWrassle = false;
                }
                if(bighead == 3)
                {
                    isCartoon = false;
                    isTelemarket = false;
                    isWrassle = true;
                }
                base.OnReloadPressed(player, gun, bSOMETHING);
                
            }

        }

        protected override void OnPickedUpByPlayer(PlayerController player)
        {
            base.OnPickedUpByPlayer(player);
            player.GunChanged += this.GunsChanged;
        }

        
        public void GunsChanged(Gun previous, Gun current, bool newGun)
        {
            if(newGun == this.gun)
            {
                Player.PostProcessBeam += this.Channels;
            }
            else
            {
                Player.PostProcessBeam -= this.Channels;
            }
        }

        private void Channels(BeamController obj)
        {
            if (isCartoon == true)
            {

            }
            if (isWrassle == true)
            {
                obj.AdjustPlayerBeamTint(Color.red, 1);
                obj.projectile.baseData.damage *= 2;
            }
            if (isTelemarket == true)
            {
                bighead2 = UnityEngine.Random.Range(1, 9);
                if (bighead2 == 1)
                {

                    obj.AdjustPlayerBeamTint(Color.red, 1);

                }
                if (bighead2 == 2)
                {

                    obj.AdjustPlayerBeamTint(ExtendedColours.orange, 1);

                }
                if (bighead2 == 3)
                {
                    obj.AdjustPlayerBeamTint(Color.yellow, 1);

                }
                if (bighead2 == 4)
                {
                    obj.AdjustPlayerBeamTint(Color.blue, 1);

                }
                if (bighead2 == 5)
                {
                    obj.AdjustPlayerBeamTint(Color.green, 1);

                }
                if (bighead2 == 6)
                {
                    obj.AdjustPlayerBeamTint(ExtendedColours.purple, 1);


                }
                if (bighead2 == 7)
                {
                    obj.AdjustPlayerBeamTint(ExtendedColours.pink, 1);

                }
                if (bighead2 == 8)
                {
                    
                    obj.AdjustPlayerBeamTint(Color.grey, 1);

                }
                obj.projectile.OnHitEnemy += this.OnHitEnemy;
            }
        }
        private IEnumerator HandleFear(PlayerController user, SpeculativeRigidbody enemy)
        {
            bool flag = this.fleeData == null || this.fleeData.Player != base.Owner;
            if (flag)
            {
                this.fleeData = new FleePlayerData();
                this.fleeData.Player = user;
                this.fleeData.StartDistance *= 2f;
            }
            bool flag2 = enemy.aiActor.behaviorSpeculator != null;
            if (flag2)
            {
                enemy.aiActor.behaviorSpeculator.FleePlayerData = this.fleeData;
                FleePlayerData fleePlayerData = new FleePlayerData();
                yield return new WaitForSeconds(1.5f);
                enemy.aiActor.behaviorSpeculator.FleePlayerData.Player = null;
            }
            yield break;
        }
        private void OnHitEnemy(Projectile arg1, SpeculativeRigidbody arg2, bool arg3)
        {
           
                if (bighead2 == 6)
                {
                    base.StartCoroutine(this.HandleFear(gun.CurrentOwner as PlayerController, arg2));
                }
                if (bighead2 == 5)
                {
                    GameActorHealthEffect irradiatedLeadEffect = PickupObjectDatabase.GetById(204).GetComponent<BulletStatusEffectItem>().HealthModifierEffect;
                    arg2.aiActor.ApplyEffect(irradiatedLeadEffect, 2f, arg1);
                }
                if (bighead2 == 2)
                {
                    GameActorFireEffect hotLeadEffect = PickupObjectDatabase.GetById(295).GetComponent<BulletStatusEffectItem>().FireModifierEffect;
                    arg2.aiActor.ApplyEffect(hotLeadEffect, 2f, arg1);
                }
                if (bighead2 == 1)
                {
                    arg2.aiActor.healthHaver.ApplyDamage(1f * (gun.CurrentOwner as PlayerController).stats.GetStatValue(PlayerStats.StatType.Damage), Vector2.zero, "Erasure", CoreDamageTypes.None, DamageCategory.Normal, true, null, false);
                    GlobalSparksDoer.DoRadialParticleBurst(50, arg2.specRigidbody.HitboxPixelCollider.UnitCenter, arg2.specRigidbody.HitboxPixelCollider.UnitCenter, 90f, 2f, 0f, null, null, Color.red, GlobalSparksDoer.SparksType.BLOODY_BLOOD);
                }
                if (bighead2 == 3)
                {
                    arg2.aiActor.ApplyEffect(this.cheeseEffect, 3f, null);
                }
                if (bighead2 == 7)
                {
                    GameActorCharmEffect charmingRoundsEffect = PickupObjectDatabase.GetById(527).GetComponent<BulletStatusEffectItem>().CharmModifierEffect;
                    arg2.aiActor.ApplyEffect(charmingRoundsEffect, 3f, null);
                }
                if (bighead2 == 4)
                {
                    GameActorFreezeEffect frostBulletsEffect = PickupObjectDatabase.GetById(278).GetComponent<BulletStatusEffectItem>().FreezeModifierEffect;
                    arg2.aiActor.ApplyEffect(frostBulletsEffect, 3f, null);
                }
                if (bighead2 == 8)
                {
                    GameActorSpeedEffect tripleCrossbowSlowEffect = (PickupObjectDatabase.GetById(381) as Gun).DefaultModule.projectiles[0].speedEffect;
                    arg2.aiActor.ApplyEffect(tripleCrossbowSlowEffect, 3f, null);
                }
            }

        public override void OnPostFired(PlayerController player, Gun gun)
        {
            gun.PreventNormalFireAudio = true;
        }
        private bool HasReloaded;
        public bool HasFlipped;

        protected override void Update()
        {
                gun.PreventNormalFireAudio = true;

                if (gun.CurrentOwner)
                {

                    if (!gun.PreventNormalFireAudio)
                    {
                        this.gun.PreventNormalFireAudio = true;
                    }
                    if (!gun.IsReloading && !HasReloaded)
                    {
                        this.HasReloaded = true;
                    }
                }
            }
        public bool isCartoon = false;
        public bool isWrassle = false;
        public bool isTelemarket = false;
        public static GameActorFireEffect hotLeadEffect = PickupObjectDatabase.GetById(295).GetComponent<BulletStatusEffectItem>().FireModifierEffect;
        public static GameActorFireEffect greenFireEffect = PickupObjectDatabase.GetById(706).GetComponent<Gun>().DefaultModule.projectiles[0].fireEffect;


        //Freezes
        public static GameActorFreezeEffect frostBulletsEffect = PickupObjectDatabase.GetById(278).GetComponent<BulletStatusEffectItem>().FreezeModifierEffect;
        public static GameActorFreezeEffect chaosBulletsFreeze = PickupObjectDatabase.GetById(569).GetComponent<ChaosBulletsItem>().FreezeModifierEffect;

        //Poisons
        public static GameActorHealthEffect irradiatedLeadEffect = PickupObjectDatabase.GetById(204).GetComponent<BulletStatusEffectItem>().HealthModifierEffect;

        //Charms
        public static GameActorCharmEffect charmingRoundsEffect = PickupObjectDatabase.GetById(527).GetComponent<BulletStatusEffectItem>().CharmModifierEffect;

        //Cheeses

        //Speed Changes
        public static GameActorSpeedEffect tripleCrossbowSlowEffect = (PickupObjectDatabase.GetById(381) as Gun).DefaultModule.projectiles[0].speedEffect;
        private FleePlayerData fleeData;

        public GameActorCheeseEffect cheeseEffect = (PickupObjectDatabase.GetById(626) as Gun).DefaultModule.projectiles[0].cheeseEffect;
        public GameActorBleedEffect bleedEffect = (PickupObjectDatabase.GetById(542) as Gun).DefaultModule.projectiles[0].bleedEffect;
        public int bighead2;


    }
   
    }
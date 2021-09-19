using System;
using System.Collections;
using Gungeon;
using MonoMod;
using UnityEngine;
using ItemAPI;
using BasicGun;
using System.Collections.Generic;

namespace CakeMod
{

    public class boneblaster : GunBehaviour
    {


        public static void Add()
        {
            // Get yourself a new gun "base" first.
            // Let's just call it "Basic Gun", and use "jpxfrd" for all sprites and as "codename" All sprites must begin with the same word as the codename. For example, your firing sprite would be named "jpxfrd_fire_001".
            Gun gun = ETGMod.Databases.Items.NewGun("boneblaster", "boneblaster");
            // "kp:basic_gun determines how you spawn in your gun through the console. You can change this command to whatever you want, as long as it follows the "name:itemname" template.
            Game.Items.Rename("outdated_gun_mods:boneblaster", "cak:boneblaster");
            gun.gameObject.AddComponent<boneblaster>();
            //These two lines determines the description of your gun, ".SetShortDescription" being the description that appears when you pick up the gun and ".SetLongDescription" being the description in the Ammonomicon entry. 
            gun.SetShortDescription("Megalovanity");
            gun.SetLongDescription("A gungeoneer reduced to only bone and determination once used this as his weapon.");
            // This is required, unless you want to use the sprites of the base gun.
            // That, by default, is the pea shooter.
            // SetupSprite sets up the default gun sprite for the ammonomicon and the "gun get" popup.
            // WARNING: Add a copy of your default sprite to Ammonomicon Encounter Icon Collection!
            // That means, "sprites/Ammonomicon Encounter Icon Collection/defaultsprite.png" in your mod .zip. You can see an example of this with inside the mod folder.
            gun.SetupSprite(null, "boneblaster_idle_001", 8);
            // ETGMod automatically checks which animations are available.
            // The numbers next to "shootAnimation" determine the animation fps. You can also tweak the animation fps of the reload animation and idle animation using this method.
            gun.SetAnimationFPS(gun.shootAnimation, 6);
            gun.SetAnimationFPS(gun.reloadAnimation, 1);
            // Every modded gun has base projectile it works with that is borrowed from other guns in the game. 
            // The gun names are the names from the JSON dump! While most are the same, some guns named completely different things. If you need help finding gun names, ask a modder on the Gungeon discord.

            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(812) as Gun, true, false);

            //gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(383) as Gun, true, false);
            // Here we just take the default projectile module and change its settings how we want it to be.
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 2;
            gun.DefaultModule.cooldownTime = 0.1f;
            gun.DefaultModule.numberOfShotsInClip = 10;
            gun.SetBaseMaxAmmo(500);
            gun.DefaultModule.angleVariance = 0f;
            gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.SKULL;
            // Here we just set the quality of the gun and the "EncounterGuid", which is used by Gungeon to identify the gun.
            gun.quality = PickupObject.ItemQuality.EXCLUDED;
            gun.encounterTrackable.EncounterGuid = "boneblaster";
            //This block of code helps clone our projectile. Basically it makes it so things like Shadow Clone and Hip Holster keep the stats/sprite of your custom gun's projectiles.
            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.transform.parent = gun.barrelOffset;
            gun.barrelOffset.localPosition += new Vector3(0.3f, 0.5f, 0f);
            //projectile.baseData allows you to modify the base properties of your projectile module.
            //In our case, our gun uses modified projectiles from the ak-47.
            //Setting static values for a custom gun's projectile stats prevents them from scaling with player stats and bullet modifiers (damage, shotspeed, knockback)
            //You have to multiply the value of the original projectile you're using instead so they scale accordingly. For example if the projectile you're using as a base has 10 damage and you want it to be 6 you use this
            //In our case, our projectile has a base damage of 5.5, so we multiply it by 1.1 so it does 10% more damage from the ak-47.
            projectile.baseData.damage *= 2f;
            projectile.baseData.speed *= 0.2f;
            projectile.AdditionalScaleMultiplier = 2f;
            projectile.transform.parent = gun.barrelOffset;
            //This determines what sprite you want your projectile to use. Note this isn't necessary if you don't want to have a custom projectile sprite.
            //The x and y values determine the size of your custom projectile
            ETGMod.Databases.Items.Add(gun, null, "ANY");

        }

        public override void PostProcessProjectile(Projectile projectile)
        {
            projectile.OnHitEnemy = (Action<Projectile, SpeculativeRigidbody, bool>)Delegate.Combine(projectile.OnHitEnemy, new Action<Projectile, SpeculativeRigidbody, bool>(this.HandleHitEnemy));
            base.PostProcessProjectile(projectile);
        }


        private void HandleHitEnemy(Projectile sourceProjectile, SpeculativeRigidbody hitRigidbody, bool fatal)
        {
            if (fatal == true && isBeam == true)
            {
                homingRadius = 16f;
                homingAngularVelocity = 400;

                Gun Seed = PickupObjectDatabase.GetById(812) as Gun;
                sourceProjectile = Seed.DefaultModule.projectiles[0];

                PlayerController player = gun.CurrentOwner as PlayerController;

                int count = 1;

                foreach (Vector2 number in pos)
                {
                    GameObject gameObject = SpawnManager.SpawnProjectile(sourceProjectile.gameObject, hitRigidbody.sprite.WorldCenter + pos[count], rotation[count], true);
                    Projectile component = gameObject.GetComponent<Projectile>();
                    component.Owner = player;
                    component.Shooter = player.specRigidbody;

                    hitRigidbody.aiActor.IsWorthShootingAt = false;
                    HomingModifier homingModifier = component.gameObject.GetComponent<HomingModifier>();
                    if (homingModifier == null)
                    {
                        homingModifier = component.gameObject.AddComponent<HomingModifier>();
                        homingModifier.HomingRadius = 0f;
                        homingModifier.AngularVelocity = 0f;

                    }
                    float num = 1f;
                    homingModifier.HomingRadius += this.homingRadius * num;
                    homingModifier.AngularVelocity += this.homingAngularVelocity * num;

                    count++;
                }

            }

        }

        List<Vector2> pos = new List<Vector2>
        {
            new Vector2(2, 0),
            new Vector2(1f, -1f),
            new Vector2(0, -2),
            new Vector2(-1f, -1f),
            new Vector2(-2, 0),
            new Vector2(-1f, 1f),
            new Vector2(0, 2),
            new Vector2(1f, 1f)
        };

        List<Quaternion> rotation = new List<Quaternion>
        {
            Quaternion.Euler(0, 0, 0),
            Quaternion.Euler(0, 0, 45),
            Quaternion.Euler(0, 0, 90),
            Quaternion.Euler(0, 0, 135),
            Quaternion.Euler(0, 0, 180),
            Quaternion.Euler(0, 0, 225),
            Quaternion.Euler(0, 0, 270),
            Quaternion.Euler(0, 0, 315)
        };

        public float homingRadius;
        public float homingAngularVelocity;
        public override void OnPostFired(PlayerController player, Gun gun)
        {
            //This determines what sound you want to play when you fire a gun.
            //Sounds names are based on the Gungeon sound dump, which can be found at EnterTheGungeon/Etg_Data/StreamingAssets/Audio/GeneratedSoundBanks/Windows/sfx.txt
            gun.PreventNormalFireAudio = true;
            AkSoundEngine.PostEvent("Play_OBJ_heart_heal_01", gameObject);
        }
        private bool HasReloaded;
        //This block of code allows us to change the reload sounds.
        protected void Update()
        {
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

        public override void OnReloadPressed(PlayerController player, Gun gun, bool bSOMETHING)
        {
            if (gun.IsReloading && this.HasReloaded)
            {

                Gun beamGun = PickupObjectDatabase.GetById(383) as Gun;
                Gun boneGun = PickupObjectDatabase.GetById(812) as Gun;
                Gun clipSprite = PickupObjectDatabase.GetById(760) as Gun;
                if (isBeam == true)
                {
                    gun.DefaultModule.projectiles[0] = boneGun.DefaultModule.projectiles[0];
                    gun.DefaultModule.numberOfShotsInClip = 10;
                    gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.SKULL;
                    isBeam = false;
                }
                else
                {
                    gun.DefaultModule.projectiles[0] = beamGun.DefaultModule.projectiles[0];
                    gun.DefaultModule.numberOfShotsInClip = 1;
                    gun.DefaultModule.projectiles[0].baseData.damage *= 2;
                    gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.CUSTOM;
                    gun.DefaultModule.customAmmoType = clipSprite.DefaultModule.customAmmoType;
                    isBeam = true;
                }

                HasReloaded = false;
                AkSoundEngine.PostEvent("Stop_WPN_All", base.gameObject);
                base.OnReloadPressed(player, gun, bSOMETHING);
                AkSoundEngine.PostEvent("Play_OBJ_goldkey_pickup_01", base.gameObject);
            }
        }


        bool isBeam = false;
        //All that's left now is sprite stuff. 
        //Your sprites should be organized, like how you see in the mod folder. 
        //Every gun requires that you have a .json to match the sprites or else the gun won't spawn at all
        //.Json determines the hand sprites for your character. You can make a gun two handed by having both "SecondaryHand" and "PrimaryHand" in the .json file, which can be edited through Notepad or Visual Studios
        //By default this gun is a one-handed weapon
        //If you need a basic two handed .json. Just use the jpxfrd2.json.
        //And finally, don't forget to add your Gun to your ETGModule class!
    }
}
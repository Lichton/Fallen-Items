using System;
using System.Collections;
using Gungeon;
using MonoMod;
using UnityEngine;
using BasicGun;
using System.Collections.Generic;
using ItemAPI;
using Dungeonator;
using SaveAPI;

namespace CakeMod
{

    public class JackpotOfGreed : GunBehaviour
    {


        public static void Add()
        {
            // Get yourself a new gun "base" first.
            // Let's just call it "Basic Gun", and use "jpxfrd" for all sprites and as "codename" All sprites must begin with the same word as the codename. For example, your firing sprite would be named "jpxfrd_fire_001".
            Gun gun = ETGMod.Databases.Items.NewGun("Jackpot Of Greed", "pot");
            // "kp:basic_gun determines how you spawn in your gun through the console. You can change this command to whatever you want, as long as it follows the "name:itemname" template.
            Game.Items.Rename("outdated_gun_mods:jackpot_of_greed", "cak:jackpot_of_greed");
            gun.gameObject.AddComponent<JackpotOfGreed>();
            //These two lines determines the description of your gun, ".SetShortDescription" being the description that appears when you pick up the gun and ".SetLongDescription" being the description in the Ammonomicon entry. 
            gun.SetShortDescription("Midas Touch");
            gun.SetLongDescription("Priceless, but useless in the Gungeon where normal currency has no meaning. Conjures infinite gold coins.");
            // This is required, unless you want to use the sprites of the base gun.
            // That, by default, is the pea shooter.
            // SetupSprite sets up the default gun sprite for the ammonomicon and the "gun get" popup.
            // WARNING: Add a copy of your default sprite to Ammonomicon Encounter Icon Collection!
            // That means, "sprites/Ammonomicon Encounter Icon Collection/defaultsprite.png" in your mod .zip. You can see an example of this with inside the mod folder.
            gun.SetupSprite(null, "pot_idle_001", 8);
            // ETGMod automatically checks which animations are available.
            // The numbers next to "shootAnimation" determine the animation fps. You can also tweak the animation fps of the reload animation and idle animation using this method.
            gun.SetAnimationFPS(gun.shootAnimation, 20);
            gun.SetAnimationFPS(gun.reloadAnimation, 13);
            // Every modded gun has base projectile it works with that is borrowed from other guns in the game. 
            // The gun names are the names from the JSON dump! While most are the same, some guns named completely different things. If you need help finding gun names, ask a modder on the Gungeon discord.
            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(56) as Gun, true, false);
            // Here we just take the default projectile module and change its settings how we want it to be.
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 0.7f;
            gun.DefaultModule.cooldownTime = 0.3f;
            gun.DefaultModule.numberOfShotsInClip = 10;
            gun.SetBaseMaxAmmo(500);
            gun.InfiniteAmmo = true;
            gun.barrelOffset.localPosition += new Vector3(0.2f, 0.05f, 0f);
            Gun gun3 = (Gun)ETGMod.Databases.Items["demon_head"];
            gun.muzzleFlashEffects = gun3.muzzleFlashEffects;
            // Here we just set the quality of the gun and the "EncounterGuid", which is used by Gungeon to identify the gun.
            gun.quality = PickupObject.ItemQuality.B;
            gun.encounterTrackable.EncounterGuid = "JackpotOfGreed";
            //This block of code helps clone our projectile. Basically it makes it so things like Shadow Clone and Hip Holster keep the stats/sprite of your custom gun's projectiles.
            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            gun.DefaultModule.projectiles[0] = projectile;
            //projectile.baseData allows you to modify the base properties of your projectile module.
            //In our case, our gun uses modified projectiles from the ak-47.
            //Setting static values for a custom gun's projectile stats prevents them from scaling with player stats and bullet modifiers (damage, shotspeed, knockback)
            //You have to multiply the value of the original projectile you're using instead so they scale accordingly. For example if the projectile you're using as a base has 10 damage and you want it to be 6 you use this
            //In our case, our projectile has a base damage of 5.5, so we multiply it by 1.1 so it does 10% more damage from the ak-47.
            projectile.baseData.damage *= 1.2f;
            projectile.baseData.speed *= 1f;
            projectile.transform.parent = gun.barrelOffset;
            //This determines what sprite you want your projectile to use. Note this isn't necessary if you don't want to have a custom projectile sprite.
            //The x and y values determine the size of your custom projectile
            projectile.SetProjectileSpriteRight("pot_projectile", 8, 8, null, null);
            ETGMod.Databases.Items.Add(gun, null, "ANY");
            gun.PlaceItemInAmmonomiconAfterItemById(197);
            gun.SetupUnlockOnStat(TrackedStats.TIMES_CLEARED_GUNGEON, 50f, DungeonPrerequisite.PrerequisiteOperation.GREATER_THAN);

        }

        public void OnProjectileHitEnemy(Projectile proj, SpeculativeRigidbody enemy, bool fatal)
        {
            if (enemy != null)
            {
                if (enemy.aiActor != null)
                {
                    AIActor aiactor = enemy.aiActor;
                    aiactor.sprite.usesOverrideMaterial = true;
                    tk2dSprite tk2dSprite = aiactor.sprite as tk2dSprite;
                    tk2dSprite.GenerateUV2 = true;
                    Material material = UnityEngine.Object.Instantiate<Material>(aiactor.sprite.renderer.material);
                    material.DisableKeyword("TINTING_OFF");
                    material.EnableKeyword("TINTING_ON");
                    material.SetColor("_OverrideColor", new Color(0.87f, 0.56f, 0f));
                    material.DisableKeyword("EMISSIVE_OFF");
                    material.EnableKeyword("EMISSIVE_ON");
                    material.SetFloat("_EmissivePower", 1.75f);
                    material.SetFloat("_EmissiveColorPower", 1f);
                    material.SetFloat("_AllColorsToggle", 1f);
                    aiactor.sprite.renderer.material = material;
                    Shader shader = Shader.Find("Brave/ItemSpecific/MetalSkinLayerShader");
                    MeshRenderer component = aiactor.sprite.GetComponent<MeshRenderer>();
                    Material[] sharedMaterials = component.sharedMaterials;
                    for (int i = 0; i < sharedMaterials.Length; i++)

                    {
                        if (sharedMaterials[i].shader == shader)
                        {
                            return;
                        }
                    }
                    Array.Resize<Material>(ref sharedMaterials, sharedMaterials.Length + 1);
                    Material material2 = new Material(shader);
                    material2.SetTexture("_MainTex", sharedMaterials[0].GetTexture("_MainTex"));
                    sharedMaterials[sharedMaterials.Length - 1] = material2;
                    component.sharedMaterials = sharedMaterials;
                    tk2dSprite.ForceBuild();
                }
            }
        }

        public override void PostProcessProjectile(Projectile projectile)
        {
            projectile.OnHitEnemy += this.OnProjectileHitEnemy;
        }
        public override void OnPostFired(PlayerController player, Gun gun)
        {
            //This determines what sound you want to play when you fire a gun.
            //Sounds names are based on the Gungeon sound dump, which can be found at EnterTheGungeon/Etg_Data/StreamingAssets/Audio/GeneratedSoundBanks/Windows/sfx.txt
            gun.PreventNormalFireAudio = true;
            AkSoundEngine.PostEvent("Play_WPN_smileyrevolver_shot_01", gameObject);
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
                HasReloaded = false;
                AkSoundEngine.PostEvent("Stop_WPN_All", base.gameObject);
                base.OnReloadPressed(player, gun, bSOMETHING);
                AkSoundEngine.PostEvent("Play_WPN_SAA_reload_01", base.gameObject);
            }
        }

        //All that's left now is sprite stuff. 
        //Your sprites should be organized, like how you see in the mod folder. 
        //Every gun requires that you have a .json to match the sprites or else the gun won't spawn at all
        //.Json determines the hand sprites for your character. You can make a gun two handed by having both "SecondaryHand" and "PrimaryHand" in the .json file, which can be edited through Notepad or Visual Studios
        //By default this gun is a one-handed weapon
        //If you need a basic two handed .json. Just use the jpxfrd2.json.
        //And finally, don't forget to add your Gun to your ETGModule class!
    }
}

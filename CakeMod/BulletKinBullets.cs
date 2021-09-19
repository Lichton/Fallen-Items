using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;

namespace CakeMod
{
    class CakeBullets : PassiveItem
    {

        public static void Init()
        {
            string itemName = "Confetti Bullets";
            string resourceName = "CakeMod/Resources/CakeBullets";

            GameObject obj = new GameObject(itemName);
            var item = obj.AddComponent<CakeBullets>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Confetti, Don't Confretti";
            string longDesc = "Shards of pure magic embed themselves in your enemies.\n\n" +
                "They shine in an odd manner, looking like glitter.\n" +
                "The confetti is painted on.";
            Gun gun = PickupObjectDatabase.GetById(28) as Gun;
            //e
            if (gun != null)
            {
                ProjectileVolleyData volley = gun.DefaultModule.finalVolley;
                if (volley != null && volley.projectiles != null)
                {
                    foreach (ProjectileModule mod in volley.projectiles)
                    {
                        if (mod != null && mod.projectiles != null)
                        {
                            foreach (Projectile proj in mod.projectiles)
                            {
                                if (proj != null && proj.GetComponent<ShaderProjModifier>() != null)
                                {
                                    shaderProjMod = proj.GetComponent<ShaderProjModifier>();
                                }
                            }
                        }
                    }
                }
            }

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cak");

            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Damage, 0.4f, StatModifier.ModifyMethod.ADDITIVE);

            item.quality = PickupObject.ItemQuality.A;
        }

        public static ShaderProjModifier shaderProjMod;

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.PostProcessProjectile += this.OnPostProcessProjectile;
        }

        public void OnPostProcessProjectile(Projectile proj, float f)
        {
            if (shaderProjMod != null)
            {
                ShaderProjModifier mod = proj.gameObject.AddComponent<ShaderProjModifier>();
                mod.ProcessProperty = shaderProjMod.ProcessProperty;
                mod.ShaderProperty = shaderProjMod.ShaderProperty;
                mod.StartValue = shaderProjMod.StartValue;
                mod.EndValue = shaderProjMod.EndValue;
                mod.LerpTime = shaderProjMod.LerpTime;
                mod.ColorAttribute = shaderProjMod.ColorAttribute;
                mod.StartColor = shaderProjMod.StartColor;
                mod.EndColor = shaderProjMod.EndColor;
                mod.OnDeath = shaderProjMod.OnDeath;
                mod.PreventCorpse = shaderProjMod.PreventCorpse;
                mod.DisablesOutlines = shaderProjMod.DisablesOutlines;
                mod.EnableEmission = shaderProjMod.EnableEmission;
                mod.GlobalSparks = shaderProjMod.GlobalSparks;
                mod.GlobalSparksColor = shaderProjMod.GlobalSparksColor;
                mod.GlobalSparksForce = shaderProjMod.GlobalSparksForce;
                mod.GlobalSparksOverrideLifespan = shaderProjMod.GlobalSparksOverrideLifespan;
                mod.AddMaterialPass = shaderProjMod.AddMaterialPass;
                mod.AddPass = shaderProjMod.AddPass;
                mod.IsGlitter = shaderProjMod.IsGlitter;
                mod.ShouldAffectBosses = shaderProjMod.ShouldAffectBosses;
                mod.AddsEncircler = shaderProjMod.AddsEncircler;
                mod.AppliesLocalSlowdown = shaderProjMod.AppliesLocalSlowdown;
                mod.LocalTimescaleMultiplier = shaderProjMod.LocalTimescaleMultiplier;
                mod.AppliesParticleSystem = shaderProjMod.AppliesParticleSystem;
                mod.ParticleSystemToSpawn = shaderProjMod.ParticleSystemToSpawn;
                mod.GlobalSparkType = shaderProjMod.GlobalSparkType;
                mod.GlobalSparksRepeat = shaderProjMod.GlobalSparksRepeat;
            }
        }

        public override DebrisObject Drop(PlayerController player)
        {
            player.PostProcessProjectile -= this.OnPostProcessProjectile;
            return base.Drop(player);
        }
    }
}

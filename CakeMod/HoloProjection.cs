
using Dungeonator;
using ItemAPI;
using UnityEngine;
using System.Collections;
using Gungeon;
using MonoMod;
using BasicGun;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Reflection;

using MonoMod.RuntimeDetour;
class HoloProjection : PassiveItem
{

    public static void Init()
    {
        string itemName = "Holo-Projection";
        string resourceName = "CakeMod/Resources/HoloProjection";

        GameObject obj = new GameObject(itemName);
        var item = obj.AddComponent<HoloProjection>();

        ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

        string shortDesc = "T31E-vizion";
        string longDesc = "Creates a holographic cloak around you that enhances your strength, but breaks on hit. The holoprojectors regenerate the shell in 10 seconds.";
            


        ItemBuilder.SetupItem(item, shortDesc, longDesc, "cak");

        item.quality = PickupObject.ItemQuality.S;
        item.CanBeDropped = false;

    }

    
    public override void Pickup(PlayerController player)
    {
        base.Pickup(player);
        Material material = new Material(this.m_glintShader);
        material.shader = ShaderCache.Acquire("Brave/Internal/HologramShader");
        Material[] playerMats = player.SetOverrideShader(m_glintShader);
        player.OnReceivedDamage += this.WackaWacka;
        AkSoundEngine.PostEvent("Play_OBJ_lightning_flash_01", base.gameObject);
        this.AddStat(PlayerStats.StatType.Damage, 1.5f, StatModifier.ModifyMethod.MULTIPLICATIVE);
        this.AddStat(PlayerStats.StatType.MovementSpeed, 1.5f, StatModifier.ModifyMethod.MULTIPLICATIVE);
        player.stats.RecalculateStats(player, true, false);
    }
    
   

    private void WackaWacka(PlayerController player)
    {
        if (BreakCheck == true)
        {


            Owner.ClearOverrideShader();
            this.RemoveStat(PlayerStats.StatType.Damage);
            this.RemoveStat(PlayerStats.StatType.MovementSpeed);
            player.stats.RecalculateStats(player, true, false);
            base.StartCoroutine(this.StartCooldown());
            AkSoundEngine.PostEvent("Play_OBJ_lightning_flash_01", base.gameObject);
            BreakCheck = false;
        }
    }

    private void AddStat(PlayerStats.StatType statType, float amount, StatModifier.ModifyMethod method = StatModifier.ModifyMethod.ADDITIVE)
    {
        StatModifier statModifier = new StatModifier
        {
            amount = amount,
            statToBoost = statType,
            modifyType = method
        };
        bool flag = this.passiveStatModifiers == null;
        if (flag)
        {
            this.passiveStatModifiers = new StatModifier[]
            {
                    statModifier
            };
        }
        else
        {
            this.passiveStatModifiers = this.passiveStatModifiers.Concat(new StatModifier[]
            {
                    statModifier
            }).ToArray<StatModifier>();
        }
    }

    private void RemoveStat(PlayerStats.StatType statType)
    {
        List<StatModifier> list = new List<StatModifier>();
        for (int i = 0; i < this.passiveStatModifiers.Length; i++)
        {
            bool flag = this.passiveStatModifiers[i].statToBoost != statType;
            if (flag)
            {
                list.Add(this.passiveStatModifiers[i]);
            }
        }
        this.passiveStatModifiers = list.ToArray();
    }
    private IEnumerator StartCooldown()
    {
        yield return new WaitForSeconds(10f);
        this.AddStat(PlayerStats.StatType.Damage, 1.5f, StatModifier.ModifyMethod.MULTIPLICATIVE);
        this.AddStat(PlayerStats.StatType.MovementSpeed, 1.5f, StatModifier.ModifyMethod.MULTIPLICATIVE);
        Owner.stats.RecalculateStats(Owner, true, false);
        Material material = new Material(this.m_glintShader);
        material.shader = ShaderCache.Acquire("Brave/Internal/HologramShader");
        Material[] playerMats = Owner.SetOverrideShader(m_glintShader);
        AkSoundEngine.PostEvent("Play_OBJ_lightning_flash_01", base.gameObject);
        BreakCheck = true;
        yield break;
    }
    public override DebrisObject Drop(PlayerController player)
    {
        player.ClearOverrideShader();
        player.OnReceivedDamage -= this.WackaWacka;
        return base.Drop(player);
    }
    private Shader m_glintShader = ShaderCache.Acquire("Brave/Internal/HologramShader");
    public bool BreakCheck = true; 
} 
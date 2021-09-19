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
class Taurus : PassiveItem
{

    public static void Init()
    {
        string itemName = "Bull Horn";
        string resourceName = "CakeMod/Resources/Horn";

        GameObject obj = new GameObject(itemName);
        var item = obj.AddComponent<Taurus>();

        ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

        string shortDesc = "Rage is Building!";
        string longDesc = "Allows the user to gain speed over time per room, then fly into a rage temporarily.";

        

        ItemBuilder.SetupItem(item, shortDesc, longDesc, "cak");

        item.quality = PickupObject.ItemQuality.S;
        item.CanBeDropped = false;

    }


    public override void Pickup(PlayerController player)
    {
        base.Pickup(player);
        
        player.OnEnteredCombat += this.WackaWacka;
        
        player.stats.RecalculateStats(player, true, false);
    }

    private void WackaWacka()
    {
        base.StartCoroutine(this.StartCooldown());
        base.StartCoroutine(this.StopTheSpeed());
        if(DoTheThing == false)
        {
            DoTheThing = true;
            this.RemoveStat(PlayerStats.StatType.MovementSpeed);
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
        yield return new WaitForSeconds(0.1f);
        Owner.stats.RecalculateStats(Owner, true, false);
        if (DoTheThing == true)
        {
            this.AddStat(PlayerStats.StatType.MovementSpeed, 1.005f, StatModifier.ModifyMethod.MULTIPLICATIVE);
        }
        Owner.stats.RecalculateStats(Owner, true, false);
        yield return new WaitForSeconds(0.1f);
        Owner.stats.RecalculateStats(Owner, true, false);
        if (DoTheThing == true)
        {
            this.AddStat(PlayerStats.StatType.MovementSpeed, 1.005f, StatModifier.ModifyMethod.MULTIPLICATIVE);
        }
        Owner.stats.RecalculateStats(Owner, true, false); yield return new WaitForSeconds(0.1f);
        Owner.stats.RecalculateStats(Owner, true, false);
        if (DoTheThing == true)
        {
            this.AddStat(PlayerStats.StatType.MovementSpeed, 1.005f, StatModifier.ModifyMethod.MULTIPLICATIVE);
        }
        Owner.stats.RecalculateStats(Owner, true, false); yield return new WaitForSeconds(0.1f);
        Owner.stats.RecalculateStats(Owner, true, false);
        if (DoTheThing == true)
        {
            this.AddStat(PlayerStats.StatType.MovementSpeed, 1.005f, StatModifier.ModifyMethod.MULTIPLICATIVE);
        }
        Owner.stats.RecalculateStats(Owner, true, false); yield return new WaitForSeconds(0.1f);
        Owner.stats.RecalculateStats(Owner, true, false);
        if (DoTheThing == true)
        {
            this.AddStat(PlayerStats.StatType.MovementSpeed, 1.005f, StatModifier.ModifyMethod.MULTIPLICATIVE);
        }
        Owner.stats.RecalculateStats(Owner, true, false); yield return new WaitForSeconds(0.1f);
        Owner.stats.RecalculateStats(Owner, true, false);
        if (DoTheThing == true)
        {
            this.AddStat(PlayerStats.StatType.MovementSpeed, 1.005f, StatModifier.ModifyMethod.MULTIPLICATIVE);
        }
        Owner.stats.RecalculateStats(Owner, true, false); yield return new WaitForSeconds(0.1f);
        Owner.stats.RecalculateStats(Owner, true, false);
        if (DoTheThing == true)
        {
            this.AddStat(PlayerStats.StatType.MovementSpeed, 1.005f, StatModifier.ModifyMethod.MULTIPLICATIVE);
        }
        Owner.stats.RecalculateStats(Owner, true, false); yield return new WaitForSeconds(0.1f);
        Owner.stats.RecalculateStats(Owner, true, false);
        if (DoTheThing == true)
        {
            this.AddStat(PlayerStats.StatType.MovementSpeed, 1.005f, StatModifier.ModifyMethod.MULTIPLICATIVE);
        }
        Owner.stats.RecalculateStats(Owner, true, false); yield return new WaitForSeconds(0.1f);
        Owner.stats.RecalculateStats(Owner, true, false);
        if (DoTheThing == true)
        {
            this.AddStat(PlayerStats.StatType.MovementSpeed, 1.005f, StatModifier.ModifyMethod.MULTIPLICATIVE);
        }
        Owner.stats.RecalculateStats(Owner, true, false); yield return new WaitForSeconds(0.1f);
        Owner.stats.RecalculateStats(Owner, true, false);
        if (DoTheThing == true)
        {
            this.AddStat(PlayerStats.StatType.MovementSpeed, 1.005f, StatModifier.ModifyMethod.MULTIPLICATIVE);
        }
        Owner.stats.RecalculateStats(Owner, true, false);
        yield return new WaitForSeconds(0.1f);
        Owner.stats.RecalculateStats(Owner, true, false);
        if (DoTheThing == true)
        {
            this.AddStat(PlayerStats.StatType.MovementSpeed, 1.005f, StatModifier.ModifyMethod.MULTIPLICATIVE);
        }
        Owner.stats.RecalculateStats(Owner, true, false);
        yield return new WaitForSeconds(0.1f);
        Owner.stats.RecalculateStats(Owner, true, false);
        if (DoTheThing == true)
        {
            this.AddStat(PlayerStats.StatType.MovementSpeed, 1.005f, StatModifier.ModifyMethod.MULTIPLICATIVE);
        }
        Owner.stats.RecalculateStats(Owner, true, false); yield return new WaitForSeconds(0.1f);
        Owner.stats.RecalculateStats(Owner, true, false);
        if (DoTheThing == true)
        {
            this.AddStat(PlayerStats.StatType.MovementSpeed, 1.005f, StatModifier.ModifyMethod.MULTIPLICATIVE);
        }
        Owner.stats.RecalculateStats(Owner, true, false); yield return new WaitForSeconds(0.1f);
        Owner.stats.RecalculateStats(Owner, true, false);
        if (DoTheThing == true)
        {
            this.AddStat(PlayerStats.StatType.MovementSpeed, 1.005f, StatModifier.ModifyMethod.MULTIPLICATIVE);
        }
        Owner.stats.RecalculateStats(Owner, true, false); yield return new WaitForSeconds(0.1f);
        Owner.stats.RecalculateStats(Owner, true, false);
        if (DoTheThing == true)
        {
            this.AddStat(PlayerStats.StatType.MovementSpeed, 1.005f, StatModifier.ModifyMethod.MULTIPLICATIVE);
        }
        Owner.stats.RecalculateStats(Owner, true, false); yield return new WaitForSeconds(0.1f);
        Owner.stats.RecalculateStats(Owner, true, false);
        if (DoTheThing == true)
        {
            this.AddStat(PlayerStats.StatType.MovementSpeed, 1.005f, StatModifier.ModifyMethod.MULTIPLICATIVE);
        }
        Owner.stats.RecalculateStats(Owner, true, false); yield return new WaitForSeconds(0.1f);
        Owner.stats.RecalculateStats(Owner, true, false);
        if (DoTheThing == true)
        {
            this.AddStat(PlayerStats.StatType.MovementSpeed, 1.005f, StatModifier.ModifyMethod.MULTIPLICATIVE);
        }
        Owner.stats.RecalculateStats(Owner, true, false); yield return new WaitForSeconds(0.1f);
        Owner.stats.RecalculateStats(Owner, true, false);
        if (DoTheThing == true)
        {
            this.AddStat(PlayerStats.StatType.MovementSpeed, 1.005f, StatModifier.ModifyMethod.MULTIPLICATIVE);
        }
        Owner.stats.RecalculateStats(Owner, true, false); yield return new WaitForSeconds(0.1f);
        Owner.stats.RecalculateStats(Owner, true, false);
        if (DoTheThing == true)
        {
            this.AddStat(PlayerStats.StatType.MovementSpeed, 1.005f, StatModifier.ModifyMethod.MULTIPLICATIVE);
        }
        Owner.stats.RecalculateStats(Owner, true, false); yield return new WaitForSeconds(0.1f);
        Owner.stats.RecalculateStats(Owner, true, false);
        if (DoTheThing == true)
        {
            this.AddStat(PlayerStats.StatType.MovementSpeed, 1.005f, StatModifier.ModifyMethod.MULTIPLICATIVE);
        }
        Owner.stats.RecalculateStats(Owner, true, false); yield return new WaitForSeconds(0.1f);
        Owner.stats.RecalculateStats(Owner, true, false);
        if (DoTheThing == true)
        {
            this.AddStat(PlayerStats.StatType.MovementSpeed, 1.005f, StatModifier.ModifyMethod.MULTIPLICATIVE);
        }
        Owner.stats.RecalculateStats(Owner, true, false);
        yield return new WaitForSeconds(0.1f);
        Owner.stats.RecalculateStats(Owner, true, false);
        if (DoTheThing == true)
        {
            this.AddStat(PlayerStats.StatType.MovementSpeed, 1.005f, StatModifier.ModifyMethod.MULTIPLICATIVE);
        }
        Owner.stats.RecalculateStats(Owner, true, false); yield return new WaitForSeconds(0.1f);
        Owner.stats.RecalculateStats(Owner, true, false);
        if (DoTheThing == true)
        {
            this.AddStat(PlayerStats.StatType.MovementSpeed, 1.005f, StatModifier.ModifyMethod.MULTIPLICATIVE);
        }
        Owner.stats.RecalculateStats(Owner, true, false); yield return new WaitForSeconds(0.1f);
        Owner.stats.RecalculateStats(Owner, true, false);
        if (DoTheThing == true)
        {
            this.AddStat(PlayerStats.StatType.MovementSpeed, 1.005f, StatModifier.ModifyMethod.MULTIPLICATIVE);
        }
        Owner.stats.RecalculateStats(Owner, true, false); yield return new WaitForSeconds(0.1f);
        Owner.stats.RecalculateStats(Owner, true, false);
        if (DoTheThing == true)
        {
            this.AddStat(PlayerStats.StatType.MovementSpeed, 1.005f, StatModifier.ModifyMethod.MULTIPLICATIVE);
        }
        Owner.stats.RecalculateStats(Owner, true, false); yield return new WaitForSeconds(0.1f);
        Owner.stats.RecalculateStats(Owner, true, false);
        if (DoTheThing == true)
        {
            this.AddStat(PlayerStats.StatType.MovementSpeed, 1.005f, StatModifier.ModifyMethod.MULTIPLICATIVE);
        }
        Owner.stats.RecalculateStats(Owner, true, false); yield return new WaitForSeconds(0.1f);
        Owner.stats.RecalculateStats(Owner, true, false);
        if (DoTheThing == true)
        {
            this.AddStat(PlayerStats.StatType.MovementSpeed, 1.005f, StatModifier.ModifyMethod.MULTIPLICATIVE);
        }
        Owner.stats.RecalculateStats(Owner, true, false); yield return new WaitForSeconds(0.1f);
        Owner.stats.RecalculateStats(Owner, true, false);
        if (DoTheThing == true)
        {
            this.AddStat(PlayerStats.StatType.MovementSpeed, 1.005f, StatModifier.ModifyMethod.MULTIPLICATIVE);
        }
        Owner.stats.RecalculateStats(Owner, true, false); yield return new WaitForSeconds(0.1f);
        Owner.stats.RecalculateStats(Owner, true, false);
        if (DoTheThing == true)
        {
            this.AddStat(PlayerStats.StatType.MovementSpeed, 1.005f, StatModifier.ModifyMethod.MULTIPLICATIVE);
        }
        Owner.stats.RecalculateStats(Owner, true, false); yield return new WaitForSeconds(0.1f);
        Owner.stats.RecalculateStats(Owner, true, false);
        if (DoTheThing == true)
        {
            this.AddStat(PlayerStats.StatType.MovementSpeed, 1.005f, StatModifier.ModifyMethod.MULTIPLICATIVE);
        }
        Owner.stats.RecalculateStats(Owner, true, false); yield return new WaitForSeconds(0.1f);
        Owner.stats.RecalculateStats(Owner, true, false);
        if (DoTheThing == true)
        {
            this.AddStat(PlayerStats.StatType.MovementSpeed, 1.005f, StatModifier.ModifyMethod.MULTIPLICATIVE);
        }
        Owner.stats.RecalculateStats(Owner, true, false);
        yield return new WaitForSeconds(0.1f);
        Owner.stats.RecalculateStats(Owner, true, false);
        if (DoTheThing == true)
        {
            this.AddStat(PlayerStats.StatType.MovementSpeed, 1.005f, StatModifier.ModifyMethod.MULTIPLICATIVE);
        }
        Owner.stats.RecalculateStats(Owner, true, false); yield return new WaitForSeconds(0.1f);
        Owner.stats.RecalculateStats(Owner, true, false);
        if (DoTheThing == true)
        {
            this.AddStat(PlayerStats.StatType.MovementSpeed, 1.005f, StatModifier.ModifyMethod.MULTIPLICATIVE);
        }
        Owner.stats.RecalculateStats(Owner, true, false); yield return new WaitForSeconds(0.1f);
        Owner.stats.RecalculateStats(Owner, true, false);
        if (DoTheThing == true)
        {
            this.AddStat(PlayerStats.StatType.MovementSpeed, 1.005f, StatModifier.ModifyMethod.MULTIPLICATIVE);
        }
        Owner.stats.RecalculateStats(Owner, true, false); yield return new WaitForSeconds(0.1f);
        Owner.stats.RecalculateStats(Owner, true, false);
        if (DoTheThing == true)
        {
            this.AddStat(PlayerStats.StatType.MovementSpeed, 1.005f, StatModifier.ModifyMethod.MULTIPLICATIVE);
        }
        Owner.stats.RecalculateStats(Owner, true, false); yield return new WaitForSeconds(0.1f);
        Owner.stats.RecalculateStats(Owner, true, false);
        if (DoTheThing == true)
        {
            this.AddStat(PlayerStats.StatType.MovementSpeed, 1.005f, StatModifier.ModifyMethod.MULTIPLICATIVE);
        }
        Owner.stats.RecalculateStats(Owner, true, false); yield return new WaitForSeconds(0.1f);
        Owner.stats.RecalculateStats(Owner, true, false);
        if (DoTheThing == true)
        {
            this.AddStat(PlayerStats.StatType.MovementSpeed, 1.005f, StatModifier.ModifyMethod.MULTIPLICATIVE);
        }
        Owner.stats.RecalculateStats(Owner, true, false); yield return new WaitForSeconds(0.1f);
        Owner.stats.RecalculateStats(Owner, true, false);
        if (DoTheThing == true)
        {
            this.AddStat(PlayerStats.StatType.MovementSpeed, 1.005f, StatModifier.ModifyMethod.MULTIPLICATIVE);
        }
        Owner.stats.RecalculateStats(Owner, true, false); yield return new WaitForSeconds(0.1f);
        Owner.stats.RecalculateStats(Owner, true, false);
        if (DoTheThing == true)
        {
            this.AddStat(PlayerStats.StatType.MovementSpeed, 1.005f, StatModifier.ModifyMethod.MULTIPLICATIVE);
        }
        Owner.stats.RecalculateStats(Owner, true, false); yield return new WaitForSeconds(0.1f);
        Owner.stats.RecalculateStats(Owner, true, false);
        if (DoTheThing == true)
        {
            this.AddStat(PlayerStats.StatType.MovementSpeed, 1.005f, StatModifier.ModifyMethod.MULTIPLICATIVE);
        }
        Owner.stats.RecalculateStats(Owner, true, false); yield return new WaitForSeconds(0.1f);
        Owner.stats.RecalculateStats(Owner, true, false);
        if (DoTheThing == true)
        {
            this.AddStat(PlayerStats.StatType.MovementSpeed, 1.005f, StatModifier.ModifyMethod.MULTIPLICATIVE);
        }
        Owner.stats.RecalculateStats(Owner, true, false);
        yield return new WaitForSeconds(0.1f);
        Owner.stats.RecalculateStats(Owner, true, false);
        if (DoTheThing == true)
        {
            this.AddStat(PlayerStats.StatType.MovementSpeed, 1.005f, StatModifier.ModifyMethod.MULTIPLICATIVE);
        }
        Owner.stats.RecalculateStats(Owner, true, false); yield return new WaitForSeconds(0.1f);
        Owner.stats.RecalculateStats(Owner, true, false);
        if (DoTheThing == true)
        {
            this.AddStat(PlayerStats.StatType.MovementSpeed, 1.005f, StatModifier.ModifyMethod.MULTIPLICATIVE);
        }
        Owner.stats.RecalculateStats(Owner, true, false); yield return new WaitForSeconds(0.1f);
        Owner.stats.RecalculateStats(Owner, true, false);
        if (DoTheThing == true)
        {
            this.AddStat(PlayerStats.StatType.MovementSpeed, 1.005f, StatModifier.ModifyMethod.MULTIPLICATIVE);
        }
        Owner.stats.RecalculateStats(Owner, true, false); yield return new WaitForSeconds(0.1f);
        Owner.stats.RecalculateStats(Owner, true, false);
        if (DoTheThing == true)
        {
            this.AddStat(PlayerStats.StatType.MovementSpeed, 1.005f, StatModifier.ModifyMethod.MULTIPLICATIVE);
        }
        Owner.stats.RecalculateStats(Owner, true, false); yield return new WaitForSeconds(0.1f);
        Owner.stats.RecalculateStats(Owner, true, false);
        if (DoTheThing == true)
        {
            this.AddStat(PlayerStats.StatType.MovementSpeed, 1.005f, StatModifier.ModifyMethod.MULTIPLICATIVE);
        }
        Owner.stats.RecalculateStats(Owner, true, false); yield return new WaitForSeconds(0.1f);
        Owner.stats.RecalculateStats(Owner, true, false);
        if (DoTheThing == true)
        {
            this.AddStat(PlayerStats.StatType.MovementSpeed, 1.005f, StatModifier.ModifyMethod.MULTIPLICATIVE);
        }
        Owner.stats.RecalculateStats(Owner, true, false); yield return new WaitForSeconds(0.1f);
        Owner.stats.RecalculateStats(Owner, true, false);
        if (DoTheThing == true)
        {
            this.AddStat(PlayerStats.StatType.MovementSpeed, 1.005f, StatModifier.ModifyMethod.MULTIPLICATIVE);
        }
        Owner.stats.RecalculateStats(Owner, true, false); yield return new WaitForSeconds(0.1f);
        Owner.stats.RecalculateStats(Owner, true, false);
        if (DoTheThing == true)
        {
            this.AddStat(PlayerStats.StatType.MovementSpeed, 1.005f, StatModifier.ModifyMethod.MULTIPLICATIVE);
        }
        Owner.stats.RecalculateStats(Owner, true, false); yield return new WaitForSeconds(0.1f);
        Owner.stats.RecalculateStats(Owner, true, false);
        if (DoTheThing == true)
        {
            this.AddStat(PlayerStats.StatType.MovementSpeed, 1.005f, StatModifier.ModifyMethod.MULTIPLICATIVE);
        }
        Owner.stats.RecalculateStats(Owner, true, false);
        yield break;
        
    }

    private IEnumerator StopTheSpeed()
    {
        yield return new WaitForSeconds(5f);
        DoTheThing = false;
        StartCoroutine(this.InflictRage(Owner));
        yield break;
    }

    private System.Collections.IEnumerator InflictRage(PlayerController player)
    {
        bool flag = this.rageActive;
        if (flag)
        {
            base.StopCoroutine(this.removeRageCoroutine);
            this.RemoveStat(PlayerStats.StatType.Damage);
            player.stats.RecalculateStats(player, true, false);
            this.rageActive = false;
        }
        player.stats.RecalculateStats(player, true, false);
        RagePassiveItem rageitem = PickupObjectDatabase.GetById(353).GetComponent<RagePassiveItem>();
        this.RageOverheadVFX = rageitem.OverheadVFX.gameObject;
        this.instanceVFX = base.Owner.PlayEffectOnActor(this.RageOverheadVFX, new Vector3(0f, 1.375f, 0f), true, true, false);
        this.AddStat(PlayerStats.StatType.Damage, 1.90f, StatModifier.ModifyMethod.MULTIPLICATIVE);
        player.stats.RecalculateStats(player, true, false);
        this.rageActive = true;
        float elapsed = 0f;
        float particleCounter = 0f;
        float Duration = 5f;
        while (elapsed < Duration)
        {
            elapsed += BraveTime.DeltaTime;
            base.Owner.baseFlatColorOverride = this.flatColorOverride.WithAlpha(Mathf.Lerp(this.flatColorOverride.a, 0f, Mathf.Clamp01(elapsed - (Duration - 1f))));
            bool flag2 = GameManager.Options.ShaderQuality != GameOptions.GenericHighMedLowOption.LOW && GameManager.Options.ShaderQuality != GameOptions.GenericHighMedLowOption.VERY_LOW && base.Owner && base.Owner.IsVisible && !base.Owner.IsFalling;
            if (flag2)
            {
                particleCounter += BraveTime.DeltaTime * 40f;
                bool flag3 = this.instanceVFX && elapsed > 1f;
                if (flag3)
                {
                    this.instanceVFX.GetComponent<tk2dSpriteAnimator>().PlayAndDestroyObject("rage_face_vfx_out", null);
                    this.instanceVFX = null;
                }
                bool flag4 = particleCounter > 1f;
                if (flag4)
                {
                    int num = Mathf.FloorToInt(particleCounter);
                    particleCounter %= 1f;
                    GlobalSparksDoer.DoRandomParticleBurst(num, base.Owner.sprite.WorldBottomLeft.ToVector3ZisY(0f), base.Owner.sprite.WorldTopRight.ToVector3ZisY(0f), Vector3.up, 90f, 0.5f, null, null, null, GlobalSparksDoer.SparksType.BLACK_PHANTOM_SMOKE);
                }
            }
            yield return null;
        }
        this.removeRageCoroutine = GameManager.Instance.StartCoroutine(this.RemoveRage(player));
        yield break;
    }


    private System.Collections.IEnumerator RemoveRage(PlayerController player)
    {
        this.stopRageCoroutineActive = true;
        bool flag = this.instanceVFX;
        if (flag)
        {
            this.instanceVFX.GetComponent<tk2dSpriteAnimator>().PlayAndDestroyObject("rage_face_vfx_out", null);
        }
        this.RemoveStat(PlayerStats.StatType.Damage);
        player.stats.RecalculateStats(player, true, false);
        this.rageActive = false;
        this.stopRageCoroutineActive = false;
        yield break;
    }

    public override DebrisObject Drop(PlayerController player)
    {
        player.ClearOverrideShader();
        player.OnEnteredCombat -= this.WackaWacka;
        return base.Drop(player);
    }
    private Shader m_glintShader = ShaderCache.Acquire("Brave/Internal/RainbowChestShader");
    public bool DoTheThing = true;

    public GameObject RageOverheadVFX;

    // Token: 0x04000176 RID: 374
    public bool rageActive = false;

    // Token: 0x04000177 RID: 375
    public bool stopRageCoroutineActive = false;

    // Token: 0x04000178 RID: 376
    private Coroutine removeRageCoroutine;

    // Token: 0x04000179 RID: 377
    public Color flatColorOverride = new Color(0.5f, 0f, 0f, 0.75f);

    // Token: 0x0400017A RID: 378
    private GameObject instanceVFX;
}
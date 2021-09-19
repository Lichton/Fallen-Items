using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;

namespace CakeMod
{
    class ChocolateBar : PlayerItem
    {
        //Call this method from the Start() method of your ETGModule extension class
        public static void Init()
        {
            //The name of the item
            string itemName = "Chocolate Bar";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it.
            string resourceName = "CakeMod/Resources/ChocolateBar";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a ActiveItem component to the object
            var item = obj.AddComponent<ChocolateBar>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "Ohm Nom Nom Nom";
            string longDesc = "This bar has strange unexplainable properties that temporarily vitalize anyone who eats it, but freezes anyone eating it in place.";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //"kts" here is the item pool. In the console you'd type kts:sweating_bullets
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cak");

            //Set the cooldown type and duration of the cooldown
            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.Damage, 2000);


            //Set some other fields
            item.consumable = false;
            item.quality = ItemQuality.B;
        }

        //Add the item's functionality down here! I stole most of this from the Stuffed Star active item code!
        float duration = 4f;
        protected override void DoEffect(PlayerController user)
        {
            base.LastOwner.PlayEffectOnActor(ResourceCache.Acquire("Global VFX/vfx_healing_sparkles_001") as GameObject, Vector3.zero, true, false, false);
            user.MovementModifiers += NoMotionModifier;
            user.IsStationary = true;
            GameManager.Instance.StartCoroutine(TheCountdown());
            GameManager.Instance.StartCoroutine(NoCustomSounds());
            GameManager.Instance.StartCoroutine(Giggles());
            GameManager.Instance.StartCoroutine(CultOfAPI());
        }
        protected override void OnPreDrop(PlayerController user)
        {

        }

        //Disable or enable the active whenever you need!
        public override bool CanBeUsed(PlayerController user)
        {
            return base.CanBeUsed(user);
        }
        private void AddStat(PlayerStats.StatType statType, float amount, StatModifier.ModifyMethod method = StatModifier.ModifyMethod.ADDITIVE)
        {
            StatModifier statModifier = new StatModifier();
            statModifier.amount = amount;
            statModifier.statToBoost = statType;
            statModifier.modifyType = method;
            foreach (StatModifier statModifier2 in this.passiveStatModifiers)
            {
                bool flag = statModifier2.statToBoost == statType;
                bool flag2 = flag;
                if (flag2)
                {
                    return;
                }
            }
            bool flag3 = this.passiveStatModifiers == null;
            bool flag4 = flag3;
            if (flag4)
            {
                this.passiveStatModifiers = new StatModifier[]
                {
                    statModifier
                };
                return;
            }
            this.passiveStatModifiers = this.passiveStatModifiers.Concat(new StatModifier[]
            {
                statModifier
            }).ToArray<StatModifier>();
        }

        // Token: 0x0600025B RID: 603 RVA: 0x00019B24 File Offset: 0x00017D24
        private void RemoveStat(PlayerStats.StatType statType)
        {
            List<StatModifier> list = new List<StatModifier>();
            for (int i = 0; i < this.passiveStatModifiers.Length; i++)
            {
                bool flag = this.passiveStatModifiers[i].statToBoost != statType;
                bool flag2 = flag;
                if (flag2)
                {
                    list.Add(this.passiveStatModifiers[i]);
                }
            }
            this.passiveStatModifiers = list.ToArray();
        }
        private void NoMotionModifier(ref Vector2 voluntaryVel, ref Vector2 involuntaryVel)
        {
            voluntaryVel = Vector2.zero;
        }
        private IEnumerator TheCountdown()
        {
            yield return new WaitForSeconds(4f);
            {
                base.LastOwner.IsStationary = false;
                base.LastOwner.MovementModifiers -= NoMotionModifier;
                AddStat(PlayerStats.StatType.Health, 1f, StatModifier.ModifyMethod.ADDITIVE);
                base.LastOwner.healthHaver.ApplyHealing(1);
                base.LastOwner.stats.RecalculateStats(base.LastOwner, true, false);
            }
            yield break;
        }

        private IEnumerator CultOfAPI()
        {
            yield return new WaitForSeconds(4.5f);
            {
                base.LastOwner.healthHaver.ApplyHealing(1);

            }
            yield break;
        }

        private IEnumerator NoCustomSounds()
        {
            yield return new WaitForSeconds(1f);
            {
                base.LastOwner.PlayEffectOnActor(ResourceCache.Acquire("Global VFX/vfx_healing_sparkles_001") as GameObject, Vector3.zero, true, false, false);
                yield return new WaitForSeconds(1f);
                base.LastOwner.PlayEffectOnActor(ResourceCache.Acquire("Global VFX/vfx_healing_sparkles_001") as GameObject, Vector3.zero, true, false, false);
                yield return new WaitForSeconds(1f);
                base.LastOwner.PlayEffectOnActor(ResourceCache.Acquire("Global VFX/vfx_healing_sparkles_001") as GameObject, Vector3.zero, true, false, false);
                yield return new WaitForSeconds(1f);
                base.LastOwner.PlayEffectOnActor(ResourceCache.Acquire("Global VFX/vfx_healing_sparkles_001") as GameObject, Vector3.zero, true, false, false);
            }
            yield break;
        }


            private IEnumerator Giggles()
            {
                yield return new WaitForSeconds(20f);
                {
                AddStat(PlayerStats.StatType.Health, -1f, StatModifier.ModifyMethod.ADDITIVE);
                this.RemoveStat(PlayerStats.StatType.Health);
                base.LastOwner.stats.RecalculateStats(base.LastOwner, true, false);
            }
                yield break;
            }

        }
    }

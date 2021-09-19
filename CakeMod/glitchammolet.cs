using System;
using Dungeonator;
using ItemAPI;
using UnityEngine;
using System.Collections;
using Gungeon;
using MonoMod;
using BasicGun;
using System.Collections.Generic;

namespace CakeMod
{
    // Token: 0x0200001E RID: 30
    internal class glitchammolet : BlankModificationItem
    {

        // Token: 0x060000C4 RID: 196 RVA: 0x0000AAD8 File Offset: 0x00008CD8
        public static void Init()
        {
            string itemName = "Glitch Ammolet";
            string resourceName = "CakeMod/Resources/glitch_ammolet";

            GameObject obj = new GameObject(itemName);
            var item = obj.AddComponent<glitchammolet>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "ammolet is null";
            string longDesc = "A strange ammolet forged in the lost chamber where the dual beholsters lay, waiting for a lone gunman to encounter them.";
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cak");
            item.quality = PickupObject.ItemQuality.EXCLUDED;
            item.PlaceItemInAmmonomiconAfterItemById(344);
            item.BlankStunTime = 1f;
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.AdditionalBlanksPerFloor, 1f, StatModifier.ModifyMethod.ADDITIVE);
            item.AddToSubShop(ItemBuilder.ShopType.OldRed, 1f);
        }

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.OnUsedBlank += this.OnUsedBlank;
            string materials = "Brave/Internal/Glitch";
            Material material = new Material(this.m_glintShader);
            material.shader = ShaderCache.Acquire("Brave/Internal/Glitch");
            material.SetFloat("_GlitchInterval", 0.7f);
            material.SetFloat("_DispProbability", 0.3f);
            material.SetFloat("_DispIntensity", 0.1f);
            material.SetFloat("_ColorProbability", 0.4f);
            material.SetFloat("_ColorIntensity", 0.3f);
            Material[] playerMats = player.SetOverrideShader(m_glintShader);
        }
        private void OnUsedBlank(PlayerController player, int blanksRemaining)
        {
            bool isInCombat = player.IsInCombat;
            if (isInCombat)
            {
                foreach (AIActor aiactor in player.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All))
                {
                    if (aiactor.healthHaver.IsBoss != true)
                    {

                        aiactor.sprite.usesOverrideMaterial = true;
                        Material material = new Material(this.m_glintShader);
                        material.shader = ShaderCache.Acquire("Brave/Internal/Glitch");
                        Shader shader = Shader.Find("Brave/ItemSpecific/Glitch");
                        aiactor.sprite.renderer.material = material;
                        MeshRenderer component = aiactor.sprite.GetComponent<MeshRenderer>();
                        int bighead = UnityEngine.Random.Range(1, 6);
                        if (bighead == 3 || bighead == 4 || bighead == 5)
                        {
                            aiactor.healthHaver.OnPreDeath += this.DropRandomConsumable;
                        }
                    }
                }
            }
        }

        private void DropRandomConsumable(Vector2 obj)
        {
            int bighead = UnityEngine.Random.Range(1, 14);
            if (bighead == 1)
            {
                LootEngine.SpawnItem(PickupObjectDatabase.GetById(73).gameObject, obj.ToVector3XUp(0), Vector2.down, 1f, false, true, false);

            }
            if (bighead == 2)
            {
                LootEngine.SpawnItem(PickupObjectDatabase.GetById(85).gameObject, obj.ToVector3XUp(0), Vector2.down, 1f, false, true, false);

            }
            if (bighead == 3)
            {

                LootEngine.SpawnItem(PickupObjectDatabase.GetById(68).gameObject, obj.ToVector3XUp(0), Vector2.down, 1f, false, true, false);
            }
            if (bighead == 4)
            {

                LootEngine.SpawnItem(PickupObjectDatabase.GetById(70).gameObject, obj.ToVector3XUp(0), Vector2.down, 1f, false, true, false);
            }
            if (bighead == 5)
            {
                LootEngine.SpawnItem(PickupObjectDatabase.GetById(120).gameObject, obj.ToVector3XUp(0), Vector2.down, 1f, false, true, false);

            }
            if (bighead == 6)
            {

                LootEngine.SpawnItem(PickupObjectDatabase.GetById(565).gameObject, obj.ToVector3XUp(0), Vector2.down, 1f, false, true, false);

            }
            if (bighead == 7)
            {

                LootEngine.SpawnItem(PickupObjectDatabase.GetById(600).gameObject, obj.ToVector3XUp(0), Vector2.down, 1f, false, true, false);

                if (bighead == 8)
                {
                    LootEngine.SpawnItem(PickupObjectDatabase.GetById(73).gameObject, obj.ToVector3XUp(0), Vector2.down, 1f, false, true, false);

                }
                if (bighead == 9)
                {
                    LootEngine.SpawnItem(PickupObjectDatabase.GetById(85).gameObject, obj.ToVector3XUp(0), Vector2.down, 1f, false, true, false);

                }
                if (bighead == 10)
                {

                    LootEngine.SpawnItem(PickupObjectDatabase.GetById(68).gameObject, obj.ToVector3XUp(0), Vector2.down, 1f, false, true, false);

                }
                if (bighead == 11)
                {

                    LootEngine.SpawnItem(PickupObjectDatabase.GetById(70).gameObject, obj.ToVector3XUp(0), Vector2.down, 1f, false, true, false);

                }
                if (bighead == 12)
                {
                    LootEngine.SpawnItem(PickupObjectDatabase.GetById(120).gameObject, obj.ToVector3XUp(0), Vector2.down, 1f, false, true, false);


                }
                if (bighead == 13)
                {

                    LootEngine.SpawnItem(PickupObjectDatabase.GetById(565).gameObject, obj.ToVector3XUp(0), Vector2.down, 1f, false, true, false);

                }

            }
        }

        public override DebrisObject Drop(PlayerController player)
        {
            player.OnUsedBlank -= this.OnUsedBlank;
            return base.Drop(player);
        }
        private Shader m_glintShader = ShaderCache.Acquire("Brave/Internal/Glitch");

    }
}

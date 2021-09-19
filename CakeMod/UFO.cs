using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;
using Dungeonator;
using System.Reflection;
using Random = System.Random;
using FullSerializer;
using System.Collections;
using Gungeon;
using MonoMod.RuntimeDetour;
using System.Xml.Serialization;

namespace CakeMod
{
    public class UFO : PlayerItem
    {
        public static void Init()
        {
            string itemName = "Astrohelm";
            string resourceName = "CakeMod/Resources/CosmoHelmet";
            GameObject obj = new GameObject(itemName);
            UFO cosmoItem = obj.AddComponent<UFO>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
            string shortDesc = "UFO";
            string longDesc = "A strange helmet with the skull of an unlucky gungeoneer floating inside. It seems to belong to a certain Cosmonaut.";
            cosmoItem.SetupItem(shortDesc, longDesc, "cak");
            cosmoItem.quality = PickupObject.ItemQuality.EXCLUDED;
            ItemBuilder.SetCooldownType(cosmoItem, ItemBuilder.CooldownType.Damage, 500);

        }



        protected override void DoEffect(PlayerController user)
        {
            this.CreateNewCompanion(base.LastOwner);
        }
         private List<CompanionController> companionsSpawned = new List<CompanionController>();
        private void CreateNewCompanion(PlayerController player)
        {

            {
                Vector3 vector = player.transform.position;
                GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(CosmoStatue.prefab, vector, Quaternion.identity);
                CompanionController orAddComponent = gameObject.GetOrAddComponent<CompanionController>();
                this.companionsSpawned.Add(orAddComponent);
                    orAddComponent.Initialize(player);
                }
            }
        public override void Pickup(PlayerController player)
        {
            
            base.Pickup(player);
        }

        protected override void OnPreDrop(PlayerController user)
        {

        }

        //Disable or enable the active whenever you need!
        public override bool CanBeUsed(PlayerController user)
        {
            return base.CanBeUsed(user);
        }
    }
}
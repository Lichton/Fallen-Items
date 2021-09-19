using System;
using System.Collections;
using Gungeon;
using MonoMod;
using UnityEngine;
using ItemAPI;
using BasicGun;
using System.Collections.Generic;
using Dungeonator;
using System.Reflection;
using MonoMod.RuntimeDetour;

namespace CakeMod
{
    class Rift : PlayerItem
    {
        //Call this method from the Start() method of your ETGModule extension class
        public static void Init()
        {
            //The name of the item
            string itemName = "Portable Rift Opener";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it.
            string resourceName = "CakeMod/Resources/jawbreaker";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a ActiveItem component to the object
            var item = obj.AddComponent<Rift>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "Hard Candy";
            string longDesc = "A strange, opaque white ball studded with gemstones. It would not be wise to attempt consumption.";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //"kts" here is the item pool. In the console you'd type kts:sweating_bullets
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cak");

            //Set the cooldown type and duration of the cooldown
            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.Timed, 3);


            //Set some other fields
            item.consumable = false;
            item.quality = ItemQuality.EXCLUDED;
            item.CanBeDropped = false;
        }

        //Add the item's functionality down here! I stole most of this from the Stuffed Star active item code!
        float duration = 10f;
        protected override void DoEffect(PlayerController user)
        {
            Rift.HoleObject = PickupObjectDatabase.GetById(155).GetComponent<SpawnObjectPlayerItem>();
            Rift hellcomponent = gameObject.GetComponent<Rift>();
            hellcomponent.synergyobject = Rift.HoleObject.objectToSpawn;
            BlackHoleDoer holer = synergyobject.GetComponent<BlackHoleDoer>();
            gameObject1 = UnityEngine.Object.Instantiate<GameObject>(holer.HellSynergyVFX, new Vector3(base.transform.position.x + 0.7f, base.transform.position.y - 0.3f, base.transform.position.z + 5f), Quaternion.Euler(0f, 0f, 0f)); ;
            MeshRenderer component = gameObject.GetComponent<MeshRenderer>();
            base.StartCoroutine(this.HoldPortalOpen(component, user.sprite.WorldCenter, gameObject1));


        }

        private void Notify(string header, string text)
        {
            tk2dSpriteCollectionData encounterIconCollection = AmmonomiconController.Instance.EncounterIconCollection;
            int spriteIdByName = encounterIconCollection.GetSpriteIdByName("CakeMod/Resources/StrangePotion");
            GameUIRoot.Instance.notificationController.DoCustomNotification(header, text, null, spriteIdByName, UINotificationController.NotificationColor.PURPLE, false, true);
        }
        private IEnumerator HoldPortalOpen(MeshRenderer component, Vector2 vector, GameObject gameObject1)
        {
            float elapsed = new float();
            while (component != null)
            {
                elapsed += BraveTime.DeltaTime;
                float t = Mathf.Clamp01(elapsed / 0.25f);
                component.material.SetFloat("_UVDistCutoff", Mathf.Lerp(0f, 0.21f, t));
                yield return null;
            }
            yield break;
        }



    protected override void OnPreDrop(PlayerController user)
        {

        }


        //Disable or enable the active whenever you need!
        public override bool CanBeUsed(PlayerController user)
        {
            return base.CanBeUsed(user);
        }
        private GameObject synergyobject;
        private GameObject gameObject1;
        private static SpawnObjectPlayerItem HoleObject;
    }
}
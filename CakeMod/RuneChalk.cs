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
    class RuneChalk : PlayerItem
    {
        //Call this method from the Start() method of your ETGModule extension class
        public static void Init()
        {
            //The name of the item
            string itemName = "Rune Chalk";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it.
            string resourceName = "CakeMod/Resources/RuneChalk";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a ActiveItem component to the object
            var item = obj.AddComponent<RuneChalk>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "Traps!";
            string longDesc = "Rune chalk often used by amatuer gungeoneers and ammomancers alike. It can be used to draw rudimentary runes with varying effects depending on their colour.";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //"kts" here is the item pool. In the console you'd type kts:sweating_bullets
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cak");

            //Set the cooldown type and duration of the cooldown
            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.Damage, 750);
            GameObject gameObject = SpriteBuilder.SpriteFromResource("CakeMod/Resources/RuneCircle", null, true);
            GameObject gameObject2 = new GameObject("Rune");

            tk2dSprite tk2dSprite = gameObject2.AddComponent<tk2dSprite>();
            tk2dSprite.SetSprite(gameObject.GetComponent<tk2dBaseSprite>().Collection, gameObject.GetComponent<tk2dBaseSprite>().spriteId);

            tk2dSprite.GetCurrentSpriteDef().material.shader = ShaderCache.Acquire("Brave/PlayerShader");
            FakePrefab.MarkAsFakePrefab(gameObject2);
            UnityEngine.Object.DontDestroyOnLoad(gameObject2);
            gameObject2.SetActive(false);
            RunePrefab = gameObject2;
            //Set some other fields
            item.consumable = false;
            item.quality = ItemQuality.B;
        }
        //Add the item's functionality down here! I stole most of this from the Stuffed Star active item code!
        public static List<int> spriteIds = new List<int>();
        public static GameObject RunePrefab;
        protected override void DoEffect(PlayerController user)
        {
            this.PlaceTrap(user);
      
        }
       
        public void PlaceTrap(PlayerController player)
        {
            Vector2 position = player.CurrentRoom.GetRandomAvailableCellDumb().ToVector2();
            GameObject theRune = UnityEngine.Object.Instantiate<GameObject>(RunePrefab, position, Quaternion.identity);
            theRune.GetComponent<tk2dBaseSprite>().PlaceAtLocalPositionByAnchor(position, tk2dBaseSprite.Anchor.LowerCenter);
            sprite = theRune.GetOrAddComponent<tk2dBaseSprite>();
            Material material = new Material(ShaderCache.Acquire("Brave/Internal/SimpleAlphaFadeUnlit"));
            material.SetTexture("_MainTex", sprite.renderer.material.mainTexture);
            material.SetTexture("_MaskTex", sprite.renderer.material.mainTexture);
            material.SetFloat("_Fade", 0.33f);
            sprite.renderer.material = material;
            sprite.renderer.sortingLayerName = "Floor";
            int bighead = UnityEngine.Random.Range(1, 10);
            if(bighead == 1)
            {
                sprite.color = Color.red;
            }
            if (bighead == 2)
            {
                sprite.color = Color.blue;
            }
            if (bighead == 3)
            {
                sprite.color = Color.green;
            }
            if (bighead == 4)
            {
                sprite.color = Color.yellow;
            }
            if (bighead == 5)
            {
                sprite.color =ExtendedColours.pink;
            }
            if (bighead == 6)
            {
                sprite.color = ExtendedColours.pink;
            }
            if (bighead == 7)
            {
                sprite.color = ExtendedColours.orange;
            }
            if (bighead == 8)
            {
                sprite.color = Color.black;
            }
            if (bighead == 9)
            {
                sprite.color = Color.white;
            }
        }
        protected override void OnPreDrop(PlayerController user)
        {

        }

        //Disable or enable the active whenever you need!
        public override bool CanBeUsed(PlayerController user)
        {
            return LastOwner.IsInCombat;
        }
    }
}


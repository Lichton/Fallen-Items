using ItemAPI;
using SaveAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CakeMod
{
    class RuneChalk2 : PassiveItem
    {
        public static void Init()
        {
            string name = "Rune Chalk";
            string resourcePath = "CakeMod/Resources/RuneChalk.png";
            GameObject gameObject = new GameObject(name);
            RuneChalk2 wyrmBlood = gameObject.AddComponent<RuneChalk2>();
            ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject);
            string shortDesc = "Traps!";
            string longDesc = "Rune chalk often used by amatuer gungeoneers and ammomancers alike. It can be used to draw rudimentary runes with varying effects depending on their colour.";
            GameObject gameObject2 = SpriteBuilder.SpriteFromResource("CakeMod/Resources/RuneCircle", null, true);
            FakePrefab.MarkAsFakePrefab(gameObject2);
            UnityEngine.Object.DontDestroyOnLoad(gameObject2);
            RunePrefab = gameObject2;
            wyrmBlood.SetupItem(shortDesc, longDesc, "cak");
            wyrmBlood.quality = PickupObject.ItemQuality.EXCLUDED;
        }
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.OnEnteredCombat += this.PlaceTrap;
        }

        // Token: 0x06000183 RID: 387 RVA: 0x0000CB3C File Offset: 0x0000AD3C
        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            return debrisObject;
        }
        public static GameObject RunePrefab;
        public void PlaceTrap()
        {
            PlayerController player = GameManager.Instance.PrimaryPlayer;
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
            if (bighead == 1)
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
                sprite.color = ExtendedColours.pink;
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
    }
}

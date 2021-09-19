using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;
using Dungeonator;


namespace CakeMod
{
    class Barrel_o_barrelsa : PlayerItem
    {
        //Call this method from the Start() method of your ETGModule extension class
        public static void Init()
        {
            //The name of the item
            string itemName = "Barrel O' Barrels";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it.
            string resourceName = "CakeMod/Resources/barrelbarrel";
            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a ActiveItem component to the object
            var item = obj.AddComponent<Barrel_o_barrelsa>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "Barrel to the Knee";
            string longDesc = "This barrel is fueled internally by the tortured souls of the Jammed. Despite having unlimited power, it can only create barrels.";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //"kts" here is the item pool. In the console you'd type kts:sweating_bullets
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cak");

            //Set the cooldown type and duration of the cooldown
            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.Damage, 100f);


            //Set some other fields
            item.consumable = false;
            item.quality = ItemQuality.EXCLUDED;
        }
        AssetBundle sharedAssets2 = ResourceManager.LoadAssetBundle("shared_auto_002");
        //Add the item's functionality down here! I stole most of this from the Stuffed Star active item code!
        float duration = 60f;
        protected override void DoEffect(PlayerController user)
        {
            float roomPosX = user.transform.position.x - user.CurrentRoom.area.basePosition.x;
            float roomPosY = user.transform.position.y - user.CurrentRoom.area.basePosition.y;
            float xOffSet = 1;
            float yOffSet = 0;
            Vector2 posInCurrentRoom = new Vector2(roomPosX + xOffSet, roomPosY + yOffSet);
            GameObject regularbarrel = sharedAssets2.LoadAsset<GameObject>("Barrel_collection");
            GameObject barrelPrefab = FakePrefab.Clone(regularbarrel);
            DungeonPlaceable woodbox = sharedAssets2.LoadAsset<DungeonPlaceable>("Barrel_collection");
            woodbox.InstantiateObject(user.CurrentRoom, posInCurrentRoom.ToIntVector2());
            bool flag = woodbox != null;
            if (flag)
            {               
                woodbox.InstantiateObject(user.CurrentRoom, posInCurrentRoom.ToIntVector2());
            }


        }

        protected override void OnPreDrop(PlayerController user)
        {

        }

        //Disable or enable the active whenever you need!
        public override bool CanBeUsed(PlayerController user)
        {
            return base.CanBeUsed(user);
        }

        public override void Update()
        {
            base.Update();
        }
    }
}
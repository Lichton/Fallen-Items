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
    class LichHat : PlayerItem
    {
        //Call this method from the Start() method of your ETGModule extension class
        public static void Init()
        {
            //The name of the item
            string itemName = "Eldritch Locket";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it.
            string resourceName = "CakeMod/Resources/EldritchPendant";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a ActiveItem component to the object
            var item = obj.AddComponent<LichHat>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "G.U Nonecraft";
            string longDesc = "A pendant tainted by otherworldly energies. Eldritch tentacles grasp at your foes.";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //"kts" here is the item pool. In the console you'd type kts:sweating_bullets
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cak");

            //Set the cooldown type and duration of the cooldown
            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.Damage, 750);


            //Set some other fields
            item.consumable = false;
            item.quality = ItemQuality.D;
        }

        //Add the item's functionality down here! I stole most of this from the Stuffed Star active item code!
        float duration = 10f;
        protected override void DoEffect(PlayerController user)
        {
            RoomHandler absoluteRoom = base.transform.position.GetAbsoluteRoom();
            randomActiveEnemy = absoluteRoom.GetRandomActiveEnemy(false);
            if (randomActiveEnemy.aiActor.EnemyGuid != "336190e29e8a4f75ab7486595b700d4a")
            {
                randomActiveEnemy.PlayEffectOnActor(EasyVFXDatabase.YellowChamberVFX, new Vector3(0f, -1f, 0f), false, false, false);
                randomActiveEnemy.behaviorSpeculator.Stun(1f, true);
                base.StartCoroutine(this.EatEm());
            }
        }

            private IEnumerator EatEm()
        {

            yield return new WaitForSeconds(1f);
            randomActiveEnemy.sprite.HeightOffGround = -1f;
            if (randomActiveEnemy.aiActor.EnemyGuid != "336190e29e8a4f75ab7486595b700d4a")
            {
                randomActiveEnemy.CanTargetEnemies = true;
                randomActiveEnemy.CanTargetPlayers = false;
                randomActiveEnemy.aiActor.Transmogrify(EnemyDatabase.GetOrLoadByGuid("336190e29e8a4f75ab7486595b700d4a"), (GameObject)ResourceCache.Acquire("Global VFX/VFX_Item_Spawn_Poof"));
                randomActiveEnemy.gameObject.AddComponent<KillOnRoomClear>();
                randomActiveEnemy.CanDropCurrency = false;
                randomActiveEnemy.HitByEnemyBullets = false;
                randomActiveEnemy.CollisionDamage = 0f;
                randomActiveEnemy.IsHarmlessEnemy = true;
                randomActiveEnemy.IgnoreForRoomClear = true;
                base.StartCoroutine(this.EatEm2());
                yield break;
            }
        }

        private IEnumerator EatEm2()
        {
            yield return new WaitForSeconds(1f);
            randomActiveEnemy.sprite.HeightOffGround = 0f;
            yield break;
        }
        protected override void OnPreDrop(PlayerController user)
        {

        }

        //Disable or enable the active whenever you need!
        public override bool CanBeUsed(PlayerController user)
        {
            return LastOwner.IsInCombat;
        }

        public AIActor randomActiveEnemy;
    }
}


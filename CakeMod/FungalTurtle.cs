using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;
using Gungeon;
using SaveAPI;
using Dungeonator;


namespace CakeMod
{
    class FungalTurtle : PassiveItem
    {

        public static void Init()
        {
            string itemName = "Fungal Turtle";
            string resourceName = "CakeMod/Resources/FungalTurtle";

            GameObject obj = new GameObject(itemName);
            var item = obj.AddComponent<FungalTurtle>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Turtle Terrorism";
            string longDesc = "A species of fungi bearing resemblance to a turtle.\n\n" +
                " This specific subspecies feeds off corpses to produce mobile, turtle-like spore clumps.";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cak");

            item.quality = PickupObject.ItemQuality.EXCLUDED;
            CakeIDs.TurtItem3 = item.PickupObjectId;
        }
        private void TransformTo(PlayerController player, string playerName)
        {
            PlayerController controller = ((GameObject)BraveResources.Load(playerName)).GetComponent<PlayerController>();
            Transform playerSprite = controller.transform.Find("PlayerSprite");
            tk2dBaseSprite sprite = playerSprite?.GetComponent<tk2dSprite>();

            var library = Instantiate(sprite.GetComponent<tk2dSpriteAnimator>().Library);
            DontDestroyOnLoad(library);

            var handSkin = controller.primaryHand.sprite.Collection;

            TurtSprites = ItemsMod.list;
            tk2dSpriteCollectionData turts = ItemsMod.HandleAnimations2(sprite, TurtSprites);
            if (turts != null)
            {
                handSkin = turts;
            }
            foreach (var clip in library.clips)
            {

                for (int i = 0; i < clip.frames.Length; i++)
                {
                    clip.frames[i].spriteCollection = turts;
                }
            }

            player.OverrideAnimationLibrary = library;
            player.ChangeHandsToCustomType(handSkin, controller.primaryHand.sprite.spriteId);
        }
        public override void Pickup(PlayerController player)
        {
            if (player.HasActiveItem(CakeIDs.TurtItem1) && player.HasPassiveItem(CakeIDs.TurtItem2) && player.HasPassiveItem(CakeIDs.TurtItem3) && player.HasPassiveItem(CakeIDs.TurtItem4))
            {
                TransformTo(player, "PlayerRogue");
            }
            player.OnKilledEnemyContext += this.GrowATurtle;
            base.Pickup(player);
        }
        private void CreateNewCompanion(PlayerController owner, Vector2 ARRRR, RoomHandler fuck)
        {
            int bighead = UnityEngine.Random.Range(1, 7);
            if (bighead == 1)
            {
                string guid = "cc9c41aa8c194e17b44ac45f993dd212";
                AIActor orLoadByGuid = EnemyDatabase.GetOrLoadByGuid(guid);
                Vector3 vector = ARRRR;
                GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(orLoadByGuid.gameObject, vector, Quaternion.identity);
                CompanionController orAddComponent = gameObject.GetOrAddComponent<CompanionController>();
                orAddComponent.aiActor.CollisionDamage = 0;
                orAddComponent.aiActor.ParentRoom = fuck;
                this.m_companions.Add(orAddComponent);
                orAddComponent.Initialize(owner);

                if (orAddComponent.specRigidbody)
                {
                    PhysicsEngine.Instance.RegisterOverlappingGhostCollisionExceptions(orAddComponent.specRigidbody, null, false);
                }
            }
            
        }
        private void GrowATurtle(PlayerController arg1, HealthHaver arg2)
        {
            AIActor orLoadByGuid = EnemyDatabase.GetOrLoadByGuid("cc9c41aa8c194e17b44ac45f993dd212");
            RoomHandler absoluteRoom = arg1.gameActor.CenterPosition.GetAbsoluteRoom();
            CreateNewCompanion(arg1, arg2.gameActor.CenterPosition, absoluteRoom);
            
            
            
        }

        private void EXPLODE(Vector2 obj)
        {
            Exploder.DoDefaultExplosion(obj, default(Vector2), null, false, CoreDamageTypes.None, true);
        }

        public override DebrisObject Drop(PlayerController player)
        {
            player.OnKilledEnemyContext -= this.GrowATurtle;
            return base.Drop(player);
        }
        public List<CompanionController> m_companions = new List<CompanionController>();
        public List<Texture2D> TurtSprites;
    }
}
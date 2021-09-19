using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;
using GungeonAPI;
using System.Reflection;

namespace CakeMod
{
    class  TurtsMelon : PassiveItem
    {

        public static void Init()
        {
            string itemName = "Mark Of The Turtle";
            string resourceName = "CakeMod/Resources/CosmoHelmet";

            GameObject obj = new GameObject(itemName);
            var item = obj.AddComponent<TurtsMelon>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Unidentified Flying Gun";
            string longDesc = "A relic of a lost gungeoneer, the Cosmonaut. It brings his likeness upon any who come in contact.";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cak");

            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Damage, 1f, StatModifier.ModifyMethod.ADDITIVE);

            item.quality = PickupObject.ItemQuality.EXCLUDED;
            item.CanBeDropped = false;
        }
        private void breakItem(PlayerController player)
        {
            base.Owner.RemovePassiveItem(this.PickupObjectId);
        }

        // Token: 0x060002B2 RID: 690 RVA: 0x00016DD0 File Offset: 0x00014FD0
        public override void Pickup(PlayerController player)
        {
            
            TransformTo(player, "PlayerRogue");

            base.Pickup(player);
        }

        public tk2dSpriteCollectionData cosmoAnimation;
        // Token: 0x060002B3 RID: 691 RVA: 0x00016DF0 File Offset: 0x00014FF0
        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject result = base.Drop(player);

            return result;
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

        private void UntransformPlayer(PlayerController player)
        {
            if (player.OverrideAnimationLibrary != null)
            {
                player.OverrideAnimationLibrary = null;
                player.RevertHandsToBaseType();
            }
        }


        // Token: 0x060002B4 RID: 692 RVA: 0x00016E1E File Offset: 0x0001501E
        protected override void OnDestroy()
        {

            base.OnDestroy();
        }
        public List<Texture2D> TurtSprites;
    }
}
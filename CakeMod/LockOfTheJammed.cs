using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;

namespace CakeMod
{
    class LockOfTheJammed : PassiveItem
    {

        public static void Init()
        {
            string itemName = "Lock Of The Jammed";
            string resourceName = "CakeMod/Resources/LockOfTheJammed";

            GameObject obj = new GameObject(itemName);
            var item = obj.AddComponent<LockOfTheJammed>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Skeyry!";
            string longDesc = "Grants the owner ability to open locks for free, as well as a free Reaper following the owner.\n\n" +
                "This mighty lock was once gold, but fell under the posession of the Jammed's Crown.\n" +
                "Now he's jammed too.";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cak");

        item.quality = PickupObject.ItemQuality.EXCLUDED;

            item.AddToSubShop(ItemBuilder.ShopType.Flynt, 1f);
        }

        public override void Pickup(PlayerController player)
        {
            if (this.m_pickedUp)
            {
                return;
            }
            for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
            {
                GameManager.Instance.AllPlayers[i].carriedConsumables.InfiniteKeys = true;
            }
            base.Pickup(player);
        }

        protected override void Update()
        {
            base.Update();
            if (this.m_pickedUp && this.m_owner != null)
            {
                if (!GameManager.Instance.Dungeon.CurseReaperActive)
                {
                    GameManager.Instance.Dungeon.SpawnCurseReaper();
                }
            }
        }

        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            debrisObject.GetComponent<LockOfTheJammed>().m_pickedUpThisRun = true;
            for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
            {
                GameManager.Instance.AllPlayers[i].carriedConsumables.InfiniteKeys = false;
            }
            return debrisObject;
        }
    }
}

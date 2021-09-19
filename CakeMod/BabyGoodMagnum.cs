using ItemAPI;
using UnityEngine;

namespace CakeMod
{
    class BabyGoodMagnum : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Baby Good Magnum";

            string resourceName = "CakeMod/Resources/BabyGoodMagnum";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<BabyGoodMagnum>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
            string shortDesc = "Cuddleable & Fireable";
            string longDesc = "A ransacked gun from a strange collector of all things 'baby good'.\n\nThis parody of a standard magnum will assist you on your travels.";
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cak");
            item.quality = ItemQuality.D;
            item.sprite.IsPerpendicular = true;
            CakeIDs.BabyGoodMagnum = item.PickupObjectId;

        }
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);

        }
        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            return debrisObject;
        }
    }
}

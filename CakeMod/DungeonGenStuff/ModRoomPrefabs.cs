using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dungeonator;
using GungeonAPI;
using ItemAPI;
using UnityEngine;

namespace CakeMod
{
    class ModRoomPrefabs
    {
        public static PrototypeDungeonRoom Mod_Entrance_Room;
        public static PrototypeDungeonRoom Mod_Exit_Room;
        public static PrototypeDungeonRoom[] Mod_Rooms;
        public static PrototypeDungeonRoom Mod_Boss;
        public static PrototypeDungeonRoom PriestRoom;
        public static List<string> Mod_RoomList; // this will contain all of our mods rooms.
        public static void InitCustomRooms()
        {
            try {
                Mod_RoomList = new List<string>()
            {
                "FloorHat_HoleRoom.room",
                "FloorHat_EmptyCircle.room"
            };
                PriestRoom = RoomFactory2.BuildFromResource2("CakeMod/Resources/ModRooms/BishopRoom.room");

                Mod_Entrance_Room = RoomFactory2.BuildFromResource2("CakeMod/Resources/ModRooms/FloorHat_Entrance.room"); // these to need to be set up just like how you would get an item sprite
                Mod_Exit_Room = RoomFactory2.BuildFromResource2("CakeMod/Resources/ModRooms/FloorHat_Exit.room");
                Mod_Entrance_Room.category = PrototypeDungeonRoom.RoomCategory.ENTRANCE;

                List<PrototypeDungeonRoom> m_floorNameRooms = new List<PrototypeDungeonRoom>();

              
                

                List<PrototypeDungeonRoom> m_floorNameRooms2 = new List<PrototypeDungeonRoom>();

                foreach (string name in Mod_RoomList)
                {
                    PrototypeDungeonRoom m_room = RoomFactory2.BuildFromResource2("CakeMod/Resources/ModRooms/" + name);
                    m_floorNameRooms2.Add(m_room);
                }

                Mod_Rooms = m_floorNameRooms.ToArray();

                foreach (PrototypeDungeonRoom room in m_floorNameRooms2)
                {
                    ModPrefabs.FloorNameRoomTable.includedRooms.elements.Add(GenerateWeightedRoom(room, 1));
                }

                Mod_Boss = RoomFactory2.BuildFromResource2("CakeMod/Resources/ModRooms/FloorHat_BossRoom.room");
                Mod_Boss.category = PrototypeDungeonRoom.RoomCategory.BOSS;
                Mod_Boss.subCategoryBoss = PrototypeDungeonRoom.RoomBossSubCategory.FLOOR_BOSS;
                Mod_Boss.subCategoryNormal = PrototypeDungeonRoom.RoomNormalSubCategory.COMBAT;
                Mod_Boss.subCategorySpecial = PrototypeDungeonRoom.RoomSpecialSubCategory.STANDARD_SHOP;
                Mod_Boss.subCategorySecret = PrototypeDungeonRoom.RoomSecretSubCategory.UNSPECIFIED_SECRET;
                Mod_Boss.roomEvents = new List<RoomEventDefinition>() {
                new RoomEventDefinition(RoomEventTriggerCondition.ON_ENTER_WITH_ENEMIES, RoomEventTriggerAction.SEAL_ROOM),
                new RoomEventDefinition(RoomEventTriggerCondition.ON_ENEMIES_CLEARED, RoomEventTriggerAction.UNSEAL_ROOM),
            };
                Mod_Boss.associatedMinimapIcon = ModPrefabs.doublebeholsterroom01.associatedMinimapIcon;
                Mod_Boss.usesProceduralLighting = false;
                Mod_Boss.usesProceduralDecoration = false;
                Mod_Boss.rewardChestSpawnPosition = new IntVector2(25, 20); //Where the reward pedestal spawns, should be changed based on room size
                Mod_Boss.overriddenTilesets = GlobalDungeonData.ValidTilesets.CATHEDRALGEON;

                foreach (PrototypeRoomExit exit in Mod_Boss.exitData.exits) { exit.exitType = PrototypeRoomExit.ExitType.ENTRANCE_ONLY; }
                RoomBuilder.AddExitToRoom(Mod_Boss, new Vector2(26, 37), DungeonData.Direction.NORTH, PrototypeRoomExit.ExitType.EXIT_ONLY, PrototypeRoomExit.ExitGroup.B);
            }
            catch (Exception e)
            {
                Tools.Print(e);
            }
        }

        public static WeightedRoom GenerateWeightedRoom(PrototypeDungeonRoom Room, float Weight = 1, bool LimitedCopies = true, int MaxCopies = 1, DungeonPrerequisite[] AdditionalPrerequisites = null)
        {
            if (Room == null) { return null; }
            if (AdditionalPrerequisites == null) { AdditionalPrerequisites = new DungeonPrerequisite[0]; }
            return new WeightedRoom() { room = Room, weight = Weight, limitedCopies = LimitedCopies, maxCopies = MaxCopies, additionalPrerequisites = AdditionalPrerequisites };
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Dungeonator;
using UnityEngine;
using ItemAPI;
using CakeMod;
using Ionic.Zip;

namespace GungeonAPI
{
    // Token: 0x02000008 RID: 8
    public static class RoomFactory2
    {
        public static readonly string dataHeader = "***DATA***";

        /*public static AssetBundle sharedAssets;
        public static AssetBundle sharedAssets2;
        public static AssetBundle braveResources;*/
        public static bool FailSafeCheck;
        private static RoomEventDefinition sealOnEnterWithEnemies = new RoomEventDefinition(RoomEventTriggerCondition.ON_ENTER_WITH_ENEMIES, RoomEventTriggerAction.SEAL_ROOM);
        private static RoomEventDefinition unsealOnRoomClear = new RoomEventDefinition(RoomEventTriggerCondition.ON_ENEMIES_CLEARED, RoomEventTriggerAction.UNSEAL_ROOM);

        // private static string [] m_AssetBundleNames = new string[] { "shared_auto_001", "shared_auto_002", "brave_resources_001" };
        public static PrototypeDungeonRoom BuildFromResource2(string roomPath, bool setRoomCategory = false, bool autoAssignToFloor = false, bool defineFullPath = false, bool assignDecorationSettings = false)
        {
            string RoomPath = (roomPath);
            if (defineFullPath) { RoomPath = roomPath; }
            Texture2D texture = ResourceExtractor.GetTextureFromResource(RoomPath);
            RoomData roomData = ExtractRoomDataFromResource(RoomPath);
            return Build(texture, roomData, setRoomCategory, autoAssignToFloor, assignDecorationSettings, roomData.weight);
        }
        public static RoomData BuildFromResource(string roomPath)
        {
            var texture = ResourceExtractor.GetTextureFromResource(roomPath);
            texture.name = Path.GetFileName(roomPath);
            RoomData roomData = ExtractRoomDataFromResource(roomPath);
            roomData.room = Build(texture, roomData);
            return roomData;
        }
        public static RoomData ExtractRoomDataFromResource(string path)
        {
            byte[] data = ResourceExtractor.ExtractEmbeddedResource(path);
            return ExtractRoomDataFromBytes(data);
        }
        public static void ApplyRoomData(PrototypeDungeonRoom room, RoomData roomData)
        {
            if (roomData.exitPositions != null)
            {
                for (int i = 0; i < roomData.exitPositions.Length; i++)
                {
                    DungeonData.Direction dir = (DungeonData.Direction)Enum.Parse(typeof(DungeonData.Direction), roomData.exitDirections[i].ToUpper());
                    AddExit(room, roomData.exitPositions[i], dir);
                }
            }
            else
            {
                AddExit(room, new Vector2(room.Width / 2, room.Height), DungeonData.Direction.NORTH);
                AddExit(room, new Vector2(room.Width / 2, 0), DungeonData.Direction.SOUTH);
                AddExit(room, new Vector2(room.Width, room.Height / 2), DungeonData.Direction.EAST);
                AddExit(room, new Vector2(0, room.Height / 2), DungeonData.Direction.WEST);
            }

            if (roomData.enemyPositions != null)
            {
                for (int i = 0; i < roomData.enemyPositions.Length; i++)
                {
                    AddEnemyToRoom(room, roomData.enemyPositions[i], roomData.enemyGUIDs[i], roomData.enemyReinforcementLayers[i], roomData.randomizeEnemyPositions);
                }
            }

            if (roomData.placeablePositions != null)
            {
                for (int i = 0; i < roomData.placeablePositions.Length; i++)
                {
                    AddPlaceableToRoom(room, roomData.placeablePositions[i], roomData.placeableGUIDs[i]);
                }
            }

            if (roomData.floors != null)
            {
                foreach (var floor in roomData.floors)
                {
                    room.prerequisites.Add(new DungeonPrerequisite()
                    {
                        prerequisiteType = DungeonPrerequisite.PrerequisiteType.TILESET,
                        requiredTileset = ShrineTools.GetEnumValue<GlobalDungeonData.ValidTilesets>(floor)
                    });
                }
            }
            //Set categories
            if (!string.IsNullOrEmpty(roomData.category))
                room.category = ShrineTools.GetEnumValue<PrototypeDungeonRoom.RoomCategory>(roomData.category);
            if (!string.IsNullOrEmpty(roomData.normalSubCategory))
                room.subCategoryNormal = ShrineTools.GetEnumValue<PrototypeDungeonRoom.RoomNormalSubCategory>(roomData.normalSubCategory);
            //========================
            if (!string.IsNullOrEmpty(roomData.bossSubCategory))
                room.subCategoryBoss = ShrineTools.GetEnumValue<PrototypeDungeonRoom.RoomBossSubCategory>(roomData.bossSubCategory);
            //========================
            if (!string.IsNullOrEmpty(roomData.specialSubCategory))
                room.subCategorySpecial = ShrineTools.GetEnumValue<PrototypeDungeonRoom.RoomSpecialSubCategory>(roomData.specialSubCategory);
            room.usesProceduralDecoration = true;
            room.allowFloorDecoration = roomData.doFloorDecoration;
            room.allowWallDecoration = roomData.doWallDecoration;
            room.usesProceduralLighting = roomData.doLighting;
        }


        public static RoomData ExtractRoomDataFromBytes(byte[] data)
        {
            string stringData = ResourceExtractor.BytesToString(data);
            return ExtractRoomData(stringData);
        }
        public static PrototypeDungeonRoom Build(Texture2D texture, RoomData roomData)
        {
            try
            {
                var room = CreateRoomFromTexture(texture);
                ApplyRoomData(room, roomData);
                room.UpdatePrecalculatedData();
                return room;
            }
            catch (Exception e)
            {
                // Tools.PrintError("Failed to build room!");
                Tools.PrintException(e);
            }

            return CreateEmptyRoom(12, 12);
        }
        public static IEnumerable<RoomData> BuildFromZipFile(string zipFilePath)
        {
            if (!ZipFile.IsZipFile(zipFilePath))
            {
                Tools.Log($"Not a valid zip file!");
                yield break;
            }
            using (var zipFile = ZipFile.Read(zipFilePath))
            {
                //Tools.PrintNoID("did it work?");
                foreach (var entry in zipFile.Entries)
                {
                    var fileName = Path.GetFileNameWithoutExtension(entry.FileName);
                    var extension = Path.GetExtension(entry.FileName);
                    if (!string.Equals(extension, ".room", StringComparison.OrdinalIgnoreCase))
                        continue;

                    byte[] zipData;
                    int capacity = (int)entry.UncompressedSize;
                    if (capacity < 0)
                        continue;

                    using (var ms = new MemoryStream(capacity))
                    {
                        entry.Extract(ms);
                        zipData = ms.ToArray();
                    }

                    var texture = ResourceExtractor.BytesToTexture(zipData, fileName);
                    var roomData = ExtractRoomDataFromBytes(zipData);
                    roomData.room = Build(texture, roomData);

                    yield return roomData;
                }
            }
        }

        public static RoomData BuildFromRoomFile(string roomPath)
        {
            var texture = ResourceExtractor.GetTextureFromFile(roomPath, ".room");
            texture.name = Path.GetFileName(roomPath);
            RoomData roomData = ExtractRoomDataFromFile(roomPath);
            roomData.room = Build(texture, roomData);
            return roomData;
        }


        public struct RoomData
        {
            public string category;
            public string normalSubCategory;
            public string specialSubCategory;
            public string bossSubCategory;
            public Vector2[] enemyPositions;
            public string[] enemyGUIDs;
            public Vector2[] placeablePositions;
            public string[] placeableGUIDs;
            public int[] enemyReinforcementLayers;
            public Vector2[] exitPositions;
            public string[] exitDirections;
            public string[] floors;
            public float weight;
            public bool isSpecialRoom;
            public bool randomizeEnemyPositions, doFloorDecoration, doWallDecoration, doLighting;
            [NonSerialized]
            public PrototypeDungeonRoom room;
        }


        public static PrototypeDungeonRoom BuildFromResource(string roomPath, bool setRoomCategory = false, bool autoAssignToFloor = false, bool defineFullPath = false, bool assignDecorationSettings = false)
        {
            string RoomPath = (roomPath);
            if (defineFullPath) { RoomPath = roomPath; }
            Texture2D texture = ResourceExtractor.GetTextureFromResource(RoomPath);
            RoomData roomData = ExtractRoomDataFromResource(RoomPath);
            return Build(texture, roomData, setRoomCategory, autoAssignToFloor, assignDecorationSettings, roomData.weight);
        }

        public static PrototypeDungeonRoom Build(Texture2D texture, RoomData roomData, bool SetRoomCategory, bool AutoAssignToFloor, bool AssignDecorationProperties, float? Weight)
        {
            try
            {
                PrototypeDungeonRoom room = CreateRoomFromTexture(texture);
                ApplyRoomData(room, roomData, SetRoomCategory, AutoAssignToFloor, AssignDecorationProperties, Weight);
                room.OnBeforeSerialize();
                room.OnAfterDeserialize();
                room.UpdatePrecalculatedData();
                return room;
            }
            catch (Exception e)
            {
                Tools.PrintError("Failed to build room!");
                Tools.PrintException(e);
            }
            return CreateEmptyRoom(12, 12);
        }

        public static void ApplyRoomData(PrototypeDungeonRoom room, RoomData roomData, bool setRoomCategory, bool autoAssignToFloor, bool assignDecorationProperties, float? Weight)
        {
            // Tools.Print("Building Exits...");
            if (roomData.exitPositions != null)
            {
                for (int i = 0; i < roomData.exitPositions.Length; i++)
                {
                    DungeonData.Direction dir = (DungeonData.Direction)Enum.Parse(typeof(DungeonData.Direction), roomData.exitDirections[i].ToUpper());
                    AddExit(room, roomData.exitPositions[i], dir);
                }
            }
            else
            {
                AddExit(room, new Vector2(room.Width / 2, room.Height), DungeonData.Direction.NORTH);
                AddExit(room, new Vector2(room.Width / 2, 0), DungeonData.Direction.SOUTH);
                AddExit(room, new Vector2(room.Width, room.Height / 2), DungeonData.Direction.EAST);
                AddExit(room, new Vector2(0, room.Height / 2), DungeonData.Direction.WEST);
            }

            // Tools.Print("Adding Enemies...");
            if (roomData.enemyPositions != null)
            {
                for (int i = 0; i < roomData.enemyPositions.Length; i++)
                {
                    AddEnemyToRoom(room, roomData.enemyPositions[i], roomData.enemyGUIDs[i], roomData.enemyReinforcementLayers[i], roomData.randomizeEnemyPositions);
                }
            }

            // Tools.Print("Adding Objects...");
            if (roomData.placeablePositions != null)
            {
                for (int i = 0; i < roomData.placeablePositions.Length; i++)
                {
                    AddPlaceableToRoom(room, roomData.placeablePositions[i], roomData.placeableGUIDs[i]);
                }
            }

            if (setRoomCategory | autoAssignToFloor)
            {
                // Set categories
                if (!string.IsNullOrEmpty(roomData.category)) { room.category = GetRoomCategory(roomData.category); }
                if (!string.IsNullOrEmpty(roomData.normalSubCategory)) { room.subCategoryNormal = GetRoomNormalSubCategory(roomData.normalSubCategory); }
                if (!string.IsNullOrEmpty(roomData.bossSubCategory)) { room.subCategoryBoss = GetRoomBossSubCategory(roomData.bossSubCategory); }
                if (!string.IsNullOrEmpty(roomData.specialSubCategory)) { room.subCategorySpecial = GetRoomSpecialSubCategory(roomData.specialSubCategory); }
            }
            if (autoAssignToFloor && roomData.floors != null)
            {
                if (!Weight.HasValue)
                {
                    if (room.category == PrototypeDungeonRoom.RoomCategory.SECRET)
                    {
                        Weight = 15; // Normal secret rooms have weight of 15.
                    }
                    else
                    {
                        Weight = 1;
                    }
                }
                if (room.category == PrototypeDungeonRoom.RoomCategory.SECRET)
                {
                    // Secret rooms are generally shared across all floors via a specific room table.
                    // Room Editor doesn't currently set this for secret rooms
                    room.OverrideMusicState = DungeonFloorMusicController.DungeonMusicState.CALM;
                    CakeMod.ModPrefabs.SecretRoomTable.includedRooms.elements.Add(CakeMod.ModRoomPrefabs.GenerateWeightedRoom(room, Weight.Value));
                }
                else
                {
                    foreach (string floor in roomData.floors) { AssignRoomToFloorRoomTable(room, GetTileSet(floor), Weight); }
                    /*ExpandPrefabs.CustomRoomTableSecretGlitchFloor.includedRooms.elements.Add(ExpandRoomPrefabs.GenerateWeightedRoom(room, Weight.Value));
                    ExpandPrefabs.CustomRoomTable.includedRooms.elements.Add(ExpandRoomPrefabs.GenerateWeightedRoom(room, Weight.Value));
                    ExpandPrefabs.CustomRoomTable2.includedRooms.elements.Add(ExpandRoomPrefabs.GenerateWeightedRoom(room, Weight.Value));*/
                }
            }

            if (assignDecorationProperties)
            {
                room.allowFloorDecoration = roomData.doFloorDecoration;
                room.allowWallDecoration = roomData.doWallDecoration;
                room.usesProceduralLighting = roomData.doLighting;
            }
        }

        public static void AssignRoomToFloorRoomTable(PrototypeDungeonRoom Room, GlobalDungeonData.ValidTilesets targetTileSet, float? Weight)
        {
            if (targetTileSet == GlobalDungeonData.ValidTilesets.CASTLEGEON)
            {
                CakeMod.ModPrefabs.CastleRoomTable.includedRooms.elements.Add(CakeMod.ModRoomPrefabs.GenerateWeightedRoom(Room, Weight.Value));
            }
            else if (targetTileSet == GlobalDungeonData.ValidTilesets.SEWERGEON)
            {
                CakeMod.ModPrefabs.SewersRoomTable.includedRooms.elements.Add(CakeMod.ModRoomPrefabs.GenerateWeightedRoom(Room, Weight.Value));
            }
            else if (targetTileSet == GlobalDungeonData.ValidTilesets.GUNGEON)
            {
                CakeMod.ModPrefabs.Gungeon_RoomTable.includedRooms.elements.Add(CakeMod.ModRoomPrefabs.GenerateWeightedRoom(Room, Weight.Value));
            }
            else if (targetTileSet == GlobalDungeonData.ValidTilesets.CATHEDRALGEON)
            {
                CakeMod.ModPrefabs.AbbeyRoomTable.includedRooms.elements.Add(CakeMod.ModRoomPrefabs.GenerateWeightedRoom(Room, Weight.Value));
            }
            else if (targetTileSet == GlobalDungeonData.ValidTilesets.MINEGEON)
            {
                CakeMod.ModPrefabs.MinesRoomTable.includedRooms.elements.Add(CakeMod.ModRoomPrefabs.GenerateWeightedRoom(Room, Weight.Value));
            }
            else if (targetTileSet == GlobalDungeonData.ValidTilesets.CATACOMBGEON)
            {
                CakeMod.ModPrefabs.CatacombsRoomTable.includedRooms.elements.Add(CakeMod.ModRoomPrefabs.GenerateWeightedRoom(Room, Weight.Value));
            }
            else if (targetTileSet == GlobalDungeonData.ValidTilesets.OFFICEGEON)
            {
                // R&G floor doesn't use room tables yet. If a room has this set, I'll assign to all floors instead.
                return;
            }
            else if (targetTileSet == GlobalDungeonData.ValidTilesets.FORGEGEON)
            {
                CakeMod.ModPrefabs.ForgeRoomTable.includedRooms.elements.Add(CakeMod.ModRoomPrefabs.GenerateWeightedRoom(Room, Weight.Value));
            }
            else if (targetTileSet == GlobalDungeonData.ValidTilesets.HELLGEON)
            {
                CakeMod.ModPrefabs.BulletHellRoomTable.includedRooms.elements.Add(CakeMod.ModRoomPrefabs.GenerateWeightedRoom(Room, Weight.Value));
            }
            return;
        }


        public static GlobalDungeonData.ValidTilesets GetTileSet(string val)
        {
            return (GlobalDungeonData.ValidTilesets)Enum.Parse(typeof(GlobalDungeonData.ValidTilesets), val.ToUpper());
        }
        public static PrototypeDungeonRoom.RoomCategory GetRoomCategory(string val)
        {
            return (PrototypeDungeonRoom.RoomCategory)Enum.Parse(typeof(PrototypeDungeonRoom.RoomCategory), val.ToUpper());
        }
        public static PrototypeDungeonRoom.RoomNormalSubCategory GetRoomNormalSubCategory(string val)
        {
            return (PrototypeDungeonRoom.RoomNormalSubCategory)Enum.Parse(typeof(PrototypeDungeonRoom.RoomNormalSubCategory), val.ToUpper());
        }
        public static PrototypeDungeonRoom.RoomBossSubCategory GetRoomBossSubCategory(string val)
        {
            return (PrototypeDungeonRoom.RoomBossSubCategory)Enum.Parse(typeof(PrototypeDungeonRoom.RoomBossSubCategory), val.ToUpper());
        }
        public static PrototypeDungeonRoom.RoomSpecialSubCategory GetRoomSpecialSubCategory(string val)
        {
            return (PrototypeDungeonRoom.RoomSpecialSubCategory)Enum.Parse(typeof(PrototypeDungeonRoom.RoomSpecialSubCategory), val.ToUpper());
        }

        public static RoomData ExtractRoomDataFromFile(string path)
        {
            string data = ResourceExtractor.BytesToString(File.ReadAllBytes(path));
            return ExtractRoomData(data);
        }



        public static RoomData ExtractRoomData(string data)
        {
            int num = (data.Length - dataHeader.Length - 1);
            for (int i = num; i > 0; i--)
            {
                string text = data.Substring(i, dataHeader.Length);
                if (text.Equals(dataHeader)) { return JsonUtility.FromJson<RoomData>(data.Substring(i + dataHeader.Length)); }
            }
            Tools.Log("No room data found at " + data);
            return default(RoomData);
        }

        public static PrototypeDungeonRoom CreateRoomFromTexture(Texture2D texture)
        {
            int width = texture.width;
            int height = texture.height;
            PrototypeDungeonRoom room = GetNewPrototypeDungeonRoom(width, height);
            room.FullCellData = new PrototypeDungeonRoomCellData[width * height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    room.FullCellData[x + y * width] = CellDataFromColor(texture.GetPixel(x, y));
                }
            }
            room.name = texture.name;
            return room;
        }

        public static PrototypeDungeonRoomCellData CellDataFromColor(Color32 color)
        {
            // if (color.Equals(Color.magenta)) return null;

            PrototypeDungeonRoomCellData data = new PrototypeDungeonRoomCellData();
            data.state = TypeFromColor(color);
            data.diagonalWallType = DiagonalWallTypeFromColor(color);
            data.appearance = new PrototypeDungeonRoomCellAppearance() { OverrideFloorType = CellVisualData.CellFloorType.Stone };

            if (color == Color.red)
            {
                data.appearance.OverrideFloorType = CellVisualData.CellFloorType.Carpet;
                data.appearance.IsPhantomCarpet = true;
            }

            return data;
        }

        public static CellType TypeFromColor(Color color)
        {
            if (color == Color.black)
            {
                return CellType.PIT;
            }
            else if (color == Color.white | color == Color.red)
            {
                return CellType.FLOOR;
            }
            return CellType.WALL;
        }

        public static DiagonalWallType DiagonalWallTypeFromColor(Color color)
        {
            if (color == Color.red)
                return DiagonalWallType.NORTHEAST;
            else if (color == Color.green)
                return DiagonalWallType.SOUTHEAST;
            else if (color == Color.blue)
                return DiagonalWallType.SOUTHWEST;
            else if (color == Color.yellow)
                return DiagonalWallType.NORTHWEST;
            else
                return DiagonalWallType.NONE;
        }

        public static PrototypeDungeonRoom CreateEmptyRoom(int width = 12, int height = 12)
        {
            try
            {
                Tools.Print("  Create Empty Room...", "5599FF");
                PrototypeDungeonRoom room = GetNewPrototypeDungeonRoom(width, height);
                AddExit(room, new Vector2(width / 2, height), DungeonData.Direction.NORTH);
                AddExit(room, new Vector2(width / 2, 0), DungeonData.Direction.SOUTH);
                AddExit(room, new Vector2(width, height / 2), DungeonData.Direction.EAST);
                AddExit(room, new Vector2(0, height / 2), DungeonData.Direction.WEST);

                room.FullCellData = new PrototypeDungeonRoomCellData[width * height];
                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        room.FullCellData[x + y * width] = new PrototypeDungeonRoomCellData()
                        {
                            state = CellType.FLOOR,
                            appearance = new PrototypeDungeonRoomCellAppearance()
                            {
                                OverrideFloorType = CellVisualData.CellFloorType.Stone,
                            },
                        };
                    }
                }

                room.OnBeforeSerialize();
                room.OnAfterDeserialize();
                room.UpdatePrecalculatedData();
                return room;
            }
            catch (Exception e)
            {
                Tools.PrintException(e);
                return null;
            }
        }

        public static GameObject GetGameObjectFromBundles(string assetPath)
        {
            if (CakeMod.ModPrefabs.shared_auto_001.LoadAsset<GameObject>(assetPath))
            {
                return CakeMod.ModPrefabs.shared_auto_001.LoadAsset<GameObject>(assetPath);
            }
            else if (CakeMod.ModPrefabs.shared_auto_002.LoadAsset<GameObject>(assetPath))
            {
                return CakeMod.ModPrefabs.shared_auto_002.LoadAsset<GameObject>(assetPath);
            }
            else if (CakeMod.ModPrefabs.braveResources.LoadAsset<GameObject>(assetPath))
            {
                return CakeMod.ModPrefabs.braveResources.LoadAsset<GameObject>(assetPath);
            }
            else
            {
                return null;
            }
        }

        public static DungeonPlaceable GetPlaceableFromBundles(string assetPath)
        {
            if (CakeMod.ModPrefabs.shared_auto_001.LoadAsset<DungeonPlaceable>(assetPath))
            {
                return CakeMod.ModPrefabs.shared_auto_001.LoadAsset<DungeonPlaceable>(assetPath);
            }
            else if (CakeMod.ModPrefabs.shared_auto_002.LoadAsset<DungeonPlaceable>(assetPath))
            {
                return CakeMod.ModPrefabs.shared_auto_002.LoadAsset<DungeonPlaceable>(assetPath);
            }
            else if (CakeMod.ModPrefabs.braveResources.LoadAsset<DungeonPlaceable>(assetPath))
            {
                return CakeMod.ModPrefabs.braveResources.LoadAsset<DungeonPlaceable>(assetPath);
            }
            else
            {
                return null;
            }
        }

        public static void AddEnemyToRoom(PrototypeDungeonRoom room, Vector2 location, string guid, int layer, bool shuffle)
        {
            DungeonPlaceable placeableContents = ScriptableObject.CreateInstance<DungeonPlaceable>();
            placeableContents.width = 1;
            placeableContents.height = 1;
            placeableContents.respectsEncounterableDifferentiator = true;
            placeableContents.variantTiers = new List<DungeonPlaceableVariant>() {
                new DungeonPlaceableVariant() {
                    percentChance = 1,
                    prerequisites = new DungeonPrerequisite[0],
                    enemyPlaceableGuid = guid,
                    materialRequirements= new DungeonPlaceableRoomMaterialRequirement[0],
                }
            };

            PrototypePlacedObjectData objectData = new PrototypePlacedObjectData()
            {
                contentsBasePosition = location,
                fieldData = new List<PrototypePlacedObjectFieldData>(),
                instancePrerequisites = new DungeonPrerequisite[0],
                linkedTriggerAreaIDs = new List<int>(),
                placeableContents = placeableContents,
            };

            if (layer > 0)
            {
                AddObjectDataToReinforcementLayer(room, objectData, layer - 1, location, shuffle);
            }
            else
            {
                room.placedObjects.Add(objectData);
                room.placedObjectPositions.Add(location);
            }

            if (!room.roomEvents.Contains(sealOnEnterWithEnemies)) { room.roomEvents.Add(sealOnEnterWithEnemies); }
            if (!room.roomEvents.Contains(unsealOnRoomClear)) { room.roomEvents.Add(unsealOnRoomClear); }
        }

        public static void AddObjectDataToReinforcementLayer(PrototypeDungeonRoom room, PrototypePlacedObjectData objectData, int layer, Vector2 location, bool shuffle)
        {
            if (room.additionalObjectLayers.Count <= layer)
            {
                for (int i = room.additionalObjectLayers.Count; i <= layer; i++)
                {
                    PrototypeRoomObjectLayer newLayer = new PrototypeRoomObjectLayer
                    {
                        layerIsReinforcementLayer = true,
                        placedObjects = new List<PrototypePlacedObjectData>(),
                        placedObjectBasePositions = new List<Vector2>(),
                        shuffle = shuffle
                    };
                    room.additionalObjectLayers.Add(newLayer);
                }
            }
            room.additionalObjectLayers[layer].placedObjects.Add(objectData);
            room.additionalObjectLayers[layer].placedObjectBasePositions.Add(location);
        }

        public static void AddPlaceableToRoom(PrototypeDungeonRoom room, Vector2 location, string assetPath)
        {
            try
            {
                if (GetGameObjectFromBundles(assetPath) != null)
                {
                    DungeonPrerequisite[] array = new DungeonPrerequisite[0];
                    room.placedObjectPositions.Add(location);
                    DungeonPlaceable dungeonPlaceable = ScriptableObject.CreateInstance<DungeonPlaceable>();
                    dungeonPlaceable.width = 2;
                    dungeonPlaceable.height = 2;
                    dungeonPlaceable.respectsEncounterableDifferentiator = true;
                    dungeonPlaceable.variantTiers = new List<DungeonPlaceableVariant> {
                        new DungeonPlaceableVariant {
                            percentChance = 1f,
                            nonDatabasePlaceable = GetGameObjectFromBundles(assetPath),
                            prerequisites = array,
                            materialRequirements = new DungeonPlaceableRoomMaterialRequirement[0]
                        }
                    };
                    room.placedObjects.Add(new PrototypePlacedObjectData
                    {
                        contentsBasePosition = location,
                        fieldData = new List<PrototypePlacedObjectFieldData>(),
                        instancePrerequisites = array,
                        linkedTriggerAreaIDs = new List<int>(),
                        placeableContents = dungeonPlaceable
                    });
                    return;
                }
                else if (GetPlaceableFromBundles(assetPath) != null)
                {
                    DungeonPrerequisite[] instancePrerequisites = new DungeonPrerequisite[0];
                    room.placedObjectPositions.Add(location);
                    room.placedObjects.Add(new PrototypePlacedObjectData
                    {
                        contentsBasePosition = location,
                        fieldData = new List<PrototypePlacedObjectFieldData>(),
                        instancePrerequisites = instancePrerequisites,
                        linkedTriggerAreaIDs = new List<int>(),
                        placeableContents = GetPlaceableFromBundles(assetPath)
                    });
                    return;
                }
                else
                {
                    Tools.PrintError("Unable to find asset in asset bundles: " + assetPath, "FF0000");
                }
            }
            catch (Exception e)
            {
                Tools.PrintException(e);
            }
        }

        public static void AddExit(PrototypeDungeonRoom room, Vector2 location, DungeonData.Direction direction)
        {
            if (room.exitData == null) { room.exitData = new PrototypeRoomExitData(); }
            if (room.exitData.exits == null) { room.exitData.exits = new List<PrototypeRoomExit>(); }

            PrototypeRoomExit exit = new PrototypeRoomExit(direction, location);
            exit.exitType = PrototypeRoomExit.ExitType.NO_RESTRICTION;
            Vector2 margin = (direction == DungeonData.Direction.EAST || direction == DungeonData.Direction.WEST) ? new Vector2(0, 1) : new Vector2(1, 0);
            exit.containedCells.Add(location + margin);
            room.exitData.exits.Add(exit);
        }

        public static PrototypeDungeonRoom GetNewPrototypeDungeonRoom(int width = 12, int height = 12)
        {
            PrototypeDungeonRoom room = ScriptableObject.CreateInstance<PrototypeDungeonRoom>();
            room.injectionFlags = new RuntimeInjectionFlags();
            room.RoomId = UnityEngine.Random.Range(10000, 1000000);
            room.pits = new List<PrototypeRoomPitEntry>();
            room.placedObjects = new List<PrototypePlacedObjectData>();
            room.placedObjectPositions = new List<Vector2>();
            room.additionalObjectLayers = new List<PrototypeRoomObjectLayer>();
            room.eventTriggerAreas = new List<PrototypeEventTriggerArea>();
            room.roomEvents = new List<RoomEventDefinition>();
            room.overriddenTilesets = 0;
            room.paths = new List<SerializedPath>();
            room.prerequisites = new List<DungeonPrerequisite>();
            room.excludedOtherRooms = new List<PrototypeDungeonRoom>();
            room.rectangularFeatures = new List<PrototypeRectangularFeature>();
            room.exitData = new PrototypeRoomExitData();
            room.exitData.exits = new List<PrototypeRoomExit>();
            room.Width = width;
            room.Height = height;

            // Not originally defined as defaults in Kyle's code.
            room.GUID = Guid.NewGuid().ToString();
            room.category = PrototypeDungeonRoom.RoomCategory.NORMAL;
            room.subCategoryBoss = PrototypeDungeonRoom.RoomBossSubCategory.FLOOR_BOSS;
            room.subCategoryNormal = PrototypeDungeonRoom.RoomNormalSubCategory.COMBAT;
            room.subCategorySpecial = PrototypeDungeonRoom.RoomSpecialSubCategory.STANDARD_SHOP;
            room.subCategorySecret = PrototypeDungeonRoom.RoomSecretSubCategory.UNSPECIFIED_SECRET;
            room.usesProceduralDecoration = true;
            room.usesProceduralLighting = true;
            room.allowWallDecoration = true;
            room.allowFloorDecoration = true;
            room.PreventMirroring = false;
            room.InvalidInCoop = false;
            room.preventAddedDecoLayering = false;
            room.precludeAllTilemapDrawing = false;
            room.drawPrecludedCeilingTiles = false;
            room.preventBorders = false;
            room.preventFacewallAO = false;
            room.usesCustomAmbientLight = false;
            room.customAmbientLight = Color.white;
            room.ForceAllowDuplicates = false;
            room.injectionFlags = new RuntimeInjectionFlags() { CastleFireplace = false, ShopAnnexed = false };
            room.IsLostWoodsRoom = false;
            room.UseCustomMusic = false;
            room.UseCustomMusicState = false;
            room.CustomMusicEvent = string.Empty;
            room.UseCustomMusicSwitch = false;
            room.CustomMusicSwitch = string.Empty;
            room.overrideRoomVisualTypeForSecretRooms = false;
            room.rewardChestSpawnPosition = new IntVector2(-1, -1);
            room.overrideRoomVisualType = -1;

            return room;
        }

        public static Dictionary<string, RoomFactory2.RoomData> rooms2 = new Dictionary<string, RoomFactory2.RoomData>();
    }
}


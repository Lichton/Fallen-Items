using Dungeonator;
using GungeonAPI;
using ItemAPI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace CakeMod
{
    public static class Toolbox
    {
        public static class ReflectionHelpers
        {

            public static IList CreateDynamicList(Type type)
            {
                bool flag = type == null;
                if (flag) { throw new ArgumentNullException("type", "Argument cannot be null."); }
                ConstructorInfo[] constructors = typeof(List<>).MakeGenericType(new Type[] { type }).GetConstructors();
                foreach (ConstructorInfo constructorInfo in constructors)
                {
                    ParameterInfo[] parameters = constructorInfo.GetParameters();
                    bool flag2 = parameters.Length != 0;
                    if (!flag2) { return (IList)constructorInfo.Invoke(null, null); }
                }
                throw new ApplicationException("Could not create a new list with type <" + type.ToString() + ">.");
            }

            public static IDictionary CreateDynamicDictionary(Type typeKey, Type typeValue)
            {
                bool flag = typeKey == null;
                if (flag)
                {
                    throw new ArgumentNullException("type_key", "Argument cannot be null.");
                }
                bool flag2 = typeValue == null;
                if (flag2) { throw new ArgumentNullException("type_value", "Argument cannot be null."); }
                ConstructorInfo[] constructors = typeof(Dictionary<,>).MakeGenericType(new Type[] { typeKey, typeValue }).GetConstructors();
                foreach (ConstructorInfo constructorInfo in constructors)
                {
                    ParameterInfo[] parameters = constructorInfo.GetParameters();
                    bool flag3 = parameters.Length != 0;
                    if (!flag3) { return (IDictionary)constructorInfo.Invoke(null, null); }
                }
                throw new ApplicationException(string.Concat(new string[] {
                "Could not create a new dictionary with types <",
                typeKey.ToString(),
                ",",
                typeValue.ToString(),
                ">."
            }));
            }

            public static T ReflectGetField<T>(Type classType, string fieldName, object o = null)
            {
                FieldInfo field = classType.GetField(fieldName, BindingFlags.Public | BindingFlags.NonPublic | ((o != null) ? BindingFlags.Instance : BindingFlags.Static));
                return (T)field.GetValue(o);
            }

            public static void ReflectSetField<T>(Type classType, string fieldName, T value, object o = null)
            {
                FieldInfo field = classType.GetField(fieldName, BindingFlags.Public | BindingFlags.NonPublic | ((o != null) ? BindingFlags.Instance : BindingFlags.Static));
                field.SetValue(o, value);
            }

            public static T ReflectGetProperty<T>(Type classType, string propName, object o = null, object[] indexes = null)
            {
                PropertyInfo property = classType.GetProperty(propName, BindingFlags.Public | BindingFlags.NonPublic | ((o != null) ? BindingFlags.Instance : BindingFlags.Static));
                return (T)property.GetValue(o, indexes);
            }

            public static void ReflectSetProperty<T>(Type classType, string propName, T value, object o = null, object[] indexes = null)
            {
                PropertyInfo property = classType.GetProperty(propName, BindingFlags.Public | BindingFlags.NonPublic | ((o != null) ? BindingFlags.Instance : BindingFlags.Static));
                property.SetValue(o, value, indexes);
            }

            public static MethodInfo ReflectGetMethod(Type classType, string methodName, Type[] methodArgumentTypes = null, Type[] genericMethodTypes = null, bool? isStatic = null)
            {
                MethodInfo[] array = ReflectTryGetMethods(classType, methodName, methodArgumentTypes, genericMethodTypes, isStatic);
                bool flag = array.Count() == 0;
                if (flag) { throw new MissingMethodException("Cannot reflect method, not found based on input parameters."); }
                bool flag2 = array.Count() > 1;
                if (flag2) { throw new InvalidOperationException("Cannot reflect method, more than one method matched based on input parameters."); }
                return array[0];
            }

            public static MethodInfo ReflectTryGetMethod(Type classType, string methodName, Type[] methodArgumentTypes = null, Type[] genericMethodTypes = null, bool? isStatic = null)
            {
                MethodInfo[] array = ReflectTryGetMethods(classType, methodName, methodArgumentTypes, genericMethodTypes, isStatic);
                bool flag = array.Count() == 0;
                MethodInfo result;
                if (flag)
                {
                    result = null;
                }
                else
                {
                    bool flag2 = array.Count() > 1;
                    if (flag2) { result = null; } else { result = array[0]; }
                }
                return result;
            }

            public static MethodInfo[] ReflectTryGetMethods(Type classType, string methodName, Type[] methodArgumentTypes = null, Type[] genericMethodTypes = null, bool? isStatic = null)
            {
                BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.NonPublic;
                bool flag = isStatic == null || isStatic.Value;
                if (flag) { bindingFlags |= BindingFlags.Static; }
                bool flag2 = isStatic == null || !isStatic.Value;
                if (flag2) { bindingFlags |= BindingFlags.Instance; }
                MethodInfo[] methods = classType.GetMethods(bindingFlags);
                List<MethodInfo> list = new List<MethodInfo>();
                for (int i = 0; i < methods.Length; i++)
                {
                    // foreach (MethodInfo methodInfo in methods) {
                    bool flag3 = methods[i].Name != methodName;
                    if (!flag3)
                    {
                        bool isGenericMethodDefinition = methods[i].IsGenericMethodDefinition;
                        if (isGenericMethodDefinition)
                        {
                            bool flag4 = genericMethodTypes == null || genericMethodTypes.Length == 0;
                            if (flag4) { goto IL_14D; }
                            Type[] genericArguments = methods[i].GetGenericArguments();
                            bool flag5 = genericArguments.Length != genericMethodTypes.Length;
                            if (flag5) { goto IL_14D; }
                            methods[i] = methods[i].MakeGenericMethod(genericMethodTypes);
                        }
                        else
                        {
                            bool flag6 = genericMethodTypes != null && genericMethodTypes.Length != 0;
                            if (flag6) { goto IL_14D; }
                        }
                        ParameterInfo[] parameters = methods[i].GetParameters();
                        bool flag7 = methodArgumentTypes != null;
                        if (!flag7) { goto IL_141; }
                        bool flag8 = parameters.Length != methodArgumentTypes.Length;
                        if (!flag8)
                        {
                            for (int j = 0; j < parameters.Length; j++)
                            {
                                ParameterInfo parameterInfo = parameters[j];
                                bool flag9 = parameterInfo.ParameterType != methodArgumentTypes[j];
                                if (flag9) { goto IL_14A; }
                            }
                            goto IL_141;
                        }
                    IL_14A:
                        goto IL_14D;
                    IL_141:
                        list.Add(methods[i]);
                    }
                IL_14D:;
                }
                return list.ToArray();
            }

            public static object InvokeRefs<T0>(MethodInfo methodInfo, object o, T0 p0)
            {
                object[] parameters = new object[] { p0 };
                return methodInfo.Invoke(o, parameters);
            }

            public static object InvokeRefs<T0>(MethodInfo methodInfo, object o, ref T0 p0)
            {
                object[] array = new object[] { p0 };
                object result = methodInfo.Invoke(o, array);
                p0 = (T0)array[0];
                return result;
            }

            public static object InvokeRefs<T0, T1>(MethodInfo methodInfo, object o, T0 p0, T1 p1)
            {
                object[] parameters = new object[] { p0, p1 };
                return methodInfo.Invoke(o, parameters);
            }

            public static object InvokeRefs<T0, T1>(MethodInfo methodInfo, object o, ref T0 p0, T1 p1)
            {
                object[] array = new object[] { p0, p1 };
                object result = methodInfo.Invoke(o, array);
                p0 = (T0)array[0];
                return result;
            }

            public static object InvokeRefs<T0, T1>(MethodInfo methodInfo, object o, T0 p0, ref T1 p1)
            {
                object[] array = new object[] { p0, p1 };
                object result = methodInfo.Invoke(o, array);
                p1 = (T1)array[1];
                return result;
            }

            public static object InvokeRefs<T0, T1>(MethodInfo methodInfo, object o, ref T0 p0, ref T1 p1)
            {
                object[] array = new object[] { p0, p1 };
                object result = methodInfo.Invoke(o, array);
                p0 = (T0)array[0];
                p1 = (T1)array[1];
                return result;
            }

            public static object InvokeRefs<T0, T1, T2>(MethodInfo methodInfo, object o, T0 p0, T1 p1, T2 p2)
            {
                object[] parameters = new object[] { p0, p1, p2 };
                return methodInfo.Invoke(o, parameters);
            }

            public static object InvokeRefs<T0, T1, T2>(MethodInfo methodInfo, object o, ref T0 p0, T1 p1, T2 p2)
            {
                object[] array = new object[] { p0, p1, p2 };
                object result = methodInfo.Invoke(o, array);
                p0 = (T0)array[0];
                return result;
            }

            public static object InvokeRefs<T0, T1, T2>(MethodInfo methodInfo, object o, T0 p0, ref T1 p1, T2 p2)
            {
                object[] array = new object[] { p0, p1, p2 };
                object result = methodInfo.Invoke(o, array);
                p1 = (T1)array[1];
                return result;
            }

            public static object InvokeRefs<T0, T1, T2>(MethodInfo methodInfo, object o, T0 p0, T1 p1, ref T2 p2)
            {
                object[] array = new object[] { p0, p1, p2 };
                object result = methodInfo.Invoke(o, array);
                p2 = (T2)array[2];
                return result;
            }

            public static object InvokeRefs<T0, T1, T2>(MethodInfo methodInfo, object o, ref T0 p0, ref T1 p1, T2 p2)
            {
                object[] array = new object[] { p0, p1, p2 };
                object result = methodInfo.Invoke(o, array);
                p0 = (T0)array[0];
                p1 = (T1)array[1];
                return result;
            }

            public static object InvokeRefs<T0, T1, T2>(MethodInfo methodInfo, object o, ref T0 p0, T1 p1, ref T2 p2)
            {
                object[] array = new object[] { p0, p1, p2 };
                object result = methodInfo.Invoke(o, array);
                p0 = (T0)array[0];
                p2 = (T2)array[2];
                return result;
            }

            public static object InvokeRefs<T0, T1, T2>(MethodInfo methodInfo, object o, T0 p0, ref T1 p1, ref T2 p2)
            {
                object[] array = new object[] { p0, p1, p2 };
                object result = methodInfo.Invoke(o, array);
                p1 = (T1)array[1];
                p2 = (T2)array[2];
                return result;
            }

            public static object InvokeRefs<T0, T1, T2>(MethodInfo methodInfo, object o, ref T0 p0, ref T1 p1, ref T2 p2)
            {
                object[] array = new object[] { p0, p1, p2 };
                object result = methodInfo.Invoke(o, array);
                p0 = (T0)array[0];
                p1 = (T1)array[1];
                p2 = (T2)array[2];
                return result;
            }
        }
        public static void Init()
        {

            AssetBundle assetBundle = ResourceManager.LoadAssetBundle("shared_auto_001");
            AssetBundle assetBundle2 = ResourceManager.LoadAssetBundle("shared_auto_002");
            shared_auto_001 = assetBundle;
            shared_auto_002 = assetBundle2;
        }
        public static void Add<T>(ref T[] array, T toAdd)
        {
            List<T> list = array.ToList();
            list.Add(toAdd);
            array = list.ToArray<T>();
        }

        public static StatModifier SetupStatModifier(PlayerStats.StatType statType, float modificationAmount, StatModifier.ModifyMethod modifyMethod = StatModifier.ModifyMethod.ADDITIVE, bool breaksOnDamage = false)
        {
            return new StatModifier
            {
                statToBoost = statType,
                amount = modificationAmount,
                modifyType = modifyMethod,
                isMeatBunBuff = breaksOnDamage
            };
        }
        public static DungeonPlaceable GenerateDungeonPlacable(GameObject ObjectPrefab = null, bool spawnsEnemy = false, bool useExternalPrefab = false, bool spawnsItem = false, string EnemyGUID = "479556d05c7c44f3b6abb3b2067fc778", int itemID = 307, Vector2? CustomOffset = null, bool itemHasDebrisObject = true, float spawnChance = 1f)
        {
            AssetBundle m_assetBundle = ResourceManager.LoadAssetBundle("shared_auto_001");
            AssetBundle m_assetBundle2 = ResourceManager.LoadAssetBundle("shared_auto_002");
            AssetBundle m_resourceBundle = ResourceManager.LoadAssetBundle("brave_resources_001");

            // Used with custom DungeonPlacable        
            GameObject ChestBrownTwoItems = m_assetBundle.LoadAsset<GameObject>("Chest_Wood_Two_Items");
            GameObject Chest_Silver = m_assetBundle.LoadAsset<GameObject>("chest_silver");
            GameObject Chest_Green = m_assetBundle.LoadAsset<GameObject>("chest_green");
            GameObject Chest_Synergy = m_assetBundle.LoadAsset<GameObject>("chest_synergy");
            GameObject Chest_Red = m_assetBundle.LoadAsset<GameObject>("chest_red");
            GameObject Chest_Black = m_assetBundle.LoadAsset<GameObject>("Chest_Black");
            GameObject Chest_Rainbow = m_assetBundle.LoadAsset<GameObject>("Chest_Rainbow");
            // GameObject Chest_Rat = m_assetBundle.LoadAsset<GameObject>("Chest_Rat");

            m_assetBundle = null;
            m_assetBundle2 = null;
            m_resourceBundle = null;

            DungeonPlaceableVariant BlueChestVariant = new DungeonPlaceableVariant();
            BlueChestVariant.percentChance = 0.35f;
            BlueChestVariant.unitOffset = new Vector2(1, 0.8f);
            BlueChestVariant.enemyPlaceableGuid = string.Empty;
            BlueChestVariant.pickupObjectPlaceableId = -1;
            BlueChestVariant.forceBlackPhantom = false;
            BlueChestVariant.addDebrisObject = false;
            BlueChestVariant.prerequisites = null;
            BlueChestVariant.materialRequirements = null;
            BlueChestVariant.nonDatabasePlaceable = Chest_Silver;

            DungeonPlaceableVariant BrownChestVariant = new DungeonPlaceableVariant();
            BrownChestVariant.percentChance = 0.28f;
            BrownChestVariant.unitOffset = new Vector2(1, 0.8f);
            BrownChestVariant.enemyPlaceableGuid = string.Empty;
            BrownChestVariant.pickupObjectPlaceableId = -1;
            BrownChestVariant.forceBlackPhantom = false;
            BrownChestVariant.addDebrisObject = false;
            BrownChestVariant.prerequisites = null;
            BrownChestVariant.materialRequirements = null;
            BrownChestVariant.nonDatabasePlaceable = ChestBrownTwoItems;

            DungeonPlaceableVariant GreenChestVariant = new DungeonPlaceableVariant();
            GreenChestVariant.percentChance = 0.25f;
            GreenChestVariant.unitOffset = new Vector2(1, 0.8f);
            GreenChestVariant.enemyPlaceableGuid = string.Empty;
            GreenChestVariant.pickupObjectPlaceableId = -1;
            GreenChestVariant.forceBlackPhantom = false;
            GreenChestVariant.addDebrisObject = false;
            GreenChestVariant.prerequisites = null;
            GreenChestVariant.materialRequirements = null;
            GreenChestVariant.nonDatabasePlaceable = Chest_Green;

            DungeonPlaceableVariant SynergyChestVariant = new DungeonPlaceableVariant();
            SynergyChestVariant.percentChance = 0.2f;
            SynergyChestVariant.unitOffset = new Vector2(1, 0.8f);
            SynergyChestVariant.enemyPlaceableGuid = string.Empty;
            SynergyChestVariant.pickupObjectPlaceableId = -1;
            SynergyChestVariant.forceBlackPhantom = false;
            SynergyChestVariant.addDebrisObject = false;
            SynergyChestVariant.prerequisites = null;
            SynergyChestVariant.materialRequirements = null;
            SynergyChestVariant.nonDatabasePlaceable = Chest_Synergy;

            DungeonPlaceableVariant RedChestVariant = new DungeonPlaceableVariant();
            RedChestVariant.percentChance = 0.15f;
            RedChestVariant.unitOffset = new Vector2(0.5f, 0.5f);
            RedChestVariant.enemyPlaceableGuid = string.Empty;
            RedChestVariant.pickupObjectPlaceableId = -1;
            RedChestVariant.forceBlackPhantom = false;
            RedChestVariant.addDebrisObject = false;
            RedChestVariant.prerequisites = null;
            RedChestVariant.materialRequirements = null;
            RedChestVariant.nonDatabasePlaceable = Chest_Red;

            DungeonPlaceableVariant BlackChestVariant = new DungeonPlaceableVariant();
            BlackChestVariant.percentChance = 0.1f;
            BlackChestVariant.unitOffset = new Vector2(0.5f, 0.5f);
            BlackChestVariant.enemyPlaceableGuid = string.Empty;
            BlackChestVariant.pickupObjectPlaceableId = -1;
            BlackChestVariant.forceBlackPhantom = false;
            BlackChestVariant.addDebrisObject = false;
            BlackChestVariant.prerequisites = null;
            BlackChestVariant.materialRequirements = null;
            BlackChestVariant.nonDatabasePlaceable = Chest_Black;

            DungeonPlaceableVariant RainbowChestVariant = new DungeonPlaceableVariant();
            RainbowChestVariant.percentChance = 0.005f;
            RainbowChestVariant.unitOffset = new Vector2(0.5f, 0.5f);
            RainbowChestVariant.enemyPlaceableGuid = string.Empty;
            RainbowChestVariant.pickupObjectPlaceableId = -1;
            RainbowChestVariant.forceBlackPhantom = false;
            RainbowChestVariant.addDebrisObject = false;
            RainbowChestVariant.prerequisites = null;
            RainbowChestVariant.materialRequirements = null;
            RainbowChestVariant.nonDatabasePlaceable = Chest_Rainbow;

            DungeonPlaceableVariant ItemVariant = new DungeonPlaceableVariant();
            ItemVariant.percentChance = spawnChance;
            if (CustomOffset.HasValue)
            {
                ItemVariant.unitOffset = CustomOffset.Value;
            }
            else
            {
                ItemVariant.unitOffset = Vector2.zero;
            }
            // ItemVariant.unitOffset = new Vector2(0.5f, 0.8f);
            ItemVariant.enemyPlaceableGuid = string.Empty;
            ItemVariant.pickupObjectPlaceableId = itemID;
            ItemVariant.forceBlackPhantom = false;
            if (itemHasDebrisObject)
            {
                ItemVariant.addDebrisObject = true;
            }
            else
            {
                ItemVariant.addDebrisObject = false;
            }
            RainbowChestVariant.prerequisites = null;
            RainbowChestVariant.materialRequirements = null;

            List<DungeonPlaceableVariant> ChestTiers = new List<DungeonPlaceableVariant>();
            ChestTiers.Add(BrownChestVariant);
            ChestTiers.Add(BlueChestVariant);
            ChestTiers.Add(GreenChestVariant);
            ChestTiers.Add(SynergyChestVariant);
            ChestTiers.Add(RedChestVariant);
            ChestTiers.Add(BlackChestVariant);
            ChestTiers.Add(RainbowChestVariant);

            DungeonPlaceableVariant EnemyVariant = new DungeonPlaceableVariant();
            EnemyVariant.percentChance = spawnChance;
            EnemyVariant.unitOffset = Vector2.zero;
            EnemyVariant.enemyPlaceableGuid = EnemyGUID;
            EnemyVariant.pickupObjectPlaceableId = -1;
            EnemyVariant.forceBlackPhantom = false;
            EnemyVariant.addDebrisObject = false;
            EnemyVariant.prerequisites = null;
            EnemyVariant.materialRequirements = null;

            List<DungeonPlaceableVariant> EnemyTiers = new List<DungeonPlaceableVariant>();
            EnemyTiers.Add(EnemyVariant);

            List<DungeonPlaceableVariant> ItemTiers = new List<DungeonPlaceableVariant>();
            ItemTiers.Add(ItemVariant);

            DungeonPlaceable m_cachedCustomPlacable = ScriptableObject.CreateInstance<DungeonPlaceable>();
            m_cachedCustomPlacable.name = "CustomChestPlacable";
            if (spawnsEnemy | useExternalPrefab)
            {
                m_cachedCustomPlacable.width = 2;
                m_cachedCustomPlacable.height = 2;
            }
            else if (spawnsItem)
            {
                m_cachedCustomPlacable.width = 1;
                m_cachedCustomPlacable.height = 1;
            }
            else
            {
                m_cachedCustomPlacable.width = 4;
                m_cachedCustomPlacable.height = 1;
            }
            m_cachedCustomPlacable.roomSequential = false;
            m_cachedCustomPlacable.respectsEncounterableDifferentiator = true;
            m_cachedCustomPlacable.UsePrefabTransformOffset = false;
            m_cachedCustomPlacable.isPassable = true;
            if (spawnsItem)
            {
                m_cachedCustomPlacable.MarkSpawnedItemsAsRatIgnored = true;
            }
            else
            {
                m_cachedCustomPlacable.MarkSpawnedItemsAsRatIgnored = false;
            }

            m_cachedCustomPlacable.DebugThisPlaceable = false;
            if (useExternalPrefab && ObjectPrefab != null)
            {
                DungeonPlaceableVariant ExternalObjectVariant = new DungeonPlaceableVariant();
                ExternalObjectVariant.percentChance = spawnChance;
                if (CustomOffset.HasValue)
                {
                    ExternalObjectVariant.unitOffset = CustomOffset.Value;
                }
                else
                {
                    ExternalObjectVariant.unitOffset = Vector2.zero;
                }
                ExternalObjectVariant.enemyPlaceableGuid = string.Empty;
                ExternalObjectVariant.pickupObjectPlaceableId = -1;
                ExternalObjectVariant.forceBlackPhantom = false;
                ExternalObjectVariant.addDebrisObject = false;
                ExternalObjectVariant.nonDatabasePlaceable = ObjectPrefab;
                List<DungeonPlaceableVariant> ExternalObjectTiers = new List<DungeonPlaceableVariant>();
                ExternalObjectTiers.Add(ExternalObjectVariant);
                m_cachedCustomPlacable.variantTiers = ExternalObjectTiers;
            }
            else if (spawnsEnemy)
            {
                m_cachedCustomPlacable.variantTiers = EnemyTiers;
            }
            else if (spawnsItem)
            {
                m_cachedCustomPlacable.variantTiers = ItemTiers;
            }
            else
            {
                m_cachedCustomPlacable.variantTiers = ChestTiers;
            }
            return m_cachedCustomPlacable;
        }

        public static Material Copy(this Material orig, Texture2D textureOverride = null, Shader shaderOverride = null)
        {
            Material m_NewMaterial = new Material(orig.shader)
            {
                name = orig.name,
                shaderKeywords = orig.shaderKeywords,
                globalIlluminationFlags = orig.globalIlluminationFlags,
                enableInstancing = orig.enableInstancing,
                doubleSidedGI = orig.doubleSidedGI,
                mainTextureOffset = orig.mainTextureOffset,
                mainTextureScale = orig.mainTextureScale,
                renderQueue = orig.renderQueue,
                color = orig.color,
                hideFlags = orig.hideFlags
            };
            if (textureOverride != null)
            {
                m_NewMaterial.mainTexture = textureOverride;
            }
            else
            {
                m_NewMaterial.mainTexture = orig.mainTexture;
            }

            if (shaderOverride != null)
            {
                m_NewMaterial.shader = shaderOverride;
            }
            else
            {
                m_NewMaterial.shader = orig.shader;
            }
            return m_NewMaterial;
        }

        public static tk2dSpriteDefinition Copy(this tk2dSpriteDefinition orig)
        {
            tk2dSpriteDefinition m_newSpriteCollection = new tk2dSpriteDefinition();

            m_newSpriteCollection.boundsDataCenter = orig.boundsDataCenter;
            m_newSpriteCollection.boundsDataExtents = orig.boundsDataExtents;
            m_newSpriteCollection.colliderConvex = orig.colliderConvex;
            m_newSpriteCollection.colliderSmoothSphereCollisions = orig.colliderSmoothSphereCollisions;
            m_newSpriteCollection.colliderType = orig.colliderType;
            m_newSpriteCollection.colliderVertices = orig.colliderVertices;
            m_newSpriteCollection.collisionLayer = orig.collisionLayer;
            m_newSpriteCollection.complexGeometry = orig.complexGeometry;
            m_newSpriteCollection.extractRegion = orig.extractRegion;
            m_newSpriteCollection.flipped = orig.flipped;
            m_newSpriteCollection.indices = orig.indices;
            if (orig.material != null) { m_newSpriteCollection.material = new Material(orig.material); }
            m_newSpriteCollection.materialId = orig.materialId;
            if (orig.materialInst != null) { m_newSpriteCollection.materialInst = new Material(orig.materialInst); }
            m_newSpriteCollection.metadata = orig.metadata;
            m_newSpriteCollection.name = orig.name;
            m_newSpriteCollection.normals = orig.normals;
            m_newSpriteCollection.physicsEngine = orig.physicsEngine;
            m_newSpriteCollection.position0 = orig.position0;
            m_newSpriteCollection.position1 = orig.position1;
            m_newSpriteCollection.position2 = orig.position2;
            m_newSpriteCollection.position3 = orig.position3;
            m_newSpriteCollection.regionH = orig.regionH;
            m_newSpriteCollection.regionW = orig.regionW;
            m_newSpriteCollection.regionX = orig.regionX;
            m_newSpriteCollection.regionY = orig.regionY;
            m_newSpriteCollection.tangents = orig.tangents;
            m_newSpriteCollection.texelSize = orig.texelSize;
            m_newSpriteCollection.untrimmedBoundsDataCenter = orig.untrimmedBoundsDataCenter;
            m_newSpriteCollection.untrimmedBoundsDataExtents = orig.untrimmedBoundsDataExtents;
            m_newSpriteCollection.uvs = orig.uvs;

            return m_newSpriteCollection;
        }
        public static tk2dSpriteCollectionData ReplaceDungeonCollection(tk2dSpriteCollectionData sourceCollection, Texture2D spriteSheet = null, List<string> spriteList = null)
        {
            if (sourceCollection == null) { return null; }
            tk2dSpriteCollectionData collectionData = UnityEngine.Object.Instantiate(sourceCollection);
            tk2dSpriteDefinition[] spriteDefinietions = new tk2dSpriteDefinition[collectionData.spriteDefinitions.Length];
            for (int i = 0; i < collectionData.spriteDefinitions.Length; i++) { spriteDefinietions[i] = collectionData.spriteDefinitions[i].Copy(); }
            collectionData.spriteDefinitions = spriteDefinietions;
            if (spriteSheet != null)
            {
                Material[] materials = sourceCollection.materials;
                Material[] newMaterials = new Material[materials.Length];
                if (materials != null)
                {
                    for (int i = 0; i < materials.Length; i++)
                    {
                        newMaterials[i] = materials[i].Copy(spriteSheet);
                    }
                    collectionData.materials = newMaterials;
                    foreach (Material material2 in collectionData.materials)
                    {
                        foreach (tk2dSpriteDefinition spriteDefinition in collectionData.spriteDefinitions)
                        {
                            bool flag3 = material2 != null && spriteDefinition.material.name.Equals(material2.name);
                            if (flag3)
                            {
                                spriteDefinition.material = material2;
                                spriteDefinition.materialInst = new Material(material2);
                            }
                        }
                    }
                }
            }
            else if (spriteList != null)
            {
                RuntimeAtlasPage runtimeAtlasPage = new RuntimeAtlasPage(0, 0, TextureFormat.RGBA32, 2);
                for (int i = 0; i < spriteList.Count; i++)
                {
                    Texture2D texture2D = ResourceExtractor.GetTextureFromResource(spriteList[i]);
                    if (!texture2D)
                    {
                        Debug.Log("[BuildDungeonCollection] Null Texture found at index: " + i);
                        goto IL_EXIT;
                    }
                    float X = (texture2D.width / 16f);
                    float Y = (texture2D.height / 16f);
                    // tk2dSpriteDefinition spriteData = collectionData.GetSpriteDefinition(i.ToString());
                    tk2dSpriteDefinition spriteData = collectionData.spriteDefinitions[i];
                    if (spriteData != null)
                    {
                        if (spriteData.boundsDataCenter != Vector3.zero)
                        {
                            try
                            {
                                // Debug.Log("[BuildDungeonCollection] Pack Existing Atlas Element at index: " + i);
                                RuntimeAtlasSegment runtimeAtlasSegment = runtimeAtlasPage.Pack(texture2D, false);
                                spriteData.materialInst.mainTexture = runtimeAtlasSegment.texture;
                                spriteData.uvs = runtimeAtlasSegment.uvs;
                                spriteData.extractRegion = true;
                                spriteData.position0 = Vector3.zero;
                                spriteData.position1 = new Vector3(X, 0, 0);
                                spriteData.position2 = new Vector3(0, Y, 0);
                                spriteData.position3 = new Vector3(X, Y, 0);
                                spriteData.boundsDataCenter = new Vector2((X / 2), (Y / 2));
                                spriteData.untrimmedBoundsDataCenter = spriteData.boundsDataCenter;
                                spriteData.boundsDataExtents = new Vector2(X, Y);
                                spriteData.untrimmedBoundsDataExtents = spriteData.boundsDataExtents;
                            }
                            catch (Exception)
                            {
                                Debug.Log("[BuildDungeonCollection] Exception caught at index: " + i);
                            }
                        }
                        else
                        {
                            // Debug.Log("Test 3. Replace Existing Atlas Element at index: " + i);
                            try
                            {
                                ETGMod.ReplaceTexture(spriteData, texture2D, true);
                            }
                            catch (Exception)
                            {
                                Debug.Log("[BuildDungeonCollection] Exception caught at index: " + i);
                            }
                        }
                    }
                    else
                    {
                        Debug.Log("[BuildDungeonCollection] SpriteData is null at index: " + i);
                    }
                IL_EXIT:;
                }
                runtimeAtlasPage.Apply();
            }
            else
            {
                Debug.Log("[BuildDungeonCollection] SpriteList is null!");
            }
            return collectionData;
        }
        public static void PlaceItemInAmmonomiconAfterItemById(this PickupObject item, int id)
        {
            item.ForcedPositionInAmmonomicon = PickupObjectDatabase.GetById(id).ForcedPositionInAmmonomicon;
        }


        public static SpeculativeRigidbody GenerateOrAddToRigidBody(GameObject targetObject, CollisionLayer collisionLayer, PixelCollider.PixelColliderGeneration colliderGenerationMode = PixelCollider.PixelColliderGeneration.Tk2dPolygon, bool collideWithTileMap = false, bool CollideWithOthers = true, bool CanBeCarried = true, bool CanBePushed = false, bool RecheckTriggers = false, bool IsTrigger = false, bool replaceExistingColliders = false, bool UsesPixelsAsUnitSize = false, IntVector2? dimensions = null, IntVector2? offset = null)
        {
            SpeculativeRigidbody m_CachedRigidBody = GameObjectExtensions.GetOrAddComponent<SpeculativeRigidbody>(targetObject);
            m_CachedRigidBody.CollideWithOthers = CollideWithOthers;
            m_CachedRigidBody.CollideWithTileMap = collideWithTileMap;
            m_CachedRigidBody.Velocity = Vector2.zero;
            m_CachedRigidBody.MaxVelocity = Vector2.zero;
            m_CachedRigidBody.ForceAlwaysUpdate = false;
            m_CachedRigidBody.CanPush = false;
            m_CachedRigidBody.CanBePushed = CanBePushed;
            m_CachedRigidBody.PushSpeedModifier = 1f;
            m_CachedRigidBody.CanCarry = false;
            m_CachedRigidBody.CanBeCarried = CanBeCarried;
            m_CachedRigidBody.PreventPiercing = false;
            m_CachedRigidBody.SkipEmptyColliders = false;
            m_CachedRigidBody.RecheckTriggers = RecheckTriggers;
            m_CachedRigidBody.UpdateCollidersOnRotation = false;
            m_CachedRigidBody.UpdateCollidersOnScale = false;

            IntVector2 Offset = IntVector2.Zero;
            IntVector2 Dimensions = IntVector2.Zero;
            if (colliderGenerationMode != PixelCollider.PixelColliderGeneration.Tk2dPolygon)
            {
                if (dimensions.HasValue)
                {
                    Dimensions = dimensions.Value;
                    if (!UsesPixelsAsUnitSize)
                    {
                        Dimensions = (new IntVector2(Dimensions.x * 16, Dimensions.y * 16));
                    }
                }
                if (offset.HasValue)
                {
                    Offset = offset.Value;
                    if (!UsesPixelsAsUnitSize)
                    {
                        Offset = (new IntVector2(Offset.x * 16, Offset.y * 16));
                    }
                }
            }
            PixelCollider m_CachedCollider = new PixelCollider()
            {
                ColliderGenerationMode = colliderGenerationMode,
                CollisionLayer = collisionLayer,
                IsTrigger = IsTrigger,
                BagleUseFirstFrameOnly = (colliderGenerationMode == PixelCollider.PixelColliderGeneration.Tk2dPolygon),
                SpecifyBagelFrame = string.Empty,
                BagelColliderNumber = 0,
                ManualOffsetX = Offset.x,
                ManualOffsetY = Offset.y,
                ManualWidth = Dimensions.x,
                ManualHeight = Dimensions.y,
                ManualDiameter = 0,
                ManualLeftX = 0,
                ManualLeftY = 0,
                ManualRightX = 0,
                ManualRightY = 0
            };

            if (replaceExistingColliders | m_CachedRigidBody.PixelColliders == null)
            {
                m_CachedRigidBody.PixelColliders = new List<PixelCollider> { m_CachedCollider };
            }
            else
            {
                m_CachedRigidBody.PixelColliders.Add(m_CachedCollider);
            }

            if (m_CachedRigidBody.sprite && colliderGenerationMode == PixelCollider.PixelColliderGeneration.Tk2dPolygon)
            {
                Bounds bounds = m_CachedRigidBody.sprite.GetBounds();
                m_CachedRigidBody.sprite.GetTrueCurrentSpriteDef().colliderVertices = new Vector3[] { bounds.center - bounds.extents, bounds.center + bounds.extents };
                // m_CachedRigidBody.ForceRegenerate();
                // m_CachedRigidBody.RegenerateCache();
            }

            return m_CachedRigidBody;
        }
        public static DungeonFlowNode GenerateFlowNode(DungeonFlow flow, PrototypeDungeonRoom.RoomCategory category, PrototypeDungeonRoom overrideRoom = null, GenericRoomTable overrideRoomTable = null, bool loopTargetIsOneWay = false, bool isWarpWing = false,
           bool handlesOwnWarping = true, float weight = 1f, DungeonFlowNode.NodePriority priority = DungeonFlowNode.NodePriority.MANDATORY, string guid = "")
        {
            if (string.IsNullOrEmpty(guid))
            {
                guid = Guid.NewGuid().ToString();
            }
            DungeonFlowNode node = new DungeonFlowNode(flow)
            {
                isSubchainStandin = false,
                nodeType = DungeonFlowNode.ControlNodeType.ROOM,
                roomCategory = category,
                percentChance = weight,
                priority = priority,
                overrideExactRoom = overrideRoom,
                overrideRoomTable = overrideRoomTable,
                capSubchain = false,
                subchainIdentifier = string.Empty,
                limitedCopiesOfSubchain = false,
                maxCopiesOfSubchain = 1,
                subchainIdentifiers = new List<string>(0),
                receivesCaps = false,
                isWarpWingEntrance = isWarpWing,
                handlesOwnWarping = handlesOwnWarping,
                forcedDoorType = DungeonFlowNode.ForcedDoorType.NONE,
                loopForcedDoorType = DungeonFlowNode.ForcedDoorType.NONE,
                nodeExpands = false,
                initialChainPrototype = "n",
                chainRules = new List<ChainRule>(0),
                minChainLength = 3,
                maxChainLength = 8,
                minChildrenToBuild = 1,
                maxChildrenToBuild = 1,
                canBuildDuplicateChildren = false,
                guidAsString = guid,
                parentNodeGuid = string.Empty,
                childNodeGuids = new List<string>(0),
                loopTargetNodeGuid = string.Empty,
                loopTargetIsOneWay = loopTargetIsOneWay,
                flow = flow
            };
            return node;
            

        }

        public static AssetBundle shared_auto_002;
        public static AssetBundle shared_auto_001;

        public static bool PlayerHasActiveSynergy(this PlayerController player, string synergyNameToCheck)
        {
            foreach (int index in player.ActiveExtraSynergies)
            {
                AdvancedSynergyEntry synergy = GameManager.Instance.SynergyManager.synergies[index];
                if (synergy.NameKey == synergyNameToCheck)
                {
                    return true;
                }
            }
            return false;
        }
    }
}

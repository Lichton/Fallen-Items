using Dungeonator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CakeMod
{
    class f1b_Hat_flow_01 : FloorNameDungeonFlows
    {
		public static DungeonFlow F1b_Hat_flow_01()
		{
			try
			{

				DungeonFlow m_CachedFlow = ScriptableObject.CreateInstance<DungeonFlow>();
				DungeonFlowNode entranceNode = GenerateDefaultNode(m_CachedFlow, PrototypeDungeonRoom.RoomCategory.ENTRANCE, ModRoomPrefabs.Mod_Entrance_Room);
				DungeonFlowNode exitNode = GenerateDefaultNode(m_CachedFlow, PrototypeDungeonRoom.RoomCategory.EXIT, ModRoomPrefabs.Mod_Exit_Room);
				DungeonFlowNode HatRoomNode_01 = GenerateDefaultNode(m_CachedFlow, PrototypeDungeonRoom.RoomCategory.HUB);
				DungeonFlowNode HatRoomNode_02 = GenerateDefaultNode(m_CachedFlow, PrototypeDungeonRoom.RoomCategory.NORMAL);

				m_CachedFlow.name = "F1b_FloorName_Flow_01";
				m_CachedFlow.fallbackRoomTable = ModPrefabs.FloorNameRoomTable;
				m_CachedFlow.phantomRoomTable = null;
				m_CachedFlow.subtypeRestrictions = new List<DungeonFlowSubtypeRestriction>(0);
				m_CachedFlow.flowInjectionData = new List<ProceduralFlowModifierData>(0);
				m_CachedFlow.sharedInjectionData = new List<SharedInjectionData>() { BaseSharedInjectionData };

				m_CachedFlow.Initialize();

				m_CachedFlow.AddNodeToFlow(entranceNode, null);
				// First Looping branch
				m_CachedFlow.AddNodeToFlow(HatRoomNode_01, entranceNode);
				m_CachedFlow.AddNodeToFlow(HatRoomNode_02, HatRoomNode_01);
				m_CachedFlow.AddNodeToFlow(exitNode, HatRoomNode_02);
				m_CachedFlow.FirstNode = entranceNode;

				return m_CachedFlow;
			}
			catch (Exception e)
			{
				ETGModConsole.Log(e.ToString());
				return null;
			}
		}
	}
}

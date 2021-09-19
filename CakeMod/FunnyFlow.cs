using Dungeonator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CakeMod
{
	class FunnyFlow : FloorNameDungeonFlows
	{
		public static DungeonFlow funnyflow()
		{
			try
			{

				DungeonFlow m_CachedFlow = ScriptableObject.CreateInstance<DungeonFlow>();

				DungeonFlowNode entranceNode = GenerateDefaultNode(m_CachedFlow, PrototypeDungeonRoom.RoomCategory.ENTRANCE, ModRoomPrefabs.Mod_Entrance_Room);
				DungeonFlowNode exitNode = GenerateDefaultNode(m_CachedFlow, PrototypeDungeonRoom.RoomCategory.EXIT, ModRoomPrefabs.Mod_Exit_Room);
				DungeonFlowNode funnynode1 = GenerateDefaultNode(m_CachedFlow, PrototypeDungeonRoom.RoomCategory.HUB);
				DungeonFlowNode funnynode2 = GenerateDefaultNode(m_CachedFlow, PrototypeDungeonRoom.RoomCategory.NORMAL);

				m_CachedFlow.name = "F1b_FloorName_Flow_01";
				m_CachedFlow.fallbackRoomTable = ModPrefabs.FloorNameRoomTable;
				m_CachedFlow.phantomRoomTable = null;
				m_CachedFlow.subtypeRestrictions = new List<DungeonFlowSubtypeRestriction>(0);
				m_CachedFlow.flowInjectionData = new List<ProceduralFlowModifierData>(0);
				m_CachedFlow.sharedInjectionData = new List<SharedInjectionData>() { BaseSharedInjectionData };

				m_CachedFlow.Initialize();

				m_CachedFlow.AddNodeToFlow(entranceNode, null);
				// First Looping branch
				m_CachedFlow.AddNodeToFlow(funnynode1, entranceNode);
				m_CachedFlow.AddNodeToFlow(funnynode2, funnynode1);
				m_CachedFlow.AddNodeToFlow(exitNode, funnynode2);
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

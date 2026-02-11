//auto generated
using Framework.AT.Runtime;
#if USE_FIXEDMATH
using ExternEngine;
#else
using FBounds = UnityEngine.Bounds;
#endif
namespace Framework.ActorSystem.Runtime
{
#if UNITY_EDITOR
	[ATClass(typeof(Framework.ActorSystem.Runtime.ActorManager),"Actor系统/管理器")]
#endif
	public class Framework_ActorSystem_Runtime_ActorManager
	{
#if UNITY_EDITOR
		[ATFunction(-1714752779,"设置空间大小",typeof(Framework.ActorSystem.Runtime.ActorManager),false)]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableBounds),"worldBounds",false, null,typeof(FBounds))]
#endif
		static bool AT_InitializeSpatialIndex(ActorManager pPointerThis, FBounds worldBounds)
		{
			pPointerThis.InitializeSpatialIndex(worldBounds);
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(-1220209520,"设置空间划分类型",typeof(Framework.ActorSystem.Runtime.ActorManager),false)]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableInt),"indexType",false, null,typeof(Framework.ActorSystem.Runtime.ESpatialIndexType))]
#endif
		static bool AT_SetSpatialIndexType(ActorManager pPointerThis,Framework.ActorSystem.Runtime.ESpatialIndexType indexType)
		{
			pPointerThis.SetSpatialIndexType(indexType);
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(654256170,"设置空间划分开关",typeof(Framework.ActorSystem.Runtime.ActorManager),false)]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableBool),"enabled",false, null,typeof(System.Boolean))]
#endif
		static bool AT_SetSpatialIndexEnabled(ActorManager pPointerThis,System.Boolean enabled)
		{
			pPointerThis.SetSpatialIndexEnabled(enabled);
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(1299128952,"获取空间划分类型",typeof(Framework.ActorSystem.Runtime.ActorManager),false)]
		[ATFunctionReturn(typeof(Framework.AT.Runtime.VariableInt), "pReturn", null,typeof(Framework.ActorSystem.Runtime.ESpatialIndexType))]
#endif
		static bool AT_GetSpatialIndexType(ActorManager pPointerThis,AgentTree pAgentTree, BaseNode pNode)
		{
			pAgentTree.SetOutportInt(pNode, 0, (int)pPointerThis.GetSpatialIndexType());
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(-1587347659,"是否开始空间划分",typeof(Framework.ActorSystem.Runtime.ActorManager),false)]
		[ATFunctionReturn(typeof(Framework.AT.Runtime.VariableBool), "pReturn", null,typeof(System.Boolean))]
#endif
		static bool AT_IsSpatialIndexEnabled(ActorManager pPointerThis,AgentTree pAgentTree, BaseNode pNode)
		{
			pAgentTree.SetOutportBool(pNode, 0, pPointerThis.IsSpatialIndexEnabled());
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(334129816,"设置地形碰撞层Mask",typeof(Framework.ActorSystem.Runtime.ActorManager),false)]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableInt),"layerMask",false, null,typeof(System.Int32))]
#endif
		static bool AT_SetTerrainLayerMask(ActorManager pPointerThis,System.Int32 layerMask)
		{
			pPointerThis.SetTerrainLayerMask(layerMask);
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(-373269392,"获取地形碰撞层Mask",typeof(Framework.ActorSystem.Runtime.ActorManager),false)]
		[ATFunctionReturn(typeof(Framework.AT.Runtime.VariableInt), "pReturn", null,typeof(System.Int32))]
#endif
		static bool AT_GetTerrainLayerMask(ActorManager pPointerThis,AgentTree pAgentTree, BaseNode pNode)
		{
			pAgentTree.SetOutportInt(pNode, 0, pPointerThis.GetTerrainLayerMask());
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(491321879,"设置地表最低高度",typeof(Framework.ActorSystem.Runtime.ActorManager),false)]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableFloat),"layerMask",false, null,typeof(System.Single))]
#endif
		static bool AT_SetTerrainHeight(ActorManager pPointerThis,System.Single layerMask)
		{
			pPointerThis.SetTerrainHeight(layerMask);
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(794724366,"获取地表最低高度",typeof(Framework.ActorSystem.Runtime.ActorManager),false)]
		[ATFunctionReturn(typeof(Framework.AT.Runtime.VariableFloat), "pReturn", null,typeof(System.Single))]
#endif
		static bool AT_GetTerrainHeight(ActorManager pPointerThis,AgentTree pAgentTree, BaseNode pNode)
		{
			pAgentTree.SetOutportFloat(pNode, 0, pPointerThis.GetTerrainHeight());
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(-903371460,"浮点区间随机",typeof(Framework.ActorSystem.Runtime.ActorManager),false)]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableFloat),"lower",false, null,typeof(System.Single))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableFloat),"upper",false, null,typeof(System.Single))]
		[ATFunctionReturn(typeof(Framework.AT.Runtime.VariableFloat), "pReturn", null,typeof(System.Single))]
#endif
		static bool AT_GetRandom(ActorManager pPointerThis,System.Single lower,System.Single upper,AgentTree pAgentTree, BaseNode pNode)
		{
			pAgentTree.SetOutportFloat(pNode, 0, pPointerThis.GetRandom(lower,upper));
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(-127158303,"整数区间随机",typeof(Framework.ActorSystem.Runtime.ActorManager),false)]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableInt),"lower",false, null,typeof(System.Int32))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableInt),"upper",false, null,typeof(System.Int32))]
		[ATFunctionReturn(typeof(Framework.AT.Runtime.VariableInt), "pReturn", null,typeof(System.Int32))]
#endif
		static bool AT_GetRandom_1(ActorManager pPointerThis,System.Int32 lower,System.Int32 upper,AgentTree pAgentTree, BaseNode pNode)
		{
			pAgentTree.SetOutportInt(pNode, 0, pPointerThis.GetRandom(lower,upper));
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(977072972,"同步创建Actor",typeof(Framework.ActorSystem.Runtime.ActorManager),false)]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableUserData),"pData",false, null,typeof(Framework.ActorSystem.Runtime.IContextData))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableUserData),"userVariable",false, null,typeof(Framework.ActorSystem.Runtime.IContextData))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableInt),"nodeID",false, null,typeof(System.Int32))]
		[ATFunctionReturn(typeof(Framework.AT.Runtime.VariableUserData), "pReturn", null,typeof(Framework.ActorSystem.Runtime.Actor))]
#endif
		static bool AT_CreateActor(ActorManager pPointerThis,Framework.ActorSystem.Runtime.IContextData pData,Framework.ActorSystem.Runtime.IContextData userVariable,System.Int32 nodeID,AgentTree pAgentTree, BaseNode pNode)
		{
			pAgentTree.SetOutportUserData(pNode, 0, pPointerThis.CreateActor(pData,userVariable,nodeID));
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(1895192195,"异步创建Actor",typeof(Framework.ActorSystem.Runtime.ActorManager),false)]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableInt),"nodeID",false, null,typeof(System.Int32))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableUserData),"pData",false, null,typeof(Framework.ActorSystem.Runtime.IContextData))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableUserData),"userVariable",false, null,typeof(Framework.ActorSystem.Runtime.IContextData))]
		[ATFunctionReturn(typeof(Framework.AT.Runtime.VariableUserData), "pReturn", null,typeof(Framework.ActorSystem.Runtime.Actor))]
#endif
		static bool AT_AsyncCreateActor(ActorManager pPointerThis,System.Int32 nodeID,Framework.ActorSystem.Runtime.IContextData pData,Framework.ActorSystem.Runtime.IContextData userVariable,AgentTree pAgentTree, BaseNode pNode)
		{
			pAgentTree.SetOutportUserData(pNode, 0, pPointerThis.AsyncCreateActor(nodeID,pData,userVariable));
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(-1912114985,"根据ID获取Actor",typeof(Framework.ActorSystem.Runtime.ActorManager),false)]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableInt),"id",false, null,typeof(System.Int32))]
		[ATFunctionReturn(typeof(Framework.AT.Runtime.VariableUserData), "pReturn", null,typeof(Framework.ActorSystem.Runtime.Actor))]
#endif
		static bool AT_GetActor(ActorManager pPointerThis,System.Int32 id,AgentTree pAgentTree, BaseNode pNode)
		{
			pAgentTree.SetOutportUserData(pNode, 0, pPointerThis.GetActor(id));
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(-66015392,"停掉所有指定对象在时间内的弹道",typeof(Framework.ActorSystem.Runtime.ActorManager),false)]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableUserData),"pActor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableFloat),"fLaucherTime",false, null,typeof(System.Single))]
#endif
		static bool AT_StopProjectileByOwner(ActorManager pPointerThis,Framework.ActorSystem.Runtime.Actor pActor,System.Single fLaucherTime)
		{
			pPointerThis.StopProjectileByOwner(pActor,fLaucherTime);
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(362626841,"停掉所有弹道",typeof(Framework.ActorSystem.Runtime.ActorManager),false)]
#endif
		static bool AT_StopAllProjectiles(ActorManager pPointerThis)
		{
			pPointerThis.StopAllProjectiles();
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(-533418135,"删除所有Actor对象",typeof(Framework.ActorSystem.Runtime.ActorManager),false)]
#endif
		static bool AT_Clear(ActorManager pPointerThis)
		{
			pPointerThis.Clear();
			return true;
		}

		public static bool DoAction(VariableUserData pUserClass, AgentTree pAgentTree, BaseNode pNode)
		{
			int actionType = pNode.type;
			switch(actionType)
			{
			case -1714752779://InitializeSpatialIndex
			{
				if(pNode.GetInportCount() <= 1) return true;
				Framework.ActorSystem.Runtime.ActorManager pModulePointer = pAgentTree.GetModule<Framework.ActorSystem.Runtime.ActorManager>();
				if(pModulePointer == null) return true;
				return AT_InitializeSpatialIndex(pModulePointer,pAgentTree.GetInportBounds(pNode,1));
			}
			case -1220209520://SetSpatialIndexType
			{
				if(pNode.GetInportCount() <= 1) return true;
				Framework.ActorSystem.Runtime.ActorManager pModulePointer = pAgentTree.GetModule<Framework.ActorSystem.Runtime.ActorManager>();
				if(pModulePointer == null) return true;
				return AT_SetSpatialIndexType(pModulePointer,(Framework.ActorSystem.Runtime.ESpatialIndexType)pAgentTree.GetInportInt(pNode,1));
			}
			case 654256170://SetSpatialIndexEnabled
			{
				if(pNode.GetInportCount() <= 1) return true;
				Framework.ActorSystem.Runtime.ActorManager pModulePointer = pAgentTree.GetModule<Framework.ActorSystem.Runtime.ActorManager>();
				if(pModulePointer == null) return true;
				return AT_SetSpatialIndexEnabled(pModulePointer,pAgentTree.GetInportBool(pNode,1));
			}
			case 1299128952://GetSpatialIndexType
			{
				if(pNode.GetInportCount() <= 0) return true;
				Framework.ActorSystem.Runtime.ActorManager pModulePointer = pAgentTree.GetModule<Framework.ActorSystem.Runtime.ActorManager>();
				if(pModulePointer == null) return true;
				return AT_GetSpatialIndexType(pModulePointer,pAgentTree, pNode);
			}
			case -1587347659://IsSpatialIndexEnabled
			{
				if(pNode.GetInportCount() <= 0) return true;
				Framework.ActorSystem.Runtime.ActorManager pModulePointer = pAgentTree.GetModule<Framework.ActorSystem.Runtime.ActorManager>();
				if(pModulePointer == null) return true;
				return AT_IsSpatialIndexEnabled(pModulePointer,pAgentTree, pNode);
			}
			case 334129816://SetTerrainLayerMask
			{
				if(pNode.GetInportCount() <= 1) return true;
				Framework.ActorSystem.Runtime.ActorManager pModulePointer = pAgentTree.GetModule<Framework.ActorSystem.Runtime.ActorManager>();
				if(pModulePointer == null) return true;
				return AT_SetTerrainLayerMask(pModulePointer,pAgentTree.GetInportInt(pNode,1));
			}
			case -373269392://GetTerrainLayerMask
			{
				if(pNode.GetInportCount() <= 0) return true;
				Framework.ActorSystem.Runtime.ActorManager pModulePointer = pAgentTree.GetModule<Framework.ActorSystem.Runtime.ActorManager>();
				if(pModulePointer == null) return true;
				return AT_GetTerrainLayerMask(pModulePointer,pAgentTree, pNode);
			}
			case 491321879://SetTerrainHeight
			{
				if(pNode.GetInportCount() <= 1) return true;
				Framework.ActorSystem.Runtime.ActorManager pModulePointer = pAgentTree.GetModule<Framework.ActorSystem.Runtime.ActorManager>();
				if(pModulePointer == null) return true;
				return AT_SetTerrainHeight(pModulePointer,pAgentTree.GetInportFloat(pNode,1));
			}
			case 794724366://GetTerrainHeight
			{
				if(pNode.GetInportCount() <= 0) return true;
				Framework.ActorSystem.Runtime.ActorManager pModulePointer = pAgentTree.GetModule<Framework.ActorSystem.Runtime.ActorManager>();
				if(pModulePointer == null) return true;
				return AT_GetTerrainHeight(pModulePointer,pAgentTree, pNode);
			}
			case -903371460://GetRandom
			{
				if(pNode.GetInportCount() <= 2) return true;
				Framework.ActorSystem.Runtime.ActorManager pModulePointer = pAgentTree.GetModule<Framework.ActorSystem.Runtime.ActorManager>();
				if(pModulePointer == null) return true;
				return AT_GetRandom(pModulePointer,pAgentTree.GetInportFloat(pNode,1),pAgentTree.GetInportFloat(pNode,2), pAgentTree, pNode);
			}
			case -127158303://GetRandom
			{
				if(pNode.GetInportCount() <= 2) return true;
				Framework.ActorSystem.Runtime.ActorManager pModulePointer = pAgentTree.GetModule<Framework.ActorSystem.Runtime.ActorManager>();
				if(pModulePointer == null) return true;
				return AT_GetRandom_1(pModulePointer,pAgentTree.GetInportInt(pNode,1),pAgentTree.GetInportInt(pNode,2), pAgentTree, pNode);
			}
			case 977072972://CreateActor
			{
				if(pNode.GetInportCount() <= 3) return true;
				Framework.ActorSystem.Runtime.ActorManager pModulePointer = pAgentTree.GetModule<Framework.ActorSystem.Runtime.ActorManager>();
				if(pModulePointer == null) return true;
				return AT_CreateActor(pModulePointer,pAgentTree.GetInportUserData<Framework.ActorSystem.Runtime.IContextData>(pNode,1),pAgentTree.GetInportUserData<Framework.ActorSystem.Runtime.IContextData>(pNode,2),pAgentTree.GetInportInt(pNode,3), pAgentTree, pNode);
			}
			case 1895192195://AsyncCreateActor
			{
				if(pNode.GetInportCount() <= 3) return true;
				Framework.ActorSystem.Runtime.ActorManager pModulePointer = pAgentTree.GetModule<Framework.ActorSystem.Runtime.ActorManager>();
				if(pModulePointer == null) return true;
				return AT_AsyncCreateActor(pModulePointer,pAgentTree.GetInportInt(pNode,1),pAgentTree.GetInportUserData<Framework.ActorSystem.Runtime.IContextData>(pNode,2),pAgentTree.GetInportUserData<Framework.ActorSystem.Runtime.IContextData>(pNode,3), pAgentTree, pNode);
			}
			case -1912114985://GetActor
			{
				if(pNode.GetInportCount() <= 1) return true;
				Framework.ActorSystem.Runtime.ActorManager pModulePointer = pAgentTree.GetModule<Framework.ActorSystem.Runtime.ActorManager>();
				if(pModulePointer == null) return true;
				return AT_GetActor(pModulePointer,pAgentTree.GetInportInt(pNode,1), pAgentTree, pNode);
			}
			case -66015392://StopProjectileByOwner
			{
				if(pNode.GetInportCount() <= 2) return true;
				Framework.ActorSystem.Runtime.ActorManager pModulePointer = pAgentTree.GetModule<Framework.ActorSystem.Runtime.ActorManager>();
				if(pModulePointer == null) return true;
				return AT_StopProjectileByOwner(pModulePointer,pAgentTree.GetInportUserData<Framework.ActorSystem.Runtime.Actor>(pNode,1),pAgentTree.GetInportFloat(pNode,2));
			}
			case 362626841://StopAllProjectiles
			{
				if(pNode.GetInportCount() <= 0) return true;
				Framework.ActorSystem.Runtime.ActorManager pModulePointer = pAgentTree.GetModule<Framework.ActorSystem.Runtime.ActorManager>();
				if(pModulePointer == null) return true;
				return AT_StopAllProjectiles(pModulePointer);
			}
			case -533418135://Clear
			{
				if(pNode.GetInportCount() <= 0) return true;
				Framework.ActorSystem.Runtime.ActorManager pModulePointer = pAgentTree.GetModule<Framework.ActorSystem.Runtime.ActorManager>();
				if(pModulePointer == null) return true;
				return AT_Clear(pModulePointer);
			}
			}
			return true;
		}
	}
}

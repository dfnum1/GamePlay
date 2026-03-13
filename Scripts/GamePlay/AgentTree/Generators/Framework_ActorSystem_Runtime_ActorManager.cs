//auto generated
using Framework.AT.Runtime;
namespace Framework.ActorSystem.Runtime
{
#if UNITY_EDITOR
	[ATClass(typeof(Framework.ActorSystem.Runtime.ActorManager))]
#endif
	public class Framework_ActorSystem_Runtime_ActorManager
	{
#if UNITY_EDITOR
		[ATFunction(-1714752779,"设置空间大小",typeof(Framework.ActorSystem.Runtime.ActorManager),false)]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableBounds),"worldBounds",false, null,typeof(ExternEngine.FBounds))]
#endif
		static bool AT_InitializeSpatialIndex(ActorManager pPointerThis,ExternEngine.FBounds worldBounds)
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
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableUserData),"pData",false, null,typeof(Framework.ActorSystem.Runtime.IActorContextData))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableUserData),"userVariable",false, null,typeof(Framework.Base.IVarData))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableInt),"actorId",false, null,typeof(System.Int32))]
		[ATFunctionReturn(typeof(Framework.AT.Runtime.VariableUserData), "pReturn", null,typeof(Framework.ActorSystem.Runtime.Actor))]
#endif
		static bool AT_CreateActor(ActorManager pPointerThis,Framework.ActorSystem.Runtime.IActorContextData pData,Framework.Base.IVarData userVariable,System.Int32 actorId,AgentTree pAgentTree, BaseNode pNode)
		{
			pAgentTree.SetOutportUserData(pNode, 0, pPointerThis.CreateActor(pData,userVariable,actorId));
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(1895192195,"异步创建Actor",typeof(Framework.ActorSystem.Runtime.ActorManager),false)]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableUserData),"pData",false, null,typeof(Framework.ActorSystem.Runtime.IActorContextData))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableUserData),"userVariable",false, null,typeof(Framework.Base.IVarData))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableInt),"actorId",false, null,typeof(System.Int32))]
		[ATFunctionReturn(typeof(Framework.AT.Runtime.VariableUserData), "pReturn", null,typeof(Framework.ActorSystem.Runtime.Actor))]
#endif
		static bool AT_AsyncCreateActor(ActorManager pPointerThis,Framework.ActorSystem.Runtime.IActorContextData pData,Framework.Base.IVarData userVariable,System.Int32 actorId,AgentTree pAgentTree, BaseNode pNode)
		{
			pAgentTree.SetOutportUserData(pNode, 0, pPointerThis.AsyncCreateActor(pData,userVariable,actorId));
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
#if UNITY_EDITOR
		[ATFunction(-2072939453,"统计类型单位数量",typeof(Framework.ActorSystem.Runtime.ActorManager),false)]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableInt),"actorType",false, null,typeof(System.Byte),drawMethod:"ActorTypeDraw")]
		[ATFunctionReturn(typeof(Framework.AT.Runtime.VariableInt), "pReturn", null,typeof(System.Int32))]
#endif
		static bool AT_StatisticsActorCount(ActorManager pPointerThis,System.Byte actorType,AgentTree pAgentTree, BaseNode pNode)
		{
			pAgentTree.SetOutportInt(pNode, 0, pPointerThis.StatisticsActorCount(actorType));
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(151431866,"统计阵营组单位数量",typeof(Framework.ActorSystem.Runtime.ActorManager),false)]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableInt),"attackGroup",false, null,typeof(System.Byte),drawMethod:"AttackGroupDraw")]
		[ATFunctionReturn(typeof(Framework.AT.Runtime.VariableInt), "pReturn", null,typeof(System.Int32))]
#endif
		static bool AT_StatisticsAttackGroupActorCount(ActorManager pPointerThis,System.Byte attackGroup,AgentTree pAgentTree, BaseNode pNode)
		{
			pAgentTree.SetOutportInt(pNode, 0, pPointerThis.StatisticsAttackGroupActorCount(attackGroup));
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(-1592230196,"统计类型和阵营组单位数量",typeof(Framework.ActorSystem.Runtime.ActorManager),false)]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableInt),"actorType",false, null,typeof(System.Byte),drawMethod:"ActorTypeDraw")]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableInt),"attackGroup",false, null,typeof(System.Byte),drawMethod:"AttackGroupDraw")]
		[ATFunctionReturn(typeof(Framework.AT.Runtime.VariableInt), "pReturn", null,typeof(System.Int32))]
#endif
		static bool AT_StatisticsActorCount_1(ActorManager pPointerThis,System.Byte actorType,System.Byte attackGroup,AgentTree pAgentTree, BaseNode pNode)
		{
			pAgentTree.SetOutportInt(pNode, 0, pPointerThis.StatisticsActorCount(actorType,attackGroup));
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(940650358,"统计类型_子类型_阵营组单位数量",typeof(Framework.ActorSystem.Runtime.ActorManager),false)]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableInt),"actorType",false, null,typeof(System.Byte),drawMethod:"ActorTypeDraw")]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableInt),"subType",false, null,typeof(System.Byte),drawMethod:"ActorSubTypeDraw",byPortArgv:"actorType")]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableInt),"attackGroup",false, null,typeof(System.Byte),drawMethod:"AttackGroupDraw")]
		[ATFunctionReturn(typeof(Framework.AT.Runtime.VariableInt), "pReturn", null,typeof(System.Int32))]
#endif
		static bool AT_StatisticsActorCount_2(ActorManager pPointerThis,System.Byte actorType,System.Byte subType,System.Byte attackGroup,AgentTree pAgentTree, BaseNode pNode)
		{
			pAgentTree.SetOutportInt(pNode, 0, pPointerThis.StatisticsActorCount(actorType,subType,attackGroup));
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(-40120954,"显示隐藏类型_阵营组单位数量",typeof(Framework.ActorSystem.Runtime.ActorManager),false)]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableBool),"bShow",false, null,typeof(System.Boolean))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableInt),"actorType",false, null,typeof(System.Byte),drawMethod:"ActorTypeDraw")]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableInt),"attackGroup",false, null,typeof(System.Byte),drawMethod:"AttackGroupDraw")]
		[ATFunctionReturn(typeof(Framework.AT.Runtime.VariableInt), "pReturn", null,typeof(System.Int32))]
#endif
		static bool AT_ShowActors(ActorManager pPointerThis,System.Boolean bShow,System.Byte actorType,System.Byte attackGroup,AgentTree pAgentTree, BaseNode pNode)
		{
			pAgentTree.SetOutportInt(pNode, 0, pPointerThis.ShowActors(bShow,actorType,attackGroup));
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(-1524093179,"显示隐藏类型_子类型_阵营组单位数量",typeof(Framework.ActorSystem.Runtime.ActorManager),false)]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableBool),"bShow",false, null,typeof(System.Boolean))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableInt),"actorType",false, null,typeof(System.Byte),drawMethod:"ActorTypeDraw")]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableInt),"subType",false, null,typeof(System.Byte),drawMethod:"ActorSubTypeDraw",byPortArgv:"actorType")]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableInt),"attackGroup",false, null,typeof(System.Byte),drawMethod:"AttackGroupDraw")]
		[ATFunctionReturn(typeof(Framework.AT.Runtime.VariableInt), "pReturn", null,typeof(System.Int32))]
#endif
		static bool AT_ShowActors_1(ActorManager pPointerThis,System.Boolean bShow,System.Byte actorType,System.Byte subType,System.Byte attackGroup,AgentTree pAgentTree, BaseNode pNode)
		{
			pAgentTree.SetOutportInt(pNode, 0, pPointerThis.ShowActors(bShow,actorType,subType,attackGroup));
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
				return AT_CreateActor(pModulePointer,pAgentTree.GetInportUserData<Framework.ActorSystem.Runtime.IActorContextData>(pNode,1),pAgentTree.GetInportUserData<Framework.Base.IVarData>(pNode,2),pAgentTree.GetInportInt(pNode,3), pAgentTree, pNode);
			}
			case 1895192195://AsyncCreateActor
			{
				if(pNode.GetInportCount() <= 3) return true;
				Framework.ActorSystem.Runtime.ActorManager pModulePointer = pAgentTree.GetModule<Framework.ActorSystem.Runtime.ActorManager>();
				if(pModulePointer == null) return true;
				return AT_AsyncCreateActor(pModulePointer,pAgentTree.GetInportUserData<Framework.ActorSystem.Runtime.IActorContextData>(pNode,1),pAgentTree.GetInportUserData<Framework.Base.IVarData>(pNode,2),pAgentTree.GetInportInt(pNode,3), pAgentTree, pNode);
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
			case -2072939453://StatisticsActorCount
			{
				if(pNode.GetInportCount() <= 1) return true;
				Framework.ActorSystem.Runtime.ActorManager pModulePointer = pAgentTree.GetModule<Framework.ActorSystem.Runtime.ActorManager>();
				if(pModulePointer == null) return true;
				return AT_StatisticsActorCount(pModulePointer,pAgentTree.GetInportByte(pNode,1), pAgentTree, pNode);
			}
			case 151431866://StatisticsAttackGroupActorCount
			{
				if(pNode.GetInportCount() <= 1) return true;
				Framework.ActorSystem.Runtime.ActorManager pModulePointer = pAgentTree.GetModule<Framework.ActorSystem.Runtime.ActorManager>();
				if(pModulePointer == null) return true;
				return AT_StatisticsAttackGroupActorCount(pModulePointer,pAgentTree.GetInportByte(pNode,1), pAgentTree, pNode);
			}
			case -1592230196://StatisticsActorCount
			{
				if(pNode.GetInportCount() <= 2) return true;
				Framework.ActorSystem.Runtime.ActorManager pModulePointer = pAgentTree.GetModule<Framework.ActorSystem.Runtime.ActorManager>();
				if(pModulePointer == null) return true;
				return AT_StatisticsActorCount_1(pModulePointer,pAgentTree.GetInportByte(pNode,1),pAgentTree.GetInportByte(pNode,2), pAgentTree, pNode);
			}
			case 940650358://StatisticsActorCount
			{
				if(pNode.GetInportCount() <= 3) return true;
				Framework.ActorSystem.Runtime.ActorManager pModulePointer = pAgentTree.GetModule<Framework.ActorSystem.Runtime.ActorManager>();
				if(pModulePointer == null) return true;
				return AT_StatisticsActorCount_2(pModulePointer,pAgentTree.GetInportByte(pNode,1),pAgentTree.GetInportByte(pNode,2),pAgentTree.GetInportByte(pNode,3), pAgentTree, pNode);
			}
			case -40120954://ShowActors
			{
				if(pNode.GetInportCount() <= 3) return true;
				Framework.ActorSystem.Runtime.ActorManager pModulePointer = pAgentTree.GetModule<Framework.ActorSystem.Runtime.ActorManager>();
				if(pModulePointer == null) return true;
				return AT_ShowActors(pModulePointer,pAgentTree.GetInportBool(pNode,1),pAgentTree.GetInportByte(pNode,2),pAgentTree.GetInportByte(pNode,3), pAgentTree, pNode);
			}
			case -1524093179://ShowActors
			{
				if(pNode.GetInportCount() <= 4) return true;
				Framework.ActorSystem.Runtime.ActorManager pModulePointer = pAgentTree.GetModule<Framework.ActorSystem.Runtime.ActorManager>();
				if(pModulePointer == null) return true;
				return AT_ShowActors_1(pModulePointer,pAgentTree.GetInportBool(pNode,1),pAgentTree.GetInportByte(pNode,2),pAgentTree.GetInportByte(pNode,3),pAgentTree.GetInportByte(pNode,4), pAgentTree, pNode);
			}
			}
			return true;
		}
	}
}

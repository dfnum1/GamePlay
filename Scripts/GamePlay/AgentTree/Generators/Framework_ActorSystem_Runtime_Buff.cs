//auto generated
using Framework.AT.Runtime;
namespace Framework.ActorSystem.Runtime
{
#if UNITY_EDITOR
	[ATClass(typeof(Framework.ActorSystem.Runtime.Buff),"Actor系统/Buff")]
#endif
	public class Framework_ActorSystem_Runtime_Buff
	{
#if UNITY_EDITOR
		[ATFunction(-1315645697,"是有拥有Buff状态",typeof(Framework.ActorSystem.Runtime.Buff),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Buff",false, null,typeof(Framework.ActorSystem.Runtime.Buff))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableInt),"effectState",false, null,typeof(System.UInt32),drawMethod:"BuffStateDraw")]
		[ATFunctionReturn(typeof(Framework.AT.Runtime.VariableBool), "pReturn", null,typeof(System.Boolean))]
#endif
		static bool AT_HasEffectState(Buff pPointerThis,System.UInt32 effectState,AgentTree pAgentTree, BaseNode pNode)
		{
			pAgentTree.SetOutportBool(pNode, 0, pPointerThis.HasEffectState(effectState));
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(2054004447,"添加Buff状态",typeof(Framework.ActorSystem.Runtime.Buff),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Buff",false, null,typeof(Framework.ActorSystem.Runtime.Buff))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableInt),"effectState",false, null,typeof(System.UInt32),drawMethod:"BuffStateDraw")]
#endif
		static bool AT_AddEffectState(Buff pPointerThis,System.UInt32 effectState)
		{
			pPointerThis.AddEffectState(effectState);
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(1654142733,"移除Buff状态",typeof(Framework.ActorSystem.Runtime.Buff),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Buff",false, null,typeof(Framework.ActorSystem.Runtime.Buff))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableInt),"effectState",false, null,typeof(System.UInt32),drawMethod:"BuffStateDraw")]
#endif
		static bool AT_RemoveEffectState(Buff pPointerThis,System.UInt32 effectState)
		{
			pPointerThis.RemoveEffectState(effectState);
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(-1870392312,"是否结束",typeof(Framework.ActorSystem.Runtime.Buff),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Buff",false, null,typeof(Framework.ActorSystem.Runtime.Buff))]
		[ATFunctionReturn(typeof(Framework.AT.Runtime.VariableBool), "pReturn", null,typeof(System.Boolean))]
#endif
		static bool AT_IsEnd(Buff pPointerThis,AgentTree pAgentTree, BaseNode pNode)
		{
			pAgentTree.SetOutportBool(pNode, 0, pPointerThis.IsEnd());
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(1007224019,"是否激活",typeof(Framework.ActorSystem.Runtime.Buff),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Buff",false, null,typeof(Framework.ActorSystem.Runtime.Buff))]
		[ATFunctionReturn(typeof(Framework.AT.Runtime.VariableBool), "pReturn", null,typeof(System.Boolean))]
#endif
		static bool AT_IsActived(Buff pPointerThis,AgentTree pAgentTree, BaseNode pNode)
		{
			pAgentTree.SetOutportBool(pNode, 0, pPointerThis.IsActived());
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(1108070059,"设置激活",typeof(Framework.ActorSystem.Runtime.Buff),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Buff",false, null,typeof(Framework.ActorSystem.Runtime.Buff))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableBool),"bActive",false, null,typeof(System.Boolean))]
#endif
		static bool AT_SetActived(Buff pPointerThis,System.Boolean bActive)
		{
			pPointerThis.SetActived(bActive);
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(-631709943,"设置等级",typeof(Framework.ActorSystem.Runtime.Buff),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Buff",false, null,typeof(Framework.ActorSystem.Runtime.Buff))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableInt),"nLevel",false, null,typeof(System.UInt32))]
#endif
		static bool AT_SetLevel(Buff pPointerThis,System.UInt32 nLevel)
		{
			pPointerThis.SetLevel(nLevel);
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(581558488,"获取等级",typeof(Framework.ActorSystem.Runtime.Buff),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Buff",false, null,typeof(Framework.ActorSystem.Runtime.Buff))]
		[ATFunctionReturn(typeof(Framework.AT.Runtime.VariableInt), "pReturn", null,typeof(System.UInt32))]
#endif
		static bool AT_GetLevel(Buff pPointerThis,AgentTree pAgentTree, BaseNode pNode)
		{
			pAgentTree.SetOutportUint(pNode, 0, pPointerThis.GetLevel());
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(-1853761918,"获取Tick次数",typeof(Framework.ActorSystem.Runtime.Buff),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Buff",false, null,typeof(Framework.ActorSystem.Runtime.Buff))]
		[ATFunctionReturn(typeof(Framework.AT.Runtime.VariableInt), "pReturn", null,typeof(System.UInt32))]
#endif
		static bool AT_GetTickCount(Buff pPointerThis,AgentTree pAgentTree, BaseNode pNode)
		{
			pAgentTree.SetOutportInt(pNode, 0, pPointerThis.GetTickCount());
			return true;
		}

		static bool CheckUserClassPointer(ref VariableUserData pUserClass, AgentTree pAgentTree, BaseNode pNode)
		{
			if(pUserClass.pPointer == null) pUserClass.pPointer = pAgentTree.GetOwnerClass(pUserClass.value);
			if(pUserClass.pPointer == null) return false;
			return true;
		}
		public static bool DoAction(VariableUserData pUserClass, AgentTree pAgentTree, BaseNode pNode)
		{
			int actionType = pNode.type;
			switch(actionType)
			{
			case -1315645697://HasEffectState
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 1) return true;
				if(!(pUserClass.pPointer is Buff)) return true;
				return AT_HasEffectState((Buff)pUserClass.pPointer,pAgentTree.GetInportUint(pNode,1), pAgentTree, pNode);
			}
			case 2054004447://AddEffectState
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 1) return true;
				if(!(pUserClass.pPointer is Buff)) return true;
				return AT_AddEffectState((Buff)pUserClass.pPointer,pAgentTree.GetInportUint(pNode,1));
			}
			case 1654142733://RemoveEffectState
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 1) return true;
				if(!(pUserClass.pPointer is Buff)) return true;
				return AT_RemoveEffectState((Buff)pUserClass.pPointer,pAgentTree.GetInportUint(pNode,1));
			}
			case -1870392312://IsEnd
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 0) return true;
				if(!(pUserClass.pPointer is Buff)) return true;
				return AT_IsEnd((Buff)pUserClass.pPointer,pAgentTree, pNode);
			}
			case 1007224019://IsActived
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 0) return true;
				if(!(pUserClass.pPointer is Buff)) return true;
				return AT_IsActived((Buff)pUserClass.pPointer,pAgentTree, pNode);
			}
			case 1108070059://SetActived
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 1) return true;
				if(!(pUserClass.pPointer is Buff)) return true;
				return AT_SetActived((Buff)pUserClass.pPointer,pAgentTree.GetInportBool(pNode,1));
			}
			case -631709943://SetLevel
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 1) return true;
				if(!(pUserClass.pPointer is Buff)) return true;
				return AT_SetLevel((Buff)pUserClass.pPointer,pAgentTree.GetInportUint(pNode,1));
			}
			case 581558488://GetLevel
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 0) return true;
				if(!(pUserClass.pPointer is Buff)) return true;
				return AT_GetLevel((Buff)pUserClass.pPointer,pAgentTree, pNode);
			}
			case -1853761918://GetTickCount
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 0) return true;
				if(!(pUserClass.pPointer is Buff)) return true;
				return AT_GetTickCount((Buff)pUserClass.pPointer,pAgentTree, pNode);
			}
			}
			return true;
		}
	}
}

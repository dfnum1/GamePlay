//auto generated
using Framework.AT.Runtime;
#if USE_FIXEDMATH
using ExternEngine;
#else
using FFloat=UnityEngine.Float;

#endif
namespace Framework.ActorSystem.Runtime
{
#if UNITY_EDITOR
	[ATClass(typeof(Framework.ActorSystem.Runtime.BuffSystem),"Actor系统/Buff系统")]
#endif
	public class Framework_ActorSystem_Runtime_BuffSystem
	{
#if UNITY_EDITOR
		[ATFunction(-50961863,"获或当前Actor",typeof(Framework.ActorSystem.Runtime.BuffSystem),false)]
		[ATFunctionArgv(typeof(VariableUserData),"BuffSystem",false, null,typeof(Framework.ActorSystem.Runtime.BuffSystem))]
		[ATFunctionReturn(typeof(Framework.AT.Runtime.VariableUserData), "pReturn", null,typeof(Framework.ActorSystem.Runtime.Actor))]
#endif
		static bool AT_GetActor(BuffSystem pPointerThis,AgentTree pAgentTree, BaseNode pNode)
		{
			pAgentTree.SetOutportUserData(pNode, 0, pPointerThis.GetActor());
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(-2082800567,"添加Buff",typeof(Framework.ActorSystem.Runtime.BuffSystem),false)]
		[ATFunctionArgv(typeof(VariableUserData),"BuffSystem",false, null,typeof(Framework.ActorSystem.Runtime.BuffSystem))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableUserData),"pBuff",false, null,typeof(Framework.ActorSystem.Runtime.Buff))]
#endif
		static bool AT_AddBuff(BuffSystem pPointerThis,Framework.ActorSystem.Runtime.Buff pBuff)
		{
			pPointerThis.AddBuff(pBuff);
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(-2136425852,"获取Buff属性值",typeof(Framework.ActorSystem.Runtime.BuffSystem),false)]
		[ATFunctionArgv(typeof(VariableUserData),"BuffSystem",false, null,typeof(Framework.ActorSystem.Runtime.BuffSystem))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableInt),"type",false, null,typeof(System.Byte),drawMethod:"DrawAttributePop")]
		[ATFunctionReturn(typeof(Framework.AT.Runtime.VariableFloat), "pReturn", null,typeof(ExternEngine.FFloat))]
#endif
		static bool AT_GetAttrValue(BuffSystem pPointerThis,System.Byte type,AgentTree pAgentTree, BaseNode pNode)
		{
			pAgentTree.SetOutportFloat(pNode, 0, pPointerThis.GetAttrValue(type));
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(-763813700,"获取Buff属性率",typeof(Framework.ActorSystem.Runtime.BuffSystem),false)]
		[ATFunctionArgv(typeof(VariableUserData),"BuffSystem",false, null,typeof(Framework.ActorSystem.Runtime.BuffSystem))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableInt),"type",false, null,typeof(System.Byte),drawMethod:"DrawAttributePop")]
		[ATFunctionReturn(typeof(Framework.AT.Runtime.VariableFloat), "pReturn", null,typeof(ExternEngine.FFloat))]
#endif
		static bool AT_GetAttrRate(BuffSystem pPointerThis,System.Byte type,AgentTree pAgentTree, BaseNode pNode)
		{
			pAgentTree.SetOutportFloat(pNode, 0, pPointerThis.GetAttrRate(type));
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
			case -50961863://GetActor
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 0) return true;
				if(!(pUserClass.pPointer is BuffSystem)) return true;
				return AT_GetActor((BuffSystem)pUserClass.pPointer,pAgentTree, pNode);
			}
			case -2082800567://AddBuff
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 1) return true;
				if(!(pUserClass.pPointer is BuffSystem)) return true;
				return AT_AddBuff((BuffSystem)pUserClass.pPointer,pAgentTree.GetInportUserData<Framework.ActorSystem.Runtime.Buff>(pNode,1));
			}
			case -2136425852://GetAttrValue
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 1) return true;
				if(!(pUserClass.pPointer is BuffSystem)) return true;
				return AT_GetAttrValue((BuffSystem)pUserClass.pPointer,pAgentTree.GetInportByte(pNode,1), pAgentTree, pNode);
			}
			case -763813700://GetAttrRate
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 1) return true;
				if(!(pUserClass.pPointer is BuffSystem)) return true;
				return AT_GetAttrRate((BuffSystem)pUserClass.pPointer,pAgentTree.GetInportByte(pNode,1), pAgentTree, pNode);
			}
			}
			return true;
		}
	}
}

//auto generated
using Framework.AT.Runtime;
namespace Framework.ActorSystem.Runtime
{
#if UNITY_EDITOR
	[ATClass(typeof(Framework.ActorSystem.Runtime.AActorStateInfo))]
#endif
	public class Framework_ActorSystem_Runtime_AActorStateInfo
	{
#if UNITY_EDITOR
		[ATFunction(-1780442991,"获取属性计算公式",typeof(Framework.ActorSystem.Runtime.AActorStateInfo),false)]
		[ATFunctionArgv(typeof(VariableUserData),"AActorStateInfo",false, null,typeof(Framework.ActorSystem.Runtime.AActorStateInfo))]
		[ATFunctionReturn(typeof(Framework.AT.Runtime.VariableInt), "pReturn", null,typeof(System.Int32))]
#endif
		static bool AT_GetAttrFormulaType(AActorStateInfo pPointerThis,AgentTree pAgentTree, BaseNode pNode)
		{
			pAgentTree.SetOutportInt(pNode, 0, pPointerThis.GetAttrFormulaType());
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(-635668939,"添加索敌目标",typeof(Framework.ActorSystem.Runtime.AActorStateInfo),false)]
		[ATFunctionArgv(typeof(VariableUserData),"AActorStateInfo",false, null,typeof(Framework.ActorSystem.Runtime.AActorStateInfo))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableUserData),"pNode",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableBool),"bClear",false, null,typeof(System.Boolean))]
#endif
		static bool AT_AddLockTarget(AActorStateInfo pPointerThis,Framework.ActorSystem.Runtime.Actor pNode,System.Boolean bClear)
		{
			pPointerThis.AddLockTarget(pNode,bClear);
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(-1695780923,"清空索敌目标",typeof(Framework.ActorSystem.Runtime.AActorStateInfo),false)]
		[ATFunctionArgv(typeof(VariableUserData),"AActorStateInfo",false, null,typeof(Framework.ActorSystem.Runtime.AActorStateInfo))]
#endif
		static bool AT_ClearLockTargets(AActorStateInfo pPointerThis)
		{
			pPointerThis.ClearLockTargets();
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
			case -1780442991://GetAttrFormulaType
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 0) return true;
				if(!(pUserClass.pPointer is Framework.ActorSystem.Runtime.AActorStateInfo)) return true;
				return AT_GetAttrFormulaType((Framework.ActorSystem.Runtime.AActorStateInfo)pUserClass.pPointer,pAgentTree, pNode);
			}
			case -635668939://AddLockTarget
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 2) return true;
				if(!(pUserClass.pPointer is Framework.ActorSystem.Runtime.AActorStateInfo)) return true;
				return AT_AddLockTarget((Framework.ActorSystem.Runtime.AActorStateInfo)pUserClass.pPointer,pAgentTree.GetInportUserData<Framework.ActorSystem.Runtime.Actor>(pNode,1),pAgentTree.GetInportBool(pNode,2));
			}
			case -1695780923://ClearLockTargets
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 0) return true;
				if(!(pUserClass.pPointer is Framework.ActorSystem.Runtime.AActorStateInfo)) return true;
				return AT_ClearLockTargets((Framework.ActorSystem.Runtime.AActorStateInfo)pUserClass.pPointer);
			}
			}
			return true;
		}
	}
}

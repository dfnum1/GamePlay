//auto generated
using Framework.AT.Runtime;
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
		[ATFunction(1944429814,"清除索敌单位",typeof(Framework.ActorSystem.Runtime.BuffSystem),false)]
		[ATFunctionArgv(typeof(VariableUserData),"BuffSystem",false, null,typeof(Framework.ActorSystem.Runtime.BuffSystem))]
#endif
		static bool AT_ClearLockTargets(BuffSystem pPointerThis)
		{
			pPointerThis.ClearLockTargets();
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
			case 1944429814://ClearLockTargets
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 0) return true;
				if(!(pUserClass.pPointer is BuffSystem)) return true;
				return AT_ClearLockTargets((BuffSystem)pUserClass.pPointer);
			}
			case -2082800567://AddBuff
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 1) return true;
				if(!(pUserClass.pPointer is BuffSystem)) return true;
				return AT_AddBuff((BuffSystem)pUserClass.pPointer,pAgentTree.GetInportUserData<Framework.ActorSystem.Runtime.Buff>(pNode,1));
			}
			}
			return true;
		}
	}
}

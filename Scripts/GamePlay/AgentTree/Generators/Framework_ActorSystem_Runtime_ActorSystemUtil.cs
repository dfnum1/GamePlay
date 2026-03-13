//auto generated
using Framework.AT.Runtime;
namespace Framework.ActorSystem.Runtime
{
#if UNITY_EDITOR
	[ATClass(typeof(Framework.ActorSystem.Runtime.ActorSystemUtil))]
#endif
	public class Framework_ActorSystem_Runtime_ActorSystemUtil
	{
#if UNITY_EDITOR
		[ATFunction(1433183848,"是否拥有Buff状态",typeof(Framework.ActorSystem.Runtime.ActorSystemUtil),false)]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableInt),"flags",false, null,typeof(System.Int32),drawMethod:"BuffStateDraw")]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableInt),"buffState",false, null,typeof(System.Int32),drawMethod:"BuffStateDraw")]
		[ATFunctionReturn(typeof(Framework.AT.Runtime.VariableBool), "pReturn", null,typeof(System.Boolean))]
#endif
		static bool AT_HasBuffState(System.Int32 flags,System.Int32 buffState,AgentTree pAgentTree, BaseNode pNode)
		{
			pAgentTree.SetOutportBool(pNode, 0, Framework.ActorSystem.Runtime.ActorSystemUtil.HasBuffState(flags,buffState));
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
			case 1433183848://HasBuffState
			{
				if(pNode.GetInportCount() <= 2) return true;
				return AT_HasBuffState(pAgentTree.GetInportInt(pNode,1),pAgentTree.GetInportInt(pNode,2), pAgentTree, pNode);
			}
			}
			return true;
		}
	}
}

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
		[ATFunction(-631709943,"设置等级",typeof(Framework.ActorSystem.Runtime.Buff),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Buff",false, null,typeof(Framework.ActorSystem.Runtime.Buff))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableInt),"nLevel",false, null,typeof(System.UInt32))]
#endif
		static bool AT_SetLevel(Buff pPointerThis,System.UInt32 nLevel)
		{
			pPointerThis.SetLevel(nLevel);
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
			case -631709943://SetLevel
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 1) return true;
				if(!(pUserClass.pPointer is Buff)) return true;
				return AT_SetLevel((Buff)pUserClass.pPointer,pAgentTree.GetInportUint(pNode,1));
			}
			}
			return true;
		}
	}
}

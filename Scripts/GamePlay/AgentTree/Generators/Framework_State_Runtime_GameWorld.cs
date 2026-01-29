//auto generated
using Framework.AT.Runtime;
namespace Framework.State.Runtime
{
#if UNITY_EDITOR
	[ATClass(typeof(Framework.State.Runtime.GameWorld),"游戏世界")]
#endif
	public class Framework_State_Runtime_GameWorld
	{
#if UNITY_EDITOR
		[ATFunction(-2013982276,"唤醒游戏状态",typeof(Framework.State.Runtime.GameWorld),false)]
#endif
		static bool AT_AwakeState(GameWorld pPointerThis)
		{
			pPointerThis.AwakeState();
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(141353757,"预备游戏状态",typeof(Framework.State.Runtime.GameWorld),false)]
#endif
		static bool AT_PreStartState(GameWorld pPointerThis)
		{
			pPointerThis.PreStartState();
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(-1477479281,"开始游戏状态",typeof(Framework.State.Runtime.GameWorld),false)]
#endif
		static bool AT_StartState(GameWorld pPointerThis)
		{
			pPointerThis.StartState();
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(-1193865250,"激活游戏状态",typeof(Framework.State.Runtime.GameWorld),false)]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableBool),"bActive",false, null,typeof(System.Boolean))]
#endif
		static bool AT_ActiveState(GameWorld pPointerThis,System.Boolean bActive)
		{
			pPointerThis.ActiveState(bActive);
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(177782018,"清理游戏世界",typeof(Framework.State.Runtime.GameWorld),false)]
#endif
		static bool AT_ClearWorld(GameWorld pPointerThis)
		{
			pPointerThis.ClearWorld();
			return true;
		}

		public static bool DoAction(VariableUserData pUserClass, AgentTree pAgentTree, BaseNode pNode)
		{
			int actionType = pNode.type;
			switch(actionType)
			{
			case -2013982276://AwakeState
			{
				if(pNode.GetInportCount() <= 0) return true;
				Framework.State.Runtime.GameWorld pModulePointer = pAgentTree.GetModule<Framework.State.Runtime.GameWorld>();
				if(pModulePointer == null) return true;
				return AT_AwakeState(pModulePointer);
			}
			case 141353757://PreStartState
			{
				if(pNode.GetInportCount() <= 0) return true;
				Framework.State.Runtime.GameWorld pModulePointer = pAgentTree.GetModule<Framework.State.Runtime.GameWorld>();
				if(pModulePointer == null) return true;
				return AT_PreStartState(pModulePointer);
			}
			case -1477479281://StartState
			{
				if(pNode.GetInportCount() <= 0) return true;
				Framework.State.Runtime.GameWorld pModulePointer = pAgentTree.GetModule<Framework.State.Runtime.GameWorld>();
				if(pModulePointer == null) return true;
				return AT_StartState(pModulePointer);
			}
			case -1193865250://ActiveState
			{
				if(pNode.GetInportCount() <= 1) return true;
				Framework.State.Runtime.GameWorld pModulePointer = pAgentTree.GetModule<Framework.State.Runtime.GameWorld>();
				if(pModulePointer == null) return true;
				return AT_ActiveState(pModulePointer,pAgentTree.GetInportBool(pNode,1));
			}
			case 177782018://ClearWorld
			{
				if(pNode.GetInportCount() <= 0) return true;
				Framework.State.Runtime.GameWorld pModulePointer = pAgentTree.GetModule<Framework.State.Runtime.GameWorld>();
				if(pModulePointer == null) return true;
				return AT_ClearWorld(pModulePointer);
			}
			}
			return true;
		}
	}
}

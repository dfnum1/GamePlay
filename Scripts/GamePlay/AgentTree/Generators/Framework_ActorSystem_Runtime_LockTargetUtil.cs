//auto generated
using Framework.AT.Runtime;
namespace Framework.ActorSystem.Runtime
{
#if UNITY_EDITOR
	[ATClass(typeof(Framework.ActorSystem.Runtime.LockTargetUtil))]
#endif
	public class Framework_ActorSystem_Runtime_LockTargetUtil
	{
#if UNITY_EDITOR
		[ATFunction(-708243251,"根据Actor类型索敌",typeof(Framework.ActorSystem.Runtime.LockTargetUtil),false)]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableUserData),"pActorState",false, null,typeof(Framework.ActorSystem.Runtime.AActorStateInfo))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableInt),"actorType",false, null,typeof(System.Byte),drawMethod:"ActorTypeDraw")]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableBool),"bClear",false, null,typeof(System.Boolean))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableBool),"bFriend",false, null,typeof(System.Boolean))]
#endif
		static bool AT_LockAttackTarget(Framework.ActorSystem.Runtime.AActorStateInfo pActorState,System.Byte actorType,System.Boolean bClear,System.Boolean bFriend)
		{
			Framework.ActorSystem.Runtime.LockTargetUtil.LockAttackTarget(pActorState,actorType,bClear,bFriend);
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(1321680880,"根据Actor类型-子类型索敌",typeof(Framework.ActorSystem.Runtime.LockTargetUtil),false)]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableUserData),"pActorState",false, null,typeof(Framework.ActorSystem.Runtime.AActorStateInfo))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableInt),"actorType",false, null,typeof(System.Byte),drawMethod:"ActorTypeDraw")]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableInt),"subType",false, null,typeof(System.Byte),drawMethod:"ActorSubTypeDraw")]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableBool),"bClear",false, null,typeof(System.Boolean))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableBool),"bFriend",false, null,typeof(System.Boolean))]
#endif
		static bool AT_LockAttackTarget_1(Framework.ActorSystem.Runtime.AActorStateInfo pActorState,System.Byte actorType,System.Byte subType,System.Boolean bClear,System.Boolean bFriend)
		{
			Framework.ActorSystem.Runtime.LockTargetUtil.LockAttackTarget(pActorState,actorType,subType,bClear,bFriend);
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(1587508542,"根据属性排序",typeof(Framework.ActorSystem.Runtime.LockTargetUtil),false)]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableUserData),"pActorState",false, null,typeof(Framework.ActorSystem.Runtime.AActorStateInfo))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableInt),"attrType",false, null,typeof(System.Byte),drawMethod:"DrawAttributePop")]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableBool),"bUpper",false, null,typeof(System.Boolean))]
#endif
		static bool AT_SortByAttr(Framework.ActorSystem.Runtime.AActorStateInfo pActorState,System.Byte attrType,System.Boolean bUpper)
		{
			Framework.ActorSystem.Runtime.LockTargetUtil.SortByAttr(pActorState,attrType,bUpper);
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(2012741022,"根据属性比排序",typeof(Framework.ActorSystem.Runtime.LockTargetUtil),false)]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableUserData),"pActorState",false, null,typeof(Framework.ActorSystem.Runtime.AActorStateInfo))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableInt),"baseType",false, null,typeof(System.Byte),drawMethod:"DrawAttributePop")]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableInt),"totalType",false, null,typeof(System.Byte),drawMethod:"DrawAttributePop")]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableBool),"bUpper",false, null,typeof(System.Boolean))]
#endif
		static bool AT_SortByDistance(Framework.ActorSystem.Runtime.AActorStateInfo pActorState,System.Byte baseType,System.Byte totalType,System.Boolean bUpper)
		{
			Framework.ActorSystem.Runtime.LockTargetUtil.SortByDistance(pActorState,baseType,totalType,bUpper);
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(32116124,"根据距离排序",typeof(Framework.ActorSystem.Runtime.LockTargetUtil),false)]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableUserData),"pActorState",false, null,typeof(Framework.ActorSystem.Runtime.AActorStateInfo))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableUserData),"pActor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableBool),"bUpper",false, null,typeof(System.Boolean))]
#endif
		static bool AT_SortByDistance_1(Framework.ActorSystem.Runtime.AActorStateInfo pActorState,Framework.ActorSystem.Runtime.Actor pActor,System.Boolean bUpper)
		{
			Framework.ActorSystem.Runtime.LockTargetUtil.SortByDistance(pActorState,pActor,bUpper);
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(-90046574,"保留锁定目标个数",typeof(Framework.ActorSystem.Runtime.LockTargetUtil),false)]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableUserData),"pActorState",false, null,typeof(Framework.ActorSystem.Runtime.AActorStateInfo))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableInt),"nRetainCnt",false, null,typeof(System.Int32))]
#endif
		static bool AT_RetainLockCount(Framework.ActorSystem.Runtime.AActorStateInfo pActorState,System.Int32 nRetainCnt)
		{
			Framework.ActorSystem.Runtime.LockTargetUtil.RetainLockCount(pActorState,nRetainCnt);
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
			case -708243251://LockAttackTarget
			{
				if(pNode.GetInportCount() <= 4) return true;
				return AT_LockAttackTarget(pAgentTree.GetInportUserData<Framework.ActorSystem.Runtime.AActorStateInfo>(pNode,1),pAgentTree.GetInportByte(pNode,2),pAgentTree.GetInportBool(pNode,3),pAgentTree.GetInportBool(pNode,4));
			}
			case 1321680880://LockAttackTarget
			{
				if(pNode.GetInportCount() <= 5) return true;
				return AT_LockAttackTarget_1(pAgentTree.GetInportUserData<Framework.ActorSystem.Runtime.AActorStateInfo>(pNode,1),pAgentTree.GetInportByte(pNode,2),pAgentTree.GetInportByte(pNode,3),pAgentTree.GetInportBool(pNode,4),pAgentTree.GetInportBool(pNode,5));
			}
			case 1587508542://SortByAttr
			{
				if(pNode.GetInportCount() <= 3) return true;
				return AT_SortByAttr(pAgentTree.GetInportUserData<Framework.ActorSystem.Runtime.AActorStateInfo>(pNode,1),pAgentTree.GetInportByte(pNode,2),pAgentTree.GetInportBool(pNode,3));
			}
			case 2012741022://SortByDistance
			{
				if(pNode.GetInportCount() <= 4) return true;
				return AT_SortByDistance(pAgentTree.GetInportUserData<Framework.ActorSystem.Runtime.AActorStateInfo>(pNode,1),pAgentTree.GetInportByte(pNode,2),pAgentTree.GetInportByte(pNode,3),pAgentTree.GetInportBool(pNode,4));
			}
			case 32116124://SortByDistance
			{
				if(pNode.GetInportCount() <= 3) return true;
				return AT_SortByDistance_1(pAgentTree.GetInportUserData<Framework.ActorSystem.Runtime.AActorStateInfo>(pNode,1),pAgentTree.GetInportUserData<Framework.ActorSystem.Runtime.Actor>(pNode,2),pAgentTree.GetInportBool(pNode,3));
			}
			case -90046574://RetainLockCount
			{
				if(pNode.GetInportCount() <= 2) return true;
				return AT_RetainLockCount(pAgentTree.GetInportUserData<Framework.ActorSystem.Runtime.AActorStateInfo>(pNode,1),pAgentTree.GetInportInt(pNode,2));
			}
			}
			return true;
		}
	}
}

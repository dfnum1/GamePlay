//auto generated
using Framework.AT.Runtime;
namespace Framework.ActorSystem.Runtime
{
#if UNITY_EDITOR
	[ATClass(typeof(Framework.ActorSystem.Runtime.Skill),"Actor系统/技能")]
#endif
	public class Framework_ActorSystem_Runtime_Skill
	{
#if UNITY_EDITOR
		[ATFunction(1009185467,"获得当前CD",typeof(Framework.ActorSystem.Runtime.Skill),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Skill",false, null,typeof(Framework.ActorSystem.Runtime.Skill))]
		[ATFunctionReturn(typeof(Framework.AT.Runtime.VariableFloat), "pReturn", null,typeof(System.Single))]
#endif
		static bool AT_GetRuntimeCD(Skill pPointerThis,AgentTree pAgentTree, BaseNode pNode)
		{
			pAgentTree.SetOutportFloat(pNode, 0, pPointerThis.GetRuntimeCD());
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(414892563,"设置等级",typeof(Framework.ActorSystem.Runtime.Skill),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Skill",false, null,typeof(Framework.ActorSystem.Runtime.Skill))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableInt),"nLevel",false, null,typeof(System.UInt32))]
#endif
		static bool AT_SetLevel(Skill pPointerThis,System.UInt32 nLevel)
		{
			pPointerThis.SetLevel(nLevel);
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(1793744141,"是否可触发",typeof(Framework.ActorSystem.Runtime.Skill),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Skill",false, null,typeof(Framework.ActorSystem.Runtime.Skill))]
		[ATFunctionReturn(typeof(Framework.AT.Runtime.VariableBool), "pReturn", null,typeof(System.Boolean))]
#endif
		static bool AT_CanTrigger(Skill pPointerThis,AgentTree pAgentTree, BaseNode pNode)
		{
			pAgentTree.SetOutportBool(pNode, 0, pPointerThis.CanTrigger());
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(-282211430,"添加索敌目标",typeof(Framework.ActorSystem.Runtime.Skill),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Skill",false, null,typeof(Framework.ActorSystem.Runtime.Skill))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableUserData),"pNode",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableBool),"bClear",false, null,typeof(System.Boolean))]
#endif
		static bool AT_AddLockTarget(Skill pPointerThis,Framework.ActorSystem.Runtime.Actor pNode,System.Boolean bClear)
		{
			pPointerThis.AddLockTarget(pNode,bClear);
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(1216193890,"清理索敌目标",typeof(Framework.ActorSystem.Runtime.Skill),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Skill",false, null,typeof(Framework.ActorSystem.Runtime.Skill))]
#endif
		static bool AT_ClearLockTargets(Skill pPointerThis)
		{
			pPointerThis.ClearLockTargets();
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(-2041930942,"获取CD时长(ms)",typeof(Framework.ActorSystem.Runtime.Skill),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Skill",false, null,typeof(Framework.ActorSystem.Runtime.Skill))]
		[ATFunctionReturn(typeof(Framework.AT.Runtime.VariableLong), "pReturn", null,typeof(System.Int64))]
#endif
		static bool AT_GetConfigCD(Skill pPointerThis,AgentTree pAgentTree, BaseNode pNode)
		{
			pAgentTree.SetOutportLong(pNode, 0, pPointerThis.GetConfigCD());
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(-1738200053,"设置CD时长(ms)",typeof(Framework.ActorSystem.Runtime.Skill),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Skill",false, null,typeof(Framework.ActorSystem.Runtime.Skill))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableLong),"cd",false, null,typeof(System.Int64))]
#endif
		static bool AT_SetConfigCD(Skill pPointerThis,System.Int64 cd)
		{
			pPointerThis.SetConfigCD(cd);
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(220708757,"获取动作类型",typeof(Framework.ActorSystem.Runtime.Skill),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Skill",false, null,typeof(Framework.ActorSystem.Runtime.Skill))]
		[ATFunctionReturn(typeof(Framework.AT.Runtime.VariableInt), "pReturn", null,typeof(Framework.ActorSystem.Runtime.EActionStateType))]
#endif
		static bool AT_GetActionType(Skill pPointerThis,AgentTree pAgentTree, BaseNode pNode)
		{
			pAgentTree.SetOutportInt(pNode, 0, (int)pPointerThis.GetActionType());
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(1148700247,"获取动作tag",typeof(Framework.ActorSystem.Runtime.Skill),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Skill",false, null,typeof(Framework.ActorSystem.Runtime.Skill))]
		[ATFunctionReturn(typeof(Framework.AT.Runtime.VariableInt), "pReturn", null,typeof(System.UInt32))]
#endif
		static bool AT_GetActionTag(Skill pPointerThis,AgentTree pAgentTree, BaseNode pNode)
		{
			pAgentTree.SetOutportUint(pNode, 0, pPointerThis.GetActionTag());
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(214929596,"设置绑定动作",typeof(Framework.ActorSystem.Runtime.Skill),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Skill",false, null,typeof(Framework.ActorSystem.Runtime.Skill))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableInt),"eType",false, null,typeof(Framework.ActorSystem.Runtime.EActionStateType))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableInt),"nTag",false, null,typeof(System.UInt32))]
#endif
		static bool AT_SetActionTypeAndTag(Skill pPointerThis,Framework.ActorSystem.Runtime.EActionStateType eType,System.UInt32 nTag)
		{
			pPointerThis.SetActionTypeAndTag(eType,nTag);
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(253184896,"获取属性计算公式",typeof(Framework.ActorSystem.Runtime.Skill),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Skill",false, null,typeof(Framework.ActorSystem.Runtime.Skill))]
		[ATFunctionReturn(typeof(Framework.AT.Runtime.VariableInt), "pReturn", null,typeof(System.Int32))]
#endif
		static bool AT_GetAttrFormulaType(Skill pPointerThis,AgentTree pAgentTree, BaseNode pNode)
		{
			pAgentTree.SetOutportInt(pNode, 0, pPointerThis.GetAttrFormulaType());
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(706527390,"设置属性计算公式",typeof(Framework.ActorSystem.Runtime.Skill),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Skill",false, null,typeof(Framework.ActorSystem.Runtime.Skill))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableInt),"type",false, null,typeof(System.Int32))]
#endif
		static bool AT_SetAttrFormulaType(Skill pPointerThis,System.Int32 type)
		{
			pPointerThis.SetAttrFormulaType(type);
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
			case 1009185467://GetRuntimeCD
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 0) return true;
				if(!(pUserClass.pPointer is Skill)) return true;
				return AT_GetRuntimeCD((Skill)pUserClass.pPointer,pAgentTree, pNode);
			}
			case 414892563://SetLevel
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 1) return true;
				if(!(pUserClass.pPointer is Skill)) return true;
				return AT_SetLevel((Skill)pUserClass.pPointer,pAgentTree.GetInportUint(pNode,1));
			}
			case 1793744141://CanTrigger
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 0) return true;
				if(!(pUserClass.pPointer is Skill)) return true;
				return AT_CanTrigger((Skill)pUserClass.pPointer,pAgentTree, pNode);
			}
			case -282211430://AddLockTarget
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 2) return true;
				if(!(pUserClass.pPointer is Skill)) return true;
				return AT_AddLockTarget((Skill)pUserClass.pPointer,pAgentTree.GetInportUserData<Framework.ActorSystem.Runtime.Actor>(pNode,1),pAgentTree.GetInportBool(pNode,2));
			}
			case 1216193890://ClearLockTargets
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 0) return true;
				if(!(pUserClass.pPointer is Skill)) return true;
				return AT_ClearLockTargets((Skill)pUserClass.pPointer);
			}
			case -2041930942://GetConfigCD
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 0) return true;
				if(!(pUserClass.pPointer is Skill)) return true;
				return AT_GetConfigCD((Skill)pUserClass.pPointer,pAgentTree, pNode);
			}
			case -1738200053://SetConfigCD
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 1) return true;
				if(!(pUserClass.pPointer is Skill)) return true;
				return AT_SetConfigCD((Skill)pUserClass.pPointer,pAgentTree.GetInportLong(pNode,1));
			}
			case 220708757://GetActionType
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 0) return true;
				if(!(pUserClass.pPointer is Skill)) return true;
				return AT_GetActionType((Skill)pUserClass.pPointer,pAgentTree, pNode);
			}
			case 1148700247://GetActionTag
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 0) return true;
				if(!(pUserClass.pPointer is Skill)) return true;
				return AT_GetActionTag((Skill)pUserClass.pPointer,pAgentTree, pNode);
			}
			case 214929596://SetActionTypeAndTag
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 2) return true;
				if(!(pUserClass.pPointer is Skill)) return true;
				return AT_SetActionTypeAndTag((Skill)pUserClass.pPointer,(Framework.ActorSystem.Runtime.EActionStateType)pAgentTree.GetInportInt(pNode,1),pAgentTree.GetInportUint(pNode,2));
			}
			case 253184896://GetAttrFormulaType
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 0) return true;
				if(!(pUserClass.pPointer is Skill)) return true;
				return AT_GetAttrFormulaType((Skill)pUserClass.pPointer,pAgentTree, pNode);
			}
			case 706527390://SetAttrFormulaType
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 1) return true;
				if(!(pUserClass.pPointer is Skill)) return true;
				return AT_SetAttrFormulaType((Skill)pUserClass.pPointer,pAgentTree.GetInportInt(pNode,1));
			}
			}
			return true;
		}
	}
}

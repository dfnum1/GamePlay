//auto generated
using Framework.AT.Runtime;
namespace Framework.ActorSystem.Runtime
{
#if UNITY_EDITOR
	[ATClass(typeof(Framework.ActorSystem.Runtime.SkillSystem),"Actor系统/技能系统")]
#endif
	public class Framework_ActorSystem_Runtime_SkillSystem
	{
#if UNITY_EDITOR
		[ATFunction(680975210,"获或当前Actor",typeof(Framework.ActorSystem.Runtime.SkillSystem),false)]
		[ATFunctionArgv(typeof(VariableUserData),"SkillSystem",false, null,typeof(Framework.ActorSystem.Runtime.SkillSystem))]
		[ATFunctionReturn(typeof(Framework.AT.Runtime.VariableUserData), "pReturn", null,typeof(Framework.ActorSystem.Runtime.Actor))]
#endif
		static bool AT_GetActor(SkillSystem pPointerThis,AgentTree pAgentTree, BaseNode pNode)
		{
			pAgentTree.SetOutportUserData(pNode, 0, pPointerThis.GetActor());
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(-180560408,"添加索敌单位",typeof(Framework.ActorSystem.Runtime.SkillSystem),false)]
		[ATFunctionArgv(typeof(VariableUserData),"SkillSystem",false, null,typeof(Framework.ActorSystem.Runtime.SkillSystem))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableUserData),"pNode",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableBool),"bClear",false, null,typeof(System.Boolean))]
#endif
		static bool AT_AddLockTarget(SkillSystem pPointerThis,Framework.ActorSystem.Runtime.Actor pNode,System.Boolean bClear)
		{
			pPointerThis.AddLockTarget(pNode,bClear);
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(1763740771,"清除索敌单位",typeof(Framework.ActorSystem.Runtime.SkillSystem),false)]
		[ATFunctionArgv(typeof(VariableUserData),"SkillSystem",false, null,typeof(Framework.ActorSystem.Runtime.SkillSystem))]
#endif
		static bool AT_ClearLockTargets(SkillSystem pPointerThis)
		{
			pPointerThis.ClearLockTargets();
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(1360030028,"添加技能",typeof(Framework.ActorSystem.Runtime.SkillSystem),false)]
		[ATFunctionArgv(typeof(VariableUserData),"SkillSystem",false, null,typeof(Framework.ActorSystem.Runtime.SkillSystem))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableUserData),"pSkill",false, null,typeof(Framework.ActorSystem.Runtime.Skill))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableInt),"eType",false, null,typeof(Framework.ActorSystem.Runtime.ESkillType))]
#endif
		static bool AT_AddSkill(SkillSystem pPointerThis,Framework.ActorSystem.Runtime.Skill pSkill,Framework.ActorSystem.Runtime.ESkillType eType)
		{
			pPointerThis.AddSkill(pSkill,eType);
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(-1373543655,"获取当前技能",typeof(Framework.ActorSystem.Runtime.SkillSystem),false)]
		[ATFunctionArgv(typeof(VariableUserData),"SkillSystem",false, null,typeof(Framework.ActorSystem.Runtime.SkillSystem))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableBool),"checkNoAction",false, null,typeof(System.Boolean))]
		[ATFunctionReturn(typeof(Framework.AT.Runtime.VariableUserData), "pReturn", null,typeof(Framework.ActorSystem.Runtime.Skill))]
#endif
		static bool AT_GetCurrentSkill(SkillSystem pPointerThis,System.Boolean checkNoAction,AgentTree pAgentTree, BaseNode pNode)
		{
			pAgentTree.SetOutportUserData(pNode, 0, pPointerThis.GetCurrentSkill(checkNoAction));
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(-1067266602,"执行技能",typeof(Framework.ActorSystem.Runtime.SkillSystem),false)]
		[ATFunctionArgv(typeof(VariableUserData),"SkillSystem",false, null,typeof(Framework.ActorSystem.Runtime.SkillSystem))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableUserData),"pSkill",false, null,typeof(Framework.ActorSystem.Runtime.Skill))]
		[ATFunctionReturn(typeof(Framework.AT.Runtime.VariableBool), "pReturn", null,typeof(System.Boolean))]
#endif
		static bool AT_DoSkill(SkillSystem pPointerThis,Framework.ActorSystem.Runtime.Skill pSkill,AgentTree pAgentTree, BaseNode pNode)
		{
			pAgentTree.SetOutportBool(pNode, 0, pPointerThis.DoSkill(pSkill));
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(680701531,"设置自动执行",typeof(Framework.ActorSystem.Runtime.SkillSystem),false)]
		[ATFunctionArgv(typeof(VariableUserData),"SkillSystem",false, null,typeof(Framework.ActorSystem.Runtime.SkillSystem))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableBool),"bAuto",false, null,typeof(System.Boolean))]
#endif
		static bool AT_EnableAutoSkill(SkillSystem pPointerThis,System.Boolean bAuto)
		{
			pPointerThis.EnableAutoSkill(bAuto);
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(786601174,"是否自动执行",typeof(Framework.ActorSystem.Runtime.SkillSystem),false)]
		[ATFunctionArgv(typeof(VariableUserData),"SkillSystem",false, null,typeof(Framework.ActorSystem.Runtime.SkillSystem))]
		[ATFunctionReturn(typeof(Framework.AT.Runtime.VariableBool), "pReturn", null,typeof(System.Boolean))]
#endif
		static bool AT_IsAutoSkill(SkillSystem pPointerThis,AgentTree pAgentTree, BaseNode pNode)
		{
			pAgentTree.SetOutportBool(pNode, 0, pPointerThis.IsAutoSkill());
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
			case 680975210://GetActor
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 0) return true;
				if(!(pUserClass.pPointer is SkillSystem)) return true;
				return AT_GetActor((SkillSystem)pUserClass.pPointer,pAgentTree, pNode);
			}
			case -180560408://AddLockTarget
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 2) return true;
				if(!(pUserClass.pPointer is SkillSystem)) return true;
				return AT_AddLockTarget((SkillSystem)pUserClass.pPointer,pAgentTree.GetInportUserData<Framework.ActorSystem.Runtime.Actor>(pNode,1),pAgentTree.GetInportBool(pNode,2));
			}
			case 1763740771://ClearLockTargets
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 0) return true;
				if(!(pUserClass.pPointer is SkillSystem)) return true;
				return AT_ClearLockTargets((SkillSystem)pUserClass.pPointer);
			}
			case 1360030028://AddSkill
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 2) return true;
				if(!(pUserClass.pPointer is SkillSystem)) return true;
				return AT_AddSkill((SkillSystem)pUserClass.pPointer,pAgentTree.GetInportUserData<Framework.ActorSystem.Runtime.Skill>(pNode,1),(Framework.ActorSystem.Runtime.ESkillType)pAgentTree.GetInportInt(pNode,2));
			}
			case -1373543655://GetCurrentSkill
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 1) return true;
				if(!(pUserClass.pPointer is SkillSystem)) return true;
				return AT_GetCurrentSkill((SkillSystem)pUserClass.pPointer,pAgentTree.GetInportBool(pNode,1), pAgentTree, pNode);
			}
			case -1067266602://DoSkill
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 1) return true;
				if(!(pUserClass.pPointer is SkillSystem)) return true;
				return AT_DoSkill((SkillSystem)pUserClass.pPointer,pAgentTree.GetInportUserData<Framework.ActorSystem.Runtime.Skill>(pNode,1), pAgentTree, pNode);
			}
			case 680701531://EnableAutoSkill
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 1) return true;
				if(!(pUserClass.pPointer is SkillSystem)) return true;
				return AT_EnableAutoSkill((SkillSystem)pUserClass.pPointer,pAgentTree.GetInportBool(pNode,1));
			}
			case 786601174://IsAutoSkill
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 0) return true;
				if(!(pUserClass.pPointer is SkillSystem)) return true;
				return AT_IsAutoSkill((SkillSystem)pUserClass.pPointer,pAgentTree, pNode);
			}
			}
			return true;
		}
	}
}

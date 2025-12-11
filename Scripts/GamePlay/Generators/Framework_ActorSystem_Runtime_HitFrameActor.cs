//auto generated
using Framework.AT.Runtime;
namespace Framework.ActorSystem.Runtime
{
#if UNITY_EDITOR
	[ATClass("Actor系统/命中帧数据")]
#endif
	public class Framework_ActorSystem_Runtime_HitFrameActor
	{
#if UNITY_EDITOR
		[ATFunction(-1036468186,"攻击者",typeof(Framework.ActorSystem.Runtime.HitFrameActor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"HitFrameActor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableUserData),"attack_ptr",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
#endif
		static bool AT_Get_attack_ptr(HitFrameActor pPointerThis, AgentTree pAgentTree, BaseNode pNode)
		{
			pAgentTree.SetOutportUserData(pNode, 0, pPointerThis.attack_ptr);
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(1958629959,"受击者",typeof(Framework.ActorSystem.Runtime.HitFrameActor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"HitFrameActor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableUserData),"target_ptr",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
#endif
		static bool AT_Get_target_ptr(HitFrameActor pPointerThis, AgentTree pAgentTree, BaseNode pNode)
		{
			pAgentTree.SetOutportUserData(pNode, 0, pPointerThis.target_ptr);
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(1151495309,"受击位置",typeof(Framework.ActorSystem.Runtime.HitFrameActor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"HitFrameActor",false, null,typeof(UnityEngine.Vector3))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableVec3),"hit_position",false, null,typeof(UnityEngine.Vector3))]
#endif
		static bool AT_Get_hit_position(HitFrameActor pPointerThis, AgentTree pAgentTree, BaseNode pNode)
		{
			pAgentTree.SetOutportVec3(pNode, 0, pPointerThis.hit_position);
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(1620485685,"受击朝向",typeof(Framework.ActorSystem.Runtime.HitFrameActor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"HitFrameActor",false, null,typeof(UnityEngine.Vector3))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableVec3),"hit_direction",false, null,typeof(UnityEngine.Vector3))]
#endif
		static bool AT_Get_hit_direction(HitFrameActor pPointerThis, AgentTree pAgentTree, BaseNode pNode)
		{
			pAgentTree.SetOutportVec3(pNode, 0, pPointerThis.hit_direction);
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(737022015,"受击类型",typeof(Framework.ActorSystem.Runtime.HitFrameActor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"HitFrameActor",false, null,typeof(Framework.ActorSystem.Runtime.EHitType))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableInt),"hitType",false, null,typeof(Framework.ActorSystem.Runtime.EHitType))]
#endif
		static bool AT_Get_hitType(HitFrameActor pPointerThis, AgentTree pAgentTree, BaseNode pNode)
		{
			pAgentTree.SetOutportInt(pNode, 0, (int)pPointerThis.hitType);
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(-1786296471,"是否打到场景",typeof(Framework.ActorSystem.Runtime.HitFrameActor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"HitFrameActor",false, null,typeof(System.Boolean))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableBool),"bHitScene",false, null,typeof(System.Boolean))]
#endif
		static bool AT_Get_bHitScene(HitFrameActor pPointerThis, AgentTree pAgentTree, BaseNode pNode)
		{
			pAgentTree.SetOutportBool(pNode, 0, pPointerThis.bHitScene);
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
			case -1036468186://attack_ptr get
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 1) return true;
				if(!(pUserClass.pPointer is HitFrameActor)) return true;
				return AT_Get_attack_ptr((HitFrameActor)pUserClass.pPointer, pAgentTree, pNode);
			}
			case 1958629959://target_ptr get
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 1) return true;
				if(!(pUserClass.pPointer is HitFrameActor)) return true;
				return AT_Get_target_ptr((HitFrameActor)pUserClass.pPointer, pAgentTree, pNode);
			}
			case 1151495309://hit_position get
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 1) return true;
				if(!(pUserClass.pPointer is HitFrameActor)) return true;
				return AT_Get_hit_position((HitFrameActor)pUserClass.pPointer, pAgentTree, pNode);
			}
			case 1620485685://hit_direction get
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 1) return true;
				if(!(pUserClass.pPointer is HitFrameActor)) return true;
				return AT_Get_hit_direction((HitFrameActor)pUserClass.pPointer, pAgentTree, pNode);
			}
			case 737022015://hitType get
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 1) return true;
				if(!(pUserClass.pPointer is HitFrameActor)) return true;
				return AT_Get_hitType((HitFrameActor)pUserClass.pPointer, pAgentTree, pNode);
			}
			case -1786296471://bHitScene get
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 1) return true;
				if(!(pUserClass.pPointer is HitFrameActor)) return true;
				return AT_Get_bHitScene((HitFrameActor)pUserClass.pPointer, pAgentTree, pNode);
			}
			}
			return true;
		}
	}
}

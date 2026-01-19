//auto generated
using Framework.AT.Runtime;
namespace Framework.ActorSystem.Runtime
{
#if UNITY_EDITOR
	[ATClass(typeof(Framework.ActorSystem.Runtime.Actor),"Actor系统/Actor")]
#endif
	public class Framework_ActorSystem_Runtime_Actor
	{
#if UNITY_EDITOR
		[ATFunction(404601789,"获取实例ID",typeof(Framework.ActorSystem.Runtime.Actor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Actor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
		[ATFunctionReturn(typeof(Framework.AT.Runtime.VariableInt), "pReturn", null,typeof(System.Int32))]
#endif
		static bool AT_GetInstanceID(Actor pPointerThis,AgentTree pAgentTree, BaseNode pNode)
		{
			pAgentTree.SetOutportInt(pNode, 0, pPointerThis.GetInstanceID());
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(1173551111,"获取攻击组",typeof(Framework.ActorSystem.Runtime.Actor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Actor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
		[ATFunctionReturn(typeof(Framework.AT.Runtime.VariableInt), "pReturn", null,typeof(System.Byte))]
#endif
		static bool AT_GetAttackGroup(Actor pPointerThis,AgentTree pAgentTree, BaseNode pNode)
		{
			pAgentTree.SetOutportByte(pNode, 0, pPointerThis.GetAttackGroup());
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(988564336,"设置攻击组",typeof(Framework.ActorSystem.Runtime.Actor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Actor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableInt),"attackGroup",false, null,typeof(System.Byte))]
#endif
		static bool AT_SetAttackGroup(Actor pPointerThis,System.Byte attackGroup)
		{
			pPointerThis.SetAttackGroup(attackGroup);
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(2059933719,"是否可攻击",typeof(Framework.ActorSystem.Runtime.Actor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Actor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableInt),"attackGroup",false, null,typeof(System.Byte))]
		[ATFunctionReturn(typeof(Framework.AT.Runtime.VariableBool), "pReturn", null,typeof(System.Boolean))]
#endif
		static bool AT_CanAttackGroup(Actor pPointerThis,System.Byte attackGroup,AgentTree pAgentTree, BaseNode pNode)
		{
			pAgentTree.SetOutportBool(pNode, 0, pPointerThis.CanAttackGroup(attackGroup));
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(1654382610,"设置速度",typeof(Framework.ActorSystem.Runtime.Actor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Actor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableVec3),"speed",false, null,typeof(UnityEngine.Vector3))]
#endif
		static bool AT_SetSpeed(Actor pPointerThis,UnityEngine.Vector3 speed)
		{
			pPointerThis.SetSpeed(speed);
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(-837711597,"设置速度XZ",typeof(Framework.ActorSystem.Runtime.Actor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Actor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableVec3),"speed",false, null,typeof(UnityEngine.Vector3))]
#endif
		static bool AT_SetSpeedXZ(Actor pPointerThis,UnityEngine.Vector3 speed)
		{
			pPointerThis.SetSpeedXZ(speed);
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(-1704271933,"获取当前速度",typeof(Framework.ActorSystem.Runtime.Actor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Actor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
		[ATFunctionReturn(typeof(Framework.AT.Runtime.VariableVec3), "pReturn", null,typeof(UnityEngine.Vector3))]
#endif
		static bool AT_GetSpeed(Actor pPointerThis,AgentTree pAgentTree, BaseNode pNode)
		{
			pAgentTree.SetOutportVec3(pNode, 0, pPointerThis.GetSpeed());
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(134270141,"设置Y速度",typeof(Framework.ActorSystem.Runtime.Actor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Actor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableFloat),"fSpeedY",false, null,typeof(System.Single))]
#endif
		static bool AT_SetSpeedY(Actor pPointerThis,System.Single fSpeedY)
		{
			pPointerThis.SetSpeedY(fSpeedY);
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(824513177,"设置摩檫力",typeof(Framework.ActorSystem.Runtime.Actor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Actor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableFloat),"fValue",false, null,typeof(System.Single))]
#endif
		static bool AT_SetFraction(Actor pPointerThis,System.Single fValue)
		{
			pPointerThis.SetFraction(fValue);
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(789226960,"获取摩檫力",typeof(Framework.ActorSystem.Runtime.Actor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Actor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
		[ATFunctionReturn(typeof(Framework.AT.Runtime.VariableFloat), "pReturn", null,typeof(System.Single))]
#endif
		static bool AT_GetFraction(Actor pPointerThis,AgentTree pAgentTree, BaseNode pNode)
		{
			pAgentTree.SetOutportFloat(pNode, 0, pPointerThis.GetFraction());
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(421729815,"启用重力",typeof(Framework.ActorSystem.Runtime.Actor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Actor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableBool),"bEnable",false, null,typeof(System.Boolean))]
#endif
		static bool AT_EnableGravity(Actor pPointerThis,System.Boolean bEnable)
		{
			pPointerThis.EnableGravity(bEnable);
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(-1711258735,"设置重力",typeof(Framework.ActorSystem.Runtime.Actor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Actor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableFloat),"fValue",false, null,typeof(System.Single))]
#endif
		static bool AT_SetGravity(Actor pPointerThis,System.Single fValue)
		{
			pPointerThis.SetGravity(fValue);
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(-1270521341,"获取重力",typeof(Framework.ActorSystem.Runtime.Actor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Actor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
		[ATFunctionReturn(typeof(Framework.AT.Runtime.VariableFloat), "pReturn", null,typeof(System.Single))]
#endif
		static bool AT_GetGravity(Actor pPointerThis,AgentTree pAgentTree, BaseNode pNode)
		{
			pAgentTree.SetOutportFloat(pNode, 0, pPointerThis.GetGravity());
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(-1753621693,"获取当前位置",typeof(Framework.ActorSystem.Runtime.Actor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Actor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
		[ATFunctionReturn(typeof(Framework.AT.Runtime.VariableVec3), "pReturn", null,typeof(UnityEngine.Vector3))]
#endif
		static bool AT_GetPosition(Actor pPointerThis,AgentTree pAgentTree, BaseNode pNode)
		{
			pAgentTree.SetOutportVec3(pNode, 0, pPointerThis.GetPosition());
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(1977532792,"获取上一次位置",typeof(Framework.ActorSystem.Runtime.Actor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Actor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
		[ATFunctionReturn(typeof(Framework.AT.Runtime.VariableVec3), "pReturn", null,typeof(UnityEngine.Vector3))]
#endif
		static bool AT_GetLastPosition(Actor pPointerThis,AgentTree pAgentTree, BaseNode pNode)
		{
			pAgentTree.SetOutportVec3(pNode, 0, pPointerThis.GetLastPosition());
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(-1990824950,"设置位置",typeof(Framework.ActorSystem.Runtime.Actor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Actor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableVec3),"vPosition",false, null,typeof(UnityEngine.Vector3))]
#endif
		static bool AT_SetPosition(Actor pPointerThis,UnityEngine.Vector3 vPosition)
		{
			pPointerThis.SetPosition(vPosition);
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(1500835776,"位置偏移",typeof(Framework.ActorSystem.Runtime.Actor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Actor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableVec3),"vOffset",false, null,typeof(UnityEngine.Vector3))]
#endif
		static bool AT_OffsetPosition(Actor pPointerThis,UnityEngine.Vector3 vOffset)
		{
			pPointerThis.OffsetPosition(vOffset);
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(-1274405658,"获取角度",typeof(Framework.ActorSystem.Runtime.Actor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Actor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
		[ATFunctionReturn(typeof(Framework.AT.Runtime.VariableVec3), "pReturn", null,typeof(UnityEngine.Vector3))]
#endif
		static bool AT_GetEulerAngle(Actor pPointerThis,AgentTree pAgentTree, BaseNode pNode)
		{
			pAgentTree.SetOutportVec3(pNode, 0, pPointerThis.GetEulerAngle());
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(56344714,"获取最终角度",typeof(Framework.ActorSystem.Runtime.Actor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Actor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
		[ATFunctionReturn(typeof(Framework.AT.Runtime.VariableVec3), "pReturn", null,typeof(UnityEngine.Vector3))]
#endif
		static bool AT_GetFinalEulerAngle(Actor pPointerThis,AgentTree pAgentTree, BaseNode pNode)
		{
			pAgentTree.SetOutportVec3(pNode, 0, pPointerThis.GetFinalEulerAngle());
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(-1061891926,"设置角度",typeof(Framework.ActorSystem.Runtime.Actor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Actor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableVec3),"vEulerAngle",false, null,typeof(UnityEngine.Vector3))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableBool),"bImmediately",false, null,typeof(System.Boolean))]
#endif
		static bool AT_SetEulerAngle(Actor pPointerThis,UnityEngine.Vector3 vEulerAngle,System.Boolean bImmediately)
		{
			pPointerThis.SetEulerAngle(vEulerAngle,bImmediately);
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(-1374303795,"获取矩阵",typeof(Framework.ActorSystem.Runtime.Actor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Actor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
		[ATFunctionReturn(typeof(Framework.AT.Runtime.VariableMatrix), "pReturn", null,typeof(UnityEngine.Matrix4x4))]
#endif
		static bool AT_GetMatrix(Actor pPointerThis,AgentTree pAgentTree, BaseNode pNode)
		{
			pAgentTree.SetOutportMatrix(pNode, 0, pPointerThis.GetMatrix());
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(-181775319,"移动到目标点",typeof(Framework.ActorSystem.Runtime.Actor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Actor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableVec3),"toPos",false, null,typeof(UnityEngine.Vector3))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableFloat),"speed",false, null,typeof(System.Single))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableBool),"bEnsureSucceed",false, null,typeof(System.Boolean))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableBool),"bUpdateDirection",false, null,typeof(System.Boolean))]
		[ATFunctionReturn(typeof(Framework.AT.Runtime.VariableFloat), "pReturn", null,typeof(System.Single))]
#endif
		static bool AT_RunTo(Actor pPointerThis,UnityEngine.Vector3 toPos,System.Single speed,System.Boolean bEnsureSucceed,System.Boolean bUpdateDirection,AgentTree pAgentTree, BaseNode pNode)
		{
			pAgentTree.SetOutportFloat(pNode, 0, pPointerThis.RunTo(toPos,speed,bEnsureSucceed,bUpdateDirection));
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(-29192949,"导航移动到目标哦点",typeof(Framework.ActorSystem.Runtime.Actor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Actor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableVec3),"toPos",false, null,typeof(UnityEngine.Vector3))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableFloat),"speed",false, null,typeof(System.Single))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableBool),"bEnsureSucceed",false, null,typeof(System.Boolean))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableBool),"bUpdateDirection",false, null,typeof(System.Boolean))]
#endif
		static bool AT_NavRunTo(Actor pPointerThis,UnityEngine.Vector3 toPos,System.Single speed,System.Boolean bEnsureSucceed,System.Boolean bUpdateDirection)
		{
			pPointerThis.NavRunTo(toPos,speed,bEnsureSucceed,bUpdateDirection);
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(-1665969306,"停止路径移动",typeof(Framework.ActorSystem.Runtime.Actor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Actor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
#endif
		static bool AT_StopRunAlongPathPoint(Actor pPointerThis)
		{
			pPointerThis.StopRunAlongPathPoint();
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(-234109610,"是否路径移动",typeof(Framework.ActorSystem.Runtime.Actor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Actor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
		[ATFunctionReturn(typeof(Framework.AT.Runtime.VariableBool), "pReturn", null,typeof(System.Boolean))]
#endif
		static bool AT_IsRunAlongPathPlaying(Actor pPointerThis,AgentTree pAgentTree, BaseNode pNode)
		{
			pAgentTree.SetOutportBool(pNode, 0, pPointerThis.IsRunAlongPathPlaying());
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(809655427,"获取包围盒",typeof(Framework.ActorSystem.Runtime.Actor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Actor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
		[ATFunctionReturn(typeof(Framework.AT.Runtime.VariableBounds), "pReturn", null,typeof(UnityEngine.Bounds))]
#endif
		static bool AT_GetBound(Actor pPointerThis,AgentTree pAgentTree, BaseNode pNode)
		{
			pAgentTree.SetOutportBounds(pNode, 0, pPointerThis.GetBound());
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(-927767726,"设置包围盒大小",typeof(Framework.ActorSystem.Runtime.Actor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Actor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableVec3),"min",false, null,typeof(UnityEngine.Vector3))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableVec3),"max",false, null,typeof(UnityEngine.Vector3))]
#endif
		static bool AT_SetBound(Actor pPointerThis,UnityEngine.Vector3 min,UnityEngine.Vector3 max)
		{
			pPointerThis.SetBound(min,max);
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(327632054,"设置方向",typeof(Framework.ActorSystem.Runtime.Actor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Actor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableVec3),"vDirection",false, null,typeof(UnityEngine.Vector3))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableFloat),"turnTime",false, null,typeof(System.Single))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableBool),"replaceTurnTime",false, null,typeof(System.Boolean))]
#endif
		static bool AT_SetDirection(Actor pPointerThis,UnityEngine.Vector3 vDirection,System.Single turnTime,System.Boolean replaceTurnTime)
		{
			pPointerThis.SetDirection(vDirection,turnTime,replaceTurnTime);
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(479826485,"获取方向",typeof(Framework.ActorSystem.Runtime.Actor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Actor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
		[ATFunctionReturn(typeof(Framework.AT.Runtime.VariableVec3), "pReturn", null,typeof(UnityEngine.Vector3))]
#endif
		static bool AT_GetDirection(Actor pPointerThis,AgentTree pAgentTree, BaseNode pNode)
		{
			pAgentTree.SetOutportVec3(pNode, 0, pPointerThis.GetDirection());
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(-266746549,"获取最终方向",typeof(Framework.ActorSystem.Runtime.Actor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Actor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
		[ATFunctionReturn(typeof(Framework.AT.Runtime.VariableVec3), "pReturn", null,typeof(UnityEngine.Vector3))]
#endif
		static bool AT_GetFinalDirection(Actor pPointerThis,AgentTree pAgentTree, BaseNode pNode)
		{
			pAgentTree.SetOutportVec3(pNode, 0, pPointerThis.GetFinalDirection());
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(-2003762858,"获取Up朝向",typeof(Framework.ActorSystem.Runtime.Actor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Actor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
		[ATFunctionReturn(typeof(Framework.AT.Runtime.VariableVec3), "pReturn", null,typeof(UnityEngine.Vector3))]
#endif
		static bool AT_GetUp(Actor pPointerThis,AgentTree pAgentTree, BaseNode pNode)
		{
			pAgentTree.SetOutportVec3(pNode, 0, pPointerThis.GetUp());
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(1387341846,"获取最终Up朝向",typeof(Framework.ActorSystem.Runtime.Actor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Actor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
		[ATFunctionReturn(typeof(Framework.AT.Runtime.VariableVec3), "pReturn", null,typeof(UnityEngine.Vector3))]
#endif
		static bool AT_GetFinalUp(Actor pPointerThis,AgentTree pAgentTree, BaseNode pNode)
		{
			pAgentTree.SetOutportVec3(pNode, 0, pPointerThis.GetFinalUp());
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(502345748,"设置Up朝向",typeof(Framework.ActorSystem.Runtime.Actor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Actor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableVec3),"up",false, null,typeof(UnityEngine.Vector3))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableFloat),"turnTime",false, null,typeof(System.Single))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableBool),"replaceTurnTime",false, null,typeof(System.Boolean))]
#endif
		static bool AT_SetUp(Actor pPointerThis,UnityEngine.Vector3 up,System.Single turnTime,System.Boolean replaceTurnTime)
		{
			pPointerThis.SetUp(up,turnTime,replaceTurnTime);
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(562451489,"获取Right朝向",typeof(Framework.ActorSystem.Runtime.Actor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Actor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
		[ATFunctionReturn(typeof(Framework.AT.Runtime.VariableVec3), "pReturn", null,typeof(UnityEngine.Vector3))]
#endif
		static bool AT_GetRight(Actor pPointerThis,AgentTree pAgentTree, BaseNode pNode)
		{
			pAgentTree.SetOutportVec3(pNode, 0, pPointerThis.GetRight());
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(775185944,"获取最终Right朝向",typeof(Framework.ActorSystem.Runtime.Actor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Actor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
		[ATFunctionReturn(typeof(Framework.AT.Runtime.VariableVec3), "pReturn", null,typeof(UnityEngine.Vector3))]
#endif
		static bool AT_GetFinalRight(Actor pPointerThis,AgentTree pAgentTree, BaseNode pNode)
		{
			pAgentTree.SetOutportVec3(pNode, 0, pPointerThis.GetFinalRight());
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(2030699697,"获取缩放",typeof(Framework.ActorSystem.Runtime.Actor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Actor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
		[ATFunctionReturn(typeof(Framework.AT.Runtime.VariableVec3), "pReturn", null,typeof(UnityEngine.Vector3))]
#endif
		static bool AT_GetScale(Actor pPointerThis,AgentTree pAgentTree, BaseNode pNode)
		{
			pAgentTree.SetOutportVec3(pNode, 0, pPointerThis.GetScale());
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(-2114241696,"设置缩放",typeof(Framework.ActorSystem.Runtime.Actor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Actor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableVec3),"scale",false, null,typeof(UnityEngine.Vector3))]
#endif
		static bool AT_SetScale(Actor pPointerThis,UnityEngine.Vector3 scale)
		{
			pPointerThis.SetScale(scale);
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(-1309849449,"设置位置角度缩放",typeof(Framework.ActorSystem.Runtime.Actor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Actor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableVec3),"vPosition",false, null,typeof(UnityEngine.Vector3))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableVec3),"vEulerAngle",false, null,typeof(UnityEngine.Vector3))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableVec3),"vScale",false, null,typeof(UnityEngine.Vector3))]
#endif
		static bool AT_SetTransfrom(Actor pPointerThis,UnityEngine.Vector3 vPosition,UnityEngine.Vector3 vEulerAngle,UnityEngine.Vector3 vScale)
		{
			pPointerThis.SetTransfrom(vPosition,vEulerAngle,vScale);
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(-322938593,"是否拥有标志",typeof(Framework.ActorSystem.Runtime.Actor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Actor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableInt),"flag",false, null,typeof(Framework.ActorSystem.Runtime.EActorFlag))]
		[ATFunctionReturn(typeof(Framework.AT.Runtime.VariableBool), "pReturn", null,typeof(System.Boolean))]
#endif
		static bool AT_IsFlag(Actor pPointerThis,Framework.ActorSystem.Runtime.EActorFlag flag,AgentTree pAgentTree, BaseNode pNode)
		{
			pAgentTree.SetOutportBool(pNode, 0, pPointerThis.IsFlag(flag));
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(-1324424049,"设置标志",typeof(Framework.ActorSystem.Runtime.Actor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Actor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableInt),"flag",false, null,typeof(Framework.ActorSystem.Runtime.EActorFlag))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableBool),"bSet",false, null,typeof(System.Boolean))]
#endif
		static bool AT_SetFlag(Actor pPointerThis,Framework.ActorSystem.Runtime.EActorFlag flag,System.Boolean bSet)
		{
			pPointerThis.SetFlag(flag,bSet);
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(-906116501,"是否开启逻辑",typeof(Framework.ActorSystem.Runtime.Actor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Actor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
		[ATFunctionReturn(typeof(Framework.AT.Runtime.VariableBool), "pReturn", null,typeof(System.Boolean))]
#endif
		static bool AT_IsCanLogic(Actor pPointerThis,AgentTree pAgentTree, BaseNode pNode)
		{
			pAgentTree.SetOutportBool(pNode, 0, pPointerThis.IsCanLogic());
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(1288257277,"是否不可见",typeof(Framework.ActorSystem.Runtime.Actor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Actor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
		[ATFunctionReturn(typeof(Framework.AT.Runtime.VariableBool), "pReturn", null,typeof(System.Boolean))]
#endif
		static bool AT_IsInvincible(Actor pPointerThis,AgentTree pAgentTree, BaseNode pNode)
		{
			pAgentTree.SetOutportBool(pNode, 0, pPointerThis.IsInvincible());
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(1288702801,"设置延迟删除时间",typeof(Framework.ActorSystem.Runtime.Actor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Actor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableFloat),"fTime",false, null,typeof(System.Single))]
#endif
		static bool AT_SetDelayDestroy(Actor pPointerThis,System.Single fTime)
		{
			pPointerThis.SetDelayDestroy(fTime);
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(-2100366979,"获取延迟删除时间",typeof(Framework.ActorSystem.Runtime.Actor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Actor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
		[ATFunctionReturn(typeof(Framework.AT.Runtime.VariableFloat), "pReturn", null,typeof(System.Single))]
#endif
		static bool AT_GetDelayDestroy(Actor pPointerThis,AgentTree pAgentTree, BaseNode pNode)
		{
			pAgentTree.SetOutportFloat(pNode, 0, pPointerThis.GetDelayDestroy());
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(-1135091635,"删除",typeof(Framework.ActorSystem.Runtime.Actor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Actor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
#endif
		static bool AT_SetDestroy(Actor pPointerThis)
		{
			pPointerThis.SetDestroy();
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(1827891106,"是否删除",typeof(Framework.ActorSystem.Runtime.Actor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Actor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
		[ATFunctionReturn(typeof(Framework.AT.Runtime.VariableBool), "pReturn", null,typeof(System.Boolean))]
#endif
		static bool AT_IsDestroy(Actor pPointerThis,AgentTree pAgentTree, BaseNode pNode)
		{
			pAgentTree.SetOutportBool(pNode, 0, pPointerThis.IsDestroy());
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(242724015,"HUD开关",typeof(Framework.ActorSystem.Runtime.Actor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Actor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableBool),"bHudBar",false, null,typeof(System.Boolean))]
#endif
		static bool AT_EnableHudBar(Actor pPointerThis,System.Boolean bHudBar)
		{
			pPointerThis.EnableHudBar(bHudBar);
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(1509444428,"是否开启HUD",typeof(Framework.ActorSystem.Runtime.Actor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Actor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
		[ATFunctionReturn(typeof(Framework.AT.Runtime.VariableBool), "pReturn", null,typeof(System.Boolean))]
#endif
		static bool AT_IsEnableHudBar(Actor pPointerThis,AgentTree pAgentTree, BaseNode pNode)
		{
			pAgentTree.SetOutportBool(pNode, 0, pPointerThis.IsEnableHudBar());
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(-1409441560,"是否拥有物理碰撞",typeof(Framework.ActorSystem.Runtime.Actor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Actor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
		[ATFunctionReturn(typeof(Framework.AT.Runtime.VariableBool), "pReturn", null,typeof(System.Boolean))]
#endif
		static bool AT_IsColliderAble(Actor pPointerThis,AgentTree pAgentTree, BaseNode pNode)
		{
			pAgentTree.SetOutportBool(pNode, 0, pPointerThis.IsColliderAble());
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(-2102292443,"设置死亡",typeof(Framework.ActorSystem.Runtime.Actor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Actor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableBool),"bKilled",false, null,typeof(System.Boolean))]
#endif
		static bool AT_SetKilled(Actor pPointerThis,System.Boolean bKilled)
		{
			pPointerThis.SetKilled(bKilled);
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(1176537280,"是否阵亡",typeof(Framework.ActorSystem.Runtime.Actor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Actor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
		[ATFunctionReturn(typeof(Framework.AT.Runtime.VariableBool), "pReturn", null,typeof(System.Boolean))]
#endif
		static bool AT_IsKilled(Actor pPointerThis,AgentTree pAgentTree, BaseNode pNode)
		{
			pAgentTree.SetOutportBool(pNode, 0, pPointerThis.IsKilled());
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(-1163030929,"设置可见性",typeof(Framework.ActorSystem.Runtime.Actor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Actor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableBool),"bVisible",false, null,typeof(System.Boolean))]
#endif
		static bool AT_SetVisible(Actor pPointerThis,System.Boolean bVisible)
		{
			pPointerThis.SetVisible(bVisible);
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(1778977152,"是否可见",typeof(Framework.ActorSystem.Runtime.Actor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Actor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
		[ATFunctionReturn(typeof(Framework.AT.Runtime.VariableBool), "pReturn", null,typeof(System.Boolean))]
#endif
		static bool AT_IsVisible(Actor pPointerThis,AgentTree pAgentTree, BaseNode pNode)
		{
			pAgentTree.SetOutportBool(pNode, 0, pPointerThis.IsVisible());
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(-1232776406,"设置激活",typeof(Framework.ActorSystem.Runtime.Actor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Actor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableBool),"bToggle",false, null,typeof(System.Boolean))]
#endif
		static bool AT_SetActived(Actor pPointerThis,System.Boolean bToggle)
		{
			pPointerThis.SetActived(bToggle);
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(1713493189,"是否激活",typeof(Framework.ActorSystem.Runtime.Actor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Actor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
		[ATFunctionReturn(typeof(Framework.AT.Runtime.VariableBool), "pReturn", null,typeof(System.Boolean))]
#endif
		static bool AT_IsActived(Actor pPointerThis,AgentTree pAgentTree, BaseNode pNode)
		{
			pAgentTree.SetOutportBool(pNode, 0, pPointerThis.IsActived());
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(1725364953,"设置逻辑开关",typeof(Framework.ActorSystem.Runtime.Actor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Actor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableBool),"bToggle",false, null,typeof(System.Boolean))]
#endif
		static bool AT_EnableLogic(Actor pPointerThis,System.Boolean bToggle)
		{
			pPointerThis.EnableLogic(bToggle);
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(-973878293,"是否开启逻辑",typeof(Framework.ActorSystem.Runtime.Actor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Actor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
		[ATFunctionReturn(typeof(Framework.AT.Runtime.VariableBool), "pReturn", null,typeof(System.Boolean))]
#endif
		static bool AT_IsLogicEnable(Actor pPointerThis,AgentTree pAgentTree, BaseNode pNode)
		{
			pAgentTree.SetOutportBool(pNode, 0, pPointerThis.IsLogicEnable());
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(-725487424,"设置启用AI",typeof(Framework.ActorSystem.Runtime.Actor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Actor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableBool),"bToggle",false, null,typeof(System.Boolean))]
#endif
		static bool AT_EnableAI(Actor pPointerThis,System.Boolean bToggle)
		{
			pPointerThis.EnableAI(bToggle);
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(-1904103641,"是否启用AI",typeof(Framework.ActorSystem.Runtime.Actor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Actor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
		[ATFunctionReturn(typeof(Framework.AT.Runtime.VariableBool), "pReturn", null,typeof(System.Boolean))]
#endif
		static bool AT_IsEnableAI(Actor pPointerThis,AgentTree pAgentTree, BaseNode pNode)
		{
			pAgentTree.SetOutportBool(pNode, 0, pPointerThis.IsEnableAI());
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(1199932339,"设置是否动态避障",typeof(Framework.ActorSystem.Runtime.Actor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Actor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableBool),"bToggle",false, null,typeof(System.Boolean))]
#endif
		static bool AT_EnableRVO(Actor pPointerThis,System.Boolean bToggle)
		{
			pPointerThis.EnableRVO(bToggle);
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(2041689143,"是否动态避障",typeof(Framework.ActorSystem.Runtime.Actor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Actor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
		[ATFunctionReturn(typeof(Framework.AT.Runtime.VariableBool), "pReturn", null,typeof(System.Boolean))]
#endif
		static bool AT_IsEnableRVO(Actor pPointerThis,AgentTree pAgentTree, BaseNode pNode)
		{
			pAgentTree.SetOutportBool(pNode, 0, pPointerThis.IsEnableRVO());
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(-112856003,"播放动作-Type Tag",typeof(Framework.ActorSystem.Runtime.Actor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Actor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableInt),"eType",false, null,typeof(Framework.ActorSystem.Runtime.EActionStateType))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableInt),"nTag",false, null,typeof(System.UInt32))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableBool),"bForce",false, null,typeof(System.Boolean))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableUserData),"pStateParam",false, null,typeof(Framework.ActorSystem.Runtime.IContextData))]
#endif
		static bool AT_StartActionState(Actor pPointerThis,Framework.ActorSystem.Runtime.EActionStateType eType,System.UInt32 nTag,System.Boolean bForce,Framework.ActorSystem.Runtime.IContextData pStateParam)
		{
			pPointerThis.StartActionState(eType,nTag,bForce,pStateParam);
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(-48442208,"播放动作-Type与Tag组合Key",typeof(Framework.ActorSystem.Runtime.Actor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Actor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableInt),"nActionTypeAndTag",false, null,typeof(System.UInt32))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableBool),"bForce",false, null,typeof(System.Boolean))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableUserData),"pStateParam",false, null,typeof(Framework.ActorSystem.Runtime.IContextData))]
#endif
		static bool AT_StartActionState_1(Actor pPointerThis,System.UInt32 nActionTypeAndTag,System.Boolean bForce,Framework.ActorSystem.Runtime.IContextData pStateParam)
		{
			pPointerThis.StartActionState(nActionTypeAndTag,bForce,pStateParam);
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(-82564754,"停止动作",typeof(Framework.ActorSystem.Runtime.Actor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Actor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableInt),"eType",false, null,typeof(Framework.ActorSystem.Runtime.EActionStateType))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableInt),"nTag",false, null,typeof(System.UInt32))]
#endif
		static bool AT_StopActionState(Actor pPointerThis,Framework.ActorSystem.Runtime.EActionStateType eType,System.UInt32 nTag)
		{
			pPointerThis.StopActionState(eType,nTag);
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(-1719976999,"设置默认动作",typeof(Framework.ActorSystem.Runtime.Actor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Actor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableInt),"eType",false, null,typeof(Framework.ActorSystem.Runtime.EActionStateType))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableInt),"tag",false, null,typeof(System.UInt32))]
#endif
		static bool AT_SetIdleType(Actor pPointerThis,Framework.ActorSystem.Runtime.EActionStateType eType,System.UInt32 tag)
		{
			pPointerThis.SetIdleType(eType,tag);
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(-151935711,"设置移动动作",typeof(Framework.ActorSystem.Runtime.Actor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Actor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableInt),"eType",false, null,typeof(Framework.ActorSystem.Runtime.EActionStateType))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableInt),"tag",false, null,typeof(System.UInt32))]
#endif
		static bool AT_SetRunType(Actor pPointerThis,Framework.ActorSystem.Runtime.EActionStateType eType,System.UInt32 tag)
		{
			pPointerThis.SetRunType(eType,tag);
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(207081506,"删除动作",typeof(Framework.ActorSystem.Runtime.Actor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Actor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableInt),"eType",false, null,typeof(Framework.ActorSystem.Runtime.EActionStateType))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableInt),"nTag",false, null,typeof(System.UInt32))]
#endif
		static bool AT_RemoveActionState(Actor pPointerThis,Framework.ActorSystem.Runtime.EActionStateType eType,System.UInt32 nTag)
		{
			pPointerThis.RemoveActionState(eType,nTag);
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(1686383617,"设置属性",typeof(Framework.ActorSystem.Runtime.Actor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Actor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableInt),"type",false, null,typeof(System.Byte),drawMethod:"DrawAttributePop")]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableFloat),"value",false, null,typeof(System.Single))]
#endif
		static bool AT_SetAttr(Actor pPointerThis,System.Byte type,System.Single value)
		{
			pPointerThis.SetAttr(type,value);
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(-149580580,"获取属性",typeof(Framework.ActorSystem.Runtime.Actor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Actor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableInt),"type",false, null,typeof(System.Byte),drawMethod:"DrawAttributePop")]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableFloat),"defVal",false, null,typeof(System.Single))]
		[ATFunctionReturn(typeof(Framework.AT.Runtime.VariableFloat), "pReturn", null,typeof(System.Single))]
#endif
		static bool AT_GetAttr(Actor pPointerThis,System.Byte type,System.Single defVal,AgentTree pAgentTree, BaseNode pNode)
		{
			pAgentTree.SetOutportFloat(pNode, 0, pPointerThis.GetAttr(type,defVal));
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(86650515,"移除属性",typeof(Framework.ActorSystem.Runtime.Actor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Actor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableInt),"type",false, null,typeof(System.Byte),drawMethod:"DrawAttributePop")]
#endif
		static bool AT_RemoveAttr(Actor pPointerThis,System.Byte type)
		{
			pPointerThis.RemoveAttr(type);
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(-1093680716,"添加属性",typeof(Framework.ActorSystem.Runtime.Actor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Actor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableInt),"type",false, null,typeof(System.Byte),drawMethod:"DrawAttributePop")]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableFloat),"value",false, null,typeof(System.Single))]
#endif
		static bool AT_AppendAttr(Actor pPointerThis,System.Byte type,System.Single value)
		{
			pPointerThis.AppendAttr(type,value);
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(-1997386568,"减少属性",typeof(Framework.ActorSystem.Runtime.Actor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Actor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableInt),"type",false, null,typeof(System.Byte),drawMethod:"DrawAttributePop")]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableInt),"value",false, null,typeof(System.Int32))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableBool),"bLowerZero",false, null,typeof(System.Boolean))]
#endif
		static bool AT_SubAttr(Actor pPointerThis,System.Byte type,System.Int32 value,System.Boolean bLowerZero)
		{
			pPointerThis.SubAttr(type,value,bLowerZero);
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(-894122339,"清除属性",typeof(Framework.ActorSystem.Runtime.Actor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Actor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
#endif
		static bool AT_ClearAttrs(Actor pPointerThis)
		{
			pPointerThis.ClearAttrs();
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(2015276513,"是否播放某类型动作",typeof(Framework.ActorSystem.Runtime.Actor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Actor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableInt),"eType",false, null,typeof(Framework.ActorSystem.Runtime.EActionStateType))]
		[ATFunctionReturn(typeof(Framework.AT.Runtime.VariableBool), "pReturn", null,typeof(System.Boolean))]
#endif
		static bool AT_IsInAction(Actor pPointerThis,Framework.ActorSystem.Runtime.EActionStateType eType,AgentTree pAgentTree, BaseNode pNode)
		{
			pAgentTree.SetOutportBool(pNode, 0, pPointerThis.IsInAction(eType));
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(-464428257,"获取技能系统",typeof(Framework.ActorSystem.Runtime.Actor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Actor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
		[ATFunctionReturn(typeof(Framework.AT.Runtime.VariableUserData), "pReturn", null,typeof(Framework.ActorSystem.Runtime.SkillSystem))]
#endif
		static bool AT_GetSkillSystem(Actor pPointerThis,AgentTree pAgentTree, BaseNode pNode)
		{
			pAgentTree.SetOutportUserData(pNode, 0, pPointerThis.GetSkillSystem());
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(-2073512697,"是否在攻击",typeof(Framework.ActorSystem.Runtime.Actor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Actor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
		[ATFunctionReturn(typeof(Framework.AT.Runtime.VariableBool), "pReturn", null,typeof(System.Boolean))]
#endif
		static bool AT_IsAttacking(Actor pPointerThis,AgentTree pAgentTree, BaseNode pNode)
		{
			pAgentTree.SetOutportBool(pNode, 0, pPointerThis.IsAttacking());
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(-448329711,"是否被过场控制",typeof(Framework.ActorSystem.Runtime.Actor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Actor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
		[ATFunctionReturn(typeof(Framework.AT.Runtime.VariableBool), "pReturn", null,typeof(System.Boolean))]
#endif
		static bool AT_IsCutscneHolded(Actor pPointerThis,AgentTree pAgentTree, BaseNode pNode)
		{
			pAgentTree.SetOutportBool(pNode, 0, pPointerThis.IsCutscneHolded());
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(2026433217,"重置逻辑冰冻",typeof(Framework.ActorSystem.Runtime.Actor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Actor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
#endif
		static bool AT_ResetFreeze(Actor pPointerThis)
		{
			pPointerThis.ResetFreeze();
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(817905509,"设置逻辑冰冻",typeof(Framework.ActorSystem.Runtime.Actor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Actor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableBool),"bToggle",false, null,typeof(System.Boolean))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableFloat),"fDuration",false, null,typeof(System.Single))]
#endif
		static bool AT_Freezed(Actor pPointerThis,System.Boolean bToggle,System.Single fDuration)
		{
			pPointerThis.Freezed(bToggle,fDuration);
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(1967178651,"是否逻辑冰冻",typeof(Framework.ActorSystem.Runtime.Actor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Actor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
		[ATFunctionReturn(typeof(Framework.AT.Runtime.VariableBool), "pReturn", null,typeof(System.Boolean))]
#endif
		static bool AT_IsFreezed(Actor pPointerThis,AgentTree pAgentTree, BaseNode pNode)
		{
			pAgentTree.SetOutportBool(pNode, 0, pPointerThis.IsFreezed());
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(-1225831306,"碰撞检测-Actor",typeof(Framework.ActorSystem.Runtime.Actor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Actor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableUserData),"pActor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
		[ATFunctionReturn(typeof(Framework.AT.Runtime.VariableBool), "pReturn", null,typeof(System.Boolean))]
#endif
		static bool AT_IsIntersecition(Actor pPointerThis,Framework.ActorSystem.Runtime.Actor pActor,AgentTree pAgentTree, BaseNode pNode)
		{
			pAgentTree.SetOutportBool(pNode, 0, pPointerThis.IsIntersecition(pActor));
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(724252282,"碰撞检测-坐标大小",typeof(Framework.ActorSystem.Runtime.Actor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Actor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableMatrix),"mtTrans",false, null,typeof(UnityEngine.Matrix4x4))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableVec3),"vCenter",false, null,typeof(UnityEngine.Vector3))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableVec3),"vHalf",false, null,typeof(UnityEngine.Vector3))]
		[ATFunctionReturn(typeof(Framework.AT.Runtime.VariableBool), "pReturn", null,typeof(System.Boolean))]
#endif
		static bool AT_IsIntersecition_1(Actor pPointerThis,UnityEngine.Matrix4x4 mtTrans,UnityEngine.Vector3 vCenter,UnityEngine.Vector3 vHalf,AgentTree pAgentTree, BaseNode pNode)
		{
			pAgentTree.SetOutportBool(pNode, 0, pPointerThis.IsIntersecition(mtTrans,vCenter,vHalf));
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(-1306368064,"碰撞检测-球性",typeof(Framework.ActorSystem.Runtime.Actor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Actor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableMatrix),"mtTrans",false, null,typeof(UnityEngine.Matrix4x4))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableFloat),"radius",false, null,typeof(System.Single))]
		[ATFunctionReturn(typeof(Framework.AT.Runtime.VariableBool), "pReturn", null,typeof(System.Boolean))]
#endif
		static bool AT_IsIntersecition_2(Actor pPointerThis,UnityEngine.Matrix4x4 mtTrans,System.Single radius,AgentTree pAgentTree, BaseNode pNode)
		{
			pAgentTree.SetOutportBool(pNode, 0, pPointerThis.IsIntersecition(mtTrans,radius));
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(239105233,"获取绑定矩阵",typeof(Framework.ActorSystem.Runtime.Actor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Actor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableString),"strSlot",false, null,typeof(System.String))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableInt),"bindSlot",false, null,typeof(System.Int32))]
		[ATFunctionReturn(typeof(Framework.AT.Runtime.VariableMatrix), "pReturn", null,typeof(UnityEngine.Matrix4x4))]
#endif
		static bool AT_GetEventBindSlot(Actor pPointerThis,System.String strSlot,System.Int32 bindSlot,AgentTree pAgentTree, BaseNode pNode)
		{
			pAgentTree.SetOutportMatrix(pNode, 0, pPointerThis.GetEventBindSlot(strSlot,bindSlot));
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
			case 404601789://GetInstanceID
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 0) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_GetInstanceID((Actor)pUserClass.pPointer,pAgentTree, pNode);
			}
			case 1173551111://GetAttackGroup
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 0) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_GetAttackGroup((Actor)pUserClass.pPointer,pAgentTree, pNode);
			}
			case 988564336://SetAttackGroup
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 1) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_SetAttackGroup((Actor)pUserClass.pPointer,pAgentTree.GetInportByte(pNode,1));
			}
			case 2059933719://CanAttackGroup
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 1) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_CanAttackGroup((Actor)pUserClass.pPointer,pAgentTree.GetInportByte(pNode,1), pAgentTree, pNode);
			}
			case 1654382610://SetSpeed
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 1) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_SetSpeed((Actor)pUserClass.pPointer,pAgentTree.GetInportVec3(pNode,1));
			}
			case -837711597://SetSpeedXZ
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 1) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_SetSpeedXZ((Actor)pUserClass.pPointer,pAgentTree.GetInportVec3(pNode,1));
			}
			case -1704271933://GetSpeed
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 0) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_GetSpeed((Actor)pUserClass.pPointer,pAgentTree, pNode);
			}
			case 134270141://SetSpeedY
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 1) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_SetSpeedY((Actor)pUserClass.pPointer,pAgentTree.GetInportFloat(pNode,1));
			}
			case 824513177://SetFraction
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 1) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_SetFraction((Actor)pUserClass.pPointer,pAgentTree.GetInportFloat(pNode,1));
			}
			case 789226960://GetFraction
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 0) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_GetFraction((Actor)pUserClass.pPointer,pAgentTree, pNode);
			}
			case 421729815://EnableGravity
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 1) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_EnableGravity((Actor)pUserClass.pPointer,pAgentTree.GetInportBool(pNode,1));
			}
			case -1711258735://SetGravity
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 1) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_SetGravity((Actor)pUserClass.pPointer,pAgentTree.GetInportFloat(pNode,1));
			}
			case -1270521341://GetGravity
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 0) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_GetGravity((Actor)pUserClass.pPointer,pAgentTree, pNode);
			}
			case -1753621693://GetPosition
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 0) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_GetPosition((Actor)pUserClass.pPointer,pAgentTree, pNode);
			}
			case 1977532792://GetLastPosition
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 0) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_GetLastPosition((Actor)pUserClass.pPointer,pAgentTree, pNode);
			}
			case -1990824950://SetPosition
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 1) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_SetPosition((Actor)pUserClass.pPointer,pAgentTree.GetInportVec3(pNode,1));
			}
			case 1500835776://OffsetPosition
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 1) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_OffsetPosition((Actor)pUserClass.pPointer,pAgentTree.GetInportVec3(pNode,1));
			}
			case -1274405658://GetEulerAngle
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 0) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_GetEulerAngle((Actor)pUserClass.pPointer,pAgentTree, pNode);
			}
			case 56344714://GetFinalEulerAngle
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 0) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_GetFinalEulerAngle((Actor)pUserClass.pPointer,pAgentTree, pNode);
			}
			case -1061891926://SetEulerAngle
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 2) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_SetEulerAngle((Actor)pUserClass.pPointer,pAgentTree.GetInportVec3(pNode,1),pAgentTree.GetInportBool(pNode,2));
			}
			case -1374303795://GetMatrix
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 0) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_GetMatrix((Actor)pUserClass.pPointer,pAgentTree, pNode);
			}
			case -181775319://RunTo
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 4) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_RunTo((Actor)pUserClass.pPointer,pAgentTree.GetInportVec3(pNode,1),pAgentTree.GetInportFloat(pNode,2),pAgentTree.GetInportBool(pNode,3),pAgentTree.GetInportBool(pNode,4), pAgentTree, pNode);
			}
			case -29192949://NavRunTo
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 4) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_NavRunTo((Actor)pUserClass.pPointer,pAgentTree.GetInportVec3(pNode,1),pAgentTree.GetInportFloat(pNode,2),pAgentTree.GetInportBool(pNode,3),pAgentTree.GetInportBool(pNode,4));
			}
			case -1665969306://StopRunAlongPathPoint
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 0) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_StopRunAlongPathPoint((Actor)pUserClass.pPointer);
			}
			case -234109610://IsRunAlongPathPlaying
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 0) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_IsRunAlongPathPlaying((Actor)pUserClass.pPointer,pAgentTree, pNode);
			}
			case 809655427://GetBound
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 0) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_GetBound((Actor)pUserClass.pPointer,pAgentTree, pNode);
			}
			case -927767726://SetBound
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 2) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_SetBound((Actor)pUserClass.pPointer,pAgentTree.GetInportVec3(pNode,1),pAgentTree.GetInportVec3(pNode,2));
			}
			case 327632054://SetDirection
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 3) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_SetDirection((Actor)pUserClass.pPointer,pAgentTree.GetInportVec3(pNode,1),pAgentTree.GetInportFloat(pNode,2),pAgentTree.GetInportBool(pNode,3));
			}
			case 479826485://GetDirection
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 0) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_GetDirection((Actor)pUserClass.pPointer,pAgentTree, pNode);
			}
			case -266746549://GetFinalDirection
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 0) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_GetFinalDirection((Actor)pUserClass.pPointer,pAgentTree, pNode);
			}
			case -2003762858://GetUp
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 0) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_GetUp((Actor)pUserClass.pPointer,pAgentTree, pNode);
			}
			case 1387341846://GetFinalUp
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 0) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_GetFinalUp((Actor)pUserClass.pPointer,pAgentTree, pNode);
			}
			case 502345748://SetUp
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 3) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_SetUp((Actor)pUserClass.pPointer,pAgentTree.GetInportVec3(pNode,1),pAgentTree.GetInportFloat(pNode,2),pAgentTree.GetInportBool(pNode,3));
			}
			case 562451489://GetRight
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 0) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_GetRight((Actor)pUserClass.pPointer,pAgentTree, pNode);
			}
			case 775185944://GetFinalRight
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 0) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_GetFinalRight((Actor)pUserClass.pPointer,pAgentTree, pNode);
			}
			case 2030699697://GetScale
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 0) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_GetScale((Actor)pUserClass.pPointer,pAgentTree, pNode);
			}
			case -2114241696://SetScale
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 1) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_SetScale((Actor)pUserClass.pPointer,pAgentTree.GetInportVec3(pNode,1));
			}
			case -1309849449://SetTransfrom
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 3) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_SetTransfrom((Actor)pUserClass.pPointer,pAgentTree.GetInportVec3(pNode,1),pAgentTree.GetInportVec3(pNode,2),pAgentTree.GetInportVec3(pNode,3));
			}
			case -322938593://IsFlag
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 1) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_IsFlag((Actor)pUserClass.pPointer,(Framework.ActorSystem.Runtime.EActorFlag)pAgentTree.GetInportInt(pNode,1), pAgentTree, pNode);
			}
			case -1324424049://SetFlag
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 2) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_SetFlag((Actor)pUserClass.pPointer,(Framework.ActorSystem.Runtime.EActorFlag)pAgentTree.GetInportInt(pNode,1),pAgentTree.GetInportBool(pNode,2));
			}
			case -906116501://IsCanLogic
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 0) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_IsCanLogic((Actor)pUserClass.pPointer,pAgentTree, pNode);
			}
			case 1288257277://IsInvincible
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 0) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_IsInvincible((Actor)pUserClass.pPointer,pAgentTree, pNode);
			}
			case 1288702801://SetDelayDestroy
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 1) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_SetDelayDestroy((Actor)pUserClass.pPointer,pAgentTree.GetInportFloat(pNode,1));
			}
			case -2100366979://GetDelayDestroy
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 0) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_GetDelayDestroy((Actor)pUserClass.pPointer,pAgentTree, pNode);
			}
			case -1135091635://SetDestroy
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 0) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_SetDestroy((Actor)pUserClass.pPointer);
			}
			case 1827891106://IsDestroy
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 0) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_IsDestroy((Actor)pUserClass.pPointer,pAgentTree, pNode);
			}
			case 242724015://EnableHudBar
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 1) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_EnableHudBar((Actor)pUserClass.pPointer,pAgentTree.GetInportBool(pNode,1));
			}
			case 1509444428://IsEnableHudBar
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 0) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_IsEnableHudBar((Actor)pUserClass.pPointer,pAgentTree, pNode);
			}
			case -1409441560://IsColliderAble
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 0) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_IsColliderAble((Actor)pUserClass.pPointer,pAgentTree, pNode);
			}
			case -2102292443://SetKilled
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 1) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_SetKilled((Actor)pUserClass.pPointer,pAgentTree.GetInportBool(pNode,1));
			}
			case 1176537280://IsKilled
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 0) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_IsKilled((Actor)pUserClass.pPointer,pAgentTree, pNode);
			}
			case -1163030929://SetVisible
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 1) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_SetVisible((Actor)pUserClass.pPointer,pAgentTree.GetInportBool(pNode,1));
			}
			case 1778977152://IsVisible
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 0) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_IsVisible((Actor)pUserClass.pPointer,pAgentTree, pNode);
			}
			case -1232776406://SetActived
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 1) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_SetActived((Actor)pUserClass.pPointer,pAgentTree.GetInportBool(pNode,1));
			}
			case 1713493189://IsActived
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 0) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_IsActived((Actor)pUserClass.pPointer,pAgentTree, pNode);
			}
			case 1725364953://EnableLogic
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 1) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_EnableLogic((Actor)pUserClass.pPointer,pAgentTree.GetInportBool(pNode,1));
			}
			case -973878293://IsLogicEnable
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 0) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_IsLogicEnable((Actor)pUserClass.pPointer,pAgentTree, pNode);
			}
			case -725487424://EnableAI
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 1) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_EnableAI((Actor)pUserClass.pPointer,pAgentTree.GetInportBool(pNode,1));
			}
			case -1904103641://IsEnableAI
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 0) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_IsEnableAI((Actor)pUserClass.pPointer,pAgentTree, pNode);
			}
			case 1199932339://EnableRVO
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 1) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_EnableRVO((Actor)pUserClass.pPointer,pAgentTree.GetInportBool(pNode,1));
			}
			case 2041689143://IsEnableRVO
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 0) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_IsEnableRVO((Actor)pUserClass.pPointer,pAgentTree, pNode);
			}
			case -112856003://StartActionState
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 4) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_StartActionState((Actor)pUserClass.pPointer,(Framework.ActorSystem.Runtime.EActionStateType)pAgentTree.GetInportInt(pNode,1),pAgentTree.GetInportUint(pNode,2),pAgentTree.GetInportBool(pNode,3),pAgentTree.GetInportUserData<Framework.ActorSystem.Runtime.IContextData>(pNode,4));
			}
			case -48442208://StartActionState
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 3) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_StartActionState_1((Actor)pUserClass.pPointer,pAgentTree.GetInportUint(pNode,1),pAgentTree.GetInportBool(pNode,2),pAgentTree.GetInportUserData<Framework.ActorSystem.Runtime.IContextData>(pNode,3));
			}
			case -82564754://StopActionState
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 2) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_StopActionState((Actor)pUserClass.pPointer,(Framework.ActorSystem.Runtime.EActionStateType)pAgentTree.GetInportInt(pNode,1),pAgentTree.GetInportUint(pNode,2));
			}
			case -1719976999://SetIdleType
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 2) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_SetIdleType((Actor)pUserClass.pPointer,(Framework.ActorSystem.Runtime.EActionStateType)pAgentTree.GetInportInt(pNode,1),pAgentTree.GetInportUint(pNode,2));
			}
			case -151935711://SetRunType
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 2) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_SetRunType((Actor)pUserClass.pPointer,(Framework.ActorSystem.Runtime.EActionStateType)pAgentTree.GetInportInt(pNode,1),pAgentTree.GetInportUint(pNode,2));
			}
			case 207081506://RemoveActionState
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 2) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_RemoveActionState((Actor)pUserClass.pPointer,(Framework.ActorSystem.Runtime.EActionStateType)pAgentTree.GetInportInt(pNode,1),pAgentTree.GetInportUint(pNode,2));
			}
			case 1686383617://SetAttr
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 2) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_SetAttr((Actor)pUserClass.pPointer,pAgentTree.GetInportByte(pNode,1),pAgentTree.GetInportFloat(pNode,2));
			}
			case -149580580://GetAttr
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 2) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_GetAttr((Actor)pUserClass.pPointer,pAgentTree.GetInportByte(pNode,1),pAgentTree.GetInportFloat(pNode,2), pAgentTree, pNode);
			}
			case 86650515://RemoveAttr
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 1) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_RemoveAttr((Actor)pUserClass.pPointer,pAgentTree.GetInportByte(pNode,1));
			}
			case -1093680716://AppendAttr
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 2) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_AppendAttr((Actor)pUserClass.pPointer,pAgentTree.GetInportByte(pNode,1),pAgentTree.GetInportFloat(pNode,2));
			}
			case -1997386568://SubAttr
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 3) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_SubAttr((Actor)pUserClass.pPointer,pAgentTree.GetInportByte(pNode,1),pAgentTree.GetInportInt(pNode,2),pAgentTree.GetInportBool(pNode,3));
			}
			case -894122339://ClearAttrs
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 0) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_ClearAttrs((Actor)pUserClass.pPointer);
			}
			case 2015276513://IsInAction
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 1) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_IsInAction((Actor)pUserClass.pPointer,(Framework.ActorSystem.Runtime.EActionStateType)pAgentTree.GetInportInt(pNode,1), pAgentTree, pNode);
			}
			case -464428257://GetSkillSystem
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 0) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_GetSkillSystem((Actor)pUserClass.pPointer,pAgentTree, pNode);
			}
			case -2073512697://IsAttacking
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 0) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_IsAttacking((Actor)pUserClass.pPointer,pAgentTree, pNode);
			}
			case -448329711://IsCutscneHolded
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 0) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_IsCutscneHolded((Actor)pUserClass.pPointer,pAgentTree, pNode);
			}
			case 2026433217://ResetFreeze
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 0) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_ResetFreeze((Actor)pUserClass.pPointer);
			}
			case 817905509://Freezed
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 2) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_Freezed((Actor)pUserClass.pPointer,pAgentTree.GetInportBool(pNode,1),pAgentTree.GetInportFloat(pNode,2));
			}
			case 1967178651://IsFreezed
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 0) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_IsFreezed((Actor)pUserClass.pPointer,pAgentTree, pNode);
			}
			case -1225831306://IsIntersecition
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 1) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_IsIntersecition((Actor)pUserClass.pPointer,pAgentTree.GetInportUserData<Framework.ActorSystem.Runtime.Actor>(pNode,1), pAgentTree, pNode);
			}
			case 724252282://IsIntersecition
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 3) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_IsIntersecition_1((Actor)pUserClass.pPointer,pAgentTree.GetInportMatrix(pNode,1),pAgentTree.GetInportVec3(pNode,2),pAgentTree.GetInportVec3(pNode,3), pAgentTree, pNode);
			}
			case -1306368064://IsIntersecition
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 2) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_IsIntersecition_2((Actor)pUserClass.pPointer,pAgentTree.GetInportMatrix(pNode,1),pAgentTree.GetInportFloat(pNode,2), pAgentTree, pNode);
			}
			case 239105233://GetEventBindSlot
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 2) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_GetEventBindSlot((Actor)pUserClass.pPointer,pAgentTree.GetInportString(pNode,1),pAgentTree.GetInportInt(pNode,2), pAgentTree, pNode);
			}
			}
			return true;
		}
	}
}

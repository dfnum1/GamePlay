//auto generated
using Framework.AT.Runtime;
namespace Framework.ActorSystem.Runtime
{
#if UNITY_EDITOR
	[ATClass("Actor")]
#endif
	public class Framework_ActorSystem_Runtime_Actor
	{
#if UNITY_EDITOR
		[ATFunction(381027485,"GetInstanceID",typeof(Framework.ActorSystem.Runtime.Actor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Actor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
		[ATFunctionReturn(typeof(Framework.AT.Runtime.VariableInt), "pReturn", null,typeof(System.Int32))]
#endif
		static bool AT_GetInstanceID(Actor pPointerThis,AgentTree pAgentTree, BaseNode pNode)
		{
			pAgentTree.SetOutportInt(pNode, 0, pPointerThis.GetInstanceID());
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(312098358,"获取攻击组",typeof(Framework.ActorSystem.Runtime.Actor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Actor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
		[ATFunctionReturn(typeof(Framework.AT.Runtime.VariableInt), "pReturn", null,typeof(System.Byte))]
#endif
		static bool AT_GetAttackGroup(Actor pPointerThis,AgentTree pAgentTree, BaseNode pNode)
		{
			pAgentTree.SetOutportByte(pNode, 0, pPointerThis.GetAttackGroup());
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(1837403457,"设置攻击组",typeof(Framework.ActorSystem.Runtime.Actor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Actor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableInt),"attackGroup",false, null,typeof(System.Byte))]
#endif
		static bool AT_SetAttackGroup(Actor pPointerThis,System.Byte attackGroup)
		{
			pPointerThis.SetAttackGroup(attackGroup);
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(765517350,"是否可攻击",typeof(Framework.ActorSystem.Runtime.Actor),false)]
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
		[ATFunction(-955549743,"设置速度",typeof(Framework.ActorSystem.Runtime.Actor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Actor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableVec3),"speed",false, null,typeof(UnityEngine.Vector3))]
#endif
		static bool AT_SetSpeed(Actor pPointerThis,UnityEngine.Vector3 speed)
		{
			pPointerThis.SetSpeed(speed);
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(-297293181,"设置速度XZ",typeof(Framework.ActorSystem.Runtime.Actor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Actor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableVec3),"speed",false, null,typeof(UnityEngine.Vector3))]
#endif
		static bool AT_SetSpeedXZ(Actor pPointerThis,UnityEngine.Vector3 speed)
		{
			pPointerThis.SetSpeedXZ(speed);
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(1073367040,"获取当前速度",typeof(Framework.ActorSystem.Runtime.Actor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Actor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
		[ATFunctionReturn(typeof(Framework.AT.Runtime.VariableVec3), "pReturn", null,typeof(UnityEngine.Vector3))]
#endif
		static bool AT_GetSpeed(Actor pPointerThis,AgentTree pAgentTree, BaseNode pNode)
		{
			pAgentTree.SetOutportVec3(pNode, 0, pPointerThis.GetSpeed());
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(833023976,"设置Y速度",typeof(Framework.ActorSystem.Runtime.Actor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Actor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableFloat),"fSpeedY",false, null,typeof(System.Single))]
#endif
		static bool AT_SetSpeedY(Actor pPointerThis,System.Single fSpeedY)
		{
			pPointerThis.SetSpeedY(fSpeedY);
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(1824765372,"设置摩檫力",typeof(Framework.ActorSystem.Runtime.Actor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Actor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableFloat),"fValue",false, null,typeof(System.Single))]
#endif
		static bool AT_SetFraction(Actor pPointerThis,System.Single fValue)
		{
			pPointerThis.SetFraction(fValue);
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(1928069877,"获取摩檫力",typeof(Framework.ActorSystem.Runtime.Actor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Actor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
		[ATFunctionReturn(typeof(Framework.AT.Runtime.VariableFloat), "pReturn", null,typeof(System.Single))]
#endif
		static bool AT_GetFraction(Actor pPointerThis,AgentTree pAgentTree, BaseNode pNode)
		{
			pAgentTree.SetOutportFloat(pNode, 0, pPointerThis.GetFraction());
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(964859665,"启用重力",typeof(Framework.ActorSystem.Runtime.Actor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Actor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableBool),"bEnable",false, null,typeof(System.Boolean))]
#endif
		static bool AT_EnableGravity(Actor pPointerThis,System.Boolean bEnable)
		{
			pPointerThis.EnableGravity(bEnable);
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(347890795,"设置重力",typeof(Framework.ActorSystem.Runtime.Actor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Actor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableFloat),"fValue",false, null,typeof(System.Single))]
#endif
		static bool AT_SetGravity(Actor pPointerThis,System.Single fValue)
		{
			pPointerThis.SetGravity(fValue);
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(989414905,"获取重力",typeof(Framework.ActorSystem.Runtime.Actor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Actor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
		[ATFunctionReturn(typeof(Framework.AT.Runtime.VariableFloat), "pReturn", null,typeof(System.Single))]
#endif
		static bool AT_GetGravity(Actor pPointerThis,AgentTree pAgentTree, BaseNode pNode)
		{
			pAgentTree.SetOutportFloat(pNode, 0, pPointerThis.GetGravity());
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(-895517594,"获取当前位置",typeof(Framework.ActorSystem.Runtime.Actor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Actor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
		[ATFunctionReturn(typeof(Framework.AT.Runtime.VariableVec3), "pReturn", null,typeof(UnityEngine.Vector3))]
#endif
		static bool AT_GetPosition(Actor pPointerThis,AgentTree pAgentTree, BaseNode pNode)
		{
			pAgentTree.SetOutportVec3(pNode, 0, pPointerThis.GetPosition());
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(-628240668,"获取上一次位置",typeof(Framework.ActorSystem.Runtime.Actor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Actor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
		[ATFunctionReturn(typeof(Framework.AT.Runtime.VariableVec3), "pReturn", null,typeof(UnityEngine.Vector3))]
#endif
		static bool AT_GetLastPosition(Actor pPointerThis,AgentTree pAgentTree, BaseNode pNode)
		{
			pAgentTree.SetOutportVec3(pNode, 0, pPointerThis.GetLastPosition());
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(-726611153,"设置位置",typeof(Framework.ActorSystem.Runtime.Actor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Actor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableVec3),"vPosition",false, null,typeof(UnityEngine.Vector3))]
#endif
		static bool AT_SetPosition(Actor pPointerThis,UnityEngine.Vector3 vPosition)
		{
			pPointerThis.SetPosition(vPosition);
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(-1338512524,"位置偏移",typeof(Framework.ActorSystem.Runtime.Actor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Actor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableVec3),"vOffset",false, null,typeof(UnityEngine.Vector3))]
#endif
		static bool AT_OffsetPosition(Actor pPointerThis,UnityEngine.Vector3 vOffset)
		{
			pPointerThis.OffsetPosition(vOffset);
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(-712341336,"获取角度",typeof(Framework.ActorSystem.Runtime.Actor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Actor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
		[ATFunctionReturn(typeof(Framework.AT.Runtime.VariableVec3), "pReturn", null,typeof(UnityEngine.Vector3))]
#endif
		static bool AT_GetEulerAngle(Actor pPointerThis,AgentTree pAgentTree, BaseNode pNode)
		{
			pAgentTree.SetOutportVec3(pNode, 0, pPointerThis.GetEulerAngle());
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(-1590410012,"设置角度",typeof(Framework.ActorSystem.Runtime.Actor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Actor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableVec3),"vEulerAngle",false, null,typeof(UnityEngine.Vector3))]
#endif
		static bool AT_SetEulerAngle(Actor pPointerThis,UnityEngine.Vector3 vEulerAngle)
		{
			pPointerThis.SetEulerAngle(vEulerAngle);
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(-1394750896,"获取矩阵",typeof(Framework.ActorSystem.Runtime.Actor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Actor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
		[ATFunctionReturn(typeof(Framework.AT.Runtime.VariableMatrix), "pReturn", null,typeof(UnityEngine.Matrix4x4))]
#endif
		static bool AT_GetMatrix(Actor pPointerThis,AgentTree pAgentTree, BaseNode pNode)
		{
			pAgentTree.SetOutportMatrix(pNode, 0, pPointerThis.GetMatrix());
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(1490974294,"设置方向",typeof(Framework.ActorSystem.Runtime.Actor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Actor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableVec3),"vDirection",false, null,typeof(UnityEngine.Vector3))]
#endif
		static bool AT_SetDirection(Actor pPointerThis,UnityEngine.Vector3 vDirection)
		{
			pPointerThis.SetDirection(vDirection);
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(1472241877,"获取方向",typeof(Framework.ActorSystem.Runtime.Actor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Actor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
		[ATFunctionReturn(typeof(Framework.AT.Runtime.VariableVec3), "pReturn", null,typeof(UnityEngine.Vector3))]
#endif
		static bool AT_GetDirection(Actor pPointerThis,AgentTree pAgentTree, BaseNode pNode)
		{
			pAgentTree.SetOutportVec3(pNode, 0, pPointerThis.GetDirection());
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(1098330067,"获取Up朝向",typeof(Framework.ActorSystem.Runtime.Actor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Actor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
		[ATFunctionReturn(typeof(Framework.AT.Runtime.VariableVec3), "pReturn", null,typeof(UnityEngine.Vector3))]
#endif
		static bool AT_GetUp(Actor pPointerThis,AgentTree pAgentTree, BaseNode pNode)
		{
			pAgentTree.SetOutportVec3(pNode, 0, pPointerThis.GetUp());
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(-736682351,"设置Up朝向",typeof(Framework.ActorSystem.Runtime.Actor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Actor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableVec3),"up",false, null,typeof(UnityEngine.Vector3))]
#endif
		static bool AT_SetUp(Actor pPointerThis,UnityEngine.Vector3 up)
		{
			pPointerThis.SetUp(up);
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(-2078882846,"获取Right朝向",typeof(Framework.ActorSystem.Runtime.Actor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Actor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
		[ATFunctionReturn(typeof(Framework.AT.Runtime.VariableVec3), "pReturn", null,typeof(UnityEngine.Vector3))]
#endif
		static bool AT_GetRight(Actor pPointerThis,AgentTree pAgentTree, BaseNode pNode)
		{
			pAgentTree.SetOutportVec3(pNode, 0, pPointerThis.GetRight());
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(-593849486,"获取缩放",typeof(Framework.ActorSystem.Runtime.Actor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Actor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
		[ATFunctionReturn(typeof(Framework.AT.Runtime.VariableVec3), "pReturn", null,typeof(UnityEngine.Vector3))]
#endif
		static bool AT_GetScale(Actor pPointerThis,AgentTree pAgentTree, BaseNode pNode)
		{
			pAgentTree.SetOutportVec3(pNode, 0, pPointerThis.GetScale());
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(611036323,"设置缩放",typeof(Framework.ActorSystem.Runtime.Actor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Actor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableVec3),"scale",false, null,typeof(UnityEngine.Vector3))]
#endif
		static bool AT_SetScale(Actor pPointerThis,UnityEngine.Vector3 scale)
		{
			pPointerThis.SetScale(scale);
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(-88835465,"设置位置角度缩放",typeof(Framework.ActorSystem.Runtime.Actor),false)]
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
		[ATFunction(-375119552,"是否拥有标志",typeof(Framework.ActorSystem.Runtime.Actor),false)]
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
		[ATFunction(724323496,"设置标志",typeof(Framework.ActorSystem.Runtime.Actor),false)]
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
		[ATFunction(-1337224865,"是否开启逻辑",typeof(Framework.ActorSystem.Runtime.Actor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Actor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
		[ATFunctionReturn(typeof(Framework.AT.Runtime.VariableBool), "pReturn", null,typeof(System.Boolean))]
#endif
		static bool AT_IsCanLogic(Actor pPointerThis,AgentTree pAgentTree, BaseNode pNode)
		{
			pAgentTree.SetOutportBool(pNode, 0, pPointerThis.IsCanLogic());
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(-902001298,"是否不可见",typeof(Framework.ActorSystem.Runtime.Actor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Actor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
		[ATFunctionReturn(typeof(Framework.AT.Runtime.VariableBool), "pReturn", null,typeof(System.Boolean))]
#endif
		static bool AT_IsInvincible(Actor pPointerThis,AgentTree pAgentTree, BaseNode pNode)
		{
			pAgentTree.SetOutportBool(pNode, 0, pPointerThis.IsInvincible());
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(-716677369,"设置延迟删除时间",typeof(Framework.ActorSystem.Runtime.Actor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Actor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableFloat),"fTime",false, null,typeof(System.Single))]
#endif
		static bool AT_SetDelayDestroy(Actor pPointerThis,System.Single fTime)
		{
			pPointerThis.SetDelayDestroy(fTime);
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(458663211,"获取延迟删除时间",typeof(Framework.ActorSystem.Runtime.Actor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Actor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
		[ATFunctionReturn(typeof(Framework.AT.Runtime.VariableFloat), "pReturn", null,typeof(System.Single))]
#endif
		static bool AT_GetDelayDestroy(Actor pPointerThis,AgentTree pAgentTree, BaseNode pNode)
		{
			pAgentTree.SetOutportFloat(pNode, 0, pPointerThis.GetDelayDestroy());
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(854312887,"删除",typeof(Framework.ActorSystem.Runtime.Actor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Actor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
#endif
		static bool AT_SetDestroy(Actor pPointerThis)
		{
			pPointerThis.SetDestroy();
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(-1507174251,"是否删除",typeof(Framework.ActorSystem.Runtime.Actor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Actor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
		[ATFunctionReturn(typeof(Framework.AT.Runtime.VariableBool), "pReturn", null,typeof(System.Boolean))]
#endif
		static bool AT_IsDestroy(Actor pPointerThis,AgentTree pAgentTree, BaseNode pNode)
		{
			pAgentTree.SetOutportBool(pNode, 0, pPointerThis.IsDestroy());
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(-130130118,"HUD开关",typeof(Framework.ActorSystem.Runtime.Actor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Actor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableBool),"bHudBar",false, null,typeof(System.Boolean))]
#endif
		static bool AT_EnableHudBar(Actor pPointerThis,System.Boolean bHudBar)
		{
			pPointerThis.EnableHudBar(bHudBar);
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(1847440768,"是否开启HUD",typeof(Framework.ActorSystem.Runtime.Actor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Actor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
		[ATFunctionReturn(typeof(Framework.AT.Runtime.VariableBool), "pReturn", null,typeof(System.Boolean))]
#endif
		static bool AT_IsEnableHudBar(Actor pPointerThis,AgentTree pAgentTree, BaseNode pNode)
		{
			pAgentTree.SetOutportBool(pNode, 0, pPointerThis.IsEnableHudBar());
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(63681228,"是否拥有物理碰撞",typeof(Framework.ActorSystem.Runtime.Actor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Actor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
		[ATFunctionReturn(typeof(Framework.AT.Runtime.VariableBool), "pReturn", null,typeof(System.Boolean))]
#endif
		static bool AT_IsColliderAble(Actor pPointerThis,AgentTree pAgentTree, BaseNode pNode)
		{
			pAgentTree.SetOutportBool(pNode, 0, pPointerThis.IsColliderAble());
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(-2139515976,"SetKilled",typeof(Framework.ActorSystem.Runtime.Actor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Actor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableBool),"bKilled",false, null,typeof(System.Boolean))]
#endif
		static bool AT_SetKilled(Actor pPointerThis,System.Boolean bKilled)
		{
			pPointerThis.SetKilled(bKilled);
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(635254065,"IsKilled",typeof(Framework.ActorSystem.Runtime.Actor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Actor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
		[ATFunctionReturn(typeof(Framework.AT.Runtime.VariableBool), "pReturn", null,typeof(System.Boolean))]
#endif
		static bool AT_IsKilled(Actor pPointerThis,AgentTree pAgentTree, BaseNode pNode)
		{
			pAgentTree.SetOutportBool(pNode, 0, pPointerThis.IsKilled());
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(873572757,"SetVisible",typeof(Framework.ActorSystem.Runtime.Actor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Actor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableBool),"bVisible",false, null,typeof(System.Boolean))]
#endif
		static bool AT_SetVisible(Actor pPointerThis,System.Boolean bVisible)
		{
			pPointerThis.SetVisible(bVisible);
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(-1596967241,"IsVisible",typeof(Framework.ActorSystem.Runtime.Actor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Actor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
		[ATFunctionReturn(typeof(Framework.AT.Runtime.VariableBool), "pReturn", null,typeof(System.Boolean))]
#endif
		static bool AT_IsVisible(Actor pPointerThis,AgentTree pAgentTree, BaseNode pNode)
		{
			pAgentTree.SetOutportBool(pNode, 0, pPointerThis.IsVisible());
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(943289552,"SetActived",typeof(Framework.ActorSystem.Runtime.Actor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Actor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableBool),"bToggle",false, null,typeof(System.Boolean))]
#endif
		static bool AT_SetActived(Actor pPointerThis,System.Boolean bToggle)
		{
			pPointerThis.SetActived(bToggle);
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(-1392972814,"IsActived",typeof(Framework.ActorSystem.Runtime.Actor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Actor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
		[ATFunctionReturn(typeof(Framework.AT.Runtime.VariableBool), "pReturn", null,typeof(System.Boolean))]
#endif
		static bool AT_IsActived(Actor pPointerThis,AgentTree pAgentTree, BaseNode pNode)
		{
			pAgentTree.SetOutportBool(pNode, 0, pPointerThis.IsActived());
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(59183278,"EnableLogic",typeof(Framework.ActorSystem.Runtime.Actor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Actor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableBool),"bToggle",false, null,typeof(System.Boolean))]
#endif
		static bool AT_EnableLogic(Actor pPointerThis,System.Boolean bToggle)
		{
			pPointerThis.EnableLogic(bToggle);
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(511331437,"EnableAI",typeof(Framework.ActorSystem.Runtime.Actor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Actor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableBool),"bToggle",false, null,typeof(System.Boolean))]
#endif
		static bool AT_EnableAI(Actor pPointerThis,System.Boolean bToggle)
		{
			pPointerThis.EnableAI(bToggle);
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(-1742940291,"IsEnableAI",typeof(Framework.ActorSystem.Runtime.Actor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Actor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
		[ATFunctionReturn(typeof(Framework.AT.Runtime.VariableBool), "pReturn", null,typeof(System.Boolean))]
#endif
		static bool AT_IsEnableAI(Actor pPointerThis,AgentTree pAgentTree, BaseNode pNode)
		{
			pAgentTree.SetOutportBool(pNode, 0, pPointerThis.IsEnableAI());
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(-733558894,"EnableRVO",typeof(Framework.ActorSystem.Runtime.Actor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Actor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableBool),"bToggle",false, null,typeof(System.Boolean))]
#endif
		static bool AT_EnableRVO(Actor pPointerThis,System.Boolean bToggle)
		{
			pPointerThis.EnableRVO(bToggle);
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(-914902675,"IsEnableRVO",typeof(Framework.ActorSystem.Runtime.Actor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Actor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
		[ATFunctionReturn(typeof(Framework.AT.Runtime.VariableBool), "pReturn", null,typeof(System.Boolean))]
#endif
		static bool AT_IsEnableRVO(Actor pPointerThis,AgentTree pAgentTree, BaseNode pNode)
		{
			pAgentTree.SetOutportBool(pNode, 0, pPointerThis.IsEnableRVO());
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(1988170200,"StartActionState",typeof(Framework.ActorSystem.Runtime.Actor),false)]
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
		[ATFunction(242567738,"StartActionState_1",typeof(Framework.ActorSystem.Runtime.Actor),false)]
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
		[ATFunction(-228769905,"StopActionState",typeof(Framework.ActorSystem.Runtime.Actor),false)]
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
		[ATFunction(1689203650,"SetIdleType",typeof(Framework.ActorSystem.Runtime.Actor),false)]
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
		[ATFunction(-2072478675,"RemoveActionState",typeof(Framework.ActorSystem.Runtime.Actor),false)]
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
		[ATFunction(-22641626,"SetAttr",typeof(Framework.ActorSystem.Runtime.Actor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Actor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableInt),"type",false, null,typeof(System.Byte))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableFloat),"value",false, null,typeof(System.Single))]
#endif
		static bool AT_SetAttr(Actor pPointerThis,System.Byte type,System.Single value)
		{
			pPointerThis.SetAttr(type,value);
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(1832336635,"GetAttr",typeof(Framework.ActorSystem.Runtime.Actor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Actor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableInt),"type",false, null,typeof(System.Byte))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableFloat),"defVal",false, null,typeof(System.Single))]
		[ATFunctionReturn(typeof(Framework.AT.Runtime.VariableFloat), "pReturn", null,typeof(System.Single))]
#endif
		static bool AT_GetAttr(Actor pPointerThis,System.Byte type,System.Single defVal,AgentTree pAgentTree, BaseNode pNode)
		{
			pAgentTree.SetOutportFloat(pNode, 0, pPointerThis.GetAttr(type,defVal));
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(-468082741,"RemoveAttr",typeof(Framework.ActorSystem.Runtime.Actor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Actor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableInt),"type",false, null,typeof(System.Byte))]
#endif
		static bool AT_RemoveAttr(Actor pPointerThis,System.Byte type)
		{
			pPointerThis.RemoveAttr(type);
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(1610379500,"AppendAttr",typeof(Framework.ActorSystem.Runtime.Actor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Actor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableInt),"type",false, null,typeof(System.Byte))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableFloat),"value",false, null,typeof(System.Single))]
#endif
		static bool AT_AppendAttr(Actor pPointerThis,System.Byte type,System.Single value)
		{
			pPointerThis.AppendAttr(type,value);
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(315679903,"SubAttr",typeof(Framework.ActorSystem.Runtime.Actor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Actor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableInt),"type",false, null,typeof(System.Byte))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableInt),"value",false, null,typeof(System.Int32))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableBool),"bLowerZero",false, null,typeof(System.Boolean))]
#endif
		static bool AT_SubAttr(Actor pPointerThis,System.Byte type,System.Int32 value,System.Boolean bLowerZero)
		{
			pPointerThis.SubAttr(type,value,bLowerZero);
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(1249179647,"ClearAttrs",typeof(Framework.ActorSystem.Runtime.Actor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Actor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
#endif
		static bool AT_ClearAttrs(Actor pPointerThis)
		{
			pPointerThis.ClearAttrs();
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(-943336217,"IsInAction",typeof(Framework.ActorSystem.Runtime.Actor),false)]
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
		[ATFunction(-1576406313,"IsAttacking",typeof(Framework.ActorSystem.Runtime.Actor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Actor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
		[ATFunctionReturn(typeof(Framework.AT.Runtime.VariableBool), "pReturn", null,typeof(System.Boolean))]
#endif
		static bool AT_IsAttacking(Actor pPointerThis,AgentTree pAgentTree, BaseNode pNode)
		{
			pAgentTree.SetOutportBool(pNode, 0, pPointerThis.IsAttacking());
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(528609504,"IsCutscneHolded",typeof(Framework.ActorSystem.Runtime.Actor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Actor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
		[ATFunctionReturn(typeof(Framework.AT.Runtime.VariableBool), "pReturn", null,typeof(System.Boolean))]
#endif
		static bool AT_IsCutscneHolded(Actor pPointerThis,AgentTree pAgentTree, BaseNode pNode)
		{
			pAgentTree.SetOutportBool(pNode, 0, pPointerThis.IsCutscneHolded());
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(-604755324,"ResetFreeze",typeof(Framework.ActorSystem.Runtime.Actor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Actor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
#endif
		static bool AT_ResetFreeze(Actor pPointerThis)
		{
			pPointerThis.ResetFreeze();
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(181418108,"Freezed",typeof(Framework.ActorSystem.Runtime.Actor),false)]
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
		[ATFunction(-1080427348,"IsFreezed",typeof(Framework.ActorSystem.Runtime.Actor),false)]
		[ATFunctionArgv(typeof(VariableUserData),"Actor",false, null,typeof(Framework.ActorSystem.Runtime.Actor))]
		[ATFunctionReturn(typeof(Framework.AT.Runtime.VariableBool), "pReturn", null,typeof(System.Boolean))]
#endif
		static bool AT_IsFreezed(Actor pPointerThis,AgentTree pAgentTree, BaseNode pNode)
		{
			pAgentTree.SetOutportBool(pNode, 0, pPointerThis.IsFreezed());
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
			case 381027485://GetInstanceID
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 0) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_GetInstanceID(pUserClass.pPointer as Actor,pAgentTree, pNode);
			}
			case 312098358://GetAttackGroup
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 0) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_GetAttackGroup(pUserClass.pPointer as Actor,pAgentTree, pNode);
			}
			case 1837403457://SetAttackGroup
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 1) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_SetAttackGroup(pUserClass.pPointer as Actor,pAgentTree.GetInportByte(pNode,1));
			}
			case 765517350://CanAttackGroup
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 1) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_CanAttackGroup(pUserClass.pPointer as Actor,pAgentTree.GetInportByte(pNode,1), pAgentTree, pNode);
			}
			case -955549743://SetSpeed
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 1) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_SetSpeed(pUserClass.pPointer as Actor,pAgentTree.GetInportVec3(pNode,1));
			}
			case -297293181://SetSpeedXZ
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 1) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_SetSpeedXZ(pUserClass.pPointer as Actor,pAgentTree.GetInportVec3(pNode,1));
			}
			case 1073367040://GetSpeed
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 0) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_GetSpeed(pUserClass.pPointer as Actor,pAgentTree, pNode);
			}
			case 833023976://SetSpeedY
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 1) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_SetSpeedY(pUserClass.pPointer as Actor,pAgentTree.GetInportFloat(pNode,1));
			}
			case 1824765372://SetFraction
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 1) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_SetFraction(pUserClass.pPointer as Actor,pAgentTree.GetInportFloat(pNode,1));
			}
			case 1928069877://GetFraction
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 0) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_GetFraction(pUserClass.pPointer as Actor,pAgentTree, pNode);
			}
			case 964859665://EnableGravity
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 1) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_EnableGravity(pUserClass.pPointer as Actor,pAgentTree.GetInportBool(pNode,1));
			}
			case 347890795://SetGravity
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 1) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_SetGravity(pUserClass.pPointer as Actor,pAgentTree.GetInportFloat(pNode,1));
			}
			case 989414905://GetGravity
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 0) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_GetGravity(pUserClass.pPointer as Actor,pAgentTree, pNode);
			}
			case -895517594://GetPosition
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 0) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_GetPosition(pUserClass.pPointer as Actor,pAgentTree, pNode);
			}
			case -628240668://GetLastPosition
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 0) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_GetLastPosition(pUserClass.pPointer as Actor,pAgentTree, pNode);
			}
			case -726611153://SetPosition
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 1) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_SetPosition(pUserClass.pPointer as Actor,pAgentTree.GetInportVec3(pNode,1));
			}
			case -1338512524://OffsetPosition
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 1) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_OffsetPosition(pUserClass.pPointer as Actor,pAgentTree.GetInportVec3(pNode,1));
			}
			case -712341336://GetEulerAngle
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 0) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_GetEulerAngle(pUserClass.pPointer as Actor,pAgentTree, pNode);
			}
			case -1590410012://SetEulerAngle
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 1) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_SetEulerAngle(pUserClass.pPointer as Actor,pAgentTree.GetInportVec3(pNode,1));
			}
			case -1394750896://GetMatrix
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 0) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_GetMatrix(pUserClass.pPointer as Actor,pAgentTree, pNode);
			}
			case 1490974294://SetDirection
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 1) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_SetDirection(pUserClass.pPointer as Actor,pAgentTree.GetInportVec3(pNode,1));
			}
			case 1472241877://GetDirection
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 0) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_GetDirection(pUserClass.pPointer as Actor,pAgentTree, pNode);
			}
			case 1098330067://GetUp
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 0) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_GetUp(pUserClass.pPointer as Actor,pAgentTree, pNode);
			}
			case -736682351://SetUp
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 1) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_SetUp(pUserClass.pPointer as Actor,pAgentTree.GetInportVec3(pNode,1));
			}
			case -2078882846://GetRight
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 0) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_GetRight(pUserClass.pPointer as Actor,pAgentTree, pNode);
			}
			case -593849486://GetScale
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 0) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_GetScale(pUserClass.pPointer as Actor,pAgentTree, pNode);
			}
			case 611036323://SetScale
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 1) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_SetScale(pUserClass.pPointer as Actor,pAgentTree.GetInportVec3(pNode,1));
			}
			case -88835465://SetTransfrom
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 3) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_SetTransfrom(pUserClass.pPointer as Actor,pAgentTree.GetInportVec3(pNode,1),pAgentTree.GetInportVec3(pNode,2),pAgentTree.GetInportVec3(pNode,3));
			}
			case -375119552://IsFlag
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 1) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_IsFlag(pUserClass.pPointer as Actor,(Framework.ActorSystem.Runtime.EActorFlag)pAgentTree.GetInportInt(pNode,1), pAgentTree, pNode);
			}
			case 724323496://SetFlag
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 2) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_SetFlag(pUserClass.pPointer as Actor,(Framework.ActorSystem.Runtime.EActorFlag)pAgentTree.GetInportInt(pNode,1),pAgentTree.GetInportBool(pNode,2));
			}
			case -1337224865://IsCanLogic
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 0) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_IsCanLogic(pUserClass.pPointer as Actor,pAgentTree, pNode);
			}
			case -902001298://IsInvincible
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 0) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_IsInvincible(pUserClass.pPointer as Actor,pAgentTree, pNode);
			}
			case -716677369://SetDelayDestroy
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 1) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_SetDelayDestroy(pUserClass.pPointer as Actor,pAgentTree.GetInportFloat(pNode,1));
			}
			case 458663211://GetDelayDestroy
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 0) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_GetDelayDestroy(pUserClass.pPointer as Actor,pAgentTree, pNode);
			}
			case 854312887://SetDestroy
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 0) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_SetDestroy(pUserClass.pPointer as Actor);
			}
			case -1507174251://IsDestroy
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 0) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_IsDestroy(pUserClass.pPointer as Actor,pAgentTree, pNode);
			}
			case -130130118://EnableHudBar
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 1) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_EnableHudBar(pUserClass.pPointer as Actor,pAgentTree.GetInportBool(pNode,1));
			}
			case 1847440768://IsEnableHudBar
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 0) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_IsEnableHudBar(pUserClass.pPointer as Actor,pAgentTree, pNode);
			}
			case 63681228://IsColliderAble
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 0) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_IsColliderAble(pUserClass.pPointer as Actor,pAgentTree, pNode);
			}
			case -2139515976://SetKilled
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 1) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_SetKilled(pUserClass.pPointer as Actor,pAgentTree.GetInportBool(pNode,1));
			}
			case 635254065://IsKilled
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 0) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_IsKilled(pUserClass.pPointer as Actor,pAgentTree, pNode);
			}
			case 873572757://SetVisible
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 1) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_SetVisible(pUserClass.pPointer as Actor,pAgentTree.GetInportBool(pNode,1));
			}
			case -1596967241://IsVisible
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 0) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_IsVisible(pUserClass.pPointer as Actor,pAgentTree, pNode);
			}
			case 943289552://SetActived
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 1) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_SetActived(pUserClass.pPointer as Actor,pAgentTree.GetInportBool(pNode,1));
			}
			case -1392972814://IsActived
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 0) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_IsActived(pUserClass.pPointer as Actor,pAgentTree, pNode);
			}
			case 59183278://EnableLogic
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 1) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_EnableLogic(pUserClass.pPointer as Actor,pAgentTree.GetInportBool(pNode,1));
			}
			case 511331437://EnableAI
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 1) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_EnableAI(pUserClass.pPointer as Actor,pAgentTree.GetInportBool(pNode,1));
			}
			case -1742940291://IsEnableAI
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 0) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_IsEnableAI(pUserClass.pPointer as Actor,pAgentTree, pNode);
			}
			case -733558894://EnableRVO
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 1) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_EnableRVO(pUserClass.pPointer as Actor,pAgentTree.GetInportBool(pNode,1));
			}
			case -914902675://IsEnableRVO
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 0) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_IsEnableRVO(pUserClass.pPointer as Actor,pAgentTree, pNode);
			}
			case 1988170200://StartActionState
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 4) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_StartActionState(pUserClass.pPointer as Actor,(Framework.ActorSystem.Runtime.EActionStateType)pAgentTree.GetInportInt(pNode,1),pAgentTree.GetInportUint(pNode,2),pAgentTree.GetInportBool(pNode,3),pAgentTree.GetInportUserData<Framework.ActorSystem.Runtime.IContextData>(pNode,4));
			}
			case 242567738://StartActionState
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 3) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_StartActionState_1(pUserClass.pPointer as Actor,pAgentTree.GetInportUint(pNode,1),pAgentTree.GetInportBool(pNode,2),pAgentTree.GetInportUserData<Framework.ActorSystem.Runtime.IContextData>(pNode,3));
			}
			case -228769905://StopActionState
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 2) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_StopActionState(pUserClass.pPointer as Actor,(Framework.ActorSystem.Runtime.EActionStateType)pAgentTree.GetInportInt(pNode,1),pAgentTree.GetInportUint(pNode,2));
			}
			case 1689203650://SetIdleType
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 2) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_SetIdleType(pUserClass.pPointer as Actor,(Framework.ActorSystem.Runtime.EActionStateType)pAgentTree.GetInportInt(pNode,1),pAgentTree.GetInportUint(pNode,2));
			}
			case -2072478675://RemoveActionState
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 2) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_RemoveActionState(pUserClass.pPointer as Actor,(Framework.ActorSystem.Runtime.EActionStateType)pAgentTree.GetInportInt(pNode,1),pAgentTree.GetInportUint(pNode,2));
			}
			case -22641626://SetAttr
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 2) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_SetAttr(pUserClass.pPointer as Actor,pAgentTree.GetInportByte(pNode,1),pAgentTree.GetInportFloat(pNode,2));
			}
			case 1832336635://GetAttr
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 2) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_GetAttr(pUserClass.pPointer as Actor,pAgentTree.GetInportByte(pNode,1),pAgentTree.GetInportFloat(pNode,2), pAgentTree, pNode);
			}
			case -468082741://RemoveAttr
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 1) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_RemoveAttr(pUserClass.pPointer as Actor,pAgentTree.GetInportByte(pNode,1));
			}
			case 1610379500://AppendAttr
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 2) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_AppendAttr(pUserClass.pPointer as Actor,pAgentTree.GetInportByte(pNode,1),pAgentTree.GetInportFloat(pNode,2));
			}
			case 315679903://SubAttr
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 3) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_SubAttr(pUserClass.pPointer as Actor,pAgentTree.GetInportByte(pNode,1),pAgentTree.GetInportInt(pNode,2),pAgentTree.GetInportBool(pNode,3));
			}
			case 1249179647://ClearAttrs
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 0) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_ClearAttrs(pUserClass.pPointer as Actor);
			}
			case -943336217://IsInAction
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 1) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_IsInAction(pUserClass.pPointer as Actor,(Framework.ActorSystem.Runtime.EActionStateType)pAgentTree.GetInportInt(pNode,1), pAgentTree, pNode);
			}
			case -1576406313://IsAttacking
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 0) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_IsAttacking(pUserClass.pPointer as Actor,pAgentTree, pNode);
			}
			case 528609504://IsCutscneHolded
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 0) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_IsCutscneHolded(pUserClass.pPointer as Actor,pAgentTree, pNode);
			}
			case -604755324://ResetFreeze
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 0) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_ResetFreeze(pUserClass.pPointer as Actor);
			}
			case 181418108://Freezed
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 2) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_Freezed(pUserClass.pPointer as Actor,pAgentTree.GetInportBool(pNode,1),pAgentTree.GetInportFloat(pNode,2));
			}
			case -1080427348://IsFreezed
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 0) return true;
				if(!(pUserClass.pPointer is Actor)) return true;
				return AT_IsFreezed(pUserClass.pPointer as Actor,pAgentTree, pNode);
			}
			}
			return false;
		}
	}
}

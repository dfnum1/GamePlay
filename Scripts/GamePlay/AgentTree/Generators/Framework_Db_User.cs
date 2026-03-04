//auto generated
using Framework.AT.Runtime;
namespace Framework.Db
{
#if UNITY_EDITOR
	[ATClass(typeof(Framework.Db.User), "用户系统/用户")]
#endif
	public class Framework_Db_User
	{
#if UNITY_EDITOR
		[ATFunction(2147233685,"获取SdkUid",typeof(Framework.Db.User),false)]
		[ATFunctionArgv(typeof(VariableUserData),"User",false, null,typeof(Framework.Db.User))]
		[ATFunctionReturn(typeof(Framework.AT.Runtime.VariableString), "pReturn", null,typeof(System.String))]
#endif
		static bool AT_GetSdkUid(User pPointerThis,AgentTree pAgentTree, BaseNode pNode)
		{
			pAgentTree.SetOutportString(pNode, 0, pPointerThis.GetSdkUid());
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(-1898955544,"设置SdkUid",typeof(Framework.Db.User),false)]
		[ATFunctionArgv(typeof(VariableUserData),"User",false, null,typeof(Framework.Db.User))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableString),"uid",false, null,typeof(System.String))]
#endif
		static bool AT_SetSDKUid(User pPointerThis,System.String uid)
		{
			pPointerThis.SetSDKUid(uid);
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(235885163,"获取Db数据",typeof(Framework.Db.User),false)]
		[ATFunctionArgv(typeof(VariableUserData),"User",false, null,typeof(Framework.Db.User))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableInt),"type",false, null,typeof(System.Int32),drawMethod:"DrawProxyDbTypePop")]
		[ATFunctionReturn(typeof(Framework.AT.Runtime.VariableUserData), "pReturn", null,typeof(Framework.Db.AProxyDB))]
#endif
		static bool AT_GetProxyDB(User pPointerThis,System.Int32 type,AgentTree pAgentTree, BaseNode pNode)
		{
			pAgentTree.SetOutportUserData(pNode, 0, pPointerThis.GetProxyDB(type));
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(1346945877,"清理所有Db数据",typeof(Framework.Db.User),false)]
		[ATFunctionArgv(typeof(VariableUserData),"User",false, null,typeof(Framework.Db.User))]
#endif
		static bool AT_Clear(User pPointerThis)
		{
			pPointerThis.Clear();
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(-1274085549,"设置登录时间",typeof(Framework.Db.User),false)]
		[ATFunctionArgv(typeof(VariableUserData),"User",false, null,typeof(Framework.Db.User))]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableLong),"time",false, null,typeof(System.Int64))]
#endif
		static bool AT_SetLastLoginTime(User pPointerThis,System.Int64 time)
		{
			pPointerThis.SetLastLoginTime(time);
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(-2045211318,"获取登录时间",typeof(Framework.Db.User),false)]
		[ATFunctionArgv(typeof(VariableUserData),"User",false, null,typeof(Framework.Db.User))]
		[ATFunctionReturn(typeof(Framework.AT.Runtime.VariableLong), "pReturn", null,typeof(System.Int64))]
#endif
		static bool AT_GetLastLoginTime(User pPointerThis,AgentTree pAgentTree, BaseNode pNode)
		{
			pAgentTree.SetOutportLong(pNode, 0, pPointerThis.GetLastLoginTime());
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
			case 2147233685://GetSdkUid
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 0) return true;
				if(!(pUserClass.pPointer is User)) return true;
				return AT_GetSdkUid((User)pUserClass.pPointer,pAgentTree, pNode);
			}
			case -1898955544://SetSDKUid
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 1) return true;
				if(!(pUserClass.pPointer is User)) return true;
				return AT_SetSDKUid((User)pUserClass.pPointer,pAgentTree.GetInportString(pNode,1));
			}
			case 235885163://GetProxyDB
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 1) return true;
				if(!(pUserClass.pPointer is User)) return true;
				return AT_GetProxyDB((User)pUserClass.pPointer,pAgentTree.GetInportInt(pNode,1), pAgentTree, pNode);
			}
			case 1346945877://Clear
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 0) return true;
				if(!(pUserClass.pPointer is User)) return true;
				return AT_Clear((User)pUserClass.pPointer);
			}
			case -1274085549://SetLastLoginTime
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 1) return true;
				if(!(pUserClass.pPointer is User)) return true;
				return AT_SetLastLoginTime((User)pUserClass.pPointer,pAgentTree.GetInportLong(pNode,1));
			}
			case -2045211318://GetLastLoginTime
			{
				if(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;
				if(pNode.GetInportCount() <= 0) return true;
				if(!(pUserClass.pPointer is User)) return true;
				return AT_GetLastLoginTime((User)pUserClass.pPointer,pAgentTree, pNode);
			}
			}
			return true;
		}
	}
}

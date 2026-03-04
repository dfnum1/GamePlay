//auto generated
using Framework.AT.Runtime;
namespace Framework.Db
{
#if UNITY_EDITOR
	[ATClass(typeof(Framework.Db.UserManager), "用户系统")]
#endif
	public class Framework_Db_UserManager
	{
#if UNITY_EDITOR
		[ATFunction(-1760455177,"获取用户",typeof(Framework.Db.UserManager),false)]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableLong),"userID",false, null,typeof(System.Int64))]
		[ATFunctionReturn(typeof(Framework.AT.Runtime.VariableUserData), "pReturn", null,typeof(Framework.Db.User))]
#endif
		static bool AT_GetUser(UserManager pPointerThis,System.Int64 userID,AgentTree pAgentTree, BaseNode pNode)
		{
			pAgentTree.SetOutportUserData(pNode, 0, pPointerThis.GetUser(userID));
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(1144685124,"获取当前用户",typeof(Framework.Db.UserManager),false)]
		[ATFunctionReturn(typeof(Framework.AT.Runtime.VariableUserData), "pReturn", null,typeof(Framework.Db.User))]
#endif
		static bool AT_GetCurUser(UserManager pPointerThis,AgentTree pAgentTree, BaseNode pNode)
		{
			pAgentTree.SetOutportUserData(pNode, 0, pPointerThis.GetCurUser());
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(1602086122,"添加用户",typeof(Framework.Db.UserManager),false)]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableLong),"userID",false, null,typeof(System.Int64))]
		[ATFunctionReturn(typeof(Framework.AT.Runtime.VariableUserData), "pReturn", null,typeof(Framework.Db.User))]
#endif
		static bool AT_AddUser(UserManager pPointerThis,System.Int64 userID,AgentTree pAgentTree, BaseNode pNode)
		{
			pAgentTree.SetOutportUserData(pNode, 0, pPointerThis.AddUser(userID));
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(1261867877,"清理用户列表",typeof(Framework.Db.UserManager),false)]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableBool),"bIncludeMyself",false, null,typeof(System.Boolean))]
#endif
		static bool AT_ClearUser(UserManager pPointerThis,System.Boolean bIncludeMyself)
		{
			pPointerThis.ClearUser(bIncludeMyself);
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(1532732646,"我自己",typeof(Framework.Db.UserManager),false)]
		[ATFunctionReturn(typeof(Framework.AT.Runtime.VariableUserData),"mySelf", null,typeof(Framework.Db.User))]
#endif
		static bool AT_Get_mySelf(UserManager pPointerThis, AgentTree pAgentTree, BaseNode pNode)
		{
			pAgentTree.SetOutportUserData(pNode, 0, pPointerThis.mySelf);
			return true;
		}

		public static bool DoAction(VariableUserData pUserClass, AgentTree pAgentTree, BaseNode pNode)
		{
			int actionType = pNode.type;
			switch(actionType)
			{
			case -1760455177://GetUser
			{
				if(pNode.GetInportCount() <= 1) return true;
				Framework.Db.UserManager pModulePointer = pAgentTree.GetModule<Framework.Db.UserManager>();
				if(pModulePointer == null) return true;
				return AT_GetUser(pModulePointer,pAgentTree.GetInportLong(pNode,1), pAgentTree, pNode);
			}
			case 1144685124://GetCurUser
			{
				if(pNode.GetInportCount() <= 0) return true;
				Framework.Db.UserManager pModulePointer = pAgentTree.GetModule<Framework.Db.UserManager>();
				if(pModulePointer == null) return true;
				return AT_GetCurUser(pModulePointer,pAgentTree, pNode);
			}
			case 1602086122://AddUser
			{
				if(pNode.GetInportCount() <= 1) return true;
				Framework.Db.UserManager pModulePointer = pAgentTree.GetModule<Framework.Db.UserManager>();
				if(pModulePointer == null) return true;
				return AT_AddUser(pModulePointer,pAgentTree.GetInportLong(pNode,1), pAgentTree, pNode);
			}
			case 1261867877://ClearUser
			{
				if(pNode.GetInportCount() <= 1) return true;
				Framework.Db.UserManager pModulePointer = pAgentTree.GetModule<Framework.Db.UserManager>();
				if(pModulePointer == null) return true;
				return AT_ClearUser(pModulePointer,pAgentTree.GetInportBool(pNode,1));
			}
			case 1532732646://mySelf get
			{
				if(pNode.GetInportCount() <= 1) return true;
				Framework.Db.UserManager pModulePointer = pAgentTree.GetModule<Framework.Db.UserManager>();
				if(pModulePointer == null) return true;
				return AT_Get_mySelf(pModulePointer, pAgentTree, pNode);
			}
			}
			return true;
		}
	}
}

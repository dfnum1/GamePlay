//auto generated
using Framework.AT.Runtime;
namespace Framework.Data
{
#if UNITY_EDITOR
	[ATClass(typeof(Framework.Data.ADataManager),"配置数据系统/管理器")]
#endif
	public class Framework_Data_ADataManager
	{
#if UNITY_EDITOR
		[ATFunction(-885779178,"获取配置数据",typeof(Framework.Data.ADataManager),false)]
		[ATFunctionArgv(typeof(Framework.AT.Runtime.VariableInt),"table",false, null,typeof(System.Int32),drawMethod:"DrawCsvTablePop")]
		[ATFunctionReturn(typeof(Framework.AT.Runtime.VariableUserData), "pReturn", null,typeof(Framework.Data.Data_Base))]
#endif
		static bool AT_GetCsvData(ADataManager pPointerThis,System.Int32 table,AgentTree pAgentTree, BaseNode pNode)
		{
			pAgentTree.SetOutportUserData(pNode, 0, pPointerThis.GetCsvData(table));
			return true;
		}
#if UNITY_EDITOR
		[ATFunction(-65229731,"Progress",typeof(Framework.Data.ADataManager),false)]
		[ATFunctionReturn(typeof(Framework.AT.Runtime.VariableFloat),"Progress", null,typeof(System.Single))]
#endif
		static bool AT_Get_Progress(ADataManager pPointerThis, AgentTree pAgentTree, BaseNode pNode)
		{
			pAgentTree.SetOutportFloat(pNode, 0, pPointerThis.Progress);
			return true;
		}

		public static bool DoAction(VariableUserData pUserClass, AgentTree pAgentTree, BaseNode pNode)
		{
			int actionType = pNode.type;
			switch(actionType)
			{
			case -885779178://GetCsvData
			{
				if(pNode.GetInportCount() <= 1) return true;
				Framework.Data.ADataManager pModulePointer = pAgentTree.GetModule<Framework.Data.ADataManager>();
				if(pModulePointer == null) return true;
				return AT_GetCsvData(pModulePointer,pAgentTree.GetInportInt(pNode,1), pAgentTree, pNode);
			}
			case -65229731://Progress get
			{
				if(pNode.GetInportCount() <= 1) return true;
				Framework.Data.ADataManager pModulePointer = pAgentTree.GetModule<Framework.Data.ADataManager>();
				if(pModulePointer == null) return true;
				return AT_Get_Progress(pModulePointer, pAgentTree, pNode);
			}
			}
			return true;
		}
	}
}

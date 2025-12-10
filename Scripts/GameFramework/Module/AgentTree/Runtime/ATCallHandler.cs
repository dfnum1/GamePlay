/********************************************************************
生成日期:	07:03:2025
类    名: 	ATCallHandler
作    者:	HappLI
描    述:	AT 调用句柄
*********************************************************************/
using System.Collections.Generic;

namespace Framework.AT.Runtime
{
	public class ATCallHandler
	{
		public delegate bool OnActionDelegate(VariableUserData pCall, AgentTree pAgentTree, BaseNode pNode);
		static Dictionary<int, OnActionDelegate> ms_CallHandles = new Dictionary<int, OnActionDelegate>(128);
		public static void RegisterHandler(int callId, OnActionDelegate callFunction)
		{
			if (callId == 0 && callFunction == null)
				return;
			ms_CallHandles[callId] = callFunction;
        }
        //-----------------------------------------------------
        internal static bool DoAction(AgentTree pAgentTree, BaseNode pNode)
		{
			if(pNode == null || pNode.GetInportCount()<=0) return true;
			var pUserClasser = pAgentTree.GetInportUserData(pNode, 0);
			if (pUserClasser.value == 0)
				return false;
			if(ms_CallHandles.TryGetValue(pUserClasser.value, out var callFunction))
			{
				return callFunction(pUserClasser, pAgentTree, pNode);
            }
			return false;
		}
	}
}

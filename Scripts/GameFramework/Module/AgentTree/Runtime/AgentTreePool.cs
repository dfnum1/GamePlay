/********************************************************************
生成日期:	07:03:2025
类    名: 	AgentTreePool
作    者:	HappLI
描    述:	行为树pool
*********************************************************************/
using System.Collections.Generic;

namespace Framework.AT.Runtime
{
    internal class AgentTreePool
    {
        private static int MAX_POOL = 32;
        private static Stack<AgentTree> ms_vAgentTreePool = null;
        //-----------------------------------------------------
        internal static AgentTree MallocAgentTree()
        {
            if (ms_vAgentTreePool != null && ms_vAgentTreePool.Count > 0)
                return ms_vAgentTreePool.Pop();
            return new AgentTree();
        }
        //-----------------------------------------------------
        internal static void FreeAgentTree(AgentTree agentTree)
        {
            if (agentTree == null) return;
            agentTree.Destroy();
            if (ms_vAgentTreePool != null && ms_vAgentTreePool.Count >= MAX_POOL)
                return;
            if (ms_vAgentTreePool == null) ms_vAgentTreePool = new Stack<AgentTree>(MAX_POOL);
            ms_vAgentTreePool.Push(agentTree);
        }
    }
}
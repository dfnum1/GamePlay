/********************************************************************
生成日期:	07:03:2025
类    名: 	AgentTreePool
作    者:	HappLI
描    述:	行为树pool
*********************************************************************/
using Framework.Core;

namespace Framework.AT.Runtime
{
    public static class AgentTreePool
    {
        //-----------------------------------------------------
        internal static AgentTree MallocAgentTree(AFramework pFramework)
        {
            AgentTree pAT = null;
            if (pFramework != null) pAT = pFramework.ShareCache.MallocAgentTree();
            else pAT = new AgentTree();
            if(pFramework!=null) pAT.SetATManager(pFramework.GetModule<AgentTreeManager>());
            return pAT;
        }
        //-----------------------------------------------------
        internal static AgentTree MallocAgentTree(AgentTreeData pATData, AFramework pFramework)
        {
            if (pATData == null || !pATData.IsValid()) return null;
            AgentTree pAT = MallocAgentTree(pFramework);
            if (pAT.Create(pATData))
                return pAT;
            FreeAgentTree(pAT);
            return null;
        }
        //-----------------------------------------------------
        internal static void FreeAgentTree(AgentTree agentTree)
        {
            if (agentTree == null) return;
            var frameWork = agentTree.GetFramework();
            if(frameWork!=null)
            {
                frameWork.ShareCache.FreeAgentTree(agentTree);
                return;
            }
            agentTree.Destroy();
        }
        //-----------------------------------------------------
        public static void Free(this AgentTree pAt)
        {
            FreeAgentTree(pAt);
        }
    }
}
/********************************************************************
生成日期:	5:11:2020  20:36
类    名: 	ActorAgentTree
作    者:	HappLI
描    述:	行为树逻辑Agent
*********************************************************************/
using Framework.AT.Runtime;
namespace Framework.ActorSystem.Runtime
{
    public class ActorAgentTree : AActorAgent
    {
        AgentTree m_pAgentTree = null;
        //--------------------------------------------------------
        protected override void OnLoadedAble(ActorContext component)
        {
            if(component.pContextData!=null && component.pContextData is AActorComponent)
            {
                LoadAT(((AActorComponent)component.pContextData).ATData);
            }
        }
        //--------------------------------------------------------
        void LoadAT(AgentTreeData atData)
        {
            if (atData != null || (m_pAgentTree != null && m_pAgentTree.GetData() != atData))
            {
                if (m_pAgentTree != null)
                {
                    AgentTreePool.FreeAgentTree(m_pAgentTree);
                    m_pAgentTree = null;
                }
                m_pAgentTree = AgentTreePool.MallocAgentTree(atData);
                if (m_pAgentTree != null)
                {
                    m_pAgentTree.AddOwnerClass(m_pActor);
                    m_pAgentTree.Enable(true);
                }
            }
        }
        //--------------------------------------------------------
        protected override void OnFlagDirty(EActorFlag flag, bool IsUsed)
        {
            if (m_pAgentTree != null && IsUsed) m_pAgentTree.Start();
        }
        //--------------------------------------------------------
        protected override void OnUpdate(float fDelta)
        {
            base.OnUpdate(fDelta);
            if (m_pAgentTree != null) m_pAgentTree.Update(fDelta);
        }
        //--------------------------------------------------------
        internal void OnSkill(Skill pSkill)
        {
        //    if (m_pAgentTree != null) m_pAgentTree.ExecuteTask(fDelta);
        }
        //--------------------------------------------------------
        protected override void OnDestroy()
        {
            base.OnDestroy();
            if(m_pAgentTree!=null)
            {
                AgentTreePool.FreeAgentTree(m_pAgentTree);
                m_pAgentTree = null;
            }
        }
	}
}
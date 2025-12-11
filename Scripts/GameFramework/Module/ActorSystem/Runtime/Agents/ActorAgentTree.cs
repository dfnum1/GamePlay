/********************************************************************
生成日期:	5:11:2020  20:36
类    名: 	ActorAgentTree
作    者:	HappLI
描    述:	行为树逻辑Agent
*********************************************************************/
using Framework.AT.Runtime;
using UnityEngine.Playables;
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
        internal void LoadAT(AgentTreeData atData)
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
        public void ExecuteTask(int task, VariableList vArgvs = null, bool bAutoReleaseAgvs = true)
        {
            if (m_pAgentTree == null) return;
            m_pAgentTree.ExecuteTask(task, vArgvs, bAutoReleaseAgvs);
        }
        //--------------------------------------------------------
        protected override void OnFlagDirty(EActorFlag flag, bool IsUsed)
        {
            if (m_pAgentTree == null) return;
            switch(flag)
            {
                case EActorFlag.Active:
                    {
                        if (IsUsed) m_pAgentTree.Start();
                    }
                    break;
                case EActorFlag.Killed:
                    {
                        if (IsUsed)
                        {
                            m_pAgentTree.ExecuteTask((int)EActorATType.onKilled);
                        }
                        else
                        {
                            m_pAgentTree.ExecuteTask((int)EActorATType.onRevive);
                        }
                    }
                    break;
            }
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
            if (m_pAgentTree == null) return;
            var argvs = VariableList.Malloc();
            var lockTargets = pSkill.GetLockTargets();
            if(lockTargets.Count>0)
            {
                argvs.AddUserData(lockTargets[0]);
                argvs.AddUserData(pSkill);
            }
            m_pAgentTree.ExecuteTask((int)EActorATType.onAttack);
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
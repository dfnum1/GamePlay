/********************************************************************
生成日期:	5:11:2020  20:36
类    名: 	ActorAgentTree
作    者:	HappLI
描    述:	行为树逻辑Agent
*********************************************************************/
using ExternEngine;
using Framework.AT.Runtime;
using Framework.Base;
using System.Collections.Generic;
using System.IO;
using UnityEngine.Playables;
namespace Framework.ActorSystem.Runtime
{
    public class ActorAgentTree : AActorAgent
    {
        AgentTree m_pMainAgentTree = null;
        private List<AgentTree> m_vSubAgentTrees = null;
        //--------------------------------------------------------
        public AgentTree GetMainAT()
        {
            return m_pMainAgentTree;
        }
        //--------------------------------------------------------
        public AgentTree AddSubAT(AgentTreeData atData)
        {
            if (atData == null)
                return null;
            if (m_pMainAgentTree != null && m_pMainAgentTree.GetData() == atData)
            {
                Logger.Warning("已经为该Actor 的主蓝图了,不能添加!");
                return null;
            }
            if(m_vSubAgentTrees!=null)
            {
                foreach(var db in m_vSubAgentTrees)
                {
                    if (db.GetData() == atData)
                    {
                        Logger.Warning("该Actor已经包含该蓝图了!");
                        return db;
                    }
                }
            }

            var pAT = AgentTreePool.MallocAgentTree(atData, GetFramework());
            if(pAT!=null)
            {
                pAT.AddOwnerClass(m_pActor);
                pAT.Enable(m_pActor.IsActived());
                pAT.Start();
                if (m_vSubAgentTrees == null)
                    m_vSubAgentTrees = new List<AgentTree>(2);
                m_vSubAgentTrees.Add(pAT);
            }
            return pAT;
        }
        //--------------------------------------------------------
        public bool DeleteSubAT(AgentTreeData atData)
        {
            if (atData == null) return false;
            if (m_vSubAgentTrees == null) return false;
            for(int i =0; i < m_vSubAgentTrees.Count ; ++i)
            {
                var pAT = m_vSubAgentTrees[i];
                if (pAT.GetData() == atData)
                {
                    AgentTreePool.FreeAgentTree(pAT);
                    m_vSubAgentTrees.RemoveAt(i);
                    return true;
                }
            }
            return false;
        }
        //--------------------------------------------------------
        protected override void OnLoadActorGraphData(ActorGraphData component)
        {
            if(component.ATData!=null)
            {
                LoadAT(component.ATData);
            }
        }
        //--------------------------------------------------------
        internal void LoadAT(AgentTreeData atData)
        {
            if (atData != null || (m_pMainAgentTree != null && m_pMainAgentTree.GetData() != atData))
            {
                if (m_pMainAgentTree != null)
                {
                    AgentTreePool.FreeAgentTree(m_pMainAgentTree);
                    m_pMainAgentTree = null;
                }
                m_pMainAgentTree = AgentTreePool.MallocAgentTree(atData, GetFramework());
                if (m_pMainAgentTree != null)
                {
                    m_pMainAgentTree.AddOwnerClass(m_pActor);
                    m_pMainAgentTree.Enable(m_pActor.IsActived());
                    m_pMainAgentTree.Start();
                }
            }
        }
        //--------------------------------------------------------
        public void ExecuteTask(int task, VariableList vArgvs = null, bool bAutoReleaseAgvs = true)
        {
            if(m_pMainAgentTree!=null) m_pMainAgentTree.ExecuteTask(task, vArgvs, false);
            if (m_vSubAgentTrees != null)
            {
                foreach (var db in m_vSubAgentTrees)
                    db.ExecuteTask(task, vArgvs, false);
            }
            if (bAutoReleaseAgvs && vArgvs!=null) vArgvs.Release();
        }
        //--------------------------------------------------------
        public void ExecuteEvent(int eventType, VariableList vArgvs = null, bool bAutoReleaseAgvs = true, EActionType eActionType = EActionType.eCustomEvent)
        {
            if (m_pMainAgentTree != null) m_pMainAgentTree.ExecuteEvent(eventType, vArgvs, false, eActionType);
            if (m_vSubAgentTrees != null)
            {
                foreach (var db in m_vSubAgentTrees)
                    db.ExecuteEvent(eventType, vArgvs, false, eActionType);
            }
            if (bAutoReleaseAgvs && vArgvs != null) vArgvs.Release();
        }
        //--------------------------------------------------------
        internal void OnHit(HitFrameActor hitFrame)
        {
            var argvs = VariableList.Malloc(GetFramework());
            argvs.AddUserData(GetActor());
            argvs.AddUserData(hitFrame);
            ExecuteTask((int)EActorATType.onHitFrame,argvs, true);
        }
        //--------------------------------------------------------
        protected override void OnFlagDirty(EActorFlag flag, bool IsUsed)
        {
            if (m_pMainAgentTree == null) return;
            switch(flag)
            {
                case EActorFlag.Active:
                    {
                        if (m_pMainAgentTree != null)
                        {
                            m_pMainAgentTree.Enable(IsUsed);
                        }
                        if(m_vSubAgentTrees!=null)
                        {
                            foreach (var db in m_vSubAgentTrees)
                                db.Enable(IsUsed);
                        }
                    }
                    break;
                case EActorFlag.Killed:
                    {
                        if (IsUsed)
                        {
                            if(m_pMainAgentTree!=null)
                            {
                                m_pMainAgentTree.ExecuteTask((int)EActorATType.onKilled);
                            }
                            if (m_vSubAgentTrees != null)
                            {
                                foreach (var db in m_vSubAgentTrees)
                                    db.ExecuteTask((int)EActorATType.onKilled);
                            }
                        }
                        else
                        {
                            if (m_pMainAgentTree != null)
                                m_pMainAgentTree.ExecuteTask((int)EActorATType.onRevive);
                            if (m_vSubAgentTrees != null)
                            {
                                foreach (var db in m_vSubAgentTrees)
                                    db.ExecuteTask((int)EActorATType.onRevive);
                            }
                        }
                    }
                    break;
            }
        }
        //--------------------------------------------------------
        protected override void OnUpdate(FFloat fDelta)
        {
            base.OnUpdate(fDelta);
            if (m_pMainAgentTree != null) m_pMainAgentTree.Update(fDelta);
            if(m_vSubAgentTrees!=null)
            {
                for(int i =0; i < m_vSubAgentTrees.Count; ++i)
                {
                    m_vSubAgentTrees[i].Update(fDelta);
                }
            }
        }
        //--------------------------------------------------------
        protected override void OnDestroy()
        {
            base.OnDestroy();
            if(m_pMainAgentTree!=null)
            {
                AgentTreePool.FreeAgentTree(m_pMainAgentTree);
                m_pMainAgentTree = null;
            }
            if(m_vSubAgentTrees!=null)
            {
                foreach(var db in m_vSubAgentTrees)
                {
                    AgentTreePool.FreeAgentTree(db);
                }
                m_vSubAgentTrees.Clear();
            }
        }
	}
}
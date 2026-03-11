/********************************************************************
生成日期:	11:07:2025
类    名: 	GameWorld
作    者:	HappLI
描    述:	游戏世界
*********************************************************************/
using Framework.AT.Runtime;
using Framework.Core;
using System.Collections.Generic;
using UnityEngine;
#if USE_FIXEDMATH
using ExternEngine;
#else
using FFloat = System.Single;
#endif
namespace Framework.State.Runtime
{
    [ATInteralExport("游戏世界", -6, icon: "GameWorld/gameworld")]
    public class GameWorld : AModule
    {
        private AState      m_pGameState;
        private List<AMode> m_vGameModes;
        GameWorldData       m_pWorldObject;
        private AgentTree   m_pAgentTree = null;
        //--------------------------------------------------------
        public void CreateWorld(GameWorldData pObject)
        {
            if (m_pWorldObject == pObject)
                return;
            ClearWorld();
            m_pWorldObject = pObject;
            if (m_pWorldObject == null)
                return;
            if (m_pWorldObject.atData != null && m_pWorldObject.atData.worldAgentTree!=null)
            {
                m_pAgentTree = m_pFramework.ShareCache.MallocAgentTree(m_pWorldObject.atData.worldAgentTree);
                if(m_pAgentTree!=null)
                {
                    m_pAgentTree.AddOwnerClass(this);
                }
            }
            m_pGameState = GameWorldHandler.Malloc<AState>( GetFramework(), m_pWorldObject.gameStateData.stateType);
            if (m_pGameState != null)
            {
                m_pGameState.CleanAPIStatus();
                m_pGameState.SetGameWorld(this);

                m_pGameState.CreateLogics(m_pWorldObject.gameStateData.stateLogics);

                if (m_pWorldObject.modeDatas != null)
                {
                    if(m_vGameModes==null) m_vGameModes = new List<AMode>(m_pWorldObject.modeDatas.Count);
                    m_vGameModes.Clear();
                    for (int i =0; i < m_pWorldObject.modeDatas.Count; ++i)
                    {
                        var modeData = m_pWorldObject.modeDatas[i];
                        var pMode = GameWorldHandler.Malloc<AMode>(GetFramework(), modeData.modeType);
                        if (pMode != null)
                        {
                            pMode.SetState(m_pGameState);
                            pMode.CreateLogics(modeData.modeLogics);
                            m_vGameModes.Add(pMode);
                            if(m_pWorldObject.gameStateData.activeMode == modeData.modeType)
                            {
                                m_pGameState.SetActiveMode(pMode);
                            }
                        }
                        else
                        {
                            Framework.Base.Logger.Assert(false, "无法创建游戏模式实例:" + modeData.modeType);
                        }
                    }
                    if(m_pGameState.GetActiveMode() == null)
                    {
                        Framework.Base.Logger.Warning("当前状态没有激活的玩法模式！！！");
                    }
                }
            }
            else
            {
                Framework.Base.Logger.Assert(false, "无法创建游戏状态实例:" + m_pWorldObject.gameStateData.stateType);
            }
        }
        //--------------------------------------------------------
        public GameWorldData GetWorldData()
        {
            return m_pWorldObject;
        }
        //--------------------------------------------------------
        public AgentTreeData GetAgentTreeData(ushort agentId)
        {
            if (agentId ==0 || m_pWorldObject == null) return null;
            if (m_pWorldObject.warAgents == null)
                return null;
            for(int i =0; i < m_pWorldObject.warAgents.Count; ++i)
            {
                if (m_pWorldObject.warAgents[i].agentId == agentId)
                    return m_pWorldObject.warAgents[i].atData;
            }
            return null;
        }
        //--------------------------------------------------------
        [ATMethod("唤醒游戏状态")]
        public void AwakeState()
        {
            if (m_pGameState == null) return;
            m_pGameState.Awake();
            if(m_pAgentTree!=null) m_pAgentTree.Start();
        }
        //--------------------------------------------------------
        [ATMethod("预备游戏状态")]
        public void PreStartState()
        {
            if (m_pGameState == null) return;
            m_pGameState.PreStart();
        }
        //--------------------------------------------------------
        [ATMethod("开始游戏状态")]
        public void StartState()
        {
            if (m_pGameState == null) return;
            m_pGameState.Start();
        }
        //--------------------------------------------------------
        [ATMethod("激活游戏状态")]
        public void ActiveState(bool bActive)
        {
            if (m_pGameState == null) return;
            m_pGameState.Active(bActive);
            if (m_pAgentTree != null) m_pAgentTree.Enable(bActive);
        }
        //--------------------------------------------------------
        public bool SetActiveMode<T>() where T : AMode
        {
            if (m_pGameState == null || m_vGameModes == null) return false;
            for (int i = 0; i < m_vGameModes.Count; ++i)
            {
                if (m_vGameModes[i] is T)
                {
                    m_pGameState.SetActiveMode(m_vGameModes[i]);
                    return true;
                }
            }
            return false;
        }
        //--------------------------------------------------------
        [ATMethod("清理游戏世界")]
        public void ClearWorld()
        {
            if (m_pAgentTree != null)
            {
                m_pAgentTree.Exit();
                m_pAgentTree.Free();
                m_pAgentTree = null;
            }
            m_pWorldObject = null;
            if (m_vGameModes!=null)
            {
                foreach(var db in m_vGameModes)
                {
                    db.Free();
                }
                m_vGameModes.Clear();
            }
            if (m_pGameState != null) m_pGameState.Free();
            m_pGameState = null;
        }
        //--------------------------------------------------------
        protected override void OnUpdate(FFloat fFrameTime)
        {
            if(m_pGameState!=null)
            {
                m_pGameState.Update(fFrameTime);
            }
            if (m_pAgentTree != null) m_pAgentTree.Update(fFrameTime);
        }
    }
}


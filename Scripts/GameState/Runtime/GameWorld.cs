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

namespace Framework.State.Runtime
{
    [ATInteralExport("游戏世界", -6, icon: "GameWorld/gameworld")]
    public class GameWorld : AModule
    {
        private AState      m_pGameState;
        private List<AMode> m_vGameModes;
        AGameWorldObject    m_pWorldObject;
        //--------------------------------------------------------
        public void CreateWorld(AGameWorldObject pObject)
        {
            if (m_pWorldObject == pObject)
                return;
            ClearWorld();
            m_pWorldObject = pObject;
            if (m_pWorldObject == null)
                return;

            m_pGameState = GameWorldHandler.Malloc<AState>(m_pWorldObject.gameStateData.stateType);
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
                        var pMode = GameWorldHandler.Malloc<AMode>(modeData.modeType);
                        if (pMode != null)
                        {
                            pMode.SetState(m_pGameState);
                            pMode.CreateLogics(modeData.modeLogics);
                            m_vGameModes.Add(pMode);
                            if(m_pWorldObject.gameStateData.activeMode == i)
                            {
                                m_pGameState.SetActiveMode(pMode);
                            }
                        }
                        else
                        {
                            Debug.Assert(false, "无法创建游戏模式实例:" + modeData.modeType);
                        }
                    }
                }
            }
            else
            {
                Debug.Assert(false, "无法创建游戏状态实例:" + m_pWorldObject.gameStateData.stateType);
            }
        }
        //--------------------------------------------------------
        [ATMethod("唤醒游戏状态")]
        public void AwakeState()
        {
            if (m_pGameState == null) return;
            m_pGameState.Awake();
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
        protected override void OnUpdate(float fFrameTime)
        {
            if(m_pGameState!=null)
            {
                m_pGameState.Update(fFrameTime);
            }
        }
    }
}


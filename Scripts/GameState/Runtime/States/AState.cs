/********************************************************************
生成日期:	11:07:2025
类    名: 	AState
作    者:	HappLI
描    述:	游戏状态基类
*********************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.State.Runtime
{
    public abstract class AState
    {
        private GameWorld           m_pWorld;
        private AMode               m_pActiveMode;
        private List<AModeLogic>    m_vLogics;
        //----------------------------------------------------------------
        public AState()
        {
            m_pWorld = null;
        }
        //----------------------------------------------------------------
        public GameWorld GetGameWorld()
        {
            return m_pWorld;
        }
        //----------------------------------------------------------------
        internal void SetGameWorld(GameWorld pWorld)
        {
            m_pWorld = pWorld;
        }
        //----------------------------------------------------------------
        internal void SetActiveMode(AMode pMode)
        {
            m_pActiveMode = pMode;
            if (m_pActiveMode != null)
            {
                m_pActiveMode.SetState(this);
            }
        }
        //----------------------------------------------------------------
        public AMode GetActiveMode()
        {
            return m_pActiveMode;
        }
        //----------------------------------------------------------------
        internal void Update(float fFrameTime)
        {
            if(m_vLogics!=null)
            {
                foreach(var db in m_vLogics)
                {
                    db.Update(fFrameTime);
                }
            }
            if (m_pActiveMode != null)
                m_pActiveMode.Update(fFrameTime);
            OnUpdate(fFrameTime);
        }
        //----------------------------------------------------------------
        protected virtual void OnUpdate(float fFrameTime) { }
        //----------------------------------------------------------------
        internal void Destroy()
        {
            OnDestroy();
            if (m_pActiveMode != null)  m_pActiveMode.Destroy();
            if(m_vLogics!=null)
            {
                foreach(var db in m_vLogics)
                {
                    db.Destroy();
                }
                m_vLogics.Clear();
            }
            m_pActiveMode = null;

            m_pWorld = null;
        }
        //----------------------------------------------------------------
        protected virtual void OnDestroy() { }
    }
}


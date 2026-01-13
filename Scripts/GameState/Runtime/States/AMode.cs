/********************************************************************
生成日期:	11:07:2025
类    名: 	AMode
作    者:	HappLI
描    述:	状态模式基类
*********************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.State.Runtime
{
    public abstract class AMode
    {
        private AState m_pState;
        private List<AModeLogic> m_vLogics;
        //----------------------------------------------------------------
        internal void SetState(AState pState)
        {
            m_pState = pState;
        }
        //----------------------------------------------------------------
        public AState GetState()
        {
            return m_pState;
        }  
        //----------------------------------------------------------------
        internal void Update(float fFrameTime)
        {
            if (m_vLogics != null)
            {
                foreach (var db in m_vLogics)
                {
                    db.Update(fFrameTime);
                }
            }
            OnUpdate(fFrameTime);
        }
        //----------------------------------------------------------------
        protected virtual void OnUpdate(float fFrameTime) { }
        //----------------------------------------------------------------
        internal void Destroy()
        {
            OnDestroy();
            if (m_vLogics != null)
            {
                foreach (var db in m_vLogics)
                {
                    db.Destroy();
                }
                m_vLogics.Clear();
            }
        }
        //----------------------------------------------------------------
        protected virtual void OnDestroy() { }
    }
}


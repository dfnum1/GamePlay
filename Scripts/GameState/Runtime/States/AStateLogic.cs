/********************************************************************
生成日期:	11:07:2025
类    名: 	AStateLogic
作    者:	HappLI
描    述:	游戏状态逻辑基类
*********************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.State.Runtime
{
    public abstract class AStateLogic
    {
        private AState m_pState;
        //----------------------------------------------------------------
        internal void Update(float fFrameTime)
        {
            OnUpdate(fFrameTime);
        }
        //----------------------------------------------------------------
        protected virtual void OnUpdate(float fFrameTime) { }
        //----------------------------------------------------------------
        internal void Destroy()
        {
            OnDestroy();
            m_pState = null;
        }
        //----------------------------------------------------------------
        protected virtual void OnDestroy() { }
    }
}


/********************************************************************
生成日期:	11:07:2025
类    名: 	AModeLogic
作    者:	HappLI
描    述:	游戏模式逻辑基类
*********************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.State.Runtime
{
    public abstract class AModeLogic
    {
        private AMode m_pMode;
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
            m_pMode = null;
        }
        //----------------------------------------------------------------
        protected virtual void OnDestroy() { }
    }
}


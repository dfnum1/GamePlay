/********************************************************************
生成日期:	11:07:2025
类    名: 	AStateLogic
作    者:	HappLI
描    述:	游戏状态逻辑基类
*********************************************************************/
using Framework.Core;
using Framework.Base;

#if USE_FIXEDMATH
using ExternEngine;
#else
using FFloat = System.Single;
#endif
namespace Framework.State.Runtime
{
    [StateIcon("gameworld/statelogic")]
    public abstract class AStateLogic : TypeObject
    {
        private AState m_pState;
        //----------------------------------------------------------------
        public AStateLogic()
        {
            m_pState = null;
        }
        //----------------------------------------------------------------
        internal void SetState(AState pState)
        {
            m_pState = pState;
        }
        //----------------------------------------------------------------
        public AMode GetActiveMode()
        {
            if (m_pState == null) return null;
            return m_pState.GetActiveMode();
        }
        //----------------------------------------------------------------
        public AState GetState()
        {
            return m_pState;
        }
        //----------------------------------------------------------------
        public GameWorld GetWorld()
        {
            if (m_pState == null) return null;
            return m_pState.GetGameWorld();
        }
        //----------------------------------------------------------------
        internal void Awake()
        {
            OnAwake();
        }
        //----------------------------------------------------------------
        internal void PreStart()
        {
            OnPreStart();
        }
        //----------------------------------------------------------------
        internal void Start()
        {
            OnStart();
        }
        //----------------------------------------------------------------
        internal void Active(bool bActive)
        {
            OnActive(bActive);
        }
        //----------------------------------------------------------------
        internal void Update(FFloat fFrameTime)
        {
            if (m_pState == null)
                return;
            OnUpdate(fFrameTime);
        }
        //----------------------------------------------------------------
        new internal void Destroy()
        {
            OnDestroy();
            m_pState = null;
        }
        //----------------------------------------------------------------
        protected virtual void OnAwake() { }
        protected virtual void OnPreStart() { }
        protected virtual void OnStart() { }
        protected virtual void OnActive(bool bActive) { }
        protected virtual void OnUpdate(FFloat fFrameTime) { }
        protected virtual void OnDestroy() { }
    }
}


/********************************************************************
生成日期:	11:07:2025
类    名: 	AStateLogic
作    者:	HappLI
描    述:	游戏状态逻辑基类
*********************************************************************/
using Framework.Core;

namespace Framework.State.Runtime
{
    [StateIcon("gameworld/statelogic")]
    public abstract class AStateLogic : TypeObject
    {
        private AState m_pState;
        //----------------------------------------------------------------
        public AStateLogic()
        {
#if UNITY_EDITOR
            GameWorldHandler.CheckInnerMalloc(GetType());
#endif
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
        internal void Update(float fFrameTime)
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
        protected virtual void OnUpdate(float fFrameTime) { }
        protected virtual void OnDestroy() { }
    }
}


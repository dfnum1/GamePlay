/********************************************************************
生成日期:	11:07:2025
类    名: 	AModeLogic
作    者:	HappLI
描    述:	游戏模式逻辑基类
*********************************************************************/
using Framework.Core;
namespace Framework.State.Runtime
{
    public abstract class AModeLogic : TypeObject
    {
        private AMode m_pMode;
        //----------------------------------------------------------------
        public AModeLogic()
        {
#if UNITY_EDITOR
            GameWorldHandler.CheckInnerMalloc(GetType());
#endif
            m_pMode = null;
        }
        //----------------------------------------------------------------
        internal void SetMode(AMode pMode)
        {
            m_pMode = pMode;
        }
        //----------------------------------------------------------------
        public AMode GetMode()
        {
            return m_pMode;
        }
        //----------------------------------------------------------------
        public AState GetState()
        {
            if (m_pMode == null) return null;
            return m_pMode.GetState();
        }
        //----------------------------------------------------------------
        public GameWorld GetWorld()
        {
            return GetState()?.GetGameWorld();
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
            if (m_pMode == null)
                return;
            OnUpdate(fFrameTime);
        }
        //----------------------------------------------------------------
        new internal void Destroy()
        {
            OnDestroy();
            m_pMode = null;
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


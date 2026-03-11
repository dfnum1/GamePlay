/********************************************************************
生成日期:	11:07:2025
类    名: 	AModeLogic
作    者:	HappLI
描    述:	游戏模式逻辑基类
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
    [StateIcon("gameworld/modelogic")]
    public abstract class AModeLogic : TypeObject
    {
        private AMode m_pMode;
        //----------------------------------------------------------------
        public AModeLogic()
        {
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
        public T GetMode<T>() where T : AMode
        {
            return m_pMode as T;
        }
        //----------------------------------------------------------------
        public AState GetState()
        {
            if (m_pMode == null) return null;
            return m_pMode.GetState();
        }
        //----------------------------------------------------------------
        public AFramework GetFramework()
        {
            if (m_pMode == null) return null;
            return m_pMode.GetFramework();
        }
        //----------------------------------------------------------------
        public T GetState<T>() where T : AState
        {
            if (m_pMode == null) return null;
            return m_pMode.GetState<T>();
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
        protected virtual void OnUpdate(FFloat fFrameTime) { }
        protected virtual void OnDestroy() { }
    }
}


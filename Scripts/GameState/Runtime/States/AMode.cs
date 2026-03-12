/********************************************************************
生成日期:	11:07:2025
类    名: 	AMode
作    者:	HappLI
描    述:	状态模式基类
*********************************************************************/
using Framework.Core;
using System.Collections.Generic;
using Framework.Base;

#if USE_FIXEDMATH
using ExternEngine;
#else
using FFloat = System.Single;
#endif
namespace Framework.State.Runtime
{
    public abstract class AMode : TypeObject
    {
        private AState              m_pState;
        private List<AModeLogic>    m_vLogics;
        private byte                m_nAPICallStatus = 0;
        //----------------------------------------------------------------
        public AMode()
        {
            m_pState = null;
            m_nAPICallStatus = 0;
        }
        //----------------------------------------------------------------
        internal void CleanAPIStatus()
        {
            m_nAPICallStatus = 0;
        }
        //----------------------------------------------------------------
        internal bool IsAPIStatus(EAPICallStatus eStatus)
        {
            return (m_nAPICallStatus & (byte)eStatus) != 0;
        }
        //----------------------------------------------------------------
        void EnableAPIStatus(EAPICallStatus eStatus, bool bEnable)
        {
            if (!bEnable)
            {
                m_nAPICallStatus &= (byte)~eStatus;
            }
            else
                m_nAPICallStatus |= (byte)eStatus;
        }
        //----------------------------------------------------------------
        internal void CreateLogics(List<GameStateLogicData> logicTypeIds)
        {
            if (m_vLogics != null)
            {
                foreach (var db in m_vLogics)
                {
                    db.Free();
                }
                m_vLogics.Clear();
            }
            if (logicTypeIds == null)
                return;
            foreach (var db in logicTypeIds)
            {
                if (!db.enabled) continue;
                var pLogic = GameWorldHandler.Malloc<AModeLogic>(GetFramework(), db.logicType);
                if (pLogic != null)
                {
                    if (m_vLogics == null) m_vLogics = new List<AModeLogic>(logicTypeIds.Count);
                    pLogic.SetMode(this);
                    m_vLogics.Add(pLogic);
                }
                else
                {
                    UnityEngine.Debug.Assert(false, "无法创建玩法模式逻辑实例:" + db);
                }
            }
        }
        //----------------------------------------------------------------
        internal void Awake()
        {
            if (IsAPIStatus(EAPICallStatus.Awake))
                return;
            EnableAPIStatus(EAPICallStatus.Awake, true);
            if(m_vLogics!=null)
            {
                foreach (var db in m_vLogics)
                {
                    db.Awake();
                }
            }
            OnAwake();
        }
        //----------------------------------------------------------------
        internal void PreStart()
        {
            if (IsAPIStatus(EAPICallStatus.PreStart))
                return;
            EnableAPIStatus(EAPICallStatus.PreStart, true);
            OnPreStart();
            if (m_vLogics != null)
            {
                foreach (var db in m_vLogics)
                {
                    db.PreStart();
                }
            }
        }
        //----------------------------------------------------------------
        internal void Start()
        {
            if (IsAPIStatus(EAPICallStatus.Start))
                return;
            EnableAPIStatus(EAPICallStatus.Start, true);
            OnStart();
            if (m_vLogics != null)
            {
                foreach (var db in m_vLogics)
                {
                    db.Start();
                }
            }
        }
        //----------------------------------------------------------------
        internal void Active(bool bActive)
        {
            if (IsAPIStatus(EAPICallStatus.Active) == bActive)
                return;
            EnableAPIStatus(EAPICallStatus.Active, bActive);
            OnActive(bActive);
            if (m_vLogics != null)
            {
                foreach (var db in m_vLogics)
                {
                    db.Active(bActive);
                }
            }
        }
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
        public T GetState<T>() where T : AState
        {
            return m_pState as T;
        }
        //----------------------------------------------------------------
        public GameWorld GetGameWorld()
        {
            return m_pState?.GetGameWorld();
        }
        //----------------------------------------------------------------
        public AFramework GetFramework()
        {
            if (m_pState == null) return null;
            return m_pState.GetFramework();
        }
        //----------------------------------------------------------------
        public FileSystem GetFileSystem()
        {
            var pFramework = GetFramework();
            if (pFramework == null) return null;
            return pFramework.GetFileSystem();
        }
        //----------------------------------------------------------------
        internal void Update(FFloat fFrameTime)
        {
            if (m_pState == null || !IsAPIStatus(EAPICallStatus.CanUpdateFlag))
                return;
            OnUpdate(fFrameTime);
            if (m_vLogics != null)
            {
                foreach (var db in m_vLogics)
                {
                    db.Update(fFrameTime);
                }
            }
        }
        //----------------------------------------------------------------
        new internal void Destroy()
        {
            if (IsAPIStatus(EAPICallStatus.Destroy))
                return;

            EnableAPIStatus(EAPICallStatus.Destroy, true);

            if (m_vLogics != null)
            {
                foreach (var db in m_vLogics)
                {
                    db.Free();
                }
                m_vLogics.Clear();
            }
            OnDestroy();
            m_pState = null;
        }
        //----------------------------------------------------------------
        public bool IsDestroyed()
        {
            return IsAPIStatus(EAPICallStatus.Destroy);
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


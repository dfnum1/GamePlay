/********************************************************************
生成日期:	11:07:2025
类    名: 	AMode
作    者:	HappLI
描    述:	状态模式基类
*********************************************************************/
using Framework.Core;
using System.Collections.Generic;

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
#if UNITY_EDITOR
            GameWorldHandler.CheckInnerMalloc(GetType());
#endif
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
        internal void CreateLogics(List<int> logicTypeIds)
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
                var pLogic = GameWorldHandler.Malloc<AModeLogic>(db);
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
            if (m_vLogics != null)
            {
                foreach (var db in m_vLogics)
                {
                    db.PreStart();
                }
            }
            OnPreStart();
        }
        //----------------------------------------------------------------
        internal void Start()
        {
            if (IsAPIStatus(EAPICallStatus.Start))
                return;
            EnableAPIStatus(EAPICallStatus.Start, true);
            if (m_vLogics != null)
            {
                foreach (var db in m_vLogics)
                {
                    db.Start();
                }
            }
            OnStart();
        }
        //----------------------------------------------------------------
        internal void Active(bool bActive)
        {
            if (IsAPIStatus(EAPICallStatus.Active) == bActive)
                return;
            EnableAPIStatus(EAPICallStatus.Active, bActive);
            if (m_vLogics != null)
            {
                foreach (var db in m_vLogics)
                {
                    db.Active(bActive);
                }
            }
            OnActive(bActive);
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
        public GameWorld GetGameWorld()
        {
            return m_pState?.GetGameWorld();
        }
        //----------------------------------------------------------------
        internal void Update(float fFrameTime)
        {
            if (m_pState == null || !IsAPIStatus(EAPICallStatus.CanUpdateFlag))
                return;
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
        new internal void Destroy()
        {
            if (IsAPIStatus(EAPICallStatus.Destroy))
                return;

            EnableAPIStatus(EAPICallStatus.Destroy, true);

            OnDestroy();
            if (m_vLogics != null)
            {
                foreach (var db in m_vLogics)
                {
                    db.Free();
                }
                m_vLogics.Clear();
            }
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


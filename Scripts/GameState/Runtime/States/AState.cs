/********************************************************************
生成日期:	11:07:2025
类    名: 	AState
作    者:	HappLI
描    述:	游戏状态基类
*********************************************************************/
using Framework.Core;
using System.Collections.Generic;

namespace Framework.State.Runtime
{
    internal enum EAPICallStatus
    {
        Awake = 1<<0,
        PreStart = 1 << 1,
        Start = 1<<2,
        Active = 1<<3,
        Destroy = 1<<4,

        CanUpdateFlag= Awake | Start | Active,
    }
    public abstract class AState : TypeObject
    {
        private GameWorld           m_pWorld;
        private AMode               m_pActiveMode;
        private List<AStateLogic>   m_vLogics;
        private byte                m_nAPICallStatus = 0;
        //----------------------------------------------------------------
        public AState()
        {
#if UNITY_EDITOR
            GameWorldHandler.CheckInnerMalloc(GetType());
#endif
            m_pWorld = null;
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
            if(m_vLogics!=null)
            {
                foreach(var db in m_vLogics)
                {
                    db.Free();
                }
                m_vLogics.Clear();
            }
            if (logicTypeIds == null)
                return;
            foreach(var db in logicTypeIds)
            {
                var pLogic = GameWorldHandler.Malloc<AStateLogic>(db);
                if (pLogic != null)
                {
                    if (m_vLogics == null) m_vLogics = new List<AStateLogic>(logicTypeIds.Count);
                    pLogic.SetState(this);
                    m_vLogics.Add(pLogic);
                }
                else 
                { 
                    UnityEngine.Debug.Assert(false, "无法创建状态逻辑实例:" + db);
                }
            }
        }
        //----------------------------------------------------------------
        internal void Awake()
        {
            if (IsAPIStatus(EAPICallStatus.Awake))
                return;
            EnableAPIStatus(EAPICallStatus.Awake, true);
            if (m_vLogics!=null)
            {
                foreach(var db in m_vLogics)
                {
                    db.Awake();
                }
            }
            if(m_pActiveMode!=null) m_pActiveMode.Awake();
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
            if (m_pActiveMode != null) m_pActiveMode.PreStart();
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
            if (m_pActiveMode != null) m_pActiveMode.Start();
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
            if (m_pActiveMode != null) m_pActiveMode.Active(bActive);
            OnActive(bActive);
        }
        //----------------------------------------------------------------
        internal bool IsActive()
        {
            return IsAPIStatus(EAPICallStatus.Active);
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
            if (m_pActiveMode == pMode)
                return;

            m_pActiveMode = pMode;
            if (m_pActiveMode != null)
            {
                m_pActiveMode.SetState(this);
            }
            if(IsAPIStatus(EAPICallStatus.Awake)) m_pActiveMode.Awake();
            if(IsAPIStatus(EAPICallStatus.PreStart)) m_pActiveMode.PreStart();
            if(IsAPIStatus(EAPICallStatus.Start)) m_pActiveMode.Start();
            if(IsAPIStatus(EAPICallStatus.Active)) m_pActiveMode.Active(true);
        }
        //----------------------------------------------------------------
        public AMode GetActiveMode()
        {
            return m_pActiveMode;
        }
        //----------------------------------------------------------------
        internal void Update(float fFrameTime)
        {
            if (m_pWorld == null || !IsAPIStatus(EAPICallStatus.CanUpdateFlag))
                return;

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
        new internal void Destroy()
        {
            if (IsAPIStatus(EAPICallStatus.Destroy))
                return;

            EnableAPIStatus(EAPICallStatus.Destroy, true);
            OnDestroy();
            if(m_vLogics!=null)
            {
                foreach(var db in m_vLogics)
                {
                    db.Free();
                }
                m_vLogics.Clear();
            }
            m_pActiveMode = null;
            m_pWorld = null;
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


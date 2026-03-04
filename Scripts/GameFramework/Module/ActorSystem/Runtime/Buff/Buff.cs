#if USE_ACTORSYSTEM
/********************************************************************
生成日期:	5:11:2020  20:36
类    名: 	Buff
作    者:	HappLI
描    述:	buff系统-单个buff
*********************************************************************/
using Framework.AT.Runtime;
using Framework.Data;
using System.Collections.Generic;
using UnityEngine;
using Framework.Core;
using Framework.Base;




#if USE_FIXEDMATH
using ExternEngine;
#else
using FFloat = System.Single;
#endif

namespace Framework.ActorSystem.Runtime
{
    [ATInteralExport("Actor系统/Buff", -4, "ActorSystem/actor_buff")]
    public class Buff : AActorStateInfo
    {
        enum EStatus : byte
        {
            eNone,
            eBegin,
            eTicking,
            eEnd,
        }
        protected byte                  m_nBuffType = 0;//增益、减益、控制等类型
        private EStatus                 m_eStatus = EStatus.eNone;
        private uint                    m_nLevel = 0;

        private byte[]                  m_arrAttrTypes = null;
        private byte[]                  m_arrAttrValueTypes = null;
        private FFloat[]                m_arrAttrValues = null;

        private float                   m_fBeginScale = 1;
        private string                  m_strBeginEffect = null;
        private string                  m_strBeginSound = null;
        private InstanceAble            m_BeginEffect = null;

        private float                   m_fStepScale = 1;
        private string                  m_strStepEffect = null;
        private string                  m_strStepSound = null;
        private InstanceAble            m_StepEffect = null;

        private float                   m_fEndScale = 1;
        private string                  m_strEndEffect = null;
        private string                  m_strEndSound = null;
        private InstanceAble            m_EndEffect = null;

        private string                  m_strBindSlot = null;
        private Vector3                 m_BindOffset = Vector3.zero;
        private Vector3                 m_BindRotateOffset = Vector3.zero;
        private byte                    m_nBindBit = (byte)ESlotBindBit.All;

        protected IContextData          m_pConfigData = null;
        protected BuffSystem            m_pOwner = null;
        protected bool                  m_bActived = false;

        private AOperatorHandle         m_BeginOpHandler = null;
        private AOperatorHandle         m_StepOpHandler = null;
        private AOperatorHandle         m_EndOpHandler = null;
        //-----------------------------------------------------
        public Buff()
        {
            m_pOwner = null;
        }
        //-----------------------------------------------------
        public void SetConfigData(IContextData pConfigData)
        {
            if (m_pConfigData == pConfigData)
                return;
            m_pConfigData = pConfigData;
            OnConfigData();
        }
        //-----------------------------------------------------
        protected virtual void OnConfigData() { }
        //-----------------------------------------------------
        public Actor GetActor()
        {
            if (m_pOwner != null)
                return m_pOwner.GetActor();
            return null;
        }
        //-----------------------------------------------------
        internal void SetSystem(BuffSystem pSystem)
        {
            m_pOwner = pSystem;
        }
        //-----------------------------------------------------
        public virtual void OnInit()
        {

        }
        //-----------------------------------------------------
        internal bool Update(FFloat fFrame)
        {
            if (!m_bActived)
                return true;

            switch(m_eStatus)
            {
                case EStatus.eNone:
                    {
                        if (m_BeginOpHandler != null) m_BeginOpHandler.Free();
                        m_BeginOpHandler = GetFramework().GetFileSystem().SpawnInstance(m_strBeginEffect, OnInstanceCallback);
                        m_BeginOpHandler.SetUserData(0, new Value1Var() { intVal = (int)m_eStatus });
                        m_eStatus = EStatus.eBegin;
                    }
                    break;
                case EStatus.eBegin:
                    {
                        if (m_StepOpHandler != null) m_StepOpHandler.Free();
                        m_StepOpHandler = GetFramework().GetFileSystem().SpawnInstance(m_strStepEffect, OnInstanceCallback);
                        m_StepOpHandler.SetUserData(0, new Value1Var() { intVal = (int)m_eStatus });
                        m_eStatus = EStatus.eTicking;
                    }
                    break;
                case EStatus.eTicking:
                    {
                        bool isEnd = false;

                        if(isEnd)
                        {
                            if (m_EndOpHandler != null) m_EndOpHandler.Free();
                            m_EndOpHandler = GetFramework().GetFileSystem().SpawnInstance(m_strEndEffect, OnInstanceCallback);
                            m_EndOpHandler.SetUserData(0, new Value1Var() { intVal = (int)m_eStatus });
                            m_eStatus = EStatus.eEnd;
                        }
                    }
                    break;
                case EStatus.eEnd:
                    {

                    }
                    break;
            }
            OnUpdate(fFrame);
            return true;
        }
        //-----------------------------------------------------
        public bool IsEnd()
        {
            return m_eStatus == EStatus.eEnd;
        }
        //-----------------------------------------------------
        public bool IsActived()
        {
            return m_bActived;
        }
        //-----------------------------------------------------
        public void SetActived(bool bActive)
        {
            m_bActived = bActive;
        }
        //-----------------------------------------------------
        public ActorManager GetActorManager()
        {
            if (m_pOwner != null)
                return m_pOwner.GetActorManager();
            return null;
        }
        //-----------------------------------------------------
        [ATMethod("设置等级")]
        public void SetLevel(uint nLevel)
        {
            m_nLevel = nLevel;
        }
        //-----------------------------------------------------
        public IContextData GetConfigData()
        {
            return m_pConfigData;
        }
        //-----------------------------------------------------
        protected virtual void OnUpdate(FFloat fFrame) 
        {
        }
        //-----------------------------------------------------
        public override void Destroy()
        {
            m_pOwner = null;
            m_pConfigData = null;
            m_bActived = false;
            m_eStatus = EStatus.eNone;
            if(m_BeginOpHandler!=null)
            {
                m_BeginOpHandler.Free(); m_BeginOpHandler = null;
            }
            if (m_StepOpHandler != null)
            {
                m_StepOpHandler.Free(); m_StepOpHandler = null;
            }
            if (m_EndOpHandler != null)
            {
                m_EndOpHandler.Free(); m_EndOpHandler = null;
            }
            if (m_BeginEffect != null) m_BeginEffect.Destroy();
            m_BeginEffect = null;

            if (m_StepEffect != null) m_StepEffect.Destroy();
            m_StepEffect = null;

            if (m_EndEffect != null) m_EndEffect.Destroy();
            m_EndEffect = null;
        }
        //-----------------------------------------------------
        public override void AddLockTarget(Actor pNode, bool bClear = false)
        {
        }
        //-----------------------------------------------------
        public override void ClearLockTargets()
        {
        }
        //-----------------------------------------------------
        public override List<Actor> GetLockTargets(bool isEmptyReLock = true)
        {
            return null;
        }
        //-----------------------------------------------------
        void OnInstanceCallback(InstanceOperator pCallback, bool doSignCheck)
        {
            if(doSignCheck)
            {
                pCallback.SetUsed(IsActived() && m_eStatus != EStatus.eNone);
                if(!pCallback.IsUsed())
                {
                    if (pCallback == m_BeginOpHandler)
                    {
                        m_BeginOpHandler = null;
                    }
                    else if (pCallback == m_StepOpHandler)
                    {
                        m_StepOpHandler = null;
                    }
                    else if (pCallback == m_EndOpHandler)
                    {
                        m_EndOpHandler = null;
                    }
                }
            }
            else
            {
                EStatus type = (EStatus)pCallback.GetUserData<Value1Var>(0).ToInt();
                var pAble = pCallback.GetInstanceAble();
                if (pAble != null)
                {
                    if (type == EStatus.eBegin) m_BeginEffect = pAble;
                    else if (type == EStatus.eTicking) m_StepEffect = pAble;
                    else if (type == EStatus.eEnd) m_EndEffect = pAble;
                }
                if (pCallback == m_BeginOpHandler)
                {
                    m_BeginOpHandler = null;
                }
                else if (pCallback == m_StepOpHandler)
                {
                    m_StepOpHandler = null;
                }
                else if (pCallback == m_EndOpHandler)
                {
                    m_EndOpHandler = null;
                }
            }
        }
    }
}
#endif
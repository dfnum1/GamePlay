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
    [ATInteralExport("Actor系统/Buff", -10, "ActorSystem/actor_buff")]
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
        private FFloat                  m_fTickStep = 0;
        private FFloat                  m_fLife = 0;
        private FFloat                  m_fTime = 0;
        private FFloat                  m_fTickTime = 0;
        private int                     m_nTickCount = 0;

        private uint                    m_nEffectStateFlags = 0;

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
            m_nEffectStateFlags = 0;
            m_bActived = false;
            m_eStatus = EStatus.eNone;
            m_nLevel = 0;
            m_fTickStep = 0;
            m_fLife = 0;
            m_fTime = 0;
            m_fTickTime = 0;
            m_nTickCount = 0;
        }
        //-----------------------------------------------------
        public void SetEffectStateFlags(uint effectStateFlag)
        {
            m_nEffectStateFlags = effectStateFlag;
        }
        //-----------------------------------------------------
        public void ClearEffectState()
        {
            m_nEffectStateFlags = 0;
        }
        //-----------------------------------------------------
        [ATMethod("是有拥有Buff状态"), ATArgvDrawer("effectState", "BuffStateDraw")]
        public bool HasEffectState(uint effectState)
        {
            if (effectState >= 32) return false;
            return (m_nEffectStateFlags & (1u << (int)effectState)) != 0;
        }
        //-----------------------------------------------------
        [ATMethod("添加Buff状态"), ATArgvDrawer("effectState", "BuffStateDraw")]
        public void AddEffectState(uint effectState)
        {
            if (effectState >= 32) return;
            m_nEffectStateFlags |= (1u << (int)effectState);
        }
        //-----------------------------------------------------
        [ATMethod("移除Buff状态"), ATArgvDrawer("effectState", "BuffStateDraw")]
        public void RemoveEffectState(uint effectState)
        {
            if (effectState >= 32) return;
            m_nEffectStateFlags &= ~(1u << (int)effectState);
        }
        //-----------------------------------------------------
        internal void CollectStats(Dictionary<byte,BuffAttr> vAttrs)
        {
            if (m_arrAttrTypes == null || m_arrAttrValues == null) return;
            for (int i = 0; i < m_arrAttrTypes.Length; ++i)
            {
                if (i >= m_arrAttrValues.Length)
                    continue;
                if (!vAttrs.TryGetValue(m_arrAttrTypes[i], out var attr))
                {
                    attr = new BuffAttr();
                    attr.Clear();
                    vAttrs[m_arrAttrTypes[i]] = attr;
                }
                if(m_arrAttrValueTypes!=null && i < m_arrAttrValueTypes.Length && m_arrAttrValueTypes[i] == (byte)EAttrValueType.eRate)
                    attr.AddRate(m_arrAttrValues[i]*m_nTickCount);
                else
                    attr.AddValue(m_arrAttrValues[i] * m_nTickCount);
            }
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
                        m_nTickCount = 1;
                        m_fTime = 0;
                        m_fTickTime = 0;
                        if (m_StepOpHandler != null) m_StepOpHandler.Free();
                        m_StepOpHandler = GetFramework().GetFileSystem().SpawnInstance(m_strStepEffect, OnInstanceCallback);
                        m_StepOpHandler.SetUserData(0, new Value1Var() { intVal = (int)m_eStatus });
                        m_eStatus = EStatus.eTicking;
                        OnTick();
                    }
                    break;
                case EStatus.eTicking:
                    {
                        bool isEnd = false;
                        m_fTime += fFrame;
                        if(m_fTickStep>0)
                        {
                            m_fTickTime += fFrame;
                            if(m_fTickTime >= m_fTickStep)
                            {
                                //! Tick todo
                                m_nTickCount++;
                                m_fTickTime = 0;
                                OnTick();
                            }
                        }
                        if(m_fTime >= m_fLife)
                        {
                            isEnd = true;
                        }
                        if (isEnd)
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
        [ATMethod("是否结束")]
        public bool IsEnd()
        {
            return m_eStatus == EStatus.eEnd;
        }
        //-----------------------------------------------------
        [ATMethod("是否激活")]
        public bool IsActived()
        {
            return m_bActived;
        }
        //-----------------------------------------------------
        [ATMethod("设置激活")]
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
        [ATMethod("获取等级")]
        public uint GetLevel()
        {
            return m_nLevel;
        }
        //-----------------------------------------------------
        [ATMethod("获取Tick次数")]
        public int GetTickCount()
        {
            return m_nTickCount;
        }
        //-----------------------------------------------------
        public IContextData GetConfigData()
        {
            return m_pConfigData;
        }
        //-----------------------------------------------------
        protected virtual void OnTick()
        {

        }
        //-----------------------------------------------------
        [ATMethod("获取Buff属性"), ATArgvDrawer("type", "DrawAttributePop")]
        public virtual FFloat GetAttr(byte type)
        {
            if (m_arrAttrTypes == null || m_arrAttrValues == null) return 0;
            for(int i =0; i < m_arrAttrTypes.Length; ++i)
            {
                if (m_arrAttrTypes[i] == type)
                {
                    if (i < m_arrAttrValues.Length)
                        return m_arrAttrValues[i]*m_nTickCount;
                }
            }
            return FFloat.zero;
        }
        //-----------------------------------------------------
        [ATMethod("获取Buff属性值类型"), ATArgvDrawer("type", "DrawAttributePop")]
        public EAttrValueType GetAttrValueType(byte type)
        {
            if (m_arrAttrTypes == null || m_arrAttrValueTypes == null) return EAttrValueType.eValue;
            for (int i = 0; i < m_arrAttrTypes.Length; ++i)
            {
                if (m_arrAttrTypes[i] == type)
                {
                    if (i < m_arrAttrValueTypes.Length)
                        return (EAttrValueType)m_arrAttrValueTypes[i];
                }
            }
            return EAttrValueType.eValue;
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

            if (m_EndEffect != null) m_EndEffect.Destroy(10);
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
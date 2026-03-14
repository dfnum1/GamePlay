#if USE_ACTORSYSTEM
/********************************************************************
生成日期:	5:11:2020  20:36
类    名: 	Buff
作    者:	HappLI
描    述:	buff系统-单个buff
*********************************************************************/
using Framework.AT.Runtime;
using System.Collections.Generic;
using UnityEngine;


#if USE_FIXEDMATH
using ExternEngine;
#else
using FFloat = System.Single;
#endif

namespace Framework.ActorSystem.Runtime
{
    [ATInteralExport("Actor系统/Buff对象", -10, "ActorSystem/actor_buff")]
    public class Buff : AActorStateInfo
    {
        enum EStatus : byte
        {
            eNone,
            eBegin,
            eTicking,
            eEnd,
            eOver,
        }
        protected byte                  m_nBuffType = 0;//增益、减益、控制等类型
        protected EBuffTargetType       m_eBuffTargetType = EBuffTargetType.eTargeter;
        private EStatus                 m_eStatus = EStatus.eNone;
        private uint                    m_nLevel = 0;
        private FFloat                  m_fTickStep = 0;
        private FFloat                  m_fLife = 0;
        private FFloat                  m_fTime = 0;
        private FFloat                  m_fTickTime = 0;
        private int                     m_nTickCount = 0;
        private bool                    m_bInstant = true;  //!<是否即时生效的Buff，非即时生效的Buff在Begin阶段不计算属性等效果，直到Ticking阶段才开始计算属性等效果
        private bool                    m_bDieKeep = false; //!<是否死亡保留，死亡保留的Buff在单位死亡后仍然保留，直到Buff结束或者复活后才移除
        private ushort                  m_nLayer = 1;
        private ushort                  m_nMaxLayer = 1;

        private byte                    m_nAttackFinishCnt = 0;
        private byte                    m_nHitFinishCnt = 0;

        private uint                    m_nEffectStateFlags = 0;

        private byte[]                  m_arrAttrTypes = null;
        private byte[]                  m_arrAttrValueTypes = null;
        private FFloat[]                m_arrAttrValues = null;

        private int[]                   m_arrBeginEvents = null;
        private float                   m_fEffectScale = 1;
        private string                  m_strBeginEffect = null;
        private string                  m_strBeginSound = null;

        private int[]                   m_arrStepEvents = null;
        private string                  m_strStepEffect = null;
        private string                  m_strStepSound = null;

        private int[]                   m_arrEndEvents = null;
        private string                  m_strEndEffect = null;
        private string                  m_strEndSound = null;

        private string                  m_strBindSlot = null;
        private Vector3                 m_BindOffset = Vector3.zero;
        private Vector3                 m_BindRotateOffset = Vector3.zero;
        private byte                    m_nBindBit = (byte)ESlotBindBit.All;

        protected IContextData          m_pConfigData = null;
        protected BuffSystem            m_pOwner = null;
        protected bool                  m_bActived = false;

        private int                     m_BeginEffectId = 0;
        private int                     m_EndEffceId = 0;
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
        public override Actor GetOwner()
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
        [ATMethod("获取Buff状态"), ATArgvDrawer("#return#", BaseATDrawerKey.Key_BuffStateDraw)]
        public uint GetEffectState()
        {
            return m_nEffectStateFlags;
        }
        //-----------------------------------------------------
        [ATMethod("是有拥有Buff状态"), ATArgvDrawer("effectState", BaseATDrawerKey.Key_BuffStateDraw)]
        public bool HasEffectState(uint effectState)
        {
            if (effectState >= 32) return false;
            return (m_nEffectStateFlags & (1u << (int)effectState)) != 0;
        }
        //-----------------------------------------------------
        [ATMethod("添加Buff状态"), ATArgvDrawer("effectState", BaseATDrawerKey.Key_BuffStateDraw)]
        public void AddEffectState(uint effectState)
        {
            if (effectState >= 32) return;
            uint last = m_nEffectStateFlags;
            m_nEffectStateFlags |= (1u << (int)effectState);

            if (last != m_nEffectStateFlags)
            {
                var agrvs = VariableList.Malloc(GetFramework());
                agrvs.AddUserData(GetOwner());
                agrvs.AddUserData(this);
                agrvs.AddInt((int)effectState);
                GetActorManager()?.OnTaskGlobalAT((int)EActorATType.onAddBuffState, agrvs, false);
                GetActorManager()?.OnTaskGlobalAT((int)EActorATType.onAddBuffState, agrvs, true);
            }
        }
        //-----------------------------------------------------
        [ATMethod("移除Buff状态"), ATArgvDrawer("effectState", BaseATDrawerKey.Key_BuffStateDraw)]
        public void RemoveEffectState(uint effectState)
        {
            if (effectState >= 32) return;
            uint last = m_nEffectStateFlags;
            m_nEffectStateFlags &= ~(1u << (int)effectState);
            if(last != m_nEffectStateFlags)
            {
                var agrvs = VariableList.Malloc(GetFramework());
                agrvs.AddUserData(GetOwner());
                agrvs.AddUserData(this);
                agrvs.AddInt((int)effectState);
                GetActorManager()?.OnTaskGlobalAT((int)EActorATType.onRemoveBuffState, agrvs, false);
                GetActorManager()?.OnTaskGlobalAT((int)EActorATType.onRemoveBuffState, agrvs, true);
            }
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
                    attr.AddRate(m_arrAttrValues[i]*(int)(m_nTickCount*m_nLayer));
                else
                    attr.AddValue(m_arrAttrValues[i] * (int)(m_nTickCount* m_nLayer));
            }
        }
        //-----------------------------------------------------
        void DoEvent(int[] events)
        {
            if (events == null) return;
            var pAT = GetOwner().GetAgent<ActorAgentTree>(true);
            if (pAT != null)
            {
                VariableList vArgvs = VariableList.Malloc(GetFramework());
                vArgvs.AddUserData(GetOwner());
                vArgvs.AddUserData(this);
                vArgvs.AddUserData(GetConfigData());
                for (int i = 0; i < events.Length; ++i)
                {
                    pAT.ExecuteEvent(events[i], vArgvs, false);
                }
                vArgvs.Release();
            }
        }
        //-----------------------------------------------------
        internal void OnAttack(Skill pSkill)
        {
            if(m_nAttackFinishCnt>0)
            {
                m_nAttackFinishCnt--;
                if(m_nAttackFinishCnt<=0)
                {
                    m_eStatus = EStatus.eEnd;
                }
            }
        }
        //-----------------------------------------------------
        internal void OnHit(HitFrameActor hifFrame)
        {
            if(m_nHitFinishCnt >0)
            {
                m_nHitFinishCnt--;
                if (m_nHitFinishCnt <= 0)
                {
                    m_eStatus = EStatus.eEnd;
                }
            }
        }
        //-----------------------------------------------------
        internal bool Update(FFloat fFrame)
        {
            bool bDirty = false;
            if (!m_bActived)
                return false;

            switch(m_eStatus)
            {
                case EStatus.eNone:
                    {
#if !USE_SERVER
                        if(m_BeginEffectId!=0) GetOwner().GetAgent<ActorPartAgent>(false)?.DeletePart(m_BeginEffectId);
                        m_BeginEffectId = GetOwner().GetAgent<ActorPartAgent>(true).AddPart(m_strBeginEffect, m_BindOffset, m_BindRotateOffset, Vector3.one * m_fEffectScale, m_strBindSlot, m_nBindBit, m_bDieKeep);
                        GetFramework().PlaySound(m_strBeginSound, GetOwner());
#endif
                        DoEvent(m_arrBeginEvents);
                        m_eStatus = EStatus.eBegin;
                    }
                    break;
                case EStatus.eBegin:
                    {
                        m_nTickCount = m_bInstant?1:0;
                        m_fTime = 0;
                        m_fTickTime = 0;
                        m_eStatus = EStatus.eTicking;
                        if (m_bInstant)
                        {
                            bDirty = true;
                            OnTick();
                        }
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
                                bDirty = true;
                                //! Tick todo
                                m_nTickCount++;
                                m_fTickTime = 0;
                                OnTick();
#if !USE_SERVER
                                GetOwner().GetAgent<ActorPartAgent>(true).AddPart(m_strStepEffect, m_BindOffset, m_BindRotateOffset, Vector3.one * m_fEffectScale, m_strBindSlot, m_nBindBit, m_bDieKeep);
                                GetFramework().PlaySound(m_strStepSound, GetOwner());
#endif
                                DoEvent(m_arrStepEvents);
                            }
                        }
                        if(m_fTime >= m_fLife)
                        {
                            isEnd = true;
                        }
                        if (isEnd)
                        {
                            m_eStatus = EStatus.eEnd;
                        }
                    }
                    break;
                case EStatus.eEnd:
                    {
#if !USE_SERVER
                        if (m_EndEffceId != 0) GetOwner().GetAgent<ActorPartAgent>(false)?.DeletePart(m_EndEffceId);
                        m_EndEffceId = GetOwner().GetAgent<ActorPartAgent>(true).AddPart(m_strEndEffect, m_BindOffset, m_BindRotateOffset, Vector3.one * m_fEffectScale, m_strBindSlot, m_nBindBit, m_bDieKeep);
                        GetFramework().PlaySound(m_strEndSound, GetOwner());
#endif
                        DoEvent(m_arrEndEvents);
                        m_eStatus = EStatus.eOver;
                        bDirty = true;
                    }
                    break;
            }
            OnUpdate(fFrame);
            return bDirty;
        }
        //-----------------------------------------------------
        [ATMethod("是否结束")]
        public bool IsEnd()
        {
            return m_eStatus == EStatus.eOver;
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
            if (m_bActived == bActive)
                return;

            m_bActived = bActive;
            var agrvs = VariableList.Malloc(GetFramework());
            agrvs.AddUserData(GetOwner());
            agrvs.AddUserData(this);
            agrvs.AddBool(m_bActived);
            GetActorManager()?.OnTaskGlobalAT((int)EActorATType.onActiveBuff, agrvs,true);
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
        [ATMethod("设置层数")]
        public void SetLayer(ushort nLayer)
        {
            m_nLayer = (ushort)Mathf.Min(m_nMaxLayer, nLayer);
        }
        //-----------------------------------------------------
        [ATMethod("获取层数")]
        public ushort GetLayer()
        {
            return m_nLayer;
        }
        //-----------------------------------------------------
        [ATMethod("设置最大层数")]
        public void SetMaxLayer(ushort nLayer)
        {
            m_nMaxLayer = nLayer;
        }
        //-----------------------------------------------------
        [ATMethod("获取最大层数")]
        public ushort GetmaxLayer()
        {
            return m_nMaxLayer;
        }
        //-----------------------------------------------------
        [ATMethod("获取Tick次数")]
        public int GetTickCount()
        {
            return m_nTickCount;
        }
        //-----------------------------------------------------
        [ATMethod("获取配置数据")]
        public IContextData GetConfigData()
        {
            return m_pConfigData;
        }
        //-----------------------------------------------------
        [ATMethod("设置死亡保持")]
        public void SetDieKeep(bool bDieKeep)
        {
            m_bDieKeep = bDieKeep;
        }
        //-----------------------------------------------------
        [ATMethod("是否死亡保持")]
        public bool IsDieKeep()
        {
            return m_bDieKeep;
        }
        //-----------------------------------------------------
        protected virtual void OnTick()
        {

        }
        //-----------------------------------------------------
        [ATMethod("获取Buff属性"), ATArgvDrawer("type", BaseATDrawerKey.Key_DrawAttributePop)]
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
            return 0.0f;
        }
        //-----------------------------------------------------
        [ATMethod("获取Buff属性值类型"), ATArgvDrawer("type", BaseATDrawerKey.Key_DrawAttributePop)]
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
            m_eStatus = EStatus.eOver;
            var part = GetOwner().GetAgent<ActorPartAgent>();
            if(part!=null)
            {
                part.DeletePart(m_BeginEffectId);
                part.DeletePart(m_EndEffceId);
                m_EndEffceId = 0;
                m_BeginEffectId = 0;
            }
            m_nLayer = 1;
            m_nMaxLayer = 1;
            m_nAttackFinishCnt = 0;
            m_nHitFinishCnt = 0;
            m_bDieKeep = false;
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
    }
}
#endif
#if USE_ACTORSYSTEM
/********************************************************************
生成日期:	5:11:2020  20:36
类    名: 	Skill
作    者:	HappLI
描    述:	技能系统-单个技能
*********************************************************************/
using Framework.AT.Runtime;
using Framework.Data;
using System.Collections.Generic;
using UnityEngine;


#if USE_FIXEDMATH
using ExternEngine;
#else
using FFloat = System.Single;
#endif

namespace Framework.ActorSystem.Runtime
{
    [ATInteralExport("Actor系统/技能", -4, "ActorSystem/actor_skill")]
    public class Skill : AActorStateInfo
    {
        protected uint              m_nSkillID = 0;
        protected uint              m_nLevel = 0;

        private EActionStateType    m_ActionType = EActionStateType.None;
        private uint                m_nActionTag = 0;

        protected IContextData      m_pConfigData = null;

        protected long              m_lConfigCD = 0;

        protected long              m_LastTriggerTime = 0;
        protected int               m_nTriggerCount  =0;
        protected int               m_nAttrFormulaType = 0;

        protected bool              m_bActived = false;
        protected SkillSystem       m_pOwner = null;
        //-----------------------------------------------------
        public Skill()
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
        internal void SetSkillSystem(SkillSystem pSystem)
        {
            m_pOwner = pSystem;
        }
        //-----------------------------------------------------
        public virtual void OnInit()
        {
            m_LastTriggerTime = 0;
            m_nTriggerCount = 0;
        }
        //-----------------------------------------------------
        public void Update(FFloat fFrame)
        {
            if (!m_bActived)
                return;
            OnUpdate(fFrame);
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
        public virtual void SetUsed()
        {
            m_nTriggerCount++;
            m_LastTriggerTime = GetActorManager().GetRunTime();
        }
        //-----------------------------------------------------
        public float GetRuntimeNormalCD()
        {
            if (GetConfigCD() <= 0)
                return 0;
            long nCurTime = GetActorManager().GetRunTime();
            long nDelta = nCurTime - m_LastTriggerTime;
            if (nDelta < 0) return 0;
            if (nDelta >= GetConfigCD()) return 0;
            return Mathf.Clamp01(nDelta / GetConfigCD());
        }
        //-----------------------------------------------------
        [ATMethod("获得当前CD")]
        public float GetRuntimeCD()
        {
            if (GetConfigCD() <= 0)
                return 0;
            long nCurTime = GetActorManager().GetRunTime();
            long nDelta = nCurTime - m_LastTriggerTime;
            if (nDelta < 0) nDelta = 0;
            if (nDelta >= GetConfigCD()) return 0;
            float fCD = nDelta / 1000.0f;
            if (fCD < 0) fCD = 0;
            return fCD;
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
        [ATMethod("是否可触发")]
        public bool CanTrigger()
        {
            if (!m_bActived || m_nLevel<=0) return false;
            if (GetRuntimeCD() > 0) return false;
            return CheckCanTrigger();
        }
        //-----------------------------------------------------
        [ATMethod("添加索敌目标")]
        public override void AddLockTarget(Actor pNode, bool bClear = false)
        {
            m_pOwner.AddLockTarget(pNode,bClear);
        }
        //-----------------------------------------------------
        [ATMethod("清理索敌目标")]
        public override void ClearLockTargets()
        {
            m_pOwner.ClearLockTargets();
        }
        //------------------------------------------------------
        public override List<Actor> GetLockTargets( bool isEmptyReLock = true)
        {
            return m_pOwner.GetLockTargets(isEmptyReLock);
        }
        //------------------------------------------------------
        public virtual bool DoLockTargets()
        {
            Actor pOwner = GetActor();
            if (pOwner == null) return false;
            ActorAgentTree pAT = pOwner.GetAgent<ActorAgentTree>();
            if(pAT!=null)
            {
                VariableList argvs = VariableList.Malloc();
                argvs.AddUserData(this);
                pAT.ExecuteTask((int)EActorATType.onLockTarget, argvs);
            }
            return true;
        }
        //-----------------------------------------------------
        protected virtual void OnUpdate(FFloat fFrame) 
        {
        }
        //-----------------------------------------------------
        protected virtual bool CheckCanTrigger() 
        {
            return true;
        }
        //-----------------------------------------------------
        [ATMethod("获取CD时长(ms)")]
        public long GetConfigCD()
        {
            return m_lConfigCD;
        }
        //-----------------------------------------------------
        [ATMethod("设置CD时长(ms)")]
        public void SetConfigCD(long cd)
        {
            m_lConfigCD = cd;
        }
        //-----------------------------------------------------
        [ATMethod("获取动作类型")]
        public EActionStateType GetActionType()
        {
            return m_ActionType;
        }
        //-----------------------------------------------------
        [ATMethod("获取动作tag")]
        public uint GetActionTag()
        {
            return m_nActionTag;
        }
        //-----------------------------------------------------
        [ATMethod("设置绑定动作")]
        public void SetActionTypeAndTag(EActionStateType eType, uint nTag)
        {
            m_ActionType = eType;
            m_nActionTag = nTag;
        }
        //-----------------------------------------------------
        [ATMethod("获取属性计算公式")]
        public int GetAttrFormulaType()
        {
            return m_nAttrFormulaType;
        }
        //-----------------------------------------------------
        [ATMethod("设置属性计算公式")]
        public void SetAttrFormulaType(int type)
        {
            m_nAttrFormulaType = type;
        }
        //-----------------------------------------------------
        public override void Destroy()
        {
            m_pOwner = null;
            m_pConfigData = null;
            m_bActived = false;
            m_nSkillID = 0;
            m_LastTriggerTime = 0;
            m_nTriggerCount = 0;
            m_nLevel = 0;
            m_ActionType = EActionStateType.None;
            m_nActionTag = 0;
            
            m_lConfigCD = 0;
            m_nAttrFormulaType = 0;
        }
    }
}
#endif
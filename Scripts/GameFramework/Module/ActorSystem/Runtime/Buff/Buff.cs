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
        protected byte                  m_nBuffType = 0;//增益、减益、控制等类型

        private uint                    m_nLevel = 0;

        private byte[]                  m_arrAttrTypes = null;
        private FFloat[]                m_arrAttrValues = null;

        private float                   m_fBeginScale = 1;
        public string                   m_strBeginEffect = null;
        public string                   m_strBeginSound = null;

        private float                   m_fStepScale = 1;
        private string                  m_strStepEffect = null;
        private string                  m_strStepSound = null;

        private float                   m_fEndScale = 1;
        private string                  m_strEndEffect = null;
        private string                  m_strEndSound = null;

        private string                  m_strBindSlot = null;
        private Vector3                 m_BindOffset = Vector3.zero;
        private Vector3                 m_BindRotateOffset = Vector3.zero;
        private byte                    m_nBindBit = (byte)ESlotBindBit.All;

        protected IContextData          m_pConfigData = null;
        protected BuffSystem            m_pOwner = null;
        protected bool                  m_bActived = false;
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
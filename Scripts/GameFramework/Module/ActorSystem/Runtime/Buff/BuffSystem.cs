#if USE_ACTORSYSTEM
/********************************************************************
生成日期:	5:11:2020  20:36
类    名: 	BuffSystem
作    者:	HappLI
描    述:	Buff系统
*********************************************************************/
using Framework.AT.Runtime;
using Framework.DrawProps;
using System.Collections.Generic;

#if USE_FIXEDMATH
using ExternEngine;
#else
using FFloat = System.Single;
#endif

namespace Framework.ActorSystem.Runtime
{
    [ATInteralExport("Actor系统/Buff系统", -3, "ActorSystem/actor_buffsytem")]
    public class BuffSystem : TypeActor
    {
        private Actor m_pOwner = null;
        protected Dictionary<uint, Buff> m_vBuffs;
        //-----------------------------------------------------
        public BuffSystem()
        {
            m_pOwner = null;
        }
        //-----------------------------------------------------
        public ActorManager GetActorManager()
        {
            if (m_pOwner != null)
                return m_pOwner.GetActorManager();
            return null;
        }
        //-----------------------------------------------------
        public void SetActor(Actor pActor)
        {
            m_pOwner = pActor;
        }
        //-----------------------------------------------------
        [ATMethod("获或当前Actor")]
        public Actor GetActor()
        {
            return m_pOwner;
        }
        //-----------------------------------------------------
        public long GetCurrentTime()
        {
            return m_pOwner.GetActorManager().GetRunTime();
        }
        //-----------------------------------------------------
        public List<Actor> GetLockTargets(bool isEmptyReLock = true)
        {
            return null;
        }
        //-----------------------------------------------------
        public void AddLockTarget(Actor pNode, bool bClear = false)
        {
        }
        //-----------------------------------------------------
        [ATMethod("清除索敌单位")]
        public void ClearLockTargets()
        {
        }
        //-----------------------------------------------------
        [ATMethod("添加Buff")]
        public void AddBuff(Buff pBuff)
        {
            if (pBuff == null) return;
            pBuff.SetSystem(this);
        }
        //-----------------------------------------------------
        public void Update(FFloat fFrame)
        {
            if (m_vBuffs != null)
            {
                foreach(var buff in m_vBuffs)
                {
                    buff.Value.Update(fFrame);
                }
            }
        }
        //-----------------------------------------------------
        public override void Destroy()
        {
        }
    }
}
#endif
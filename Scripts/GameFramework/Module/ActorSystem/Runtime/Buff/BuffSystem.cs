#if USE_ACTORSYSTEM
/********************************************************************
生成日期:	5:11:2020  20:36
类    名: 	BuffSystem
作    者:	HappLI
描    述:	Buff系统
*********************************************************************/
using Framework.AT.Runtime;
using System.Collections.Generic;
#if USE_FIXEDMATH
using ExternEngine;
#else
using FFloat = System.Single;
#endif

namespace Framework.ActorSystem.Runtime
{
    //-----------------------------------------------------
    [ATInteralExport("Actor系统/Buff系统", -9, "BuffSystem/actor_buffsystem")]
    public class BuffSystem : TypeActor
    {
        private Actor m_pOwner = null;
        protected Dictionary<uint, Buff> m_vBuffs;

        private bool                        m_bDirty = false;
        private Dictionary<byte, BuffAttr>  m_vBuffAttrs = null;
        private Dictionary<byte, BuffAttr>  m_vLastBuffAttrs = null;
        private Dictionary<byte, BuffAttr> m_vTempBuffAttrs = null;
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
        [ATMethod("获取Buff属性值"),ATArgvDrawer("type", "DrawAttributePop")]
        public FFloat GetAttrValue(byte type)
        {
            if (m_vTempBuffAttrs == null) m_vTempBuffAttrs = m_vBuffAttrs;
            if (m_vTempBuffAttrs != null && m_vTempBuffAttrs.TryGetValue(type, out var attr))
            {
                return attr.GetValue();
            }
            return 0;
        }
        //-----------------------------------------------------
        [ATMethod("获取Buff属性率"), ATArgvDrawer("type", "DrawAttributePop")]
        public FFloat GetAttrRate(byte type)
        {
            if (m_vTempBuffAttrs == null) m_vTempBuffAttrs = m_vBuffAttrs;
            if (m_vTempBuffAttrs != null && m_vTempBuffAttrs.TryGetValue(type, out var attr))
            {
                return attr.GetRate();
            }
            return 0;
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
            if(m_bDirty)
            {
                m_bDirty = false;
                if (m_vBuffs != null)
                {
                    if (m_vBuffAttrs == null) m_vBuffAttrs = new Dictionary<byte, BuffAttr>(32);
                    else
                    {
                        if (m_vLastBuffAttrs == null) m_vLastBuffAttrs = new Dictionary<byte, BuffAttr>(m_vBuffAttrs.Count);
                        m_vLastBuffAttrs.Clear();
                        foreach (var db in m_vBuffAttrs)
                        {
                            m_vLastBuffAttrs.Add(db.Key, db.Value);
                        }
                    }
                    m_vBuffAttrs.Clear();
                    foreach (var buff in m_vBuffs)
                    {
                        buff.Value.CollectStats(m_vBuffAttrs);
                    }
                    if(m_vLastBuffAttrs!=null)
                    {
                        foreach (var db in m_vBuffAttrs)
                        {
                            if(m_vLastBuffAttrs.TryGetValue(db.Key, out var lastValue))
                            {
                                if(!lastValue.Equals(db.Value))
                                {
                                    m_vTempBuffAttrs = m_vLastBuffAttrs;
                                    FFloat oldValue = m_pOwner.GetAttr(db.Key);
                                    m_vTempBuffAttrs = m_vBuffAttrs;
                                    m_pOwner.GetActorParameter().DoAttrDirtyCall(db.Key, oldValue, m_pOwner.GetAttr(db.Key));
                                }
                            }
                        }
                        foreach(var db in m_vLastBuffAttrs)
                        {
                            if (!m_vBuffAttrs.TryGetValue(db.Key, out var lastValue))
                            {
                                m_vTempBuffAttrs = m_vLastBuffAttrs;
                                FFloat oldValue = m_pOwner.GetAttr(db.Key);
                                m_vTempBuffAttrs = m_vBuffAttrs;
                                m_pOwner.GetActorParameter().DoAttrDirtyCall(db.Key, oldValue, m_pOwner.GetAttr(db.Key));
                            }
                        }

                        m_vTempBuffAttrs = null;
                    }
                }
            }
        }
        //-----------------------------------------------------
        public override void Destroy()
        {
            if (m_vLastBuffAttrs != null) m_vLastBuffAttrs.Clear();
            if (m_vBuffAttrs != null) m_vBuffAttrs.Clear();
            m_vTempBuffAttrs = null;
        }
    }
}
#endif
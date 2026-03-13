#if USE_ACTORSYSTEM
/********************************************************************
生成日期:	5:11:2020  20:36
类    名: 	BuffSystem
作    者:	HappLI
描    述:	Buff系统
*********************************************************************/
using Framework.AT.Runtime;
using System.Collections.Generic;
using TagLib.Id3v2;

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
        private Actor                       m_pOwner = null;
        protected LinkedList<Buff>          m_vBuffs = null;

        private bool                        m_bDirty = false;
        private Dictionary<byte, BuffAttr>  m_vBuffAttrs = null;
        private Dictionary<byte, BuffAttr>  m_vLastBuffAttrs = null;
        private Dictionary<byte, BuffAttr>  m_vTempBuffAttrs = null;
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
        [ATMethod("添加Buff")]
        public void AddBuff(Buff pBuff)
        {
            if (pBuff == null) return;
            pBuff.SetSystem(this);
            if (m_vBuffs == null) m_vBuffs = new LinkedList<Buff>();
            else
            {
                if (m_vBuffs.Contains(pBuff))
                    return;
            }
            m_vBuffs.AddLast(pBuff);
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
        internal void OnAttack(Skill pSkill)
        {
            if (pSkill == null) return;
            if (m_vBuffs == null) return;
            for (var node = m_vBuffs.First; node != null;)
            {
                node.Value.OnAttack(pSkill);
            }
        }
        //-----------------------------------------------------
        internal void OnHit(HitFrameActor hifFrame)
        {
            if (m_vBuffs == null) return;
            for (var node = m_vBuffs.First; node != null;)
            {
                node.Value.OnHit(hifFrame);
            }
        }
        //-----------------------------------------------------
        internal void OnFlagDirty(EActorFlag flag, bool IsUsed)
        {
            if(IsUsed)
            {
                if(flag == EActorFlag.Killed)
                {
                    if (m_vBuffs != null)
                    {
                        for (var node = m_vBuffs.First; node != null;)
                        {
                            var next = node.Next;
                            Buff cur = node.Value;
                            node = next;
                            if (cur.IsDieKeep()) continue;
                            cur.Destroy();
                        }
                    }
                }
            }
        }
        //-----------------------------------------------------
        public void Update(FFloat fFrame)
        {
            if (m_vBuffs != null)
            {
                for (var node = m_vBuffs.First; node != null;)
                {
                    var next = node.Next;
                    bool bDirty = node.Value.Update(fFrame);
                    if (bDirty) m_bDirty = true;
                    if (node.Value.IsEnd())
                    {
                        m_bDirty = true;
                        node.Value.Free();
                        m_vBuffs.Remove(node);
                    }
                    node = next;
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
                    for (var node = m_vBuffs.First; node != null; node = node.Next)
                    {
                        node.Value.CollectStats(m_vBuffAttrs);
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
            m_bDirty = false;
            if(m_vBuffs!=null)
            {
                for (var node = m_vBuffs.First; node != null; node = node.Next)
                {
                    node.Value.Free();
                }
                m_vBuffs.Clear();
            }
            m_pOwner = null;
        }
    }
}
#endif
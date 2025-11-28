/********************************************************************
生成日期:	06:30:2025
类    名: 	ActorParameter
作    者:	HappLI
描    述:	Actor单位属性参数
*********************************************************************/
using System.Collections.Generic;
namespace Framework.ActorSystem.Runtime
{
    public interface IActorAttrDirtyCallback
    {
        void OnActorAttrDirty(Actor pActor, byte type, float oldValue, float newValue);
    }
    public class ActorParameter : TypeObject
    {
        IContextData                            m_pConfigData = null;
        Dictionary<byte, int>                   m_vAttributes = null;
        Actor m_pActor;
        byte                                    m_HpAttrType = 1;
        private List<IActorAttrDirtyCallback>   m_vCallbacks = null;

        protected byte                          m_nAttackGroup = 0;
        protected int                           m_nGUID = 0;
        public ActorParameter()
        {
            m_pActor = null;
        }
        //--------------------------------------------------------
        public void SetGUID(int guid)
        {
            m_nGUID = guid;
        }
        //--------------------------------------------------------
        public int GetGUID()
        {
            return m_nGUID;
        }
        //--------------------------------------------------------
        public void SetActor(Actor pActor)
        {
            m_pActor = pActor;
        }
        //--------------------------------------------------------
        public void SetCfgData(IContextData cfgData)
        {
            m_pConfigData = cfgData;
        }
        //--------------------------------------------------------
        public IContextData GetCfgData()
        {
            return m_pConfigData;
        }
        //--------------------------------------------------------
        public byte GetAttackGroup()
        {
            return m_nAttackGroup;
        }
        //------------------------------------------------------
        public void SetAttackGroup(byte attackGroup)
        {
            m_nAttackGroup = attackGroup;
        }
        //------------------------------------------------------
        public virtual bool CanAttackGroup(byte attackGroup)
        {
            return m_nAttackGroup != attackGroup;
        }
        //--------------------------------------------------------
        public ActorManager GetActorManager()
        {
            return m_pActor?.GetActorManager();
        }
        //--------------------------------------------------------
        public float GetModelHeight()
        {
            return 0;
        }
        //--------------------------------------------------------
        public void SetHpAttrType(byte type)
        {
            m_HpAttrType = type;
        }
        //--------------------------------------------------------
        internal void SetAttrs(byte[] attiTypes, int[] values)
        {
            if (attiTypes == null || values == null)
                return;
            if (attiTypes.Length != values.Length)
                return;

            for (int i = 0; i < attiTypes.Length; ++i)
            {
                SetAttr(attiTypes[i], values[i]);
            }
        }
        //--------------------------------------------------------
        internal void SetAttr(byte type, int value)
        {
            int oldValue = 0;
            if (!m_vAttributes.TryGetValue(type, out oldValue))
                oldValue = -1;
            m_vAttributes[type] = value;
            DoAttrDirtyCall(type, oldValue, m_vAttributes[type]);
        }
        //--------------------------------------------------------
        internal int GetAttr(byte type, int defVal = 0)
        {
            if (m_vAttributes.TryGetValue(type, out var val))
                return val;
            return defVal;
        }
        //--------------------------------------------------------
        internal void RemoveAttr(byte type)
        {
            m_vAttributes.Remove(type);
        }
        //--------------------------------------------------------
        internal void AppendAttrs(byte[] attiTypes, int[] values)
        {
            if (attiTypes == null || values == null)
                return;
            if (attiTypes.Length != values.Length)
                return;
            for (int i = 0; i < attiTypes.Length; ++i)
            {
                AppendAttr(attiTypes[i], values[i]);
            }
        }
        //--------------------------------------------------------
        internal void AppendAttr(byte type, int value)
        {
            int oldValue = 0;
            if (m_vAttributes.TryGetValue(type, out oldValue))
            {
                m_vAttributes[type] = oldValue + value;
            }
            else
            {
                oldValue = -1;
                m_vAttributes[type] = value;
            }
            DoAttrDirtyCall(type, oldValue, m_vAttributes[type]);
            m_pActor.GetActorManager().OnActorAttriDirtyCallback(m_pActor, type, value, oldValue);
        }
        //--------------------------------------------------------
        internal void SubAttrs(byte[] attiTypes, int[] values)
        {
            if (attiTypes == null || values == null)
                return;
            if (attiTypes.Length != values.Length)
                return;
            for (int i = 0; i < attiTypes.Length; ++i)
            {
                SubAttr(attiTypes[i], values[i]);
            }
        }
        //--------------------------------------------------------
        internal void SubAttr(byte type, int value, bool bLowerZero = false)
        {
            if (m_vAttributes.TryGetValue(type, out var val))
            {
                int oldValue = val;
                if (bLowerZero)
                {
                    if (val < value)
                        m_vAttributes[type] = 0;
                    else
                        m_vAttributes[type] = val - value;
                }
                else
                    m_vAttributes[type] = val - value;
                DoAttrDirtyCall(type, oldValue, m_vAttributes[type]);
            }
        }
        //--------------------------------------------------------
        internal void ClearAttrs()
        {
            if (m_vAttributes != null)
                m_vAttributes.Clear();
        }
        //--------------------------------------------------------
        void DoAttrDirtyCall(byte type, int oldValue, int newValue)
        {
            if (oldValue == newValue)
                return;

            m_pActor.GetActorManager().OnActorAttriDirtyCallback(m_pActor, type, oldValue, newValue);

            if (m_vCallbacks == null)
                return;
            foreach (var db in m_vCallbacks)
            {
                db.OnActorAttrDirty(m_pActor, type, oldValue, newValue);
            }
        }
        //--------------------------------------------------------
        internal void Update(float fDeltaTime)
        {
            if (m_HpAttrType > 0)
            {
                if (!m_pActor.IsKilled() && m_vAttributes.TryGetValue(m_HpAttrType, out var attrValue) && attrValue <= 0)
                {
                    m_pActor.SetKilled(true);
                }
            }
        }
        //--------------------------------------------------------
        internal void Clear()
        {
            ClearAttrs();
            m_HpAttrType = 1;
            m_pConfigData = null;
            m_nGUID = 0;
            m_nAttackGroup = 0;
            if (m_vCallbacks != null) m_vCallbacks.Clear();
        }
        //--------------------------------------------------------
        public override void Destroy()
        {
            Clear();
        }
    }
}
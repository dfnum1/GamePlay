#if USE_ACTORSYSTEM
/********************************************************************
生成日期:	5:11:2020  20:36
类    名: 	Buff属性及状态定义
作    者:	HappLI
描    述:	Buff系统
*********************************************************************/
using Framework.Base;


#if USE_FIXEDMATH
using ExternEngine;
#else
using FFloat = System.Single;
#endif

namespace Framework.ActorSystem.Runtime
{
    public enum EBuffTargetType
    {
        eAttacker = 0,//施法者
        eTargeter = 1,//受击者
        eFriender = 2,//友方单位
    }
    //-----------------------------------------------------
    [System.Serializable]
    public struct BuffStateData
    {
        public byte state;
        public string name;
        public string desc;
    }
    //-----------------------------------------------------
    struct BuffAttr : IUserData
    {
        FFloat m_value;
        FFloat m_rate;
        //-----------------------------------------------------
        public bool Equal(BuffAttr attr)
        {
            return this.m_value == attr.m_value && this.m_rate == attr.m_rate;
        }
        //-----------------------------------------------------
        public FFloat GetValue()
        {
            return m_value;
        }
        //-----------------------------------------------------
        public FFloat GetRate()
        {
            return m_rate;
        }
        //-----------------------------------------------------
        public void SetValue(FFloat value)
        {
            this.m_value = value;
        }
        //-----------------------------------------------------
        public void AddValue(FFloat value)
        {
            this.m_value += value;
        }
        //-----------------------------------------------------
        public void SubValue(FFloat value)
        {
            this.m_value -= value;
        }
        //-----------------------------------------------------
        public void SetRate(FFloat rate)
        {
            this.m_rate = rate;
        }
        //-----------------------------------------------------
        public void AddRate(FFloat value)
        {
            this.m_rate += value;
        }
        //-----------------------------------------------------
        public void SubRate(FFloat value)
        {
            this.m_rate -= value;
        }
        //-----------------------------------------------------
        public void Clear()
        {
            this.m_value = 0;
            this.m_rate = 0;
        }
    }
}
#endif
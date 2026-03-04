/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	AProxyDB
作    者:	HappLI
描    述:	服务器数据
*********************************************************************/
using Framework.Base;
using Framework.Core;

namespace Framework.Db
{
    public abstract class AProxyDB : TypeObject
    {
        protected User m_pUser;
        //------------------------------------------------------
        public AProxyDB()
        {
#if UNITY_EDITOR
            DBRtti.CheckLegal(this.GetType());
#endif
        }
        //------------------------------------------------------
        public void Init(User user)
        {
            m_pUser = user;
            OnInit();
        }
        //------------------------------------------------------
        public string SerializeDB()
        {
            return OnSerializeDB();
        }
        //------------------------------------------------------
        public bool UnSerializeDB(string jsonContent)
        {
            return OnUnSerializeDB(jsonContent);
        }
        protected virtual void OnInit() { }
        protected virtual bool OnUnSerializeDB(string jsonContent) { return false; }
        protected virtual string OnSerializeDB() { return null; }
        //------------------------------------------------------
        public virtual void Clear() { }
        //------------------------------------------------------
        public void SetDirty()
        {
            if (m_pUser == null) return;
            m_pUser.OnDirtyDBEvent(this);
        }
        //------------------------------------------------------
        public User GetUser()
        {
            return m_pUser;
        }
        //------------------------------------------------------
        public AFramework GetFramework()
        {
            if (m_pUser == null) return null;
            return m_pUser.GetFramework();
        }
        //------------------------------------------------------
        public virtual void ApplayData(IUserData userData)
        {

        }
        //------------------------------------------------------
        public override void Destroy()
        {
            m_pUser = null;
            Clear();
        }
    }
}

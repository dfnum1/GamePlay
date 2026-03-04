/********************************************************************
生成日期:	25:7:2019   14:35
类    名: 	UserManager
作    者:	HappLI
描    述:   用户管理
*********************************************************************/

using Framework.AT.Runtime;
using Framework.Base;
using Framework.Core;
using System.Collections.Generic;

namespace Framework.Db
{
    [ATInteralExport("用户系统", -7, icon: "GameDb/db_system")]
    public class UserManager : AModule
    {
        List<User> m_vOthers = null;

#if !USE_SERVER
        User m_pMySelf = null;
        [ATField("我自己")]
        public User mySelf
        {
            get { return m_pMySelf; }
            set { m_pMySelf = value; }
        }
        private static UserManager ms_pInstance = null;
        //------------------------------------------------------
        public static UserManager getInstance()
        {
            return ms_pInstance;
        }
        //------------------------------------------------------
        public static User Current
        {
            get
            {
                if (ms_pInstance == null) return null;
                return ms_pInstance.mySelf;
            }
        }
        //------------------------------------------------------
        public static User MySelf
        {
            get
            {
                if (ms_pInstance == null) return null;
                return ms_pInstance.mySelf;

            }
        }   
#endif
        //------------------------------------------------------
        public UserManager()
        {
        }
        //------------------------------------------------------
        protected override void OnAwake()
        {
#if !USE_SERVER
            ms_pInstance = this;
            m_pMySelf = TypeInstancePool.Malloc<User>(GetFramework());
#endif
        }
        //------------------------------------------------------
        [ATMethod("获取用户")]
        public User GetUser(long userID)
        {
#if !USE_SERVER
            if (m_pMySelf.userID == userID) return m_pMySelf;
#endif
            for (int i = 0; i < m_vOthers.Count; ++i)
            {
                if (m_vOthers[i].userID == userID)
                    return m_vOthers[i];
            }
            return null;
        }
#if !USE_SERVER
        //------------------------------------------------------
        [ATMethod("获取当前用户")]
        public User GetCurUser()
        {
            return mySelf;
        }
#endif
        //------------------------------------------------------
        [ATMethod("添加用户")]
        public User AddUser(long userID)
        {
            if(m_vOthers!=null)
            {
                for (int i = 0; i < m_vOthers.Count; ++i)
                {
                    if (m_vOthers[i].userID == userID)
                        return m_vOthers[i];
                }
            }

            User newUser = TypeInstancePool.Malloc<User>(GetFramework());
            newUser.userID = userID;
            if (m_vOthers == null) m_vOthers = new List<User>(2);
            m_vOthers.Add(newUser);
            return newUser;
        }
        //------------------------------------------------------
        [ATMethod("清理用户列表")]
        public void ClearUser(bool bIncludeMyself = true)
        {
#if !USE_SERVER
            if (bIncludeMyself) m_pMySelf.Clear();
#endif
            if(m_vOthers!=null)
            {
                for (int i = 0; i < m_vOthers.Count; ++i)
                {
                    m_vOthers[i].Free();
                }
                m_vOthers.Clear();
            }
        }
        //------------------------------------------------------
        protected override void OnDestroy()
        {
            ClearUser();
#if !USE_SERVER
            ms_pInstance = null;
#endif
        }
    }
}


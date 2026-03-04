/********************************************************************
生成日期:	25:7:2019   14:35
类    名: 	User
作    者:	HappLI
描    述:   玩家数据
*********************************************************************/
using System;
using UnityEngine;
using Framework.Core;
using Framework.AT.Runtime;
using Framework.Base;
using System.Collections.Generic;

namespace Framework.Db
{
    [System.Serializable]
    [ATInteralExport("用户系统/用户", -8)]
    public class User : TypeObject
    {
        public Action<AProxyDB>             OnDirtyDB = null;

        public                              long userID = 0;
        string                              m_strSDKUid = null;

        private Dictionary<int, AProxyDB>   m_vProxyDBs = null;
        private AFramework                  m_pFramework = null;
        private long                        m_lLastLoginTime;
        //------------------------------------------------------
        public User():base()
        {
            OnDirtyDB = null;
            userID = 0;
            m_strSDKUid = null;
            m_lLastLoginTime = 0;
        }
        //------------------------------------------------------
        [ATMethod("获取SdkUid")]
        public string GetSdkUid()
        {
            if (!string.IsNullOrEmpty(m_strSDKUid)) return m_strSDKUid;
            return userID.ToString();
        }
        //------------------------------------------------------
        [ATMethod("设置SdkUid")]
        public void SetSDKUid(string uid)
        {
            m_strSDKUid = uid;
            if (userID == 0) userID = BaseUtil.StringToHashID64(uid);
        }
        //------------------------------------------------------
        public void ApplayData(IUserData userData)
        {
        }
        //------------------------------------------------------
        public User(AFramework pFramework)
        {
            m_pFramework = pFramework;
        }
        //------------------------------------------------------
        public AFramework GetFramework()
        {
            return m_pFramework;
        }
        //------------------------------------------------------
        public void OnDirtyDBEvent(AProxyDB db)
        {
            OnDirtyDB?.Invoke(db);
        }
        //------------------------------------------------------
        [ATMethod("获取Db数据"), ATArgvDrawer("type", "DrawProxyDbTypePop")]
        public AProxyDB GetProxyDB(int type)
        {
            if (m_vProxyDBs == null) return null;
            if (m_vProxyDBs.TryGetValue(type, out var proxy))
                return proxy;
            return null;
        }
        //------------------------------------------------------
        public T ProxyDB<T>(int type = -1) where T : AProxyDB, new()
        {
            int typeIndex = (int)type;
            if (type == -1) typeIndex = DBRtti.GetTypeId(typeof(T));
            if (m_vProxyDBs == null) m_vProxyDBs = new Dictionary<int, AProxyDB>(8);
            else
            {
                if (m_vProxyDBs.TryGetValue(type, out var proxy))
                    return proxy as T;
            }
            T newDb = new T();
            newDb.Init(this);
            m_vProxyDBs[type] = newDb;
            return newDb;
        }
        //------------------------------------------------------
        public void UnSerializeProxyDB(int type, string jsonData)
        {
            System.Type dbType = DBRtti.GetType(type);
            if (dbType == null) return;
            int typeIndex = (int)type;
            if (m_vProxyDBs == null) m_vProxyDBs = new Dictionary<int, AProxyDB>(8);
            if (!m_vProxyDBs.TryGetValue(typeIndex, out var proxyDB))
            {
                proxyDB = DBRtti.Malloc<AProxyDB>(this, type);
                m_vProxyDBs[typeIndex] = proxyDB;
            }
            if (proxyDB == null)
                return;

            if(!proxyDB.UnSerializeDB(jsonData))
                JsonUtility.FromJsonOverwrite(jsonData, proxyDB);
        }
        //------------------------------------------------------
        public string SerializeProxyDB(int type)
        {
            int typeIndex = (int)type;
            if (!m_vProxyDBs.TryGetValue(typeIndex, out var proxyDB) || proxyDB == null)
            {
                return null;
            }
            string json = proxyDB.SerializeDB();
            if (!string.IsNullOrEmpty(json)) return json;
            return JsonUtility.ToJson(proxyDB, true);
        }
        //------------------------------------------------------
        public Dictionary<int, AProxyDB> GetProxyDBs()
        {
            return m_vProxyDBs;
        }
        //------------------------------------------------------
        [ATMethod("清理所有Db数据")]
        public void Clear()
        {
            userID = 0;
            m_strSDKUid = null;
            if(m_vProxyDBs!=null)
            {
                foreach(var db in m_vProxyDBs)
                {
                    if (db.Value != null)
                    {
                        db.Value.Free();
                    }
                }
                m_vProxyDBs.Clear();
            }
            m_lLastLoginTime = 0;
        }
        //------------------------------------------------------
        public override void Destroy()
        {
            Clear();
        }
        //------------------------------------------------------
        [ATMethod("设置登录时间")]
        public void SetLastLoginTime(long time)
        {
            m_lLastLoginTime= time;
        }
        //------------------------------------------------------
        [ATMethod("获取登录时间")]
        public long GetLastLoginTime()
        {
            return m_lLastLoginTime;
        }
    }
}


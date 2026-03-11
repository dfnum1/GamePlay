/********************************************************************
生成日期:	1:11:2020 10:05
类    名: 	AOperatorHandle
作    者:	HappLI
描    述:	操作句柄
*********************************************************************/
using Framework.Base;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.Core
{
    public abstract class AOperatorHandle : TypeObject
    {
        private string m_strAssetPath;
#if UNITY_EDITOR
        private string m_strRawAssetPath;
#endif
        private bool m_bUsed = true;
        private bool m_bPermanent = false;
        private bool m_bAsync = false;
        private int m_nPriority = 0;

        private IUserData m_pUserData1 = null;
        private IUserData m_pUserData2 = null;
        private IUserData m_pUserData3 = null;
        private IUserData m_pUserData4 = null;

        private UnityEngine.Object m_pObject = null;
        //------------------------------------------------------
        public bool IsPermanent()
        {
            return m_bPermanent;
        }
        //------------------------------------------------------
        public void SetPermanent(bool permanent)
        {
            m_bPermanent = permanent;
        }
        //------------------------------------------------------
        public bool IsAsync()
        {
            return m_bAsync;
        }
        //------------------------------------------------------
        public void SetAsync(bool async)
        {
            m_bAsync = async;
        }
        //------------------------------------------------------
        public string GetAssetPath()
        {
            return m_strAssetPath;
        }
        //------------------------------------------------------
        public void SetAssetPath(string path)
        {
            m_strAssetPath = path;
        }
#if UNITY_EDITOR
        //------------------------------------------------------
        public string GetRawAssetPath()
        {
            return m_strRawAssetPath;
        }
        //------------------------------------------------------
        public void SetRawAssetPath(string path)
        {
            m_strRawAssetPath = path;
        }
#endif
        //------------------------------------------------------
        public int GetPriority()
        {
            return m_nPriority;
        }
        //------------------------------------------------------
        public void SetPriority(int priority)
        {
            m_nPriority = priority;
        }
        //------------------------------------------------------
        public bool IsValid()
        {
            return !string.IsNullOrEmpty(m_strAssetPath);
        }
        //------------------------------------------------------
        public bool IsUsed()
        {
            return m_bUsed;
        }
        //------------------------------------------------------
        public void SetUsed(bool used)
        {
            m_bUsed = used;
        }
        //------------------------------------------------------
        internal void SetObject(UnityEngine.Object obj)
        {
            m_pObject = obj;
        }
        //------------------------------------------------------
        public UnityEngine.Object GetObject()
        {
            return m_pObject;
        }
        //------------------------------------------------------
        public T GetObject<T>() where T : UnityEngine.Object
        {
            if (m_pObject == null) return null;
            return m_pObject as T;
        }
        //------------------------------------------------------
        public void SetUserData(int index, IUserData userData)
        {
            switch (index)
            {
                case 0:
                    m_pUserData1 = userData;
                    break;
                case 1:
                    m_pUserData2 = userData;
                    break;
                case 2:
                    m_pUserData3 = userData;
                    break;
                default:
                    m_pUserData4 = userData;
                    break;
            }
        }
        //------------------------------------------------------
        public IUserData GetUserData(int index)
        {
            switch (index)
            {
                case 0:
                    return m_pUserData1;
                case 1:
                    return m_pUserData2;
                case 2:
                    return m_pUserData3;
                default:
                    return m_pUserData4;
            }
            return null;
        }
        //------------------------------------------------------
        public bool HasUserData(int index)
        {
            switch (index)
            {
                case 0:
                    return m_pUserData1 != null;
                case 1:
                    return m_pUserData2 != null;
                case 2:
                    return m_pUserData3 != null;
                case 3:
                    return m_pUserData4 != null;
            }
            return false;
        }
        //------------------------------------------------------
        public bool HasUserData<T>(int index) where T : IUserData
        {
            switch (index)
            {
                case 0:
                    return m_pUserData1 != null && m_pUserData1 is T;
                case 1:
                    return m_pUserData2 != null && m_pUserData2 is T;
                case 2:
                    return m_pUserData3 != null && m_pUserData3 is T;
                case 3:
                    return m_pUserData4 != null && m_pUserData4 is T;
            }
            return false;
        }
        //------------------------------------------------------
        public T GetUserData<T>(int index) where T : IUserData
        {
            switch (index)
            {
                case 0:
                    return (T)m_pUserData1;
                case 1:
                    return (T)m_pUserData2;
                case 2:
                    return (T)m_pUserData3;
                default:
                    return (T)m_pUserData4;
            }
            return default(T);
        }
        //------------------------------------------------------
        internal void DoCallback(bool doSignCheck)
        {
            if (doSignCheck)
                DoSignCheckCallback();
            else
                DoCallback();
        }
        //------------------------------------------------------
        protected abstract void DoCallback();
        //------------------------------------------------------
        protected abstract void DoSignCheckCallback();
        //------------------------------------------------------
        public T Cast<T>()where T : AOperatorHandle
        {
            return this as T;
        }
        //------------------------------------------------------
        public override void Destroy()
        {
            base.Destroy();
            m_strAssetPath = null;
#if UNITY_EDITOR
            m_strRawAssetPath = null;
#endif
            m_pUserData1 = null;
            m_pUserData2 = null;
            m_pUserData3 = null;
            m_pUserData4 = null;
            m_bUsed = true;
            m_bPermanent = false;
            m_bAsync = false;
            m_nPriority = 0;
            m_pObject = null;
        }
    }
    //------------------------------------------------------
    public interface IOperatorCallback
    {
        void OnOperatorCallback(AOperatorHandle pCallback, bool doSignCheck);
    }
    //------------------------------------------------------
    public class AssetOperator : AOperatorHandle
    {
        private List<System.Action<AssetOperator, bool>> m_OnCallback = null;
        //------------------------------------------------------
        public void AddCallback(System.Action<AssetOperator, bool> callback)
        {
            if (callback == null) return;
            if (m_OnCallback == null)
                m_OnCallback = new List<System.Action<AssetOperator, bool>>(2);
            else
            {
                if (m_OnCallback.Contains(callback))
                    return;
            }
            m_OnCallback.Add(callback);
        }
        //------------------------------------------------------
        protected override void DoCallback()
        {
            if (m_OnCallback != null)
            {
                for (int i = 0; i < m_OnCallback.Count; i++)
                {
                    if (m_OnCallback[i] != null)
                        m_OnCallback[i](this, false);
                }
            }
        }
        //------------------------------------------------------
        protected override void DoSignCheckCallback()
        {
            if (m_OnCallback != null)
            {
                for (int i = 0; i < m_OnCallback.Count; i++)
                {
                    if (m_OnCallback[i] != null)
                        m_OnCallback[i](this, true);
                }
            }
        }
        //------------------------------------------------------
        public override void Destroy()
        {
            base.Destroy();
            if (m_OnCallback != null)
                m_OnCallback.Clear();
        }
    }
    //------------------------------------------------------
    public enum ELoadSceneMode
    {
        eSingle = 0,
        eAdditive = 1
    }
    public class SceneOperator : AOperatorHandle
    {
        private ELoadSceneMode m_eLoadSceneMode = ELoadSceneMode.eSingle;
        private List<System.Action<SceneOperator, bool>> m_OnCallback = null;
        //------------------------------------------------------
        public void AddCallback(System.Action<SceneOperator, bool> callback)
        {
            if (callback == null) return;
            if (m_OnCallback == null)
                m_OnCallback = new List<System.Action<SceneOperator, bool>>(2);
            else
            {
                if (m_OnCallback.Contains(callback))
                    return;
            }
            m_OnCallback.Add(callback);
        }
        //------------------------------------------------------
        protected override void DoCallback()
        {
            if (m_OnCallback != null)
            {
                for (int i = 0; i < m_OnCallback.Count; i++)
                {
                    if (m_OnCallback[i] != null)
                        m_OnCallback[i](this, false);
                }
            }
        }
        //------------------------------------------------------
        protected override void DoSignCheckCallback()
        {
            if (m_OnCallback != null)
            {
                for (int i = 0; i < m_OnCallback.Count; i++)
                {
                    if (m_OnCallback[i] != null)
                        m_OnCallback[i](this, true);
                }
            }
        }
        //------------------------------------------------------
        public void SetLoadSceneMode(ELoadSceneMode mode)
        {
            m_eLoadSceneMode = mode;
        }
        //------------------------------------------------------
        public ELoadSceneMode GetLoadSceneMode()
        {
            return m_eLoadSceneMode;
        }
        //------------------------------------------------------
        public override void Destroy()
        {
            base.Destroy();
            if (m_OnCallback != null) m_OnCallback.Clear();
            m_eLoadSceneMode = ELoadSceneMode.eSingle;
        }
    }
    //------------------------------------------------------
    public class InstanceOperator : AOperatorHandle
    {
        Transform m_pByParent = null;
        int m_nLimitCount = -1;
        bool m_bPreload = false;
        InstanceAble m_pAble = null;
        private List<System.Action<InstanceOperator, bool>> m_OnCallback = null;
        //------------------------------------------------------
        public void AddCallback(System.Action<InstanceOperator, bool> callback)
        {
            if (callback == null) return;
            if (m_OnCallback == null)
                m_OnCallback = new List<System.Action<InstanceOperator, bool>>(2);
            else
            {
                if (m_OnCallback.Contains(callback))
                    return;
            }
            m_OnCallback.Add(callback);
        }
        //------------------------------------------------------
        protected override void DoCallback()
        {
            if (m_OnCallback != null)
            {
                for (int i = 0; i < m_OnCallback.Count; i++)
                {
                    if (m_OnCallback[i] != null)
                        m_OnCallback[i](this, false);
                }
            }
        }
        //------------------------------------------------------
        protected override void DoSignCheckCallback()
        {
            if (m_OnCallback != null)
            {
                for (int i = 0; i < m_OnCallback.Count; i++)
                {
                    if (m_OnCallback[i] != null)
                        m_OnCallback[i](this, true);
                }
            }
        }
        //------------------------------------------------------
        internal void SetInstanceAble(InstanceAble pAble)
        {
            m_pAble = pAble;
        }
        //------------------------------------------------------
        public InstanceAble GetInstanceAble()
        {
            if(m_pAble == null)
            {
                var pObj = GetObject<GameObject>();
                if (pObj == null) return m_pAble;
                m_pAble = pObj.GetComponent<InstanceAble>();
                if (m_pAble == null) m_pAble = pObj.AddComponent<InstanceAble>();
            }
            return m_pAble;
        }
        //------------------------------------------------------
        public void SetPreload(bool preload)
        {
            m_bPreload = preload;
        }
        //------------------------------------------------------
        public bool IsPreload()
        {
            return m_bPreload;
        }
        //------------------------------------------------------
        public void SetByParent(Transform byParent)
        {
            m_pByParent = byParent;
        }
        //------------------------------------------------------
        public Transform GetByParent()
        {
            return m_pByParent;
        }
        //------------------------------------------------------
        public void SetLimitCount(int limitCount)
        {
            m_nLimitCount = limitCount;
        }
        //------------------------------------------------------
        public int GetLimitCount()
        {
            return m_nLimitCount;
        }
        //------------------------------------------------------
        public override void Destroy()
        {
            base.Destroy();
            if (m_OnCallback != null) m_OnCallback.Clear();
            SetUsed(false);
            m_pAble = null;
            m_pByParent = null;
            m_bPreload = false;
            m_nLimitCount = -1;
        }
    }
    //------------------------------------------------------
    public static class OperatorHandlerUtil
    {
        //------------------------------------------------------
        public static T Malloc<T>(AFramework pFramework) where T : AOperatorHandle, new()
        {
            if (pFramework == null) return new T();
            return pFramework.ShareCache.Malloc<T>();
        }
        //------------------------------------------------------
        public static void Free(this AOperatorHandle handle)
        {
            if (handle == null) return;
            handle.Free();
        }
    }
}
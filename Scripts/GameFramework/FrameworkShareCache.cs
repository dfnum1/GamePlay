/********************************************************************
生成日期:	3:10:2019  15:03
类    名: 	ShareCache
作    者:	HappLI
描    述:	共享参数
*********************************************************************/
using Framework.ActorSystem.Runtime;
using Framework.AT.Runtime;
using Framework.Cutscene.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework.Base;

#if USE_FIXEDMATH
using ExternEngine;
#else
using FFloat = System.Single;
using FMatrix4x4 = UnityEngine.Matrix4x4;
using FQuaternion = UnityEngine.Quaternion;
using FVector2 = UnityEngine.Vector2;
using FVector3 = UnityEngine.Vector3;
using FBounds = UnityEngine.Bounds;
using FRay = UnityEngine.Ray;
#endif

namespace Framework.Core
{
    public class FrameworkShareCache
    {
        AFramework m_pFramework;
        private int m_nAgentPoolCapacity = 32;
        private Stack<AgentTree> m_vAgentTreePool = null;
        private int m_nTypePoolCapacity = 128;
        Dictionary<System.IntPtr, Stack<TypeObject>> m_vPools = new Dictionary<System.IntPtr, Stack<TypeObject>>(16);

        private Dictionary<string, int> m_stringHash = null;
        private int                 m_nStringHashId = 0;

        ListCache<FVector3>         m_pFVec3ListCache = null;
        ListCache<Vector3>          m_pVec3ListCache = null;
        ListCache<IUserData>        m_pUserDataListCache = null;
        ListCache<int>              m_pIntListCache = null;
        ListCache<float>            m_pFloatListCache = null;
        ListCache<long>             m_pLongListCache = null;
        ListCache<double>           m_pDoubleListCache = null;
        HashSetCache<IUserData>     m_pUserDataHashSetCache = null;
        HashSetCache<int>           m_pIntHashSetCache = null;
        HashSetCache<float>         m_pFloatHashSetCache = null;
        HashSetCache<long>          m_pLongHashSetCache = null;
        HashSetCache<double>        m_pDoubleHashSetCache = null;

        internal FrameworkShareCache(AFramework pFramework)
        {
            m_pFramework = pFramework;
        }
        //--------------------------------------------------------
        public int StringID(string name)
        {
            if (string.IsNullOrEmpty(name)) return 0;
            if (m_stringHash == null)
                m_stringHash = new Dictionary<string, int>(32);
            m_nStringHashId++;
            m_stringHash[name] = m_nStringHashId;
            return m_nStringHashId;
        }
        //--------------------------------------------------------
        internal T Malloc<T>() where T : TypeObject, new()
        {
            System.IntPtr handle = typeof(T).TypeHandle.Value;
            if (m_vPools.TryGetValue(handle, out var pool) && pool.Count > 0)
            {
                var user = pool.Pop();
                user.SetFramework(m_pFramework);
                return user as T;
            }
#if UNITY_EDITOR
            TypeInstancePool.SetMallockInner(typeof(T));
#endif
            T newT = new T();
#if UNITY_EDITOR
            TypeInstancePool.SetMallockInner(null);
#endif
            newT.SetFramework(m_pFramework);
            m_pFramework.UnRegisterFunction(newT);
            return newT;
        }
        //--------------------------------------------------------
        internal void Free(TypeObject pObj)
        {
            System.IntPtr handle = pObj.GetType().TypeHandle.Value;
            if (!m_vPools.TryGetValue(handle, out var pool))
            {
                pool = new Stack<TypeObject>(m_nTypePoolCapacity);
                m_vPools[handle] = pool;
            }
            m_pFramework.UnRegisterFunction(pObj);
            pObj.Destroy();
            if (pool.Count < m_nTypePoolCapacity) pool.Push(pObj);
        }
        //-----------------------------------------------------
        public AgentTree MallocAgentTree()
        {
            if (m_vAgentTreePool == null || m_vAgentTreePool.Count<=0)
            {
#if UNITY_EDITOR
                TypeInstancePool.SetMallockInner(typeof(AgentTree));
#endif
                var pAT = new AgentTree();
#if UNITY_EDITOR
                TypeInstancePool.SetMallockInner(null);
#endif
                return pAT;
            }
            return m_vAgentTreePool.Pop();
        }
        //-----------------------------------------------------
        public AgentTree MallocAgentTree(AgentTreeData pATData)
        {
            if (pATData == null || !pATData.IsValid()) return null;
            AgentTree pAT = MallocAgentTree();
            if(pAT.Create(pATData))
            {
                return pAT;
            }
            FreeAgentTree(pAT);
            return null;
        }
        //-----------------------------------------------------
        public void FreeAgentTree(AgentTree agentTree)
        {
            if (agentTree == null) return;
            agentTree.Destroy();
            if (m_vAgentTreePool != null && m_vAgentTreePool.Count >= m_nAgentPoolCapacity)
                return;
            if (m_vAgentTreePool == null) m_vAgentTreePool = new Stack<AgentTree>(m_nAgentPoolCapacity);
            m_vAgentTreePool.Push(agentTree);
        }
        //-----------------------------------------------------
        public List<int> CacheIntList(int capcity = 4)
        {
            if (m_pIntListCache == null)
                m_pIntListCache = new ListCache<int>(4);
            return m_pIntListCache.Get(capcity);
        }
        //-----------------------------------------------------
        public List<float> CacheFloatList(int capcity = 4)
        {
            if (m_pFloatListCache == null)
                m_pFloatListCache = new ListCache<float>(4);
            return m_pFloatListCache.Get(capcity);
        }
        //-----------------------------------------------------
        public List<long> CacheLongList(int capcity = 4)
        {
            if (m_pLongListCache == null)
                m_pLongListCache = new ListCache<long>(4);
            return m_pLongListCache.Get(capcity);
        }
        //-----------------------------------------------------
        public List<double> CacheDoubleList(int capcity = 4)
        {
            if (m_pDoubleListCache == null)
                m_pDoubleListCache = new ListCache<double>(4);
            return m_pDoubleListCache.Get(capcity);
        }
        //-----------------------------------------------------
        public HashSet<int> CacheIntSet(int capcity = 4)
        {
            if (m_pIntHashSetCache == null)
                m_pIntHashSetCache = new HashSetCache<int>(4);
            return m_pIntHashSetCache.Get(capcity);
        }
        //-----------------------------------------------------
        public HashSet<float> CacheFloatSet(int capcity = 4)
        {
            if (m_pFloatHashSetCache == null)
                m_pFloatHashSetCache = new HashSetCache<float>(4);
            return m_pFloatHashSetCache.Get(capcity);
        }
        //-----------------------------------------------------
        public HashSet<long> CacheLongSet(int capcity = 4)
        {
            if (m_pLongHashSetCache == null)
                m_pLongHashSetCache = new HashSetCache<long>(4);
            return m_pLongHashSetCache.Get(capcity);
        }
        //-----------------------------------------------------
        public HashSet<double> CacheDoubleSet(int capcity = 4)
        {
            if (m_pDoubleHashSetCache == null)
                m_pDoubleHashSetCache = new HashSetCache<double>(4);
            return m_pDoubleHashSetCache.Get(capcity);
        }
        //-----------------------------------------------------
        public List<FVector3> CacheFVector3List(int capcity = 4)
        {
            if (m_pFVec3ListCache == null)
                m_pFVec3ListCache = new ListCache<FVector3>(4);
            return m_pFVec3ListCache.Get(capcity);
        }
        //-----------------------------------------------------
        public List<Vector3> CacheVector3List(int capcity = 4)
        {
            if (m_pVec3ListCache == null)
                m_pVec3ListCache = new ListCache<Vector3>(4);
            return m_pVec3ListCache.Get(capcity);
        }
        //-----------------------------------------------------
        public List<IUserData> CacheDataList(int capcity = 4)
        {
            if (m_pUserDataListCache == null)
                m_pUserDataListCache = new ListCache<IUserData>(4);
            return m_pUserDataListCache.Get(capcity);
        }
        //-----------------------------------------------------
        public HashSet<IUserData> CacheDataHashSet(int capcity = 4)
        {
            if (m_pUserDataHashSetCache == null)
                m_pUserDataHashSetCache = new HashSetCache<IUserData>(4);
            return m_pUserDataHashSetCache.Get(capcity);
        }
        //-----------------------------------------------------
        internal void ClearCaches()
        {
            if (m_pVec3ListCache != null) m_pVec3ListCache.Clear();
            if (m_pFVec3ListCache != null) m_pFVec3ListCache.Clear();
            if (m_pUserDataListCache != null) m_pUserDataListCache.Clear();
            if (m_pIntListCache != null) m_pIntListCache.Clear();
            if (m_pFloatListCache != null) m_pFloatListCache.Clear();
            if (m_pLongListCache != null) m_pLongListCache.Clear();
            if (m_pDoubleListCache != null) m_pDoubleListCache.Clear();
            if (m_pUserDataHashSetCache != null) m_pUserDataHashSetCache.Clear();
            if (m_pIntHashSetCache != null) m_pIntHashSetCache.Clear();
            if (m_pFloatHashSetCache != null) m_pFloatHashSetCache.Clear();
            if (m_pLongHashSetCache != null) m_pLongHashSetCache.Clear();
            if (m_pDoubleHashSetCache != null) m_pDoubleHashSetCache.Clear();
        }
        //-----------------------------------------------------
    }
    //-----------------------------------------------------
    //ListCache
    //-----------------------------------------------------
    class ListCache<T>
    {
        List<T>[] m_vCaches = null;
        private int m_nCapacity = 4;
        private int m_nIndex = 0;
        //-----------------------------------------------------
        public ListCache(int capcity)
        {
            m_nCapacity = Mathf.Max(capcity, 4);
        }
        //-----------------------------------------------------
        public List<T> Get(int capcity = 4)
        {
            if(m_vCaches == null)
            {
                m_vCaches = new List<T>[m_nCapacity];
            }
            if (m_nIndex>= m_vCaches.Length)
                return new List<T>(capcity);
            var temp= m_vCaches[m_nIndex];
            if(temp == null)
            {
                temp = new List<T>(capcity);
                m_vCaches[m_nIndex] = temp;
            }
            temp.Clear();
            m_nIndex++;
            return temp;
        }
        //-----------------------------------------------------
        public void Clear()
        {
            m_nIndex = 0;
            if (m_vCaches == null)
                return;
            for(int i =0; i < m_vCaches.Length; ++i)
            {
                if (m_vCaches[i] != null) m_vCaches[i].Clear();
            }
        }
    }
    //-----------------------------------------------------
    //ListCache
    //-----------------------------------------------------
    class HashSetCache<T>
    {
        HashSet<T>[] m_vCaches = null;
        private int m_nCapacity = 4;
        private int m_nIndex = 0;
        //-----------------------------------------------------
        public HashSetCache(int capcity)
        {
            m_nCapacity = Mathf.Max(capcity, 4);
        }
        //-----------------------------------------------------
        public HashSet<T> Get(int capcity = 4)
        {
            if (m_vCaches == null)
            {
                m_vCaches = new HashSet<T>[m_nCapacity];
            }
            if (m_nIndex >= m_vCaches.Length)
                return new HashSet<T>(capcity);
            var temp = m_vCaches[m_nIndex];
            if (temp == null)
            {
                temp = new HashSet<T>(capcity);
                m_vCaches[m_nIndex] = temp;
            }
            m_nIndex++;
            return temp;
        }
        //-----------------------------------------------------
        public void Clear()
        {
            m_nIndex = 0;
            if (m_vCaches == null)
                return;
            for (int i = 0; i < m_vCaches.Length; ++i)
            {
                if (m_vCaches[i] != null) m_vCaches[i].Clear();
            }
        }
    }
}
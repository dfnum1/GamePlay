/********************************************************************
生成日期:	3:10:2019  15:03
类    名: 	FrameworkShareCache
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
        internal FrameworkShareCache(AFramework pFramework)
        {
            m_pFramework = pFramework;
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
            T newT = new T();
            newT.SetFramework(m_pFramework);
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
            pObj.Destroy();
            if (pool.Count < m_nTypePoolCapacity) pool.Push(pObj);
        }
        //-----------------------------------------------------
        public AgentTree MallocAgentTree()
        {
            if (m_vAgentTreePool == null || m_vAgentTreePool.Count<=0)
            {
                return new AgentTree();
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
    }
}
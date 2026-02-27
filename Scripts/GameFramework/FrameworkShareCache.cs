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
        internal FrameworkShareCache(AFramework pFramework)
        {
            m_pFramework = pFramework;
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
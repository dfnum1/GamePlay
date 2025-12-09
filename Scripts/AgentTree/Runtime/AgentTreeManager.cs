/********************************************************************
生成日期:	07:03:2025
类    名: 	AgentTree
作    者:	HappLI
描    述:	行为树
*********************************************************************/
using Framework.Core;
using System.Collections.Generic;
namespace Framework.AT.Runtime
{
    public interface IAgentTreeCallback
    {
        public bool OnNotifyExecutedNode(AgentTree pAgentTree, BaseNode pNode);
        public int GetAgentTreeRttiTypeId(System.Type type);
    }
    //-----------------------------------------------------
    //! AgentTreeManager
    //-----------------------------------------------------
    public class AgentTreeManager
    {
        LinkedList<IAgentTreeCallback> m_vCallback = null;
        //-----------------------------------------------------
        public AgentTreeManager()
        {
        }
        //-----------------------------------------------------
        public void RegisterCallback(IAgentTreeCallback pCallback)
        {
            if (m_vCallback == null) m_vCallback = new LinkedList<IAgentTreeCallback>();
            if (!m_vCallback.Contains(pCallback))
                m_vCallback.AddLast(pCallback);
        }
        //-----------------------------------------------------
        public void UnregisterCallback(IAgentTreeCallback pCallback)
        {
            if (m_vCallback == null) return;
            if (m_vCallback.Contains(pCallback))
                m_vCallback.Remove(pCallback);
        }
        //-----------------------------------------------------
        internal bool OnNotifyExecutedNode(AgentTree pAgentTree, BaseNode pNode)
        {
            if(m_vCallback!=null)
            {
                for (var callback = m_vCallback.First; callback != null; callback = callback.Next)
                {
                    if (callback.Value.OnNotifyExecutedNode(pAgentTree, pNode))
                        return true;
                }
            }
            return false;
        }
        //-----------------------------------------------------
        public int GetRttiId(IUserData pPointer)
        {
            if (pPointer == null) return 0;
            return GetRttiId(pPointer.GetType());
        }
        //-----------------------------------------------------
        public int GetRttiId(System.Type type)
        {
            if (m_vCallback != null)
            {
                for (var callback = m_vCallback.First; callback != null; callback = callback.Next)
                {
                    var typeId = callback.Value.GetAgentTreeRttiTypeId(type);
                    if (typeId != 0) return typeId;
                }
            }
            return 0;
        }
        //-----------------------------------------------------
        internal AgentTree MallocAgentTree()
        {
            return AgentTreePool.MallocAgentTree();
        }
        //-----------------------------------------------------
        internal void FreeAgentTree(AgentTree pDater)
        {
            AgentTreePool.FreeAgentTree(pDater);
        }
        //-----------------------------------------------------
        public void Destroy()
        {
            if (m_vCallback != null) m_vCallback.Clear();
        }
    }
}
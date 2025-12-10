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
        public bool OnATExecutedNode(AgentTree pAgentTree, BaseNode pNode);
    }
    //-----------------------------------------------------
    //! AgentTreeManager
    //-----------------------------------------------------
    public class AgentTreeManager : AModule
    {
        LinkedList<IAgentTreeCallback> m_vCallback = null;
        //-----------------------------------------------------
        public AgentTreeManager()
        {
        }
        //-----------------------------------------------------
        protected override void OnInit()
        {
#if UNITY_EDITOR
            Editor.AgentTreeUtil.EditorInit();
#endif
        }
        //-----------------------------------------------------
        public AgentTree CreateAgentTree(AgentTreeData atData)
        {
            if (atData == null) return null;
            AgentTree pAT = MallocAgentTree();
            if(!pAT.Create(atData))
            {
                FreeAgentTree(pAT);
                return null;
            }
            return pAT;
        }
        //-----------------------------------------------------
        public void DestroyAgentTree(AgentTree pAT)
        {
            if (pAT == null) return;
            pAT.Destroy();
            FreeAgentTree(pAT);
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
                foreach(var db in m_vCallback)
                {
                    if (db.OnATExecutedNode(pAgentTree, pNode))
                        return true;
                }
            }
            return ATCallHandler.DoAction(pAgentTree, pNode);
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
            return ATRtti.GetClassTypeId(type);
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
        internal void OnDestroyAgentTree(AgentTree pAt)
        {

        }
        //-----------------------------------------------------
        protected override void OnDestroy()
        {
            if (m_vCallback != null) m_vCallback.Clear();
        }
    }
}
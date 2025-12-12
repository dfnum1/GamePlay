/********************************************************************
生成日期:	07:03:2025
类    名: 	AgentTree
作    者:	HappLI
描    述:	行为树
*********************************************************************/
using Framework.Core;
using System.Collections.Generic;
using UnityEngine;
namespace Framework.AT.Runtime
{
    public interface IAgentTreeCallback
    {
        public bool OnATExecutedNode(AgentTree pAgentTree, BaseNode pNode);
    }
    //-----------------------------------------------------
    //! AgentTreeManager
    //-----------------------------------------------------
    public class AgentTreeManager : AModule, ITouchInput
    {
        LinkedList<IAgentTreeCallback>  m_vCallback = null;
        private HashSet<AgentTree>      m_vMouseInputEventTask = null;
        private Camera                  m_pMainCamera = null;
        //-----------------------------------------------------
        public AgentTreeManager()
        {
        }
        //-----------------------------------------------------
        protected override void OnInit()
        {
            m_pMainCamera = Camera.main;
#if UNITY_EDITOR
            Editor.AgentTreeUtil.EditorInit();
#endif
        }
        //-----------------------------------------------------
        public void SetMainCamera(Camera main)
        {
            m_pMainCamera = main;
        }
        //-----------------------------------------------------
        public Camera GetMainCamera()
        {
            if (m_pMainCamera == null) m_pMainCamera = Camera.main;
            return m_pMainCamera;
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
        internal void AddMouseInputTask(AgentTree pAT)
        {
            if (m_vMouseInputEventTask == null)
                m_vMouseInputEventTask = new HashSet<AgentTree>(16);
            m_vMouseInputEventTask.Add(pAT);
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
            AgentTree pAT = AgentTreePool.MallocAgentTree();
            if (GetFramework() != null) pAT.SetATManager(GetFramework().GetModule<AgentTreeManager>());
            return pAT;
        }
        //-----------------------------------------------------
        internal void FreeAgentTree(AgentTree pDater)
        {
            AgentTreePool.FreeAgentTree(pDater);
        }
        //-----------------------------------------------------
        internal void OnDestroyAgentTree(AgentTree pAt)
        {
            if (m_vMouseInputEventTask != null) m_vMouseInputEventTask.Remove(pAt);
        }
        //-----------------------------------------------------
        protected override void OnDestroy()
        {
            if (m_vCallback != null) m_vCallback.Clear();
            m_pMainCamera = null;
            if (m_vMouseInputEventTask != null) m_vMouseInputEventTask.Clear();
        }
        //-----------------------------------------------------
        public void OnTouchBegin(TouchInput.TouchData touch)
        {
            if(m_vMouseInputEventTask!=null)
            {
                foreach(var db in m_vMouseInputEventTask)
                {
                    db.MouseInputEvent(EATMouseType.Begin, touch);
                }
            }
        }
        //-----------------------------------------------------
        public void OnTouchMove(TouchInput.TouchData touch)
        {
            if (m_vMouseInputEventTask != null)
            {
                foreach (var db in m_vMouseInputEventTask)
                {
                    db.MouseInputEvent(EATMouseType.Move, touch);
                }
            }
        }
        //-----------------------------------------------------
        public void OnTouchEnd(TouchInput.TouchData touch)
        {
            if (m_vMouseInputEventTask != null)
            {
                foreach (var db in m_vMouseInputEventTask)
                {
                    db.MouseInputEvent(EATMouseType.End, touch);
                }
            }
        }
        //-----------------------------------------------------
        public void OnTouchWheel(float wheel, Vector2 mouse)
        {
            if (m_vMouseInputEventTask != null)
            {
                TouchInput.TouchData touch = TouchInput.TouchData.DEF;
                touch.position = mouse;
                touch.lastPosition = mouse;
                touch.deltaPosition = new Vector2(wheel, wheel); 
                foreach (var db in m_vMouseInputEventTask)
                {
                    db.MouseInputEvent(EATMouseType.Wheel, touch);
                }
            }
        }
    }
}
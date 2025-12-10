/********************************************************************
生成日期:	06:30:2025
类    名: 	PreviewLogic
作    者:	HappLI
描    述:	graphview 对接行为树的控制逻辑
*********************************************************************/
#if UNITY_EDITOR
using Framework.AT.Editor;
using Framework.AT.Runtime;
using Framework.ED;
using UnityEngine;

namespace Framework.AT.Editor
{
    [EditorBinder(typeof(AgentTreeWindow), "PreviewRect")]
    public class GraphViewLogic : AAgentTreeLogic
    {
        private Vector2 m_dragBoxStart;
        private Rect m_selectionBox;
        public enum EActivityType { Idle, HoldNode, DragNode, HoldGrid, DragGrid, DragLink }
        EActivityType m_currentActivity = EActivityType.Idle;
        public bool isPanning { get; private set; }
        public Vector2 panOffset { get { return m_panOffset; } set { m_panOffset = value; GetOwner().Repaint(); } }
        private Vector2 m_panOffset;
        public float zoom { get { return m_zoom; } set { m_zoom = Mathf.Clamp(value, EditorPreferences.GetSettings().minZoom, EditorPreferences.GetSettings().maxZoom); GetOwner().Repaint(); } }
        private float m_zoom = 1;
        AgentTreeGraphContoller m_pGraphView = new AgentTreeGraphContoller();
        //--------------------------------------------------------
        protected override void OnEnable()
        {
            m_pGraphView.OnEnable(this);
        }
        //--------------------------------------------------------
        protected override void OnDisable()
        {
            m_pGraphView.OnDisable();
        }
        //--------------------------------------------------------
        protected override void OnGUI()
        {
            var rect = GetRect();
            m_pGraphView.OnGUI(rect);
        }
        //------------------------------------------------------
        protected override void OnEvent(Event e)
        {
            if (!GetRect().Contains(e.mousePosition))
                return;
            GetOwner().wantsMouseMove = true;
            m_pGraphView.OnEvent(GetRect(), e);
        }
        //------------------------------------------------------
        protected override void OnUpdate(float delta)
        {
            m_pGraphView.OnUpdate(delta);
        }
        //--------------------------------------------------------
        public override void OnChangeSelect(object pOwner)
        {
            if (pOwner == null)
                return;
            OnRefreshData(pOwner);
        }
        //-----------------------------------------------------
        public override void OnRefreshData(System.Object pOwner)
        {
            m_pGraphView.OnRefreshData(GetOwner<AgentTreeWindow>().GetATData(), pOwner);
        }
        //--------------------------------------------------------
        public override void OnSaveChanges()
        {
            m_pGraphView.OnSaveChanges();
        }
        //--------------------------------------------------------
        public override void OnNotifyExecutedNode(AgentTree pAgentTree, BaseNode pNode)
        {
            m_pGraphView.OnNotifyExecutedNode(pAgentTree, pNode);
        }
    }
}

#endif
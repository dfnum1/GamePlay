/********************************************************************
生成日期:	06:30:2025
类    名: 	AgentTreeGraphContoller
作    者:	HappLI
描    述:	行为树视图控制器
*********************************************************************/
#if UNITY_EDITOR
using Framework.AT.Runtime;
using Framework.ED;
using UnityEngine;
using UnityEngine.UIElements;

namespace Framework.AT.Editor
{
    public class AgentTreeGraphContoller
    {
        VisualElement m_pRoot;
         AEditorLogic m_pOwnerEditorLogic;
        AgentTreeGraphView m_pGraphView;
        //--------------------------------------------------------
        public void OnEnable(AEditorLogic logic)
        {
            m_pOwnerEditorLogic = logic;

            m_pRoot = new VisualElement();
            logic.GetOwner().rootVisualElement.Add(m_pRoot);

            m_pGraphView = new AgentTreeGraphView(logic);
            m_pGraphView.name = "AgentTree";
            m_pGraphView.StretchToParentSize(); // 推荐
            m_pRoot.Add(m_pGraphView);
        }
        //--------------------------------------------------------
        public void OnDisable()
        {
            m_pOwnerEditorLogic.GetOwner().rootVisualElement.Remove(m_pRoot);
        }
        //--------------------------------------------------------
        public void OnGUI(Rect rect)
        {
            if (m_pGraphView == null)
                return;
            DrawGrid(rect, 1,m_pGraphView.viewTransform.position);
            m_pRoot.style.position = Position.Absolute;
            m_pRoot.style.left = rect.x;
            m_pRoot.style.top = rect.y;
            m_pRoot.style.width = rect.width;
            m_pRoot.style.height = rect.height;
            m_pGraphView.OnGUI(rect);
        }
        //------------------------------------------------------
        public void DrawGrid(Rect rect, float zoom, Vector2 panOffset)
        {
            Vector2 center = rect.size / 2f;
            Texture2D gridTex = EditorPreferences.GetSettings().gridTexture;
            Texture2D crossTex = EditorPreferences.GetSettings().crossTexture;

            // Offset from origin in tile units
            float xOffset = -(center.x * zoom + panOffset.x) / gridTex.width;
            float yOffset = ((center.y - rect.size.y) * zoom + panOffset.y) / gridTex.height;

            Vector2 tileOffset = new Vector2(xOffset, yOffset);

            // Amount of tiles
            float tileAmountX = Mathf.Round(rect.size.x * zoom) / gridTex.width;
            float tileAmountY = Mathf.Round(rect.size.y * zoom) / gridTex.height;

            Vector2 tileAmount = new Vector2(tileAmountX, tileAmountY);


            Color color = GUI.color;

            // Draw tiled background
            GUI.color = Color.white;
            GUI.DrawTextureWithTexCoords(rect, gridTex, new Rect(tileOffset, tileAmount));

            // Draw tiled background
            GUI.color = Color.white;
            GUI.DrawTextureWithTexCoords(rect, crossTex, new Rect(tileOffset + new Vector2(0.5f, 0.5f), tileAmount));
            GUI.color = color;
        }
        //------------------------------------------------------
        public void OnEvent(Rect rect, Event e)
        {
        }
        //-----------------------------------------------------
        public void OnUpdate(float fTime)
        {
            if(m_pGraphView!=null) m_pGraphView.OnUpdate(fTime);
        }
        //-----------------------------------------------------
        public void OnRefreshData(AgentTreeData pATData, System.Object pOwner)
        {
            m_pGraphView.SetAgentTree(pATData, pOwner);
        }
        //--------------------------------------------------------
        public void OnSaveChanges()
        {
            m_pGraphView.OnSaveChanges();
        }
        //-----------------------------------------------------
        //  public void OnEnableCutscene(CutsceneInstance pCutscene, bool bEnable)
        //  {
        //      m_pGraphView.OnEnableCutscene(pCutscene, bEnable);
        //  }
        //--------------------------------------------------------
        public void OnNotifyExecutedNode(AgentTree pAgentTree, BaseNode pNode)
        {
            m_pGraphView.OnNotifyExecutedNode(pAgentTree, pNode);
        }
    }
}

#endif
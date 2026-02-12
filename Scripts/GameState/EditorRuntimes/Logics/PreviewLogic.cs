/********************************************************************
生成日期:	11:07:2025
类    名: 	PreviewLogic
作    者:	HappLI
描    述:	预览窗口
*********************************************************************/
#if UNITY_EDITOR
using Framework.ED;
using Framework.State.Runtime;
using UnityEditor;
using UnityEngine;

namespace Framework.State.Editor
{
    [EditorBinder(typeof(GameWorldEditor), "PreviewRect")]
    public class PreviewLogic : AStateEditorLogic
    {
        TargetPreview m_Preview;
        GUIStyle m_PreviewStyle;
        bool m_bPreviewDataInit = false;
        IGameWorldItem m_pGameworldItem = null;
        //--------------------------------------------------------
        protected override void OnEnable()
        {
            if (m_Preview == null) m_Preview = new TargetPreview(GetOwner());
            GameObject[] roots = new GameObject[1];
            roots[0] = new GameObject("WarEditorRoot");
            m_Preview.AddPreview(roots[0]);

            m_Preview.SetCamera(0.01f, 10000f, 60f);
            m_Preview.Initialize(roots);
            m_Preview.SetPreviewInstance(roots[0] as GameObject);
            m_Preview.OnDrawAfterCB = this.OnDraw;
            m_Preview.bLeftMouseForbidMove = true;
            m_Preview.SetFloorTexture(Framework.ED.EditorUtils.GetFloorTexture());

            m_bPreviewDataInit = false;
        }
        //--------------------------------------------------------
        protected override void OnDisable()
        {
            var worldData = GetWorldData();
            if (worldData != null)
            {
                AGameCfgData cfgData = worldData.gameLevel.GetGameData<AGameCfgData>();
                if (cfgData != null)
                {
                    cfgData.GetEditor()?.OnPreviewDisable(m_Preview);
                }
            }
            if (m_Preview != null) m_Preview.Destroy();
            m_Preview = null;
            m_bPreviewDataInit = false;
        }
        //--------------------------------------------------------
        public override void OnGameItemSelected(IGameWorldItem pGameItem)
        {
            m_pGameworldItem = pGameItem;
        }
        //--------------------------------------------------------
        void OnDraw(int controllerId, Camera camera, Event evt)
        {
            var worldData = GetWorldData();
            if (worldData == null || worldData.gameLevel == null)
                return;

            AGameCfgData cfgData = worldData.gameLevel.GetGameData<AGameCfgData>();
            if (cfgData == null)
                return;

            if(!m_bPreviewDataInit)
            {
                cfgData.GetEditor()?.OnPreviewEnable(m_Preview);
                m_bPreviewDataInit = true;
            }
            cfgData.GetEditor()?.OnPreviewView(m_Preview);
        }
        //--------------------------------------------------------
        public override void OnSceneView(SceneView sceneView)
        {
            var worldData = GetWorldData();
            if (worldData == null || worldData.gameLevel == null)
                return;

            AGameCfgData cfgData = worldData.gameLevel.GetGameData<AGameCfgData>();
            if (cfgData == null)
                return;
            cfgData.GetEditor()?.OnSceneView(sceneView);
        }
        //--------------------------------------------------------
        protected override void OnGUI()
        {
            var window = GetOwner<GameWorldEditor>();
            if (m_Preview != null && window.PreviewRect.width > 0 && window.PreviewRect.height > 0)
            {
                if (m_PreviewStyle == null) m_PreviewStyle = new GUIStyle(EditorStyles.textField);
                m_Preview.OnPreviewGUI(window.PreviewRect, m_PreviewStyle);
            }
        }
    }
}

#endif
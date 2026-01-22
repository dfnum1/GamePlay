/********************************************************************
生成日期:	11:07:2025
类    名: 	InspectorDrawLogic
作    者:	HappLI
描    述:	数据面板逻辑
*********************************************************************/
#if UNITY_EDITOR
using Framework.ED;
using Framework.State.Runtime;
using UnityEditor;
using UnityEngine;

namespace Framework.State.Editor
{
    [EditorBinder(typeof(GameWorldEditor), "InspectorRect")]
    public class InspectorDrawLogic : AStateEditorLogic
    {
        Vector2 m_Scoller;
        IGameWorldItem m_pGameworldItem = null;
        //--------------------------------------------------------
        protected override void OnEnable()
        {
        }
        //--------------------------------------------------------
        protected override void OnDisable()
        {
            m_pGameworldItem = null;
        }
        //--------------------------------------------------------
        protected override void OnUpdate(float delta)
        {
            base.OnUpdate(delta);
        }
        //--------------------------------------------------------
        public override void OnSceneView(SceneView sceneView)
        {
        }
        //--------------------------------------------------------
        public override void OnGameItemSelected(IGameWorldItem pGameItem)
        {
            m_pGameworldItem = pGameItem;
        }
        //--------------------------------------------------------
        protected override void OnGUI()
        {
            var window = GetOwner<GameWorldEditor>();
            Rect rect = GetRect();
            GUILayout.BeginArea(new Rect(rect.x, rect.y + 20, rect.width, rect.height - 20));
            m_Scoller = GUILayout.BeginScrollView(m_Scoller);
            if (m_pGameworldItem != null)
            {
                if (m_pGameworldItem is GameStateData)
                {
                    OnDrawGameState(m_pGameworldItem as GameStateData);
                }
                else if (m_pGameworldItem is GameStateModeData)
                {
                    OnDrawGameMode(m_pGameworldItem as GameStateModeData);
                }
            }
            else
            {
                GUILayout.Label("未选择任何对象", StateEditorUtil.panelTitleStyle);
            }
            GUILayout.EndScrollView();

            GUILayout.EndArea();
            UIDrawUtils.DrawColorLine(new Vector2(rect.xMin, rect.y + 20), new Vector2(rect.xMax, rect.y + 20), new Color(1,1,1,0.5f));
            GUILayout.BeginArea(new Rect(rect.x, rect.y, rect.width, 20));
            GUILayout.Label("属性面板", StateEditorUtil.panelTitleStyle);
            GUILayout.EndArea();
        }
        //--------------------------------------------------------
        void OnDrawGameState(GameStateData stateData)
        {
            using (new GUILabelWidthScope(70))
            {
                stateData.name = EditorGUILayout.DelayedTextField("名称:", stateData.name);
                GameStateTypeProvider.Draw(new GUIContent("游戏状态:"), stateData.stateType, (clsId) => {
                    stateData.stateType = clsId;
                });

                GUILayout.Label("描述:");
                stateData.strDesc = EditorGUILayout.TextArea(stateData.strDesc, GUILayout.MinHeight(50));
            }
        }
        //--------------------------------------------------------
        void OnDrawGameMode(GameStateModeData stateData)
        {
            GameModeTypeProvider.Draw(new GUIContent("游戏状态模式:"), stateData.modeType, (clsId) => {
                stateData.modeType = clsId;
            });
        }
    }
}

#endif
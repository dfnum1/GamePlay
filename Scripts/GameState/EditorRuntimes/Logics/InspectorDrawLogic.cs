/********************************************************************
生成日期:	11:07:2025
类    名: 	InspectorDrawLogic
作    者:	HappLI
描    述:	数据面板逻辑
*********************************************************************/
#if UNITY_EDITOR
using Framework.ED;
using Framework.State.Runtime;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Framework.State.Editor
{
    [EditorBinder(typeof(GameWorldEditor), "InspectorRect")]
    public class InspectorDrawLogic : AStateEditorLogic
    {
        Vector2 m_Scoller;
        List<string> m_vModePops = new List<string>();
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
            Rect rect = GetRect();
            if (rect.width <= 20)
                return;
            var worldData = GetWorldData();
            if (worldData == null)
                return;
            var window = GetOwner<GameWorldEditor>();
            GUILayout.BeginArea(new Rect(rect.x, rect.y + 20, rect.width, rect.height - 20));
            m_Scoller = GUILayout.BeginScrollView(m_Scoller);
            if (m_pGameworldItem != null)
            {
                m_vModePops.Clear();
                if (worldData.modeDatas != null)
                {
                    foreach(var db in worldData.modeDatas)
                    {
                        m_vModePops.Add(db.name);
                    }
                }

                if (m_pGameworldItem is GameStateData)
                {
                    OnDrawGameState(m_pGameworldItem as GameStateData);
                }
                else if (m_pGameworldItem is GameStateModeData)
                {
                    OnDrawGameMode(m_pGameworldItem as GameStateModeData);
                }
                else if (m_pGameworldItem is GameLevelData)
                {
                    OnDrawGameLevel(m_pGameworldItem as GameLevelData);
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
                EditorGUI.BeginChangeCheck();
                stateData.name = EditorGUILayout.DelayedTextField("名称:", stateData.name);
                if (EditorGUI.EndChangeCheck())
                {
                    UndoRegister(false);
                }
                GameStateTypeProvider.Draw(new GUIContent("游戏状态:"), stateData.stateType, (clsId, index) => {
                    stateData.stateType = clsId;
                });

                //! 激活模式
                {
                    int modeIndex = -1;
                    var modes = GetWorldData().modeDatas;
                    if (modes != null)
                    {
                        for (int i = 0; i < modes.Count; ++i)
                        {
                            if (modes[i].modeType == stateData.activeMode)
                            {
                                modeIndex = i;
                            }
                        }
                    }
                    modeIndex = EditorGUILayout.Popup("激活模式:", modeIndex, m_vModePops.ToArray());
                    if (modeIndex >= 0 && modeIndex < m_vModePops.Count)
                    {
                        if (modes != null)
                        {
                            if (stateData.activeMode != modes[modeIndex].modeType)
                            {
                                UndoRegister(false);
                                stateData.activeMode = modes[modeIndex].modeType;
                            }
                        }
                    }
                    else
                    {
                        if(stateData.activeMode!=0)
                        {
                            UndoRegister(false);
                            stateData.activeMode = 0;
                        }
                    }

                    if (modeIndex < 0)
                    {
                        EditorGUILayout.HelpBox("当前游戏状态还没有设置激活的玩法模式哦！", MessageType.Warning);
                    }
                }

                //状态逻辑列表
                {
                    stateData.stateLogics = StateEditorUtil.DrawStateLogics(this,"逻辑:",stateData.stateLogics);
                }

                GUILayout.Label("描述:");
                EditorGUI.BeginChangeCheck();
                stateData.strDesc = EditorGUILayout.TextArea(stateData.strDesc, GUILayout.MinHeight(50));
                if (EditorGUI.EndChangeCheck())
                {
                    UndoRegister(false);
                }
            }
        }
        //--------------------------------------------------------
        void OnDrawGameMode(GameStateModeData stateData)
        {
            using (new GUILabelWidthScope(70))
            {
                EditorGUI.BeginChangeCheck();
                stateData.name = EditorGUILayout.DelayedTextField("名称:", stateData.name);
                if (EditorGUI.EndChangeCheck())
                {
                    UndoRegister(false);
                }
                GameModeTypeProvider.Draw(new GUIContent("游戏玩法模式:"), stateData.modeType, (clsId, index) =>
                {
                    UndoRegister(false);
                    stateData.modeType = clsId;
                });

                if(GUILayout.Button("设置为当前状态的激活模式"))
                {
                    var data = GetWorldData();
                    if(data!=null)
                    {
                        UndoRegister(false);
                        data.gameStateData.activeMode = stateData.modeType;
                    }
                }

                {
                    stateData.modeLogics = StateEditorUtil.DrawModeLogics(this, "逻辑:", stateData.modeLogics);
                }

                GUILayout.Label("描述:");
                EditorGUI.BeginChangeCheck();
                stateData.strDesc = EditorGUILayout.TextArea(stateData.strDesc, GUILayout.MinHeight(50));
                if (EditorGUI.EndChangeCheck())
                {
                    UndoRegister(false);
                }
            }
        }
        //--------------------------------------------------------
        void OnDrawGameLevel(GameLevelData stateData)
        {
            EditorGUI.BeginChangeCheck();
            stateData.name = EditorGUILayout.DelayedTextField("名称:", stateData.name);
            if (EditorGUI.EndChangeCheck())
            {
                UndoRegister(false);
            }
            GameLevelDataProvider.Draw(new GUIContent("游戏数据类型:"), stateData.dataType, (clsId, index) =>
            {
                UndoRegister(false);
                stateData.dataType = clsId;
            });

            GUILayout.Label("描述:");
            EditorGUI.BeginChangeCheck();
            stateData.strDesc = EditorGUILayout.TextArea(stateData.strDesc, GUILayout.MinHeight(50));
            if (EditorGUI.EndChangeCheck())
            {
                UndoRegister(false);
            }
            var cfgData = stateData.GetGameData<AGameCfgData>();
            if (cfgData != null)
            {
                Framework.ED.InspectorDrawUtil.BeginChangeCheck(GetLogic<UndoLogic>());
                if (cfgData.GetEditor() != null)
                {
                    cfgData.GetEditor().OnInspectorGUI();
                }
                else
                    Framework.ED.InspectorDrawUtil.DrawProperty(cfgData,null);
                Framework.ED.InspectorDrawUtil.EndChangeCheck();
            }
        }
    }
}

#endif
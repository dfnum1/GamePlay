#if UNITY_EDITOR
/********************************************************************
生成日期:	11:06:2023
类    名: 	TimelineDrawLogic
作    者:	HappLI
描    述:	
*********************************************************************/
using Framework.ActorSystem.Runtime;
using Framework.ED;
using UnityEditor;
using UnityEngine;

namespace Framework.ActorSystem.Editor
{
    [EditorBinder(typeof(ActionEditorWindow), "ToolBarRect")]
    public class ToolBarDrawLogic : ActionEditorLogic
    {
        //--------------------------------------------------------
        protected override void OnGUI()
        {
            try
            {
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("保存", new GUILayoutOption[] { GUILayout.Width(80) }))
                {
                    GetOwner().SaveChanges();
                }
                if (GUILayout.Button("技能编辑器", new GUILayoutOption[] { GUILayout.Width(80) }))
                {
                    ActionEditorWindow editor = GetOwner<ActionEditorWindow>();
                    editor.OpenSkillEditor();
                }
                if (GUILayout.Button("动画调试", new GUILayoutOption[] { GUILayout.Width(80) }))
                {
                    GraphPlayableUtil.DebugPlayable(GetActor());
                }
                if (GUILayout.Button("显示树空间", new GUILayoutOption[] { GUILayout.Width(80) }))
                {
                    ActionEditorWindow editor = GetOwner<ActionEditorWindow>();
                    editor.ShowSpatialActorDebug();
                }
                GUILayout.Button("文档说明", new GUILayoutOption[] { GUILayout.Width(80) });
                GUILayout.EndHorizontal();
            }
            catch
            {

            }

        }
    }
}

#endif
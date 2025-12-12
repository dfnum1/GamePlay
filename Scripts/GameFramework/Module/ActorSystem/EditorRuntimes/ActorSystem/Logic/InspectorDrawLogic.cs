#if UNITY_EDITOR
/********************************************************************
生成日期:	11:06:2023
类    名: 	InspectorDrawLogic
作    者:	HappLI
描    述:	
*********************************************************************/
using UnityEngine;
using Framework.ED;

namespace Framework.ActorSystem.Editor
{
    [EditorBinder(typeof(ActionEditorWindow), "InspectorRect")]
    public class InspectorDrawLogic : ActionEditorLogic
    {
        Vector2 m_Scoller;
        //--------------------------------------------------------
        protected override void OnEnable()
        {
        }
        //--------------------------------------------------------
        protected override void OnGUI()
        {
            var window = GetOwner<ActionEditorWindow>();
            GUILayout.BeginArea(window.InspectorRect);
            
            GUILayout.EndArea();
        }
    }
}

#endif
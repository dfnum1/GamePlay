/********************************************************************
生成日期:	06:30:2025
类    名: 	CustomEventData
作    者:	HappLI
描    述:	蓝图自定义事件
*********************************************************************/
#if UNITY_EDITOR
using Framework.AT.Editor;
using UnityEditor;
#endif
using System.Collections.Generic;
using UnityEngine;

namespace Framework.AT.Runtime
{
    //-----------------------------------------------------
    [System.Serializable]
    public class CustomEventData
    {
        [System.Serializable]
        public struct EventParam
        {
            public string name;
            public EVariableType type;
            public int clsId;
            public string defaultValue;
            public bool canEdit;
#if UNITY_EDITOR
            public string desc;
#endif
        }
        [System.Serializable]
        public class EventData
        {
            public int eventType;
            public string name;
            public List<EventParam> inputs;
            public List<EventParam> outputs;
#if UNITY_EDITOR
            public string desc;
#endif
        }
        public List<EventData> vEvents;
    }
    //-----------------------------------------------------
    //! ACustomEventObject 
    //-----------------------------------------------------
    public abstract class ACustomEventObject : ScriptableObject
    {
        public CustomEventData eventData;
    }
#if UNITY_EDITOR
    [CustomEditor(typeof(ACustomEventObject))]
    public class ACustomEventObjectEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            ACustomEventObject cutsceneObject = (ACustomEventObject)target;
            if (GUILayout.Button("编辑"))
            {
            }
        }
        [UnityEditor.Callbacks.OnOpenAsset(0)]
        public static bool OnOpenAsset(int instanceID, int line)
        {
            var obj = EditorUtility.InstanceIDToObject(instanceID);
            if (obj != null && obj is ACustomEventObject)
            {
                ACustomEventObject atObj = obj as ACustomEventObject;
                return true;
            }
            return false;
        }
    }
#endif
}
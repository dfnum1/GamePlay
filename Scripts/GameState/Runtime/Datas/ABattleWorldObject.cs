/********************************************************************
生成日期:	11:07:2025
类    名: 	ABattleWorldObject
作    者:	HappLI
描    述:	一个战争世界数据体
*********************************************************************/

using Framework.ActorSystem.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using Framework.State.Editor;
using Framework.ActorSystem.Editor;
using UnityEditor;
#endif
namespace Framework.State.Runtime
{
    [AddComponentMenu("")]
    public abstract class ABattleWorldObject : ScriptableObject
    {
        public BattleVariables warVariables = new BattleVariables();
        public List<BattleAgentData> warAgents = new List<BattleAgentData>();
    }

#if UNITY_EDITOR
    [UnityEditor.CustomEditor(typeof(ABattleWorldObject), true)]
    public class ABattleWorldObjectEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            ABattleWorldObject controller = target as ABattleWorldObject;
            Color color = GUI.color;
            OnInnerInspectorGUI();
            if (GUILayout.Button("编辑"))
            {
                BattleWorldEditor.OpenTarget(controller);
            }
            serializedObject.ApplyModifiedProperties();
        }
        //------------------------------------------------------
        protected virtual void OnInnerInspectorGUI() { }
        //-----------------------------------------------------
        [UnityEditor.Callbacks.OnOpenAsset(0)]
        internal static bool OnOpenAsset(int instanceID, int line)
        {
            var obj = EditorUtility.InstanceIDToObject(instanceID);
            if (obj != null && obj is ABattleWorldObject)
            {
                ABattleWorldObject pGo = obj as ABattleWorldObject;
                BattleWorldEditor.OpenTarget(pGo);
                return true;
            }
            return false;
        }
    }
#endif
}


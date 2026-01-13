/********************************************************************
生成日期:	11:07:2025
类    名: 	AGameWorldObject
作    者:	HappLI
描    述:	一个战争世界数据体
*********************************************************************/
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
    public abstract class AGameWorldObject : ScriptableObject
    {
        public GameStateData gameStateData = new GameStateData();
        public GameVariables warVariables = new GameVariables();
        public List<GameAgentData> warAgents = new List<GameAgentData>();
    }

#if UNITY_EDITOR
    [UnityEditor.CustomEditor(typeof(AGameWorldObject), true)]
    public class AGameWorldObjectEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            AGameWorldObject controller = target as AGameWorldObject;
            Color color = GUI.color;
            OnInnerInspectorGUI();
            if (GUILayout.Button("编辑"))
            {
                GameWorldEditor.OpenTarget(controller);
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
            if (obj != null && obj is AGameWorldObject)
            {
                AGameWorldObject pGo = obj as AGameWorldObject;
                GameWorldEditor.OpenTarget(pGo);
                return true;
            }
            return false;
        }
    }
#endif
}


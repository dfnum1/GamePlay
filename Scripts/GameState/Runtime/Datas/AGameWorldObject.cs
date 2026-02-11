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
using UnityEditor;
#endif
namespace Framework.State.Runtime
{
    //------------------------------------------------------
    //! 游戏世界对象接口
    //------------------------------------------------------
    public interface IGameWorldItem
    {

    }
    //------------------------------------------------------
    //! 游戏世界对象基类
    //------------------------------------------------------
    [AddComponentMenu("")]
    public abstract class AGameWorldObject : ScriptableObject
    {
        public GameWorldData gameWorldData;
    }
    //------------------------------------------------------
    //! 自定义编辑器
    //------------------------------------------------------
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

